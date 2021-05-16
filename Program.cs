    using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonoGameDiscordBot.Configuration.Options;
using MonoGameDiscordBot.Services;

namespace MonoGameDiscordBot
{
    internal class Program
    {
        /// <summary>
        ///     Create the host builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, configBuilder) =>
            {
                configBuilder.Sources.Clear();

                IHostEnvironment env = hostContext.HostingEnvironment;

                string environmentName = env.EnvironmentName.ToLower();

                if (string.IsNullOrEmpty(environmentName))
                	throw new ArgumentNullException(nameof(environmentName),"The value \"environmentName\" is invalid or null");

                configBuilder.AddJsonFile(Constants.DefaultAppConfigFileName, optional: true);
                configBuilder.AddJsonFile(Constants.GetAppConfigJsonFileName(environmentName), optional: true);
            })
            .ConfigureHostConfiguration((configBuilder) =>
            {
                configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configBuilder.AddCommandLine(args);
                configBuilder.AddEnvironmentVariables(Constants.EnvironmentVariablePrefix);
                configBuilder.AddJsonFile(Constants.HostConfigFileName, optional: true);
                configBuilder.SetFileLoadExceptionHandler((exceptionContext) => {
                });
            })
            .ConfigureLogging((hostContext, logging) =>
            {
                if (hostContext.HostingEnvironment.IsDevelopment())
                {
                    logging.AddDebug();
                }

                logging.AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOptions();
                services.Configure<DiscordOptions>(hostContext.Configuration.GetSection(nameof(DiscordOptions)));
                services.AddHostedService<MonoGameDiscordBotService>();
            });
        }
        
        /// <summary>
        ///     
        /// </summary>
        /// <param name="args">The command-line arguments</param>
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}
