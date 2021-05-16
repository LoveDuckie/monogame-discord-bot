namespace MonoGameDiscordBot
{
    /// <summary>
    ///     
    /// </summary>
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

        /// <summary>
        ///     
        /// </summary>
        public static readonly string DefaultAppConfigFileName = $"{AppConfigFileName}.json";
        #endregion

        #region Static Methods
        /// <summary>
        ///     
        /// </summary>
        /// <param name="environmentName"></param>
        /// <returns>Returns the newly generated string from the environment name</returns>
        public static string GetHostConfigJsonFileName(string environmentName) => $"{HostConfigFileName}.{environmentName}.json";

        /// <summary>
        ///     
        /// </summary>
        /// <param name="environmentName"></param>
        /// <returns>Returns the newly generated string from the environment name</returns>
        public static string GetAppConfigJsonFileName(string environmentName) => $"{AppConfigFileName}.{environmentName}.json";
        #endregion
    }
}
