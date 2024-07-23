using System;
using System.Globalization;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.FileManagement;
using FWBS.OMS.SearchEngine;

namespace FWBS.OMS.UI
{
    using FWBS.OMS.UI.Windows;

    /// <summary>
    /// Static search class that runs different types of searches.  These searches include
    /// custom and globally used ones. The class can also be constructed as an instance search dialog.
    /// </summary>
    public class Search
    {
        #region Fields

        private SearchEngine.SearchList _searchList = null;
        private string _code = "";
        private FWBS.Common.KeyValueCollection _params = null;
        private System.Drawing.Size _size = new System.Drawing.Size(-1, -1);
        private object _parent = null;
        private bool _asType = true;
        private string _message = "";
        private bool _autoSelect = false;
        private bool _hidebuttons = false;
        private bool _hidebuttonsEdit = false;

        #endregion

        #region Constructors

        public Search()
        {
        }

        public Search(string code)
        {
            _code = code;
        }

        public Search(SearchEngine.SearchList searchList)
        {
            _searchList = searchList;
            _code = _searchList.Code;
        }

        #endregion

        #region Properties

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }


        public FWBS.Common.KeyValueCollection Parameters
        {
            get
            {
                return _params;
            }
            set
            {
                _params = value;
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public object Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public bool AsType
        {
            get
            {
                return _asType;
            }
            set
            {
                _asType = value;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public bool AutoSelect
        {
            get
            {
                return _autoSelect;
            }
            set
            {
                _autoSelect = value;
            }
        }
        /// <summary>
        /// For cosmetic reason there may be a requirement to hide the buttons on the search form
        /// </summary>
        public bool HideButtonsOnEdit
        {
            get
            {
                return _hidebuttonsEdit;
            }
            set
            {
                _hidebuttonsEdit = value;
            }
        }

        public bool HideButtons
        {
            get
            {
                return _hidebuttons;
            }
            set
            {
                HideButtonsOnEdit = value;
                _hidebuttons = value;
            }
        }

        #endregion

        #region Instance Methods

        public FWBS.Common.KeyValueCollection Show(IWin32Window owner)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (Services.CheckLogin())
                {
                    frmSearch frm = null;

                    if (_searchList == null)
                        frm = new frmSearch(_code, _asType, _parent, _params, _autoSelect, Common.TriState.Null);
                    else
                        frm = new frmSearch(_searchList, _autoSelect, Common.TriState.Null);

                    if (_hidebuttonsEdit) frm.ButtonPanelHiddenOnEdit = true;
                    if (_hidebuttons) frm.ButtonPanelHidden = true;
                    if (_size.Width != -1 && _size.Height != -1) frm.Size = _size;
                    frm.Message = _message;
                    if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                    {
                        FWBS.Common.KeyValueCollection retVals = frm.ReturnValues;
                        frm.Dispose();
                        return retVals;
                    }
                    else
                    {
                        frm.Dispose();
                        return null;
                    }
                }
                else
                    return null;
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

        public FWBS.Common.KeyValueCollection[] ShowEx(IWin32Window owner)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (Services.CheckLogin())
                {
                    frmSearch frm = null;

                    if (_searchList == null)
                        frm = new frmSearch(_code, _asType, _parent, _params, _autoSelect, Common.TriState.True);
                    else
                        frm = new frmSearch(_searchList, _autoSelect, Common.TriState.True);

                    if (_hidebuttonsEdit) frm.ButtonPanelHiddenOnEdit = true;
                    if (_hidebuttons) frm.ButtonPanelHidden = true;
                    if (_size.Width != -1 && _size.Height != -1) frm.Size = _size;
                    frm.Message = _message;
                    if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                    {
                        FWBS.Common.KeyValueCollection[] retVals = frm.SelectedItems;
                        frm.Dispose();
                        return retVals;
                    }
                    else
                    {
                        frm.Dispose();
                        return null;
                    }
                }
                else
                    return null;
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

        public FWBS.Common.KeyValueCollection Show()
        {
            return Show(null);
        }


        #endregion

    }


}

namespace FWBS.OMS.UI.Windows
{
    using FWBS.OMS.UI.Windows.DocumentManagement;

    partial class Services
    {
        public sealed class Searches : FWBS.OMS.UI.Search
        {
            public Searches()
            {}
        

            public Searches(string code) : base(code)
            {
            }

            public Searches(SearchEngine.SearchList searchList) : base(searchList)
            {
            }


            /// <summary>
            /// Displays the file conflict search screen on an already existing file.
            /// </summary>
            /// <param name="file">The file to conflict check for.</param>
            public static void ShowFileConflict(IWin32Window owner, OMSFile file)
            {
                if (file == null)
                    file = Services.SelectFile(owner);
                if (file == null) return;

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (Services.CheckLogin())
                    {
                        using (frmConflictSearch frm = new frmConflictSearch(file))//;
                        {
                            frm.ShowDialog(owner);
                        }
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            public static void ShowFileConflict(OMSFile file)
            {
                ShowFileConflict(null, file);
            }

            /// <summary>
            /// Displays a precedent search screen.
            /// </summary>
            /// <returns></returns>
            internal static Precedent FindPrecedent(IWin32Window owner, System.Windows.Forms.Design.IWindowsFormsEditorService IWFES)
            {
                FWBS.Common.KeyValueCollection ret = null;

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (Services.CheckLogin())
                    {
                        using (frmSearchPrecedent frm = new frmSearchPrecedent())//;
                        {
                            if (IWFES != null)
                            {
                                if (IWFES.ShowDialog(frm) == System.Windows.Forms.DialogResult.OK)
                                    ret = frm.ReturnValues;
                            }
                            else
                            {
                                if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                                    ret = frm.ReturnValues;
                            }
                        }
                    }

                    if (ret == null || ret.Count == 0)
                        return null;

                    long precid = 0;

                    try
                    {
                        precid = Convert.ToInt64(ret["PRECID"].Value);
                    }
                    catch
                    {
                        return null;
                    }
                    try
                    {
                        return Precedent.GetPrecedent(precid);
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(owner, ex);
                        return null;
                    }

                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }


            }

            public static Precedent FindPrecedent(System.Windows.Forms.Design.IWindowsFormsEditorService IWFES)
            {
                return FindPrecedent(null, IWFES);
            }

            public static Precedent FindPrecedent(IWin32Window owner)
            {
                return FindPrecedent(owner, null);
            }

            public static Precedent FindPrecedent()
            {
                return FindPrecedent(null, null);
            }
          
            public static Team FindTeam(Func<bool> checkAccess, out bool accessDenied)
            {
                FWBS.Common.KeyValueCollection ret = null;
                accessDenied = false;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (Services.CheckLogin())
                    {
                        accessDenied = !checkAccess();
                        if (accessDenied)
                        {
                            return null;
                        }
                        var collection = new KeyValueCollection();
                        var culture = CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                        collection.Add("UI", culture.Name);

                        SearchList list = new SearchList("SCHSYSTEAMLST", null, collection);
                        using (frmSearch frm = new frmSearch(list, false, TriState.False))
                        {
                            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                ret = frm.ReturnValues;
                        }
                    }

                    if (ret == null || ret.Count == 0)
                        return null;

                    try
                    {
                        int teamid = Convert.ToInt32(ret["tmId"].Value);
                        return Team.GetTeam(teamid);
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ex);
                        return null;
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            /// <summary>
            /// Displays a search form based on the search type passed.
            /// </summary>
            /// <param name="code">Search type / code to use.</param>
            /// <param name="asType">Specifies that the code is a group type.</param>
            /// <param name="param">Replacement parameters.</param>
            /// <param name="size">Size of form NB: Leave null if not required</param>
            /// <param name="parent">The parent object for the search.</param>
            /// <returns>Returns an object array of values that uniquely identify the chosen row within the search list.</returns> 
            public static FWBS.Common.KeyValueCollection ShowSearch(IWin32Window owner, string code, bool asType, System.Drawing.Size size, object parent, FWBS.Common.KeyValueCollection param)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (Services.CheckLogin())
                    {
                        using (frmSearch frm = new frmSearch(code, asType, parent, param, false, Common.TriState.Null))
                        {
                            if (size.Width != -1 && size.Height != -1) frm.Size = frm.LogicalToDeviceUnits(size);
                            if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                            {
                                FWBS.Common.KeyValueCollection retVals = frm.ReturnValues;
                                return retVals;
                            }
                            else
                                return null;
                        }
                    }
                    else
                        return null;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }

            }
                                
            public static FWBS.Common.KeyValueCollection ShowSearch(string type, System.Drawing.Size size, object parent, FWBS.Common.KeyValueCollection param)
            {
                return ShowSearch(null, type, true, size, parent, param);
            }

            public static FWBS.Common.KeyValueCollection ShowSearch(string type, object parent, FWBS.Common.KeyValueCollection param)
            {
                return ShowSearch(null, type, true, new System.Drawing.Size(-1, -1), parent, param);
            }

            public static FWBS.Common.KeyValueCollection ShowSearch(IWin32Window owner, string type, object parent, FWBS.Common.KeyValueCollection param)
            {
                return ShowSearch(owner, type, true, new System.Drawing.Size(-1, -1), parent, param);
            }


            
            /// <summary>
            /// Displays a search form based on the search type passed.
            /// </summary>
            /// <param name="code">Search type / code to use.</param>
            /// <param name="asType">Specifies that the code is a group type.</param>
            /// <param name="param">Replacement parameters.</param>
            /// <param name="size">Size of form NB: Leave null if not required</param>
            /// <param name="parent">The parent object for the search.</param>
            /// <returns>Returns an object array of values for the selected rows within the search list.</returns> 
            /// <returns></returns>
            public static FWBS.Common.KeyValueCollection[] ShowSearchMulti(IWin32Window owner, string code, bool asType, System.Drawing.Size size, object parent, FWBS.Common.KeyValueCollection param)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (Services.CheckLogin())
                    {
                        using (frmSearch frm = new frmSearch(code, asType, parent, param, false, Common.TriState.Null))
                        {
                            if (size.Width != -1 && size.Height != -1) frm.Size = size;
                            if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                            {
                                FWBS.Common.KeyValueCollection[] retVals = frm.SelectedItems;
                                return retVals;
                            }
                            else
                                return null;
                        }
                    }
                    else
                        return null;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            
            public static FWBS.Common.KeyValueCollection[] ShowSearchMulti(string type, System.Drawing.Size size, object parent, FWBS.Common.KeyValueCollection param)
            {
                return ShowSearchMulti(null, type, true, size, parent, param);
            }

            public static FWBS.Common.KeyValueCollection[] ShowSearchMulti(string type, object parent, FWBS.Common.KeyValueCollection param)
            {
                return ShowSearchMulti(null, type, true, new System.Drawing.Size(-1, -1), parent, param);
            }

            public static FWBS.Common.KeyValueCollection[] ShowSearchMulti(IWin32Window owner, string type, object parent, FWBS.Common.KeyValueCollection param)
            {
                return ShowSearchMulti(owner, type, true, new System.Drawing.Size(-1, -1), parent, param);
            }



            public static Associate[] PickAssociates(IWin32Window owner, OMSFile file)
            {
                Search sch = new Search();
                sch.Code = FWBS.OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.AssociateFilter);
                sch.Message = String.Empty;
                sch.Parent = file;
                sch.AsType = false;
                sch.AutoSelect = false;

                FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
                pars.Add("CONTTYPE", String.Empty);
                pars.Add("ASSOCTYPE", String.Empty);
                sch.Parameters = pars;

                FWBS.Common.KeyValueCollection[] rets = sch.ShowEx(owner);
                System.Collections.Generic.List<Associate> list = new System.Collections.Generic.List<Associate>();

                if (rets == null || rets.Length == 0)
                    return null;

                foreach (Common.KeyValueCollection r in rets)
                {
                    if (r == null || r.Count == 0)
                        continue;
                    try
                    {
                        list.Add(Associate.GetAssociate(Convert.ToInt64(r["ASSOCID"].Value)));
                    }
                    catch
                    {
                        continue;
                    }
                }

                return list.ToArray();
            }

            public static Associate[] PickAssociates(OMSFile file)
            {
                return PickAssociates(null, file);
            }



            /// <summary>
            /// Displays a list of associates on a file.
            /// </summary>
            /// <returns>An associate object.</returns>
            public static Associate PickAssociate(IWin32Window owner, OMSFile file, string contactType, string assocType, string message, bool autoSelect, bool hideButtons, bool simple = false)
            {
                Search sch = new Search();
                sch.Code = FWBS.OMS.Session.CurrentSession.DefaultSystemSearchList(simple ? SystemSearchLists.AssociatesShort : SystemSearchLists.AssociateFilter);
                sch.Message = message;
                sch.Parent = file;
                sch.AsType = false;
                sch.AutoSelect = autoSelect;

                FWBS.Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
                pars.Add("CONTTYPE", contactType);
                pars.Add("ASSOCTYPE", assocType);

                sch.Parameters = pars;

                sch.HideButtonsOnEdit = hideButtons;

                FWBS.Common.KeyValueCollection ret = sch.Show(owner);

                if (ret == null || ret.Count == 0)
                    return null;

                long associd = 0;
                try
                {
                    associd = Convert.ToInt64(ret["ASSOCID"].Value);
                }
                catch
                {
                    return null;
                }
                try
                {
                    return Associate.GetAssociate(associd);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(owner, ex);
                    return null;
                }
            }


            /// <summary>
            /// Displays a list of associates on a file.
            /// </summary>
            /// <returns>An associate object.</returns>
            public static Associate PickAssociate(IWin32Window owner, OMSFile file, string contactType, string assocType, string message, bool autoSelect)
            {
                return PickAssociate(owner, file, contactType, assocType, message, autoSelect, false);
            }

            public static Associate PickAssociate(IWin32Window owner, OMSFile file, bool simple = false)
            {
                return PickAssociate(owner, file, "", "", "", false, false, simple);
            }

            public static Associate PickAssociate(OMSFile file, bool simple = false)
            {
                return PickAssociate(null, file, "", "", "", false, false, simple);
            }

            /// <summary>
            /// Picks an appointment from a files appointment list.
            /// </summary>
            /// <param name="file"></param>
            /// <returns></returns>
            public static Appointment PickAppointment(IWin32Window owner, OMSFile file, string message)
            {
                Search sch = new Search();
                sch.Code = FWBS.OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.Appointments);
                sch.AsType = false;
                sch.Parent = file;
                sch.Message = message;
                sch.AutoSelect = true;
                FWBS.Common.KeyValueCollection ret = sch.Show(owner);
                if (ret == null || ret.Count == 0)
                    return null;

                int apntid = 0;
                try
                {
                    apntid = Convert.ToInt32(ret["APPID"].Value);
                }
                catch
                {
                    return null;
                }
                try
                {
                    return Appointment.GetAppointment(apntid);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(owner, ex);
                    return null;
                }
            }

            public static Appointment PickAppointment(OMSFile file)
            {
                return PickAppointment(null, file, "");
            }

            /// <summary>
            /// Displays the address search screen.
            /// </summary>
            /// <returns>An address object.</returns>
            public static Address FindAddress(IWin32Window owner)
            {
                FWBS.Common.KeyValueCollection ret = ShowSearch(owner, Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.Address), null, null);
                if (ret == null || ret.Count == 0)
                    return null;

                long addid = 0;
                try
                {
                    addid = Convert.ToInt64(ret["ADDID"].Value);
                }
                catch
                {
                    return null;
                }
                try
                {
                    return Address.GetAddress(addid);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(owner, ex);
                    return null;
                }
            }

            public static Address FindAddress()
            {
                return FindAddress(null);
            }

            /// <summary>
            /// Fetches an address from an external source (web service) and creates a new address object
            /// ready to use.
            /// </summary>
            [Obsolete("This method has been deprecated in V10.1")]
            public static Address FindAddressEx(IWin32Window owner)
            {

                FWBS.Common.KeyValueCollection ret = ShowSearch(owner, Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.AddressPostcode), false, new System.Drawing.Size(-1, -1), null, null);
                if (ret == null || ret.Count == 0)
                    return null;

                string addid = "";
                try
                {
                    addid = Convert.ToString(ret["Id"].Value);
                }
                catch
                {
                    return null;
                }
                try
                {
                    return Address.GetAddress(addid);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(owner, ex);
                    return null;
                }
            }

            [Obsolete("This method has been deprecated in V10.1")]
            public static Address FindAddressEx()
            {
                return FindAddressEx(null);
            }


            /// <summary>
            /// Displays the user search screen.
            /// </summary>
            /// <returns>An OMS User object.</returns>
            public static User FindUser(IWin32Window owner)
            {
                FWBS.Common.KeyValueCollection ret = ShowSearch(owner, Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.User), null, null);
                if (ret == null || ret.Count == 0)
                    return null;

                int usrid = 0;
                try
                {
                    usrid = Convert.ToInt32(ret["USRID"].Value);
                }
                catch
                {
                    return null;
                }
                try
                {
                    return User.GetUser(usrid);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(owner, ex);
                    return null;
                }
            }

