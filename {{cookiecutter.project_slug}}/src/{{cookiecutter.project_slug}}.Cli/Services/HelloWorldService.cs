// <copyright file="HelloWorldService.cs" company="{{cookiecutter.company}}">
// Copyright (c) {{cookiecutter.company}}. All rights reserved.
// </copyright>

namespace {{cookiecutter.project_slug}}.Cli.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Example service that prints "Hello World" and terminates
    /// the application.
    /// </summary>
    public class HelloWorldService : IHostedService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelloWorldService"/> class.
        /// </summary>
        /// <param name="lifetime">Application lifetime.</param>
        protected HelloWorldService(IHostApplicationLifetime lifetime)
        {
            this.Lifetime = lifetime;
        }

        /// <summary>
        /// Gets the application lifetime object.
        /// </summary>
        public IHostApplicationLifetime Lifetime { get; }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Hello World");
            this.Lifetime.StopApplication();
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
