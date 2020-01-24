using System.Collections.Generic;
using AwesomeRaven.Entities.ValueObjects;

namespace AwesomeRaven.Entities
{
    public class Region
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<Territory> Territories { get; set; } = new List<Territory>();
    }
}