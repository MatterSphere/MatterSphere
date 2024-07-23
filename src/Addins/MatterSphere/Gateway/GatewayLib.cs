using System;
using System.Data;
using FWBS.MatterSphereIntegration.Gateway;

namespace FWBS.MatterSphereIntegration
{
    public class GatewayLib
    {
        public static DataTable ListFeeEarners(string EndPointAddress, string Passcode, long CompanyID, string UserName)
        {
            var n = new GatewayNotifications(EndPointAddress, Passcode, CompanyID, UserName);

            var response = n.ListFeeEarners();

            if (response.Success)
            {
                DataTable tbl = new DataTable("FeeEarners");
                tbl.Columns.Add("UserEmail", typeof(string));
                tbl.Columns.Add("DeviceType", typeof(string));
                tbl.Columns.Add("SerialNumber", typeof(string));
                tbl.Columns.Add("Granted", typeof(Boolean));


                foreach (FeeEarnerDetails f in response.FeeEarners)
                {
                    DataRow row = tbl.NewRow();

                    row["UserEmail"] = f.UserEmail;
                    row["DeviceType"] = f.Device.DeviceType;
                    row["SerialNumber"] = f.Device.SerialNumber;
                    row["Granted"] = f.Device.Granted;

                    tbl.Rows.Add(row);
                }

                tbl.AcceptChanges();

                return tbl;

            }
            else
            {
                throw new Exception(response.Message);
            }
        }

        internal static AccountDetails GetAccountDetails(string EndPointAddress, string Passcode, long CompanyID, string UserName)
        {
            var gateway = new GatewayNotifications(EndPointAddress, Passcode, CompanyID, UserName);
            return gateway.GetCompanyAccountDetails();
        }

        internal static AdminResponse StoreAccountDetails(string EndPointAddress, string Passcode, long CompanyID, string UserName, AccountDetails details)
        {
            var gateway = new GatewayNotifications(EndPointAddress, Passcode, CompanyID, UserName);
            return gateway.UpdateCompanyAccountDetails(details);
        }
    }

}
