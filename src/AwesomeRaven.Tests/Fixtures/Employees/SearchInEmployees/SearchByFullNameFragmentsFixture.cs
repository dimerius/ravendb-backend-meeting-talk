using System;
using System.Linq;
using AwesomeRaven.Entities;
using AwesomeRaven.Raven;
using AwesomeRaven.Tests.Fixtures.Data;
using Raven.Client.Documents;

namespace AwesomeRaven.Tests.Fixtures.Employees.SearchInEmployees
{
    public class SearchByFullNameFragmentsFixture : IHaveDataSetup, IRavenClient, IDisposable
    {
        private readonly IRavenClient _raven;

        public IDocumentStore Store => _raven.Store;

        public SearchByFullNameFragmentsFixture()
        {
            _raven = new RavenDbTestClient();
            //seed database
            this.PrepareData();
        }

        public void PrepareData()
        {
            using var session = _raven.Store.OpenSession();

            var employees = new EmployeeCollection().Employees();

            foreach (var employee in employees)
            {
                session.Store(employee);
            }

            session.SaveChanges();
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
