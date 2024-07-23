using System;

namespace FWBS.OMS.Security.Permissions
{
    public abstract class Permission
    {
        public static string GetSecurableObjectType(ISecurable securedObject)
        {
            if (securedObject == null)
                throw new ArgumentNullException("securedObject");

            object[] attrs = securedObject.GetType().GetCustomAttributes(typeof(SecurableTypeAttribute), false);
            if (attrs.Length > 0)
                return ((SecurableTypeAttribute)attrs[0]).SecurableTypeCode;
            else
                return securedObject.GetType().Name.ToUpperInvariant();

        }

        public Permission(string permissionType) : this(Session.OMS, permissionType)
        {
        }

        protected Permission(ISecurable securedObject, StandardPermissionType permissionType)
            : this(securedObject, StandardTypeToString(permissionType))
        {
            standardType = permissionType;
        }

        public Permission(ISecurable securedObject, string permissionType)
        {
            if (securedObject == null)
                throw new ArgumentNullException("securedObject");

            if (String.IsNullOrEmpty(permissionType))
                throw new ArgumentNullException("permissionType");

            this.securedObject = securedObject;
            this.code = permissionType;
            this.standardType = StringToStandardType(permissionType);
            this.securableType = GetSecurableObjectType(securedObject);
           
        }

        private readonly ISecurable securedObject;
        public ISecurable SecuredObject
        {
            get
            {
                return securedObject;
            }
        }

        private readonly StandardPermissionType standardType;
        public StandardPermissionType StandardSecurableType
        {
            get
            {
                return standardType;
            }
        }

        private readonly string securableType;
        public string SecurableType
        {
            get
            {
                return securableType;
            }
        }

        private readonly string code;
        public string Code
        {
            get
            {
                return code;
            }
        }

        public void Check()
        {
            if (standardType == StandardPermissionType.Unknown)
            {
                 if (Supports(code))
                     SecurityManager.CurrentManager.CheckPermission(this);
            }
            else
            {
                if (Supports(standardType))
                    SecurityManager.CurrentManager.CheckPermission(this);
            }
        }

        protected abstract bool Supports(StandardPermissionType type);

        protected virtual bool Supports(string type)
        {
            return false;
        }

        public static Permission CreatePermission(StandardPermissionType permissionType, object securedObject = null)
        {
            Permission permission = null;
            if (securedObject == null)
            {
                permission = new SystemPermission(permissionType);
            }
            else if(securedObject is OMSFile)
            {
                permission = new FilePermission((OMSFile)securedObject, permissionType);
            }
            else if (securedObject is Client)
            {
                permission = new ClientPermission((Client)securedObject, permissionType);
            }
            else if (securedObject is Contact)
            {
                permission = new ContactPermission((Contact)securedObject, permissionType);
            }
            else if (securedObject is Associate)
            {
                permission = new AssociatePermission((Associate)securedObject, permissionType);
            }
            else if (securedObject is OMSDocument)
            {
                permission = new DocumentPermission((OMSDocument)securedObject, permissionType);
            }
            else if (securedObject is Precedent)
            {
                permission = new PrecedentPermission((Precedent)securedObject, permissionType);
            }
            return permission;
        }

        public static string StandardTypeToString(StandardPermissionType type)
        {
            switch (type)
            {
                case StandardPermissionType.CreateAssociate:
                     return "CREATEASS";
                case StandardPermissionType.CreateClient:
                     return "CREATECLI";
                case StandardPermissionType.CreateContact:
                     return "CREATECON";
                case StandardPermissionType.SaveDocument:
                     return "SAVEDOC";
                case StandardPermissionType.CreateFile:
                     return "CREATEFIL";
                case StandardPermissionType.CreatePrecedent:
                    return "CREATEPREC";
                case StandardPermissionType.UpdatePermissions:
                    return "PERMISSIONS";
                case StandardPermissionType.ViewPermissions:
                    return "VPERMISSIONS";
                case StandardPermissionType.AdminKit:
                    return "ADMIN";
                case StandardPermissionType.ReportsViewer:
                    return "REPORTS";
                case StandardPermissionType.UpdateAssociate:
                    return "MODIFYASS";
                case StandardPermissionType.UpdateContact:
                    return "MODIFYCON";
                case StandardPermissionType.UpdateClient:
                    return "MODIFYCLI";
                case StandardPermissionType.UpdateFile:
                    return "MODIFYFIL";
                case StandardPermissionType.UpdateDocument:
                    return "MODIFYDOC";
                case StandardPermissionType.UpdatePrecedent:
                    return "PRECEDIT";
                case StandardPermissionType.DeleteAssociate:
                    return "DELASS";
                case StandardPermissionType.DeleteDocument:
                    return "DOCDELETED";
                case StandardPermissionType.List:
                    return "LIST";
                case StandardPermissionType.Read:
                    return "READ";
                case StandardPermissionType.Update:
                    return "UPDATE";
                case StandardPermissionType.Delete:
                    return "DELETE";
                case StandardPermissionType.SecurityAdmin:
                    return "SECADMIN";
            }

            return String.Empty;
        }

        public static StandardPermissionType StringToStandardType(string type)
        {
            switch (type)
            {
                case "CREATEASS":
                    return StandardPermissionType.CreateAssociate;
                case "CREATECLI":
                    return StandardPermissionType.CreateClient;
                case "CREATECON":
                    return StandardPermissionType.CreateContact;
                case "SAVEDOC":
                    return StandardPermissionType.SaveDocument;
                case "CREATEFIL":
                    return StandardPermissionType.CreateFile;
                case "CREATEPREC":
                    return StandardPermissionType.CreatePrecedent;
                case "PERMISSIONS":
                    return StandardPermissionType.UpdatePermissions;
                case "VPERMISSIONS":
                    return StandardPermissionType.ViewPermissions;
                case "ADMIN":
                    return StandardPermissionType.AdminKit;
                case "REPORTS":
                    return StandardPermissionType.ReportsViewer;
                case "MODIFYDOC":
                    return StandardPermissionType.UpdateDocument;
                case "MODIFYASS":
                    return StandardPermissionType.UpdateAssociate;
                case "MODIFYCON":
                    return StandardPermissionType.UpdateContact;
                case "MODIFYCLI":
                    return StandardPermissionType.UpdateClient;
                case "MODIFYFIL":
                    return StandardPermissionType.UpdateFile;
                case "PRECEDIT":
                    return StandardPermissionType.UpdatePrecedent;
                case "DELASS":
                    return StandardPermissionType.DeleteAssociate;
                case "DOCDELETED":
                    return StandardPermissionType.DeleteDocument;
                case "LIST":
                    return StandardPermissionType.List;
                case "READ":
                    return StandardPermissionType.Read;
                case "UPDATE":
                    return StandardPermissionType.Update;
                case "DELETE":
                    return StandardPermissionType.Delete;
                case "SECADMIN":
                    return StandardPermissionType.SecurityAdmin;
            }

            return StandardPermissionType.Unknown;
        }
    }
}
