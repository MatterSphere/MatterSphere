using System;
using System.Globalization;
using FWBS.OMS;

namespace Fwbs.Oms.DialogInterceptor
{
    using Fwbs.WinFinder;

    public class SaveAsDialog : Dialog, System.Windows.Forms.IWin32Window
    {
        #region Constructors

        public SaveAsDialog(Window win, DialogConfig config) : base(win, config)
        {
        }

        #endregion

        #region Properties

        protected virtual Window FileNameBox
        {
            get
            {
                Window w = null;
                if (Configuration.FileNameDialogId > -1)
                {
                    w = InternalWindow.GetDialogItem(Configuration.FileNameDialogId);
                }

                if (w == null)
                {
                    w = FindByClassHierarchy(Configuration.FileNameClass);
                }

                return w;
            }
        }

        public string Extension
        {
            get
            {
                return Configuration.FileExtension;
            }
        }

        public string FileName
        {
            get
            {
                Window box = FileNameBox;
                if (box == null)
                    throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("WNAMENOTFOUND", "Filename window cannot be found!", "").Text);
                return box.Text;
            }
            set
            {
                if (value == null)
                    value = String.Empty;
                else
                    value = System.IO.Path.ChangeExtension(value, Extension);

                Window box = FileNameBox;
                if (box == null)
                    throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("WNAMENOTFOUND", "Filename window cannot be found!", "").Text);

                // WORKAROUND: send WM_CHAR message to ensure that dialog will understand that file name has been changed.
                box.SendMessage(WindowMessage.Char, new IntPtr(' '), new IntPtr(1));
                box.Text = value;
                //Make sure it is defo set as quick succession call to FileName and Ok seems to 
                //not correctly registers.
                box.Text = value;

            }
        }

        #endregion

        public override void Ok()
        {
            try
            {
                string filename = FileName;
                System.IO.FileInfo file = new System.IO.FileInfo(filename);
                if (!file.Directory.Exists)
                    System.IO.Directory.CreateDirectory(file.Directory.FullName);

                base.Ok();
                
                int ctr = 0;
                int maxTimeOut = FWBS.Common.ConvertDef.ToInt32(new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, "", @"DialogInterceptor", "MaxTimeOut").GetSetting(30), 30);
                System.Diagnostics.Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Maximum Time Out : {0} seconds", maxTimeOut), "OMSUtils");
                
                while (ctr < maxTimeOut * 2)
                {
                    System.Threading.Thread.Sleep(500);
                    if (System.IO.File.Exists(filename)) 
                        break;
                    System.Diagnostics.Debug.WriteLine("File does not currently exist", "OMSUtils");
                    ctr++;
                }
                
                if (!System.IO.File.Exists(filename))
                    throw new System.IO.FileNotFoundException(Session.CurrentSession.Resources.GetMessage("FILENOTSAVED", "The dialog interceptor could not save the file correctly", "").Text, filename);
            }
            catch
            {
                throw;
            }
        }


    }
}
