using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ProGet.Universal.Push
{
    /// <summary>
    /// Packs a universal package archive
    /// </summary>
    /// <seealso cref="UPackTool{UniversalPackagePackSettings}" />
    public sealed class UniversalPackagePusher : UPackTool<UniversalPackagePushSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalPackagePusher"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tools.</param>
        /// <param name="resolver">The resolver.</param>
        public UniversalPackagePusher(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, IUPackToolResolver resolver)
            : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Executes the command using the specified settings
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Execute(UniversalPackagePushSettings settings)
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

            builder.Append("push");

            builder.AppendQuoted(settings.Package.FullPath);
            builder.AppendQuoted(settings.Target);

            if (settings.HasCredentials())
            {
                if (!settings.AreCredentialsValid())
                {
                    throw new ArgumentException("Both username and password must be specified for authentication");
                }
                
                builder.Append("--user={0}", $"{settings.UserName}:{settings.Password}");
            }

            Run(settings, builder);
        }
    }
}
