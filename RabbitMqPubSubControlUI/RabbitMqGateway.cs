using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqPubSubControlUI
{
    internal sealed class RabbitMqGateway
    {
        private HttpClient _httpClient;
        public async Task<int> GetQueueMessageCount(string username, string password, string hostName, int port, string vhost, string queue)
        {
            var httpClient = MakeHttpClient(username, password);
            var httpResponseMessage = await httpClient.GetAsync($"http://{username}:{password}@{hostName}:{port}/api/queues/{vhost}/{queue}");
            httpResponseMessage.EnsureSuccessStatusCode();
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RabbitMqData>(content).messages;
        }

        private HttpClient MakeHttpClient(string username, string password)
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                return _httpClient;
            }

            return _httpClient;
        }
    }


    public class RabbitMqData
    {
        public int messages { get; set; }
    }
}
