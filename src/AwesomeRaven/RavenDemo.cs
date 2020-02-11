using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeRaven.Entities;
using AwesomeRaven.Raven;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries;

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
                _ => new object()
            };


        private async Task<object> SearchForEmployeeByFullNameAsync()
        {
            _logger.LogInformation("Please input fragments of first and last name separated by space.");
            var employeeName = Console.ReadLine();
            
            var searchFragments =
                employeeName?.Split(new char[] {' ', ':', ';'}, StringSplitOptions.RemoveEmptyEntries);

            if (searchFragments is null || searchFragments.Length == 0)
            {
                return new List<object>();
            }
            
            var firstName = searchFragments.First();
            var lastName = searchFragments.Skip(1).LastOrDefault();

            var firstNameFragment = !(firstName is null) ? $"{firstName}*" : null;
            var lastNameFragment = !(lastName is null) ? $"{lastName}*" : null;

            using var session = _raven.Store.OpenAsyncSession();
            
            var employeeQuery =
                session.Query<Employee>()
                .Search(e =>  e.FirstName, firstNameFragment);
            
            if (!(lastNameFragment is null))
            {
                employeeQuery = employeeQuery
                    .Search(e => e.LastName, lastNameFragment, boost: 5, options: SearchOptions.And);
            }
            
            var employee = await employeeQuery
                .Take(20)
                .Select(e => new { e.FirstName, e.LastName})
                .ToListAsync();
            
            return employee;
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
    }

    public enum PerformOperation
    {
        SearchForEmployeeByFullName,
        FetchOrdersInRange
    }
}