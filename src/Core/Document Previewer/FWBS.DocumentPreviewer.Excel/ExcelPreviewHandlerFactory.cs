using System;

namespace Fwbs.Documents.Preview.Excel
{
    using System.ComponentModel.Composition;
    using Handlers;

    [Export(XLSX, typeof(IPreviewHandlerFactory))]
    [Export(XLSM, typeof(IPreviewHandlerFactory))]
    [Export(XLSB, typeof(IPreviewHandlerFactory))]
    [Export(XLS, typeof(IPreviewHandlerFactory))]
    [Export(XLTX, typeof(IPreviewHandlerFactory))]
    [Export(XLTM, typeof(IPreviewHandlerFactory))]
    [Export(XLT, typeof(IPreviewHandlerFactory))]
    public class ExcelPreviewHandlerFactory : IPreviewHandlerFactory 
    {
        public const string XLSX = "XLSX";
        public const string XLSM = "XLSM";
        public const string XLSB = "XLSB";
        public const string XLS = "XLS";
        public const string XLTX = "XLTX";
        public const string XLTM = "XLTM";
        public const string XLT = "XLT";

        public const string ClassID = "F0D0A836-AC13-4DAF-A877-291A210FC854";

        public Guid ID
        {
            get
            {
                return new Guid(ClassID);
            }
        }

        public IPreviewHandler CreateHandler()
        {
            return new ExcelPreviewHandlerControl();
        }
    }
}
