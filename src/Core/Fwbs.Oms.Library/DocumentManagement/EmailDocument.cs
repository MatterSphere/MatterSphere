using System;

namespace FWBS.OMS.DocumentManagement
{
    public sealed class EmailDocument : FWBS.OMS.CommonObject
    {
        private OMSDocument _doc;

        public EmailDocument() : base()
        {
        }

        public EmailDocument(OMSDocument doc)
            : base(doc.ID)
        {
        }



        #region CommonObject

        public void Fetch(long docid)
        {
            base.Fetch(docid);
        }

        protected override string DefaultForm
        {
            get
            {
                return String.Empty;
            }
        }

        public override string FieldPrimaryKey
        {
            get
            {
                return "docID";
            }
        }

        public override object Parent
        {
            get
            {
                return _doc.Parent;
            }
        }

        protected override string PrimaryTableName
        {
            get
            {
                return "dbDocumentEmail";
            }
        }

        protected override string SelectStatement
        {
            get
            {
                return "select * from dbDocumentEmail";
            }
        }

        #endregion

        #region Properties

        public string StoreID
        {
            get
            {
                return Convert.ToString(GetExtraInfo("docStoreID"));
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    SetExtraInfo("docStoreID", DBNull.Value);
                else
                    SetExtraInfo("docStoreID", value);

            }
        }

        public string EntryID
        {
            get
            {
                return Convert.ToString(GetExtraInfo("docEntryID"));
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SetExtraInfo("docEntryID", DBNull.Value);
                else
                    SetExtraInfo("docEntryID", value);
            }
        }

        public string From
        {
            get
            {
                return Convert.ToString(GetExtraInfo("docFrom"));
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SetExtraInfo("docFrom", DBNull.Value);
                else
                {
                    if (value.Length > 1000)
                        value = value.Substring(0, 1000);

                    SetExtraInfo("docFrom", value);
                }
            }
        }

        public string To
        {
            get
            {
                return Convert.ToString(GetExtraInfo("docTo"));
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SetExtraInfo("docTo", DBNull.Value);
                else
                {
                    if (value.Length > 1000)
                        value = value.Substring(0, 1000);

                    SetExtraInfo("docTo", value);
                }
            }
        }

        public string CC
        {
            get
            {
                return Convert.ToString(GetExtraInfo("docCC"));
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SetExtraInfo("docCC", DBNull.Value);
                else
                {
                    if (value.Length > 1000)
                        value = value.Substring(0, 1000);

                    SetExtraInfo("docCC", value);
                }
            }
        }

        public string Class
        {
            get
            {
                return Convert.ToString(GetExtraInfo("docclass"));
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SetExtraInfo("docclass", DBNull.Value);
                else
                    SetExtraInfo("docclass", value);
            }
        }

        public int Attachments
        {
            get
            {
                return Convert.ToInt32(GetExtraInfo("docAttachments"));
            }
            set
            {
                SetExtraInfo("docAttachments", value);
            }
        }

        public DateTime? Sent
        {
            get
            {
                object ret = GetExtraInfo("docSent");
                if (ret == DBNull.Value)
                    return null;
                else
                    return Convert.ToDateTime(ret);
            }
            set
            {
                if (value == null)
                    SetExtraInfo("docSent", DBNull.Value);
                else
                    SetExtraInfo("docSent", value);

            }
        }

        public DateTime? Received
        {
            get
            {
                object ret = GetExtraInfo("docReceived");
                if (ret == DBNull.Value)
                    return null;
                else
                    return Convert.ToDateTime(ret);
            }
            set
            {
                if (value == null)
                    SetExtraInfo("docReceived", DBNull.Value);
                else
                    SetExtraInfo("docReceived", value);

            }
        }

         public string ConversationTopic
        {
            get
            {
                object ret = GetExtraInfo("docConversationTopic");
                if (ret == DBNull.Value)
                    return string.Empty;
                else
                    return (string)ret;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SetExtraInfo("docConversationTopic", DBNull.Value);
                else
                {
                    if (value.Length > 250)
                        value = value.Substring(0, 250);

                    SetExtraInfo("docConversationTopic", value);
                }
            }
        }

         public string ConversationIndex
         {
             get
             {
                 object ret = GetExtraInfo("docConversationIndex");
                 if (ret == DBNull.Value)
                     return string.Empty;
                 else
                     return (string)ret;
             }
             set
             {
                 if (String.IsNullOrEmpty(value))
                     SetExtraInfo("docConversationIndex", DBNull.Value);
                 else
                 {
                      if (value.Length > 250)
                        value = value.Substring(0, 250);

                     SetExtraInfo("docConversationIndex", value);
                 }
             }
         }


  
        #endregion
    }
}
