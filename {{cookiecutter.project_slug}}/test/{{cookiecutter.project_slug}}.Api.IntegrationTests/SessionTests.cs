namespace {{cookiecutter.project_slug}}.Api.IntegrationTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using {{cookiecutter.project_slug}}.Api;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Xunit;

    public class SessionTests
    : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SessionTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/session")]
        [InlineData("/v2019-11-04/session")]
        [InlineData("/v{{cookiecutter.api_version}}/session")]
        public async Task Post_CreateSession(string url)
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("https://localhost");

            // Not logged in
            var response = await client.GetAsync($"{url}/cookieinfo");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            // Create session with JWT
            var token = _factory.TokenIssuer.GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            response = await client.PostAsync(url, new StringContent(string.Empty, Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Cookie should be secure/samesite=strict/httponly
            var cookieSettings = response.GetCookieSettings(".AspNetCore.Cookies");
            Assert.NotEmpty(cookieSettings);
            Assert.Contains("secure", cookieSettings);

            Assert.DoesNotContain("samesite", cookieSettings); // API on diff domain (possibly) than SPA
            //samesite = none

            Assert.Contains("httponly", cookieSettings);

            // Check that we are authenticated
            client.DefaultRequestHeaders.Authorization = null;
            response = await client.GetAsync($"{url}/cookieinfo");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("/session/delete")]
        [InlineData("/v2019-11-04/session/delete")]
        [InlineData("/v{{cookiecutter.api_version}}/session/delete")]
        public async Task Delete_SignOutSession(string url)
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("https://localhost");

            // Create session with JWT
            var token = _factory.TokenIssuer.GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync("/session", new StringContent(string.Empty, Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Log out
            response = await client.PostAsync(url, new StringContent(string.Empty, Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Not logged in
            response = await client.GetAsync("/session/cookieinfo");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
