using System;

namespace FWBS.OMS.Extensibility
{
	public enum LoadBehaviour
	{
		Inactive = 0,
		Startup = 1,
		OnDemand = 2
	}

	public enum AddinStatus
	{
		Unloaded,
		Loaded,
		Disabled,
		Errors
	}
}
