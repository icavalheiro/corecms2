using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCMS.Tools
{
    /// <summary>
    /// Simple wrapper class to easy console debugging.
    /// </summary>
    public static class Debug
    {
        public static bool DisableLogs = false;

        /// <summary>
        /// Logs "what" into the console if the logs are not disabled.
        /// </summary>
        /// <param name="what">What to log into the console.</param>
        public static void Log(object what)
        {
            if(!DisableLogs)
                Console.WriteLine(what);
        }
    }
}
