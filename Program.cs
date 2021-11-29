    using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonoGameDiscordBot.Configuration.Options;
using MonoGameDiscordBot.Services;

namespace MonoGameDiscordBot
{
    /// <summary>
    /// 
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     Create the host builder.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>Returns the generated host builder.</returns>
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
                if (hostContext.HostingEnvironment.IsDevelopment())
                {
                    services.AddDistributedMemoryCache((options) =>
                    {
                    });
                }
                else
                {
                    IConfigurationSection redisConfigSection = hostContext.Configuration.GetSection(nameof(RedisOptions));

                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.InstanceName = "";

                        options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                        {
                             
                        };
                    });
                }

                services.Configure<DiscordOptions>(hostContext.Configuration.GetSection(nameof(DiscordOptions)));
                services.Configure<DiscordBotOptions>(hostContext.Configuration.GetSection(nameof(DiscordBotOptions)));
                services.Configure<RedisOptions>(hostContext.Configuration.GetSection(nameof(RedisOptions)));
                services.AddOptions();
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
