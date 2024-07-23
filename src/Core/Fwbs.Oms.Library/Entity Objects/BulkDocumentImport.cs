using System;
using System.Collections;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{
    public class BulkDocumentImport : IEnquiryCompatible
    {

        [EnquiryUsage(true)]
        public BulkDocumentImport()
        {
            Wallet = "GENERAL";
            Description = "Imported '%namenoext%'";
        }

        /// <summary>
        /// Gets or Sets the selected Associate.
        /// </summary>
        public Associate SelectedAssociate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the User who authored the document.
        /// </summary>
        public User AuthoredBy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the stored description of the document.
        /// </summary>
        [EnquiryUsage(true)]
        public string AlternateDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the stored description of the document.
        /// </summary>
        [EnquiryUsage(true)]
        public string Description
        {
            get;
            set;
        }

        [EnquiryUsage(true)]
        public string AppendClientViewableNotepad
        {
            get;
            set;
        }

        [EnquiryUsage(true)]
        public string AppendFileNotViewableNotepad
        {
            get;
            set;
        }

        [EnquiryUsage(true)]
        public bool HasReminder
        {
            get;
            set;
        }

        [EnquiryUsage(true)]
        public string ReminderSubject
        {
            get;
            set;
        }

        [EnquiryUsage(true)]
        public int ReminderFeeEarner
        {
            get;
            set;
        }

        [EnquiryUsage(true)]
        public bool SkipTime
        {
            get;
            set;
        }

        [EnquiryUsage(true)]
        public string ReminderNote
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the document waller property.
        /// </summary>
        [EnquiryUsage(true)]
        public string Wallet
        {
            get;
            set;
        }

        private int _reminderdays;

        [EnquiryUsage(true)]
        public int ReminderDays
        {
            get
            {
                return _reminderdays;
            }
            set
            {
                int oldval = ReminderDays;

                if (oldval != value)
                {
                    //UTCFIX: DM - 30/11/06 - Make Reminder date must be local.
                    _reminderdays = value;

                    Common.DateTimeNULL olddue = ReminderDue;
                    _reminderdue = DateTime.Today.AddDays(_reminderdays);

                    OnPropertyChanged(new PropertyChangedEventArgs("ReminderDue", olddue, this.ReminderDue));
                }
            }
        }

        private Common.DateTimeNULL _reminderdue;
        
        [EnquiryUsage(true)]
        public Common.DateTimeNULL ReminderDue
        {
            get
            {
                return _reminderdue;
            }
            set
            {
                Common.DateTimeNULL olddue = ReminderDue;
                int olddays = ReminderDays;

                //UTCFIX: DM - 30/11/06 - DateTimeNull should handle the date comparisons.
                if (!olddue.Equals(value))
                {
                    _reminderdue = value;
                    if (_reminderdue.IsNull)
                    {
                        _reminderdue = DateTime.Today;
                    }

                    //UTCFIX: DM - 30/11/06 - Make the following dates local kinds for the arithmatic.
                    DateTime today = DateTime.Today;
                    DateTime due = _reminderdue.ToLocalTime();
                    TimeSpan diff = due.Subtract(today);
                    _reminderdays = Convert.ToInt32(diff.TotalDays);

                    OnPropertyChanged(new PropertyChangedEventArgs("ReminderDays", olddays, this.ReminderDays));
                    OnPropertyChanged(new PropertyChangedEventArgs("ReminderDue", olddue, this.ReminderDue));
                }
            }
        }


        public IEnumerable UnProfiledDocumentsToSave
        {
            get;
            set;
        }


        public IEnumerable ProfiledDocumentsToSaveAsNew
        {
            get;
            set;
        }


        public IEnumerable ProfiledDocumentsToSaveWithExistingProfileInformation
        {
            get;
            set;
        }


        [EnquiryUsage(true)]
        public Guid FolderGuid
        {
            get;
            set;
        }


        #region IEnquiryCompatible Members

        /// <summary>
        /// Raises the property changed event with the specified event arguments.
        /// </summary>
        /// <param name="e">Property Changed Event Arguments.</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public Enquiry Edit(FWBS.Common.KeyValueCollection param)
        {
            return null;
        }

        public Enquiry Edit(string customForm, FWBS.Common.KeyValueCollection param)
        {
            return null;
        }

        #endregion

        #region IExtraInfo Members

        public object GetExtraInfo(string fieldName)
        {
            return null;
        }

        public Type GetExtraInfoType(string fieldName)
        {
            return null;
        }

        public void SetExtraInfo(string fieldName, object val)
        {
            
        }

        public System.Data.DataSet GetDataset()
        {
            return new System.Data.DataSet();
        }

        public System.Data.DataTable GetDataTable()
        {
            var data = new System.Data.DataTable();
            data.Columns.Add("Column");
            data.Rows.Add("");
            return data;
        }

        #endregion

        #region IUpdateable Members

        public void Update()
        {
            
        }

        public void Refresh()
        {

        }

        public void Refresh(bool applyChanges)
        {

        }

        public void Cancel()
        {

        }

        public bool IsDirty
        {
            get { return true; }
        }

        public bool IsNew
        {
            get { return true; }
        }

        #endregion

        #region IParent Members

        public object Parent
        {
            get { return null; }
        }

        #endregion
    }
}
