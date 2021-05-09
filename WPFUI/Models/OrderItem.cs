using System;

namespace WPFUI
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ArticleId { get; set; }
        public int Amount { get; set; }
    }
}
