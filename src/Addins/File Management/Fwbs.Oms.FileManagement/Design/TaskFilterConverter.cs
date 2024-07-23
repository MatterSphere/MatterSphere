using System.ComponentModel;

namespace FWBS.OMS.FileManagement.Design
{
    internal class TaskFilterConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            Configuration.TaskTypeConfig config = context.Instance as Configuration.TaskTypeConfig;
            System.Collections.ArrayList ar = new System.Collections.ArrayList();
            if (config != null)
            {
                foreach (Configuration.MilestoneTaskConfig tsk in config.Application.MilestoneTasks)
                {
                    string f = tsk.TaskFilter;
                    if (f.Length == 0)
                        continue;
                    if (!ar.Contains(f))
                        ar.Add(f);
                }

            }
            return new StandardValuesCollection(ar);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    internal class TaskGroupConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            Configuration.TaskTypeConfig tskconfig = context.Instance as Configuration.TaskTypeConfig;
            Configuration.MilestoneTaskConfig msconfig = context.Instance as Configuration.MilestoneTaskConfig;

            System.Collections.ArrayList ar = new System.Collections.ArrayList();
            FMApplication app = null;

            if (tskconfig != null)
                app = tskconfig.Application;
            else if (msconfig != null)
                app = msconfig.Application;

            if (app != null)
            {
                foreach (Configuration.MilestoneTaskConfig tsk in app.MilestoneTasks)
                {
                    string f = tsk.TaskGroup;
                    if (f.Length == 0)
                        continue;
                    if (!ar.Contains(f))
                        ar.Add(f);
                }

            }
            return new StandardValuesCollection(ar);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
