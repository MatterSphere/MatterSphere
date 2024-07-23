using System.Drawing;
using System.Reflection;
using System.Resources;

namespace FWBS.Common
{
    public enum VirtualKeys
	{
		VK_LBUTTON		= 0x01,
		VK_CANCEL		= 0x03,
		VK_BACK			= 0x08,
		VK_TAB			= 0x09,
		VK_CLEAR		= 0x0C,
		VK_RETURN		= 0x0D,
		VK_SHIFT		= 0x10,
		VK_CONTROL		= 0x11,
		VK_MENU			= 0x12,
		VK_CAPITAL		= 0x14,
		VK_ESCAPE		= 0x1B,
		VK_SPACE		= 0x20,
		VK_PRIOR		= 0x21,
		VK_NEXT			= 0x22,
		VK_END			= 0x23,
		VK_HOME			= 0x24,
		VK_LEFT			= 0x25,
		VK_UP			= 0x26,
		VK_RIGHT		= 0x27,
		VK_DOWN			= 0x28,
		VK_SELECT		= 0x29,
		VK_EXECUTE		= 0x2B,
		VK_SNAPSHOT		= 0x2C,
		VK_HELP			= 0x2F,
		VK_0			= 0x30,
		VK_1			= 0x31,
		VK_2			= 0x32,
		VK_3			= 0x33,
		VK_4			= 0x34,
		VK_5			= 0x35,
		VK_6			= 0x36,
		VK_7			= 0x37,
		VK_8			= 0x38,
		VK_9			= 0x39,
		VK_A			= 0x41,
		VK_B			= 0x42,
		VK_C			= 0x43,
		VK_D			= 0x44,
		VK_E			= 0x45,
		VK_F			= 0x46,
		VK_G			= 0x47,
		VK_H			= 0x48,
		VK_I			= 0x49,
		VK_J			= 0x4A,
		VK_K			= 0x4B,
		VK_L			= 0x4C,
		VK_M			= 0x4D,
		VK_N			= 0x4E,
		VK_O			= 0x4F,
		VK_P			= 0x50,
		VK_Q			= 0x51,
		VK_R			= 0x52,
		VK_S			= 0x53,
		VK_T			= 0x54,
		VK_U			= 0x55,
		VK_V			= 0x56,
		VK_W			= 0x57,
		VK_X			= 0x58,
		VK_Y			= 0x59,
		VK_Z			= 0x5A,
		VK_NUMPAD0		= 0x60,
		VK_NUMPAD1		= 0x61,
		VK_NUMPAD2		= 0x62,
		VK_NUMPAD3		= 0x63,
		VK_NUMPAD4		= 0x64,
		VK_NUMPAD5		= 0x65,
		VK_NUMPAD6		= 0x66,
		VK_NUMPAD7		= 0x67,
		VK_NUMPAD8		= 0x68,
		VK_NUMPAD9		= 0x69,
		VK_MULTIPLY		= 0x6A,
		VK_ADD			= 0x6B,
		VK_SEPARATOR	= 0x6C,
		VK_SUBTRACT		= 0x6D,
		VK_DECIMAL		= 0x6E,
		VK_DIVIDE		= 0x6F,
		VK_ATTN			= 0xF6,
		VK_CRSEL		= 0xF7,
		VK_EXSEL		= 0xF8,
		VK_EREOF		= 0xF9,
		VK_PLAY			= 0xFA,  
		VK_ZOOM			= 0xFB,
		VK_NONAME		= 0xFC,
		VK_PA1			= 0xFD,
		VK_OEM_CLEAR	= 0xFE,
		VK_LWIN			= 0x5B,
		VK_RWIN			= 0x5C,
		VK_APPS			= 0x5D,   
		VK_LSHIFT		= 0xA0,   
		VK_RSHIFT		= 0xA1,   
		VK_LCONTROL		= 0xA2,   
		VK_RCONTROL		= 0xA3,   
		VK_LMENU		= 0xA4,   
		VK_RMENU		= 0xA5
	}

	/// <summary>
	/// A static class that accessed the resource file for displayed information for exceptions etc...
	/// </summary>
	sealed public class ApplicationResource
	{
		private string _baseName = "";
		
		/// <summary>
		/// Disables the creation of a class.
		/// </summary>
		private ApplicationResource(){}

		public ApplicationResource(string baseName)
		{
			_baseName = baseName;
		}

		/// <summary>
		/// Returns a string based on a resource ID.
		/// </summary>
		/// <param name="resID">Resource key value.</param>
		/// <param name="param">Array List of parameters to be replaced.</param>
		/// <returns>String resource.</returns>
		public string GetResString(string resID, params string [] param)
		{
			string text = GetResString(resID);
			for (int ctr = param.GetLowerBound(0); ctr <= param.GetUpperBound(0); ctr++)
				text = text.Replace("%" + (ctr+1).ToString(), param[ctr].ToString());
			return text;
		}

		public string GetResString(string resID)
		{
			ResourceManager rm;
			string ret = "";
			rm = new ResourceManager(_baseName, Assembly.GetCallingAssembly());
			ret = rm.GetString(resID);
			return ret;
		}


		/// <summary>
		/// Returns a bitmap from a resource file.
		/// </summary>
		/// <param name="resID">String resource identifier.</param>
		/// <returns>Returns a bitmap picture.</returns>
		public Bitmap GetPicture(string resID)
		{
			ResourceManager rm;
			Bitmap bmp = null;
			rm = new ResourceManager(_baseName, Assembly.GetCallingAssembly());
			bmp = (Bitmap)rm.GetObject(resID);
			return bmp;
		}

		
	}
	
}