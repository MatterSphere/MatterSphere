using System;

namespace FWBS.OMS.FileManagement
{
	public class ReturnValue
	{
		internal ReturnValue()
		{
		}

		public readonly static ReturnValue Success = new ReturnValue();
		public readonly static ReturnValue Failed = new ReturnValue();
	}
}
