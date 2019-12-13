// <copyright file="HealthController.cs" company="{{cookiecutter.company}}">
// Copyright (c) {{cookiecutter.company}}. All rights reserved.
// </copyright>

namespace {{cookiecutter.project_slug}}.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for healthcheck endpoints.
    /// </summary>
    [ApiVersion("2019-11-04")]
    [ApiVersion("{{cookiecutter.api_version}}")]
    [ApiController]
    [Route("health")]
    [Route("v{version:apiVersion}/health")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthController"/> class.
        /// </summary>
        public HealthController()
        {
        }

        /// <summary>
        /// Endpoint that returns OK.
        /// </summary>
        /// <returns>"OK".</returns>
        [HttpGet]
        public string Get()
        {
            return "OK";
        }
    }
}
