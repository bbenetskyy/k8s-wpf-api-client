using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace k8s_wpf_api_client.Commands
{
    public static class HttpCommands
    {
        public static async Task<string[]> CheckAsync(string ip,
            string port, int checkCount = 1)
        {
            using (var client = GetHttpClient(ip, port))
            {
                return await MakeRequestAsync(ip, port, checkCount,
                    client.GetAsync("/"));
            }
        }

        public static async Task<string[]> KillAsync(string ip,
            string port, int checkCount = 1)
        {
            using (var client = GetHttpClient(ip, port))
            {
                return await MakeRequestAsync(ip, port, checkCount,
                    client.PostAsync("/shutdown", null));
            }
        }

        private static async Task<string[]> MakeRequestAsync(string ip,
            string port,
            int checkCount,
            Task<HttpResponseMessage> requestTask)
        {
            var resList = new List<string>();
            for (var i = 0; i < checkCount; i++)
            {
                var response = await requestTask;
                if (response.IsSuccessStatusCode)
                {
                    resList.Add(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    resList.Add($"Error Code {response.StatusCode} : Message - {response.ReasonPhrase}");
                }
            }

            return resList.ToArray();
        }

        private static HttpClient GetHttpClient(string ip,
            string port)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri($"http://{ip}:{port}/")
            };
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
