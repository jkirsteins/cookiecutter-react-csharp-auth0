namespace {{cookiecutter.project_slug}}.Api.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using {{cookiecutter.project_slug}}.Api;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class HealthTests
    : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public HealthTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/v2019-11-04/health")]
        [InlineData("/v{{cookiecutter.api_version}}/health")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/plain; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var body = await response.Content.ReadAsStringAsync();
            Assert.Equal("OK", body);
        }
    }
}
