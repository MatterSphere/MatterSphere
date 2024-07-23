using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS;
using FWBS.OMS.Addin.Security;
using FWBS.OMS.Interfaces;
using FWBS.OMS.Security;
using FWBS.OMS.UI.Windows;
using MsgBox = FWBS.OMS.UI.Windows.MessageBox;

namespace FWBS.MatterSphereIntegration
{


    public class RemoteAccount
    {

        public static void EditSecurity(ISecurable securable, long contactId)
        {
            if (securable == null)
                throw new ArgumentNullException("omsObject");

            Contact contact = Contact.GetContact(contactId);
            User user = GetUserForContact(contact);

            if (user == null)
                return;

            string accessType = Convert.ToString(user.GetExtraInfo("AccessType"));
            if (!(accessType.Equals("EXTERNAL",StringComparison.InvariantCultureIgnoreCase)))
            {
                MsgBox.ShowInformation("MSSECEXTONLY","The selected user is an internal user. To edit internal users please use the security tab.");
                return;
            }

            if (contact.User == null || contact.User.ID != user.ID)
            {
                contact.User = user;
                contact.Update();
            }

            string currentPolicy = GetCurrentSecurityPolicy(user, securable);
            string description = GetDescription(securable);
            DataTable policies = GetPolicies();
            Guid securityId = GetSecurityID(user);

            KeyValueCollection kvc = new KeyValueCollection();
            kvc.Add("description", description);
            kvc.Add("securable", securable);
            kvc.Add("policies", policies);
            kvc.Add("currentpolicy", currentPolicy);
            kvc.Add("securityid", securityId);
            kvc.Add("user", user);

            DataTable dt = FWBS.OMS.UI.Windows.Services.ShowOMSItem("SCRREMACCSECV3", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, kvc) as DataTable;

            if (dt != null)
            {
                string newPolicyID = Convert.ToString(dt.Rows[0]["NewPoliciesList"]);
                if (string.IsNullOrEmpty(newPolicyID) && !string.IsNullOrEmpty(currentPolicy))
                    throw new OMSException2("MSSECNULLPOLICY", "You cannot set the policy to No Access after a policy has been applied. Please select a policy which denies the user access.");

                UpdatePermissions(securable, user, currentPolicy, newPolicyID, securityId);
                     
                //CM 260215 - All Internal group management (WI.7318)
                Permissions perms = new Permissions(securable as IExtraInfo);
                DataTable dtParties = perms.ListParties();
                if (dtParties != null)
                {                    
                    if (hasExternalUsersOnly(dtParties) || hasExternalUsersAndSelectedMattersOnly(dtParties))
                        AddAllInternalGroup(perms);
                }
            }
        }


        private static bool hasExternalUsersAndSelectedMattersOnly(DataTable dtParties)
        {
            return dtParties.Rows.Count > 0 && dtParties.Select("AccessType = 'EXTERNAL'").Length > 0 && (dtParties.Rows.Count == NumberOfSelectedMattersOnlyItems(dtParties) + dtParties.Select("AccessType = 'EXTERNAL'").Length);
        }
       
 
        private static bool hasExternalUsersOnly(DataTable dtParties)
        {
            return dtParties.Select("AccessType = 'EXTERNAL'").Length > 0 && dtParties.Select("AccessType = 'INTERNAL'").Length == 0;
        }
        
       
        private static void AddAllInternalGroup(Permissions perms)
        {
            perms.AccessType = "INTERNAL";
            perms.Refresh();
            perms.AddPermission("DF08633D-9262-489F-B7F1-9A4FC56B41BC;AllInternal;51");
            perms.UpdatePermission();
        }
        

        private static int NumberOfSelectedMattersOnlyItems(DataTable Parties)
        {
            if (!Parties.Columns.Contains("SelectedMattersOnly"))
                return 0;
            else
                return Parties.Select("SelectedMattersOnly = 1").Length;
        }


        private static void UpdatePermissions(ISecurable securable, User user, string currentPolicy, string newPolicy, Guid securityId)
        {
            Permissions perms = new Permissions(securable as IExtraInfo);
            perms.AccessType = "EXTERNAL";
            perms.Refresh();

            if (newPolicy != currentPolicy)
            {
                string secID = Convert.ToString(securityId);

                if (!string.IsNullOrEmpty(newPolicy))
                {
                    if (string.IsNullOrEmpty(currentPolicy))
                        perms.AddPermission(user);

                    perms.ModifiedPolicy(secID); //needed to ensure the change is recognised
                    perms.ChangePolicy(secID, newPolicy);

                }
                else
                {
                    perms.DeletePermission(secID);
                }
                perms.UpdatePermission();
            }
        }

