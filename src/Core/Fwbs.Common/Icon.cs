using System;
using System.Runtime.InteropServices;

namespace FWBS.Common
{
	/// <summary>
	/// Provides static methods to read system icons for both folders and files.
	/// </summary>
	/// <example>
	/// <code>IconReader.GetFileIcon("c:\\general.xls");</code>
	/// </example>
	public class IconReader
	{
		/// <summary>
		/// Options to specify the size of icons to return.
		/// </summary>
		public enum IconSize
		{
			/// <summary>
			/// Specify large icon - 32 pixels by 32 pixels.
			/// </summary>
			Large = 0,
			/// <summary>
			/// Specify small icon - 16 pixels by 16 pixels.
			/// </summary>
			Small = 1
		}
       

		/// <summary>
		/// Returns an icon for a given file - indicated by the name parameter.
		/// </summary>
		/// <param name="name">Pathname for file.</param>
		/// <param name="size">Large or small</param>
		/// <param name="linkOverlay">Whether to include the link icon</param>
		/// <returns>System.Drawing.Icon</returns>
		public static System.Drawing.Icon GetFileIcon(string name, IconSize size, bool linkOverlay)
		{
			Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
			uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES | Shell32.SHGFI_TYPENAME;

			if (true == linkOverlay) flags += Shell32.SHGFI_LINKOVERLAY;

			/* Check the size specified for return. */
			if (IconSize.Small == size)
			{
				flags += Shell32.SHGFI_SMALLICON ;
			} 
			else 
			{
				flags += Shell32.SHGFI_LARGEICON ;
			}

			Shell32.SHGetFileInfo(	name, 
				Shell32.FILE_ATTRIBUTE_NORMAL, 
				ref shfi, 
				(uint) System.Runtime.InteropServices.Marshal.SizeOf(shfi), 
				flags );

			// Copy (clone) the returned icon to a new object, thus allowing us to clean-up properly
			System.Drawing.Icon icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
			User32.DestroyIcon( shfi.hIcon );		// Cleanup
			return icon;
		}

	
	}

	internal class Shell32  
	{
		
		public const int 	MAX_PATH = 256;
		[StructLayout(LayoutKind.Sequential)]
			public struct SHITEMID
		{
			public ushort cb;
			[MarshalAs(UnmanagedType.LPArray)]
			public byte[] abID;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct ITEMIDLIST
		{
			public SHITEMID mkid;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct BROWSEINFO 
		{ 
			public IntPtr		hwndOwner; 
			public IntPtr		pidlRoot; 
			public IntPtr 		pszDisplayName;
			[MarshalAs(UnmanagedType.LPTStr)] 
			public string 		lpszTitle; 
			public uint 		ulFlags; 
			public IntPtr		lpfn; 
			public int			lParam; 
			public IntPtr 		iImage; 
		} 

		// Browsing for directory.
		public const uint BIF_RETURNONLYFSDIRS   =	0x0001;
		public const uint BIF_DONTGOBELOWDOMAIN  =	0x0002;
		public const uint BIF_STATUSTEXT         =	0x0004;
		public const uint BIF_RETURNFSANCESTORS  =	0x0008;
		public const uint BIF_EDITBOX            =	0x0010;
		public const uint BIF_VALIDATE           =	0x0020;
		public const uint BIF_NEWDIALOGSTYLE     =	0x0040;
		public const uint BIF_USENEWUI           =	(BIF_NEWDIALOGSTYLE | BIF_EDITBOX);
		public const uint BIF_BROWSEINCLUDEURLS  =	0x0080;
		public const uint BIF_BROWSEFORCOMPUTER  =	0x1000;
		public const uint BIF_BROWSEFORPRINTER   =	0x2000;
		public const uint BIF_BROWSEINCLUDEFILES =	0x4000;
		public const uint BIF_SHAREABLE          =	0x8000;

		[StructLayout(LayoutKind.Sequential)]
			public struct SHFILEINFO
		{ 
			public const int NAMESIZE = 80;
			public IntPtr	hIcon; 
			public int		iIcon; 
			public uint	dwAttributes; 
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAX_PATH)]
			public string szDisplayName; 
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=NAMESIZE)]
			public string szTypeName; 
		};

		public const uint SHGFI_ICON				= 0x000000100;     // get icon
		public const uint SHGFI_DISPLAYNAME			= 0x000000200;     // get display name
		public const uint SHGFI_TYPENAME          	= 0x000000400;     // get type name
		public const uint SHGFI_ATTRIBUTES        	= 0x000000800;     // get attributes
		public const uint SHGFI_ICONLOCATION      	= 0x000001000;     // get icon location
		public const uint SHGFI_EXETYPE           	= 0x000002000;     // return exe type
		public const uint SHGFI_SYSICONINDEX      	= 0x000004000;     // get system icon index
		public const uint SHGFI_LINKOVERLAY       	= 0x000008000;     // put a link overlay on icon
		public const uint SHGFI_SELECTED          	= 0x000010000;     // show icon in selected state
		public const uint SHGFI_ATTR_SPECIFIED    	= 0x000020000;     // get only specified attributes
		public const uint SHGFI_LARGEICON         	= 0x000000000;     // get large icon
		public const uint SHGFI_SMALLICON         	= 0x000000001;     // get small icon
		public const uint SHGFI_OPENICON          	= 0x000000002;     // get open icon
		public const uint SHGFI_SHELLICONSIZE     	= 0x000000004;     // get shell size icon
		public const uint SHGFI_PIDL              	= 0x000000008;     // pszPath is a pidl
		public const uint SHGFI_USEFILEATTRIBUTES 	= 0x000000010;     // use passed dwFileAttribute
		public const uint SHGFI_ADDOVERLAYS       	= 0x000000020;     // apply the appropriate overlays
		public const uint SHGFI_OVERLAYINDEX      	= 0x000000040;     // Get the index of the overlay

		public const uint FILE_ATTRIBUTE_DIRECTORY  = 0x00000010;  
		public const uint FILE_ATTRIBUTE_NORMAL     = 0x00000080;  

		[DllImport("Shell32.dll")]
		public static extern IntPtr SHGetFileInfo(
			string pszPath,
			uint dwFileAttributes,
			ref SHFILEINFO psfi,
			uint cbFileInfo,
			uint uFlags
			);
	}

	internal class User32
	{
		/// <summary>
		/// Provides access to function required to delete handle. This method is used internally
		/// and is not required to be called separately.
		/// </summary>
		/// <param name="hIcon">Pointer to icon handle.</param>
		/// <returns>N/A</returns>
		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool DestroyIcon( IntPtr hIcon );
	}
}
