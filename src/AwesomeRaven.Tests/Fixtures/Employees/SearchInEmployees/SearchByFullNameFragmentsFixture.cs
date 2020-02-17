using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeRaven.Entities;
using AwesomeRaven.Entities.ValueObjects;
using AwesomeRaven.Raven;
using AwesomeRaven.Raven.Indexes;
using AwesomeRaven.Tests.Fixtures.Data;
using Raven.Client.Documents;
using Xunit;

namespace AwesomeRaven.Tests.Fixtures.Employees.SearchInEmployees
{
    public class SearchByFullNameFragmentsFixture : IHaveDataSetup, IRavenClient, IDisposable
    {
        private readonly IRavenClient _raven;

        public IDocumentStore Store => _raven.Store;

        public SearchByFullNameFragmentsFixture()
        {
            _raven = new RavenDbTestClient();
        }

        public async Task PrepareData()
        {
            using var session = _raven.Store.OpenAsyncSession();

            var employees = new EmployeeCollection().Employees();

            foreach (var employee in employees)
            {
                await session.StoreAsync(employee);
            }

            await session.SaveChangesAsync();
            session.Advanced.WaitForIndexesAfterSaveChanges();
        }

        public void Dispose()
        {
            using var session = _raven.Store.OpenSession();
            var employees = session.Query<Employee>().ToList();

            foreach (var employee in employees)
            {
                session.Delete(employee);
            }

            session.SaveChanges();
        }
    }
}
