using System;
using System.CodeDom;
using System.Collections;
using System.Reflection;
using FWBS.OMS.Script;
using MBox = System.Windows.Forms.MessageBox;
using swf = System.Windows.Forms;

namespace FWBS.OMS.FileManagement
{


    /// <summary>
    /// A base script type for the FileManagement application.
    /// </summary>
    public class ApplicationScriptType : Script.ScriptType
    {

        #region Fields

        private FMApplicationInstance application = null;
        private FMApplication parentapp = null;
        private static int count;
        private static object lockobj = new object();
        private ContextFactory _contextfactory = new ContextFactory();

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Constructs a new session scriptlet.
        /// </summary>
        public ApplicationScriptType()
        {
            lock (lockobj)
            {
                if (this.GetType() != typeof(ApplicationScriptType))
                {
                    count++;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    application = null;
                    parentapp = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        ~ApplicationScriptType()
        {
            lock (lockobj)
            {
                if (this.GetType() != typeof(ApplicationScriptType))
                {
                    count--;
                }
            }


            Dispose(false);
        }

        #endregion

        #region ScripType Implementation

        public override object CurrentObj
        {
            get
            {
                return application;
            }
        }

        /// <summary>
        /// Gets all the code namespace imports.
        /// </summary>
        protected override CodeNamespaceImport[] NamespaceImports
        {
            get
            {
                CodeNamespaceImport[] ns = new CodeNamespaceImport[]
                {
				    new CodeNamespaceImport("System"),
				    new CodeNamespaceImport("System.Data"),
                    new CodeNamespaceImport("System.Xml"),
				    new CodeNamespaceImport("System.Windows.Forms"),
				    new CodeNamespaceImport("FWBS.Common.UI"),
				    new CodeNamespaceImport("FWBS.Common"),
				    new CodeNamespaceImport("FWBS.OMS"),
				    new CodeNamespaceImport("FWBS.OMS.UI.Windows"),
				    new CodeNamespaceImport("FWBS.OMS.FileManagement")
                };
                return ns;
            }
        }

        /// <summary>
        /// Gets a string array of assembly references to include.
        /// </summary>
        protected override string[] AssemblyReferences
        {
            get
            {
                return new string[] {"FWBS.OMS.FileManagement" };
            }
        }

        public CodeMemberMethod[] GetEvents()
        {
            return base.Methods;
        }

        public override CodeMemberMethod[] Methods
        {
            get
            {

                CodeMemberMethod[] meths = base.Methods;
                System.Collections.Hashtable actions = new System.Collections.Hashtable();

                foreach (CodeMemberMethod m in meths)
                {
                    if (actions.Contains(m.Name) == false)
                    {
                        actions.Add(m.Name, m);
                    }
                }

                string[] names = parentapp.GetActionMethodNames();

                foreach (string name in names)
                {
                    CodeMemberMethod m = new CodeMemberMethod();
                    m.Name = Convert.ToString(name);
                    m.ReturnType = new CodeTypeReference(typeof(void));
					m.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Action), "action"));

                    if (actions.Contains(name) == false)
                    {
                        actions.Add(name, m);
                    }
                }

                CodeMemberMethod[] list = new CodeMemberMethod[actions.Count];
                actions.Values.CopyTo(list, 0);

                return list;

            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Intialises the scriptlet with a FileManagement application object.
        /// </summary>
        public void SetAppObject(FMApplication app)
        {
            if (app == null)
                throw new ArgumentNullException("app", "A FileManagement application scriptlet needs an application configuration object.");
            parentapp = app;
        }

        public void SetAppObject(FMApplicationInstance app)
        {
            if (app == null)
                throw new ArgumentNullException("app", "A FileManagement application scriptlet needs an application configuration object.");
            application = app;
            parentapp = app.Parent;
        }

        #endregion

        #region Script Wrapping Methods

        protected internal void Execute(Action action)
        {
            switch (action.ActionType)
            {
                case ActionType.File:
                    ExecuteFileAction(action);
                    break;
                case ActionType.Task:
                    ExecuteTaskAction(action);
                    break;
            }
        }

        protected internal ActionVisibility IsActionAvailable(Action action)
        {
            switch (action.ActionType)
            {
                case ActionType.File:
                    return IsFileActionAvailable(action);
                case ActionType.Task:
                    return IsTaskActionAvailable(action);
            }
            return ActionVisibility.Unspecified;
        }

        protected bool CanCompleteStage(byte stage)
        {
            ArrayList reasons = new ArrayList();
            return CanCompleteStage(stage, reasons);
        }

        protected bool CanCompleteStage(byte stage, ArrayList reasons)
        {
            return Application.CanCompleteStage(stage, reasons);
        }

        protected bool CanComplete(Milestones.MilestoneStage stage)
        {
            ArrayList reasons = new ArrayList();
            return CanComplete(stage, reasons);
        }

        protected internal bool CanComplete(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
        {
            try
            {
                return CanCompleteStage(stage, reasons);
            }
            catch (Exception ex)
            {
                reasons.Add(ex.Message);
                return false;
            }
        }

        protected bool CanUnCompleteStage(byte stage)
        {
            ArrayList reasons = new ArrayList();
            return CanUnCompleteStage(stage, reasons);
        }

        protected bool CanUnCompleteStage(byte stage, ArrayList reasons)
        {
            return Application.CanUnCompleteStage(stage, reasons);
        }

        protected bool CanUnComplete(Milestones.MilestoneStage stage)
        {
            ArrayList reasons = new ArrayList();
            return CanUnComplete(stage, reasons);
        }

        protected internal bool CanUnComplete(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
        {
            try
            {
                return CanUnCompleteStage(stage, reasons);
            }
            catch (Exception ex)
            {
                reasons.Add(ex.Message);
                return false;
            }
        }

        protected bool CanCompleteTask(string filter)
        {
            ArrayList reasons = new ArrayList();
            return CanCompleteTask(filter, reasons);
        }

        protected bool CanCompleteTask(string filter, ArrayList reasons)
        {
            return Application.CanCompleteTask(filter, reasons);
        }


        protected bool CanComplete(Milestones.Task task)
        {
            ArrayList reasons = new ArrayList();
            return CanComplete(task, reasons);
        }

        protected internal bool CanComplete(Milestones.Task task, System.Collections.ArrayList reasons)
        {
            try
            {
                return CanCompleteTask(task, reasons);
            }
            catch (Exception ex)
            {
                reasons.Add(ex.Message);
                return false;
            }
        }

        protected bool CanUnCompleteTask(string filter)
        {
            ArrayList reasons = new ArrayList();
            return CanUnCompleteTask(filter, reasons);
        }

        protected bool CanUnCompleteTask(string filter, ArrayList reasons)
        {
            return Application.CanUnCompleteTask(filter, reasons);
        }


        protected bool CanUnComplete(Milestones.Task task)
        {
            ArrayList reasons = new ArrayList();
            return CanUnComplete(task, reasons);
        }
        protected internal bool CanUnComplete(Milestones.Task task, System.Collections.ArrayList reasons)
        {
            try
            {
                return CanUnCompleteTask(task, reasons);
            }
            catch (Exception ex)
            {
                reasons.Add(ex.Message);
                return false;
            }
        }

		/// <summary>
		/// Retrieves the MethodInfo for an Action
		/// </summary>
		/// <param name="action"></param>
		/// <returns>The MethodInfo for the <paramref name="action"/> or null if the method doesn't exist or the parameters aren't correct</returns>
		private MethodInfo GetMethod(Action action)
		{
			MethodInfo meth = null;

			if (action.Method.Length == 0)
				return null;

			meth = GetType().GetMethod(action.Method, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase, null, new[] { typeof(Action) }, null);

			if (meth == null)
				meth = GetType().GetMethod(action.Method, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			return meth;
		}

		private object Invoke(Action action, MethodInfo method)
		{
			if (action == null)
                throw new ArgumentNullException("action");

			if (method == null)
                throw new ArgumentNullException("method");

			ParameterInfo[] pi = method.GetParameters();

			if (pi.Length == 1 && pi[0].ParameterType == typeof(Action))
				return method.Invoke(this, new object[] { action });

			if (pi.Length == 0)
				return method.Invoke(this, null);

			throw new OMSException2("FMWRONGMETHSIG", "Invalid Method Signature. The actions method must be a parameterless method or a method with 1 parameter that is an Action");
		}

        #endregion

        #region Script Virtual Methods

        /// <summary>
        /// Executes a file level action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        [ScriptMethodOverridable()]
        public virtual void ExecuteFileAction(Action action)
        {
			MethodInfo meth = GetMethod(action);
            if (meth == null)
            {
                FWBS.OMS.UI.Windows.MessageBox.ShowInformation("FMNOTIMPLEMENTD", "FileManagement command not implemented!");
            }
            else
            {
				Invoke(action, meth);
            }
        }

        /// <summary>
        /// Executes a task level action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        [ScriptMethodOverridable()]
        public virtual void ExecuteTaskAction(Action action)
        {
			MethodInfo meth = GetMethod(action);
            
            if (meth == null)
            {
                FWBS.OMS.UI.Windows.MessageBox.ShowInformation("FMNOTIMPLEMENTD", "FileManagement command not implemented!");
            }
            else
            {
				Invoke(action, meth);
            }
        }

        /// <summary>
        /// Checks to see if the specified file action is available.
        /// </summary>
        /// <param name="action"></param>
        [ScriptMethodOverridable()]
        public virtual ActionVisibility IsFileActionAvailable(Action action)
        {
            return ActionVisibility.Unspecified;
        }


        /// <summary>
        /// Checks to see if the specified action is available.
        /// </summary>
        /// <param name="action"></param>
        [ScriptMethodOverridable()]
        public virtual ActionVisibility IsTaskActionAvailable(Action action)
        {
            return ActionVisibility.Unspecified;
        }

        /// <summary>
        /// A method that gets called file event has been executed.
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual void OnFileEvent(Extensibility.ObjectEventArgs e, ref bool Handled)
        {
        }

        /// <summary>
        /// A method that gets called when a milestone event has been executed.
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual void OnMilestoneEvent(Extensibility.ObjectEventArgs e, ref bool Handled)
        {
        }

        /// <summary>
        /// A method that gets called when a task event has been executed.
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual void OnTaskEvent(Extensibility.ObjectEventArgs e, ref bool Handled)
        {
        }

        /// <summary>
        /// A method that gets called when the system needs to know whether a milestone stage can be completed or not.
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual bool CanCompleteStage(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
        {
            return true;
        }
        [ScriptMethodOverridable()]
        public virtual bool CanCompleteStageUI(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
        {
            //This will occur on all scripts unless overidden.
            if (stage.Tasks.AreCompleted)
                return true;
            else
            {
                reasons.Add(CurrentSession.Resources.GetMessage("FMSTAGECOMP", "The stage cannot be completed as all tasks have to be completed first.", "").Text);
                return false;
            }
        }

        /// <summary>
        /// A method that gets called when the system needs to know whether a manual milestone task can be added.
        /// This is only used for UI purposes.  Manual tasks can still be added through code.
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual bool CanAddManualTask()
        {
            return true;
        }

        /// <summary>
        /// A method that gets called when the system needs to know whether a task can be completed or not.
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual bool CanCompleteTask(Milestones.Task task, System.Collections.ArrayList reasons)
        {
            return true;
        }
        [ScriptMethodOverridable()]
        public virtual bool CanCompleteTaskUI(Milestones.Task task, System.Collections.ArrayList reasons)
        {
            //This will occur on all scripts unless overidden.
            if (task.Manual)
                return true;
            else
            {
                reasons.Add(CurrentSession.Resources.GetMessage("MANUALTASKTICK", "The specified task cannot be completed manually, an automated action must complete it.", "").Text);
                return false;
            }
        }

        /// <summary>
        /// A method that gets called when the system needs to know whether a milestone stage can be uncompleted or not.
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual bool CanUnCompleteStage(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
        {
            return true;
        }
        [ScriptMethodOverridable()]
        public virtual bool CanUnCompleteStageUI(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
        {
            //This will occur on all scripts unless overidden.
            if (stage.Tasks.AreIncomplete)
                return true;
            else
            {
                reasons.Add(CurrentSession.Resources.GetMessage("FMSTAGEUNCOMP", "The stage cannot be uncompleted as all tasks have to be uncompleted first.", "").Text);
                return false;
            }
        }

		/// <summary>
		/// A method that gets called when the system needs to know whether a milestone stage due date can be altered.
		/// </summary>
		[ScriptMethodOverridable()]
		[VersionConditional("5.0.0.0")]
		public virtual bool CanChangeStageDueUI(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
		{
			return true;
		}

		[ScriptMethodOverridable()]
		[VersionConditional("5.0.0.0")]
		public virtual bool CanChangeStageDue(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
		{
			return true;
		}
		
		/// <summary>
		/// A method that gets called after the Milestone Plan's static task have been created
		/// </summary>
		[ScriptMethodOverridable()]
		[VersionConditional("5.0.0.0")]
		public virtual void OnMilestonePlanTasksApplied()
		{			
		}
		
        /// <summary>
        /// A method that gets called when the system needs to know whether a task can be uncompleted or not.
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual bool CanUnCompleteTask(Milestones.Task task, System.Collections.ArrayList reasons)
        {
            return true;
        }
        [ScriptMethodOverridable()]
        public virtual bool CanUnCompleteTaskUI(Milestones.Task task, System.Collections.ArrayList reasons)
        {
            //This will occur on all scripts unless overidden.
            if (task.Manual)
                return true;
            else
            {
                reasons.Add(CurrentSession.Resources.GetMessage("MANUALTSKUNTICK", "The specified task cannot be uncompleted manually, an automated action must uncomplete it.", "").Text);
                return false;
            }
        }

        [ScriptMethodOverridable()]
        public virtual void OnMilestoneResetting(ref bool cancel, System.Collections.ArrayList reasons)
        {
        }

        [ScriptMethodOverridable()]
        public virtual void OnMilestoneReset()
        {
        }

        [ScriptMethodOverridable()]
        public virtual void OnMilestoneRemoving(ref bool cancel, System.Collections.ArrayList reasons)
        {
        }

        [ScriptMethodOverridable()]
        public virtual void OnMilestoneRemoved()
        {
        }

        [ScriptMethodOverridable()]
        public virtual void OnManualTaskAdded(Milestones.Task task)
        {
        }
	
        #endregion

        #region Properties

        protected FMApplicationInstance Application
        {
            get
            {
                return application;
            }
        }

        protected OMSFile CurrentFile
        {
            get
            {
                return application.CurrentFile;
            }
        }

        protected Milestones.Task CurrentTask
        {
            get
            {
                return application.CurrentTask;
            }
        }

        protected Milestones.MilestonePlan CurrentPlan
        {
            get
            {
                return application.CurrentPlan;
            }
        }

        private IContext _context = null;

        public override IContext Context
        {
            get
            {
                if (_context == null)
                    _context = _contextfactory.CreateContext(CurrentFile, null);
                return _context;
            }
        }

        #endregion

        #region Helper Methods

        protected void RestoreTask(params string[] filters)
        {
            Application.RestoreTask(filters);
        }

        protected void RestoreGroupedTasks(params string[] groups)
        {
            Application.RestoreGroupedTasks(groups);
        }

        protected bool IsStageCompleted(params byte[] stages)
        {
            return Application.IsStageCompleted(stages);
        }

        protected bool AreNextStageTasksComplete()
        {
            return AreStageTasksCompleted(CurrentPlan.NextStage.StageNumber);
        }

        protected bool AreStageTasksCompleted(params byte[] stages)
        {
            return Application.AreStageTasksCompleted(stages);
        }

        protected bool AreGroupedTasksCompleted(params string[] groups)
        {
            return Application.AreGroupedTasksCompleted(groups);
        }

        protected bool IsTaskCompleted(params string[] filters)
        {
            return Application.IsTaskCompleted(filters);
        }


        protected void CompleteNextStage()
        {
            CompleteStage(CurrentPlan.NextStage.StageNumber);
        }

        protected void CompleteStage(params byte[] stages)
        {
            CompleteStage(ErrorOption.Fail, stages);
        }

        protected void CompleteStage(ErrorOption option, params byte[] stages)
        {
            Application.CompleteStage(option, stages);
        }

        protected void CompleteStageTasks(byte stage)
        {
            CompleteStageTasks(stage, ErrorOption.Fail);
        }

        protected void CompleteStageTasks(byte stage, ErrorOption option)
        {
            Application.CompleteStageTasks(stage, option);
        }

        protected void CompleteTask(params string[] filters)
        {
            CompleteTask(ErrorOption.Fail, filters);
        }

        protected void CompleteTask(ErrorOption option, params string[] filters)
        {
            Application.CompleteTask(option, filters);
        }

        protected void CompleteGroupedTasks(params string[] groups)
        {
            CompleteGroupedTasks(ErrorOption.Fail, groups);
        }

        protected void CompleteGroupedTasks(ErrorOption option, params string[] groups)
        {
            Application.CompleteGroupedTasks(option, groups);
        }

        protected void UnCompleteStage(params byte[] stages)
        {
            UnCompleteStage(ErrorOption.Fail, stages);
        }

        protected void UnCompleteStage(ErrorOption option, params byte[] stages)
        {
            Application.UnCompleteStage(option, stages);
        }

        protected void UnCompleteStageTasks(byte stage)
        {
            UnCompleteStageTasks(stage, ErrorOption.Fail);
        }

        protected void UnCompleteStageTasks(byte stage, ErrorOption option)
        {
            Application.UnCompleteStageTasks(stage, option);
        }

        protected void UnCompleteTask(params string[] filters)
        {
            UnCompleteTask(ErrorOption.Fail, filters);
        }

        protected void UnCompleteTask(ErrorOption option, params string[] filters)
        {
            Application.UnCompleteTask(option, filters);
        }

        protected void UnCompleteGroupedTasks(params string[] groups)
        {
            UnCompleteGroupedTasks(ErrorOption.Fail, groups);
        }

        protected void UnCompleteGroupedTasks(ErrorOption option, params string[] groups)
        {
            Application.UnCompleteGroupedTasks(option, groups);
        }


        protected void EmailTo(string from, string to, string subject, string body)
        {
            Session.CurrentSession.SendMail(from, to, subject, body);
        }

        protected void AddFileNote(string note)
        {
            CurrentFile.AppendNoteText(NoteAppendingLocation.Beginning, note);
        }

        protected void AddFileEvent(string type, string description, string extraInfo)
        {
            CurrentFile.AddEvent(type, description, extraInfo);
        }

        protected ReturnValue ShowTaskWizard()
        {
            Milestones.Task task;
            return ShowTaskWizard(out task);
        }

        protected ReturnValue ShowTaskWizard(out Milestones.Task task)
        {
            return Application.ShowTaskWizard(out task);
        }

        protected ReturnValue RunWizard(string code, Common.KeyValueCollection pars)
        {
            object data;
            return RunWizard(code, pars, out data);
        }

        protected ReturnValue RunWizard(string code, Common.KeyValueCollection pars, out object data)
        {
            return RunWizard(code, pars, CurrentFile, out data);
        }

        protected ReturnValue RunWizard(string code, Common.KeyValueCollection pars, FWBS.OMS.Interfaces.IEnquiryCompatible bindingObject, out object data)
        {
            if (bindingObject == null)
                throw new ArgumentNullException("bindingObject");

            FWBS.OMS.EnquiryEngine.Enquiry enq = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiry(code, bindingObject.Parent, bindingObject, pars);
            FWBS.OMS.UI.Windows.Services.Wizards wiz = new FWBS.OMS.UI.Windows.Services.Wizards(enq);
            data = wiz.Show();

            if (data == null)
                return ReturnValue.Failed;
            else
                return ReturnValue.Success;
        }

        protected Milestones.Task AddTaskToNextStage(string description, string group, string filter)
        {
            return AddTask(description, CurrentPlan.NextStage.StageNumber, group, filter, Convert.ToDateTime(CurrentFile.MilestonePlan.NextStageDueDate));
        }

        protected Milestones.Task AddTaskToNextStage(string description, string group, string filter, DateTime due)
        {
            return AddTask(description, CurrentPlan.NextStage.StageNumber, group, filter, due);
        }

        protected Milestones.Task AddTask(string description, byte stage, string group, string filter)
        {
            return Application.AddTask(description, stage, group, filter);
        }

        protected Milestones.Task AddTask(string description, byte stage, string group, string filter, DateTime due)
        {
            return Application.AddTask(null, description, stage, group, filter, due);
        }

        protected Milestones.Task AddTask(string team, string description, byte stage, string group, string filter)
        {
            return Application.AddTask(team, description, stage, group, filter, null);
        }

        protected Milestones.Task AddTask(string team, string description, byte stage, string group, string filter, DateTime due)
        {
            return Application.AddTask(team, description, stage, group, filter, due);
        }

        protected void AssignTaskToUser(Milestones.Task task, string initials)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            if (String.IsNullOrEmpty(initials))
                throw new ArgumentNullException("initials");

            User user = User.GetUser(initials);
            task.AssignedTo = user;
        }

        protected void AssignTaskToUser(Milestones.Task task, User user)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            if (user == null)
                throw new ArgumentNullException("user");

            task.AssignedTo = user;
        }

        protected void AssignTaskToUser(string filter, string initials)
        {
            Application.AssignTaskToUser(initials, filter);
        }

        protected void AssignTaskToUser(string filter, User user)
        {
            Application.AssignTaskToUser(user, filter);
        }

        protected void AssignTaskToTeam(Milestones.Task task, Team team)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            if (team == null)
                throw new ArgumentNullException("team");

            task.AssignedTeam = team;
        }

        protected void AssignTaskToTeam(Milestones.Task task, string team)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            if (String.IsNullOrEmpty(team))
                throw new ArgumentNullException("team");

            Team t = Team.GetTeam(team);
            task.AssignedTeam = t;
        }

        protected void AssignTaskToTeam(string filter, Team team)
        {
            Application.AssignTaskToTeam(team, filter);
        }

        protected void AssignTaskToTeam(string filter, string team)
        {
            Application.AssignTaskToTeam(team, filter);
        }

        protected void RemoveGroupedTasks(params string[] groups)
        {
            Application.RemoveGroupedTasks(groups);
        }

        protected void RemoveTask(params string[] filter)
        {
            Application.RemoveTask(filter);
        }

        protected void UpdatePlan()
        {
            // Check there is a Current Plan before trying to update
            if (CurrentPlan != null)
                CurrentPlan.Update();
        }

        protected void WriteLetter()
        {
            //Create a letter but do not save quickly
            FWBS.OMS.UI.Windows.Services.TemplateStart(null, "LETTERHEAD", CurrentFile.DefaultAssociate);
        }
        protected void WriteLetterTo(string contType, string assocFormat)
        {
            //Create a letter but do not save quickly
            FWBS.OMS.UI.Windows.Services.TemplateStart(null, "LETTERHEAD", CurrentFile.GetBestFitAssociate(contType, assocFormat));
        }
        protected void CreateDocument(string precedent)
        {
            //Create a document, but do not save quickly
            CreateDocument(precedent, false);
        }
        protected void CreateDocument(string precedent, bool quickSave)
        {

            //Construct the job to do from the precedent
            PrecedentJob job = new PrecedentJob(Precedent.GetPrecedent(precedent, CurrentFile.DefaultAssociate));

            //Assign the Associate to the default associate
            job.Associate = CurrentFile.DefaultAssociate;

            if (quickSave)
                job.SaveMode = PrecSaveMode.Quick;

            FWBS.OMS.UI.Windows.Services.ProcessJob(null, job);
        }

        protected void CreateDocumentTo(string precedent, string contType, string assocFormat)
        {
            CreateDocumentTo(precedent, contType, assocFormat, false);
        }

        protected void CreateDocumentTo(string precedent, string contType, string assocFormat, bool quickSave)
        {
            Associate assoc = CurrentFile.GetBestFitAssociate(contType, assocFormat);

            //Construct the job to do from the precedent
            PrecedentJob job = new PrecedentJob(Precedent.GetPrecedent(precedent, assoc));

            //Assign the Associate to the default associate
            job.Associate = assoc;

            if (quickSave)
            {
                job.SaveMode = PrecSaveMode.Quick;
                job.PrintMode = PrecPrintMode.Print;
            }

            FWBS.OMS.UI.Windows.Services.ProcessJob(null, job);
        }

        protected void MaximiseWindow()
        {
            if (FWBS.OMS.UI.Windows.Services.MainWindow != null)
            {
                foreach (System.Windows.Forms.Form frm in FWBS.OMS.UI.Windows.Services.MainWindow.OwnedForms)
                {
                    frm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                }
            }
        }
        protected void MinimiseWindow()
        {
            if (FWBS.OMS.UI.Windows.Services.MainWindow != null)
            {
                foreach (System.Windows.Forms.Form frm in FWBS.OMS.UI.Windows.Services.MainWindow.OwnedForms)
                {
                    frm.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                }
            }
        }
        protected void RestoreWindow()
        {
            if (FWBS.OMS.UI.Windows.Services.MainWindow != null)
            {
                foreach (System.Windows.Forms.Form frm in FWBS.OMS.UI.Windows.Services.MainWindow.OwnedForms)
                {
                    frm.WindowState = System.Windows.Forms.FormWindowState.Normal;
                }
            }
        }

        protected void MsgBox(string message, string caption)
        {
            MBox.Show(message, caption, swf.MessageBoxButtons.OK, swf.MessageBoxIcon.Information);
        }

        protected swf.DialogResult MsgBoxYesNo(string message, string caption)
        {
            return MBox.Show(message, caption, swf.MessageBoxButtons.YesNo, swf.MessageBoxIcon.Question, swf.MessageBoxDefaultButton.Button2);
        }

        protected swf.DialogResult MsgBoxYesNoCancel(string message, string caption)
        {
            return MBox.Show(message, caption, swf.MessageBoxButtons.YesNoCancel, swf.MessageBoxIcon.Question, swf.MessageBoxDefaultButton.Button2);
        }

        protected object GetExtData(string extCode, string fldName)
        {
            return CurrentFile.ExtendedData[extCode].GetExtendedData(fldName);
        }

        protected void SetExtData(string extCode, string fldName, object value)
        {
            CurrentFile.ExtendedData[extCode].SetExtendedData(fldName, value);
        }

        #endregion


    }
}
