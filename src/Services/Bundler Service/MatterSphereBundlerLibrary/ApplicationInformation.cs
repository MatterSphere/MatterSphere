using System;

namespace MatterSphereBundlerLibrary
{
    /// <summary>
    /// Represents Application Information for creating bundle.
    /// </summary>
    public class ApplicationInformation
    {
        /// <summary>
        /// Gets or sets the name of calling application.
        /// </summary>
        public string CallingApplication { get; set; }

        #region Properties

        /// <summary>
        /// Gets or sets the matter id.
        /// </summary>
        public long MatterID { get; set; }

        /// <summary>
        /// Gets or sets the user-requester.
        /// In the format e.g. DOMAIN\User.
        /// </summary> 
        public string RequestedBy { get; set; }


        /// <summary>
        /// Gets or sets requester email
        /// </summary>
        public string RequestedByEmailAddress { get; set; }

        /// <summary>
        /// The file to which the bundle data is to be saved
        /// </summary>
        public string XMLBundleFileName { get; set; }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName { get; set; }


        /// <summary>
        /// Gets or sets the matter description.
        /// </summary>
        public string MatterDescription { get; set; }

        /// <summary>
        /// Gets or sets the client matter reference.
        /// In the format ClientNumber/MatterNumber.
        /// </summary>
        public string ClientMatterRef { get; set; }

        /// <summary>
        /// Gets or sets the bundle description.
        /// </summary>
        public string BundleDescription { get; set; }

        /// <summary>
        /// Gets or sets the current user id.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bundle has to be saved to a matter.
        /// </summary>
        public bool SaveBundleToMatter { get; set; }

        /// <summary>
        /// Gets or sets the document folder Guid bundle document.
        /// </summary>
        public Guid DocFolderGuid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to show markup.
        /// </summary>
        public bool PublishShowingMarkup { get; set; }

        #endregion
    }
}
