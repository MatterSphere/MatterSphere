using System;
using System.Data;
using System.Data.SqlClient;

namespace FWBS.OMS.OMSEXPORT
{
	/// <summary>
	/// Child class to export to Aderant.
	/// </summary>
    /// 

   
    
    public class OMSExportIGO : OMSExportBase , IDisposable	
	{
		//constructor calls base contstructor
		public OMSExportIGO():base()
		{}
		
		#region Fields
		/// <summary>
		/// Used to query by the base class
		/// </summary>
        private const string APPNAME = "IGO";
        /// <summary>
  
        // get the Indigo values from the registry
        string companyNo = StaticLibrary.GetSetting("CompanyNo","IGO","");
        string branchNo = StaticLibrary.GetSetting("BranchNo","IGO","");
        string computer = StaticLibrary.GetSetting("Computer","IGO","");
        string userCode = StaticLibrary.GetSetting("UserCode","IGO","");
        string programName = StaticLibrary.GetSetting("ProgramName","IGO","");
        string version = StaticLibrary.GetSetting("Version","IGO","");
        string message = StaticLibrary.GetSetting("Message","IGO","");
        int progID = 0;

        // This will hold the Program Log ID for the session
        //int progID = -1;

        //Opened up base property
        private SqlConnection _IGOcnn;

    public int GetProgramLogID()
    {
        //This has been removed as it is not necessary for this feature any more.  GM 22 Sept 2009
        return -1;
    }


		
		#endregion

		#region OMSExportBase Members

		protected override string ExportObject
		{
			get
			{
				return APPNAME;
			}
		}

 	
		
