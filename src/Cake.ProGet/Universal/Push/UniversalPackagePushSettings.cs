using System;
using Cake.Core.IO;
using Cake.Core.Tooling;
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace Cake.ProGet.Universal.Push
{
    /// <summary>
    /// Contains settings used for the <see cref="UniversalPackagePusher"/> command.
    /// </summary>
    /// <seealso cref="Cake.Core.Tooling.ToolSettings" />
    public sealed class UniversalPackagePushSettings : ToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackagePushSettings"/> class.
        /// </summary>
        public UniversalPackagePushSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackagePushSettings"/> class.
        /// </summary>
        /// <param name="packageFilePath">The package file path.</param>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentNullException">The <see cref="FilePath"/> of the upack to push</exception>
        /// <exception cref="System.ArgumentException">Value cannot be null or empty. - target</exception>
        public UniversalPackagePushSettings(FilePath packageFilePath, string target)
        {
            if (packageFilePath == null)
            {
                throw new ArgumentNullException(nameof(packageFilePath));
            }

            if (string.IsNullOrEmpty(target))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(target));
            }

            this.Package = packageFilePath;
            this.Target = target;
        }

        /// <summary>
        /// Gets or sets the <see cref="FilePath"/> of the package to push
        /// </summary>
        public FilePath Package { get; set; }

        /// <summary>
        /// Gets or sets the URL of a upack API endpoint.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the username for an authenticated endpoint
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for an authenticated endpoint
        /// </summary>
        public string Password { get; set; }

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
    }
}