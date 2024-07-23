using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// A list selection enquiry control which generally holds a caption and a combo box.  
    /// This particular control can be used for picking fixed items in a combo box style list.
    /// </summary>
    public class eComboBoxList2 : eComboBox2
	{
		#region Constructors
		/// <summary>
		/// Uses the already existing combo box object from its base control to change its
		/// Dropdown style.
		/// </summary>
		public eComboBoxList2() : base()
		{
			((ComboBox)_ctrl).DropDownStyle = ComboBoxStyle.DropDownList;
		}
		#endregion
	}
}
