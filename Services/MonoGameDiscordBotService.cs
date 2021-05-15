using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonoGameDiscordBot.Configuration.Options;

namespace MonoGameDiscordBot.Services
{
    /// <summary>
    ///     
    /// </summary>
    public sealed class MonoGameDiscordBotService : IHostedService, IDisposable
    {
        #region Fields
        /// <summary>
        ///     
        /// </summary>
        private IHostApplicationLifetime appLifeTime;

        /// <summary>
        /// 
        /// </summary>
        private ILogger serviceLogger;

        /// <summary>
        ///     
        /// </summary>
        private IHostEnvironment appEnvironment;

        /// <summary>
        ///     
        /// </summary>
        private IDistributedCache cache;

        /// <summary>
        ///     
        /// </summary>
        private DiscordSocketClient discordClient;

        /// <summary>
        /// 
        /// </summary>
        private IOptions<DiscordOptions> discordOptions;
        #endregion

        #region Properties
        /// <summary>
        ///     The lifetime.
        /// </summary>
        public IHostApplicationLifetime AppLifeTime { get => appLifeTime; private set => appLifeTime = value; }

        /// <summary>
        ///     The application environment.
        /// </summary>
        public IHostEnvironment AppEnvironment { get => appEnvironment; private set => appEnvironment = value; }

        /// <summary>
        ///     The service logger
        /// </summary>
        public ILogger ServiceLogger { get => serviceLogger; private set => serviceLogger = value; }

        /// <summary>
        ///     The cache
        /// </summary>
        public IDistributedCache Cache { get => cache; private set => cache = value; }

        /// <summary>
        ///     The options container specifying discord secrets.
        /// </summary>
        public IOptions<DiscordOptions> DiscordOptions { get => discordOptions; private set => discordOptions = value; }

        /// <summary>
        /// 
        /// </summary>
        public DiscordSocketClient DiscordClient { get => discordClient; set => discordClient = value; }
        #endregion

        #region Constructors
        /// <summary>
        ///     The DI based constructor
        /// </summary>
        /// <param name="appLifeTime"></param>
        /// <param name="appEnvironment"></param>
        /// <param name="logger"></param>
        /// <param name="cache"></param>
        public MonoGameDiscordBotService(IHostApplicationLifetime appLifeTime, 
            IHostEnvironment appEnvironment, 
            ILogger<MonoGameDiscordBotService> logger, 
            IDistributedCache cache, 
            IOptions<DiscordOptions> discordOptions)
        {
            AppLifeTime = appLifeTime ?? throw new ArgumentNullException(nameof(appLifeTime));
            AppEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            ServiceLogger = logger ?? throw new ArgumentNullException(nameof(logger));
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            DiscordOptions = discordOptions ?? throw new ArgumentNullException(nameof(discordOptions));

            DiscordClient = new DiscordSocketClient(new DiscordSocketConfig()
            {
                UseSystemClock = true
            });

            AppLifeTime.ApplicationStarted.Register(OnStarted);
            AppLifeTime.ApplicationStopped.Register(OnStopped);
            AppLifeTime.ApplicationStopping.Register(OnStopping);
        }

        ~MonoGameDiscordBotService()
        {
            Dispose(false);
        }
        #endregion

        #region Lifetime Methods
        /// <summary>
        ///     Invoked at the beginning of the application's lifecycle
        /// </summary>
        private void OnStarted()
        {
            ServiceLogger.LogInformation("Started");
        }

        /// <summary>
        ///     Invoked when the application has terminated.
        /// </summary>
        private void OnStopped()
        {

            ServiceLogger.LogInformation("Stopped");
        }

        /// <summary>
        ///     Invoked when the application is being terminated.
        /// </summary>
        private void OnStopping()
        {
            ServiceLogger.LogInformation("Stopping");
        }
        #endregion

        

        #region Methods
        /// <summary>
        ///     
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await DiscordClient.LoginAsync(Discord.TokenType.Bot, DiscordOptions.Value.ClientKey, true);
            await DiscordClient.StartAsync();
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Returns the async task</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await DiscordClient.StopAsync();
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            else
            {
                DiscordClient.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
