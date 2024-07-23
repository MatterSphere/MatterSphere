using System.Xml.Serialization;

namespace MatterSphereBundlerLibrary
{
    /// <summary>
    /// Represents the document.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Gets or sets the encrypted password for a protected document.
        /// </summary>
        [XmlAttribute]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the whole path including the filename and extension
        /// </summary>
        public string Path { get; set; }
    }
}
