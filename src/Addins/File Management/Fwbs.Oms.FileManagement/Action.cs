using System;

namespace FWBS.OMS.FileManagement
{
	public sealed class Action
	{
		#region Fields

        private Configuration.ActionConfig _actionConfiguration = null;

        public Configuration.ActionConfig ActionConfiguration
        {
            get { return _actionConfiguration; }
            internal set { _actionConfiguration = value; }
        }
		internal ActionType ActionType = ActionType.File;
		internal Milestones.MilestoneStage stage;
		internal Milestones.Task task;

		#endregion

		#region Constructors


		internal Action(Configuration.ActionConfig configuration, ActionType type, Milestones.MilestoneStage stage = null, Milestones.Task task = null)
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			ActionType = type;
			ActionConfiguration = configuration;
			this.stage = stage;
			this.task = task;
		}

		#endregion

		#region Properties

		public string Code
		{
			get
			{
				return ActionConfiguration.Code;
			}
		}

		public string Description
		{
			get
			{
				return ActionConfiguration.Description;
			}
		}

		public string Method
		{
			get
			{
				return ActionConfiguration.Method;
			}
		}

		/// <summary>
		/// When the action is being Executed this will contain either
		/// The OMSFile's current stage if it is a Matter based Action
		/// or The Stage that the action belongs to if it's a Stage based Action
		/// It will be null for a Task based action
		/// </summary>
		public Milestones.MilestoneStage Stage
		{
			get
			{
				return stage;
			}
		}

		/// <summary>
		/// When the action is being Executed this will contain the Task that the action was executed from
		/// or Null if it is a Matter or Stage based Action
		/// </summary>
		public Milestones.Task Task
		{
			get
			{
				return task;
			}
		}
		#endregion

	}

	internal enum ActionType
	{
		File,
		Task
	}
}
