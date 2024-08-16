using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp_Chat_Streaming.Models;

namespace ConsoleApp_Chat_Streaming.Helper
{
    public class ChatEndpointHelper
    {
        private readonly HttpClient _httpClient;

        public ChatEndpointHelper(HttpClient httpClient, string baseAddress)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri(baseAddress ?? throw new ArgumentNullException(nameof(baseAddress)));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async IAsyncEnumerable<string> ChatStreamAsync2(string message)
        {
            var payload = new UserMessageRequest { Prompt = message };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_httpClient.BaseAddress!, "/Chat/ChatRDC"),
                Content = content
            };

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(responseStream);
                char[] buffer = new char[8192];
                int charsRead;
                while ((charsRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    yield return new string(buffer, 0, charsRead);
                }
            }
            else
            {
                Console.WriteLine($"Server response code: {response.StatusCode}");
                yield return "";
            }
        }

        public async IAsyncEnumerable<string> ChatStreamAsync(string message)
        {
            var payload = new UserMessageRequest { Prompt = message };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_httpClient.BaseAddress!, "/stream_chat"),
                Content = content
            };


            // Add a custom header - we will hardcode the vechileId just for demo purposes
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("vechileId", "12345");

            // Using PostAsJsonAsync to send the POST request with the payload serialized as JSON
            var response = await _httpClient.PostAsJsonAsync("/Chat/ChatRDC", payload);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(responseStream);

                while (!reader.EndOfStream)
                {
                    var chunk = await reader.ReadLineAsync();
                    if (chunk != null)
                    {
                        Console.WriteLine(chunk);
                        yield return chunk;
                    }
                }
            }
            else
            {
                Console.WriteLine($"Server response code: {response.StatusCode}");
                yield return "";
            }
        }
    }

}