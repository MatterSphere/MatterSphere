using System;

namespace FWBS.OMS.Security
{
    [Flags]
	public enum SecurityOptions : long
	{
		IsSecured = 1,
		IsExternallyVisible = 2,
	}

    public interface ISecurable
    {
        string SecurityId { get;}
		SecurityOptions SecurityOptions { get; set; }
    }
}
