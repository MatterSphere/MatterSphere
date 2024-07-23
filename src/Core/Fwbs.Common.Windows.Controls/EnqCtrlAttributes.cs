using System;

namespace FWBS.Common.UI
{
	/// <summary>
	/// Attribute used to supply a code for a custom control
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)] //should ohnly be used at class level
	public class ControlCodeAttribute : System.Attribute 
	{
		private string _code;
		private string _category;
		
		/// <summary>
		/// returns the internal code converted to uppercase for tidiness
		/// </summary>
		public string ControlCode               
		{
			get 
			{ 
				return _code.ToUpper(); 
			}
			
		}
		/// <summary>
		/// Category the control will appear under default is OMS
		/// </summary>
		public string ControlCategory
		{
			get
			{
				return _category.ToUpper();
			}
			set
			{
				_category = value;
			}
		}
		
		/// <summary>
		/// Constructor used to pass the control code
		/// </summary>
		/// <param name="code">code to use</param>
		public ControlCodeAttribute(string code)  
		{
			this._code = code;
			this._category = @"OMS";
		}

		
	}

}
