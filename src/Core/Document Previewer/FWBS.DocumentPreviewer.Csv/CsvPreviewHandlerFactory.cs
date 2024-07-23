using System;

namespace Fwbs.Documents.Preview.Csv
{
    using System.ComponentModel.Composition;
    using Handlers;

    [Export(CSV, typeof(IPreviewHandlerFactory))]
    public class CsvPreviewHandlerFactory : IPreviewHandlerFactory 
    {
        public const string ClassID = "27D7B7CC-A4B9-4126-B4CB-385589072A49";

        public const string CSV = "CSV";

        public Guid ID
        {
            get
            {
                return new Guid(ClassID);
            }
        }

        public IPreviewHandler CreateHandler()
        {
            return new CSVPreviewHandlerControl();
        }
    }
}
