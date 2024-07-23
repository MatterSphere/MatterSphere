using System;

namespace FWBS.OMS.FileManagement.Milestones
{
	public enum StageData
	{
		AchievedDate,
		DueDate,
		CalculatedDays
	}

	public delegate void StageChangedEventHandler(object sender, StageChangedEventArgs e);

	public sealed class StageChangedEventArgs : EventArgs
	{
		private MilestoneStage stage;
		private StageData data;

		private StageChangedEventArgs()
		{
		}

		internal StageChangedEventArgs(MilestoneStage stage, StageData data)
		{
			this.stage = stage;
			this.data = data;
		}

		public MilestoneStage Stage
		{
			get
			{
				return stage;
			}
		}

		public StageData Data
		{
			get
			{
				return data;
			}
		}
	}

	public delegate void TaskChangedEventHandler(object sender, TaskChangedEventArgs e);

	public sealed class TaskChangedEventArgs : EventArgs
	{
		private MilestoneStage stage;
		private Milestones.Task task;

		private TaskChangedEventArgs()
		{
		}

		internal TaskChangedEventArgs(MilestoneStage stage, Milestones.Task task)
		{
			this.stage = stage;
			this.task = task;
		}

		public MilestoneStage Stage
		{
			get
			{
				return stage;
			}
		}

		public Milestones.Task Task
		{
			get
			{
				return task;
			}
		}
	}
	
}