        private static string GetCurrentSecurityPolicy(User user, ISecurable securable)
        {
            Permissions perms = new Permissions(securable as IExtraInfo);
            perms.AccessType = "EXTERNAL";
            perms.Refresh();

            Guid securityID = GetSecurityID(user);
            return perms.GetPolicyID(securityID);
        }

        private static Guid GetSecurityID(User user)
        {
            string SecurityID = Convert.ToString(user.GetExtraInfo("SecurityID"));
            if (!string.IsNullOrEmpty(SecurityID))
                return new Guid(SecurityID);

            throw new OMSException("MSSECNOSECID", "The user accounts SecurityID is not set up correctly. Cannot apply any security changes");
        }

        private static DataTable GetPolicies()
        {
            ObjectPolicy policy = new ObjectPolicy();
            DataTable policies = policy.ListPolicies(true, true);
            policies.DefaultView.Sort = "PolicyDescription";
            return policies;
        }

        private static User GetUserForContact(Contact contact)
        {
            if (contact.HasUserAccount)
                return contact.User;
            else
                return AddUser(contact);
        }

        private static User AddUser(Contact contact)
        {
            Dictionary<User, List<Contact>> relatedUsersAndContacts = GetUsersAndRelatedContacts(contact.DefaultEmail);
            
            if (relatedUsersAndContacts.Count == 0)
                return CreateNewUser(contact); 

            
            switch (PromptToLinkToExistingUser(relatedUsersAndContacts, contact.DefaultEmail))
            {
                case DialogResult.Yes:
                    {
                        return relatedUsersAndContacts.Keys.First();
                    }
                case DialogResult.No:
                    {
                        return CreateNewUser(contact);                            
                    }
                case DialogResult.Cancel:                        
                default:
                    {
                        return null;                            
                    }
            }                                              
        }

