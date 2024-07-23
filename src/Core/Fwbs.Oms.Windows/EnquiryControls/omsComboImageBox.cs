using System.ComponentModel;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eComboImageBox.
    /// </summary>
    public enum omsImageLists{None, imgFolderForms16,imgFolderForms32,CoolButtons16,ToolsButton16,CoolButtons24,Arrows,Entities32,Entities16,Developments16,AdminMenu32,AdminMenu16,Windows8,PlusMinus}
	public class omsComboImageBox : FWBS.Common.UI.Windows.eComboImageBox
	{
		#region Fields
		private omsImageLists _omsimagelists = omsImageLists.None;
		#endregion

		#region Constructors
		public omsComboImageBox() : base()
		{

		}
		#endregion

		#region Properties
		[Category("Appearance")]
		[DefaultValue(omsImageLists.None)]
		public omsImageLists Resources
		{
			get
			{
				return _omsimagelists;
			}
			set
			{
				if (_omsimagelists != value)
				{
					switch (value)
					{
						case omsImageLists.AdminMenu16:
						{
							_imagelist = Images.AdminMenu16();
							break;
						}
						case omsImageLists.AdminMenu32:
						{
							_imagelist = Images.AdminMenu32();
							break;
						}
						case omsImageLists.Arrows:
						{
							_imagelist = Images.Arrows;
							break;
						}
                        case omsImageLists.PlusMinus:
                        {
                            _imagelist = Images.PlusMinus;
                            break;
                        }
						case omsImageLists.CoolButtons16:
						{
							_imagelist = Images.CoolButtons16();
							break;
						}
						case omsImageLists.CoolButtons24:
						{
                            _imagelist = Images.GetCoolButtons24();
							break;
						}
						case omsImageLists.Developments16:
						{
							_imagelist = Images.Developments();
							break;
						}
						case omsImageLists.Entities16:
						{
							_imagelist = Images.Entities();
							break;
						}
						case omsImageLists.Entities32:
						{
							_imagelist = Images.Entities32();
							break;
						}
						case omsImageLists.imgFolderForms16:
						{
                            _imagelist = Images.GetFolderFormsIcons(Images.IconSize.Size16);
							break;
						}
						case omsImageLists.imgFolderForms32:
						{
                            _imagelist = Images.GetFolderFormsIcons(Images.IconSize.Size32);
							break;
						}
						case omsImageLists.None:
						{
							_imagelist = null;
							break;
						}
                        case omsImageLists.Windows8:
                        {
                            _imagelist = Images.Windows8();
                            break;
                        }
					}
				}
				_omsimagelists = value;
			}
		}

        #endregion
    }
}
