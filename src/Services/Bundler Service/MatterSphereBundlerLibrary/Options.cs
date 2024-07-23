using System;

namespace MatterSphereBundlerLibrary
{
    /// <summary>
    /// Represent the options of the bundle.
    /// </summary>
    public class Options
    {
        #region Properties

        /// <summary>
        /// Gets or sets the TableOfContents.
        /// </summary>
        public TableOfContents TableOfContents { get; set; }

        /// <summary>
        /// Gets or sets the TableOfContentsHeader.
        /// </summary>
        public TableOfContentsHeader TableOfContentsHeader { get; set; }

        /// <summary>
        /// Gets or sets the page numbering options.
        /// </summary>
        public PageNumbering PageNumbering { get; set; }

        /// <summary>
        /// Gets or sets the cover page.
        /// </summary>
        public string CoverPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether first document in list as cover page.
        /// </summary>
        public bool FirstDocumentInListAsCoverPage { get; set; }

        /// <summary>
        /// Gets or sets the back page.
        /// </summary>
        public string BackPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to include blank page.
        /// </summary>
        public bool BlankPageInserts { get; set; }

        public ImageCompressionOptions ImageCompressionOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to view bundled document.
        /// </summary>
        private bool ViewBundledDocument { get; set; }

        #endregion Properties
    }

    public class ImageCompressionOptions
    {
        public bool CompressImages { get; set; }
        
        public int ImageQuality { get; set; }
        
        public bool ResizeImages { get; set; }
        
        public int MaxResolution { get; set; }
    }
}
