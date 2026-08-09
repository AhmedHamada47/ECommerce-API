using Domain.Contracts;
using Domain.Models.Order;
using System.Linq.Expressions;

namespace Services.Specification
{
    internal class OrderWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsSpecification()
            : base(order => true)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
        }

        public OrderWithItemsSpecification(string buyerEmail)
            : base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
        }

        public OrderWithItemsSpecification(int id, string buyerEmail)
            : base(order => order.Id == id && order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
