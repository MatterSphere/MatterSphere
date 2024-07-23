using System;

namespace MatterSphereBundlerLibrary
{
    /// <summary>
    /// Represent the page numbering options of the bundle.
    /// </summary>
    public class PageNumbering
    {
        public static readonly string DefaultFormat = "Document Bundle Page # of ##";

        public enum HorizontalAlignment
        {
            None = 0,
            Left = 1,
            Center = 2,
            Right = 3
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether need to include in bundle PDF.
        /// </summary>
        public bool IncludeInBundledPDF { get; set; }

        /// <summary>
        /// Gets or sets a string value for stamping page numbers.
        /// Value must include # character which is replaced with the page number in the process of stamping.
        /// It may optionally contain ## characters which are replaced with the total number of pages in the document.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets horizontal alignment of stamp on the page.
        /// </summary>
        public HorizontalAlignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to perform per-document page numbering.
        /// </summary>
        public bool PerDocument  { get; set; }

        #endregion Properties
    }
}