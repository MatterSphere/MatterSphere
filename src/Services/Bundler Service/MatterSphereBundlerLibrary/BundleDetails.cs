using System;

namespace MatterSphereBundlerLibrary
{
    /// <summary>
    /// Represents the bundle details.
    /// </summary>
    public class BundleDetails
    {
        public enum Status : byte
        {
            Success = 0,
            Error = 1,
            Warning = 2
        }

        /// <summary>
        /// Initializes the new instance of the <see cref="BundleDetails"/>
        /// </summary>
        public BundleDetails() { }

        #region Properties

        /// <summary>
        /// Gets or sets the bundle identifier.
        /// </summary>
        public long BundleID { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public Options Options { get; set; }

        /// <summary>
        /// Gets or sets the documents for a bundle.
        /// </summary>
        public Document[] Documents { get; set; }

        /// <summary>
        /// Gets or sets the application information.
        /// </summary>
        public ApplicationInformation ApplicationInformation { get; set; }

        #endregion Properties   
    }
}
