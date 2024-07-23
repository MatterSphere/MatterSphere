using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.Design.Import;
using FWBS.OMS.Design.Package;

namespace FWBS.OMS.UI.Windows.Admin
{
    public class PackageDeploy
    {
        public event ProgressEventHandler InProgress;
        public event CancelEventHandler ShowDialog;

        public Func<bool?, bool> IsCopyPackage { get; set; }
        public Func<bool?, string, bool?> ReadReadme { get; set; }
        public Func<bool?, bool> IsBackup { get; set; }
        public Action<IWin32Window, FWBS.OMS.Design.Import.Global> ActionAfterInstall { get; set; }
        public Action<int, bool> OutcomeMessage { get; set; }
        public Action<Exception> OutcomeError { get; set; }
        public Action Complete { get; set; }

        /// <summary>
        /// PackageDeploy class Constructor
        /// </summary>
        /// <param name="IsCopyPackage">Do we need to copy package for further modification on current environment?</param>
        /// <param name="ReadReadme">Read package's readme file </param>
        /// <param name="IsBackup">Do we need to backup package?</param>
        /// <param name="ActionAfterInstall">Do action after intalling package</param>
        /// <param name="OutcomeError">Pass Exception to delegate for further processing</param>
        /// <param name="OutcomeMessage">Action which shows message about result of deploying</param>
        /// <param name="Complete">Action which is called upon completion</param>
        public PackageDeploy(Func<bool?, bool> IsCopyPackage,
            Func<bool?, string, bool?> ReadReadme,
            Func<bool?, bool> IsBackup,
            Action<IWin32Window, FWBS.OMS.Design.Import.Global> ActionAfterInstall,
            Action<int, bool> OutcomeMessage,
            Action<Exception> OutcomeError,
            Action Complete)
        {
            this.IsCopyPackage = IsCopyPackage;
            this.ReadReadme = ReadReadme;
            this.IsBackup = IsBackup;
            this.ActionAfterInstall = ActionAfterInstall;
            this.OutcomeMessage = OutcomeMessage;
            this.OutcomeError = OutcomeError;
            this.Complete = Complete;
        }

        public SystemUpdateReturnStates ProcessManifest(IWin32Window owner, string manifestfilename, bool? defaultcopy, bool? defaultreadme, bool? defaultbackup, bool showoutcome, out object returntext, bool allowVersionWarning)
        {
            returntext = null;
            bool isPackageCopy = false;
            int failed = 0;

            try
            {
                System.IO.FileInfo importpak = new System.IO.FileInfo(manifestfilename);
                Environment.CurrentDirectory = importpak.Directory.FullName;
                
                isPackageCopy = IsCopyPackage(defaultcopy);

                bool? read = ReadReadme(defaultreadme, manifestfilename);

                if (read == null)
                {
                    return SystemUpdateReturnStates.Cancel;
                }

                var backup = IsBackup(defaultbackup);

                FWBS.OMS.Design.Import.Global _import = new FWBS.OMS.Design.Import.Global(owner);

                _import.ShowDialog += new CancelEventHandler(_import_ShowDialog);
                _import.Progress += new ProgressEventHandler(_import_Progress);

                failed = _import.Import(manifestfilename, backup, isPackageCopy, allowVersionWarning);

                if (String.IsNullOrEmpty(_import.TreeView.AfterInstall) == false && failed != -1)
                {
                    ActionAfterInstall(owner, _import);
                }

                if (_import.TreeView.DependentPackages != null && _import.TreeView.DependentPackages.Count > 0)
                {
                    Console.WriteLine("[Automation] - The package has one ore more dependencies.");

                    foreach (Dependent dependent in _import.TreeView.DependentPackages)
                    {
                        Console.WriteLine("[Automation] - Dependency - 'Desc: " + dependent.Package.Description + "'; Code:'" + dependent.Package.Code + "'");
                    }
                }

                if (!string.IsNullOrEmpty(_import.TreeView.PackageRequiredLicenses))
                {
                    Console.WriteLine("[Automation] - The package has the following licenses - '" + _import.TreeView.PackageRequiredLicenses + "'");
                }

                Session.CurrentSession.Resources.Refresh();
                Session.CurrentSession.ClearCache(true);

                if (showoutcome)
                {
                    OutcomeMessage(failed, backup);
                }
                else
                {
                    if (failed > -1)
                        returntext = FWBS.OMS.Session.CurrentSession.Resources.GetResource("IMPSUC2", "Import Successful... with %1% failed or cancelled", "", failed.ToString()).Text;
                }
            }
            catch (Exception ex)
            {
                if (showoutcome)
                    OutcomeError(ex);
                else
                    returntext = ex;

                return SystemUpdateReturnStates.Failed;
            }
            finally
            {
                Complete();
            }

            if (failed == -1)
                return SystemUpdateReturnStates.Cancel;
            else if (failed > 0)
                return SystemUpdateReturnStates.Failed;
            else
                return SystemUpdateReturnStates.Success;
        }

        private void _import_ShowDialog(object sender, CancelEventArgs e)
        {
            if (ShowDialog != null)
                ShowDialog(sender, e);
        }

        private void _import_Progress(object sender, FWBS.OMS.Design.Import.ProgressEventArgs e)
        {
            if (InProgress != null)
                InProgress(sender, e);
        }
    }
}
