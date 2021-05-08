using System.Net.Http;
using WPFUI.Models;
using Newtonsoft.Json;
using System.Web.Http;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace WPFUI.API
{
    class ArticlesAPI
    {
        private const string URL = "http://localhost:8080/api/";
        static HttpClient client = new HttpClient();
        static JsonSerializer jsonSerializer = new JsonSerializer();

        public ArticlesAPI()
        {
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<List<Article>> GetArticlesAsync()
        {
            List<Article> articles = null;
            HttpResponseMessage response = client.GetAsync("articles").Result;

            response.EnsureSuccessStatusCode();

            if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                articles = jsonSerializer.Deserialize<List<Article>>(jsonReader);
            }

            return articles;
        }
    }
}
