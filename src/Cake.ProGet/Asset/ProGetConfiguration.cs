namespace Cake.ProGet.Asset
{
    /// <summary>
    /// The username and password for the ProGet instance.
    /// </summary>
    public sealed class ProGetConfiguration 
    {
        /// <summary>
        /// Gets or sets the ProGet Username
        /// </summary>
        public string ProGetUser { get; set; }

        /// <summary>
        /// Gets or sets the ProGet Password
        /// </summary>
        public string ProGetPassword { get; set; }
    }
}