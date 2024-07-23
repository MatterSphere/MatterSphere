using System.ComponentModel;
using FWBS.OMS.Design.Import;

namespace FWBS.OMS.UI.Windows.Admin
{
    public static class PackageDeployFactory
    {
        public static PackageDeploy Create(ProgressEventHandler progressDelegate, CancelEventHandler cancelDelegate)
        {
            PackageDeploy packageDeploy = new PackageDeploy(SystemUpdateDefaultActions.IsCopyPackage,
                SystemUpdateDefaultActions.ReadReadmeFile,
                SystemUpdateDefaultActions.IsBackup,
                SystemUpdateDefaultActions.ActionAfterInstall,
                SystemUpdateDefaultActions.OutcomeMessage,
                SystemUpdateDefaultActions.OutcomeError,
                SystemUpdateDefaultActions.Complete);

            packageDeploy.InProgress += progressDelegate;
            packageDeploy.ShowDialog += cancelDelegate;

            return packageDeploy;
        }

        public static PackageDeploy Create4Automation()
        {
            PackageDeploy packageDeploy = new PackageDeploy((b) => false,
                (b, s) => false,
                (b) => false,
                (w, g) => { },
                (f, b) => { },
                (e) => { },
                () => { });

            return packageDeploy;
        }
    }
}
