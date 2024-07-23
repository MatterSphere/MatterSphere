using MsAdUsersSyncService.MSADSync;
using Nancy;
using Nancy.ModelBinding;

namespace MsAdUsersSyncService.Web
{
    public class GroupServiceModule : NancyModule
    {
        public GroupServiceModule() : base("/GroupService")
        {
            Post["/"] = parameters =>
            {
                GroupServiceInfo groupServiceInfo = this.Bind<GroupServiceInfo>();

                string distinguishedName = groupServiceInfo.DistinguishedName;
                string defaultRoot = groupServiceInfo.DefaultRoot;
                bool isAAD = Config.GetConfigurationItem("MatterSphereLoginType") == "AAD";
                ADMS adms = new ADMS();

                string root = adms.GetDomainName(defaultRoot);

                var response = new
                {
                    Root = root,
                    GroupName = adms.GetActiveDirectoryGroupName(distinguishedName),
                    GroupUsersXml = adms.GetGroupUsersXml(distinguishedName, root, isAAD)
                };

                return this.Response.AsJson(response);
            };
        }
    }
}