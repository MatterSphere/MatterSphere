using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public enum SystemUpdateReturnStates
    {
        Success,
        Cancel,
        Failed
    }

    public interface ISystemUpdate
    {
        PackageDeploy PackageDeployment { get; }
    }

    public static class SystemUpdateDefaultActions
    {
        public static bool IsCopyPackage(bool? defaultcopy)
        {
            bool Copy = false;

            if (defaultcopy == null)
            {
                DialogResult result3 = MessageBox.ShowYesNoQuestion("COPYORIMP1",
                    "Copy the Package. Click Yes to Copy the Package or No and it will Install it");
                if (result3 == DialogResult.Yes) Copy = true;
            }
            else
            {
                Copy = (bool)defaultcopy;
            }

            return Copy;
        }

        public static bool? ReadReadmeFile(bool? defaultreadme, string manifestfilename)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(manifestfilename.ToLower().Replace(".manifest.xml", "") + "_readme.doc");

            if (defaultreadme == null)
            {
                if (file.Exists)
                {
                    DialogResult result = MessageBox.ShowYesNoCancel("IMPRMYESNO", "Do you wish to View the Package Readme?");
                    if (result == DialogResult.Yes)
                    {
                        Services.ProcessStart(file.FullName, String.Empty, InputValidation.ValidateEmptyInput);
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return null;
                    }
                }
            }
            else if (defaultreadme == true)
            {
                if (file.Exists)
                {
                    Services.ProcessStart(file.FullName, String.Empty, InputValidation.ValidateEmptyInput);
                    return true;
                }
            }

            return false;
        }

        public static bool IsBackup(bool? defaultbackup)
        {
            bool backup;
            if (defaultbackup == null)
                backup = (MessageBox.ShowYesNoQuestion("IMPBACK", "Do you wish to make a Backup ?") == DialogResult.Yes);
            else
                backup = (bool)defaultbackup;
            return backup;
        }

        public static void ActionAfterInstall(IWin32Window owner, OMS.Design.Import.Global _import)
        {
          
        }

        public static void OutcomeMessage(int failed, bool backup)
        {
            if (failed > -1)
                MessageBox.ShowInformation(FWBS.OMS.Session.CurrentSession.Resources.GetResource("IMPSUC2", "Import Successful... with %1% failed or cancelled", "", failed.ToString()).Text);
            if (backup)
                MessageBox.ShowInformation("IMPBACLOC", "Please note the backup location can be found in your My Documents\\My Package Backups Folder");
        }

        public static void OutcomeError(Exception e)
        {
            ErrorBox.Show(e);
        }

        public static void Complete()
        {

        }
    }

}
