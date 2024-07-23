using System;

namespace FWBS.Common
{
	/// <summary>
	/// Provides COM based wrapper functions..
	/// </summary>
	public sealed class COM
	{
		private COM()
		{
		}

		public static void DisposeObject(object obj)
		{
			try
			{
				if (obj == null)
					return;
				
				int refCount = System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
			
				while (refCount > 0)
					   refCount = System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
			}
			catch
			{
				
			}
			finally
			{
				obj = null;
			}
		}
		
	}
}
