using System;
using System.Collections.Generic;
using Fwbs.Framework.ComponentModel.Composition;

namespace Fwbs.Documents.Preview
{
    [Export(typeof(IPreviewer))]
	[System.ComponentModel.Composition.PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
	internal sealed class Previewer : IPreviewer
	{
		private PreviewerControl control;
		private IContainer container;

		[ImportingConstructor]
		public Previewer(IContainer container)
		{
			if (container == null)
				throw new ArgumentNullException("container");

			this.container = container; 
			
			SetupPreviewer();
		}

		private void SetupPreviewer()
		{
			if (control != null)
			{
				control.Disposed -= new EventHandler(control_Disposed);
				control.Dispose();
			}

			control = new PreviewerControl();
			control.Disposed += new EventHandler(control_Disposed);
			control.Container = container;
		}

		private void control_Disposed(object sender, EventArgs e)
		{
			control.Disposed -= new EventHandler(control_Disposed);
		}

		public void PreviewFile(System.IO.FileInfo file)
		{
			ValidateUIElement();

			control.PreviewFile(file);
		}

		public void ShowMessage(string message)
		{
			ValidateUIElement();

			control.ShowMessage(message);
		}

		public Dictionary<string, string> CultureProperties
		{
			get
			{
				ValidateUIElement();

				return control.CultureProperties;
			}
			set
			{
				ValidateUIElement();

				control.CultureProperties = value;
			}
		}

		public void Load()
		{
			ValidateUIElement();

			control.Load();
		}

		public void Unload()
		{
			ValidateUIElement();

			control.Unload();
		}

		public object UIElement
		{
			get 
			{
				ValidateUIElement();

				return control; 
			}
		}

		private void ValidateUIElement()
		{
			if (control == null || control.IsDisposed)
				throw new ObjectDisposedException("UI Element on preview has been disposed and needs reconstructing.");

		}

		public void Dispose()
		{
			if (control != null)
			{
				control.Dispose();
				control = null;
			}
			PreviewHandlerFactory.ShellExtensions.Clear();
		}
	}
}
