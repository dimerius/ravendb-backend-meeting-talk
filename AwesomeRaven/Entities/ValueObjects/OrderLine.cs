using System.Collections.Generic;
using AwesomeRaven.Common;

namespace AwesomeRaven.Entities.ValueObjects
{
    public class OrderLine : ValueObject
    {
        public string? Product { get; set; }
        public string? ProductName { get; set; }
        public decimal PricePerUnit { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        
        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Product;
            yield return ProductName;
            yield return PricePerUnit;
            yield return Quantity;
            yield return Discount;
        }
    }
}