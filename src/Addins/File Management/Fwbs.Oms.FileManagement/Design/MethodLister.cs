using System.ComponentModel;

namespace FWBS.OMS.FileManagement.Design
{
    internal class MethodLister : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			Configuration.ActionConfig action = context.Instance as Configuration.ActionConfig;
			string [] methods = new string[0];
			if (action != null)
			{
				methods = action.Application.GetActionMethodNames();
			}

			return new StandardValuesCollection(methods);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

	}
}
