using FWBS.OMS.Extensibility;
using FWBS.OMS.Interfaces;
using System;
using System.Data;

namespace FWBS.OMS
{
    public static class ExtensibilityExtensions
    {
        public static void OnExtLoaded(this IEnquiryCompatible enquiryCompatible)
        {
            OnObjectEvent(enquiryCompatible, ObjectEvent.Loaded);
        }

        private static bool ExtCreating(this IEnquiryCompatible enquiryCompatible, bool canCancel = true)
        {
            ObjectEventArgs objectEventArgs = OnObjectEvent(enquiryCompatible, ObjectEvent.Creating, canCancel);
            return objectEventArgs.Cancel && objectEventArgs.CanCancel;
        }
        private static void ExtCreated(this IEnquiryCompatible enquiryCompatible)
        {
            OnObjectEvent(enquiryCompatible, ObjectEvent.Created);
        }
        private static bool ExtDeleting(this IEnquiryCompatible enquiryCompatible, bool canCancel = true)
        {
            ObjectEventArgs objectEventArgs = OnObjectEvent(enquiryCompatible, ObjectEvent.Deleting, canCancel);
            return objectEventArgs.Cancel && objectEventArgs.CanCancel;
        }

        private static void ExtDeleted(this IEnquiryCompatible enquiryCompatible)
        {
            OnObjectEvent(enquiryCompatible, ObjectEvent.Deleted);
        }

        private static bool ExtUpdating(this IEnquiryCompatible enquiryCompatible, bool canCancel = true)
        {
            ObjectEventArgs objectEventArgs = OnObjectEvent(enquiryCompatible, ObjectEvent.Updating, canCancel);
            return objectEventArgs.Cancel && objectEventArgs.CanCancel;
        }

        private static void ExtUpdated(this IEnquiryCompatible enquiryCompatible)
        {
            OnObjectEvent(enquiryCompatible, ObjectEvent.Updated);

        }

        public static bool OnExtValueChanging(this IEnquiryCompatible enquiryCompatible, bool canCancel = true, ValueChangingEventArgs vce = null)
        {
            ObjectEventArgs objectEventArgs = OnObjectEvent(enquiryCompatible, ObjectEvent.ValueChanging, canCancel, vce);
            return objectEventArgs.Cancel && objectEventArgs.CanCancel;
        }

        public static void OnExtValueChanged(this IEnquiryCompatible enquiryCompatible, ValueChangedEventArgs vce)
        {
            OnObjectEvent(enquiryCompatible, ObjectEvent.ValueChanged, true, vce);
        }

        public static bool OnExtRefreshing(this IEnquiryCompatible enquiryCompatible, bool canCancel = true)
        {
            ObjectEventArgs objectEventArgs = OnObjectEvent(enquiryCompatible, ObjectEvent.Refreshing, canCancel);
            return objectEventArgs.Cancel && objectEventArgs.CanCancel;
        }

        public static void OnExtRefreshed(this IEnquiryCompatible enquiryCompatible)
        {
            OnObjectEvent(enquiryCompatible, ObjectEvent.Refreshed);

        }

        private static ObjectEventArgs OnObjectEvent(IEnquiryCompatible enquiryCompatible, ObjectEvent ev, bool canCancel = true, EventArgs ea = null)
        {
            //Call the extensibility event for addins.
            ObjectEventArgs e = new ObjectEventArgs(enquiryCompatible, ev, canCancel, ea);
            Session.CurrentSession.OnObjectEvent(e);
            return e;
        }

        public static bool OnExtCreatingUpdatingOrDeleting(this IEnquiryCompatible enquiryCompatible, ObjectState state)
        {
            bool oev = false;
            if (state != ObjectState.Unchanged)
            {
                switch (state)
                {
                    case ObjectState.Added:
                        oev = ExtCreating(enquiryCompatible);
                        break;
                    case ObjectState.Deleted:
                        oev = ExtDeleting(enquiryCompatible);
                        break;
                    case ObjectState.Modified:
                        oev = ExtUpdating(enquiryCompatible);
                        break;
                    default:
                        goto case ObjectState.Added;
                }
            }
            return oev;
        }

        public static void OnExtCreatedUpdatedDeleted(this IEnquiryCompatible enquiryCompatible, ObjectState state)
        {
            if (state != ObjectState.Unchanged)
            {
                switch (state)
                {
                    case ObjectState.Added:
                        ExtCreated(enquiryCompatible);
                        break;
                    case ObjectState.Deleted:
                        ExtDeleted(enquiryCompatible);
                        break;
                    case ObjectState.Modified:
                        ExtUpdated(enquiryCompatible);
                        break;
                    default:
                        goto case ObjectState.Added;
                }
            }
        }

        public static void SetExtraInfo(this IEnquiryCompatible enquiryCompatible, DataRow data, string fieldName, object val)
        {
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

            object original = enquiryCompatible.GetExtraInfo(fieldName);
            ValueChangingEventArgs changing = new ValueChangingEventArgs(fieldName, original, val);
            if (Convert.ToString(changing.OriginalValue) != Convert.ToString(changing.ProposedValue))
            {
                if (OnExtValueChanging(enquiryCompatible, true, changing))
                    return;
            }

            data[fieldName] = val;

            ValueChangedEventArgs changed = new ValueChangedEventArgs(fieldName, original, val);
            if(Convert.ToString(changed.OriginalValue) != Convert.ToString(changed.ProposedValue))
                OnExtValueChanged(enquiryCompatible, changed);
        }
    }
}
