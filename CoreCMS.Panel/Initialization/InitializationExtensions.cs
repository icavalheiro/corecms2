using System;
using System.Collections.Generic;
using System.Text;
using CoreCMS.Panel.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCMS.Panel
{
    public static class InitializationExtensions
    {
        /// <summary>
        /// Enables the application to serve the static files that are necessary for the CoreCMS Panel
        /// to work properly, such files include CSS, JavaScript and Images used in the HTML pages of
        /// the Panel.
        /// </summary>
        /// <param name="services"></param>
        public static void AddCoreCMSPanelStaticFiles(this IServiceCollection services)
        {
            //setup static files
            services.ConfigureOptions(typeof(StaticFilesSetup));
        }
    }
}
