using System;
using System.Collections;

namespace FWBS.OMS.FileManagement.Addins
{
    using FWBS.OMS.Extensibility;

    public class Addin : FWBS.OMS.Extensibility.OMSExtensibility
	{
		#region Fields
        
		internal static Hashtable LoadedFMApps = Hashtable.Synchronized(new Hashtable());

		#endregion
        
		#region Cosntructors

		public Addin()
		{
		}

		#endregion

		#region OMSExtensibility

		public override void OnDisconnection()
		{
            UnloadApps();
		}

		public override void OnStartupComplete(out Type[] registeredTypes)
		{
			registeredTypes = new Type[5];
			registeredTypes[0] = typeof(FWBS.OMS.FileType);
			registeredTypes[1] = typeof(FWBS.OMS.OMSFile);
			registeredTypes[2] = typeof(FWBS.OMS.Milestones_OMS2K);
			registeredTypes[3] = typeof(FWBS.OMS.Task);
			registeredTypes[4] = typeof(FWBS.OMS.Tasks);

		}

		public override void OnBeforeShutdown()
		{
            UnloadApps();
		}

        public override object GetObjectExtender(Type type)
        {
            if (type == typeof(OMSFile))
                return new FileExtender();
        
            return null;
        }

		public override object GetObjectExtender(object obj)
		{
			if (obj is FWBS.OMS.FileType)
				return new FileTypeExtender((FileType)obj);
			else if (obj is FWBS.OMS.OMSFile)
				return FMApplication.GetFileExtender((OMSFile)obj);
			else if (obj is Milestones_OMS2K)
				return null;
			else if (obj is FWBS.OMS.Task)
				return null;
			else if (obj is FWBS.OMS.Tasks)
				return null;
			else
				return null;
		}

		public override void OnObjectEvent(Extensibility.ObjectEventArgs e)
		{

			if (e.Sender is FWBS.OMS.FileType)
			{
				FileType ft = (FWBS.OMS.FileType)e.Sender;
				if (e.Event == Extensibility.ObjectEvent.Loaded)
					FMApplication.GetApplication(ft);
			}
			else if (e.Sender is FWBS.OMS.OMSFile)
			{
				OMSFile f = (FWBS.OMS.OMSFile)e.Sender;
				FMApplicationInstance wf = FMApplication.GetApplicationInstance(f);
                if (wf == null)
                    return;
				wf.Initialise(f);
				wf.ExecuteFileEvent(f, e);
			}
			else if (e.Sender is Milestones_OMS2K)
			{
                if (e.Event != ObjectEvent.Deleting && e.Event != ObjectEvent.Deleted)
                {
                    Milestones_OMS2K plan = (Milestones_OMS2K)e.Sender;
                    OMSFile f = plan.OMSFile;
                    FMApplicationInstance wf = FMApplication.GetApplicationInstance(f);
                    if (wf == null)
                        return;
                    wf.Initialise(f);
                    wf.InitialiseMilestonePlan(plan);
                    wf.ExecuteMilestoneEvent(wf.CurrentPlan, e);
                }
			}
			else if (e.Sender is FWBS.OMS.Task)
			{
				Task tsk = (FWBS.OMS.Task) e.Sender;
                if (tsk.IsNew == false)
                {
                    OMSFile f = tsk.File;
                    FMApplicationInstance wf = FMApplication.GetApplicationInstance(f);
                    if (wf == null)
                        return;
                    wf.Initialise(f);
                    wf.InitialiseTask(tsk);
                    wf.ExecuteTaskEvent(wf.CurrentTask, e);
                }
			}
			else if (e.Sender is FWBS.OMS.Tasks)
			{
			}
				return;
		}

		#endregion

        private void UnloadApps()
        {
            foreach (FMApplication app in LoadedFMApps.Values)
            {
                if (app != null)
                {
                    app.Dispose();
                }
            } 

            LoadedFMApps.Clear();
        }

        internal static  void RefreshApplications()
        {
            foreach (FMApplication app in Addin.LoadedFMApps.Values)
            {
                if (app != null)
                {
                    foreach (FMApplicationInstance appinst in app.Instances.Values)
                    {
                        if (appinst.CurrentFile != null)
                        {
                            appinst.CurrentFile.Tasks.Refresh();

                        }

                        if (appinst.CurrentPlan != null)
                            appinst.CurrentPlan.Refresh();
                    }
                }
            }
        }

	}

}
