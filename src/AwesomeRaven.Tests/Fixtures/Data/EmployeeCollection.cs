using System;
using System.Collections.Generic;
using AwesomeRaven.Entities;
using AwesomeRaven.Entities.ValueObjects;

namespace AwesomeRaven.Tests.Fixtures.Data
{
    public class EmployeeCollection
    {
        public IEnumerable<Employee> Employees()
        {
            yield return new Employee
            {
                LastName = "Callahan",
                FirstName = "Laura",
                Title = "Inside Sales Coordinator",
                Address = new Address
                {
                    Line1 = "4726 - 11th Ave. N.E.",
                    Line2 = null,
                    City = "Seattle",
                    Region = "WA",
                    PostalCode = "98105",
                    Country = "USA",
                    Location = new Location
                    {
                        Latitude = 47.66416419999999,
                        Longitude = -122.3160148
                    }
                },
                HiredAt = new DateTime(1994, 3, 5),
                Birthday = new DateTime(1958, 1, 9),
                HomePhone = "(206) 555-1189",
                Extension = "2344",
                ReportsTo = null,
                Notes = new List<string>
                {
                    "Laura received a BA in psychology from the University of Washington.  She has also completed a course in business French.  She reads and writes French."
                },
                Territories = new List<string>
                {
                    "19428",
                    "44122",
                    "45839",
                    "53404"
                }
            };

            yield return new Employee
            {
                LastName = "King",
                FirstName = "Robert",
                Title = "Sales Representative",
                Address = new Address
                {
                    Line1 = "Edgeham Hollow\r\nWinchester Way",
                    Line2 = null,
                    City = "London",
                    Region = null,
                    PostalCode = "RG1 9SP",
                    Country = "USA",
                    Location = null
                },
                HiredAt = new DateTime(1994, 1, 2),
                Birthday = new DateTime(1960, 5, 29),
                HomePhone = "(71) 555-5598",
                Extension = "465",
                ReportsTo = "employees/5-A",
                Notes = new List<string>
                {
                    "Robert King served in the Peace Corps and traveled extensively before completing his degree in English at the University of Michigan in 1992, the year he joined the company.  After completing a course entitled \"Selling in Europe,\" he was transferred to the London office in March 1993."
                },
                Territories = new List<string>
                {
                    "60179",
                    "60601",
                    "80202",
                    "80909",
                    "90405",
                    "94025",
                    "94105",
                    "95008",
                    "95054",
                    "95060"
                }
            };
        }
    }
}
