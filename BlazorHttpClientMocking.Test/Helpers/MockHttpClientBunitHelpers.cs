using Bunit;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace BlazorHttpClientMocking.Test.Helpers
{
    public static class MockHttpClientBunitHelpers
    {
      
        public static MockHttpMessageHandler AddMockHttpClient(this TestServiceProvider services, string baseAddress = @"http://localhost")
        {
            var mockHttpHandler = new MockHttpMessageHandler();
            var httpClient = mockHttpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost");
            services.AddSingleton<HttpClient>(httpClient);
            return mockHttpHandler;
        }

        public static T? FromResponse<T>(this HttpResponseMessage? response, JsonSerializerOptions? options = null)
        {
            if (response == null)
            {
                return default(T);
            }
            if (options == null)
            {
                options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true                  
                };
            }
            string responseString = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<T>(responseString, options);
            return result;
        }

        public static async Task<T?> FromResponseAsync<T>(this HttpResponseMessage? response)
        {
            if (response == null)
            {
                return await Task.FromResult(default(T));
            }
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return result;
        }

        public static MockedRequest RespondJson<T>(this MockedRequest request, T content)
        {
            request.Respond(req =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonSerializer.Serialize(content));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            });
            return request;
        }

        public static MockedRequest RespondJson<T>(this MockedRequest request, Func<T> contentProvider)
        {
            request.Respond(req =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(JsonSerializer.Serialize(contentProvider()));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            });
            return request;
        }


    }
}
