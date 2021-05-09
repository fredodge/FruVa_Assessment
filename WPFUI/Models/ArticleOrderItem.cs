using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.Models
{
    class ArticleOrderItem
    {
        public OrderItem OrderItem { get; set; }
        public Article Article { get; set; }
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ArticleId { get; set; }
        public int Amount { get; set; }
        public int MainArticleNumber { get; set; }
        public int DetailArticleNumber { get; set; }
        public string ArticleName { get; set; }
        public string PackageSize { get; set; }
        public string ArticleGroupNumber { get; set; }
        public string ArticleGroupName { get; set; }
        public string OriginCountry { get; set; }
        public string TradeClass { get; set; }
        public decimal Colli { get; set; }
        public string Caliber { get; set; }
        public string Variety { get; set; }
        public string OwnBrand { get; set; }
        public string SearchQuery { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
