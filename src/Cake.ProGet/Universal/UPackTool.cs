using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ProGet.Universal
{
    /// <summary>
    /// Provides access to the UPack.exe tool
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    /// <seealso cref="Cake.Core.Tooling.Tool{TSettings}" />
    public abstract class UPackTool<TSettings> : Tool<TSettings> where TSettings : ToolSettings
    {
        private readonly IUPackToolResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="UPackTool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tools.</param>
        /// <param name="resolver">The resolver.</param>
        protected UPackTool(
           IFileSystem fileSystem,
           ICakeEnvironment environment,
           IProcessRunner processRunner,
           IToolLocator tools,
           IUPackToolResolver resolver) : base(fileSystem, environment, processRunner, tools)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }
            
            _resolver = resolver;

            this.FileSystem = fileSystem;
        }

        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>
        /// The file system.
        /// </value>
        protected IFileSystem FileSystem { get; }

        /// <inheritdoc />
        protected override string GetToolName()
        {
            return "UPack";
        }

        /// <inheritdoc />
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "upack.exe" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected sealed override IEnumerable<FilePath> GetAlternativeToolPaths(TSettings settings)
        {
            var path = _resolver.ResolvePath();
            if (path != null)
            {
                return new[] { path };
            }
            return Enumerable.Empty<FilePath>();
        }
    }
}
