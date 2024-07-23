#region References
using System;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom;
#endregion

namespace FWBS.OMS.Workflow.Admin
{
	public class ExpressionEditorService : IExpressionEditorService, IDisposable
	{
		#region Fields
		// cache the project resolver and share among all editors
		private static DotNetProjectResolver dotNetProjectResolver;
		private static int refCount = 0;
		private bool _disposed;
		private readonly List<ExpressionEditorInstance> editors = new List<ExpressionEditorInstance>();
		#endregion

		private ExpressionEditorService()
		{
			Interlocked.Increment(ref refCount);
		}

		public static ExpressionEditorService CreateExpressionEditorService()
		{
			Trace.TraceInformation($"Creating {nameof(ExpressionEditorService)}...");
			var expressionEditorService = new ExpressionEditorService();
			if (dotNetProjectResolver == null)
			{
				dotNetProjectResolver = CreateProjectResolver();
				AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
			}
			return expressionEditorService;
		}

		#region IExpressionEditorService interace
		public void CloseExpressionEditors()
		{
			// get rid of wpf editor controls
			foreach (ExpressionEditorInstance editor in this.editors)
			{
				editor.Close();
			}
			this.editors.Clear();
		}

		public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text)
		{
			ExpressionEditorInstance instance = new ExpressionEditorInstance(assemblies, importedNamespaces, variables, text, null, System.Windows.Size.Empty, dotNetProjectResolver);
			this.editors.Add(instance);
			return instance;
		}

		public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, System.Windows.Size initialSize)
		{
			ExpressionEditorInstance instance = new ExpressionEditorInstance(assemblies, importedNamespaces, variables, text, null, initialSize, dotNetProjectResolver);
			this.editors.Add(instance);
			return instance;
		}

		public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType)
		{
			ExpressionEditorInstance instance = new ExpressionEditorInstance(assemblies, importedNamespaces, variables, text, expressionType, System.Windows.Size.Empty, dotNetProjectResolver);
			this.editors.Add(instance);
			return instance;
		}

		public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType, System.Windows.Size initialSize)
		{
			ExpressionEditorInstance instance = new ExpressionEditorInstance(assemblies, importedNamespaces, variables, text, expressionType, initialSize, dotNetProjectResolver);
			this.editors.Add(instance);
			return instance;
		}

		public void UpdateContext(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces)
		{
			// Supposed to update assembly references ...
		}
		#endregion

		private static DotNetProjectResolver CreateProjectResolver()
		{
			FWBS.OMS.Design.CodeBuilder.CacheSplashForm splashForm = new FWBS.OMS.Design.CodeBuilder.CacheSplashForm();

			var projectResolver = new DotNetProjectResolver
			{
				CachePath = Path.Combine(Global.GetDBAppDataPath(), "Intellisense Cache", FWBS.OMS.Session.CurrentSession.AssemblyVersion.ToString())
			};

			Trace.TraceInformation($"Resolving assemblies...");
			Trace.Indent();
			try
			{
				splashForm.Show();
				splashForm.Refresh();

				projectResolver.AddExternalReferenceForMSCorLib();

				// Use all assemblies in the AppDomain - the assembly context that gets passed to the editor is of no use as it gives you the list below!

				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					Trace.TraceInformation($"Resolving assembly: {assembly.FullName}...");
					// can't do dynamic assemblies since there is no corresponding files
					if (assembly.IsDynamic)
					{
						Trace.TraceInformation($"Skipping Dynamic assembly: {assembly.FullName}.");
						continue;
					}

					// exclude Actipro assemblies
					if (assembly.FullName.IndexOf("ActiPro", StringComparison.InvariantCultureIgnoreCase) > -1)
					{
						Trace.TraceInformation($"Skipping ActiproSoftware assembly: {assembly.FullName}.");
						continue;
					}

					try
					{
						Trace.TraceInformation($"Adding external references for: {assembly.FullName}...");
						projectResolver.AddExternalReference(assembly);
						Trace.TraceInformation($"Done.");
					}
					catch (Exception ex)
					{
						Trace.TraceError($"Exception occurred while adding external references: {assembly.FullName}...");
						Trace.TraceError(ex.ToString());
						Trace.TraceError(ex.InnerException?.ToString());
						try
						{
							Trace.TraceInformation($"Adding a system assembly as an external reference: {assembly.Location}...");
							projectResolver.AddExternalReferenceForSystemAssembly(assembly.Location);
							Trace.TraceInformation($"Done.");
						}
						catch (Exception ex2)
						{
							Trace.TraceError($"Exception occurred while adding a system assembly: {assembly.Location}...");
							Trace.TraceError(ex2.ToString());
							Trace.TraceError(ex2.InnerException?.ToString());
							//FWBS.OMS.UI.Windows.ErrorBox.Show(ex2);
						}
					}
				}
			}
			finally
			{
				splashForm.Dispose();
			}

			Trace.TraceInformation($"Resolving assemblies complete.");
			Trace.Unindent();

			return projectResolver;
		}

		private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			if (dotNetProjectResolver == null)
			{
				return;
			}

			// if it is dynamic then ignore
			if (args.LoadedAssembly.IsDynamic)
			{
				return;
			}
			// it is not dynamic, try to add as a reference
			try
			{
				dotNetProjectResolver.AddExternalReference(args.LoadedAssembly);
			}
			catch
			{
				try
				{
					dotNetProjectResolver.AddExternalReferenceForSystemAssembly(args.LoadedAssembly.Location);
				}
				catch (Exception ex)
				{
					FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
				}
			}
		}

		#region IDisposable
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			Trace.TraceInformation($"Disposing {nameof(ExpressionEditorService)}...");
			if (_disposed)
			{
				return;
			}
		
			if (disposing)
			{
				Interlocked.Decrement(ref refCount);
				this.CloseExpressionEditors();
		
				if (Interlocked.CompareExchange(ref refCount, 0, 0) == 0)
				{
					AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
		
					dotNetProjectResolver?.Dispose();
					dotNetProjectResolver?.PruneCache();
					dotNetProjectResolver = null;
		
					_disposed = true;
				}
			}
		}
		#endregion
	}
}
