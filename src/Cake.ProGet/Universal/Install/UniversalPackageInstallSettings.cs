using System;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ProGet.Universal.Install
{
    /// <summary>
    /// Contains settings used for the <see cref="UniversalPackageInstaller"/> command.
    /// </summary>
    /// <seealso cref="Cake.Core.Tooling.ToolSettings" />
    public sealed class UniversalPackageInstallSettings : ToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackageInstallSettings"/> class.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="source">The source.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <exception cref="System.ArgumentException">
        /// Value cannot be null or empty. - package
        /// or
        /// Value cannot be null or empty. - source
        /// </exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the target directory is null</exception>
        public UniversalPackageInstallSettings(string package, string source, DirectoryPath targetDirectory)
        {
            if (string.IsNullOrEmpty(package))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(package));
            }
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(source));
            }
            if (targetDirectory == null)
            {
                throw new ArgumentNullException(nameof(targetDirectory));
            }

            this.Package = package;
            this.Source = source;
            this.TargetDirectory = targetDirectory;
        }

        /// <summary>
        /// Gets the package to install
        /// </summary>
        public string Package { get; }

        /// <summary>
        /// Gets or sets the version to install.  If version is not specified, the latest will be retrieved.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets the URL of a upack API endpoint to install from.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Gets the target directory to install the package
        /// </summary>
        public DirectoryPath TargetDirectory { get; }

        /// <summary>
        /// Gets or sets the username for an authenticated endpoint
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for an authenticated endpoint
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the package should be overwritten if it already exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if package should be overwritten; otherwise, <c>false</c>.
        /// </value>
        public bool Overwrite { get; set; }

        /// <summary>
        /// Gets whether or not credentials are specified
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has credentials; otherwise, <c>false</c>.
        /// </returns>
        public bool HasCredentials()
        {
            return !string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Password);
        }

        /// <summary>
        /// Gets whether or not both username and password are specified
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has valid credentials; otherwise <c>false</c>
        /// </returns>
        public bool AreCredentialsValid()
        {
            return this.HasCredentials() && !string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password);
        }

        /// <summary>
        /// Determines whether this instance has a version specified.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has version; otherwise, <c>false</c>.
        /// </returns>
        public bool HasVersion()
        {
            return !string.IsNullOrWhiteSpace(Version);
        }
    }
}
