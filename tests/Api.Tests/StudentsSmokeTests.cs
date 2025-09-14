using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Api.Tests
{
    public class StudentsSmokeTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StudentsSmokeTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        private record TokenResponse(string access_token);

        private async Task AuthorizeAsync()
        {
            var resp = await _client.PostAsJsonAsync(
                "/api/security/token",
                new { userName = "admin", password = "wsei" });

            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var token = await resp.Content.ReadFromJsonAsync<TokenResponse>();
            if (token is null || string.IsNullOrWhiteSpace(token.access_token))
                throw new InvalidOperationException("Token was null/empty");

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
        }

        [Fact]
        public async Task Get_students_returns_ok()
        {
            await AuthorizeAsync();

            var resp = await _client.GetAsync("/api/students");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Post_students_creates_and_returns_201_or_200()
        {
            await AuthorizeAsync();

            var resp = await _client.PostAsJsonAsync(
                "/api/students",
                new { email = "john@uni.edu", firstName = "John", lastName = "Doe" });

            resp.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK);
        }
    }
}
