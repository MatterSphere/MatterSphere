using System;
using System.Collections.Generic;
using System.Linq;

namespace Fwbs.Office.Outlook
{
    using System.Runtime.InteropServices;
    using Redemption;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed class RedemptionUserProperties :
        OutlookObject,
        MSOutlook.UserProperties
    {
        #region Fields

        private readonly PropertyTagMappings propTagMappings;
        private readonly OutlookItem item;
        private Redemption.MAPIUtils utils;
        private Dictionary<string, RedemptionUserProperty> propsbyname;
        private List<RedemptionUserProperty> propsbyindex;

        #endregion

        #region Constructors

        public RedemptionUserProperties(OutlookItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            this.item = item;

            this.utils = Redemption.RedemptionFactory.Default.CreateMAPIUtils();

            var cachedItems = FWBS.OMS.Session.CurrentSession.CachedItems;
            if (cachedItems.ContainsKey(PropertyTags.MAPPINGS_CACHEABLE_KEY))
            {
                this.propTagMappings = (PropertyTagMappings)cachedItems[PropertyTags.MAPPINGS_CACHEABLE_KEY];
            }
            else
            {
                this.propTagMappings = new PropertyTagMappings();
                cachedItems.Add(PropertyTags.MAPPINGS_CACHEABLE_KEY, this.propTagMappings);
            }

            Rebuild();

        }

        #endregion

        #region Overrides

        public override OutlookApplication Application
        {
            get { return item.Application; }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (utils != null)
                    {
                        if (Marshal.IsComObject(utils))
                            Marshal.ReleaseComObject(utils);
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        #endregion

        #region Properties

        internal OutlookItem InternalItem
        {
            get
            {
                return item;
            }
        }

        internal RDOFolderFields RDOFolderFields
        {
            get
            {
                return item.Folder.FolderFields;
            }
        }



        #endregion

        #region Methods


        public void Rebuild()
        {
            propsbyname = GetCustomProperties().ToDictionary(p => p.Name);
            propsbyindex = propsbyname.Values.ToList();
        }


        private IEnumerable<RedemptionUserProperty> GetCustomProperties()
        {
            RDOFolderFields folderFields = this.RDOFolderFields;
            object mapiobj = item.MAPIOBJECT;
            PropList proplist = utils.HrGetPropList(mapiobj, true);

            var customProps = new List<RedemptionUserProperty>();
            using (var mapper = propTagMappings.GetMapper(utils, mapiobj))
            {
                for (int i = 1, count = proplist.Count; i <= count; i++)
                {
                    int proptag = proplist[i];

                    if ((proptag | PropertyTags.CUSTOM_PROPERTY_BASELINE) != proptag)
                        continue;

                    INamedProperty namedProp = mapper.GetNamesFromIDs(proptag);
                    if (namedProp == null || namedProp.ID == null || namedProp.GUID == null)
                        continue;

                    if (!namedProp.GUID.Equals(PropertyTags.PROPTAG_CUSTOM_PROPERTY, StringComparison.OrdinalIgnoreCase))
                        continue;

                    string name = Convert.ToString(namedProp.ID);
                    RDOFolderField field = GetFolderField(folderFields, namedProp.GUID, name);

                    customProps.Add(new RedemptionUserProperty(this, name, proptag, Helpers.ToOutlookType(proptag), field));
                }
            }

            return customProps;
        }

        private static RDOFolderField GetFolderField(RDOFolderFields folderFields, string propGuid, string name)
        {
            return (from f in folderFields.Cast<RDOFolderField>()
                    where f.GUID.Equals(propGuid, StringComparison.OrdinalIgnoreCase) && f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                    select f).FirstOrDefault();
        }

        #endregion

        #region UserProperties Members

        MSOutlook.Application MSOutlook.UserProperties.Application
        {
            get
            {
                return Application;
            }
        }

        public MSOutlook.UserProperty Add(string Name, MSOutlook.OlUserPropertyType Type, object AddToFolderFields)
        {
            return Add(Name, Type, AddToFolderFields, null);
        }
        public MSOutlook.UserProperty Add(string Name, MSOutlook.OlUserPropertyType Type, object AddToFolderFields, object DisplayFormat)
        {
            RDOFolderField field = null;

            if (AddToFolderFields is bool && ((bool)AddToFolderFields))
                field = RDOFolderFields.Add(Name, Type.ToRedemptionType(), PropertyTags.PROPTAG_CUSTOM_PROPERTY, DisplayFormat);

            var propid = utils.GetIDsFromNames(item.MAPIOBJECT, PropertyTags.PROPTAG_CUSTOM_PROPERTY, Name, true);

            var rup = new RedemptionUserProperty(this, Name, propid, Type, field);

            RedemptionUserProperty orig;
            if (propsbyname.TryGetValue(Name, out orig))
            {
                propsbyname.Remove(Name);
                propsbyindex.Remove(orig);
            }

            propsbyname.Add(Name, rup);
            propsbyindex.Add(rup);

            return rup;
        }



        public MSOutlook.OlObjectClass Class
        {
            get { return MSOutlook.OlObjectClass.olUserProperties; }
        }

        public int Count
        {
            get { return propsbyindex.Count; }
        }

        public MSOutlook.UserProperty Find(string Name, object Custom)
        {
            RedemptionUserProperty p;
            if (propsbyname.TryGetValue(Name, out p))
                return p;

            return null;
        }

        public object Parent
        {
            get { return item; }
        }

        public void Remove(int Index)
        {
            var prop = (RedemptionUserProperty)this[Index];
            if (prop != null)
            {
                prop.Delete();
            }
        }

        internal void Remove(RedemptionUserProperty prop)
        {
            //Used by the delete method on the USerProperty
            if (prop == null)
                return;

            propsbyindex.Remove(prop);
            propsbyname.Remove(prop.Name);
        }

        public MSOutlook.NameSpace Session
        {
            get { return Application.Session; }
        }

        public MSOutlook.UserProperty this[object Index]
        {
            get
            {
                //1 - BASE

                string name = Convert.ToString(Index ?? String.Empty);
                int index;
                if (!(Index is string) && int.TryParse(name, out index))
                {
                    if (index < 1 || index > propsbyindex.Count)
                        throw new ArgumentOutOfRangeException("Index", Index, String.Empty);

                    index--;
                    return propsbyindex[index];
                }
                else
                {
                    RedemptionUserProperty rp;
                    if (propsbyname.TryGetValue(name, out rp))
                        return rp;

                }

                return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            foreach (var p in propsbyindex)
            {
                yield return p;
            }

            yield break;
        }

        #endregion
    }
}
