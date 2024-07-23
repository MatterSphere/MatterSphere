using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FWBS.OMS.Script
{
    internal static class ScriptUtils
    {
        public static string ToBase64(string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        public static string FromBase64(string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        public static string GetDirectory(IScriptDefinition definition)
        {
            var dn = Path.Combine(Global.GetCachePath().ToString(), "Scriptlets");
            dn = Path.Combine(dn, definition.Name);
            return dn;
        }

        public static string GetOutputName(ScriptGen gen)
        {
            var factory = new ScriptFactory();

            var def = factory.CreateDefinition(gen);

            return GetOutputName(def);
        }

        public static string GetOutputName(IScriptDefinition definition)
        {
            var dn = GetDirectory(definition);

            var dir = Directory.CreateDirectory(dn);

            var file = dir.EnumerateFiles(String.Format("{0}.{1}.*.dll", definition.Version, definition.Name)).OrderByDescending(f => f.CreationTimeUtc).FirstOrDefault();

            if (file == null)
            {
                return CreateOutputName(definition);
            }
            else
            {
                return file.FullName;
            }
        }

        public static string CreateOutputName(IScriptDefinition definition)
        {
            var dn = GetDirectory(definition);

            var dir = Directory.CreateDirectory(dn);

            return Path.Combine(dn, new ScriptFileName(definition).ToString());
            
        }

        public static void SaveToFile(FileInfo file, string base64data, DateTime? modified)
        {
            file.Directory.Create();

            using (var strm = System.IO.File.Create(file.FullName))
            {
                byte[] buffer = Convert.FromBase64String(base64data);
                strm.Write(buffer, 0, buffer.Length);
            }

            if (modified.HasValue)
            {
                try { file.LastWriteTime = modified.Value; }
                catch (FormatException) { }
                catch (InvalidCastException) { }
            }
        }


        internal static IEnumerable<Tuple<string, string>> SplitProviderOptions(string options)
        {
            var vals = options.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            var list = new List<Tuple<string, string>>();
            foreach (var val in vals)
            {
                var vals2 = val.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (vals2.Length >= 2)
                {
                    list.Add(new Tuple<string, string>(vals2[0], vals2[1]));
                }
            }

            return list.AsReadOnly();
        }

        internal static void Extract(EmbeddedAssemblyReference embedded, string outputFile)
        {

            var output = new FileInfo(outputFile);

            var file = new FileInfo(Path.Combine(output.Directory.FullName, embedded.Name));

            embedded.Location = file.FullName;

            if (!embedded.Modified.HasValue || (file.LastWriteTime != embedded.Modified))
                ScriptUtils.SaveToFile(file, embedded.Data, embedded.Modified);

        }


        public static string UnitToText(IScriptDefinition definition, CodeCompileUnit unit)
        {
            string options;

            var provider = ScriptCompiler.CreateProvider(definition, out options);
           
            var sb = new StringBuilder();

            using (var tw = new System.IO.StringWriter(sb))
            {
                provider.GenerateCodeFromCompileUnit(unit, tw, CreateDefaultCodeGenerationOptions());
            }

            
            var str = sb.ToString();

            if (!(unit is CodeSnippetCompileUnit))
            {
                StripGeneratedCodeComment(definition, ref str);
            }

            return str;
        }

        private static void StripGeneratedCodeComment(IScriptDefinition definition, ref string val)
        {
            var lines = val.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (lines.Length < 9)
                return;

            var remove = lines.Take(9).ToArray();

            if (!remove.Last().EndsWith("-"))
                return;

            var valid =  lines.Skip(9).ToArray();

            val = String.Join(Environment.NewLine,valid);

        }

        public static CodeGeneratorOptions CreateDefaultCodeGenerationOptions()
        {
            return new CodeGeneratorOptions() { BracingStyle = "C" };
        }
    }
}
