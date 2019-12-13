namespace {{cookiecutter.project_slug}}.Api.IntegrationTests
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using {{cookiecutter.project_slug}}.Api;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Protocols;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using System.Net.Http;
    using NSubstitute;

    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        public JwtTokenIssuer TokenIssuer { get; private set; }

        public CustomWebApplicationFactory() : base()
        {
            this.TokenIssuer = new JwtTokenIssuer();
            IdentityModelEventSource.ShowPII = true;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTests");

            builder.ConfigureTestServices(services =>
            {
            });
        }
    }
}
