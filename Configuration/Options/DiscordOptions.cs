using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDiscordBot.Configuration.Options
{
    public sealed class DiscordOptions
    {
        #region Fields
        /// <summary>
        ///     Use the system clock
        /// </summary>
        private bool useSystemClock;

        /// <summary>
        ///     The size of the message cache
        /// </summary>
        private int messageCacheSize;
        
        /// <summary>
        ///     The host of the gateway.
        /// </summary>
        private string gatewayHost;
        
        /// <summary>
        ///     The timeout for the handler.
        /// </summary>
        private int? handlerTimeout;
        #endregion

        #region Properties
        /// <summary>
        ///     
        /// </summary>
        public bool UseSystemClock { get => useSystemClock; set => useSystemClock = value; }

        /// <summary>
        ///     
        /// </summary>
        public int MessageCacheSize { get => messageCacheSize; set => messageCacheSize = value; }

        /// <summary>
        ///     
        /// </summary>
        public string GatewayHost { get => gatewayHost; set => gatewayHost = value; }

        /// <summary>
        ///     
        /// </summary>
        public int? HandlerTimeout { get => handlerTimeout; set => handlerTimeout = value; } 
        #endregion
    }
}
