using System;
using System.Text;

namespace FWBS.OMS.Design.CodeBuilder
{
	internal partial class NewWFActivityForm : FWBS.OMS.UI.Windows.BaseForm
    {
		#region Constructors
		internal NewWFActivityForm()
		{
			InitializeComponent();

			this.IsCSharp = true;
		}

		internal NewWFActivityForm(bool isCsharp)
			: this()
		{
			this.IsCSharp = isCsharp;
		}
		#endregion

		#region Load event
		private void NewWFActivityForm_Load(object sender, EventArgs e)
		{
			this.listBoxActivityType.Items.Add(typeof(System.Activities.Activity).ToString());
			this.listBoxActivityType.Items.Add(typeof(System.Activities.CodeActivity).ToString());
			this.listBoxActivityType.Items.Add(typeof(System.Activities.NativeActivity).ToString());
			// select the 1st item
			this.listBoxActivityType.SelectedIndex = 0;
		}
		#endregion

		#region Properties
		internal bool IsCSharp { get; set; }
		internal string TemplateCode { get; set; }
		#endregion

		#region buttonOK_Click
		private void buttonOK_Click(object sender, EventArgs e)
		{
			// Validate controls anyway - should not be able to click OK button unless there is some text in the boxes...
			if (string.IsNullOrWhiteSpace(this.textBoxClassName.Text) ||
				(this.listBoxActivityType.SelectedItem == null))
			{
				;	// do nothing
			}
			else
			{
				// generate template
				this.TemplateCode = this.GenerateSourceCodeTemplate(this.IsCSharp, this.listBoxActivityType.SelectedItem as string, this.textBoxClassName.Text, this.textBoxReturnTypeName.Text.Trim());
				// exit dialog
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}
		#endregion

		#region textBoxClassName_TextChanged
		private void textBoxClassName_TextChanged(object sender, EventArgs e)
		{
			// enable/disable OK button
			if ((this.textBoxClassName.Text.Length > 0) && (this.textBoxClassName.Text.Trim().Length == 0))
			{
				this.textBoxClassName.Text = string.Empty;
			}
			this.buttonOK.Enabled = (this.textBoxClassName.Text.Length > 0);
		}
		#endregion

		#region GenerateSourceCodeTemplate
		private string GenerateSourceCodeTemplate(
			bool isCsharp,
			string activityTypeName,
			string className,
			string returnTypeName)
		{
			StringBuilder sb = new StringBuilder();
			bool hasReturnType = !string.IsNullOrEmpty(returnTypeName);

			if (!isCsharp)
			{
				#region VB.NET
				sb.Append("\tPublic NotInheritable Class  " + className + " Inherits " + activityTypeName);

				if (hasReturnType)
				{
					sb.AppendLine("(Of " + returnTypeName + ")");
				}
				else
				{
					sb.AppendLine();
				}

				sb.AppendLine("\t\t' Input argument.");
				sb.AppendLine("\t\t<RequiredArgument()>\t\t' This means the Argument must be supplied to this activity");
				sb.Append("\t\tProperty InArg() As InArgument(Of ");
				if (hasReturnType)
				{
					sb.Append(returnTypeName);
				}
				else
				{
					sb.Append("String");
				}
				sb.AppendLine(")");
				sb.AppendLine();

				string typeStr = typeof(System.Activities.Activity).ToString();
				if (activityTypeName == typeStr)
				{
					#region Activity<>
					if (!hasReturnType)
					{
						sb.AppendLine("\t\t' Output argument.");
						sb.Append("\t\tProperty OutArg() As OutArgument(Of String)");
						sb.AppendLine();
					}

					// Handle Activity type
					sb.AppendLine("\t\tProtected Sub New()");
					sb.AppendLine("\t\t\t' Define the implementation of this activity.");
					sb.AppendLine("\t\tEnd Sub");
					#endregion
				}
				else
				{
					#region CodeActivity<> or NativeActivity()
					typeStr = typeof(System.Activities.CodeActivity).ToString();
					bool codeActivity = (activityTypeName == typeStr);

					if (!hasReturnType)
					{
						sb.AppendLine("\t\t' Output argument.");
						sb.Append("\t\tProperty OutArg() As OutArgument(Of String)");
						sb.AppendLine();
					}

					// Handle Activity type
					sb.AppendLine(string.Format("\t\tProtected Overrides {0} Execute(context As {1}) {2}",
						(hasReturnType && codeActivity) ? "Function" : "Sub",
						codeActivity ? "CodeActivityContext" : "NativeActivityContext",
						(hasReturnType && codeActivity) ? "As " + returnTypeName : ""));
					sb.AppendLine("\t\t\t' Obtain the runtime value of the InArg input argument");
					if (hasReturnType)
					{
						sb.AppendLine(string.Format("\t\t\tDim obj As {0} = context.GetValue(Me.InArg)", returnTypeName));
						sb.AppendLine("\t\t\t' Return the value");
						if (codeActivity)
						{
							sb.AppendLine("\t\t\tExecute = obj");
						}
						else
						{
							sb.AppendLine("\t\t\tcontext.SetValue(Me.Result, obj)");
						}
					}
					else
					{
						sb.AppendLine("\t\t\tDim obj As String = context.GetValue(Me.InArg)");
						sb.AppendLine("\t\t\t' Set the OutArg value");
						sb.AppendLine("\t\t\tcontext.SetValue(Me.OutArg, obj)");
					}
					sb.AppendLine("\t\tEnd " + ((hasReturnType && codeActivity) ? "Function" : "Sub"));
					#endregion
				}

				// Class closure
				sb.AppendLine("\tEnd Class");
				#endregion
			}
			else
			{
				#region C#
				sb.Append("\tpublic sealed class " + className + " : " + activityTypeName);
				if (hasReturnType)
				{
					sb.AppendLine("<" + returnTypeName + ">");
				}
				else
				{
					sb.AppendLine();
				}
				sb.AppendLine("\t{");

				sb.AppendLine("\t\t// Input argument.");
				sb.AppendLine("\t\t[RequiredArgument]\t\t// This means the Argument must be supplied to this activity");
				sb.Append("\t\tpublic InArgument<");
				if (hasReturnType)
				{
					sb.Append(returnTypeName);
				}
				else
				{
					sb.Append("string");
				}
				sb.AppendLine("> InArg { get; set; }");
				sb.AppendLine();

				string typeStr = typeof(System.Activities.Activity).ToString();
				if (activityTypeName == typeStr)
				{
					#region Activity<>
					if (!hasReturnType)
					{
						sb.AppendLine("\t\t// Output argument.");
						sb.AppendLine("\t\tpublic OutArgument<string> OutArg { get; set; }");
						sb.AppendLine();
					}

					// Handle Activity type
					sb.AppendLine("\t\tpublic " + className + "()");
					sb.AppendLine("\t\t{");
					sb.AppendLine("\t\t\t// Define the implementation of this activity.");
					sb.AppendLine(string.Format("\t\t\tthis.Implementation = () => new Assign<{0}>", hasReturnType ? returnTypeName : "string"));
					sb.AppendLine("\t\t\t\t{");
					if (hasReturnType)
					{
						sb.AppendLine(string.Format("\t\t\t\t\tValue = new InArgument<{0}>(context => InArg.Get(context)),", returnTypeName));
						sb.AppendLine(string.Format("\t\t\t\t\tTo = new LambdaReference<{0}>(context => Result.Get(context)),", returnTypeName));
					}
					else
					{
						sb.AppendLine("\t\t\t\t\tValue = new LambdaValue<string>(context => InArg.Get(context) + \" says hello world\"),");
						sb.AppendLine("\t\t\t\t\tTo = new LambdaReference<string>(context => OutArg.Get(context)),");
					}
					sb.AppendLine("\t\t\t\t};");
					sb.AppendLine("\t\t}");
					#endregion
				}
				else
				{
					#region CodeActivity<> or NativeActivity()
					typeStr = typeof(System.Activities.CodeActivity).ToString();
					bool codeActivity = (activityTypeName == typeStr);

					if (!hasReturnType)
					{
						sb.AppendLine("\t\t// Output argument.");
						sb.AppendLine("\t\tpublic OutArgument<string> OutArg { get; set; }");
						sb.AppendLine();
					}

					// Handle Activity type
					sb.AppendLine(string.Format("\t\tprotected override {0} Execute({1} context)", (hasReturnType && codeActivity) ? returnTypeName : "void", codeActivity ? "CodeActivityContext" : "NativeActivityContext"));
					sb.AppendLine("\t\t{");
					sb.AppendLine("\t\t\t// Obtain the runtime value of the InArg input argument");
					if (hasReturnType)
					{
						sb.AppendLine(string.Format("\t\t\t{0} obj = context.GetValue(this.InArg);", returnTypeName));
						sb.AppendLine("\t\t\t// Return the value");
						if (codeActivity)
						{
							sb.AppendLine("\t\t\treturn obj;");
						}
						else
						{
							sb.AppendLine("\t\t\tResult.Set(context, obj);");
						}
					}
					else
					{
						sb.AppendLine("\t\t\tstring obj = context.GetValue(this.InArg);");
						sb.AppendLine("\t\t\t// Set the OutArg value");
						sb.AppendLine("\t\t\tcontext.SetValue(this.OutArg, obj);");
					}
					sb.AppendLine("\t\t}");
					#endregion
				}

				// Class closure
				sb.AppendLine("\t}");
				#endregion
			}

			return sb.ToString();
		}
		#endregion
	}
}
