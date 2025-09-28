using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specifications
{
    public class OrdersForUserSpecifications : BaseSpecification<Order>
    {
        public OrdersForUserSpecifications(string email)
            : base(O => O.BuyerEmail == email) 
        {
           
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOrderByDesc(O => O.OrderDate);
        }

        public OrdersForUserSpecifications(int orderId, string buyerEmail)
            : base(O => O.Id == orderId && O.BuyerEmail == buyerEmail) 
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
