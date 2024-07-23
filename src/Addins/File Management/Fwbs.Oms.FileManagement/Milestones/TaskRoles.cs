namespace FWBS.OMS.FileManagement.Milestones
{
    public static class TaskRoles
    {
        public const string MilestonePlanReset = "MSPLANRESET";
        public const string ApplicationReset = "FMAPPRESET";


        public const string DeleteAny = "TASKDELATEALL";
        public const string DeleteTeam = "TASKDELETETEAM";
        public const string UpdateAny = "TASKUPDATEALL";
        public const string UpdateTeam = "TASKUPDATETEAM";
        public const string AddManual = "TASKADDMANUAL";

        public static bool IsInRole(string role)
        {
            System.Data.DataView vw = new System.Data.DataView(CodeLookup.GetLookups("USRROLES"));
            vw.RowFilter = string.Format("cdcode = '{0}'", role.Replace("'", "''"));
            if (vw.Count == 0)
                return true;
            else
                return Session.CurrentSession.CurrentUser.IsInRoles(role);          

        }
    }
}
