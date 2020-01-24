using System.Collections.Generic;
using AwesomeRaven.Common;

namespace AwesomeRaven.Entities.ValueObjects
{
    public class Contact : ValueObject
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        
        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Name;
            yield return Title;
        }
    }
}