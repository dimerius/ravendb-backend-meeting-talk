using System.Linq;
using AwesomeRaven.Entities;
using Raven.Client.Documents.Indexes;

namespace AwesomeRaven.Raven.Indexes
{
    public class Employee_Search_ByName : AbstractIndexCreationTask<Employee>
    {
        public class Result
        {
            public string? FirstName { get; set; } 
            public string? LastName { get; set; }
            public string? FullName { get; set; }
        }
        
        public Employee_Search_ByName()
        {
            Map = employees => employees.Select(employee => new
            {
                employee.FirstName,
                employee.LastName,
                FullName = employee.FirstName + " " + employee.LastName 
            });
        }
    }
}
