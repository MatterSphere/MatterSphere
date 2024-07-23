using System;

namespace FWBS.OMS.FileManagement
{
	internal class FileTypeExtender
	{
		private FileType type;

		private FileTypeExtender()
		{
		}

		internal FileTypeExtender(FileType ft)
		{
			type = ft;
		}

		[Lookup("FILEMANAPP")]
		[LocCategory("FILEMANAGEMENT")]
        [System.ComponentModel.Editor(typeof(FWBS.OMS.Design.DataListEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [FWBS.OMS.Design.DataList("DSFMAPPS", Parse = true, UseNull = true, NullValue = FMApplication.NO_APP)]
        [System.ComponentModel.TypeConverter(typeof(FWBS.OMS.Design.DataListConverter))]
		public string FileManagementApplication
		{
			get
			{
				return type.GetSetting("fileManagementApplication", FMApplication.NO_APP).Trim();
			}
			set
			{
				if (value == null) value = FMApplication.NO_APP;
                if (value != FileManagementApplication)
                {
                    type.SetSetting("fileManagementApplication", value);
                }
			}
		}
	}
}
