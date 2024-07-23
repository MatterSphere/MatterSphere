using System.Collections.Generic;

namespace FWBS.OMS.FileManagement.Addins.Dashboard
{
    public class TasksProvider : Tasks
    {
        public void Complete(long taskId)
        {
            base.CompleteTask(taskId);
        }

        public void Assign(long taskId, int userId)
        {
            FWBS.OMS.Tasks.AssignTo( new []{ taskId }, null, userId);
        }

        public void AssignTask(long taskId)
        {
            base.AssignTasks(new List<long>() { taskId });
        }

        public void Unassign(long taskId)
        {
            base.UnassignTasks(new List<long> { taskId});
        }

        public void Delete(long taskId)
        {
            base.DeleteTask(taskId);
        }

        public void Delete(IEnumerable<long> taskIds)
        {
            foreach (long taskId in taskIds)
            {
                base.DeleteTask(taskId, false);
            }
        }
    }
}