        private static DialogResult PromptToLinkToExistingUser(Dictionary<User, List<Contact>> relatedUsersAndContacts, string email)
        {
            if (relatedUsersAndContacts == null)
                throw new ArgumentNullException("relatedUsersAndContacts");

            if (relatedUsersAndContacts.Count == 0)
                throw new ArgumentException("relatedUsersAndContacts must contain at least one user");

            StringBuilder message = new StringBuilder();

            var kvp = relatedUsersAndContacts.First();
            var user = kvp.Key;
            var contacts = kvp.Value;

            string associatedTo = Session.CurrentSession.Resources.GetResource("MSEMAILINUSE","The email address %1% is already associated to the user %2%","",email, user.FullName).Text;
            message.AppendLine(associatedTo);

            string relatedTo = Session.CurrentSession.Resources.GetResource("MSRELATEDTO", "Who is related to the following contact(s): ","").Text;
            if (contacts != null && contacts.Count != 0)
                message.AppendLine(relatedTo);

            foreach (var contact in contacts)
            {
                string contactDescription = Session.CurrentSession.Resources.GetResource("MSCONTACTDESC", "     %1% (ID:%2%)", "",contact.Name, contact.ID.ToString()).Text;
                message.AppendLine(contactDescription);
            }

            string promptLink = Session.CurrentSession.Resources.GetResource("MSPROMPTLINK", "If you are sure they are the same person then click Yes to link the contact to the user. Click no to create a new user.","").Text;
            message.AppendLine();
            message.Append(promptLink);

            string caption = Session.CurrentSession.Resources.GetResource("MSPROMPTLINKCAP", "Link to existing user", "").Text;
            return System.Windows.Forms.MessageBox.Show(null, message.ToString(), caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
        }

        private static User CreateNewUser(Contact contact)
        {
            User tmpuser = CreateUser(contact, null);
            User user = FWBS.OMS.UI.Windows.Services.ShowOMSItem("SCRUSRCLIENT", null, tmpuser, null) as User;

            if (user == null)
            {
                return null;
            }

            user.Refresh();
            FWBS.OMS.EnquiryEngine.DataLists ds = new FWBS.OMS.EnquiryEngine.DataLists("PTLADDUSR");
            FWBS.Common.KeyValueCollection dsparams = new FWBS.Common.KeyValueCollection();
            dsparams.Add("MAIL", user.Email);
            ds.ChangeParameters(dsparams);
            ds.Run(false);

            return user;
        }

        private static string GetDescription(ISecurable securable)
        {
            IExtraInfo info = securable as IExtraInfo;

            object description = string.Empty;

            if (info is OMSFile)
                description = info.GetExtraInfo("filedesc");

            else if (info is Client)
                description = info.GetExtraInfo("CLName");

            else if (info is OMSDocument)
                description = info.GetExtraInfo("docdesc");

            else if (info is Contact)
                description = info.GetExtraInfo("contname");

            else
                throw new OMSException("OBJNOTSECURABLE", "The current object is not securable");

            return Convert.ToString(description);
        }

        public static Dictionary<User, List<Contact>> GetUsersAndRelatedContacts(string email)
        {
            Dictionary<User, List<Contact>> users = new Dictionary<User, List<Contact>>();

            if (string.IsNullOrEmpty(email))
                return users;

            if (!FWBS.OMS.EnquiryEngine.DataLists.Exists("DSDUPEMAIL"))
                throw new OMSException2("DUPEMAILFAIL", "Duplicate email check failed. Is the datalist DSDUPEMAIL installed!!!");

            FWBS.OMS.EnquiryEngine.DataLists ds = new FWBS.OMS.EnquiryEngine.DataLists("DSDUPEMAIL");

            FWBS.Common.KeyValueCollection dsparams = new FWBS.Common.KeyValueCollection();
            dsparams.Add("email", email);
            ds.ChangeParameters(dsparams);

            DataTable dt = ds.Run(false) as DataTable;

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int userId = Convert.ToInt32(dt.Rows[0]["usrId"]);
                    User user = User.GetUser(userId);
                    if (!users.ContainsKey(user))
                        users.Add(user, new List<Contact>());

                    long contId;
                    if (!Int64.TryParse(Convert.ToString(row["contID"]), out contId))
                        continue;

                    Contact contact = Contact.GetContact(contId);
                    users[user].Add(contact);
                }
            }
            return users;
        }

        internal static GatewayNotifications GetGateway()
        {
            FWBS.OMS.Session session = FWBS.OMS.Session.CurrentSession;

            string endPointAddress = Convert.ToString(session.GetSpecificData("MSGateway"));
            string passcode = Convert.ToString(session.GetSpecificData("MSPassword"));
            string userName = session.CurrentUser.FullName;
            long companyID;

            companyID = long.MinValue;

            if (string.IsNullOrEmpty(endPointAddress))
                throw new ApplicationException("Specific Data 'MSGateway' is not configured. Cannot perform Matter Sphere Gateway Admin functions");

            if (string.IsNullOrEmpty(passcode))
                throw new ApplicationException("Specific Data 'MSPassword' is not configured. Cannot perform Matter Sphere Gateway Admin functions");

            //Allow a company ID override to be provided in Specific Data incase a company has more than 1 matter centre system
            if (!Int64.TryParse(Convert.ToString(session.GetSpecificData("MSCompanyID")), out companyID))
                companyID = session.CompanyID;

            return new GatewayNotifications(endPointAddress, passcode, companyID, userName);
        }

        private const string ACCESS_TYPE = "EXTERNAL";
        private const string USER_TYPE = "CLIENT";       

        /// <summary>
        /// Resets the users password and sends an email with the logon details and new passsord
        /// </summary>
        /// <param name="user"></param>
        public static void SendNotificationEmail(User user)
        {
            string password = GeneratePronouncablePassword(9);
            user.ResetPassword();
            string oldPassword = "";
            user.ChangePassword(oldPassword, password, password);

            SendNotificationEmail(user, GetEmailSubject(), GetEmailBody(user, password));
        }

        internal static void SendNotificationEmail(User user, string subject, string body)
        {
            if (Session.CurrentSession.IsMailEnabled == false)
                throw new MailDisabledException();

            string to = user.Email;
            string from = Session.CurrentSession.CurrentUser.Email;
            string fromFriendlyName = Session.CurrentSession.CompanyName;

            if (string.IsNullOrEmpty(to))
                throw new Exception("Cannot send notification email. The Client does not have an email address configured");

            if (string.IsNullOrEmpty(from))
                throw new Exception("Cannot send notification email. The current logged on user does not have an email address configured");

            SendEmail(from, fromFriendlyName, to, subject, body);
        }

