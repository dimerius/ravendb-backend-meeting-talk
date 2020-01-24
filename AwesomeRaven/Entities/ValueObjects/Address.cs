using System.Collections.Generic;
using AwesomeRaven.Common;

namespace AwesomeRaven.Entities.ValueObjects
{
    public class Address : ValueObject
    {
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public Location Location { get; set; } = new Location();

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Line1;
            yield return Line2;
            yield return City;
            yield return Region;
            yield return PostalCode;
            yield return Country;
            yield return Location;
        }
    }
}