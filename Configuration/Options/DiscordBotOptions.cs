using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDiscordBot.Configuration.Options
{
    public sealed class DiscordBotOptions
    {
        #region Fields
        /// <summary>
        ///     
        /// </summary>
        private string token; 
        #endregion

        #region Properties
        /// <summary>
        ///     
        /// </summary>
        public string Token { get => token; set => token = value; } 
        #endregion
    }
}