        internal static void SendEmail(string from, string friendlyNameFrom, string to, string subject, string body)
        {
            MailAddress fromAddress = new MailAddress(from, friendlyNameFrom);
            MailAddress toAddress = new MailAddress(to);

            using (MailMessage message = new MailMessage(fromAddress, toAddress))
            {
                message.Subject = subject;
                message.Body = body;

                Session.CurrentSession.SendMail(message);
            }
        }

        
        #region RemoteUsers

        public static User CreateUser(Contact contact, string type)
        {
            if (contact.HasUserAccount)
                throw new OMSException("ERRUSERACCEXIST", "There is already a user account associated with this contact");

            string name = contact.Name;
            string email = contact.DefaultEmail;

            return CreateUser(name, email, type, null, null);
        }

        internal static string GetEmailBody(User user, string newPassword)
        {
            StringBuilder defaultMessage = new StringBuilder();
            defaultMessage.AppendLine("You are receiving this email because either a new account has been created for you or your password has been reset.");
            defaultMessage.AppendLine();
            defaultMessage.AppendLine("Your %1% account details are:");
            defaultMessage.AppendLine();
            defaultMessage.AppendLine("Username: %2%");
            defaultMessage.AppendLine("Password: %3%");
            defaultMessage.AppendLine();
            defaultMessage.AppendLine("You can access your %FILES% at %4%");
            string body = Session.CurrentSession.Resources.GetResource("MSACCEMAILBODY"
                , defaultMessage.ToString()
                , ""
                , true
                , Session.CurrentSession.CompanyName // %1%
                , user.Email // %2%
                , newPassword // %3%
                , Session.CurrentSession.Website // %4%                
                , "").Text;

            return body;
        }

        internal static string GetEmailSubject()
        {
            string subject = Session.CurrentSession.Resources.GetResource("MSACCEMAILSUB", @"%1% Account Details"
                 , ""
                 , true
                 , Session.CurrentSession.CompanyName // %1%
                 , "").Text;

            return subject;
        }

        /// <summary>
        /// Generate a password that is pronouncable and is therefore more memorable but is still random
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static string GeneratePronouncablePassword(int length)
        {
            string vowels = "aaaaaeeeeeeeiiiiiooooouuu";
            string consonants = "bbcccdddfffgghhhhjklllmmmnnnnnpprrrrrsssssttttttvwwxyyz";
            string symbols = "@$!%*?&";
            string[] vowelafter = { "th", "ch", "sh", "qu" };
            string[] consonantafter = { "oo", "ee" };
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            string pass = "";
            bool isvowel = false;
            length = length - 2; //because we have a 2 digits suffix
            for (int i = 0; i < length; i++)
            {
                if (isvowel)
                {
                    if (rnd.Next(0, 5) == 0 && i < (length - 1))
                    {
                        pass += consonantafter[rnd.Next(0, consonantafter.Length)];
                    }
                    else
                    {
                        pass += vowels.Substring(rnd.Next(0, vowels.Length), 1);
                    }
                }
                else
                {
                    if (rnd.Next(0, 5) == 0 && i < (length - 1))
                    {
                        pass += vowelafter[rnd.Next(0, vowelafter.Length)];
                    }
                    else
                    {
                        pass += consonants.Substring(rnd.Next(0, consonants.Length), 1);
                    }
                }
                isvowel = !isvowel;
            }

            //generate the prefix using the symbol list
            string prefix = symbols.Substring(rnd.Next(0, symbols.Length),1);

            //truncate the password to be <length> chars if longer
            if (pass.Length > length)
            {
                pass = pass.Substring(0, length);
            }

            //capitalise the first char of the password
            pass = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pass);

            //create the 2 digit suffix
            string suffix = string.Format("{0}{1}", rnd.Next(9), rnd.Next(9));

