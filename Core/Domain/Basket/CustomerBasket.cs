using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Basket
{
    public class CustomerBasket
    {
        public string Id { get; set; } // get from Client
        public ICollection<BasketItem> MyProperty { get; set; } = [];

    }
}
