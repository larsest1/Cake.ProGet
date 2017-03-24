using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ProGet.Universal.Unpack
{
    /// <summary>
    /// Unpacks a universal package archive
    /// </summary>
    /// <seealso cref="UPackTool{UniversalPackageUnpackSettings}" />
    public sealed class UniversalPackageUnpacker : UPackTool<UniversalPackageUnpackSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackageUnpacker"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tools.</param>
        /// <param name="resolver">The resolver.</param>
        public UniversalPackageUnpacker(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, IUPackToolResolver resolver)
            : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Executes the command using the specified settings
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Execute(UniversalPackageUnpackSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (!this.FileSystem.GetFile(settings.Package).Exists)
            {
                throw new ArgumentException($"Universal package file does not exist at '{settings.Package.FullPath}'");
            }

            var builder = new ProcessArgumentBuilder();

            builder.Append("unpack");

            builder.AppendQuoted(settings.Package.FullPath);
            builder.AppendQuoted(settings.TargetDirectory.FullPath);
            
            if (settings.Overwrite)
            {
                builder.Append("--overwrite");
            }
            
            Run(settings, builder);
        }
    }
}
