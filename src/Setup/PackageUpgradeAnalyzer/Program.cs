using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace PackageUpgradeAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine();
            Console.WriteLine(((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute))).Description);
            Console.WriteLine();

            if (args.Length < 1 || args.Length > 2 || args[0].Equals("/?") || (args.Length == 2 && !args[0].Equals("/all", StringComparison.InvariantCultureIgnoreCase)))
            {
                Console.WriteLine("Usage:>\t{1} [/all] path{0}{0}\t/all\tIndicates that all updated objects should be tracked.{0}\t\tWhen this parameter is omitted, only potential conflicts will be detected.{0}{0}\tpath\tSpecifies the path to a folder where 3E MatterSphere packages are located.",
                    Environment.NewLine, Path.GetFileName(assembly.Location));
            }
            else
            {
                try
                {
                    bool trackAllChanges = args.Length > 1;
                    string connectionString = ConfigurationManager.ConnectionStrings["MatterSphere"]?.ConnectionString;
                    string reportFile = Path.GetFullPath(ConfigurationManager.AppSettings["ReportFile"]);

                    string packageDirectory = Path.GetFullPath(args[args.Length - 1].Trim('"'));
                    if (!Directory.Exists(packageDirectory))
                        throw new DirectoryNotFoundException();

                    int totalConflictCount = AnalyzePackages(connectionString, reportFile, packageDirectory, trackAllChanges);
                    Console.WriteLine("{0}{1} conflicts found.", Environment.NewLine, totalConflictCount);
                    if (totalConflictCount > 0 || trackAllChanges)
                    {
                        Console.WriteLine("Please see the report file \"{0}\".", reportFile);
                    }
                }
                catch (AggregateException ae)
                {
                    foreach (Exception ex in ae.Flatten().InnerExceptions)
                        Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine();
        }

        private static int AnalyzePackages(string connectionString, string reportFile, string packageDirectory, bool trackAllChanges)
        {
            int totalConflictCount = 0;
            using (StreamWriter reportWriter = new StreamWriter(reportFile))
            {
                reportWriter.WriteLine("==========================================================================================================================");
                reportWriter.WriteLine("Content        | Database Object  Version     Updated        UserId | Package Object   Version     Updated        UserId |");
                reportWriter.WriteLine("==========================================================================================================================");

                using (DataProvider dataProvider = new DataProvider(connectionString))
                {
                    var packageManifests = Directory.EnumerateFiles(packageDirectory, "*.Manifest.xml", SearchOption.AllDirectories);
                    foreach (string packageManifest in packageManifests)
                    {
                        string name = Path.GetFileName(packageManifest);
                        name = name.Substring(0, name.IndexOf('.')).ToUpper();
                        Console.Write("{0, -15}", name);

                        int conflictCount = AnalyzePackage(name, Path.GetDirectoryName(packageManifest), dataProvider, reportWriter, trackAllChanges);
                        if (conflictCount > 0)
                        {
                            Console.WriteLine(" - {0} conflicts", conflictCount);
                            totalConflictCount += conflictCount;
                        }
                        else
                        {
                            Console.WriteLine(" - OK");
                        }
                        reportWriter.WriteLine("--------------------------------------------------------------------------------------------------------------------------");
                    }
                }
            }
            return totalConflictCount;
        }

        private static int AnalyzePackage(string packageName, string packageRootDir, DataProvider dataProvider, StreamWriter reportWriter, bool trackAllChanges)
        {
            int conflictCount = 0;
            reportWriter.WriteLine("{0}{1}", Environment.NewLine, packageName);

            foreach (string omsType in dataProvider.OmsTypes)
            {
                string typeDir = Path.Combine(packageRootDir, omsType);
                if (Directory.Exists(typeDir))
                {
                    string versionColumnName = dataProvider.GetVersionColumnName(omsType);

                    foreach (string codeDir in Directory.EnumerateDirectories(typeDir))
                    {
                        string code = Path.GetFileName(codeDir);

                        OmsObjectInfo dbObjInfo = dataProvider.GetOmsObjectInfo(omsType, code);
                        if (dbObjInfo == null) continue;

                        XmlHelper xmlHelper = new XmlHelper(Path.Combine(codeDir, "manifest.xml"), versionColumnName);
                        OmsObjectInfo xmlObjInfo = xmlHelper.GetOmsObjectInfo(omsType, code);
                        if (xmlObjInfo == null) continue;

                        var status = dbObjInfo.CanBeUpgradedBy(xmlObjInfo);
                        if (status != OmsObjectInfo.UpgradeStatus.Yes)
                        {
                            reportWriter.WriteLine("{1,-14} {0} {2} {0} {3} {0}", (char)status, omsType, dbObjInfo, xmlObjInfo);
                            conflictCount++;
                        }
                        else if (trackAllChanges && dbObjInfo.Version != xmlObjInfo.Version)
                        {
                            reportWriter.WriteLine("{1,-14} {0} {2} {0} {3} {0}", (char)status, omsType, dbObjInfo, xmlObjInfo);
                        }
                    }
                }
            }

            return conflictCount;
        }
    }
}
