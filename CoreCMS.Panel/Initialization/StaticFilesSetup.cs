using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;

namespace CoreCMS.Panel.Initialization
{
    internal class StaticFilesSetup : IPostConfigureOptions<StaticFileOptions>
    {
        private readonly IHostingEnvironment _environment;

        /// <summary>
        /// Constructor with dependecy injection.
        /// </summary>
        /// <param name="environment">Current environment that this applciation is running on.</param>
        public StaticFilesSetup(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Called by the MVC runtime on application initalization.
        /// </summary>
        /// <param name="name">Name (unused)</param>
        /// <param name="options">The options for the static file provider.</param>
        public void PostConfigure(string name, StaticFileOptions options)
        {
            //Lets check if the calling application already has a static file provider, if not lets create a new one.
            options.ContentTypeProvider = options.ContentTypeProvider ?? new FileExtensionContentTypeProvider();

            //Do we have any file provider on the calling applciation?
            if (options.FileProvider == null && _environment.WebRootFileProvider == null)
            {
                //we must have ate least one
                throw new InvalidOperationException("Missing FileProvider.");
            }

            //Get the file provider
            options.FileProvider = options.FileProvider ?? _environment.WebRootFileProvider;
            
            // Add our resource files from the assembly into the file provider pipeline
            var filesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, "Assets");

            //merge the provider pipelines
            options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
        }
    }
}
