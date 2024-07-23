using System;
using System.ComponentModel;

namespace FWBS.OMS
{
    using System.Data;
    using DocumentManagement.Storage;

    [System.ComponentModel.DefaultProperty("Code")]
	public class DocType : BuiltInOMSType, IStorageItemType
	{
		public static DocType GetDocType(string code)
		{
            Session.CurrentSession.CheckLoggedIn();

            DocType ut = Session.CurrentSession.CurrentDocumentTypes[code] as DocType;

            if (ut == null)
            {
                ut = new DocType(code);
            }
            return ut;
		}

        #region Constructors

		public DocType() : base ("")
		{
		}

        internal DocType(string code)
            : base(code)
		{
			//An edit contructor should add the object created to the session memory collection.
			if (IsNew == false) 
			{
				foreach (DataRow row in _omstype.Tables["FORM"].Rows)
					row["frmdesc"] = Convert.ToString(_omstype.Tables["FORM"].Rows[0]["frmdesc"]).Replace("%DOCTYPE%", this.Description);
				Session.CurrentSession.CurrentDocumentTypes.Add(Code, this);
				
				//Call the extensibility event for addins.
				OnObjectEvent(Extensibility.ObjectEvent.Loaded);
			}
		}

		#endregion

		#region Abstraction

		public override OMSType Clone()
		{
			DocType ut = new DocType();
			ut._omstype = _omstype.Copy();
			ut._omstype.Tables[0].Clear();
			ut._omstype.AcceptChanges();
			ut._omstype.Tables[0].Rows.Add(_omstype.Tables[0].Rows[0].ItemArray);
			ut.SetExtraInfo("typecode", DBNull.Value);
			ut.SetExtraInfo("typedesc", DBNull.Value);
			ut.BuildXML();
			return ut;
		}

		public override Type OMSObjectType
		{
			get
			{
				return typeof(OMSDocument);
			}
		}

		
		public override string SearchListName
		{
			get
			{
				return "ADMDOCTYPE";
			}
		}

		public override string CodeLookupGroup
		{
			get
			{
				return "DOCTYPE";
			}
		}

		protected override string TableName
		{
			get
			{
				return "dbDocumentType";;
			}
		}

		protected override string Procedure
		{
			get
			{
				return "sprDocumentType";
			}
		}


        public override System.Drawing.Icon GetAlternateGlyph()
        {
            return Common.IconReader.GetFileIcon(String.Format("test.{0}", DefaultDocExtension), Common.IconReader.IconSize.Small, false);   
        }

		protected override string CacheFolder
		{
			get
			{
				return "DocumentTypes";
			}
		}

		public override Interfaces.IOMSType GetObject(object id)
		{
			return OMSDocument.GetDocument(Convert.ToInt64(id));
		}

		public override Interfaces.IOMSType CreateObject(object [] parameters)
		{
			return null;
		}

		#endregion

        /// <summary>
        /// Gets or Sets whether the client adds a default combined associate when there is more than one contact for the client when creating a new file.
        /// </summary>
        [LocCategory("(DETAILS)")]
        [Lookup("DOCUMENTDETAILS")]
        [System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.CodeDescriptionEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        public string DocumentDetailsConfig
        {
            get
            {
                System.Xml.XmlElement el = GetConfigRoot().SelectSingleNode("DocumentDetail") as System.Xml.XmlElement;
                if (el == null)
                    return "";
                else
                    return el.InnerText;
            }
            set
            {
                System.Xml.XmlElement el = GetConfigRoot().SelectSingleNode("DocumentDetail") as System.Xml.XmlElement;
                if (el == null)
                {
                    el = _config.CreateElement("DocumentDetail");
                    GetConfigRoot().AppendChild(el);
                }

                if (value == null)
                    el.InnerText = "";
                else
                    el.InnerText = value;

                OnDirty();
            }
        }

        [LocCategory("Document")]
        [Lookup("DefDocFileExt")]
		public string DefaultDocExtension
		{
			get
			{
				return Convert.ToString(GetExtraInfo("typefileext"));
			}
			set
			{
                if (value != DefaultDocExtension)
                {
                    if (value == null || value == "")
                        SetExtraInfo("typefileext", DBNull.Value);
                    else
                        SetExtraInfo("typefileext", value);
                }
			}
		}

        [LocCategory("Document")]
        [Lookup("DefPrecFileExt")]
        public string DefaultPrecExtension
        {
            get
            {
                return Convert.ToString(GetExtraInfo("typeprecext"));
            }
            set
            {
                if (value != DefaultPrecExtension)
                {
                    if (value == null || value == "")
                        SetExtraInfo("typeprecext", DBNull.Value);
                    else
                        SetExtraInfo("typeprecext", value);
                }
            }
        }

        [LocCategory("Document")]
        [Lookup("DefStorage")]
        [Design.DataList("DSSTOREPROVS")]
        [Editor("FWBS.OMS.Design.DataListEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(Design.DataListConverter))]
		public short DefaultStorageProvider
		{
			get
			{
				return Common.ConvertDef.ToInt16(GetExtraInfo("typeDefaultStorage"), -1);
			}
			set
			{
                if (value != DefaultStorageProvider)
                {
                    if (value < 0)
                        SetExtraInfo("typeDefaultStorage", DBNull.Value);
                    else
                        SetExtraInfo("typeDefaultStorage", value);
                }
			}
		}

        [LocCategory("Document")]
        [Lookup("DefApplication")]
        [Design.DataList("DSAPPLICATIONS")]
        [Editor("FWBS.OMS.Design.DataListEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(Design.DataListConverter))]
		public short DefaultApplication
		{
			get
			{
				return Convert.ToInt16(GetExtraInfo("typeDefaultApp"));
			}
			set
			{
                if (value != DefaultApplication)
                {
                    SetExtraInfo("typeDefaultApp", value);
                }
			}
		}

        [LocCategory("Document")]
        [Lookup("SPFeaturesSupp")]
        [Editor("FWBS.OMS.UI.Windows.Design.FlagsEditor, omsAdmin", typeof(System.Drawing.Design.UITypeEditor))]
        public StorageFeature StorageFeaturesSupported
        {
            get
            {
                try
                {
                    return (StorageFeature)Enum.ToObject(typeof(StorageFeature), (int)GetExtraInfo("typeDocSupports"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                if (value != StorageFeaturesSupported)
                {
                    SetExtraInfo("typeDocSupports", (int)value);
                }
            }
        }

        #region IStorageItemType Members

        string DocumentManagement.Storage.IStorageItemType.Code
        {
            get 
            {
                return Code;
            }
        }

        string DocumentManagement.Storage.IStorageItemType.Extension
        {
            get 
            {
                return DefaultDocExtension;
            }
        }

        DocumentManagement.Storage.StorageProvider FWBS.OMS.DocumentManagement.Storage.IStorageItemType.GetDefaultStorageProvider()
        {
            return DocumentManagement.Storage.StorageManager.CurrentManager.GetStorageProvider(DefaultStorageProvider);
        }


        public bool Supports(StorageFeature feature)
        {
            StorageFeature features = StorageFeaturesSupported;
            features |= DocumentManagement.Storage.StorageFeature.Retrieving;
            features |= DocumentManagement.Storage.StorageFeature.Storing;
            features |= DocumentManagement.Storage.StorageFeature.Register;
            return ((features | feature) == features);
        }

        #endregion
    }
}
