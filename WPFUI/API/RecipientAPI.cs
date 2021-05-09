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
    class RecipientAPI
    {
        private const string URL = "http://localhost:8080/api/";
        HttpClient client = new HttpClient();
        JsonSerializer jsonSerializer = new JsonSerializer();

        public RecipientAPI()
        {
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<List<Recipient>> GetRecipientsAsync()
        {
            List<Recipient> Recipient = null;
            HttpResponseMessage response = client.GetAsync("recipients").Result;

            response.EnsureSuccessStatusCode();

            if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                Recipient = jsonSerializer.Deserialize<List<Recipient>>(jsonReader);
            }

            return Recipient;
        }
    }
}
