// <copyright file="Startup.cs" company="{{cookiecutter.company}}">
// Copyright (c) {{cookiecutter.company}}. All rights reserved.
// </copyright>

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Backend.IntegrationTests")]

namespace {{cookiecutter.project_slug}}.Api
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Security.Claims;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using System.Xml.XPath;
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.CookiePolicy;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// The Startup class for configuring the application.
    /// </summary>
    public class Startup
    {
        private readonly IWebHostEnvironment environment;

        static Startup()
        {
            // Place static initialization here, e.g. MongoDB
            // conventions, or similar ...
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">Environment.</param>
        /// <param name="configuration">Configuration.</param>
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.environment = env;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets configuration for the current environment.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = ApiVersion.Parse("{{cookiecutter.api_version}}");
            });

            services.AddHttpsRedirection(o =>
            {
                o.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
                o.HttpsPort = 443;
            });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(cao =>
            {
                cao.Cookie.Domain = this.Configuration["Frontend:CookieDomain"];
                cao.Cookie.SameSite = SameSiteMode.None;

                cao.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = $"https://{this.Configuration["Auth0:Domain"]}/";
                options.Audience = this.Configuration["Auth0:Audience"];

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = (context) =>
                    {
                        var token = (context.SecurityToken as JwtSecurityToken).RawData;
                        var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;
                        claimsIdentity.AddClaim(new Claim("access_token", token));
                        return Task.CompletedTask;
                    },
                };
            });

            services.AddProblemDetails(setup =>
            {
                setup.IncludeExceptionDetails = _ => !this.environment.IsDevelopment();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v{{cookiecutter.api_version}}", new OpenApiInfo { Title = "{{cookiecutter.project_slug}} API", Version = "v{{cookiecutter.api_version}}" });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "{{cookiecutter.api_version}}.Api.xml");
                c.IncludeXmlComments(filePath);
                var xmlDocFile = Path.Combine(AppContext.BaseDirectory, $"{this.environment.ApplicationName}.xml");
                if (File.Exists(xmlDocFile))
                {
                    var comments = new XPathDocument(xmlDocFile);
                    c.OperationFilter<XmlCommentsOperationFilter>(comments);
                    c.SchemaFilter<XmlCommentsSchemaFilter>(comments);
#pragma warning disable CS0618 // Type or member is obsolete
                    c.DescribeAllEnumsAsStrings();
#pragma warning restore CS0618 // Type or member is obsolete
                }
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCasePropertyNamingPolicy();
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Web host environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v{{cookiecutter.api_version}}/swagger.json", "API v{{cookiecutter.api_version}}");
                c.SwaggerEndpoint("/swagger/v2019-11-04/swagger.json", "API v2019-11-04");
            });

            app.UseRouting();
            app.UseProblemDetails();
            app.UseHttpsRedirection();
            app.UseHsts();

            app.UseCors(builder =>
            {
                string[] cors = this.Configuration["Frontend:CorsOrigins"].Split(",");
                builder.WithOrigins(cors)
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
            });

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always,
                HttpOnly = HttpOnlyPolicy.Always,
            });

            app.UseAuthentication();
            app.UseAuthorization();

            // This must come after UseAuth*
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseMvc();
        }
    }
}
