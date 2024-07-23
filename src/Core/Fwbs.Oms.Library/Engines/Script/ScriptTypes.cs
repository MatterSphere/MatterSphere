#region References
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
#endregion

namespace FWBS.OMS.Script
{





    /// <summary>
    /// An abstract script type class describes delegate and assembly referncing information.
    /// </summary>
    public abstract class ScriptType : MarshalByRefObject, IScriptType
	{
		#region Fields

		/// <summary>
		/// Reference to the parent script gen.
		/// </summary>
		private ScriptGen _script = null;
		/// <summary>
		/// Reference to the Private Shared Varables
		/// </summary>
		private System.Collections.Hashtable _private = new System.Collections.Hashtable();

		private ScriptType _proxy;

		#endregion

		#region Constructors

		public ScriptType()
		{
			Initialise();
		}

		#endregion

		#region Abstraction Layer

		/// <summary>
		/// Gets a string array of assembly references to include.
		/// </summary>
		internal protected virtual string[] AssemblyReferences
		{
			get
			{
				return new string[0];
			}
		}

		/// <summary>
		/// The Current Object used by the Code Lookup Editor
		/// </summary>
		public abstract object CurrentObj{get;}

		public abstract IContext Context { get; }
	
		/// <summary>
		/// Gets all the code namespace imports.
		/// </summary>
		internal protected virtual CodeNamespaceImport[] NamespaceImports
		{
			get
			{
				CodeNamespaceImport[] ns = new CodeNamespaceImport[]
				{
					new CodeNamespaceImport("System"),
					new CodeNamespaceImport("System.Data"),
					new CodeNamespaceImport("System.Windows.Forms"),
					new CodeNamespaceImport("FWBS.Common.UI"),
					new CodeNamespaceImport("FWBS.Common"),
					new CodeNamespaceImport("System.Xml"),
				};
				return ns;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Checks to see if a method by a specified name exists.
		/// </summary>
		/// <param name="methodName">The method name to check form.</param>
		/// <returns>True, if the method exists.</returns>
		protected MethodInfo GetMethod(string methodName)
		{
			MethodInfo meth = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			return meth;
		}


		private static Type DeReferenceTypeName(Type t) 
		{
			string name = t.FullName;
			if (name.EndsWith("&"))
			{
				name = String.Format("{0}, {1}", name.Substring(0, t.FullName.Length - 1), t.Assembly.FullName);
				Type t2 = Session.CurrentSession.TypeManager.Load(name);
				if (t2 == null)
					return t;
				else
					return t2;
			}
			else
				return t;
		}

		protected void HandleException(Exception ex)
		{
			Session.CurrentSession.OnShowException(this, ex);
		}

		protected internal void ExecuteWorkflow(string name)
		{
			try
			{
				Type t = FWBS.OMS.Session.CurrentSession.TypeManager.Load("FWBS.OMS.Workflow.WFRuntime,FWBS.OMS.Workflow");
				MethodInfo mi = t.GetMethod("Execute", new Type[] { typeof(string), typeof(TimeSpan), typeof(IDictionary<string, object>), typeof(FWBS.OMS.IContext) });
				if (mi == null)
					throw new InvalidOperationException("Cannot find the Execute method on WFRuntime.");

				// Give the workflow 1 day execution time frame
				mi.Invoke(null, new object[] { name, TimeSpan.FromMilliseconds(Int32.MaxValue), null, Context });
			}
			catch (Exception ex)
			{
				// Pass inner exceptioni if it exists since that has the real cause ...
				throw new OMSException2("ERRWFRUN", "Error executing Workflow '%1%'", "", ex.InnerException != null ? ex.InnerException : ex, false, name);
			}
		}

		/// <summary>
		/// Gets all the methods that are needed to be assigned.
		/// </summary>
		public virtual CodeMemberMethod [] Methods
		{
			get
			{
				MemberInfo[] members = GetType().FindMembers(MemberTypes.Method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, new MemberFilter(Filter), null);
				CodeMemberMethod[] methods = new CodeMemberMethod[members.Length];
				int ctr = 0;
				foreach (MethodInfo meth in members)
				{
					CodeMemberMethod m = new CodeMemberMethod();
					   
					m.Name = meth.Name;
					m.ReturnType = new CodeTypeReference(meth.ReturnType);
					var versioncond = meth.Attribute<VersionConditionalAttribute>();
					if (versioncond != null)
						m.Comments.Add(new CodeCommentStatement(String.Format(ScriptCompiler.VersionFormat, versioncond.Version.Major, versioncond.Version.Minor, versioncond.Version.Revision, versioncond.Version.Build)));

					foreach (ParameterInfo param in meth.GetParameters())
					{                       
						CodeParameterDeclarationExpression p = new CodeParameterDeclarationExpression();
						p.Name = param.Name;
						//I am now setting the type by name and getting rid of the & for byref parameters.
						p.Type = new CodeTypeReference(DeReferenceTypeName(param.ParameterType));

						if (param.IsOut)
							p.Direction = FieldDirection.Out;
						else if (param.IsRetval)
							p.Direction = FieldDirection.Ref;
						else if (param.IsIn)
							p.Direction = FieldDirection.In;
						else
						{
							if (param.ParameterType.IsByRef)
								p.Direction = FieldDirection.Ref;
							else
								p.Direction = FieldDirection.In;
						}

					   
						m.Parameters.Add(p);
					}
					
					MemberAttributes att = MemberAttributes.Override;
					if (meth.IsAssembly)
						att |= MemberAttributes.Assembly;
					if (meth.IsStatic)
						att |= MemberAttributes.Static;
					if (meth.IsFamily)
						att |= MemberAttributes.Family;
					if (meth.IsPrivate)
						att |= MemberAttributes.Private;
					if (meth.IsPublic)
						att |= MemberAttributes.Public;
					if (meth.IsFamilyAndAssembly)
						att |= MemberAttributes.FamilyAndAssembly;
					if (meth.IsFamilyOrAssembly)
						att |= MemberAttributes.FamilyOrAssembly;
					if (meth.IsFinal)
						att |= MemberAttributes.Final;


					m.Attributes = att;
					methods[ctr] = m;
					ctr++;
				}
				return methods;
			}
		}

		/// <summary>
		/// Sets the internal script generator property.
		/// </summary>
		/// <param name="script">Script Generator object.</param>
        /// <param name="proxy"></param>
		internal void SetScriptGeneratorObject(ScriptGen script, ScriptType proxy)
		{
			_script = script;
			_proxy = proxy;
		}

		/// <summary>
		/// Set A Global Shared Variable
		/// </summary>
		/// <param name="Name">String name of the Variable</param>
		/// <param name="val">Ojbect to Store</param>
		protected void SetVar(string Name, object val)
		{
			if (_private.Contains(Name) == false)
				_private.Add(Name,val);
			else
				_private[Name] = val;
		}

		/// <summary>
		/// Returns A Global Shared Variable
		/// </summary>
		/// <param name="Name">String name of the Variable</param>
		/// <param name="DefValue">If the Variable cannot be Found return this</param>
		/// <returns>Returns the Shared Variable</returns>
		protected object GetVar(string Name, object DefValue)
		{
			try
			{
				if (_private.Contains(Name))
					return _private[Name];
				else
					return DefValue;
			}
			catch
			{
				return DefValue;
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the reference to the current session.
		/// </summary>
		protected Session CurrentSession
		{
			get
			{
				return FWBS.OMS.Session.CurrentSession;
			}
		}

		protected ScriptType CurrentScript
		{
			get
			{
				return _proxy ?? this;
			}
		}

		/// <summary>
		/// Gets the parent script generator.
		/// </summary>
		protected ScriptGen ScriptGenerator
		{
			get
			{
				return _script;
			}
		}

		public string[] Assemblies
		{
			get
			{
				return this.AssemblyReferences;
			}
		}

		protected internal virtual string Namespace
		{
			get
			{
				return "OMS.Scriptlets";
			}
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// This method is called to evaluate each member passed so that only overridable
		/// methods are exposed in the list.
		/// </summary>
		/// <param name="m">Member info object.</param>
		/// <param name="filterCriteria">Filter criteria.</param>
		/// <returns>Returns true if the filter succeeds</returns>
		private static bool Filter (MemberInfo m, object filterCriteria)
		{
			object[] attr =  m.GetCustomAttributes(typeof(ScriptMethodOverridableAttribute), false);
			if (attr.Length > 0)
			{
				MethodInfo meth = (MethodInfo)m;
				if (meth.IsVirtual || meth.IsAbstract)
					return true;
				else
					return false;
			}
			else
				return false;
		}


		#endregion

		#region IDisposable Implementation

		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by this object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_script = null;
				_proxy = null;
			}
		}
		

		#endregion
	
		#region Script Virtual Methods

		[ScriptMethodOverridable()]
		protected virtual void Initialise()
		{
		}

		#endregion

		
	}







	
				 

}
