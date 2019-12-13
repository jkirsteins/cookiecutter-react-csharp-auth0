// <copyright file="FrontendSettings.cs" company="{{cookiecutter.company}}">
// Copyright (c) {{cookiecutter.company}}. All rights reserved.
// </copyright>

namespace {{cookiecutter.project_slug}}.Api
{
    /// <summary>
    /// VGF frontend settings.
    /// </summary>
    public class FrontendSettings
    {
        /// <summary>
        /// Gets the allowed CORS origins.
        /// </summary>
        /// <value>Comma-separated origins (e.g. https://app.example.com).</value>
        public string CorsOrigins { get; internal set; }

        /// <summary>
        /// Gets the public frontend URL.
        /// </summary>
        /// <value>URL.</value>
        public string Url { get; internal set; }
    }
}
