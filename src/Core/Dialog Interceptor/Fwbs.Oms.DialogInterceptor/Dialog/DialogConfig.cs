
namespace Fwbs.Oms.DialogInterceptor
{
    public sealed class DialogConfig
    {
        private string dialogType;
        public string DialogType
        {
            get { return dialogType; }
            internal set { dialogType = value; }
        }

        private string windowClass;
        public string WindowClass
        {
            get { return windowClass; }
            internal set { windowClass = value; }
        }

        private string windowTitle;
        public string WindowTitle
        {
            get { return windowTitle; }
            internal set { windowTitle = value; }
        }

        private int cancelDialogId = -1;
        public int CancelDialogId
        {
            get { return cancelDialogId; }
            internal set { cancelDialogId = value; }
        }
        private int okDialogId = -1;
        public int OkDialogId
        {
            get { return okDialogId; }
            internal set { okDialogId = value; }
        }
        private int fileNameDialogId = -1;
        public int FileNameDialogId
        {
            get { return fileNameDialogId; }
            internal set { fileNameDialogId = value; }
        }


        private string cancelButtonClass;
        public string CancelButtonClass
        {
            get { return cancelButtonClass; }
            internal set { cancelButtonClass = value; }
        }

        private string cancelButtonText;
        public string CancelButtonText
        {
            get { return cancelButtonText; }
            internal set { cancelButtonText = value; }
        }

        private string okButtonClass;
        public string OkButtonClass
        {
            get { return okButtonClass; }
            internal set { okButtonClass = value; }
        }

        private string okButtonText;
        public string OkButtonText
        {
            get { return okButtonText; }
            internal set { okButtonText = value; }
        }

        private string fileNameClass;
        public string FileNameClass
        {
            get { return fileNameClass; }
            internal set { fileNameClass = value; }
        }

        private string fileExtension;
        public string FileExtension
        {
            get { return fileExtension; }
            internal set { fileExtension = value; }
        }
    }
}
