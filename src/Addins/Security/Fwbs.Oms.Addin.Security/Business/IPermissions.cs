namespace FWBS.OMS.Addin.Security.Business
{
    interface IPermissions
    {
        string UpdatePermission();
        string PolicyID { get; set;}
        System.Data.DataTable Permissions { get;}

    }
}
