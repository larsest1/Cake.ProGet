using System;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ProGet.Universal.Pack
{
    /// <summary>
    /// Contains settings used for the <see cref="UniversalPackagePacker"/> command.
    /// </summary>
    /// <seealso cref="Cake.Core.Tooling.ToolSettings" />
    public sealed class UniversalPackagePackSettings : ToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackagePackSettings"/> class.
        /// </summary>
        /// <param name="metadataFilePath">The metadata file path.</param>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// metadataFilePath
        /// or
        /// sourceDirectory
        /// </exception>
        public UniversalPackagePackSettings(FilePath metadataFilePath, DirectoryPath sourceDirectory)
        {
            if (metadataFilePath == null)
            {
                throw new ArgumentNullException(nameof(metadataFilePath));
            }
            if (sourceDirectory == null)
            {
                throw new ArgumentNullException(nameof(sourceDirectory));
            }

            this.MetadataFilePath = metadataFilePath;
            this.SourceDirectory = sourceDirectory;
        }

        /// <summary>
        /// Gets the upack.json metadata file path.
        /// </summary>
        public FilePath MetadataFilePath { get; }

        /// <summary>
        /// Gets the directory containing files to add to the package.
        /// </summary>
        public DirectoryPath SourceDirectory { get; }

        /// <summary>
        /// Gets or sets the directory where the .upack file will be created. If not specified, the current working directory is used.
        /// </summary>
        public DirectoryPath TargetDirectory { get; set; }
    }
}