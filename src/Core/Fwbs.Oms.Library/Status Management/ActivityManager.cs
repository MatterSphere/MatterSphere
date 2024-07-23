using System;
using FWBS.OMS.StatusManagement.Activities;

namespace FWBS.OMS.StatusManagement
{

    #region "Client Activity"
    public class ClientActivity
    {
        private long _clID;
        private ClientStatusActivityType _activity;

        public ClientActivity(FWBS.OMS.Client client, ClientStatusActivityType activity)
        {
            _clID = client.ClientID;
            _activity = activity;
        }

        public bool IsAllowed()
        {
            ClientStatusManager c = new ClientStatusManager(_clID);

            switch (_activity)
            {
                case ClientStatusActivityType.FileCreation:
                    return c.FileCreation;
                case ClientStatusActivityType.TimeEntry:
                    return c.TimeEntry;
                default:
                    throw new Exception("The supplied activity type is not supported");
            }
        }

        public void Check()
        {
            if (!IsAllowed())
                throw new FWBS.OMS.Security.PermissionsException("ERRCLMSTACTDENY", "Activity Denied");
        }

    }
    #endregion

    #region "File Activity
    public class FileActivity
    {
        private OMSFile _file;
        private FileStatusActivityType _activity;

        public FileActivity(FWBS.OMS.OMSFile file, FileStatusActivityType activity)
        {
            _file = file; 
            _activity = activity;
        }

        public bool IsAllowed()
        {
            FileStatusManager f = null;

            if (_file.FileStatusManager == null || _file.FileStatusManager.FileID == 0 || _file.FileStatusManager.TimeOut < DateTime.Now)
            {
                f = new FileStatusManager(_file.ID);
                _file.FileStatusManager = f;
            }
            else
            {
                f = _file.FileStatusManager;
            }

            switch (_activity)
            {
                case FileStatusActivityType.AppointmentCreation:
                    return f.AppointmentCreation;
                case FileStatusActivityType.AssociateCreation:
                    return f.AssociateCreation;
                case FileStatusActivityType.DocumentModification:
                    return f.DocumentModification;
                case FileStatusActivityType.TaskflowProcessing:
                    return f.TaskflowProcessing;
                case FileStatusActivityType.TimeEntry:
                    return f.TimeEntry;
                default:
                    throw new Exception("The supplied activity type is not supported");
            }
        }

        public void Check()
        {
            if (!IsAllowed())
                throw new FWBS.OMS.Security.PermissionsException("ERRCLMSTACTDENY", "Activity Denied");
        }

    }
    #endregion

}
