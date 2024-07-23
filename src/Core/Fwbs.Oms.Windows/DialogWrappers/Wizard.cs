using System;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.UI.DialogWrappers;


namespace FWBS.OMS.UI
{
    using FWBS.OMS.UI.Windows;

    /// <summary>
    /// Static wizard class that runs different types of wizards.  These wizards include
    /// custom and globally used ones.
    /// </summary>
    public class Wizard
    {

        #region Fields

        private EnquiryEngine.Enquiry _enq = null;
        private EnquiryForm _enqform = null;
        private frmWizard frm = null;

        #endregion

        #region Constructors

        protected Wizard()
        {
        }

        public Wizard(EnquiryEngine.Enquiry enq)
        {
            using (var trace = Fwbs.Framework.Diagnostics.TraceDuration.Start(this, "Constructor"))
            {
                _enq = enq;
                frm = new frmWizard(_enq);
                _enqform = frm.EnquiryForm;
            }

        }

        #endregion

        #region Properties
        public EnquiryForm EnquiryForm
        {
            get
            {
                return _enqform;
            }
        }

        public Form Form
        {
            get
            {
                return frm;
            }
        }

        #endregion

        #region Instance Methods

        public object Show(IWin32Window owner)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                object obj = null;
                if (Services.CheckLogin())
                {
                    if (frm.ShowDialog(owner) == DialogResult.OK)
                        obj = frm.EnquiryForm.Enquiry.Object;

                    frm.Dispose();
                }

                return obj;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(owner, ex);
                return null;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public object Show()
        {
            return Show(null);
        }


        #endregion

    }

}

namespace FWBS.OMS.UI.Windows
{
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.DocumentManagement.Storage;
    using FWBS.OMS.EnquiryEngine;
    using FWBS.OMS.Security.Permissions;
    using FWBS.OMS.StatusManagement;
    using FWBS.OMS.StatusManagement.Activities;

    partial class Services
    {
        public sealed class Wizards : FWBS.OMS.UI.Wizard
        {
            public Wizards(EnquiryEngine.Enquiry enq)
                : base(enq)
            {
            }

            public Form Wizard
            {
                get
                {
                    return base.Form;
                }
            }

            #region Static
            /// <summary>
            /// Runs a file review wizard.
            /// </summary>
            /// <param name="file">The file to review.</param>
            /// <returns>True if the file review has been successfull.</returns>
            public static bool FileReview(OMSFile file)
            {

                if (file == null)
                    file = SelectFile();

                if (file != null)
                {
                    file = GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.FileReview), file, file, null) as OMSFile;
                }
                if (file != null) OnOMSTypeRefresh();
                return (file != null);
            }

