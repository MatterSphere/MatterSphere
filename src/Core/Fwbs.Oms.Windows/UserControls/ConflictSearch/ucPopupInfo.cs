using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using FWBS.OMS.Data;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls
{
    /// <summary>
    /// User control containing information about existing client / contact within the system.
    /// </summary>
    public partial class ucPopupInfo : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ucPopupInfo"/> class.
        /// </summary>
        /// <param name="code">Searchlist code.</param>
        /// <param name="id">Contact / Client ID.</param>
        public ucPopupInfo(string code, string id)
        {
            if (code != "SCHCLISEACHADV" && code != "SCHCONSRCHALL")
            {
                throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("SLNOTSUPP", "Searchlist ''%1%'' is not supported.", "", code).Text);
            }

            this.InitializeComponent();
            this.SetIcons();

            this.GenerateResults(code, id);
        }

        private void GenerateResults(string code, string id)
        {
            if (Session.CurrentSession?.IsConnected == false)
                return;

            int temp;
            if (!int.TryParse(id, out temp))
            {
                throw new ArgumentException(Session.CurrentSession.Resources.GetMessage("IDNOTVALID", "''%1%'' is not a valid ID.", "", id).Text, nameof(id));
            }

            try
            {
                string query = null;
                DataLists dl = null;

                if (code == "SCHCLISEACHADV")
                {
                    query =
                        $"select c.clid, clname as name, cltypecode as type, cladd1 as addline1, CONCAT(clAdd4, ' ', clPostCode) as location, cmp.contRegCoName, CONCAT(mat.ClientNumber, '-', mat.MatNo) AS matter  from vwDBClientHeader c left join dbContactCompany cmp on c.contID = cmp.contID left join vwDBMatters mat on CLNO = mat.ClientNumber where c.clID = '{id}'";
                    dl = new DataLists("DSCLIENTTYPE");
                }
                else if (code == "SCHCONSRCHALL")
                {
                    query =
                        $"select distinct c.contId, c.contName as name, c.contTypeCode as type, adr.addLine1, CONCAT(adr.addLine4, ' ', adr.addPostcode) AS location, cmp.contRegCoName, CONCAT(mat.ClientNumber, '-', mat.MatNo) AS matter from dbContact c left join dbAddress adr on c.contDefaultAddress = adr.addID left join dbContactCompany cmp on c.contID = cmp.contID left join dbAssociates soc on c.contID = soc.contID left join vwDBMatters mat on soc.fileID = mat.MatterID where c.contid = '{id}'";
                    dl = new DataLists("DSCONTTYPEALL");
                }

                if (query == null)
                {
                    return;
                }

                DataTableExecuteParameters pars = new DataTableExecuteParameters {Sql = query};
                DataTable dataTable = Session.CurrentSession?.CurrentConnection.Execute(pars);
                if (dataTable == null)
                {
                    return;
                }

                DataTable types = dl.Run() as DataTable;
                var typeParsed =
                    types?.Rows.First<DataRow>(xd =>
                        Convert.ToString(xd[0]) == Convert.ToString(dataTable.Rows[0]["type"]))[1];

                this.lblType.Text = Convert.ToString(typeParsed);

                this.lblName.Text = Convert.ToString(dataTable.Rows[0]["name"]);

                this.lblAddress.Text = Convert.ToString(dataTable.Rows[0]["addLine1"]);

                string location = Convert.ToString(dataTable.Rows[0]["location"]);
                this.lblLocation.Text = location != " "
                    ? location
                    : Session.CurrentSession.Resources.GetResource("NOLOCATIONFOUND", "No Location found", "").Text;

                string organization = Convert.ToString(dataTable.Rows[0]["contRegCoName"]);
                this.lblOrganization.Text = organization != string.Empty
                    ? organization
                    : Session.CurrentSession.Resources.GetResource("NOORGFOUND", "No Organization found", "").Text;

                string matter = Convert.ToString(dataTable.Rows[0]["matter"]);
                this.lblMatters.Text = matter != "-"
                    ? matter
                    : Session.CurrentSession.Resources.GetResource("NOMATTERSFOUND", "No Matters found", "").Text;

                for (var ind = 1; ind < dataTable.Rows.Count; ind++)
                {
                    this.lblMatters.Text += ", " + Convert.ToString(dataTable.Rows[ind]["matter"]);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void SetIcons()
        {
            this.MatterPicture.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "file");
            this.ContactPicture.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "contact");
        }

        private void ucPopupInfo_DpiChangedAfterParent(object sender, EventArgs e)
        {
            this.SetIcons();
        }
    }
}