using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FWBS.Common;

namespace FWBS.OMS
{
    public sealed class DistributedAssemblyManager
	{
		private Dictionary<string, DistributedAssemblyListItem> distributedassemblies = null;
		private Dictionary<string, DistributedAssemblyListItem> distributedmodules = null;

		public DistributedAssemblyManager()
		{
			// Set the Location of the Distributed Assmeblies
			DistributedAssembliesDirectory = new DirectoryInfo(Global.GetDBAppDataPath() + @"\Distributed\" + Session.CurrentSession.AssemblyVersion);
			if (DistributedAssembliesDirectory.Exists == false)
				DistributedAssembliesDirectory.Create();

			// Set the Location of the Distributed Modules
			DistributedAssembliesModuleDirectory = new DirectoryInfo(Global.GetDBAppDataPath() + @"\Distributed\Modules\" + Session.CurrentSession.AssemblyVersion);
			if (DistributedAssembliesModuleDirectory.Exists == false)
				DistributedAssembliesModuleDirectory.Create();

		}

		public DirectoryInfo DistributedAssembliesDirectory
		{
			get;
			private set;
		}

		public DirectoryInfo DistributedAssembliesModuleDirectory
		{
			get;
			private set;
		}

		public bool IsDistributedAssembly(Assembly assembly)
		{
			BuildCache();
			string location = assembly.Location;
			if (string.IsNullOrEmpty(location))
				location = new Uri(assembly.CodeBase).LocalPath;
			FileInfo assemblyFile = new FileInfo(location);
			FileInfo assembly2 = GetExtractedAssemblyFileInfo(assembly.GetName().Name, DistributedAssembliesDirectory.FullName);
			FileInfo assembly3 = GetExtractedModuleFileInfo(assembly.GetName().Name, DistributedAssembliesModuleDirectory.FullName);

			if (assemblyFile.FullName.Equals(assembly2.FullName, StringComparison.OrdinalIgnoreCase) || assemblyFile.FullName.Equals(assembly3.FullName, StringComparison.OrdinalIgnoreCase))
				return IsDistributedAssembly(assemblyFile.Name, distributedassemblies) || IsDistributedAssembly(assemblyFile.Name, distributedmodules);

			return false;
		}

		private bool IsDistributedAssembly(string assemblyfilename, Dictionary<string, DistributedAssemblyListItem> cache)
		{
			if (cache == null) return false;
			if (cache.ContainsKey(assemblyfilename.ToUpperInvariant()))
				return true;
			return false;
		}

		internal DistributedCheckResult CheckForUpdatedAssembly(string assemblyfilename)
		{
			return CheckForUpdatedAssembly(assemblyfilename, distributedassemblies, DistributedAssembliesDirectory.FullName);
		}

		private DistributedCheckResult CheckForUpdatedAssembly(string assemblyfilename, Dictionary<string, DistributedAssemblyListItem> cache, string path)
		{
			BuildCache();
			if (cache == null) return DistributedCheckResult.Error;
			var assemblyfile = GetExtractedAssemblyFileInfo(assemblyfilename, path);

			if (cache.ContainsKey(assemblyfilename.ToUpperInvariant()) == false)
				return DistributedCheckResult.NotFound;

			var filedate = cache[assemblyfilename.ToUpperInvariant()];

			if (assemblyfile.Exists == false)
				return DistributedCheckResult.NotDownloaded;

			// ToString is becuase sql store more milliseconds then the LastWriteTimeUtc so a direct
			// comparison always returns false
			if (assemblyfile.LastWriteTimeUtc.ToString() != filedate.Modified.ToString())
				return DistributedCheckResult.OutOfDate;

			return DistributedCheckResult.Ok;
		}

		private const string DLL = ".dll";

		/// <summary>
		/// Appends the specified file extension if not already present
		/// </summary>
		/// <param name="filename">Filename</param>
		/// <param name="extension">Extension to append including the dot</param>
		/// <returns></returns>
		private static string AppendFileExtension(string filename, string extension)
		{
			if (!filename.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase))
				filename = filename + extension;

			return filename;
		}

		internal static FileInfo GetExtractedAssemblyFileInfo(string assemblyfilename, string path)
		{
			string filename = AppendFileExtension(assemblyfilename, DLL);

			// NOTE: If you change the path format ensure that you also change it in source file WFRuntime.cs
			FileInfo file = new FileInfo(Path.Combine(path, filename, filename));
			return file;
		}

		internal static FileInfo GetExtractedModuleFileInfo(string modulefilename, string path)
		{
			string filename = AppendFileExtension(modulefilename, DLL);

			FileInfo file = new FileInfo(Path.Combine(path, filename));
			return file;
		}

		internal void ExtractModules()
		{
			var dis = new DistributedAssemblies();
			BuildCache();
			foreach (var item in distributedmodules.Values)
			{
				var result = CheckForUpdatedAssembly(item.AssemblyName, distributedmodules, DistributedAssembliesModuleDirectory.FullName);
				switch (result)
				{
					case DistributedCheckResult.OutOfDate:
					case DistributedCheckResult.NotDownloaded:
						dis.FetchCurrent(item.AssemblyName);
						dis.ExtractFromDatabase();
						break;
				}
			}
		}

		private void BuildCache()
		{
			if (distributedassemblies == null)
			{
				DataTable dt = null;
				List<IDataParameter> parms = new List<IDataParameter>();
				parms.Add(Session.CurrentSession.Connection.CreateParameter("@Version", Session.CurrentSession.AssemblyVersion.ToString()));
				try
				{
					dt = Session.CurrentSession.Connection.ExecuteSQLTable(String.Format("select Assembly, Modified, OMSVersion, Version, PackageType from dbAssembly where (OMSVersion = @Version OR OMSVersion = '')", Session.CurrentSession.AssemblyVersion.ToString()), "Modified", parms.ToArray());
				}
				catch
				{
					dt = Session.CurrentSession.Connection.ExecuteSQLTable(String.Format("select Assembly, Modified, OMSVersion, Version, '' as PackageType from dbAssembly where (OMSVersion = @Version OR OMSVersion = '')", Session.CurrentSession.AssemblyVersion.ToString()), "Modified", parms.ToArray());
				}
				distributedassemblies = new Dictionary<string, DistributedAssemblyListItem>();
				distributedmodules = new Dictionary<string, DistributedAssemblyListItem>();
				foreach (DataRow item in dt.Rows)
				{
					DistributedPackageType packagetype = (DistributedPackageType)ConvertDef.ToEnum(item["PackageType"], DistributedPackageType.Assembly);
					switch (packagetype)
					{
						case DistributedPackageType.Modules:
							BuildCacheItem(item, distributedmodules);
							break;
						case DistributedPackageType.Assembly:
						case DistributedPackageType.Package:
							BuildCacheItem(item, distributedassemblies);
							break;
						default:
							break;
					}
				}
			}
		}

		private void BuildCacheItem(DataRow item, Dictionary<string, DistributedAssemblyListItem> cache)
		{
			string key = Convert.ToString(item["Assembly"]).ToUpper();
			if (cache.ContainsKey(key))
			{
				var entry = cache[key];
				if (String.IsNullOrEmpty(entry.OMSVersion) && !String.IsNullOrEmpty(Convert.ToString(item["OMSVersion"])))
					cache[key] = new DistributedAssemblyListItem(Convert.ToString(item["Assembly"]), Convert.ToDateTime(item["Modified"]), Convert.ToString(item["Version"]), Convert.ToString(item["OMSVersion"]));
			}
			else
				cache.Add(Convert.ToString(item["Assembly"]).ToUpper(), new DistributedAssemblyListItem(Convert.ToString(item["Assembly"]), Convert.ToDateTime(item["Modified"]), Convert.ToString(item["Version"]), Convert.ToString(item["OMSVersion"])));
		}

		internal void ClearCache()
		{
			distributedassemblies = null;
			distributedmodules = null;
			_warningalreadyshown = false;
		}

		internal void UninstallCheck()
		{
			if (DistributedAssembliesDirectory == null)
                throw new ArgumentException(Session.CurrentSession.Resources.GetMessage("CRSESSDADISNL", "CurrentSession.DistributedAssembliesDirectory is null", "").Text);

			BuildCache();
			foreach (var item in DistributedAssembliesDirectory.GetFiles("*.dll", SearchOption.AllDirectories))
			{
				if (!distributedassemblies.ContainsKey(item.Name.ToUpperInvariant()))
				{
					item.Directory.Delete(true);
				}
			}
			foreach (var item in DistributedAssembliesModuleDirectory.GetFiles("*.dll", SearchOption.AllDirectories))
			{
				if (!distributedmodules.ContainsKey(item.Name.ToUpperInvariant()))
				{
					item.Directory.Delete(true);
				}
			}
		}

		internal IEnumerable<DistributedAssemblyListItem> ListActiveAssemblies()
		{
			BuildCache();
			return distributedassemblies.Values;
		}

		internal List<string> CheckLoadedAssemblies()
		{
				List<string> warnings = new List<string>();
				var loaded = AppDomain.CurrentDomain.GetAssemblies();
				foreach (var dist in distributedassemblies.Values)
				{
					foreach (var loadassembly in loaded)
					{
						if (loadassembly.IsDynamic)
							continue;
						
						FileInfo loadedfile = null;
						Uri loadeduri = null;

						if (!string.IsNullOrEmpty(loadassembly.Location))
							loadedfile = new FileInfo(loadassembly.Location);

						if (!string.IsNullOrEmpty(loadassembly.CodeBase))
							loadeduri = new Uri(loadassembly.CodeBase);

						if (loadeduri != null && loadeduri.IsFile)
						{
							loadedfile = new FileInfo(loadeduri.LocalPath);
						}

						if (loadedfile == null)
							continue;

						if (loadedfile.Name.Equals(dist.AssemblyName, StringComparison.OrdinalIgnoreCase))
						{
							if (loadedfile.LastWriteTimeUtc.ToString("ddMMyyhhmmss") != dist.Modified.ToString("ddMMyyhhmmss"))
							{
								warnings.Add(string.Format("'{0}' is already loaded and the database contains a different version.", dist.AssemblyName));
							}
						}
					}
				}
				return warnings;
		}

		private bool _warningalreadyshown;
		internal void ShowUpdateWarning(IEnumerable<string> warnings)
		{
			if (_warningalreadyshown == false)
			{
				var res = Session.CurrentSession.Resources.GetMessage("MSGUPDATESYS", "Your system has recently been updated. You will need to restart to complete this update.", "");

				var sb = new StringBuilder(res.Text);

				if (warnings != null && warnings.Count() > 0)
				{
					sb.AppendLine();
					sb.AppendLine();
					foreach(var warn in warnings)
					{
						sb.AppendLine(warn);
					}
				}

			   
				Session.CurrentSession.OnWarning(Session.CurrentSession, new MessageEventArgs(sb.ToString()));
				
				_warningalreadyshown = true;
			}
		}

	}
}
