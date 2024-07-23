using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using FWBS.OMS.UI.Windows.Design;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for OMSObject.
    /// </summary>
    public class OmsObject : FWBS.OMS.OmsObject
	{
		private readonly HashSet<string> _hiddenProps;

		#region Constructors
		
		/// <summary>
		/// Consructor will Load a OMS Object from the Database
		/// </summary>
		/// <param name="objCode"></param>
		public OmsObject(String objCode) : base(objCode)
		{
			if (!IsDashboardTileCompatible(objCode))
			{
				_hiddenProps = new HashSet<string> { "TileCode", "TileMinSize", "TilePriority", "UserRolesDisplay" };
			}
		}

        private bool IsDashboardTileCompatible(string objCode)
        {
            OMSObjectTypes objectType = ObjectType;
            if (objectType != OMSObjectTypes.List && objectType != OMSObjectTypes.Addin)
                return false;

            string typeCompatible = TypeCompatible.ToUpperInvariant();
            if (typeCompatible != "FWBS.OMS.USER" && typeCompatible != "FWBS.OMS.OMSFILE")
                return false;

            if (objectType == OMSObjectTypes.Addin &&
                (!objCode.StartsWith("DSH", StringComparison.InvariantCultureIgnoreCase) || DashboardSysObject.Exists(objCode)))
                return false;

            return true;
        }

		#endregion

		[Editor(typeof(CodeDescriptionEditor),typeof(UITypeEditor))]
		public override string DetailedDescription
		{
			get
			{
				return base.DetailedDescription;
			}
			set
			{
				base.DetailedDescription = value;
			}
		}

		protected override PropertyDescriptorCollection FilterProperties(List<PropertyDescriptor> props)
		{
			if (_hiddenProps != null)
			{
				props.RemoveAll(p => _hiddenProps.Contains(p.Name));
			}
			return base.FilterProperties(props);
		}

	}
}
