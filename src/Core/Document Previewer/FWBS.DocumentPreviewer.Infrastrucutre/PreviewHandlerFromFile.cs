using System.IO;

namespace Fwbs.Documents.Preview.Handlers
{

    public class PreviewHandlerFromFile : PreviewHandler, IInitializeWithFile
	{
		protected FileInfo file { get; private set; }
		protected bool HasChanged { get; private set; }

		#region IInitializeWithFile Members

		public virtual void Initialize(string pszFilePath, uint grfMode)
		{
            var newFile = new FileInfo(pszFilePath);
            HasChanged = (file == null) || (file.FullName != newFile.FullName) || (file.LastWriteTimeUtc != newFile.LastWriteTimeUtc);
			file = newFile;
		}

        #endregion

        protected string CreateTempPath(string extension)
        {
            var path = Path.Combine(file.Directory.FullName, "Previews");
            var dir = new DirectoryInfo(path);

            if (!dir.Exists)
                dir.Create();

            return Path.Combine(dir.FullName, Path.GetFileNameWithoutExtension(file.Name) + "-preview" + extension);
        }

        public override void Unload()
		{
			base.Unload();
		}

	}
}
