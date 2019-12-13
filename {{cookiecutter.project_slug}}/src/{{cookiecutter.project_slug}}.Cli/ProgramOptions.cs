// <copyright file="ProgramOptions.cs" company="{{cookiecutter.company}}">
// Copyright (c) {{cookiecutter.company}}. All rights reserved.
// </copyright>

namespace {{cookiecutter.project_slug}}.Cli
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Command line options that can be passed to the program.
    /// </summary>
    public class ProgramOptions
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the list of services that should be started.
        /// </summary>
        public IEnumerable<Type> Service { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ connection URI.
        /// </summary>
        public Uri RabbitMqUri { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ quality-of-service value.
        /// </summary>
        public ushort RabbitMqQoS { get; set; }
    }
}
