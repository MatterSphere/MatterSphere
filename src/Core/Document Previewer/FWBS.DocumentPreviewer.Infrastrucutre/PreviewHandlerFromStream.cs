namespace Fwbs.Documents.Preview.Handlers
{
    using System.IO;

    public class PreviewHandlerFromStream : PreviewHandler, IInitializeWithStream
    {
        #region IInitializeWithStream Members

        protected Stream stream;

        public void Initialize(System.Runtime.InteropServices.ComTypes.IStream pstream, uint grfMode)
        {
            this.stream = pstream as Stream;
            if (this.stream == null)
                this.stream = new ReadOnlyStream(pstream);
        }

        #endregion

        

    }
}
