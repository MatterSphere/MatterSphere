using System;

namespace FWBS.OMS.FileManagement
{
	/// <summary>
    /// FileManagement exception.
	/// </summary>
	public sealed class FMException : FWBS.OMS.OMSException2
	{
		public FMException(string code, string description) : base(code, description)
		{
		}

		public FMException(string code, string description, Exception innerException) : base(code, description,innerException)
		{
		}
	
		public FMException(string code, string description, Exception innerException, bool parser, params string [] param) : base(code, description, innerException, parser, param)
		{
		}

	}
}
