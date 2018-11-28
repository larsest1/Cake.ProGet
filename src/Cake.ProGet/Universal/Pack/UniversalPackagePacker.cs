using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ProGet.Universal.Pack
{
    /// <summary>
    /// Packs a universal package archive
    /// </summary>
    /// <seealso cref="UPackTool{PackSettings}" />
    public sealed class UniversalPackagePacker : UPackTool<UniversalPackagePackSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackagePacker"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tools.</param>
        /// <param name="resolver">The resolver.</param>
        public UniversalPackagePacker(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, IUPackToolResolver resolver)
            : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Executes the command using the specified settings
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Execute(UniversalPackagePackSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.MetadataFilePath == null)
            {
                throw new CakeException("Required setting MetadataFilePath not specified.");
            }

            if (settings.SourceDirectory == null)
            {
                throw new CakeException("Required setting SourceDirectory not specified.");
            }

            if (!this.FileSystem.GetFile(settings.MetadataFilePath).Exists)
            {
                throw new CakeException($"Metadata file does not exist at '{settings.MetadataFilePath.FullPath}'");
            }

            if (!this.FileSystem.GetDirectory(settings.SourceDirectory).Exists)
            {
                throw new CakeException($"Source directory does not exist at '{settings.SourceDirectory.FullPath}'");
            }

            var builder = new ProcessArgumentBuilder();

            builder.Append("pack");
            
            builder.AppendQuoted(settings.SourceDirectory.MakeAbsolute(Environment).FullPath);
            builder.AppendQuoted("--metadata=\"{0}\"", settings.MetadataFilePath.MakeAbsolute(Environment).FullPath);

            // make sure the target directory exists.
            if (settings.TargetDirectory != null)
            {
                var dir = this.FileSystem.GetDirectory(settings.TargetDirectory);
                if (!dir.Exists)
                {
                    dir.Create();
                }

                builder.Append("--targetDirectory=\"{0}\"", settings.TargetDirectory.MakeAbsolute(Environment).FullPath);
            }

            Run(settings, builder);
        }
    }
}
