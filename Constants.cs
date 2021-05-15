using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDiscordBot
{
    public static class Constants
    {
        #region Constants
        /// <summary>
        /// 
        /// </summary>
        public const string EnvironmentVariablePrefix = "MONOGAME_DISCORD_";
        
        /// <summary>
        ///     
        /// </summary>
        public const string HostConfigFileName = "hostconfig";
        
        /// <summary>
        ///     
        /// </summary>
        public const string AppConfigFileName = "appconfig";
        #endregion

        #region Static Methods
        /// <summary>
        ///     
        /// </summary>
        /// <param name="environmentName"></param>
        /// <returns>Returns the newly generated string from the environment name</returns>
        public static string GetHostConfigJsonFileName(string environmentName)
        {
            return $"{HostConfigFileName}.{environmentName}.json";
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="environmentName"></param>
        /// <returns>Returns the newly generated string from the environment name</returns>
        public static string GetAppConfigJsonFileName(string environmentName)
        {
            return $"{AppConfigFileName}.{environmentName}.json";
        } 
        #endregion
    }
}