            //put the pieces together
            return prefix + pass + suffix;
        }

        private string GeneratePassword()
        {
            return GeneratePronouncablePassword(9);
        }

        #endregion

        #region Static
        public static User CreateUser(string fullName, string email, string type, string initials, string accessType)
        {
            //Assign default values to Alias, Initials and AccessType since they are required in matter centre

            string alias = Guid.NewGuid().ToString();
            initials = !string.IsNullOrEmpty(initials) ? initials : fullName;
            accessType = !string.IsNullOrEmpty(accessType) ? accessType : ACCESS_TYPE;
            type = !string.IsNullOrEmpty(type) ? type : USER_TYPE;

            UserType userType = null;
            try
            {
                userType = UserType.GetUserType(type);
            }
            catch (Exception ex)
            {
                throw new OMSException2("USRTYPEMISS", "The %1% user type does not exist. Unable to create remote accounts until it has been created", ex, true, type);
            }

            User user = null;
            user = new User(userType);

            user.FullName = fullName;
            user.Email = email;
            user.Alias = alias;          
            user.Initials = initials.ToUpper().Substring(0, initials.Length < 31 ? initials.Length : 30); //Trim to the max allowed characters in matter centre
            user.SetExtraInfo("AccessType", accessType);
            user.SetExtraInfo("usrWorksFor", -1);
            user.SetExtraInfo("usrActive", true);

            return user;
        }
        #endregion        
    }

    public class ClientAccount
    {
        User _user;
        EnquiryForm _enquiryForm;
        InteractiveLawSettings _currentSetting;
        bool _useInteractiveLawGateway;
        Button _btnResetPassword;
        Button _btnReset2FA;
        private bool _resetPasswordOnUpdate;
        private string _originalEmail;

        private string Email
        {
            get
            {
                return Convert.ToString(_enquiryForm.GetIBasicEnquiryControl2("Email").Value);
            }
        }

        private bool IALawActive
        {
            get
            {
                return ConvDef.ToBoolean(_enquiryForm.GetIBasicEnquiryControl2("IALawMobileActive").Value, false);
            }
        }

        private string Name
        {
            get
            {
                return Convert.ToString(_enquiryForm.GetIBasicEnquiryControl2("Name").Value);
            }
        }

        #region Constructors
        public ClientAccount(EnquiryForm enquiryForm)
            : this(enquiryForm, true)
        {

        }

        public ClientAccount(EnquiryForm enquiryForm, bool useInteractiveLawGateway)
        {
            _useInteractiveLawGateway = useInteractiveLawGateway;

            if (enquiryForm == null)
                throw new ArgumentNullException("enquiryForm");

            _enquiryForm = enquiryForm;
            _enquiryForm.Updating += new System.ComponentModel.CancelEventHandler(Updating);

            _user = _enquiryForm.Enquiry.Object as User;
            if (_user == null)
                throw new ApplicationException("User is null");

            _btnResetPassword = _enquiryForm.GetControl("btnResetPassword") as Button;
            _btnResetPassword.Click += new EventHandler(_btnResetPassword_Click);

            if (_user.IsNew)
            {
                _btnResetPassword.Visible = false;
                _resetPasswordOnUpdate = true;
            }

            _originalEmail = Email;
            _currentSetting = new InteractiveLawSettings(_user);

            _btnReset2FA = _enquiryForm.GetControl("btnReset2FA") as Button;
            _btnReset2FA.Click += new EventHandler(_btnReset2FA_Click);
        }

        void _btnResetPassword_Click(object sender, EventArgs e)
        {
            _resetPasswordOnUpdate = true;
            MsgBox.ShowInformation("SENDEMAILONSAV", "The client will be sent their logon details when you save your changes.");
            _btnResetPassword.Enabled = false;
            _enquiryForm.IsDirty = true;
        }

        void _btnReset2FA_Click(object sender, EventArgs e)
        {
            FWBS.OMS.EnquiryEngine.DataLists ds = new FWBS.OMS.EnquiryEngine.DataLists("PTLRESET2FA");
            FWBS.Common.KeyValueCollection dsparams = new FWBS.Common.KeyValueCollection();
            dsparams.Add("MAIL", Email);
            ds.ChangeParameters(dsparams);
            ds.Run(false);
        }

        #endregion

        #region Methods

        void Updating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_enquiryForm.IsDirty)
                return;

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email))
            {
                return;
            }

            CheckForDuplicateEmail(e);

            if (_resetPasswordOnUpdate || _originalEmail != Email)
                SendAccountDetailsEmail();

            if ((IALawActive != _currentSetting.Active) || (IALawActive && Email != _currentSetting.EmailAddress))
            {
                if (IALawActive)
                {
                    GrantMobileAccess(e);
                    return;
                }
                else
                {
                    RevokeMobileAccess(e);
                }
            }
        }

        private void CheckForDuplicateEmail(System.ComponentModel.CancelEventArgs e)
        {
            Dictionary<User, List<Contact>> users = RemoteAccount.GetUsersAndRelatedContacts(Email);
            if (users.Count == 0)
                return;

            foreach (User user in users.Keys)
            {
                if (user.ID == _user.ID)
                    return;
            }

            e.Cancel = true;
            throw new OMSException("MSDUPEMAIL", "Another user is using this email address. Please enter a different email address.");
        }

        private void RevokeMobileAccess(System.ComponentModel.CancelEventArgs e)
        {
            if (_useInteractiveLawGateway == false)
                return;

            if (string.IsNullOrEmpty(_currentSetting.EmailAddress))
                return;

            GatewayResponse resp = RemoteAccount.GetGateway().RemoveInteractiveLawAccount(_currentSetting.EmailAddress);
            if (resp.Success || resp.ErrorMessage.StartsWith("User does not exist", StringComparison.InvariantCultureIgnoreCase))
            {
                _currentSetting.EmailAddress = string.Empty;
                _currentSetting.Active = false;
                e.Cancel = e.Cancel || false;
            }
            else
            {
                ShowGatewayError(resp);
            }
        }

        private void GrantMobileAccess(System.ComponentModel.CancelEventArgs e)
        {
            if (_useInteractiveLawGateway == false)
                return;

            if (Email != _currentSetting.EmailAddress)
            {
                RevokeMobileAccess(e);
            }

            GatewayResponse resp = RemoteAccount.GetGateway().CreateInteractiveLawAccount(Email);
            if (resp.Success || resp.ErrorMessage.StartsWith("User is already Registered", StringComparison.InvariantCultureIgnoreCase))
            {
                _currentSetting.EmailAddress = Email;
                _currentSetting.Active = true;
                e.Cancel = e.Cancel || false;
                e.Cancel = true;
            }
            else
            {
                ShowGatewayError(resp);
                e.Cancel = true;
            }

            e.Cancel = e.Cancel || !resp.Success;
        }

        private void ShowGatewayError(GatewayResponse resp)
        {
            MsgBox.ShowInformation("MSGWERROR", "Unable to update the InteractiveLaw Gateway. The response received was: %1%", resp.ErrorMessage);
        }        

        private void SendAccountDetailsEmail()
        {            
            if (PromptToResetPassword() != DialogResult.Yes)
                return;

            SendEmail();
            _resetPasswordOnUpdate = false;
        }

        private void SendEmail()
        {            
            RemoteAccount.SendNotificationEmail(_user);
        }   

        private DialogResult PromptToResetPassword()
        {
            return MsgBox.ShowYesNoQuestion("MSSENDEMAIL", "Do you want to send the client an email containing their logon details?");
        }
        #endregion

        internal class InteractiveLawSettings
        {
            User _user;
            private const String InteractiveLawSettingsCode = "EXTUSERCLI";

            internal InteractiveLawSettings(User user)
            {
                _user = user;
            }

            internal string EmailAddress
            {
                get
                {
                    return Convert.ToString(_user.ExtendedData[InteractiveLawSettingsCode].GetExtendedData("Email"));
                }
                set
                {
                    _user.ExtendedData[InteractiveLawSettingsCode].SetExtendedData("Email", value);
                    SetUpdatedInformation();
                }
            }

            internal bool Active
            {
                get
                {
                    return FWBS.Common.ConvDef.ToBoolean(_user.ExtendedData[InteractiveLawSettingsCode].GetExtendedData("Active"), false);
                }
                set
                {
                    _user.ExtendedData[InteractiveLawSettingsCode].SetExtendedData("Active", value);
                    SetUpdatedInformation();
                }
            }

            internal void SetUpdatedInformation()
            {
                _user.ExtendedData[InteractiveLawSettingsCode].SetExtendedData("UpdatedDate", DateTime.Now);
                _user.ExtendedData[InteractiveLawSettingsCode].SetExtendedData("UpdatedBy", FWBS.OMS.Session.CurrentSession.CurrentUser.ID);
            }
        }
    }
}
