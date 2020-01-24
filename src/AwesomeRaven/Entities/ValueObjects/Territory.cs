using System;
using System.Collections.Generic;
using AwesomeRaven.Common;

namespace AwesomeRaven.Entities.ValueObjects
{
    public class Territory : ValueObject
    {
        public string? Code { get; set; } 
        public string? Name { get; set; } 
        public string? Area { get; set; }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Code;
            yield return Name;
            yield return Area;
        }
    }
}