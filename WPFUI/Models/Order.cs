using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public Guid Id { get; set; }
        public string OrderName { get; set; }
        public byte[] DeliveryDay { get; set; }
        public System.Guid RecipientId { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
