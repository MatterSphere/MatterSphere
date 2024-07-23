#region References
using System;
using System.Runtime.InteropServices;
#endregion

namespace FWBS.Sharepoint
{
	[ComImport]
	[Guid("E06BAFCD-756F-4AD9-A8CF-D313CA9DDA40")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface IMCSPIntegration
	{
		[PreserveSig]
		bool AddPropertyValuePair([In] [MarshalAs(UnmanagedType.LPWStr)] string name, [In] [MarshalAs(UnmanagedType.LPWStr)] string value);

		[PreserveSig]
		string InsertProperties([In] [MarshalAs(UnmanagedType.LPWStr)] string customXmlPart);
	}
}
