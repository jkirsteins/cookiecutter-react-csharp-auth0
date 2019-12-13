// <copyright file="Program.cs" company="{{cookiecutter.company}}">
// Copyright (c) {{cookiecutter.company}}. All rights reserved.
// </copyright>

namespace {{cookiecutter.project_slug}}.Cli
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Hosting;
    using System.CommandLine.Invocation;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Class containing program entry point.
    /// </summary>
    public static class Program
    {
        private const string ServiceNamespacePrefix = "{{cookiecutter.project_slug}}.Cli.Services.";

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>s.</returns>
        public static async Task<int> Main(string[] args)
        {
            var allowedServices = Assembly.GetExecutingAssembly().GetTypes().Where(
                a => a.IsSubclassOf(typeof(BackgroundService))).Where(
                a => !a.IsAbstract).Select(
                a => a.FullName).Select(
                a => a.Substring(ServiceNamespacePrefix.Length)).ToArray();

            var serviceArg = new Argument<IEnumerable<Type>>(ConvertBackgroundService)
            {
                Arity = ArgumentArity.OneOrMore,
            };

            var rootCommand = new RootCommand
            {
                new Option("--service", "Service to start")
                {
                    Argument = serviceArg,
                    Required = true,
                },
            };

            rootCommand.Description = "Service runner";

            rootCommand.Handler = CommandHandler.Create<IHost>(h =>
            {
                h.WaitForShutdown();
            });

            return await new CommandLineBuilder(rootCommand).UseDefaults().UseHost(host =>
            {
                host.ConfigureHostConfiguration(c =>
                {
                    c.AddEnvironmentVariables(b => b.Prefix = "{{cookiecutter.env_var_prefix}}:");
                });

                host.ConfigureServices((context, services) =>
                {
                    services.AddLogging(o =>
                    {
                        o.AddConsole();
                        o.SetMinimumLevel(LogLevel.Debug);
                    });

                    // Add dynamically
                    services.AddOptions<ProgramOptions>().BindCommandLine();
                    var sp = services.BuildServiceProvider();
                    var cliOpts = sp.GetRequiredService<IOptions<ProgramOptions>>().Value;

                    // Invoke AddHostedService<T>() where T is the parsed type
                    var hostServiceExt = typeof(ServiceCollectionHostedServiceExtensions);
                    MethodInfo method = hostServiceExt.GetMethods().Single(o => o.IsGenericMethod && o.Name == "AddHostedService" && o.GetParameters().Length == 1);

                    foreach (var serviceType in cliOpts.Service)
                    {
                        MethodInfo generic = method.MakeGenericMethod(serviceType);
                        generic.Invoke(null, new[] { services });
                    }
                });
            }).Build().InvokeAsync(args).ConfigureAwait(false);
        }

        private static bool ConvertBackgroundService(SymbolResult result, out IEnumerable<Type> value)
        {
            value = result.Tokens.Select(o =>
            {
                var inName = o.Value;
                var singleItem = Type.GetType($"{ServiceNamespacePrefix}{inName}");
                var success = singleItem != null && singleItem.IsSubclassOf(typeof(BackgroundService)) && !singleItem.IsAbstract;
                return success ? singleItem : null;
            }).Where(o => o != null).ToArray();  // ToArray() so it runs only once

            return value.Count() == result.Tokens.Count;
        }
    }
}
