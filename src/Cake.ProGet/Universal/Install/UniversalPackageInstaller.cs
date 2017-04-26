using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ProGet.Universal.Install
{
    /// <summary>
    /// Installs a universal package archive
    /// </summary>
    /// <seealso cref="UPackTool{UniversalPackageInstallSettings}" />
    public sealed class UniversalPackageInstaller : UPackTool<UniversalPackageInstallSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackageInstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tools.</param>
        /// <param name="resolver">The resolver.</param>
        public UniversalPackageInstaller(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, IUPackToolResolver resolver)
            : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Executes the command using the specified settings
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Execute(UniversalPackageInstallSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrEmpty(settings.Package))
            {
                throw new CakeException("Required setting Package not specified.");
            }

            if (string.IsNullOrEmpty(settings.Source))
            {
                throw new CakeException("Required setting Source not specified.");
            }

            if (settings.TargetDirectory == null)
            {
                throw new CakeException("Required setting TargetDirectory not specified.");
            }

            var builder = new ProcessArgumentBuilder();

            builder.Append("install");

            builder.Append(settings.Package);

            if (settings.HasVersion())
            {
                builder.Append(settings.Version);
            }

            builder.Append("--source={0}", settings.Source);
            builder.Append("--target=\"{0}\"",  settings.TargetDirectory.MakeAbsolute(this.Environment));

            if (settings.Overwrite)
            {
                builder.Append("--overwrite");
            }

            if (settings.HasCredentials())
            {
                if (!settings.AreCredentialsValid())
                {
                    throw new CakeException("Both username and password must be specified for authentication");
                }

                builder.Append("--user={0}", $"{settings.UserName}:{settings.Password}");
            }

            Run(settings, builder);
        }
    }
}