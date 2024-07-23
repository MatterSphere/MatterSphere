using System;
using System.Runtime.InteropServices;

namespace Fwbs.Documents
{
    public sealed class MsgDocument : IRawDocument, ICustomPropertiesDocument, IDisposable
    {
        private const string USER_PROPERTIES_GUID = "{00020329-0000-0000-C000-000000000046}";

        #region Fields

        private Redemption.MAPIUtils utils;
        private Redemption.MessageItem msg;

        #endregion

        #region ICustomPropertiesDocument Members

        public void ReadCustomProperties(CustomPropertyCollection properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (!IsOpen)
                throw new FileClosedException();

            MsgPropertyConverter conv = new MsgPropertyConverter();

            Redemption.PropList props = utils.HrGetPropList(msg, true);
            for (int ctr = 1; ctr <= props.Count; ctr++)
            {
                int proptag = (int)props[ctr];
                Redemption.NamedProperty name = utils.GetNamesFromIDs(msg, proptag);
                if (name != null)
                {
                    if (name.GUID == USER_PROPERTIES_GUID)
                    {
                        object val = msg.get_Fields(proptag);
                        if (val != null)
                        {
                            Redemption.rdoUserPropertyType type = conv.ToSourceType(val.GetType());
                            val = conv.FromSource(val, type);

                            try
                            {
                                CustomProperty prop = properties.Add(Convert.ToString(name.ID,System.Globalization.CultureInfo.InvariantCulture));
                                prop.Value = val;
                            }
                            catch(ArgumentException)
                            {
                                //Not a valid property name.
                            }
                        }
                    }
                }
            }

            properties.Accept();
        }

        private int FindPropertyTag(string id)
        {
            Redemption.PropList props = utils.HrGetPropList(msg, true);
            for (int ctr = 1; ctr <= props.Count; ctr++)
            {
                int proptag = (int)props[ctr];
                Redemption.NamedProperty name = utils.GetNamesFromIDs(msg, proptag);
                if (name != null)
                {
                    if (name.GUID == USER_PROPERTIES_GUID)
                    {
                        if (Convert.ToString(name.ID, System.Globalization.CultureInfo.InvariantCulture).ToUpperInvariant() == id.ToUpperInvariant())
                            return proptag;
                    }
                }
            }

            return utils.GetIDsFromNames(msg, USER_PROPERTIES_GUID, id, true);
        }

        public void WriteCustomProperties(CustomPropertyCollection properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");


            if (!IsOpen)
                throw new FileClosedException();

            MsgPropertyConverter conv = new MsgPropertyConverter();

            foreach (CustomProperty prop in properties)
            {
                int proptag = FindPropertyTag(prop.Name);
                if (prop.IsDeleted)
                {
                    msg.set_Fields(proptag, null);
                }
                else if (prop.HasChanged)
                {
                    object val = msg.get_Fields(proptag);
                    
                    if (val != null)
                    {
                        Redemption.rdoUserPropertyType type = conv.ToSourceType(val.GetType());
                        val = conv.FromSource(val, type);
                    }

                    if (val == null || !val.Equals(prop.Value))
                    {
                        val = conv.ToSource(prop.Value);
                        msg.set_Fields(proptag, null);
                        proptag = FindPropertyTag(prop.Name);
                        msg.set_Fields(proptag, val);
                    }

                }
            }
        }

        #endregion

        #region IRawDocument Members

        public void Open(System.IO.FileInfo file)
        {

            if (!IsOpen)
            {
                if (file == null)
                    throw new ArgumentNullException("file");

                if (!System.IO.File.Exists(file.FullName))
                    throw new System.IO.FileNotFoundException("", file.FullName);

                try
                {

                    utils = Redemption.RedemptionFactory.Default.CreateMAPIUtils();
                    msg = utils.GetItemFromMsgFile(file.FullName, false);

                }
                catch (Exception ex)
                {
                    throw new System.IO.FileLoadException(ex.Message, file.FullName, ex);
                }
            }

        }

        public void Save()
        {
            if (!IsOpen)
                throw new FileClosedException();

            msg.Save();
        }

        public void Close()
        {
            if (msg != null)
            {
                Marshal.FinalReleaseComObject(msg);
                msg = null;
            }

            if (utils != null)
            {
                Marshal.FinalReleaseComObject(utils);
                utils = null;
            }
        }

        public bool IsOpen
        {
            get { return (msg!= null); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
