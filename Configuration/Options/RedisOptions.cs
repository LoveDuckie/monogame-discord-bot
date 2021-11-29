using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDiscordBot.Configuration.Options
{
    public sealed class RedisOptions
    {
        #region Fields
        /// <summary>
        ///     
        /// </summary>
        private bool ssl;
        /// <summary>
        ///     
        /// </summary>
        private string host;
        /// <summary>
        ///     
        /// </summary>
        private string password; 
        #endregion

        /// <summary>
        ///     
        /// </summary>
        public string Password { get => password; set => password = value; }

        /// <summary>
        /// 
        /// </summary>
        public string Host { get => host; set => host = value; }

        /// <summary>
        /// 
        /// </summary>
        public bool Ssl { get => ssl; set => ssl = value; }
    }
}
