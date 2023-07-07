using BlazorHttpClientMocking.Test.Helpers;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using RichardSzalay.MockHttp;
using static BlazorHttpClientMocking.Pages.FetchData;
using System.Text.Json.Serialization;
using FluentAssertions;

namespace BlazorHttpClientMocking.Test
{
    public class FetchDataTests
    {
        [Fact]
        public async Task FetchData_HttpClient_Request_SuccessResponse()
        {
            //Arrange 
            using var ctx = new TestContext();
            var httpMock = ctx.Services.AddMockHttpClient();
            string knownUrl = @"sample-data/weather.json";
            var sampleData = await SerializationHelpers.DeserializeJsonAsync<WeatherForecast[]>(knownUrl);
            httpMock.When("/sample-data/weather.json").RespondJson(sampleData);

            //Act
            var httpClient = ctx.Services.BuildServiceProvider().GetService<HttpClient>();
            var httpClientResponse = await httpClient!.GetAsync(knownUrl);
            httpClientResponse.EnsureSuccessStatusCode();
            var forecasts = await httpClientResponse.FromResponseAsync<WeatherForecast[]>();

            //Assert 
            forecasts.Should().NotBeNull();
            forecasts.Should().HaveCount(5);
        }

    }
}