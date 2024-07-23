using System;

namespace FWBS.OMS
{
    using DocumentManagement.Storage;

    [System.ComponentModel.DefaultProperty("Code")]
	public class PrecType : CommonObject, DocumentManagement.Storage.IStorageItemType
	{
		public static PrecType GetPrecType(string code)
		{
			PrecType t = new PrecType();
			t.Fetch(code);
			return t;
		}

		protected override string DefaultForm
		{
			get
			{
				return null;
			}
		}


		public override string FieldPrimaryKey
		{
			get
			{
				return "typecode";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "SELECT * FROM dbdocumenttype";
			}
		}



		#region Properties
		

		[LocCategory("(Details)")]
		public string Code
		{
			get
			{
				return Convert.ToString(base.UniqueID);
			}
			set
			{
				base.UniqueID = value;
			}
		}

        public string DefaultPrecExtension
		{
			get
			{
				return Convert.ToString(GetExtraInfo("typeprecext"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("typeprecext", DBNull.Value);
				else
					SetExtraInfo("typeprecext", value);
			}
		}

		public short DefaultStorageProvider
		{
			get
			{
				return Common.ConvertDef.ToInt16(GetExtraInfo("typeDefaultStorage"), -1);
			}
			set
			{
				if (value < 0)
					SetExtraInfo("typeDefaultStorage", DBNull.Value);
				else
					SetExtraInfo("typeDefaultStorage", value);
			}
		}


		public short DefaultApplication
		{
			get
			{
				return Convert.ToInt16(GetExtraInfo("typeDefaultApp"));
			}
			set
			{
				SetExtraInfo("typeDefaultApp", value);
			}
		}

        public StorageFeature StorageFeaturesSupported
        {
            get
            {
                try
                {
                    return (StorageFeature)Enum.ToObject(typeof(StorageFeature), (int)GetExtraInfo("typePrecSupports"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                SetExtraInfo("typePrecSupports", (int)value);
            }
        }

		#endregion


		
		#region IParent Implementation

		public override object Parent
		{
			get
			{
				return null;
			}
		}

		#endregion

        #region IStorageItemType Members

        string IStorageItemType.Code
        {
            get 
            {
                return Code;
            }
        }

        string IStorageItemType.Extension
        {
            get 
            {
                return DefaultPrecExtension;
            }
        }

        DocumentManagement.Storage.StorageProvider DocumentManagement.Storage.IStorageItemType.GetDefaultStorageProvider()
        {
            return DocumentManagement.Storage.StorageManager.CurrentManager.GetStorageProvider(DefaultStorageProvider);
        }

        public bool Supports(StorageFeature feature)
        {
            return ((StorageFeaturesSupported | feature) == StorageFeaturesSupported);
        }

        #endregion
    }
}
