using System.Linq;
using AwesomeRaven.Entities;
using Raven.Client.Documents.Indexes;

namespace AwesomeRaven.Raven.Indexes
{
    public class Orders_ByCompany : AbstractIndexCreationTask<Order, Orders_ByCompany.Orders_ByCompany_Result>
    {
        public class Orders_ByCompany_Result
        {
            public string Company { get; set; }
            public long Count { get; set; }
            public decimal Total { get; set; }
        }

        public Orders_ByCompany()
        {
            Map = orders => orders.Select(order => new
            {
                order.Company,
                Count = 1,
                Total = order.Lines.Sum(line => line.Quantity * line.PricePerUnit * (1 - line.Discount))
            });

            Reduce = results => results.GroupBy(result => result.Company).Select(result => new
            {
                Company = result.Key,
                Count = result.Sum(o => o.Count),
                Total = result.Sum(o => o.Total)
            });
        }
    }
}
