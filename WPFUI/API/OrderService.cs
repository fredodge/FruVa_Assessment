using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WPFUI.Models;

namespace WPFUI.API
{
    class OrderService
    {
        private const string URLOrder = "https://localhost:5001/api/";
        HttpClient client = new HttpClient();
        JsonSerializer jsonSerializer = new JsonSerializer();

        public OrderService()
        {
            client.BaseAddress = new Uri(URLOrder);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<List<Order>> GetOrdersAsync()
        {
            List<Order> orders = null;
            HttpResponseMessage response = await client.GetAsync("Orders");

            response.EnsureSuccessStatusCode();

            if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = response.Content.ReadAsStreamAsync().Result;

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                orders = jsonSerializer.Deserialize<List<Order>>(jsonReader);
            }

            return orders;
        }

        [HttpGet]
        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            Order order = null;
            HttpResponseMessage response = await client.GetAsync($"Orders/{orderId}");

            response.EnsureSuccessStatusCode();

            if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = response.Content.ReadAsStreamAsync().Result;

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                order = jsonSerializer.Deserialize<Order>(jsonReader);
            }

            return order;
        }

        [HttpPost]
        public async Task<Order> PostOrder(Order order)
        {
            var orderJson = JsonConvert.SerializeObject(order);
            var buffer = System.Text.Encoding.UTF8.GetBytes(orderJson);

            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync("Orders", byteContent);

            response.EnsureSuccessStatusCode();

            var returnedOrder = new Order();
            if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = response.Content.ReadAsStreamAsync().Result;

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                returnedOrder = jsonSerializer.Deserialize<Order>(jsonReader);
            }

            return returnedOrder;
        }

        [HttpPut]
        public async Task<Order> PutOrder(Order order)
        {
            var orderJson = JsonConvert.SerializeObject(order);
            var buffer = System.Text.Encoding.UTF8.GetBytes(orderJson);

            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PutAsync("Orders", byteContent);

            response.EnsureSuccessStatusCode();

            var returnedOrder = new Order();
            if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = response.Content.ReadAsStreamAsync().Result;

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                returnedOrder = jsonSerializer.Deserialize<Order>(jsonReader);
            }

            return returnedOrder;
        }

        [HttpDelete]
        public async Task<bool> DeleteOrder(Guid orderId)
        {
            HttpResponseMessage response = await client.DeleteAsync($"Orders/{orderId}");

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