            /// <summary>
            /// Creates a contact email address.
            /// </summary>
            /// <param name="location">The type of the email, HOME, WORK etc...</param>
            /// <returns>A contact email object.</returns>
            public static ContactEmail CreateEmail(string location)
            {
                Common.KeyValueCollection pars = new Common.KeyValueCollection();
                if (location == "") location = "HOME";
                pars.Add("LOCATION", location);
                DataTable ret = GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.Email), null, EnquiryMode.Add, true, pars) as DataTable;
                if (ret != null)
                {
                    ContactEmail email = new ContactEmail(Convert.ToString(ret.Rows[0]["_location"]), Convert.ToString(ret.Rows[0]["_email"]));
                    return email;
                }
                return null;
            }

            /// <summary>
            /// Creates a contact telephone number.
            /// </summary>
            /// <param name="type">Primary number type, Telephone, Fax etc..</param>
            /// <param name="location">The type of the number, HOME, WORK etc...</param>
            /// <returns>A contact number object.</returns>
            public static ContactNumber CreateNumber(string type, string location)
            {
                return CreateNumber(null, type, location);
            }

            public static ContactNumber CreateNumber(IWin32Window owner, string type, string location)
            {
                Common.KeyValueCollection pars = new Common.KeyValueCollection();
                if (type == "") type = "TELEPHONE";
                pars.Add("TYPE", type);
                if (location == "") location = "HOME";
                pars.Add("LOCATION", location);
                DataTable ret = GetWizard(owner, Session.CurrentSession.DefaultSystemForm(SystemForms.TelephoneNumber), null, EnquiryMode.Add, true, pars) as DataTable;
                if (ret != null)
                {
                    ContactNumber num = new ContactNumber(Convert.ToString(ret.Rows[0]["_type"]), Convert.ToString(ret.Rows[0]["_location"]), Convert.ToString(ret.Rows[0]["_number"]));
                    return num;
                }
                return null;
            }

            /// <summary>
            /// Creates UFN Information on a File.
            /// </summary>
            /// <param name="file">The file to create the UFN Information under.</param>
            /// <returns>Boolean True or False</returns>
            public static bool CreateUFNInformation(OMSFile file)
            {
                if (file == null)
                    file = SelectFile();

                if (file != null)
                {
                    using (frmUFNWizard _ufn = new frmUFNWizard(file))
                    {
                        object n = _ufn.ShowDialog();
                        if (n != null)
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return false;
            }

            /// <summary>
            /// Fire this Wizard if the Address is Linked to more than one Contact
            /// </summary>
            /// <param name="address">The Address Object to Check for Other links</param>
            /// <param name="contact">The Current Contact so it will not be included in the Results</param>
            /// <returns>CANCEL if the Wizard is Cancel, or MODIFYALL to Update the Address for All Contacts, MODIFYONE to update the Contacts Address</returns>
            public static string MultiLinkedAddressResolver(FWBS.OMS.Address address, FWBS.OMS.Contact contact)
            {
                FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                param.Add("AddID", address.ID);
                param.Add("ContID", contact.ID);
                DataTable data = GetWizard("SCRMADDLINK", null, EnquiryEngine.EnquiryMode.Add, false, param) as DataTable;
                if (data == null)
                    return "CANCEL";
                else
                    return Convert.ToString(data.Rows[0]["cmbActions"]);

            }

            /// <summary>
            /// Fire this wizard if the email address is Linked to more than one Associate
            /// </summary>
            /// <param name="address"></param>
            /// <param name="contact"></param>
            /// <returns></returns>
            public static string MultiLinkedEmailAddressResolver(string originalAddress, FWBS.OMS.Contact contact)
            {
                string assocIDs = "";
                FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                param.Add("emailAddress", originalAddress);
                param.Add("ContID", contact.ID);
                DataTable data = GetWizard("sEADDRESOLVE", null, EnquiryEngine.EnquiryMode.Add, false, param) as DataTable;
                if (data != null)
                    assocIDs = Convert.ToString(data.Rows[0]["cmpAssocIDs"]);
                return assocIDs;
            }


            /// <summary>
            /// Activates a save appointment wizard.
            /// </summary>
            /// <param name="appointment">The already constructed appointment to be saved through the wizard.</param>
            /// <returns>True, if the wizard successfully finishes.</returns>
            public static bool SaveAppointment(ref Appointment appointment)
            {
                return (GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.AppointmentWizard), appointment.Parent, appointment, null) != null);
            }


            /// <summary>
            /// Creates an appointment by prompting the user for a file.
            /// </summary>
            /// <returns>New appointment, or null if the wizard is cancelled.</returns>
            public static Appointment CreateAppointment()
            {
                return CreateAppointment(null);
            }

            /// <summary>
            /// Creates an appointment with the specified file.
            /// </summary>
            /// <param name="file">The file to create the appointment under.</param>
            /// <returns>New appointment object, or null if the wizard is cancelled.</returns>
            public static Appointment CreateAppointment(OMSFile file)
            {
                Appointment app = null;

                if (file == null)
                    file = SelectFile();

                if (file != null)
                {
                    app = new Appointment(file, "");
                    if (SaveAppointment(ref app) == false)
                        app = null;
                }
                return app;
            }

            /// <summary>
            /// Activates a save task wizard.
            /// </summary>
            /// <param name="task">The already constructed task to be saved through the wizard.</param>
            /// <returns>True, if the wizard successfully finishes.</returns>
            public static bool SaveTask(ref Task task)
            {
                return (GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.TaskWizard), task.Parent, task, null) != null);
            }

            /// <summary>
            /// Creates a task by prompting the user for a file.
            /// </summary>
            /// <returns>A task object, or null if the wizard is cancelled.</returns>
            public static Task CreateTask()
            {
                return CreateTask(null);
            }

            /// <summary>
            /// Creates a task with the specified file.
            /// </summary>
            /// <param name="file">The file to create the task under.</param>
            /// <returns>A task object, or null if the wizard is cancelled.</returns>
            public static Task CreateTask(OMSFile file)
            {
                Task tsk = null;

                if (file == null)
                    file = SelectFile();

                if (file != null)
                {
                    tsk = new Task(file, "");
                    if (SaveTask(ref tsk) == false)
                        tsk = null;
                }
                return tsk;
            }

            /// <summary>
            /// Executes the key date wizard dialog.
            /// </summary>
            /// <param name="file">The OMS file to link the dates to.</param>
            public static bool CreateKeyDate(OMSFile file)
            {
                if (file == null)
                    file = SelectFile();

                if (file != null)
                {
                    if (CheckLogin())
                    {
                        using (frmDateWizard wiz = new frmDateWizard(file))
                        {
                            if (wiz.ShowDialog() == DialogResult.OK)
                                return true;
                            else
                                return false;
                        }
                    }
                }
                return false;
            }

            /// <summary>
            /// Executes the key date wizard dialog.
            /// </summary>
            /// <param name="fileid">The file id to use.</param>
            public static bool CreateKeyDate(long fileid)
            {
                return CreateKeyDate(OMSFile.GetFile(fileid));
            }

            /// <summary>
            /// Executes the key date wizard dialog.
            /// </summary>
            public static bool CreateKeyDate()
            {
                return CreateKeyDate(null);
            }

            /// <summary>
            /// Shows the Manual Time Wizard
            /// </summary>
            /// <param name="fileid">File ID</param>
            public static void CreateManualTime(Int64 fileid)
            {
                if (Session.CurrentSession.IsPackageInstalled("TimeRecording") && Session.CurrentSession.IsLicensedFor("TIMEREC"))
                {
                    OMSFile _omsfile = OMSFile.GetFile(fileid);
                    FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.ManualTimeWizard), _omsfile, null);
                }
                else
                {
                    MessageBox.ShowInformation("ERRTIMPACLIC", "Error cannot show Time Recording Wizard because the Package is not installed or the user is not licensed for Time Recording");
                }
            }

            public static Contact CreateContact()
            {
                return CreateContact(false);
            }

            public static Contact CreateContact(FWBS.OMS.Interfaces.IOMSApp appcontroller)
            {
                return CreateContact(false, appcontroller);
            }

            /// <summary>
            /// Executes the contact take on wizard.
            /// </summary>
            /// <param name="SkipSearch"></param>
            /// <param name="appcontroller"></param>
            /// <param name="owner">The owner window of the wizard.</param>
            /// <param name="modal">Specifies whether to show true modal dialog (system default) or a semi-modal form.</param>
            /// <returns>The created contact object or null if failed or cancelled.</returns>
            public static Contact CreateContact(bool SkipSearch, FWBS.OMS.Interfaces.IOMSApp appcontroller = null, IWin32Window owner = null, bool modal = true)
            {
                new SystemPermission(StandardPermissionType.CreateContact).Check();
                Contact obj = null;
                if (CheckLogin())
                {
                    try
                    {
                        using (frmContactWizard frm = new frmContactWizard(null))
                        {
                            frm.SkipSearch = SkipSearch;
                            DialogResult result = (modal || owner == null) ? frm.ShowDialog(owner) : frm.ShowModal(owner);
                            if (result == DialogResult.OK)
                                obj = frm.Contact;
                        }
                    }
                    catch (Exception ex)
                    {
                        obj = null;
                        ErrorBox.Show(owner, ex);
                    }
                }
                return obj;
            }

            /// <summary>
            /// Executes the contact take on wizard.
            /// </summary>
            /// <returns>The created contact object or null if failed or cancelled.</returns>
            public static Contact CreateContact(Client client, bool SkipSearch)
            {
                client.Validate(true, true);
                return CreateContact(SkipSearch);
            }           

            /// <summary>
            /// Executes the contact take on wizard.
            /// </summary>
            /// <returns>The created contact object or null if failed or cancelled.</returns>
            public static Contact CreateContact(Int64 clid, bool SkipSearch)
            {
                Client client = Client.GetClient(clid);
                return CreateContact(client, SkipSearch);
            }

            /// <summary>
            /// Executes the contact take on wizard.
            /// </summary>
            /// <returns>True or False if failed or cancelled.</returns>
            public static bool CreateContactBool(bool SkipSearch)
            {
                bool obj = false;

                if (CheckLogin())
                {
                    try
                    {
                        using (frmContactWizard frm = new frmContactWizard(null))
                        {
                            frm.SkipSearch = SkipSearch;
                            if (frm.ShowDialog() == DialogResult.OK)
                                obj = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        obj = false;
                        ErrorBox.Show(ex);
                    }
                }
                return obj;
            }

            /// <summary>
            /// Executes the client take on wizard.
            /// </summary>
            /// <returns>The created client object or null if failed or cancelled.</returns>
            public static Client CreateClient()
            {
                return CreateClient(null, false);
            }

            /// <summary>
            /// Executes the client take on wizard.
            /// </summary>
            /// <param name="Quick">Uses Quick Client Mode</param>
            /// <returns>The created client object or null if failed or cancelled.</returns>
            public static Client CreateClient(bool Quick)
            {
                return CreateClient(null, Quick);
            }

            /// <summary>
            /// Executes the client take on wizard with a default contact.
            /// </summary>
            /// <param name="defaultContact">The default contact to assign to the new client.</param>
            /// <param name="quick">Uses Quick Client Mode</param>
            /// <returns>The created client object or null if failed or cancelled.</returns>
            public static Client CreateClient(Contact defaultContact, bool quick)
            {
                Client obj = null;
                if (CheckLogin())
                {
                    try
                    {
                        new SystemPermission(StandardPermissionType.CreateClient).Check();
                        using (frmClientTakeonWizard frm = new frmClientTakeonWizard(defaultContact, false, quick, null))//;
                        {
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                obj = frm.Client;
                                if (frm.ViewClient)
                                    ShowClient(obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        obj = null;
                        ErrorBox.Show(ex);
                    }
                }
                return obj;
            }


            /// <summary>
            /// Executes the pre-client take on wizard.
            /// </summary>
            /// <returns>The created client object or null if failed or cancelled.</returns>
            public static Client CreatePreClient()
            {
                return CreatePreClient(null);
            }

            /// <summary>
            /// Executes the pre-client take on wizard with a default contact.
            /// </summary>
            /// <param name="defaultContact">The default contact to assign to the new client.</param>
            /// <returns>The created client object or null if failed or cancelled.</returns>
            public static Client CreatePreClient(Contact defaultContact)
            {
                Client obj = null;
                if (CheckLogin())
                {
                    try
                    {
                        using (frmClientTakeonWizard frm = new frmClientTakeonWizard(defaultContact, true, false, null))//;
                        {
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                obj = frm.Client;
                                if (frm.ViewClient)
                                    ShowClient(obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        obj = null;
                        ErrorBox.Show(ex);
                    }
                }
                return obj;
            }

            /// <summary>
            /// Executes the file take on wizard.
            /// </summary>
            /// <param name="client">Client object to add a new file to.</param>
            /// <param name="owner">The owner window of the wizard.</param>
            /// <param name="modal">Specifies whether to show true modal dialog (system default) or a semi-modal form.</param>
            /// <returns>The created file object or null if failed or cancelled.</returns>
            public static OMSFile CreateFile(Client client, IWin32Window owner = null, bool modal = true)
            {
                new SystemPermission(StandardPermissionType.CreateFile).Check();

                OMSFile obj = null;

                if (client == null)
                    client = SelectClient();

                if (client != null)
                {
                    if (CheckLogin())
                    {
                        try
                        {
                            new ClientPermission(client, StandardPermissionType.CreateFile).Check();
                            new ClientActivity(client, ClientStatusActivityType.FileCreation).Check();

                            using (frmFileWizard frm = new frmFileWizard(client, null))
                            {
                                DialogResult result = (modal || owner == null) ? frm.ShowDialog(owner) : frm.ShowModal(owner);
                                if (result == DialogResult.OK)
                                {
                                    obj = frm.OMSFile;
                                    if (frm.ViewFile)
                                        ShowFile(obj);
                                    if (obj != null && obj.GenerateJobsOnCreation)
                                        CheckJobList(null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            obj = null;
                            ErrorBox.Show(owner, ex);
                        }
                    }
                }
                return obj;
            }

            /// <summary>
            /// Executes the file take on wizard.
            /// </summary>
            /// <param name="clientID">Client identifier to add a new file to.</param>
            /// <returns>The created file object or null if failed or cancelled.</returns>
            public static OMSFile CreateFile(long clientID)
            {
                return CreateFile(Client.GetClient(clientID));
            }

            /// <summary>
            /// Executes the file take on wizard with a prompt.
            /// </summary>
            /// <param name="prompt">If true the a client select form will be prompted, otherwise the current client is used.</param>
            /// <returns>The created file object or null if failed or cancelled.</returns>
            public static OMSFile CreateFile(bool prompt)
            {
                Client client = prompt ? null : Session.CurrentSession.CurrentClient;
                return CreateFile(client);
            }

            /// <summary>
            /// Executes the associate creation wizard.
            /// </summary>
            /// <param name="fileid">The file identifier that is needed to associate a contact to.</param>
            /// <returns>The newly created associate, or null if the wizard cancels at any point.</returns>
            public static Associate CreateAssociate(long fileid)
            {
                return CreateAssociate(OMSFile.GetFile(fileid));
            }

            /// <summary>
            /// Executes the associate creation wizard which will ask you to search for a contact..
            /// </summary>
            /// <param name="file">The file that is needed to associate a contact to.</param>
            /// <returns>The newly created associate, or null if the wizard cancels at any point.</returns>
            public static Associate CreateAssociate(OMSFile file)
            {
                return CreateAssociate(file, null, false);
            }


            /// <summary>
            /// Executes the associate creation wizard.
            /// </summary>
            /// <param name="file">The file that is needed to associate a contact to.</param>
            /// <returns>The newly created associate, or null if the wizard cancels at any point.</returns>
            /// <param name="contact">The contact to be associated.</param>
            public static Associate CreateAssociate(OMSFile file, Contact contact, bool Offline)
            {
                Associate obj = null;
                using (frmAssociateWizard frm = GetAssociateWizard(file, contact, Offline))
                {
                    if (frm != null && frm.ShowDialog() == DialogResult.OK)
                    {
                        obj = frm.Associate;
                    }
                }
                return obj;
            }

            internal static frmAssociateWizard GetAssociateWizard(OMSFile file, Contact contact, bool Offline, WizardStyle style = WizardStyle.Dialog)
            {
                frmAssociateWizard frm = null;
                if (file == null)
                    file = SelectFile();
                if (file != null)
                {
                    if (CheckLogin())
                    {
                        try
                        {
                            new SystemPermission(StandardPermissionType.CreateAssociate).Check();
                            new FilePermission(file, StandardPermissionType.CreateAssociate).Check();
                            new FileActivity(file, FileStatusActivityType.AssociateCreation).Check();
                            frm = new frmAssociateWizard(file, contact, Offline, style);
                        }
                        catch (Exception ex)
                        {
                            frm = null;
                            ErrorBox.Show(ex);
                        }
                    }
                }
                return frm;
            }


            /// <summary>
            /// Runs wizard to import Active Directory users
            /// </summary>
            /// <returns></returns>
            public static DataTable ImportADUsers()
            {

                DataTable dtRet = null;

                if (CheckLogin())
                {
                    try
                    {

                        using (frmADImportUsersWizard frm = new frmADImportUsersWizard())//;
                        {
                            if (frm.ShowDialog() == DialogResult.OK)
                                dtRet = frm.Users;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ex);
                    }
                }
                return dtRet;
            }


            /// <summary>
            /// Executes the remote associate creation wizard.
            /// </summary>
            /// <param name="fileid">The file id that is needed to associate a Remote Account</param>
            /// <returns>The newly created associate, or null if the wizard cancels at any point.</returns>
            public static Associate CreateRemoteAssociate(Int64 fileid)
            {
                return CreateRemoteAssociate(OMSFile.GetFile(fileid));
            }

            /// <summary>
            /// Executes the remote associate creation wizard.
            /// </summary>
            /// <param name="file">The file that is needed to associate a Remote Account</param>
            /// <returns>The newly created associate, or null if the wizard cancels at any point.</returns>
            public static Associate CreateRemoteAssociate(OMSFile file)
            {
                Associate obj = null;

                if (file == null)
                    file = SelectFile();

                if (file != null)
                {
                    if (CheckLogin())
                    {
                        try
                        {
                            using (frmAssociateRemoteWizard frm = new frmAssociateRemoteWizard(file, null))
                            {
                                if (frm.ShowDialog() == DialogResult.OK)
                                    obj = frm.EnquiryForm.Enquiry.Object as Associate;
                            }
                        }
                        catch (Exception ex)
                        {
                            obj = null;
                            ErrorBox.Show(ex);
                        }
                    }
                }
                return obj;
            }


            /// <summary>
            /// Gets a specific wizard using its wizard enquiry code and enquiry mode.  It will return
            /// an output parameter that will give the object that the wizard is manipulating.
            /// </summary>
            /// <param name="owner">The owner window of the wizard.</param>
            /// <param name="wizardCode">Unique enquiry wizard code.</param>
            /// <param name="parent">The parent to use for the enquiry form.</param>
            /// <param name="mode">Specifies the wizard to be in ad or edit mode.  If in edit mode an item will be need in the param parameter.</param>
            /// <param name="offline">Specifies whether the database will be updated or not.</param>
            /// <param name="param">Parameters that may be needed to replace items in the enquiry to run properly.</param>
            /// <param name="modal">Specifies whether to show true modal dialog (system default) or a semi-modal form.</param>
            /// <returns>The underlying data source of the item created.</returns>
            public static object GetWizard(IWin32Window owner, string wizardCode, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param, bool modal = true)
            {
                object obj = null;
                try
                {
                    using (frmWizard frm = GetWizardForm(owner, wizardCode, parent, mode, offline, param))
                    {
                        if (frm != null)
                        {
                            frm.FormClosed += (s, e) =>
                            {
                                if (((frmWizard)s).DialogResult == DialogResult.OK)
                                    obj = ((frmWizard)s).EnquiryForm.Enquiry.Object;
                            };
                            DialogResult result = (modal || owner == null) ? frm.ShowDialog(owner) : frm.ShowModal(owner);
                        }
                    }
                }
                catch (Exception ex)
                {
                    obj = null;
                    ErrorBox.Show(owner, ex);
                }
                return obj;
            }

            internal static frmWizard GetWizardForm(IWin32Window owner, string wizardCode, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param, WizardStyle style = WizardStyle.Dialog)
            {
                frmWizard frm = null;
                if (CheckLogin())
                {
                    try
                    {
                        frm = new frmWizard(wizardCode, parent, mode, offline, param, style);
                    }
                    catch (Exception ex)
                    {
                        frm = null;
                        ErrorBox.Show(owner, ex);
                    }
                }
                return frm;
            }


            public static object GetWizard(string wizardCode, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param)
            {
                return GetWizard(null, wizardCode, parent, mode, offline, param);
            }


            public static object GetWizard(string wizardCode, object parent, EnquiryMode mode, Common.KeyValueCollection param)
            {
                return GetWizard(null, wizardCode, parent, mode, false, param);
            }


            public static object GetWizard(string wizardCode, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, Common.KeyValueCollection param)
            {
                return GetWizard(null, wizardCode, businessObject.Parent, businessObject, param);
            }

            /// <summary>
            /// New to prevent creation of new record with offline flag
            /// </summary>
            /// <param name="wizardCode"></param>
            /// <param name="businessObject"></param>
            /// <param name="offline"></param>
            /// <param name="param"></param>
            /// <returns></returns>
            public static object GetWizard(string wizardCode, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, bool offline, Common.KeyValueCollection param)
            {
                return GetWizard(null, wizardCode, businessObject.Parent, businessObject, offline, param);
            }

            /// <summary>
            /// Edits an existing library object with the specified wizard enquiry code. It will return
            /// an output parameter that will give the object that the wizard is manipulating.
            /// </summary>
            /// <param name="wizardCode">Enquiry wizard code to use for the edit.</param>
            /// <param name="parent">The parent to use for the enquiry form.</param>
            /// <param name="businessObject">Business object to edit (i.e, CurrentUser)</param>
            /// <param name="param">Parameters that may be needed to replace items in the enquiry to run properly.</param>
            /// <returns>The underlying data source of the item created.</returns>
            public static object GetWizard(IWin32Window owner, string wizardCode, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, Common.KeyValueCollection param)
            {
                return GetWizard(owner, wizardCode, parent, businessObject, false, param);
            }

            public static object GetWizard(IWin32Window owner, string wizardCode, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, bool offline, Common.KeyValueCollection param)
            {
                object obj = null;
                if (CheckLogin())
                {
                    try
                    {
                        using (frmWizard frm = new frmWizard(wizardCode, parent, businessObject, offline, param))
                        {
                            if (frm.ShowDialog(owner) == DialogResult.OK)
                                obj = frm.EnquiryForm.Enquiry.Object;
                        }
                    }
                    catch (Exception ex)
                    {
                        obj = null;
                        ErrorBox.Show(owner, ex);
                    }
                }

                return obj;
            }


            public static object GetWizard(string wizardCode, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, Common.KeyValueCollection param)
            {
                return GetWizard(null, wizardCode, parent, businessObject, param);
            }


            /// <summary>
            /// Edits an existing library object.
            /// </summary>
            /// <param name="businessObject">Business object to edit (i.e, CurrentUser)</param>
            /// <param name="param">Parameters that may be needed to replace items in the enquiry to run properly.</param>
            /// <returns>The underlying data source of the item created.</returns>
            public static object GetWizard(IWin32Window owner, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, Common.KeyValueCollection param)
            {
                object obj = null;
                if (CheckLogin())
                {
                    try
                    {
                        using (frmWizard frm = new frmWizard(businessObject, param))
                        {
                            if (frm.ShowDialog(owner) == DialogResult.OK)
                                obj = frm.EnquiryForm.Enquiry.Object;
                        }
                    }
                    catch (Exception ex)
                    {
                        obj = null;
                        ErrorBox.Show(owner, ex);
                    }
                }

                return obj;
            }


            public static object GetWizard(FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, Common.KeyValueCollection param)
            {
                return GetWizard(null as IWin32Window, businessObject, param);
            }





            /// <summary>
            /// Creates a user object.
            /// </summary>
            /// <returns>The created user object or null if failed or cancelled.</returns>
            public static User CreateUser()
            {
                return GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.UserWizard), null, EnquiryMode.Add, null) as User;
            }


            /// <summary>
            /// Creates a new fee earner.
            /// </summary>
            /// <returns>The created fee earner object or null if failed or cancelled.</returns>
            public static FeeEarner CreateFeeEarner()
            {
                return GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.FeeEarnerWizard), null, EnquiryMode.Add, null) as FeeEarner;
            }

            public static FeeEarner CreateFeeEarner(User user)
            {
                FeeEarner fee = user.UpgradeToFeeEarner();
                FeeEarner changed = GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.FeeEarnerWizard), fee, null) as FeeEarner;
                if (changed == null) fee.DowngradeToUser();
                return changed;
            }

            /// <summary>
            /// Displays the welcome wizard for the currently logged in user.
            /// </summary>
            /// <returns>Returns true if the wizard was successfully completed.</returns>
            public static bool Welcome()
            {
                object obj = null;
                if (CheckLogin())
                    obj = GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.UserSettings), null, Session.CurrentSession.CurrentUser, null);
                return (obj != null);
            }



            /// <summary>
            /// Creates an address object.
            /// </summary>
            /// <returns>The created address object or null if failed or cancelled.</returns>
            public static Address CreateAddress()
            {
                return GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.AddressWizard), null, EnquiryMode.Add, false, null) as Address;
            }

            /// <summary>
            /// Creates an address object.
            /// </summary>
            /// <returns>The created address object or null if failed or cancelled.</returns>
            public static Address CreateAddress(bool offline)
            {
                return GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.AddressWizard), null, EnquiryMode.Add, offline, null) as Address;
            }

            /// <summary>
            /// Creates an address object.
            /// </summary>
            /// <returns>The created address object or null if failed or cancelhled.</returns>
            public static Address CreateAddress(Address AddressToClone)
            {
                return GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.AddressEdit), AddressToClone.Clone(), null) as Address;
            }

            /// <summary>
            /// Creates an address object.
            /// </summary>
            /// <returns>The created address object or null if failed or cancelhled.</returns>
            public static Address CreateAddress(Address AddressToClone, bool offline)
            {
                return GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.AddressEdit), AddressToClone.Clone(), offline, null) as Address;
            }

            /// <summary>
            /// Creates a code lookup object for contact groups. 
            /// </summary>
            /// <returns>The created code lookup object or null if failed or cancelled.</returns>
            public static CodeLookupLocalized CreateContactGroup()
            {
                return GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.ContactGroupWizard), null, EnquiryMode.Add, false, null) as CodeLookupLocalized;
            }


            /// <summary>
            /// Activates a save document wizard.
            /// </summary>
            /// <param name="document">The already constructed document to be saved through the wizard.</param>
            /// <returns>True, if the wizard successfully finishes.</returns>

            public static bool SaveDocument(ref OMSDocument document)
            {
                return SaveDocument(null, ref document);
            }

            public static bool SaveDocument(IWin32Window owner, ref OMSDocument document)
            {
                SetMaxRevisionCountOnDocument(document);
                return (GetWizard(owner, Session.CurrentSession.DefaultSystemForm(SystemForms.SaveDocumentWizard), document.OMSFile, document, null) != null);
            }

            public static bool SaveDocument(IWin32Window owner, string defaultPage, ref OMSDocument document)
            {
            
                SetMaxRevisionCountOnDocument(document);
                using (Enquiry enq = Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.SaveDocumentWizard), document.OMSFile, document, null))
                {
                    Wizard wiz = new Wizard(enq);
                    if (!String.IsNullOrEmpty(defaultPage))
                        wiz.EnquiryForm.GotoPage(defaultPage);
                    if (wiz.Show(owner) != null)
                        return true;
                    else
                        return false;
                }
            }
            

            private static void SetMaxRevisionCountOnDocument(OMSDocument document)
            {
                IStorageItemVersionable versionable = document;
                DocumentVersion version = (DocumentVersion)versionable.GetLatestVersion();

                if (version.Label.Split('.').Length - 1 >= FWBS.OMS.Session.CurrentSession.DocumentMaximumRevisionCount)
                {
                    document.ReachDocMaxRevisionCount = true;
                }
                else
                {
                    document.ReachDocMaxRevisionCount = false;
                }
            }

            public static bool SaveDocument(IWin32Window owner, ref DocumentVersion version)
            {
                SetMaxRevisionCountOnDocument(version);
                return (GetWizard(owner, Session.CurrentSession.DefaultSystemForm(SystemForms.SaveDocumentWizard), version.ParentDocument.OMSFile, version.ParentDocument, null) != null);
            }

            private static void SetMaxRevisionCountOnDocument(DocumentVersion version)
            {
                if (version.Label.Split('.').Length - 1 >= FWBS.OMS.Session.CurrentSession.DocumentMaximumRevisionCount)
                {
                    version.ParentDocument.ReachDocMaxRevisionCount = true;
                }
                else
                {
                    version.ParentDocument.ReachDocMaxRevisionCount = false;
                }
            }




            /// <summary>
            /// Activates a save precedent wizard.
            /// </summary>
            /// <param name="precedent">The already constructed precedent to be saved through the wizard.</param>
            /// <param name="app">The application that is running the wizard.</param>
            /// <returns>True, if the wizard successfully finishes.</returns>
            public static bool SavePrecedent(ref Precedent precedent, FWBS.OMS.Interfaces.IOMSApp app)
            {
                if (precedent.IsNew)
                    new SystemPermission(StandardPermissionType.CreatePrecedent).Check();
                else
                    new SystemPermission(StandardPermissionType.UpdatePrecedent).Check();

                Guid guid = app.GetType().GUID;
                Apps.RegisteredApplication regapp = Apps.ApplicationManager.CurrentManager.GetRegisteredApplication(guid);
                Common.KeyValueCollection pars = new Common.KeyValueCollection();
                pars.Add("APP", regapp.ID);
                bool ret = (GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.SavePrecedentWizard), null, precedent, pars) != null);
                if (ret)
                {
                    if (_frmprec != null)
                    {
                        _frmprec.Close();
                        _frmprec.Dispose();
                        _frmprec = null;
                    }
                }
                return ret;
            }

            /// <summary>
            /// Activates a system settings wizard.
            /// </summary>
            public static bool SaveSystemSettings(IWin32Window owner)
            {
                return (GetWizard(owner, Session.CurrentSession.DefaultSystemForm(SystemForms.SystemEdit), null, Session.OMS, null) != null);
            }


            /// <summary>
            /// Executes a static method on the wizard class layer. 
            /// </summary>
            /// <param name="method">Method to invoke.</param>
            /// <returns>Return value, if any.</returns>
            public static object Run(string method)
            {
                return Services.Run(typeof(Wizards).FullName, method, null, new Common.KeyValueCollection());
            }


            /// <summary>
            /// Executes a static method on the wizard class layer. 
            /// </summary>
            /// <param name="method">Method to invoke.</param>
            /// <param name="parameters">Parameters xml file, use null for no parameters.</param>
            /// <param name="replacementParameters">The values used to replace %n% parameters.</param>
            /// <returns>Return value, if any.</returns>
            public static object Run(string method, string parameters, Common.KeyValueCollection replacementParameters)
            {
                return Services.Run(typeof(Wizards).FullName, method, parameters, replacementParameters);
            }
            #endregion

        }
    }
}
