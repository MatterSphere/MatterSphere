using System;
using System.Collections.Generic;
using System.Xml;

namespace FWBS.OMS.FileManagement.Configuration
{
    public sealed class TaskTypeConfigCollection : Crownwood.Magic.Collections.CollectionWithEvents
    {
        #region Fields

        private XmlElement _info;
        private FMApplication _app;

        #endregion

        #region Constructors

        private TaskTypeConfigCollection() { }

        internal TaskTypeConfigCollection(FMApplication app, XmlElement info)
        {
            if (app == null)
                throw new ArgumentNullException("app");

            if (info == null)
                throw new ArgumentNullException("info");

            _app = app;
            _info = info;

            foreach (XmlNode nd in _info.ChildNodes)
            {
                if (nd is XmlElement)
                {
                    Add(new TaskTypeConfig(_app, (XmlElement)nd.Clone()));
                }
            }

        }



        #endregion

        #region Collection Specifics

        internal TaskTypeConfig Add(TaskTypeConfig value)
        {
            // Use base class to process actual collection operation
            base.List.Add(value as object);
            return value;
        }

        internal void AddRange(TaskTypeConfig[] values)
        {
            // Use existing method to add each array entry
            foreach (TaskTypeConfig val in values)
            {
                if (val != null) Add(val);
            }
        }

        internal void Remove(TaskTypeConfig value)
        {
            // Use base class to process actual collection operation
            base.List.Remove(value as object);
        }

        internal void Insert(int index, TaskTypeConfig value)
        {
            // Use base class to process actual collection operation
            base.List.Insert(index, value as object);
        }

        internal bool Contains(TaskTypeConfig value)
        {
            // Use base class to process actual collection operation
            return base.List.Contains(value as object);
        }

        public TaskTypeConfig this[int index]
        {
            // Use base class to process actual collection operation
            get { return (base.List[index] as TaskTypeConfig); }
        }

        internal int IndexOf(TaskTypeConfig value)
        {
            // Find the 0 based index of the requested entry
            return base.List.IndexOf(value);
        }

        #endregion

        #region Indexers

        public TaskTypeConfig[] Match(Milestones.Task task)
        {

            List<TaskTypeConfig> list = new List<TaskTypeConfig>();

            foreach (TaskTypeConfig tt in this)
            {
                if (tt != null)
                {
                    bool match = false;
                    if (tt.TaskType == TaskTypeConfig.GLOBAL_TASK_TYPE_SCOPE)
                        match = true;
                    else
                    {
                        if (tt.TaskType.ToLower() == task.Type.ToLower())
                            match = true;
                        else
                            match = false;
                    }

                    if (match)
                    {
                        if (tt.TaskFilter.Length == 0)
                            match = true;
                        else
                        {
                            if (tt.TaskFilter.ToLower() == task.FilterId.ToLower())
                                match = true;
                            else
                                match = false;
                        }
                    }

                    if (match)
                    {
                        if (tt.TaskGroup.Length == 0)
                            match = true;
                        else
                        {
                            if (tt.TaskGroup.ToLower() == task.Group.ToLower())
                                match = true;
                            else
                                match = false;
                        }
                    }

                    if (match)
                        list.Add(tt);
                }
            }

            return list.ToArray();
        }

        public TaskTypeConfig this[Milestones.Task task]
        {
            get
            {
                var list = Match(task);
                if (list.Length == 0)
                    return null;

                return list[0];
            }
        }

        #endregion

    }
}
