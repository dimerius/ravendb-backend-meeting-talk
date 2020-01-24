using System.Collections.Generic;
using AwesomeRaven.Common;

namespace AwesomeRaven.Entities.ValueObjects
{
    public class Location : ValueObject
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}