using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
//// ReSharper disable UnusedMember.Global

namespace Cake.ProGet.Asset
{
    /// <summary>
    /// Aliases for ProGet Asset Directory operations.
    /// </summary>
    [CakeAliasCategory("ProGetAsset")]
    public static class ProGetAssetAliases
    {
        /// <summary>
        /// Pushes an asset to the ProGet Asset Directory.
        /// </summary>
        /// <param name="context">The Cake Context.</param>
        /// <param name="assetPath">The FilePath of the asset.</param>
        /// <param name="assetUri">The base URI of the asset to push.</param>
        /// <param name="config">An IProGetConfiguration with the username and password.</param>
        /// <exception cref="ArgumentNullException">Thrown if context is null.</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("ProGetAsset")]
        public static void ProGetPushAsset(this ICakeContext context, FilePath assetPath, string assetUri, ProGetConfiguration config)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var asset = new ProGetAssetPusher(context.Log, config);
            asset.Publish(assetPath, assetUri);
        }

        /// <summary>
        /// Determines if the asset is published at an existing URI.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="assetUri">The URI of the asset.</param>
        /// <param name="config">An IProGetConfiguration with the username and password.</param>
        /// <returns>True if the asset exists, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if context is null.</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("ProGetAsset")]
        public static bool ProGetDoesAssetExist(this ICakeContext context, string assetUri, ProGetConfiguration config)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            var asset = new ProGetAssetPusher(context.Log, config);
            return asset.DoesAssetExist(assetUri);
        }

        /// <summary>
        /// Deletes an asset from the asset directory.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="assetUri">The URI of the asset.</param>
        /// <param name="config">See <see cref="ProGetConfiguration"/>.</param>
        /// <returns>True if the asset is deleted, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if context or config is null.</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("ProGetAsset")]
        public static bool ProGetDeleteAsset(this ICakeContext context, string assetUri, ProGetConfiguration config)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            var asset = new ProGetAssetPusher(context.Log, config);
            return asset.DeleteAsset(assetUri);
        }

        /// <summary>
        /// Download a single asset from a ProGet Asset Directory.
        /// </summary>
        /// <param name="context">The <see cref="ICakeContext"/> Cake context.</param>
        /// <param name="assetUri">The URI to the asset in the ProGet Asset Directory.</param>
        /// <param name="outputPath">The output path for the downloaded artifact.</param>
        /// <param name="config">The ProGet Configuration.</param>
        /// <exception cref="ArgumentNullException">Thrown if context or config is null.</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("ProGetAsset")]
        public static void ProGetDownloadAsset(this ICakeContext context, string assetUri, FilePath outputPath,
            ProGetConfiguration config)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            var asset = new ProGetAssetDownloader(config);
            asset.GetSingleAsset(assetUri, outputPath);
        }
        
        /// <summary>
        /// Download an entire ProGet Asset Directory as a zip file.
        /// </summary>
        /// <param name="context">The <see cref="ICakeContext"/> Cake context.</param>
        /// <param name="assetDirectoryUri">The URI to the ProGet Asset Directory.</param>
        /// <param name="outputPath">The output path for the downloaded artifact.</param>
        /// <param name="config">The ProGet Configuration.</param>
        /// <exception cref="ArgumentNullException">Thrown if context or config is null.</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("ProGetAsset")]
        public static void ProGetDownloadAssetDirectory(this ICakeContext context, string assetDirectoryUri,
            FilePath outputPath, ProGetConfiguration config)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            var asset = new ProGetAssetDownloader(config);
            asset.GetDirectoryOfAssets(assetDirectoryUri, outputPath);
        }

        /// <summary>
        /// Creates a new Asset Directory.
        /// </summary>
        /// <param name="context">The <see cref="ICakeContext"/> Cake context.</param>
        /// <param name="assetDirectoryUri">The URI of the ProGet Asset Directory.</param>
        /// <param name="config">The ProGetConfiguration.</param>
        /// <exception cref="ArgumentNullException">Thrown if context or config is null.</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("ProGetAsset")]
        public static void ProGetCreateAssetDirectory(this ICakeContext context, string assetDirectoryUri,
            ProGetConfiguration config)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            var asset = new ProGetAssetDirectoryLister(config);
            asset.CreateDirectory(assetDirectoryUri);
        }

        /// <summary>
        /// Lists files in an Asset Directory.
        /// </summary>
        /// <param name="context">The <see cref="ICakeContext"/> Cake context.</param>
        /// <param name="assetDirectoryUri">The URI of the ProGet Asset Directory.</param>
        /// <param name="config">The ProGet Configuration.</param>
        /// <returns>A list of <see cref="ProGetDirectoryListing"/> entries.</returns>
        /// <exception cref="ArgumentNullException">Thrown if context or config is null.</exception>
        [CakeMethodAlias]
        [CakeAliasCategory("ProGetAsset")]
        public static List<ProGetDirectoryListing> ProGetListAssetDirectory(this ICakeContext context, string assetDirectoryUri,
            ProGetConfiguration config)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            
            var asset = new ProGetAssetDirectoryLister(config);
            return asset.ListDirectory(assetDirectoryUri);
        }
    }
}