using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDiscordBot.Configuration.Options
{
    public class RedisOptions
    {
        /// <summary>
        ///     
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Host { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool Ssl { get; set; }
    }
}
