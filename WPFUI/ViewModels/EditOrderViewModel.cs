using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel;

namespace WPFUI.ViewModels
{
    class EditOrderViewModel : IDisposable
    {
        private Logger Log;
        public Dictionary<string, List<Articles>> articles_context;
        public List<Recipients> recipients;

        public EditOrderViewModel()
        {
            Log = new Logger();
            articles_context = new Dictionary<string, List<Articles>>();
            recipients = new List<Recipients>();
        }

        public void Load()
        {
            using (var context = new FruVa_Assessment_APIEntities())
            {
                context.Database.Connection.Open();

                foreach ( var articles in context.Articles.GroupBy(a => a.ArticleGroupName) )
                {
                    var temp = new List<Articles>();
                    foreach ( var article in articles.AsEnumerable()) temp.Add(article);

                    articles_context.Add(articles.First().ArticleGroupName, temp);
                }

                foreach ( var recipient in context.Recipients)
                {
                    recipients.Add(recipient);
                }

                context.Database.Connection.Close();
            }
            Log.Log("Articles & Recipients loaded.");
        }

        public void Dispose()
        {
            using (var context = new FruVa_Assessment_APIEntities()) { context.Dispose(); }
        }
    }
}