		/// <summary>
		/// Nothing to implement here for this project
		/// Most is the first time any work needs doing
		/// </summary>
        protected override void InitialiseProcess()
		{
            try
            {
                DataTable tbl = GetDataTable("exec sprIGOExportMatterTypes");
         
                // If there is data to export then get a program log ID
                if (tbl != null && progID == 0)
                    GetProgramLogID();

                if (_IGOcnn == null)
                    OpenIGOConnection();

                foreach (DataRow row in tbl.Rows)
                {

                    SqlCommand cmdIGO = new SqlCommand("FWBS_InsertMatterType", _IGOcnn);
                    cmdIGO.CommandType = CommandType.StoredProcedure;
                    cmdIGO.Parameters.Add(new SqlParameter("@P$CompanyNo", Convert.ToInt32(companyNo)));
                    cmdIGO.Parameters.Add(new SqlParameter("@P$WorkTypeCode", Convert.ToInt32(row["WorkTypeCode"])));
                    cmdIGO.Parameters.Add(new SqlParameter("@P$WorkTypeDescription", Convert.ToString(row["Description"])));
                    cmdIGO.Parameters.Add(new SqlParameter("@P$WorkTypeID", ""));
                    cmdIGO.ExecuteNonQuery();

                }
                 

                tbl = GetDataTable("exec sprIGOExportTimeActivity");

                // If there is data to export then get a program log ID
                if (tbl != null && progID == 0)
                    GetProgramLogID();

                if (_IGOcnn == null)
                    OpenIGOConnection();

                foreach (DataRow row in tbl.Rows)
                {
                    //Export Time Activities
                    SqlCommand cmdIGO2 = new SqlCommand("FWBS_InsertTimeActivity", _IGOcnn);
                    cmdIGO2.CommandType = CommandType.StoredProcedure;
                    cmdIGO2.Parameters.Add(new SqlParameter("@P$CompanyNo", Convert.ToInt32(companyNo)));
                    cmdIGO2.Parameters.Add(new SqlParameter("@P$ActivityCode", Convert.ToInt32(row["ActivityCode"])));
                    cmdIGO2.Parameters.Add(new SqlParameter("@P$Description", Convert.ToString(row["Description"])));
                    cmdIGO2.Parameters.Add(new SqlParameter("@P$Chargeable", Convert.ToBoolean(row["Chargeable"])));
                    cmdIGO2.Parameters.Add(new SqlParameter("@P$UseQuantity", Convert.ToBoolean(row["UseQuantity"])));
                    cmdIGO2.Parameters.Add(new SqlParameter("@P$ActivityID", ""));
                    cmdIGO2.ExecuteNonQuery();
                }

                
            }
            catch (Exception ex)
            {
                string s = "Initialisation: " + ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        /// <summary>
        /// Doesn'at actually export the user as we cannot do this so just locates the record
        /// </summary>
        /// <param name="row"></param>
        /// <returns>Indigo UserID</returns>
        protected override int ExportFeeEarner(DataRow row)
        {
            try
            {
                // If there is data to export then get a program log ID
                if (progID == 0)
                    GetProgramLogID();

                if (_IGOcnn == null)
                    OpenIGOConnection();

                int feeEarnerID;

                // Export Fee Earners
                SqlCommand cmd = new SqlCommand("FWBS_InsertFeeEarner", _IGOcnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@P$CompanyNo", Convert.ToInt32(companyNo)));
                cmd.Parameters.Add(new SqlParameter("@P$BranchNo", Convert.ToString(row["BranchNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerCode", Convert.ToString(row["FeeEarnerCode"])));
                cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerName", Convert.ToString(row["FeeEarnerName"])));
                cmd.Parameters.Add(new SqlParameter("@P$Grade", Convert.ToInt32(row["Grade"])));
                cmd.Parameters.Add(new SqlParameter("@P$MinsPerUnit", Convert.ToInt32(row["MinsPerUnit"])));
                cmd.Parameters.Add(new SqlParameter("@P$IsPartner", Convert.ToBoolean(row["IsPartner"])));
                cmd.Parameters.Add(new SqlParameter("@P$CostRate", Convert.ToDecimal(row["CostRate"])));
                cmd.Parameters.Add(new SqlParameter("@P$ChargeRate", Convert.ToDecimal(row["ChargeRate"])));
                cmd.Parameters.Add(new SqlParameter("@P$DailyTargetTime", Convert.ToInt32(row["DailyTargetTime"])));
                cmd.Parameters.Add(new SqlParameter("@P$DailyTargetValue", Convert.ToDecimal(row["DailyTargetValue"])));
                cmd.Parameters.Add(new SqlParameter("@P$UserCode", Convert.ToString(row["UserCode"])));
                //If fee earner already exists, could be updated in the future
                if (row["FEEEARNERID"] == DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerID", SqlDbType.Int));
                else
                    cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerID", Convert.ToInt32(row["FEEEARNERID"])));
                cmd.Parameters["@P$FeeEarnerID"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                feeEarnerID = (int)cmd.Parameters["@P$FeeEarnerID"].Value;

                return feeEarnerID;
            }
            catch (Exception ex)
            {
                string s = "ExportFeeEarner: " + ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        /// <summary>
        /// Doesn'at actually export the FeeEarner as we cannot do this so just locates the record
        /// </summary>
        /// <param name="row"></param>
        /// <returns>Indigo FeeEarnerID</returns>
        protected override int ExportUser(DataRow row)
        {
            try
            {
                // If there is data to export then get a program log ID
                if (progID == 0)
                    GetProgramLogID();

                if (_IGOcnn == null)
                    OpenIGOConnection();

                int userID;

                // Export Fee Earners
                SqlCommand cmd = new SqlCommand("FWBS_InsertUser", _IGOcnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@P$UserCode", Convert.ToString(row["UserCode"])));
                cmd.Parameters.Add(new SqlParameter("@P$UserName", Convert.ToString(row["UserFullName"])));
                cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerCode", Convert.ToString(row["FeeEarnerCode"])));

                //If fee earner already exists, could be updated in the future
                if (row["USERID"] == DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$USERID", SqlDbType.Int));
                else
                    cmd.Parameters.Add(new SqlParameter("@P$USERID", Convert.ToInt32(row["USERID"])));
                cmd.Parameters["@P$USERID"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                userID = (int)cmd.Parameters["@P$USERID"].Value;

                return userID;
            }
            catch (Exception ex)
            {
                string s = "ExportUser: " + ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }
        

        /// <summary>
		/// Exports an individual client record
		/// </summary>
		/// <param name="row">Data row from client query</param>
		/// <returns>Indigo ClientID</returns>
		protected override object ExportClient(System.Data.DataRow row)
		{

			try
			{
                
                // If there is data to export then get a program log ID
                if (progID == 0)
                    GetProgramLogID();

                if (_IGOcnn == null)
                    OpenIGOConnection();

                SqlCommand cmd = new SqlCommand("FWBS_InsertClient2", _IGOcnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@P$CompanyNo", Convert.ToInt32(companyNo)));
                cmd.Parameters.Add(new SqlParameter("@P$BranchNo", Convert.ToInt32(branchNo)));
                cmd.Parameters.Add(new SqlParameter("@P$ClientNo", Convert.ToString(row["ClientNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$ClientName", Convert.ToString(row["ClientName"])));
                cmd.Parameters.Add(new SqlParameter("@P$ClientKey", Convert.ToString(row["ClientKey"])));
                cmd.Parameters.Add(new SqlParameter("@P$CompanyName", Convert.ToString(row["CompanyName"])));
                cmd.Parameters.Add(new SqlParameter("@P$PartnerID", Convert.ToInt32(row["PartnerID"])));
                cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerID", Convert.ToInt32(row["FeeEarnerID"])));
                if (row["DateOfBirth"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$DateOfBirth", Convert.ToDateTime(row["DateOfBirth"])));
                cmd.Parameters.Add(new SqlParameter("@P$NINumber", Convert.ToString(row["NINumber"])));
                cmd.Parameters.Add(new SqlParameter("@P$Salutation", Convert.ToString(row["Salutation"])));
                cmd.Parameters.Add(new SqlParameter("@P$NextOfKin", Convert.ToString(row["NextOfKin"])));
                cmd.Parameters.Add(new SqlParameter("@P$ClientType", Convert.ToString(row["ClientType"])));
                cmd.Parameters.Add(new SqlParameter("@P$BusinessSource", Convert.ToString(row["BusinessSource"])));
                cmd.Parameters.Add(new SqlParameter("@P$MailShot", Convert.ToBoolean(row["MailShot"])));
                cmd.Parameters.Add(new SqlParameter("@P$TermsOfEngagement", Convert.ToBoolean(row["TermsOfEngagement"])));
                cmd.Parameters.Add(new SqlParameter("@P$AssociatedClient", Convert.ToString(row["AssociatedClient"])));
                cmd.Parameters.Add(new SqlParameter("@P$ConflictSearchDone", Convert.ToBoolean(row["ConflictSearchDone"])));
                cmd.Parameters.Add(new SqlParameter("@P$ConflictingClients", Convert.ToString(row["ConflictingClients"])));
                cmd.Parameters.Add(new SqlParameter("@P$ConnectedClients", Convert.ToString(row["ConnectedClients"])));
                cmd.Parameters.Add(new SqlParameter("@P$CreditControl", Convert.ToBoolean(row["CreditControl"])));
                cmd.Parameters.Add(new SqlParameter("@P$MoneyLaunderingChecked", Convert.ToBoolean(row["MoneyLaunderingChecked"])));
                cmd.Parameters.Add(new SqlParameter("@P$MoneyLaunderingNotes", Convert.ToString(row["MoneyLaunderingNotes"])));
                if (row["Addressee"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$Addressee", Convert.ToString(row["Addressee"])));
                cmd.Parameters.Add(new SqlParameter("@P$AddressLine1", Convert.ToString(row["AddressLine1"])));
                cmd.Parameters.Add(new SqlParameter("@P$AddressLine2", Convert.ToString(row["AddressLine2"])));
                cmd.Parameters.Add(new SqlParameter("@P$Town", Convert.ToString(row["Town"])));
                cmd.Parameters.Add(new SqlParameter("@P$County", Convert.ToString(row["County"])));
                cmd.Parameters.Add(new SqlParameter("@P$PostCode", Convert.ToString(row["PostCode"])));
                cmd.Parameters.Add(new SqlParameter("@P$PhoneNo", Convert.ToString(row["PhoneNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$MobileNo", Convert.ToString(row["MobileNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$FaxNo", Convert.ToString(row["FaxNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$EmailAddress", Convert.ToString(row["EmailAddress"])));
                cmd.Parameters.Add(new SqlParameter("@P$ClientID", SqlDbType.Int));
                //Added 18/7/07 by SJ
                cmd.Parameters.Add(new SqlParameter("@P$DateOpened", Convert.ToDateTime(row["DateOpened"])));
                cmd.Parameters["@P$ClientID"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int clientID = (int)cmd.Parameters["@P$ClientID"].Value;

               return clientID;
             
			}
			catch(Exception ex)
			{
				string s = "ExportClient: " + ex.Message;
				Exception enew = new Exception(s);
				throw enew;
			}
			
		}
		
				
		/// <summary>
		/// Updates the individual client record
		/// </summary>
		/// <param name="row">clinet data row</param>
		/// <returns>true if successful</returns>
		protected override bool UpdateClient(DataRow row)
		{

            try
            {

                // If there is data to export then get a program log ID
                if (progID == 0)
                    GetProgramLogID();

                if (_IGOcnn == null)
                    OpenIGOConnection();

                SqlCommand cmd = new SqlCommand("FWBS_UpdateClient2", _IGOcnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@P$CompanyNo", Convert.ToInt32(companyNo)));
                cmd.Parameters.Add(new SqlParameter("@P$BranchNo", Convert.ToInt32(branchNo)));
                cmd.Parameters.Add(new SqlParameter("@P$ClientNo", Convert.ToString(row["ClientNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$ClientName", Convert.ToString(row["ClientName"])));
                cmd.Parameters.Add(new SqlParameter("@P$ClientKey", Convert.ToString(row["ClientKey"])));
                cmd.Parameters.Add(new SqlParameter("@P$CompanyName", Convert.ToString(row["CompanyName"])));
                cmd.Parameters.Add(new SqlParameter("@P$PartnerID", Convert.ToInt32(row["PartnerID"])));
                cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerID", Convert.ToInt32(row["FeeEarnerID"])));
                if (row["DateOfBirth"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$DateOfBirth", Convert.ToDateTime(row["DateOfBirth"])));
                cmd.Parameters.Add(new SqlParameter("@P$NINumber", Convert.ToString(row["NINumber"])));
                cmd.Parameters.Add(new SqlParameter("@P$Salutation", Convert.ToString(row["Salutation"])));
                cmd.Parameters.Add(new SqlParameter("@P$NextOfKin", Convert.ToString(row["NextOfKin"])));
                cmd.Parameters.Add(new SqlParameter("@P$ClientType", Convert.ToString(row["ClientType"])));
                cmd.Parameters.Add(new SqlParameter("@P$BusinessSource", Convert.ToString(row["BusinessSource"])));
                cmd.Parameters.Add(new SqlParameter("@P$MailShot", Convert.ToBoolean(row["MailShot"])));
                cmd.Parameters.Add(new SqlParameter("@P$TermsOfEngagement", Convert.ToBoolean(row["TermsOfEngagement"])));
                cmd.Parameters.Add(new SqlParameter("@P$AssociatedClient", Convert.ToString(row["AssociatedClient"])));
                cmd.Parameters.Add(new SqlParameter("@P$ConflictSearchDone", Convert.ToBoolean(row["ConflictSearchDone"])));
                cmd.Parameters.Add(new SqlParameter("@P$ConflictingClients", Convert.ToString(row["ConflictingClients"])));
                cmd.Parameters.Add(new SqlParameter("@P$ConnectedClients", Convert.ToString(row["ConnectedClients"])));
                cmd.Parameters.Add(new SqlParameter("@P$CreditControl", Convert.ToBoolean(row["CreditControl"])));
                cmd.Parameters.Add(new SqlParameter("@P$MoneyLaunderingChecked", Convert.ToBoolean(row["MoneyLaunderingChecked"])));
                cmd.Parameters.Add(new SqlParameter("@P$MoneyLaunderingNotes", Convert.ToString(row["MoneyLaunderingNotes"])));
                if (row["Addressee"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$Addressee", Convert.ToString(row["Addressee"])));
                cmd.Parameters.Add(new SqlParameter("@P$AddressLine1", Convert.ToString(row["AddressLine1"])));
                cmd.Parameters.Add(new SqlParameter("@P$AddressLine2", Convert.ToString(row["AddressLine2"])));
                cmd.Parameters.Add(new SqlParameter("@P$Town", Convert.ToString(row["Town"])));
                cmd.Parameters.Add(new SqlParameter("@P$County", Convert.ToString(row["County"])));
                cmd.Parameters.Add(new SqlParameter("@P$PostCode", Convert.ToString(row["PostCode"])));
                cmd.Parameters.Add(new SqlParameter("@P$PhoneNo", Convert.ToString(row["PhoneNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$MobileNo", Convert.ToString(row["MobileNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$FaxNo", Convert.ToString(row["FaxNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$EmailAddress", Convert.ToString(row["EmailAddress"])));
                //Added 18/7/07 by SJ
                cmd.Parameters.Add(new SqlParameter("@P$DateOpened", Convert.ToDateTime(row["DateOpened"])));
                cmd.ExecuteNonQuery();

               
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateClient failed for client ID " + Convert.ToString(row["clID"]) +
                    " with ID of " + "" + " Error: " + ex.Message);
            }
		}
        protected override object ExportContact(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateContact(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
		/// Inserts a new matter record into Indigo system
		/// </summary>
		/// <param name="row">Data row of matter</param>
		/// <returns>Indigo MatterID</returns>
		protected override object ExportMatter(DataRow row)
		{
			try
			{
                // If there is data to export then get a program log ID
                if (progID == 0)
                    GetProgramLogID();

                if (_IGOcnn == null)
                    OpenIGOConnection();

                SqlCommand cmd = new SqlCommand("FWBS_InsertMatter2", _IGOcnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@P$CompanyNo", Convert.ToInt32(companyNo)));
                cmd.Parameters.Add(new SqlParameter("@P$BranchNo", Convert.ToString(row["BranchNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$ClientNo", Convert.ToString(row["ClientNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$MatterNo", Convert.ToInt32(row["MatterNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$MatterDescription", Convert.ToString(row["MatterDescription"])));
                cmd.Parameters.Add(new SqlParameter("@P$PartnerID", Convert.ToInt32(row["PartnerID"])));
                cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerID", Convert.ToInt32(row["FeeEarnerID"])));
                if (row["LegalAidCategory"] != DBNull.Value)
                  cmd.Parameters.Add(new SqlParameter("@P$LegalAidCategory", Convert.ToInt32(row["LegalAidCategory"])));
                if (row["LegalAidDate"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$LegalAidDate", Convert.ToDateTime(row["LegalAidDate"]).ToLocalTime()));
                cmd.Parameters.Add(new SqlParameter("@P$WorkTypeCode", Convert.ToInt32(row["WorkTypeCode"])));
                cmd.Parameters.Add(new SqlParameter("@P$UserNotes", Convert.ToString(row["UserNotes"])));
                cmd.Parameters.Add(new SqlParameter("@P$MatterID", SqlDbType.Int));
                //Added 18/07/07 SJ
                cmd.Parameters.Add(new SqlParameter("@P$DateOpened", Convert.ToDateTime(row["DateOpened"])));
                if (row["OfficeBankNo"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$OfficeBankNo", Convert.ToInt32(row["OfficeBankNo"])));
                if (row["ClientBankNo"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$ClientBankNo", Convert.ToInt32(row["ClientBankNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$ChargeRate", Convert.ToDecimal(row["ChargeRate"])));
                cmd.Parameters.Add(new SqlParameter("@P$ZeroChargeRate", Convert.ToBoolean(row["ZeroChargeRate"])));
                cmd.Parameters.Add(new SqlParameter("@P$TotalCreditWarning", Convert.ToDecimal(row["TotalCreditWarning"])));
                cmd.Parameters.Add(new SqlParameter("@P$TotalCreditLimit", Convert.ToDecimal(row["TotalCreditLimit"])));
                cmd.Parameters.Add(new SqlParameter("@P$WIPCreditWarning", Convert.ToDecimal(row["WIPCreditWarning"])));
                cmd.Parameters.Add(new SqlParameter("@P$WIPCreditLimit", Convert.ToDecimal(row["WIPCreditLimit"])));
                cmd.Parameters.Add(new SqlParameter("@P$EstimatedCosts", Convert.ToDecimal(row["EstimatedCosts"])));
                //New Parameters added 23-Sept-2009 by GM for V2 Indigo Procs
                cmd.Parameters.Add(new SqlParameter("@P$FundType", Convert.ToString(row["FundType"])));
                cmd.Parameters.Add(new SqlParameter("@P$Alert", Convert.ToString(row["Alert"])));
                cmd.Parameters["@P$MatterID"].Direction = ParameterDirection.Output;
                
                //New for grindeys 1/11/2011 only add if columns exist
                if (row.Table.Columns["MatterKey"] != null)
                    cmd.Parameters.Add(new SqlParameter("P$MatterKey", Convert.ToString(row["MatterKey"])));
                
                cmd.ExecuteNonQuery();

                int matterID = (int)cmd.Parameters["@P$MatterID"].Value;
               
                return matterID;
			}
			catch(Exception ex)
			{
				string s = "ExportMatter: " + ex.Message;
				Exception enew = new Exception(s);
				throw enew;
			}

		}
		
				
		/// <summary>
		/// Updates matter in Indigo where changed in OMS
		/// </summary>
		/// <param name="row">Data row for the matter</param>
		/// <returns>True if updated OK</returns>
		protected override bool UpdateMatter(DataRow row)
		{
            try
            {

                // If there is data to export then get a program log ID
                if (progID == 0)
                    GetProgramLogID();

                if (_IGOcnn == null)
                    OpenIGOConnection();


                SqlCommand cmd = new SqlCommand("FWBS_UpdateMatter2", _IGOcnn);
                cmd.CommandType = CommandType.StoredProcedure;
                //Always pass through these 4 parameters as they are required
                cmd.Parameters.Add(new SqlParameter("@P$CompanyNo", Convert.ToInt32(companyNo)));
                cmd.Parameters.Add(new SqlParameter("@P$BranchNo", Convert.ToString(row["BranchNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$ClientNo", Convert.ToString(row["ClientNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$MatterNo", Convert.ToInt32(row["MatterNo"])));
                if (row["MatterDescription"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$MatterDescription", Convert.ToString(row["MatterDescription"])));
                if (row["PartnerID"] != DBNull.Value)
                cmd.Parameters.Add(new SqlParameter("@P$PartnerID", Convert.ToInt32(row["PartnerID"])));
                if (row["FeeEarnerID"] != DBNull.Value)
                cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerID", Convert.ToInt32(row["FeeEarnerID"])));
                if (row["LegalAidCategory"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$LegalAidCategory", Convert.ToInt32(row["LegalAidCategory"])));
                if (row["LegalAidDate"] != DBNull.Value)
                     cmd.Parameters.Add(new SqlParameter("@P$LegalAidDate", Convert.ToDateTime(row["LegalAidDate"]).ToLocalTime()));
                if (row["WorkTypeCode"] != DBNull.Value)
                cmd.Parameters.Add(new SqlParameter("@P$WorkTypeCode", Convert.ToInt32(row["WorkTypeCode"])));
                if (row["UserNotes"] != DBNull.Value)
                cmd.Parameters.Add(new SqlParameter("@P$UserNotes", Convert.ToString(row["UserNotes"])));
                
                

                //Added 18/07/07 SJ
                if (row["DateOpened"] != DBNull.Value)
                cmd.Parameters.Add(new SqlParameter("@P$DateOpened", Convert.ToDateTime(row["DateOpened"])));
                if (row["OfficeBankNo"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$OfficeBankNo", Convert.ToInt32(row["OfficeBankNo"])));
                if (row["ClientBankNo"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$ClientBankNo", Convert.ToInt32(row["ClientBankNo"])));
                if (row["ChargeRate"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$ChargeRate", Convert.ToDecimal(row["ChargeRate"])));
                if (row["ZeroChargeRate"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$ZeroChargeRate", Convert.ToBoolean(row["ZeroChargeRate"])));
                if (row["TotalCreditWarning"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$TotalCreditWarning", Convert.ToDecimal(row["TotalCreditWarning"])));
                if (row["TotalCreditLimit"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$TotalCreditLimit", Convert.ToDecimal(row["TotalCreditLimit"])));
                if (row["WIPCreditWarning"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$WIPCreditWarning", Convert.ToDecimal(row["WIPCreditWarning"])));
                if (row["WIPCreditLimit"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$WIPCreditLimit", Convert.ToDecimal(row["WIPCreditLimit"])));
                if (row["EstimatedCosts"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$EstimatedCosts", Convert.ToDecimal(row["EstimatedCosts"])));
                //New Parameters added 23-Sept-2009 by GM for V2 Indigo Procs
                if (row["FundType"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$FundType", Convert.ToString(row["FundType"])));
                if (row["Alert"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$Alert", Convert.ToString(row["Alert"])));

                //New for grindeys 1/11/2011 only add if columns exist
                if (row.Table.Columns["MatterKey"] != null)
                    cmd.Parameters.Add(new SqlParameter("P$MatterKey", Convert.ToString(row["MatterKey"])));


                //Output parameters
                cmd.Parameters.Add(new SqlParameter("@P$MatterID", SqlDbType.Int));
                cmd.Parameters["@P$MatterID"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
       

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateMatter failed for matter ID " + Convert.ToString(row["fileID"]) +
                    " with ID of " + "" + " Error: " + ex.Message);
            }
			
		}


		
		
		/// <summary>
		/// Exports time entry record
		/// </summary>
		/// <param name="row">Time entry data row</param>
		/// <returns>Does not return anything</returns>
		protected override object ExportTimeRecord(DataRow row)
		{
			try
			{
                
                // If there is data to export then get a program log ID
                if (progID == 0)
                    GetProgramLogID();


                if (_IGOcnn == null)
                    OpenIGOConnection();

                SqlCommand cmd = new SqlCommand("FWBS_InsertTimeRecord2", _IGOcnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@P$CompanyNo", Convert.ToInt32(companyNo)));
                cmd.Parameters.Add(new SqlParameter("@P$BranchNo", Convert.ToInt32(branchNo)));
                cmd.Parameters.Add(new SqlParameter("@P$ClientNo", Convert.ToString(row["ClientNo"])));
                cmd.Parameters.Add(new SqlParameter("@P$MatterNo", Convert.ToInt32(row["MatterNo"])));
                if (row["PostingDate"] != DBNull.Value)
                  cmd.Parameters.Add(new SqlParameter("@P$PostingDate", Convert.ToDateTime(row["PostingDate"]).ToLocalTime()));
                cmd.Parameters.Add(new SqlParameter("@P$YearNo", 0));
                cmd.Parameters.Add(new SqlParameter("@P$PeriodNo", 0));
                cmd.Parameters.Add(new SqlParameter("@P$ProgramLogID", progID));
                cmd.Parameters.Add(new SqlParameter("@P$FeeEarnerID", Convert.ToInt32(row["FeeEarnerID"])));
                cmd.Parameters.Add(new SqlParameter("@P$ActivityCode", Convert.ToInt32(row["ActivityCode"])));
                if (row["Minutes"] != DBNull.Value)
                    cmd.Parameters.Add(new SqlParameter("@P$Minutes", Convert.ToInt32(row["Minutes"])));
                if (row["Quantity"] != DBNull.Value)
                 cmd.Parameters.Add(new SqlParameter("@P$Quantity", Convert.ToInt32(row["Quantity"])));
                if (row["Units"] != DBNull.Value)
                  cmd.Parameters.Add(new SqlParameter("@P$Units", Convert.ToInt32(row["Units"])));
                cmd.Parameters.Add(new SqlParameter("@P$CostRate", Convert.ToDecimal(row["CostRate"])));
                if (row["CostValue"] != DBNull.Value)
                  cmd.Parameters.Add(new SqlParameter("@P$CostValue", Convert.ToDecimal(row["CostValue"])));
                cmd.Parameters.Add(new SqlParameter("@P$ChargeRate", Convert.ToDecimal(row["ChargeRate"])));
                    if (row["ChargeValue"] != DBNull.Value)
                cmd.Parameters.Add(new SqlParameter("@P$ChargeValue", Convert.ToDecimal(row["ChargeValue"])));
                cmd.Parameters.Add(new SqlParameter("@P$Narrative", Convert.ToString(row["Narrative"])));
                if (row["TimeStarted"] != DBNull.Value)
                  cmd.Parameters.Add(new SqlParameter("@P$TimeStarted", Convert.ToDateTime(row["TimeStarted"]).ToLocalTime()));
                if (row["TimeEnded"] != DBNull.Value)
                   cmd.Parameters.Add(new SqlParameter("@P$TimeEnded", Convert.ToDateTime(row["TimeEnded"]).ToLocalTime()));
                
                //New parameter to avoid issue where update fails - Indigo Stored proc
                cmd.Parameters.Add(new SqlParameter("@P$TimeLedgerID", Convert.ToInt64(row["ID"])));
                //Output parameter
                cmd.Parameters.Add(new SqlParameter("@P$TimeTransID", SqlDbType.Int));
                cmd.Parameters["@P$TimeTransID"].Direction = ParameterDirection.Output;
                //Now execute the query
                cmd.ExecuteNonQuery();

                int timeID = (int)cmd.Parameters["@P$TimeTransID"].Value;

				return timeID;
			}
			catch(Exception ex)
			{
				string s = "ExportTimeRecord: " + ex.Message;
				Exception enew = new Exception(s);
				throw enew;
			}
		}
		
		
		/// <summary>
		/// Export financial record
		/// </summary>
		/// <param name="row">Financial record</param>
		/// <returns>ID of record NOTE: No implementation at the moment so retuns 0</returns>
		protected override object ExportFinancialRecord(DataRow row)
		{
            try
            {
                
                if (progID == 0)                
                    GetProgramLogID();

                if (_IGOcnn == null)
                    OpenIGOConnection();
                               
                //Export EChit
                SqlCommand cmdIGO = new SqlCommand("FWBS_InsertEChit2", _IGOcnn);
                cmdIGO.CommandType = CommandType.StoredProcedure;
                cmdIGO.Parameters.Add(new SqlParameter("@P$CompanyNo", Convert.ToInt32(companyNo)));
                cmdIGO.Parameters.Add(new SqlParameter("@P$BranchNo", Convert.ToInt32(branchNo)));
                cmdIGO.Parameters.Add(new SqlParameter("@P$ClientNo", Convert.ToString(row["ClientNo"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$MatterNo", Convert.ToInt32(row["MatterNo"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$PostingDate", Convert.ToDateTime(row["PostingDate"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$YearNo", Convert.ToInt32(row["YearNo"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$PeriodNo", Convert.ToInt32(row["PeriodNo"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$ProgramLogID", progID));
                cmdIGO.Parameters.Add(new SqlParameter("@P$PostingType", Convert.ToString(row["PostingType"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$UserID", Convert.ToInt32(row["UserID"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$FeeEarnerID", Convert.ToInt32(row["FeeEarnerID"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$Reference", Convert.ToString(row["Reference"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$Narrative", Convert.ToString(row["Narrative"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$OtherTextType", Convert.ToInt32(row["OtherTextType"])));

                cmdIGO.Parameters.Add(new SqlParameter("@P$NetValue", Convert.ToDecimal(row["NetValue"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$VATValue", Convert.ToDecimal(row["VATValue"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$Notes", Convert.ToString(row["Notes"])));
                cmdIGO.Parameters.Add(new SqlParameter("@P$TransID", SqlDbType.Int));
                //Added next line for Authorised Exports 23 Sept 2009 by GM
                cmdIGO.Parameters.Add(new SqlParameter("@P$AuthorisedUserID", Convert.ToInt32(row["AuthorisedUserID"])));
                cmdIGO.Parameters["@P$TransID"].Direction = ParameterDirection.Output;

                //this is being used at grindeys for the bank Account name 1/11/2011
                cmdIGO.Parameters.Add(new SqlParameter("@P$OtherText", Convert.ToString(row["OtherText"])));

                //New for grindeys 1/11/2011 only add if columns exist
                if (row.Table.Columns["BankSortCode"] != null)
                    cmdIGO.Parameters.Add(new SqlParameter("P$BankSortCode", Convert.ToString(row["BankSortCode"])));
                if (row.Table.Columns["BankAccountNo"] != null)
                    cmdIGO.Parameters.Add(new SqlParameter("P$BankAccountNo", Convert.ToString(row["BankAccountNo"])));

                //New for Conveyancing Direct 3/2/2015 only add if column exist
                if (row.Table.Columns["ExportUser"] != null)
                    cmdIGO.Parameters.Add(new SqlParameter("@P$ExportUser", Convert.ToString(row["ExportUser"])));

                
                cmdIGO.ExecuteNonQuery();
                int extID = (int)cmdIGO.Parameters["@P$TransID"].Value;

                if (extID >= 0)
                {
                    return extID;
                }
                else
                {
                    throw new ApplicationException(Convert.ToString(extID));
                }
            }
            
            catch (Exception ex)
            {
                string s = "Export Financials: " + ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }			
		}

        /// <summary>
        /// Not Needed for Indigo yet
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override string ExportLookup(DataRow row)
        {
            return "";
        }

        /// <summary>
        /// performa any finishing off
        /// </summary>
        protected override void FinaliseProcess()
        {
            try
            {
               //perform any required closing of connections etc.
                RaiseStatusEvent("Finalising Process");

                if (_IGOcnn != null)
                {
                    if (_IGOcnn.State == ConnectionState.Open)
                        _IGOcnn.Close();

                    _IGOcnn.Dispose();
                    _IGOcnn = null;
                }
            }
            catch (Exception ex)
            {
                string s = "Finalising Process: " + ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
            finally
            {
                _IGOcnn = null;
            }
        }
		#endregion
				
		#region IDisposable Members
		
		/// <summary>
		/// Nulls all the web servce references
		/// </summary>
		new public void Dispose()
		{
			base.Dispose();
		}

		#endregion

        #region Private methods

        /// <summary>
        /// Opens a connection to the Indigo Database
        /// </summary>
        private void OpenIGOConnection()
        {

            // Open a connection to the OMS Database and check for the table fdIndigoPeriods
            RaiseStatusEvent("Opening IGO Database connection.");
            _IGOcnn = new SqlConnection(StaticLibrary.IGOConnectionString());
            _IGOcnn.Open();
            RaiseStatusEvent("Database connection open.");
        
        



        }

        #endregion
    }
}
