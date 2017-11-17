using System;
//// ReSharper disable UnusedMember.Global

namespace Cake.ProGet.Asset
{
    /// <summary>
    /// This is a single ProGet Asset Directory item.
    /// </summary>
    public sealed class ProGetDirectoryListing
    {
        /// <summary>
        /// Gets or sets a string containing the local name of the asset. This property does not include the full path. 
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets a string containing the full path of the parent directory of the asset. This property does not include the name of the asset itself. This property may be omitted if the asset is contained in the directory root. 
        /// </summary>
        public string Parent { get; set; }
        
        /// <summary>
        /// Gets or sets a string containing either the Content-Type of the the asset, or the literal text dir if the item represents a subdirectory. 
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Gets or sets the original creation time of the file in UTC.
        /// </summary>
        public DateTime Created { get; set; }
        
        /// <summary>
        /// Gets or sets the last modified or updated time of the file in UTC. This property is omitted if the item represents a subdirectory.
        /// </summary>
        public DateTime Modified { get; set; }
        
        /// <summary>
        /// Gets or sets the number of bytes in size of the asset item. This property is omitted if the item represents a subdirectory.
        /// </summary>
        public long Size { get; set; }
        
        /// <summary>
        /// Gets or sets the string containing the SHA1 hash of the asset item. This property is omitted if the item represents a subdirectory. 
        /// </summary>
        public string Sha1 { get; set; }
    }
}