            public static User FindUser()
            {
                return FindUser(null);
            }

            /// <summary>
            /// Displays the fee earner search screen.
            /// </summary>
            /// <returns>An OMS User object.</returns>
            [Obsolete("No longer supported in 3E MatterSphere")]
            public static FeeEarner FindFeeEarner(IWin32Window owner)
            {
                FWBS.Common.KeyValueCollection ret = ShowSearch(owner, Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.FeeEarner), null, null);
                if (ret == null || ret.Count == 0)
                    return null;

                int feeid = 0;
                try
                {
                    feeid = Convert.ToInt32(ret["FEEUSRID"].Value);
                }
                catch
                {
                    return null;
                }
                try
                {
                    return FeeEarner.GetFeeEarner(feeid);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(owner, ex);
                    return null;
                }
            }

            [Obsolete("No longer supported in 3E MatterSphere")]
            public static FeeEarner FindFeeEarner()
            {
                return FindFeeEarner(null);
            }

            /// <summary>
            /// Displays the document search screen based on a file identifier filter.
            /// </summary>
            /// <param name="fileID">The file to base the search on.</param>
            /// <returns>An OMS Document object.</returns>
            public static OMSDocument FindDocument(IWin32Window owner, OMSFile file)
            {
                DocumentPicker picker = new DocumentPicker();
                if (file == null)
                    picker.Type = DocumentPickerType.Search;
                else
                    picker.Type = DocumentPickerType.File;
                picker.File = file;
                OMSDocument[] docs = picker.Show(owner);
                if (docs != null && docs.Length > 0)
                    return docs[0];
                else
                    return null;

            }

            public static OMSDocument FindDocument(OMSFile file)
            {
                return FindDocument(null, file);
            }

            /// <summary>
            /// Displays the document list screen based on a file identifier filter.
            /// </summary>
            /// <returns>An OMS Document object.</returns>
            public static OMSDocument PickDocument(IWin32Window owner, OMSFile file)
            {
                DocumentPicker picker = new DocumentPicker();
                picker.Type = DocumentPickerType.File;
                picker.File = file;
                OMSDocument[] docs = picker.Show(owner);
                if (docs != null && docs.Length > 0)
                    return docs[0];
                else
                    return null;

            }


            public static OMSDocument PickDocument(OMSFile file)
            {
                return PickDocument(null, file);
            }

            public static OMSDocument[] PickDocuments(IWin32Window owner, OMSFile file)
            {
                DocumentPicker picker = new DocumentPicker();
                picker.Type = DocumentPickerType.File;
                picker.File = file;
                return picker.Show(owner);
            }



            /// <summary>
            /// Displays the client search screen.
            /// </summary>
            /// <returns>A found client object.</returns>
            public static Client FindClient(IWin32Window owner)
            {
                FWBS.Common.KeyValueCollection ret = ShowSearch(owner, Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.Client), true, new System.Drawing.Size(750, 590), null, null);
                if (ret == null || ret.Count == 0)
                    return null;

                long clid = 0;
                try
                {
                    clid = Convert.ToInt64(ret["CLID"].Value);
                }
                catch
                {
                    return null;
                }
                try
                {
                    return Client.GetClient(clid);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(owner, ex);
                    return null;
                }
            }

            public static Client FindClient()
            {
                return FindClient(null);
            }

            /// <summary>
            /// Displays the contact search screen.
            /// </summary>
            /// <returns>A found contact object.</returns>
            public static Contact FindContact(IWin32Window owner)
            {
                FWBS.Common.KeyValueCollection ret = ShowSearch(owner, Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.ContactConflict), true, new System.Drawing.Size(750, 590), null, null);
                if (ret == null || ret.Count == 0)
                    return null;

                long contid = 0;
                try
                {
                    contid = Convert.ToInt64(ret["CONTID"].Value);
                }
                catch
                {
                    return null;
                }
                try
                {
                    return Contact.GetContact(contid);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(owner, ex);
                    return null;
                }
            }

            public static Contact FindContact()
            {
                return FindContact(null);
            }

            /// <summary>
            /// Displays the file search screen.
            /// </summary>
            /// <returns>A found file object.</returns>
            public static OMSFile FindFile(IWin32Window owner)
            {
                FWBS.Common.KeyValueCollection ret = ShowSearch(owner, Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.ClientFile), true, new System.Drawing.Size(750, 590), null, null);
                if (ret == null || ret.Count == 0)
                    return null;

                long fileid = 0;
                try
                {
                    fileid = Convert.ToInt64(ret["FILEID"].Value);
                }
                catch
                {
                    return null;
                }
                try
                {
                    return OMSFile.GetFile(fileid);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(owner, ex);
                    return null;
                }
            }


            public static OMSFile FindFile()
            {
                return FindFile(null);
            }
        }
    
    }
}
