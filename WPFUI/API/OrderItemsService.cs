using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using WPFUI.Models;

namespace WPFUI.API
{
    class OrderItemsService
    {
        private const string URLOrder = "https://localhost:5001/api/";
        HttpClient client = new HttpClient();
        JsonSerializer jsonSerializer = new JsonSerializer();

        public OrderItemsService()
        {
            client.BaseAddress = new Uri(URLOrder);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<List<OrderItem>> GetOrderItemsAsync()
        {
            List<OrderItem> orderItems = null;
            HttpResponseMessage response = await client .GetAsync("orderItems");

            response.EnsureSuccessStatusCode();

            if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = response.Content.ReadAsStreamAsync().Result;

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                orderItems = jsonSerializer.Deserialize<List<OrderItem>>(jsonReader);
            }

            return orderItems;
        }

        [HttpGet]
        public async Task<List<OrderItem>> GetOrderItemsByOrderAsync(Guid orderId)
        {
            List<OrderItem> orderItems = null;
            HttpResponseMessage response = await client.GetAsync($"orderItems/order/{orderId}");

            response.EnsureSuccessStatusCode();

            if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = response.Content.ReadAsStreamAsync().Result;

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                orderItems = jsonSerializer.Deserialize<List<OrderItem>>(jsonReader);
            }

            return orderItems;
        }

        [HttpPost]
        public async Task<OrderItem> PostOrderItem(OrderItem orderItem)
        {
            var orderItemJson = JsonConvert.SerializeObject(orderItem);
            var buffer = System.Text.Encoding.UTF8.GetBytes(orderItemJson);

            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync("orderItems", byteContent);

            response.EnsureSuccessStatusCode();

            var returnedOrderItem = new OrderItem();
            if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = response.Content.ReadAsStreamAsync().Result;

                var streamReader = new StreamReader(contentStream);
                var jsonReader = new JsonTextReader(streamReader);

                returnedOrderItem = jsonSerializer.Deserialize<OrderItem>(jsonReader);
            }

            return returnedOrderItem;
        }

        [HttpDelete]
        public async Task<bool> DeleteOrderItem(Guid orderItemId)
        {
            HttpResponseMessage response = await client.DeleteAsync($"orderItems/{orderItemId}");

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
