using System;
using Cake.Core.IO;
using Cake.Core.Tooling;
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace Cake.ProGet.Universal.Unpack
{
    /// <summary>
    /// Contains settings used for the <see cref="UniversalPackageUnpacker"/> command.
    /// </summary>
    /// <seealso cref="Cake.Core.Tooling.ToolSettings" />
    public sealed class UniversalPackageUnpackSettings : ToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackageUnpackSettings"/> class.
        /// </summary>
        public UniversalPackageUnpackSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackageUnpackSettings"/> class.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <exception cref="System.ArgumentException">
        /// Value cannot be null or empty. - package
        /// or
        /// Value cannot be null or empty. - source
        /// </exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the target directory is null</exception>
        public UniversalPackageUnpackSettings(FilePath package, DirectoryPath targetDirectory)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (targetDirectory == null)
            {
                throw new ArgumentNullException(nameof(targetDirectory));
            }

            this.Package = package;
            this.TargetDirectory = targetDirectory;
        }

        /// <summary>
        /// Gets or sets the upack file to unpack
        /// </summary>
        public FilePath Package { get; set; }

        /// <summary>
        /// Gets or sets the target directory to unpack the package
        /// </summary>
        public DirectoryPath TargetDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the package should be overwritten if it already exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if package should be overwritten; otherwise, <c>false</c>.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool Overwrite { get; set; }
    }
}
