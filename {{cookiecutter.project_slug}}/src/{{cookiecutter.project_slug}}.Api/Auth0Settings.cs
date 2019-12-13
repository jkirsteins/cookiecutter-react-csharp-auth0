// <copyright file="Auth0Settings.cs" company="{{cookiecutter.company}}">
// Copyright (c) {{cookiecutter.company}}. All rights reserved.
// </copyright>

namespace {{cookiecutter.project_slug}}.Api
{
    /// <summary>
    /// Auth0 typed settings class.
    /// </summary>
    public class Auth0Settings
    {
        /// <summary>
        /// Gets Auth0 domain.
        /// </summary>
        /// <value>String representing the domain.</value>
        public string Domain { get; internal set; }

        /// <summary>
        /// Gets Auth0 audience.
        /// </summary>
        /// <value>Auth0 audience.</value>
        public string Audience { get; internal set; }

        /// <summary>
        /// Gets Auth0 client secret.
        /// </summary>
        /// <value>Secret.</value>
        public string ClientSecret { get; internal set; }
    }
}
