using System;

namespace FWBS.OMS.FileManagement
{
    internal class FileExtender
	{
        private FileManagementCommonObject obj;
        private readonly OMSFile file;
        private bool fmappstoresupported;

		internal FileExtender()
		{
            fmappstoresupported = Session.CurrentSession.IsTable("dbFileManagementAppData");
            if (!fmappstoresupported)
                throw new NotSupportedException("dbFileManagementAppData does not exist for this feature to work.");

		}

		internal FileExtender(OMSFile file) : this()
		{
            if (file == null)
                throw new ArgumentNullException("file");

            this.file = file;
		}

        public long FileId
        {
            get
            {
                return file.ID;
            }
        }

       

        [Lookup("FILEMANAPP")]
		[LocCategory("FILEMANAGEMENT")]
        [System.ComponentModel.Editor(typeof(FWBS.OMS.Design.DataListEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [FWBS.OMS.Design.DataList("DSFMAPPS", Parse = true, UseNull = true, NullValue = FMApplication.NO_APP)]
        [System.ComponentModel.TypeConverter(typeof(FWBS.OMS.Design.DataListConverter))]
		[EnquiryEngine.EnquiryUsage(true)]
        public string FileManagementApplicationUI
		{
			get
			{
                return FileManagementApplication;
			}
			set
			{
				if (value == null) value = FMApplication.NO_APP;
                if (value != FileManagementApplication)
                {
                    FileManagementApplication = value;
                }
			}
		}

        private void CheckFileManagementObject()
        {
            if (!fmappstoresupported)
                return;

            if (obj == null || obj.IsDeleted)
            {
                obj = new FileManagementCommonObject(file);

            }
        }

        public void Update()
        {
            if (!fmappstoresupported)
                return;

            if (obj != null)
            {
                if (obj.IsDeleted)
                    return;

                if (obj.IsDirty)
                    obj.Update();
            }
        }


        public string FileManagementApplication
        {
            get
            {             

                CheckFileManagementObject();

                if (!fmappstoresupported)
                    return FileManagement.FMApplication.NO_APP;

                return Convert.ToString(obj.GetExtraInfo("appcode"));
            }
            set
            {
                CheckFileManagementObject();

                if (!fmappstoresupported)
                    return;

                if (value == null) value = FMApplication.NO_APP;
                if (value != FileManagementApplication)
                {
                    obj.SetExtraInfo("appcode", value);
                }
            }
        }

        private sealed class FileManagementCommonObject : FWBS.OMS.CommonObject
        {
            private OMSFile file;

            public FileManagementCommonObject(OMSFile file)
            {
                if (file == null)
                    throw new ArgumentNullException("file");
                
                this.file = file;

                if (Exists(file.ID) && file.IsNew == false)
                    Fetch(file.ID);
                else
                    Create();

                this.file.Updated += new EventHandler(file_Updated);
                SetExtraInfo("fileid", file.ID);
            }

            private void file_Updated(object sender, EventArgs e)
            {
                this.file.Updated -= new EventHandler(file_Updated);

                //appcode does not allow nulls
                if (IsDirty)
                {
                     Update();
                }
            }

            protected override string DatabaseTableName
            {
                get
                {
                    return "dbFileManagementAppData";
                }
            }

            public override void Delete()
            {

                //Got to override the delete as the base delete calls update.
                //The update may call the delete causing a infinite loop.
                _data.Rows[0].Delete();
            }

            public override void Update()
            {
                if (file.IsNew)
                    return;

                //Even at the end of a file save wizard the file is flagged
                //as fals as a secondary update must occur.
                string appcode = Convert.ToString(GetExtraInfo("appcode"));

                SetExtraInfo("fileid", file.ID);


                if (appcode == FMApplication.NO_APP)
                {
                    if (IsNew)
                        return;
                    else
                    {
                        Delete();
                        base.Update();
                    }
                }

                else
                    base.Update();
                
                
            }

            public override string FieldPrimaryKey
            {
                get { return "fileid"; }
            }

            public override object Parent
            {
                get { return null; }
            }

            protected override string DefaultForm
            {
                get { return String.Empty; }
            }

            protected override string SelectStatement
            {
                get { return "select * from dbFileManagementAppData"; }
            }

            protected override string PrimaryTableName
            {
                get { return "FMAPPDATA"; }
            }
        }

	}
}
