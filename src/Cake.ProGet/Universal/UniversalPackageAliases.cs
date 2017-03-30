using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.ProGet.Universal.Install;
using Cake.ProGet.Universal.Pack;
using Cake.ProGet.Universal.Push;
using Cake.ProGet.Universal.Unpack;

namespace Cake.ProGet.Universal
{
    /// <summary>
    /// Contains functionality for working with <see href="https://inedo.com/support/documentation/proget/reference/universal-feed-api-and-package-format">UPack</see>
    /// </summary>
    [CakeAliasCategory("UPack")]
    [CakeNamespaceImport("Cake.ProGet.Universal.Pack")]
    [CakeNamespaceImport("Cake.ProGet.Universal.Push")]
    [CakeNamespaceImport("Cake.ProGet.Universal.Install")]
    [CakeNamespaceImport("Cake.ProGet.Universal.Unpack")]
    public static class UniversalPackageAliases
    {
        /// <summary>
        /// Creates a universal package
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the context is null</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("Pack")]
        public static void Pack(this ICakeContext context, UniversalPackagePackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new UPackToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new UniversalPackagePacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);

            runner.Execute(settings);
        }

        /// <summary>
        /// Publishes a universal package
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the context is null</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("Push")]
        public static void Push(this ICakeContext context, UniversalPackagePushSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new UPackToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new UniversalPackagePusher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);

            runner.Execute(settings);
        }

        /// <summary>
        /// Unpacks a universal package
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the context is null</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("Unpack")]
        public static void Unpack(this ICakeContext context, UniversalPackageUnpackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new UPackToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new UniversalPackageUnpacker(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);

            runner.Execute(settings);
        }

        /// <summary>
        /// Installs a universal package
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the context is null</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("Install")]
        public static void Install(this ICakeContext context, UniversalPackageInstallSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resolver = new UPackToolResolver(context.FileSystem, context.Environment, context.Tools);
            var runner = new UniversalPackageInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, resolver);

            runner.Execute(settings);
        }
    }
}
