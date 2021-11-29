using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonoGameDiscordBot.Configuration.Options;

namespace MonoGameDiscordBot.Services
{
    /// <summary>
    ///     Hosted service for the Discord bot and all its functionality
    /// </summary>
    public sealed partial class MonoGameDiscordBotService : IHostedService, IDisposable
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
        private IOptions<DiscordBotOptions> discordBotOptions;
        private bool isDisposed;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

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
        ///     The cache.
        /// </summary>
        public IDistributedCache Cache { get => cache; private set => cache = value; }

        /// <summary>
        ///     The options container specifying discord secrets.
        /// </summary>
        public IOptions<DiscordOptions> DiscordOptions { get => discordOptions; private set => discordOptions = value; }

        /// <summary>
        ///     The options for the bot.
        /// </summary>
        public IOptions<DiscordBotOptions> DiscordBotOptions { get => discordBotOptions; private set => discordBotOptions = value; }

        /// <summary>
        ///     The client used for connecting to Discord.
        /// </summary>
        public DiscordSocketClient DiscordClient { get => discordClient; set => discordClient = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public MonoGameDiscordBotService()
        {

        }

        /// <summary>
        ///     The DI based constructor
        /// </summary>
        /// <param name="appLifeTime"></param>
        /// <param name="appEnvironment"></param>
        /// <param name="logger"></param>
        /// <param name="cache"></param>
        /// <param name="discordOptions"></param>
        /// <param name="discordBotOptions"></param>
        public MonoGameDiscordBotService(IHostApplicationLifetime appLifeTime,
            IHostEnvironment appEnvironment,
            ILogger<MonoGameDiscordBotService> logger,
            IDistributedCache cache,
            IOptions<DiscordOptions> discordOptions,
            IOptions<DiscordBotOptions> discordBotOptions
            ) : this()
        {
            AppLifeTime = appLifeTime ?? throw new ArgumentNullException(nameof(appLifeTime));
            AppEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            ServiceLogger = logger ?? throw new ArgumentNullException(nameof(logger));
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            DiscordOptions = discordOptions ?? throw new ArgumentNullException(nameof(discordOptions));
            DiscordBotOptions = discordBotOptions ?? throw new ArgumentNullException(nameof(discordBotOptions));
            DiscordClient = new DiscordSocketClient(new DiscordSocketConfig()
            {
                UseSystemClock = true
            });

            AppLifeTime.ApplicationStarted.Register(OnStarted);
            AppLifeTime.ApplicationStopped.Register(OnStopped);
            AppLifeTime.ApplicationStopping.Register(OnStopping);
        }

        /// <summary>
        ///     
        /// </summary>
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
        ///     The asynchronous method that is used for starting the hosted service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            DiscordClient.ChannelCreated += DiscordClient_ChannelCreated;
            DiscordClient.ChannelDestroyed += DiscordClient_ChannelDestroyed;
            DiscordClient.CurrentUserUpdated += DiscordClient_CurrentUserUpdated;
            DiscordClient.Disconnected += DiscordClient_Disconnected;
            DiscordClient.Connected += DiscordClient_Connected;
            DiscordClient.GuildAvailable += DiscordClient_GuildAvailable;
            DiscordClient.GuildUpdated += DiscordClient_GuildUpdated;
            DiscordClient.GuildMemberUpdated += DiscordClient_GuildMemberUpdated;
            DiscordClient.MessageDeleted += DiscordClient_MessageDeleted;
            DiscordClient.MessageReceived += DiscordClient_MessageReceived;
            DiscordClient.MessageUpdated += DiscordClient_MessageUpdated;
            DiscordClient.MessagesBulkDeleted += DiscordClient_MessagesBulkDeleted;

            await DiscordClient.LoginAsync(TokenType.Bot, DiscordBotOptions.Value.Token, true);
            await DiscordClient.StartAsync();
        }

        #region Events
        /// <summary>
        ///     Invoked when messages are deleted in bulk.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        private Task DiscordClient_MessagesBulkDeleted(IReadOnlyCollection<Cacheable<IMessage, ulong>> arg1, ISocketMessageChannel arg2)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        private Task DiscordClient_MessageUpdated(Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task DiscordClient_MessageReceived(SocketMessage arg)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        private Task DiscordClient_MessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        private Task DiscordClient_GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        private Task DiscordClient_GuildUpdated(SocketGuild arg1, SocketGuild arg2)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task DiscordClient_GuildAvailable(SocketGuild arg)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     The event callback for when the Discord client has connected
        /// </summary>
        /// <returns></returns>
        private Task DiscordClient_Connected()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     The event callback for when the Discord client has disconnected
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task DiscordClient_Disconnected(Exception arg)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        private Task DiscordClient_CurrentUserUpdated(SocketSelfUser arg1, SocketSelfUser arg2)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task DiscordClient_ChannelDestroyed(SocketChannel arg)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task DiscordClient_ChannelCreated(SocketChannel arg)
        {
            return Task.CompletedTask;
        }
        #endregion

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
        /// <param name="disposing">Boolean value indicating if we are explicitly disposing</param>
        public void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            DiscordClient?.Dispose();
            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
