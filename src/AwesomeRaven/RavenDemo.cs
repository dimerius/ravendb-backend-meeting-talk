using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AwesomeRaven.DTOs.Search;
using AwesomeRaven.Entities;
using AwesomeRaven.Raven;
using AwesomeRaven.Raven.Indexes;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Subscriptions;

namespace AwesomeRaven
{
    public class RavenDemo
    {
        private readonly IRavenClient _raven;
        private readonly ILogger<RavenDemo> _logger;

        public RavenDemo(IRavenClient raven, ILogger<RavenDemo> logger) =>
            (_raven, _logger) = (raven, logger);

        public async Task<object> Execute(PerformOperation operation) =>
            operation switch
            {
                PerformOperation.SearchForEmployeeByFullName => await this.SearchForEmployeeByFullNameAsync(),
                PerformOperation.FetchOrdersInRange => await this.LoadOrdersInRangeAsync(),
                PerformOperation.SuggestEmployeeNames => await this.SuggestEmployeeNamesAsync(),
                PerformOperation.SubscribeToProductCollection => SubscribeToProductCollection(),
                _ => new object()
            };


        private async Task<object> SearchForEmployeeByFullNameAsync()
        {
            _logger.LogInformation("Please input fragments of first and last name separated by space.");
            var employeeName = Console.ReadLine();

            var searchFragments =
                employeeName?.Split(new[] {' ', ':', ';'}, StringSplitOptions.RemoveEmptyEntries);

            if (searchFragments is null || searchFragments.Length == 0)
            {
                return new List<object>();
            }

            var firstName = searchFragments.First();
            var lastName = searchFragments.Skip(1).LastOrDefault();

            var firstNameFragment = $"{firstName}*";
            var lastNameFragment = !(lastName is null) ? $"{lastName}*" : null;

            using var session = _raven.Store.OpenAsyncSession();
            _logger.LogTrace("Opened a RavenDb connection.");

            var employeeQuery =
                session.Query<Employee>()
                    .Search(e => e.FirstName, firstNameFragment);

            if (!(lastNameFragment is null))
            {
                employeeQuery = employeeQuery
                    .Search(e => e.LastName, lastNameFragment, boost: 5, options: SearchOptions.And);
            }

            var employee = await employeeQuery
                .Take(20)
                .Select(e => new {e.FirstName, e.LastName})
                .ToListAsync();

            _logger.LogTrace("Executed {amount} call/s to database", session.Advanced.NumberOfRequests);

            return employee;
        }

        private async Task<object> SuggestEmployeeNamesAsync()
        {
            _logger.LogInformation("Please try to input full name.");
            var messedUpName = Console.ReadLine();

            using var session = _raven.Store.OpenAsyncSession();
            _logger.LogTrace("Opened a RavenDb connection.");

            var suggestedEmployees = await session.Advanced
                .AsyncDocumentQuery<Employee_Search_ByName.Result, Employee_Search_ByName>()
                .WhereEquals(e => e.FullName, messedUpName)
                .Fuzzy(0.5m)
                .Take(20)
                .SelectFields<SuggestedEmployee>()
                .ToListAsync();

            var suggestedNames = suggestedEmployees.Select(e => $"{e.FirstName} {e.LastName}");

            _logger.LogTrace("Executed {amount} call/s to database", session.Advanced.NumberOfRequests);
            _logger.LogInformation("Did you mean any of these employees?");

            return suggestedNames;
        }

        private async Task<IEnumerable<Order>> LoadOrdersInRangeAsync()
        {
            var from = new DateTime(1998, 1, 1);
            var to = new DateTime(1999, 3, 1);

            using var session = _raven.Store.OpenAsyncSession();
            return await session.Query<Order>()
                .Where(o => o.OrderedAt >= from && o.OrderedAt <= to)
                .ToListAsync();
        }

        private object SubscribeToProductCollection()
        {
            var subscriptionName= _raven.Store.Subscriptions.Create<Product>();

            var options = new SubscriptionWorkerOptions(subscriptionName);
            options.MaxErroneousPeriod = TimeSpan.FromMinutes(2);
            options.TimeToWaitBeforeConnectionRetry = TimeSpan.FromSeconds(5);

            var subscriptionWorker = _raven.Store.Subscriptions.GetSubscriptionWorker<Product>(options);
                
            subscriptionWorker.OnSubscriptionConnectionRetry += exception =>
            {
                _logger.LogWarning("Error during subscription processing: {subscriptionName} {exception}",
                    subscriptionName, exception);
            };
            
            var cancellationTokenSource = new CancellationTokenSource();
            var ct = cancellationTokenSource.Token;

            Task.Run(async () =>
            {
                try
                {
                    while (!ct.IsCancellationRequested)
                    {
                        await subscriptionWorker.Run(batch =>
                        {
                            foreach (var item in batch.Items)
                            {
                                _logger.LogInformation($"Product have changed {item.Id}");
                            }
                        }, ct);
                    }
                }
                finally
                {
                    subscriptionWorker.Dispose();
                    _raven.Store.Subscriptions.Delete(subscriptionName);
                }
            }, ct);

            Console.ReadLine();
            cancellationTokenSource.Cancel();

            return new object();
        }
    }

    public enum PerformOperation
    {
        SubscribeToProductCollection,
        SearchForEmployeeByFullName,
        SuggestEmployeeNames,
        FetchOrdersInRange
    }
}
