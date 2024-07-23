using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FWBS.OMS.Script
{

    internal class ScriptCompiler : IScriptCompiler
    {
        public const string VersionFormat = "VERSION_{0}_{1}_{2}_{3}";

        public static CodeDomProvider CreateProvider(IScriptDefinition definition)
        {
            string options;
            return CreateProvider(definition, out options);
        }

        public static CodeDomProvider CreateProvider(ScriptLanguage language)
        {
            if (language == ScriptLanguage.CSharp)
            {
                return CodeDomProvider.CreateProvider("csharp");
            }
            else
            {
                return CodeDomProvider.CreateProvider("vb");
            }
        }

        public static CodeDomProvider CreateProvider(IScriptDefinition definition, out string options)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            CodeDomProvider provider = null;

            var dict = GetProviderOptions(definition, out options);

            if (definition.Language == ScriptLanguage.CSharp)
            {
                provider = CodeDomProvider.CreateProvider("csharp", dict);
            }
            else
            {
                provider = CodeDomProvider.CreateProvider("vb", dict);
            }

            return provider;
        }

        private static IDictionary<string, string> GetProviderOptions(IScriptDefinition definition, out string options)
        {
            var dict = new Dictionary<string, string>();

            var sb = new StringBuilder();

            var reg = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Scripting", "ProviderOptions");

            var regval = Convert.ToString(reg.GetSetting(String.Empty));

            foreach (var item in definition.ProviderOptions)
            {
                dict[item.Item1] = item.Item2;
                if (sb.Length > 0)
                    sb.Append(" ");
                sb.AppendFormat("{0}={1}", item.Item1, item.Item2);
            }

            foreach (var item in ScriptUtils.SplitProviderOptions(regval))
            {
                dict[item.Item1] = item.Item2;
            }

            if (sb.Length > 0)
                sb.Append(" ");
            sb.Append(regval);

            options = sb.ToString();

            return dict;
        }

        #region Constants

        private const string BuildStartedMessage = "----- Build started: Scriptlet: {0}";
        private const string BuildProviderMessage = "Provider: {0}";
        private const string BuildOptionsMessage = "Options: {0}";
        private const string BuildReferenceMessage = "Refererence: {0}";
        private const string BuildOutputMessage = "{0} -> {1}";
        private const string BuildWarningMessage = "{0}: warning {1}: {2} line {3}";
        private const string BuildErrorMessage = "{0}: error {1}: {2} line {3}";
        private const string BuildCompleteMessage = "Compile complete -- {0} errors, {1} warnings";
        private const string BuildEndMessage = "========== Build: {0} succeeded or up-to-date, {1} failed, {2} skipped ==========";

        #endregion

        #region Fields

        private readonly IScriptDefinition definition;
        private readonly IScriptBuilder builder;

        #endregion

        #region Constructors

        public ScriptCompiler(IScriptDefinition definition, IScriptBuilder builder)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            if (builder == null)
                throw new ArgumentNullException("builder");

            this.builder = builder;
            this.definition = definition;
        }

        #endregion

        #region Events

        public event MessageEventHandler Output;
        public event EventHandler Start;
        public event EventHandler Finished;
        public event EventHandler Error;

        #endregion

        #region Event Raising Methods

        protected void OnOutput(MessageEventArgs e)
        {
            Trace.WriteLine(e.Message, "Scripting");

            var ev = Output;
            if (ev != null)
                ev(this, e);
        }

        protected void OnStart()
        {
            var ev = Start;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }

        protected void OnFinished()
        {
            var ev = Finished;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }

        protected void OnError()
        {
            var ev = Error;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }


        #endregion

        #region IScriptCompiler

        public CompilerResults Compile(CompileOptions options)
        {
            options = options ?? new CompileOptions();

            if (string.IsNullOrEmpty(definition.Name))
                throw new ArgumentException(Session.CurrentSession.Resources.GetMessage("SCRMTHVNMBFCMP", "The script must have a name before it is compiled.", "").Text);

            using (Fwbs.Framework.Diagnostics.TraceDuration.Start(this, String.Format("Compile - {0}", definition.Name)))
            {

                // Raise the Compile Start Event
                OnStart();

                Environment.CurrentDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

                var parameters = new CompilerParameters();

                string provideroptions;

                var provider = CreateProvider(definition, out provideroptions);

                ApplyParameters(parameters);

                parameters.OutputAssembly = options.Force ? ScriptUtils.CreateOutputName(definition) : ScriptUtils.GetOutputName(definition);

                OnOutput(new MessageEventArgs(String.Format(BuildStartedMessage, definition.Name)));

                var errors = new List<CompilerError>();

                ApplyReferences(parameters, errors);

                var code = builder.Build(definition).ToArray();

                var files = WriteSourceCode(options, parameters, provider, code);

                if (!String.IsNullOrWhiteSpace(provideroptions))
                    OnOutput(new MessageEventArgs(String.Format(BuildProviderMessage, provideroptions)));

                OnOutput(new MessageEventArgs(String.Format(BuildOptionsMessage, parameters.CompilerOptions)));

                var results = files.Length == 0 ? provider.CompileAssemblyFromDom(parameters, code) : provider.CompileAssemblyFromFile(parameters, files);

                results.Errors.AddRange(errors.ToArray());


                int errs = 0;
                int warns = 0;

                foreach (CompilerError err in results.Errors)
                {
                    if (err.IsWarning)
                    {
                        warns++;
                        OnOutput(new MessageEventArgs(String.Format(BuildWarningMessage, err.FileName, err.ErrorNumber, err.ErrorText, err.Line, MessageSeverity.Warning)));
                    }
                    else
                    {
                        errs++;
                        OnOutput(new MessageEventArgs(String.Format(BuildErrorMessage, err.FileName, err.ErrorNumber, err.ErrorText, err.Line, MessageSeverity.Error)));
                    }
                }

                OnOutput(new MessageEventArgs(String.Empty));

                OnOutput(new MessageEventArgs(String.Format(BuildCompleteMessage, errs, warns)));

                if (!results.Errors.HasErrors)
                {
                    OnOutput(new MessageEventArgs(String.Format(BuildOutputMessage, definition.Name, results.PathToAssembly)));
                    OnOutput(new MessageEventArgs(String.Format(BuildEndMessage, 1, 0, 0)));
                    OnFinished();
                }
                else
                {

                    OnOutput(new MessageEventArgs(String.Format(BuildEndMessage, 0, 1, 0)));
                    OnError();
                }


                return results;
            }


        }

        private string[] WriteSourceCode(CompileOptions options, CompilerParameters parameters, CodeDomProvider provider, IEnumerable<CodeCompileUnit> code)
        {
            var files = new List<string>();

            if (IsDebugEnabled)
            {
                int ctr = 0;

                var output = new FileInfo(parameters.OutputAssembly);

                foreach (var unit in code)
                {
                    if (unit == null)
                        continue;

                    var path = Path.Combine(output.Directory.FullName, String.Format("{0}-{1}.{2}", Path.GetFileNameWithoutExtension(output.Name), ctr, provider.FileExtension));
                    using (var file = new StreamWriter(path))
                    {
                        file.Write(ScriptUtils.UnitToText(definition, unit));
                    }
                    files.Add(path);
                    ctr++;
                }
            }

            return files.ToArray();
        }


        private string GetConditionalCompilationSymbols(IScriptDefinition definition)
        {

            var reg = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Scripting", "ConditionalCompilationOptions");

            var val = Convert.ToString(reg.GetSetting(String.Empty));

            var list = new List<string>(val.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

            var attrs = typeof(ScriptType).Assembly.GetCustomAttributes(typeof(FWBS.VersionConditionalAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                foreach (VersionConditionalAttribute attr in attrs)
                {
                    list.Add(String.Format(ScriptCompiler.VersionFormat, attr.Version.Major, attr.Version.Minor, attr.Version.Build, attr.Version.Revision));
                }
            }

            list.AddRange(definition.ConditionalCompilationSymbols);

            return String.Join(",", list.ToArray());
        }

        private string GetCompilerOptions(IScriptDefinition definition)
        {

            var reg = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Scripting", "CompilerOptions");

            var val = Convert.ToString(reg.GetSetting(String.Empty));

            var list = new List<string>(val.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

            list.AddRange(definition.CompilerOptions);

            return String.Join(" ", list.ToArray());
        }

        private void ApplyParameters(CompilerParameters parameters)
        {

            var conditionaloptions = GetConditionalCompilationSymbols(definition);

            var compileroptions = GetCompilerOptions(definition);

            CompilerParameters compparams = new CompilerParameters();

            if (!String.IsNullOrWhiteSpace(conditionaloptions))
                parameters.CompilerOptions = String.Format("/d:{0}", conditionaloptions);

            if (!String.IsNullOrWhiteSpace(compileroptions))
            {
                if (String.IsNullOrWhiteSpace(parameters.CompilerOptions))
                    parameters.CompilerOptions = compileroptions;
                else
                    parameters.CompilerOptions = String.Format("{0} {1}", parameters.CompilerOptions, compileroptions);
            }

            parameters.GenerateInMemory = false;
            parameters.IncludeDebugInformation = IsDebugEnabled;
            parameters.WarningLevel = 3;
        }

        private void ApplyReferences(CompilerParameters parameters, List<CompilerError> errors)
        {
            AddScriptReferences(errors);

            //Add all the assembly references that are needed.
            foreach (var r in definition.References)
            {
                if (AddReference(parameters, r))
                    continue;

                var embedded = r as EmbeddedAssemblyReference;
                var distrib = r as DistributedAssemblyReference;
                var script = r as ScriptReference;
                var ass = r as AssemblyReference;

                if (embedded != null)
                {
                    AddAssemblyReference(embedded, parameters, errors);
                }
                else if (distrib != null)
                {
                    AddAssemblyReference(distrib, errors);
                }
                else if (script != null)
                {
                }
                else if (ass != null)
                {
                    AddAssemblyReference(ass, errors);
                }

                if (AddReference(parameters, r))
                    continue;

            }




        }

        private bool AddReference(CompilerParameters parameters, IReference r)
        {      
            if (!string.IsNullOrWhiteSpace(r.Location) && File.Exists(r.Location))
            {
                if (!String.IsNullOrWhiteSpace(r.Name) && r.IsGlobal)
                {
                    // To fix WPF loading issue - change to add the reference ‘Location’ (i.e. full path of .DLL) instead of ‘Name’ for global reference assemblies.
                    parameters.ReferencedAssemblies.Add(r.Location);
                    OnOutput(new MessageEventArgs(String.Format(BuildReferenceMessage, r.Name)));
                }
                else
                {
                    parameters.ReferencedAssemblies.Add(r.Location);
                    OnOutput(new MessageEventArgs(String.Format(BuildReferenceMessage, r.Location)));
                }
                return true;
            }

          
            return false;
        }

        private void AddAssemblyReference(AssemblyReference reference, List<CompilerError> errors)
        {
            try
            {
                var assembly = Session.CurrentSession.AssemblyManager.Load(reference.AssemblyName ?? reference.Name);
                reference.IsGlobal = assembly.GlobalAssemblyCache;
                reference.Location = assembly.Location;
            }
            catch (FileNotFoundException fnfex)
            {
                errors.Add(new CompilerError("", 0, 0, "", fnfex.Message) { IsWarning = true });
            }
            catch (Exception ex)
            {
                errors.Add(new CompilerError("", 0, 0, "", ex.Message));
            }
        }

        private void AddAssemblyReference(DistributedAssemblyReference reference, List<CompilerError> errors)
        {
            try
            {
                var assembly = Session.CurrentSession.AssemblyManager.Load(reference.AssemblyName ?? reference.Name);
                reference.Location = assembly.Location;
            }
            catch (FileNotFoundException fnfex)
            {
                errors.Add(new CompilerError("", 0, 0, "", fnfex.Message) { IsWarning = true });
            }
            catch (Exception ex)
            {
                errors.Add(new CompilerError("", 0, 0, "", ex.Message));
            }
        }

        private void AddScriptReferences(List<CompilerError> errors)
        {
            IEnumerable<ScriptTuple> refs;

            if (CheckForCircularReferences(errors, out refs))
                return;


            foreach (var r in refs)
            {
                AddScriptReference(r, errors);
            }

        }

        private void AddScriptReference(Tuple<IScriptDefinition, ScriptReference, ScriptGen> script, List<CompilerError> errors)
        {
            IScriptType scriptlet = null;


            if (CheckForSelfReference(script.Item2, errors))
                return;

            var factory = new ScriptFactory();

            var loader = factory.CreateLoader(script.Item1);

            try
            {
                scriptlet = loader.Load(new LoadOptions() { Compile = LoadCompileOption.Never, ThrowException = false });
                if (scriptlet == null)
                {
                    var compiler = factory.CreateCompiler(script.Item1);

                    var results = compiler.Compile(new CompileOptions() { Force = true });

                    errors.AddRange(results.Errors.Cast<CompilerError>());

                    if (results.Errors.HasErrors)
                        return;

                    try
                    {
                        scriptlet = loader.Load(new LoadOptions() { Compile = LoadCompileOption.Never });
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new CompilerError("", 0, 0, "", ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(new CompilerError("", 0, 0, "", ex.Message));
            }

            if (scriptlet != null)
                script.Item2.Location = scriptlet.GetType().Assembly.Location;

        }

        private bool CheckForSelfReference(ScriptReference reference, List<CompilerError> errors)
        {
            if (string.Equals(reference.Name, definition.Name, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add(new CompilerError("", 0, 0, "", String.Format("Reference '{0}' has been skipped due to self referencing.", reference.Name)) { IsWarning = true });
                return true;
            }
            return false;
        }

        private bool CheckForCircularReferences(List<CompilerError> errors, out IEnumerable<ScriptTuple> defs)
        {

            Tuple<ScriptTuple, ScriptTuple> circular;

            defs = ScriptGen.GetScriptReferences(definition, out circular);

            if (circular != null)
            {
                errors.Add(new CompilerError("", 0, 0, "", String.Format("Reference '{0}' cannot be added due to being a circular reference with {1}.", circular.Item1.Item1.Name, circular.Item2.Item1.Name)));
                return true;
            }

            return false;
        }

        private void AddAssemblyReference(EmbeddedAssemblyReference embedded, CompilerParameters parameters, List<CompilerError> errors)
        {

            try
            {
                ScriptUtils.Extract(embedded, parameters.OutputAssembly);
            }
            catch (Exception ex)
            {
                errors.Add(new CompilerError("", 0, 0, "", ex.Message));
            }

        }

        private static bool IsDebugEnabled
        {
            get
            {
                return CanCompileToFile || Session.CurrentSession.IsInDebug;
            }
        }

        private static bool CanCompileToFile
        {
            get
            {
                bool compileToFile = false;
                var compileRegKey = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Scripting", "CompileToFile");
                if (String.IsNullOrEmpty(Convert.ToString(compileRegKey.GetSetting(""))))
                {
                    var oldcompileRegKey = new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "CompileToFile");
                    compileToFile = Convert.ToBoolean(oldcompileRegKey.GetSetting(false));
                }
                else
                {
                    compileToFile = Convert.ToBoolean(compileRegKey.GetSetting(false));
                }
                return compileToFile;
            }
        }

        #endregion




    }
}
