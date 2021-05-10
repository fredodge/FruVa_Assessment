using System;

namespace WPFUI.Models
{
    class OrderViewItem
    {
        public Guid Id { get; set; }
        public string OrderName { get; set; }
        public DateTime DeliveryDay { get; set; }
        public string RecipientName { get; set; }
        public int ArticleAmount { get; set; }
        public string ArticleNames { get; set; }
    }
}
