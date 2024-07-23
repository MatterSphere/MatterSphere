using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FWBS.Common;
using FWBS.OMS.Data;

namespace FWBS.OMS.Script
{
    internal enum ScriptFlags
    {
        None = 0,
        Basic = 1,
        Advanced = 2,
    }


    public class ScriptGen : CommonObject, IDisposable
    {


        #region Constructor

        public ScriptGen()
        {
            base.Create();
        }

        /// <summary>
        /// The default constructor that sets the new assembly up with the relevant reference includes.
        /// </summary>
        /// <param name="ScriptCode">The type of script type object to create.</param>
        /// <param name="ScriptType">The type of script type object to create.</param>
        public ScriptGen(string ScriptCode, string ScriptType)
        {

            if (String.IsNullOrWhiteSpace(ScriptType))
                throw new ArgumentNullException("ScriptType");

            base.Create();

            _data.Columns["scrVersion"].DefaultValue = 1;
            _data.Columns["scrFormat"].DefaultValue = 0;


            SetExtraInfo("scrCode", ScriptCode);
            SetExtraInfo("scrType", ScriptType);
            

            Type t = Session.CurrentSession.TypeManager.Load(GetAssemblyTypeInfo(ScriptType));
            _scriptType = (ScriptType)Activator.CreateInstance(t);

            __settings = new Common.ConfigSetting(_data.Rows[0], "scrText");

            if (String.IsNullOrWhiteSpace(ScriptCode))
            {
                IsDirty = false;
            }
            else
            {
                ConvertToAdvancedInternal();

                IsDirty = true;
            }
        }

        public override bool IsDirty
        {
            get;
            set;
        }

        /// <summary>
        /// A constructor that fetches a particular script item from the database.
        /// </summary>
        /// <param name="code">Script code name.</param>
        internal ScriptGen(string code)
        {
            Fetch(code);
        }


        private ScriptGen(DataTable data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            _data = data.Copy();
            Init();
        }
        #endregion

        #region Fields
        /// <summary>
        /// .NET Source code language being used.
        /// </summary>
        private ScriptLanguage _lang = ScriptLanguage.CSharp;

        private ScriptFlags? _flags;

        /// <summary>
        /// The script type to base the script on.
        /// </summary>
        private ScriptType _scriptType = null;

        /// <summary>
        /// The script type to base the script on.
        /// </summary>
        private ScriptType _scriptlet = null;
        /// <summary>
        /// An xml representation of the scripting.
        /// </summary>
        private Common.ConfigSetting __settings = null;

        #endregion

        #region Events

        public event FWBS.OMS.MessageEventHandler CompileOutput;
        public event EventHandler CompileStart;
        public event EventHandler CompileFinished;
        public event EventHandler CompileError;
        public event EventHandler Converted;
        public event EventHandler NewScript;

        #endregion

        #region CommonObject

        protected override string SelectStatement
        {
            get { return "select * from dbscript"; }
        }

        protected override string PrimaryTableName
        {
            get { return "SCRIPT"; }
        }

        protected override string DefaultForm
        {
            get { return ""; }
        }

        public override object Parent
        {
            get { return null; }
        }

        public override string FieldPrimaryKey
        {
            get { return "scrCode"; }
        }

        protected override DataTable FetchSchema()
        {
            var dt = base.FetchSchema();

            if (dt.Columns.Contains("scrFlags"))
                _flags = ScriptFlags.None;

            return dt;
        }

        protected override void Fetch(object id)
        {
            string sql = String.Format(@"select S.scrCode, S.scrType, CONVERT(NTEXT,'') as scrText, S.scrVersion, S.scrAuthor, S.scrFormat {0} S.Created, S.CreatedBy, S.Updated, S.UpdatedBy, T.scrAssemblyType,
    							dbo.GetCodeLookupDesc('SCRIPT', S.scrCode, @UI) as [scrdesc], 
    							dbo.GetCodeLookupDesc('SCRIPTTYPE', T.scrType, @UI) as [scrTypeDesc] 
    							from dbscript S 
    							inner join dbscripttype T on T.scrtype = S.scrType 
    							where S.scrcode = @SCRIPT", _flags.HasValue ? ", scrFlags," : ",");

            _data = Session.CurrentSession.Connection.ExecuteSQLTable(sql, PrimaryTableName, new IDataParameter[2] { Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name), Session.CurrentSession.Connection.AddParameter("SCRIPT", SqlDbType.NVarChar, 15, id) });

            if (_data == null || _data.Rows.Count == 0)
            {
                throw new ScriptException(HelpIndexes.ScriptNotFound, Convert.ToString(id));
            }

            Init();
        }

        public new ScriptGen Clone()
        {
            ScriptGen clone = base.Clone() as ScriptGen;
            clone.__settings = null;
            string xml = clone._settings.DocObject.OuterXml;
            clone.Language = this.Language;
            return clone;
        }
        #endregion

        private FWBS.Common.ConfigSetting _settings
        {
            get
            {
                if (__settings == null)
                {
                    Trace.WriteLine(String.Format("Downloading Script XML for '{0}' ...", this.Code), "Scripting");
                    string sql = @"select scrText from dbscript where scrcode = @SCRIPT";
                    DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "SCRTEXT", new IDataParameter[2] { Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name), Session.CurrentSession.Connection.AddParameter("SCRIPT", SqlDbType.NVarChar, 15, this.Code) });
                    if (data.Rows.Count != 0)
                        _data.Rows[0]["scrText"] = data.Rows[0]["scrText"];
                    __settings = new Common.ConfigSetting(_data.Rows[0], "scrText");

                    if (_flags.HasValue && _flags == ScriptFlags.None)
                    {
                        _flags |= AdvancedScriptInternal ? ScriptFlags.Advanced : ScriptFlags.Basic;
                        SetExtraInfo("scrFlags", (int)_flags);
                        Update();
                    }
                }
                return __settings;
            }
        }

        private void Init()
        {
            _lang = (ScriptLanguage)FWBS.Common.ConvertDef.ToEnum(GetExtraInfo("scrFormat").ToString(), ScriptLanguage.CSharp);

            if (_flags.HasValue)
            {
                _flags = (ScriptFlags)FWBS.Common.ConvertDef.ToEnum(GetExtraInfo("scrFlags").ToString(), ScriptFlags.None);
            }

            Type t = Session.CurrentSession.TypeManager.Load(Convert.ToString(GetExtraInfo("scrassemblytype")));

            _scriptType = (ScriptType)Activator.CreateInstance(t);
            _data.AcceptChanges();
            IsDirty = false;
        }


        #region IDisposable Implementation

        /// <summary>
        /// Disposes all internal objects used by this object.
        /// </summary>
        /// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_data != null)
                    {
                        _data.Dispose();
                        _data = null;
                    }
                    _scriptType = null;

                    if (_scriptlet != null)
                    {
                        _scriptlet.Dispose();
                        _scriptlet = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }

            //Dispose unmanaged objects.
        }


        #endregion

        #region Event Raising Methods

        /// <summary>
        /// Raises the CompileOutput event handler.
        /// </summary>
        protected void OnCompileOutput(MessageEventArgs e)
        {
            if (CompileOutput != null)
                CompileOutput(this, e);
        }

        /// <summary>
        /// Raises the CompileStart event handler.
        /// </summary>
        protected void OnCompileStart()
        {
            if (CompileStart != null)
                CompileStart(this, new EventArgs());
        }

        /// <summary>
        /// Raises the CompileFinished event handler.
        /// </summary>
        protected void OnCompileFinished()
        {
            if (CompileFinished != null)
                CompileFinished(this, new EventArgs());
        }

        /// <summary>
        /// Raises the CompileError event handler.
        /// </summary>
        protected void OnCompileError()
        {
            if (CompileError != null)
                CompileError(this, new EventArgs());
        }

        protected void OnConverted()
        {
            if (Converted != null)
                Converted(this, EventArgs.Empty);
        }


        protected void OnNewScript()
        {
            if (NewScript != null)
                NewScript(this, EventArgs.Empty);
        }


        #endregion

        #region Methods

        public void ClearCustomFields()
        {
            System.Xml.XmlElement el = _settings.DocObject.SelectSingleNode("/config/script/customFields") as System.Xml.XmlElement;
            if (el != null)
            {
                el.InnerXml = String.Empty;
            }
        }

        public void ClearProcedures()
        {
            ClearProcedures("/config/script/methods");
            ClearProcedures("/config/script/dynamicMethods");
            ClearProcedures("/config/script/staticMethods");
        }

        private void ClearProcedures(string xpath)
        {
            System.Xml.XmlElement el = _settings.DocObject.SelectSingleNode(xpath) as System.Xml.XmlElement;
            if (el != null)
            {
                for (int ctr = el.ChildNodes.Count - 1; ctr >= 0; ctr--)
                {
                    System.Xml.XmlElement e = el.ChildNodes[ctr] as System.Xml.XmlElement;
                    if (e != null)
                    {
                        if (e.InnerText.Trim() == String.Empty)
                        {
                            el.RemoveChild(e);
                            IsDirty = true;
                            OnDataChanged();
                        }
                    }
                }
            }
        }

        public bool RemoveField(string fieldName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/fields/field[@name='" + System.Xml.XmlConvert.EncodeNmToken(fieldName) + "']");
            if (nd != null)
            {
                nd.ParentNode.RemoveChild(nd);
                IsDirty = true;
                OnDataChanged();
                return true;
            }
            else
                return false;
        }

        public bool RemoveCustomField(string fieldName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/customFields/customField[@name='" + System.Xml.XmlConvert.EncodeNmToken(fieldName) + "']");
            if (nd != null)
            {
                nd.ParentNode.RemoveChild(nd);
                IsDirty = true;
                OnDataChanged();
                return true;
            }
            else
                return false;
        }


        public bool RemoveProcedureCode(string ProcedureName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/methods/method[@name='" + System.Xml.XmlConvert.EncodeNmToken(ProcedureName) + "']");
            if (nd != null)
            {
                nd.ParentNode.RemoveChild(nd);
                IsDirty = true;
                OnDataChanged();
                return true;
            }
            else
                return false;
        }

        public bool RemoveStaticProcedureCode(string ProcedureName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/staticMethods/staticMethod[@name='" + System.Xml.XmlConvert.EncodeNmToken(ProcedureName) + "']");
            if (nd != null)
            {
                nd.ParentNode.RemoveChild(nd);
                IsDirty = true;
                OnDataChanged();
                return true;
            }
            else
                return false;
        }

        public bool RemoveDynamicProcedureCode(string ProcedureName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/dynamicMethods/dynamicMethod[@name='" + System.Xml.XmlConvert.EncodeNmToken(ProcedureName) + "']");
            if (nd != null)
            {
                nd.ParentNode.RemoveChild(nd);
                IsDirty = true;
                OnDataChanged();
                return true;
            }
            else
                return false;
        }

        public IReference[] GetReferences()
        {
            var def = new BasicScriptDefinition(this);

            return def.References.ToArray();
        }

        public void SetReferences(IReference[] references)
        {
            if (references == null)
                throw new ArgumentNullException("references");

            Dictionary<string, IReference> removedup = new Dictionary<string, IReference>();
            foreach (var item in references)
            {
                if (!removedup.ContainsKey(item.Name))
                    removedup.Add(item.Name,item);
            }
            references = removedup.Values.ToArray();


            var assemblies = references
             .OfType<AssemblyReference>()
             .Where(r => r != null && r.IsRequired == false)
             .Select(r => r.Location ?? r.Name)
             .ToArray();

            IEnumerable<IReference> dist = references
             .OfType<DistributedAssemblyReference>()
             .Where(r => r != null)
             .ToArray();

            var embedded = references
             .OfType<EmbeddedAssemblyReference>()
             .Where(r => r != null)
             .ToArray();

            var scripted = references
             .OfType<ScriptReference>()
             .Where(r => r != null)
             .Select(r => r.Location ?? r.Name)
             .ToArray();

            SetReferences(assemblies, true);
            SetDistributedReferences(dist.Union(embedded).ToArray());
            SetScriptReferences(scripted);

            if (IsDirty)
            {
                OnDataChanged();
            }
        }


        private void SetDistributedReferences(IReference[] references)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/distributions");
            if (nd == null)
            {
                nd = _settings.DocObject.CreateElement("distributions");
                _settings.DocObject.SelectSingleNode("/config/script").AppendChild(nd);
            }

            nd.RemoveAll();
            foreach (var s in references)
            {
                if (s == null || string.IsNullOrWhiteSpace(s.Name))
                    continue;

                System.Xml.XmlElement nnd = nd.OwnerDocument.CreateElement("distribution");

                var dist = s as DistributedAssemblyReference;
                var embedded = s as EmbeddedAssemblyReference;

                if (dist == null && embedded == null)
                    continue;

                var att_name = _settings.DocObject.CreateAttribute("filename");
                nnd.Attributes.Append(att_name);
                att_name.Value = s.Name;

                var att_isdist = _settings.DocObject.CreateAttribute("isDistributed");
                nnd.Attributes.Append(att_isdist);

                if (embedded != null)
                {
                    if (String.IsNullOrWhiteSpace(embedded.Location))
                        continue;

                    nnd.Attributes.Remove(att_isdist);

                    var file = new System.IO.FileInfo(embedded.Location);

                    att_name.Value = s.Location;

                    if (file.Exists)
                    {
                        nnd.InnerText = Serialize(file.FullName);                        
                        var att_mod = _settings.DocObject.CreateAttribute("modified");
                        att_mod.Value = file.LastWriteTime.ToLongDateString() + " " + file.LastWriteTime.ToLongTimeString();
                        nnd.Attributes.Append(att_mod);
                    }                    

                }

                if (dist != null)
                {
                    att_isdist.Value = bool.TrueString;
                }

                nd.AppendChild(nnd);

                SetReferences(new string[] { s.Name }, false);
            }
            IsDirty = true;

        }





        private void SetReferences(string[] references, bool remove)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/references");
            if (nd == null)
            {
                nd = _settings.DocObject.CreateElement("references");
                _settings.DocObject.SelectSingleNode("/config/script").AppendChild(nd);
            }

            if (remove)
                nd.RemoveAll();

            foreach (string s in references)
            {
                if (String.IsNullOrWhiteSpace(s))
                    continue;

                
                System.Xml.XmlElement nnd = nd.OwnerDocument.CreateElement("reference");
                nnd.InnerText = s;
                nd.AppendChild(nnd);
            }
            IsDirty = true;
        }

        private void SetScriptReferences(string[] references)
        {
            var el_script = _settings.DocObject.SelectSingleNode("/config/script");

            var nd = el_script.SelectSingleNode("scriptReferences");

            if (nd == null)
            {
                nd = _settings.DocObject.CreateElement("scriptReferences");
                el_script.AppendChild(nd);
            }

            nd.RemoveAll();

            foreach (string s in references)
            {
                if (String.IsNullOrEmpty(s))
                    continue;

                var r = nd.OwnerDocument.CreateElement("reference");
                r.InnerText = s;
                nd.AppendChild(r);
            }

            IsDirty = true;
        }


        private System.Xml.XmlElement CreateAdvancedScriptElement()
        {
            System.Xml.XmlElement nd1 = (System.Xml.XmlElement)_settings.DocObject.SelectSingleNode("/config/script/units");
            if (nd1 == null)
            {
                nd1 = _settings.DocObject.CreateElement("script");
                _settings.DocObject.SelectSingleNode("/config").AppendChild(nd1);
                nd1 = _settings.DocObject.CreateElement("units");
                _settings.DocObject.SelectSingleNode("/config/script").AppendChild(nd1);
            }

            System.Xml.XmlElement nd2 = (System.Xml.XmlElement)nd1.SelectSingleNode("units");
            if (nd2 == null)
            {
                nd2 = _settings.DocObject.CreateElement("units");
                nd1.AppendChild(nd2);
            }

            return nd2;
        }


        public bool AdvancedScript
        {
            get
            {
                if (!_flags.HasValue || _flags.Value == ScriptFlags.None)
                {
                    return AdvancedScriptInternal;
                }
                else
                {
                    return _flags.Value.HasFlag(ScriptFlags.Advanced);
                }
            }
            internal set
            {
                if (_flags.HasValue)
                {
                    var old = _flags;

                    if (value)
                    {
                        _flags |= ScriptFlags.Advanced;
                        _flags &= ~ScriptFlags.Basic;
                    }
                    else
                    {
                        _flags &= ~ScriptFlags.Advanced;
                        _flags |= ScriptFlags.Basic;
                    }

                    if (_flags != old)
                        IsDirty = true;
                }
            }
        }

        internal ScriptFlags? Flags
        {
            get
            {
                return _flags;
            }
            set
            {
                _flags = value;
            }
        }

        private bool AdvancedScriptInternal
        {
            get
            {
                System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/units");
                if (nd != null && nd.ChildNodes.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public string SourceCode
        {
            get
            {
                if (!AdvancedScript)
                    return null;

                var el = CreateAdvancedScriptElement();

                if (String.IsNullOrWhiteSpace(el.InnerText))
                    return null;

                return DeSerialize(el.InnerText);
            }
            set
            {
                if (!AdvancedScript)
                    throw new InvalidOperationException("Not setup for advanced scripting.");

                var el = CreateAdvancedScriptElement();

                el.InnerText = ScriptUtils.ToBase64(value);
                IsDirty = true;
                OnDataChanged();
            }
        }



        public string SourceFile
        {
            get
            {
                if (!AdvancedScript)
                    return null;
                
                var advanced = new AdvancedScriptDefinition(this);
                return advanced.SourceFile;

            }
            private set
            {
                if (!AdvancedScript)
                    throw new InvalidOperationException("Not setup for advanced scripting.");

                if (String.IsNullOrWhiteSpace(value) || !File.Exists(value))
                    throw new FileNotFoundException();

                var el = CreateAdvancedScriptElement();

                var file = new FileInfo(value);

                System.Xml.XmlAttribute att = el.Attributes["filename"];
                if (att == null)
                {
                    att = _settings.DocObject.CreateAttribute("filename");
                    el.Attributes.Append(att);
                }

                att.Value = file.Name;

                att = el.Attributes["modified"];
                if (att == null)
                {
                    att = _settings.DocObject.CreateAttribute("modified");
                    el.Attributes.Append(att);
                }

                att.Value = file.LastWriteTime.ToLongDateString() + " " + file.LastWriteTime.ToLongTimeString();

                IsDirty = true;
                OnDataChanged();

            }
        }

        public void SetSourceFile(string sourceFile)
        {
            SourceFile = sourceFile;

            var el = CreateAdvancedScriptElement();

            el.InnerText = Serialize(sourceFile);

            IsDirty = true;
            OnDataChanged();
        }


        public void ExtractDistribution(System.IO.DirectoryInfo dir)
        {
            System.Collections.Generic.List<Exception> err = new System.Collections.Generic.List<Exception>();
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/distributions");
            if (nd != null)
            {

                foreach (System.Xml.XmlNode x in nd.ChildNodes)
                {
                    try
                    {

                        System.IO.FileInfo file = new System.IO.FileInfo(x.Attributes["filename"].Value);
                        file = new System.IO.FileInfo(dir.FullName + "\\" + file.Name);
                        var embedded = new EmbeddedAssemblyReference(file.Name);
                        embedded.Location = file.FullName;
                        embedded.Data = x.InnerText;

                        string modified = "";
                        if (x.Attributes["modified"] != null) modified = x.Attributes["modified"].Value;
                        DateTime mod;

                        if (DateTime.TryParse(modified, out mod))
                        {
                            embedded.Modified = mod;
                        }

                        string isDistributed = "";
                        if (x.Attributes["isDistributed"] != null) isDistributed = x.Attributes["isDistributed"].Value;

                        bool isdist;
                        if (bool.TryParse(isDistributed, out isdist) && !isdist)
                            ScriptUtils.Extract(embedded, file.FullName);
                    }
                    catch (Exception ex)
                    {
                        if (CompileOutput != null)
                            OnCompileOutput(new MessageEventArgs(ex));
                        else
                            throw ex;
                    }
                }
            }
        }



        public void SetProcedureCode(string ProcedureName, string Code)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/methods/method[@name='" + System.Xml.XmlConvert.EncodeNmToken(ProcedureName) + "']");
            if (nd != null)
                nd.InnerText = Code;
            else
            {
                _settings.Current = "/config/script/methods";
                FWBS.Common.ConfigSettingItem item = _settings.AddChildItem("method");
                item.SetString("name", ProcedureName);
                item.SetString(Code);
            }
            IsDirty = true;
            OnDataChanged();

        }

        public string GetProcedureCode(string ProcedureName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/methods/method[@name='" + System.Xml.XmlConvert.EncodeNmToken(ProcedureName) + "']");
            string script = "";
            if (nd != null)
                script = nd.InnerText;
            return script;
        }

        public void SetWorkFlow(string procedureName, string workflowname, string delegateType = null)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/workflows/workflow[@name='" + System.Xml.XmlConvert.EncodeNmToken(procedureName) + "']");
            if (nd != null)
                nd.InnerText = workflowname;
            else
            {
                _settings.Current = "/config/script/workflows";
                FWBS.Common.ConfigSettingItem item = _settings.AddChildItem("workflow");
                item.SetString("name", procedureName);
                item.SetString("delegate", delegateType);
                item.SetString(workflowname);
            }
            this.IsDirty = true;
            OnDataChanged();
        }

        public void ClearWorkFlow(string procedureName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/workflows/workflow[@name='" + System.Xml.XmlConvert.EncodeNmToken(procedureName) + "']");
            nd.RemoveAll();
            this.IsDirty = true;
            OnDataChanged();
        }

        public string GetWorkflow(string procedureName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/workflows/workflow[@name='" + System.Xml.XmlConvert.EncodeNmToken(procedureName) + "']");
            string script = "";
            if (nd != null)
                script = nd.InnerText;
            return script;
        }

        public IEnumerable<System.Xml.XmlElement> GetWorkflowMethods()
        {
            var nodes = _settings.DocObject.SelectNodes("/config/script/workflows/workflow");

            foreach (System.Xml.XmlElement n in nodes)
                yield return n;

        }

        public void SetDynamicProcedureCode(string ProcedureName, string Code, string delegateType)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/dynamicMethods/dynamicMethod[@name='" + System.Xml.XmlConvert.EncodeNmToken(ProcedureName) + "']");
            if (nd != null)
                nd.InnerText = Code;
            else
            {
                _settings.Current = "/config/script/dynamicMethods";
                FWBS.Common.ConfigSettingItem item = _settings.AddChildItem("dynamicMethod");
                item.SetString("name", ProcedureName);
                item.SetString("delegate", delegateType);
                item.SetString(Code);
            }
            IsDirty = true;
            OnDataChanged();
        }

        public string GetDynamicProcedureCode(string ProcedureName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/dynamicMethods/dynamicMethod[@name='" + System.Xml.XmlConvert.EncodeNmToken(ProcedureName) + "']");
            string script = "";
            if (nd != null)
                script = nd.InnerText;
            return script;
        }

        public void SetField(string fieldName, string type)
        {
            System.Xml.XmlElement el = _settings.DocObject.SelectSingleNode("/config/script/fields/field[@name='" + System.Xml.XmlConvert.EncodeNmToken(fieldName) + "']") as System.Xml.XmlElement;
            if (el != null)
                el.SetAttribute("type", type);
            else
            {
                _settings.Current = "/config/script/fields";
                FWBS.Common.ConfigSettingItem item = _settings.AddChildItem("field");
                item.SetString("name", fieldName);
                item.SetString("type", type);
            }
            IsDirty = true;
            OnDataChanged();
        }

        public void SetCustomField(string fieldName, string type)
        {
            System.Xml.XmlElement el = _settings.DocObject.SelectSingleNode("/config/script/customFields/customField[@name='" + System.Xml.XmlConvert.EncodeNmToken(fieldName) + "']") as System.Xml.XmlElement;
            if (el != null)
                el.SetAttribute("type", type);
            else
            {
                _settings.Current = "/config/script/customFields";
                FWBS.Common.ConfigSettingItem item = _settings.AddChildItem("customField");
                item.SetString("name", fieldName);
                item.SetString("type", type);
            }
            IsDirty = true;
            OnDataChanged();
        }

        public void SetStaticProcedureCode(string ProcedureName, string Code)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/staticMethods/staticMethod[@name='" + System.Xml.XmlConvert.EncodeNmToken(ProcedureName) + "']");
            if (nd != null)
                nd.InnerText = Code;
            else
            {
                _settings.Current = "/config/script/staticMethods";
                FWBS.Common.ConfigSettingItem item = _settings.AddChildItem("staticMethod");
                item.SetString("name", ProcedureName);
                item.SetString(Code);
            }
            IsDirty = true;
            OnDataChanged();
        }

        public string GetStaticProcedureCode(string ProcedureName)
        {
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/staticMethods/staticMethod[@name='" + System.Xml.XmlConvert.EncodeNmToken(ProcedureName) + "']");
            string script = "";
            if (nd != null)
                script = nd.InnerText;
            return script;
        }

        public FWBS.Common.ConfigSettingItem[] GetMethodProcedures()
        {
            _settings.Current = "/config/script/methods";
            return _settings.CurrentChildItems;
        }

        public FWBS.Common.ConfigSettingItem[] GetUnAssignedDynamicProcedures()
        {
            _settings.Current = "/config/script/dynamicMethods";
            return _settings.CurrentChildItems;
        }

        public FWBS.Common.ConfigSettingItem[] GetStaticProcedures()
        {
            _settings.Current = "/config/script/staticMethods";
            return _settings.CurrentChildItems;
        }

        public string GetAssemblyTypeInfo(string ScriptType)
        {
            return GetScriptTypeName(ScriptType);
        }

        public static string GetScriptTypeName(string ScriptType)
        {
            Session.CurrentSession.CheckLoggedIn();

            DataTable _scripttype = Session.CurrentSession.Connection.ExecuteSQLTable("select scrAssemblyType from dbScriptType where [scrType] = @SCRIPT", "ScriptType", new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("SCRIPT", SqlDbType.NVarChar, 15, ScriptType) });
            if (_scripttype.Rows.Count > 0)
                return Convert.ToString(_scripttype.Rows[0]["scrAssemblyType"]);
            else
                throw new ScriptException(HelpIndexes.ScriptNotFound, ScriptType);
        }

        private string Serialize(string fileName)
        {
            using (System.IO.FileStream reader = new System.IO.FileStream(fileName, System.IO.FileMode.Open,FileAccess.Read,FileShare.Read))
            {
                byte[] buffer = new byte[reader.Length];
                reader.Read(buffer, 0, (int)reader.Length);
                return Convert.ToBase64String(buffer);
            }
        }

        public void RenameClass(string OldName, string NewName)
        {
            string outputname = CreateOutputName();
            System.IO.DirectoryInfo dir = new System.IO.FileInfo(outputname).Directory;
            System.Xml.XmlNode nd = _settings.DocObject.SelectSingleNode("/config/script/units");
            if (nd != null)
            {
                System.Xml.XmlNode x = nd.ChildNodes[0];
                string code = DeSerialize(x.InnerText);

                if (this._lang == ScriptLanguage.CSharp)
                {
                    code = code.Replace("public class " + OldName, "public partial class " + NewName);
                    code = code.Replace("public partial class " + OldName, "public partial class " + NewName);
                }
                else
                {
                    code = code.Replace("Public Class " + OldName, "Public Class " + NewName);
                }



                x.InnerText = ScriptUtils.ToBase64(code);
                x.Attributes["filename"].Value = string.Empty;
                x.Attributes["modified"].Value = "01 Jan 1999 00:00:00";
            }
        }

        private string DeSerialize(string serializedFile)
        {

            string str = ScriptUtils.FromBase64(serializedFile);

            if (Language == ScriptLanguage.CSharp)
            {
                return str.Replace(String.Format("public class {0}", ClassName), String.Format("public partial class {0}", ClassName));
            }

            return str;
        }


        private void DeSerialize(string fileName, string serializedFile, string modified)
        {
            using (System.IO.FileStream reader = System.IO.File.Create(fileName))
            {
                byte[] buffer = Convert.FromBase64String(serializedFile);
                reader.Write(buffer, 0, buffer.Length);
            }
            if (modified != "")
            {
                System.IO.FileInfo file = new System.IO.FileInfo(fileName);
                try { file.LastWriteTime = Convert.ToDateTime(modified); }
                catch (FormatException) { }
                catch (InvalidCastException) { }
            }
        }

        public void ConvertToAdvanced()
        {
            if (AdvancedScript)
                return;

            ConvertToAdvancedInternal();

            OnConverted();

            this.IsDirty = true;
            OnDataChanged();
        }

        private void ConvertToAdvancedInternal()
        {
            var basic = new BasicScriptDefinition(this);

            var builder = basic.CreateBuilder();

            var code = builder.Build(basic).First();

            var el = CreateAdvancedScriptElement();

            if (_flags.HasValue)
            {
                _flags = ScriptFlags.Advanced;
            }


            var advanced = new AdvancedScriptDefinition(this);

            SourceCode = ScriptUtils.UnitToText(advanced, code);

            SourceFile = advanced.SourceFile;
        }

        public void ConvertToBasic()
        {
            if (!AdvancedScript)
                return;

            var el = CreateAdvancedScriptElement();

            el.ParentNode.RemoveAll();

            OnConverted();

            IsDirty = true;
            OnDataChanged();
        }

        /// <summary>
        /// Compiles the script into an assembly.
        /// </summary>
        public bool Compile()
        {
            return Compile(false);
        }

        /// <summary>
        /// Compiles the script into an assembly using the Force will overload to Delete any old code
        /// </summary>
        /// <param name="force">Force the application to compile from scratch</param>
        /// <returns></returns>
        public bool Compile(bool force)
        {
            return Compile(force, false);
        }

        /// <summary>
        /// Compiles the script into an assembly using the Force will overload to Delete any old code
        /// </summary>
        /// <param name="force">Force the application to compile from scratch</param>
        /// <param name="compilerResults">The compiler results</param>
        /// <returns></returns>
        public bool Compile(bool force, out CompilerResults compilerResults)
        {
            return Compile(force, false, out compilerResults);
        }

        /// <summary>
        /// Compiles the Script into an assembly if you elect to you can ask for it to just compile the source code file.
        /// </summary>
        /// <param name="force">Force the application to compile from scratch</param>
        /// <param name="sourcefileonly">Force the application to compile only the source code file</param>
        /// <returns></returns>
        public bool Compile(bool force, bool sourcefileonly)
        {
            CompilerResults compilerResults;
            return Compile(force, sourcefileonly, out compilerResults);
        }

        /// <summary>
        /// Compiles the Script into an assembly if you elect to you can ask for it to just compile the source code file.
        /// </summary>
        /// <param name="force">Force the application to compile from scratch</param>
        /// <param name="sourcefileonly">Force the application to compile only the source code file</param>
        /// <param name="compilerResults">The compiler results</param>
        /// <returns></returns>
        public bool Compile(bool force, bool sourcefileonly, out CompilerResults compilerResults)
        {
            if (string.IsNullOrEmpty(Code))
                throw new ArgumentException("Code cannot be null or empty");

            var factory = new ScriptFactory();

            var def = factory.CreateDefinition(this);

            var compiler = (ScriptCompiler)factory.CreateCompiler(def);

            EventHandler error = (s, e) => OnCompileError();
            EventHandler start = (s, e) => OnCompileStart();
            EventHandler finished = (s, e) => OnCompileFinished();
            MessageEventHandler output = (s, e) => OnCompileOutput(e);

            try
            {

                compiler.Error += error;
                compiler.Start += start;
                compiler.Finished += finished;
                compiler.Output += output;

                compilerResults = compiler.Compile(new CompileOptions() { Force = force });

                return !compilerResults.Errors.HasErrors;
            }
            finally
            {
                compiler.Error -= error;
                compiler.Start -= start;
                compiler.Finished -= finished;
                compiler.Output -= output;
            }

        }


        public string CreateOverrideMethod(string name, string workflow)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");


            var meth = _scriptType.Methods.First(m => m.Name == name);

            var version = meth.Comments.FirstOrDefault<CodeCommentStatement>((n) => n.Comment.Text.StartsWith("VERSION"));

            using (var tw = new System.IO.StringWriter())
            {

                var provider = ScriptCompiler.CreateProvider(_lang);

                if (version != null)
                {
                    meth.Comments.Remove(version);
                }

                if (!String.IsNullOrWhiteSpace(workflow))
                {
                    var tcf = new CodeTryCatchFinallyStatement();
                    var invoke = CodeDomScriptDefinition.CreateWorkflowInvokeMethod(workflow);
                    tcf.TryStatements.Add(CreateVersionDirective(new Version(5, 0, 0, 0)));
                    tcf.TryStatements.Add(invoke);
                    tcf.TryStatements.Add(CreateEndIfDirective());
                    meth.Statements.Add(tcf);
                    tcf.AddCatchErrorBoxHandler();
                }

                provider.GenerateCodeFromMember(meth, tw, ScriptUtils.CreateDefaultCodeGenerationOptions());

                var sb = tw.GetStringBuilder();

                if (version != null && !String.IsNullOrWhiteSpace(version.Comment.Text))
                {
                    if (_lang == ScriptLanguage.CSharp)
                    {
                        sb.Insert(0, String.Format("#if {0}", version.Comment.Text));
                        sb.AppendLine("#endif");
                    }
                    else
                    {
                        sb.Insert(0, String.Format("#If {0} Then", version.Comment.Text));
                        sb.AppendLine("#End If");
                    }

                }
                
                return StripDirectiveEndOfLine(sb.ToString());

            }
        }


        public string CreateEventHandler(string name, string delegateType, string workflow)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            CodeTryCatchFinallyStatement tcf;

            var provider = ScriptCompiler.CreateProvider(_lang);

            using (var tw = new StringWriter())
            {
                var meth = CodeDomScriptDefinition.CreateDelegateMethod(name, delegateType, " ", out tcf);

                if (!String.IsNullOrWhiteSpace(workflow))
                {
                    var invoke = CodeDomScriptDefinition.CreateWorkflowInvokeMethod(workflow);
                    tcf.TryStatements.Clear();
                    tcf.TryStatements.Add(CreateVersionDirective(new Version(5, 0, 0, 0)));
                    tcf.TryStatements.Add(invoke);
                    tcf.TryStatements.Add(CreateEndIfDirective());
                }
                tcf.AddCatchErrorBoxHandler();

                provider.GenerateCodeFromMember(meth, tw, ScriptUtils.CreateDefaultCodeGenerationOptions());

                return StripDirectiveEndOfLine(tw.GetStringBuilder().ToString());
            }

        }

        private static string StripDirectiveEndOfLine(string val)
        {
            return val.Replace(" //;", "");
        }

        private CodeSnippetExpression CreateVersionDirective(Version version)
        {
            var str = String.Format(ScriptCompiler.VersionFormat, version.Major, version.Minor, version.Revision, version.Build);

            if (_lang == ScriptLanguage.CSharp)
            {
                return new CodeSnippetExpression(String.Format("#if {0} //",str));
            }
            else
            {
                return new CodeSnippetExpression(String.Format("#If {0} Then", str));
            }

        }

        private CodeSnippetExpression CreateEndIfDirective()
        {
            if (_lang == ScriptLanguage.CSharp)
                return new CodeSnippetExpression("#endif //");
            else
                return new CodeSnippetExpression("#End If");
        }

        public void Load()
        {
            Load(false);
        }

        /// <summary>
        /// Loads the assembly into the current application domain.
        /// </summary>
        public void Load(bool DesignMode)
        {

            if (DesignMode == false)
            {
                if (_scriptlet == null && Code != "")
                {
                    var factory = new ScriptFactory();

                    var def = factory.CreateDefinition(this);

                    var loader = factory.CreateLoader(def);

                    try
                    {
                        _scriptlet = (ScriptType)loader.Load(null);
                    }
                    catch (InvalidScriptDefinitionException)
                    {
                        AdvancedScript = false;

                        Update();

                        def = factory.CreateDefinition(this);

                        loader = factory.CreateLoader(def);

                        _scriptlet = (ScriptType)loader.Load(null);                        
                    }

                    _scriptlet.SetScriptGeneratorObject(this, null);
                }
            }
        }

        public override void SetExtraInfo(string fieldName, object val)
        {
            base.SetExtraInfo(fieldName, val);
            IsDirty = true;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Fetches a scriplet object from the OMS script library.
        /// </summary>
        /// <param name="code">The specific script to pull back.</param>
        /// <returns>A script generator object.</returns>
        public static ScriptGen GetScript(string code)
        {
            Session.CurrentSession.CheckLoggedIn();
            return new ScriptGen(code);
        }

        /// <summary>
        /// Does the Code Exist
        /// </summary>
        /// <returns>Boolean.</returns>
        public static bool Exists(string Code)
        {
            Session.CurrentSession.CheckLoggedIn();
            object dt = Session.CurrentSession.Connection.ExecuteSQLScalar("select scrcode from dbScript where scrCode = @Code", new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code) });
            if (dt != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns a DataTable of Scripts that match a Script Type
        /// </summary>
        /// <param name="ScriptType">Script Type code e.g. Precedent</param>
        /// <returns>returns a DataTable</returns>
        public static DataTable GetScripts(string ScriptType)
        {
            Session.CurrentSession.CheckLoggedIn();
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT scrCode FROM DBScript where scrType = @Code", "EXISTS", new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, ScriptType) });
            return dt;
        }

        internal static IEnumerable<ScriptTuple> GetScriptReferences(IScriptDefinition definition, out Tuple<ScriptTuple, ScriptTuple> circularReference)
        {
            Session.CurrentSession.CheckLoggedIn();

            if (definition == null)
                throw new ArgumentNullException("definition");

            circularReference = null;

            var factory = new ScriptFactory();

            var visited = new Dictionary<string, ScriptTuple>(StringComparer.OrdinalIgnoreCase);

            var refs = new List<ScriptTuple>();

            visited.Add(definition.Name, new ScriptTuple(definition, new ScriptReference(definition.Name), null));

            foreach (var reference in definition.References.OfValidType<ScriptReference>().Where(r => r != null))
            {
                var gen = ScriptGen.GetScript(reference.Name);

                var def = factory.CreateDefinition(gen);

                var tuple = new ScriptTuple(def, reference, gen);

                refs.Add(tuple);

                var ret = RecurseScriptReferences(factory, tuple, visited);

                if (ret != null && ret.Item1 != null && ret.Item2 != null)
                {
                    circularReference = ret;
                    break;
                }
            }


            return refs.ToArray();
        }


        private static Tuple<ScriptTuple, ScriptTuple> RecurseScriptReferences(ScriptFactory factory, ScriptTuple parent, Dictionary<string, ScriptTuple> visited)
        {
            if (parent == null)
                return null;

            ScriptTuple circular;
            if (visited.TryGetValue(parent.Item1.Name, out circular))
                return new Tuple<ScriptTuple, ScriptTuple>(circular, null);


            visited.Add(parent.Item1.Name, parent);

            foreach (var reference2 in parent.Item1.References.OfValidType<ScriptReference>().Where(r => r != null))
            {
                var gen = ScriptGen.GetScript(reference2.Name);

                var def = factory.CreateDefinition(gen);

                var tuple = new ScriptTuple(def, reference2, gen);

                var ret = RecurseScriptReferences(factory, tuple, visited);

                if (ret != null)
                    return new Tuple<ScriptTuple, ScriptTuple>(tuple, ret.Item1);
            }

            visited.Remove(parent.Item1.Name);

            return null;
        }

        public static DataTable GetScripts()
        {
            Session.CurrentSession.CheckLoggedIn();
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT scrCode FROM DBScript", "SCRIPTS", null);
            return dt;
        }

        public static ScriptGen[] GetMenuScripts()
        {
            return GetScriptsByType("MENU");
        }

        public static ScriptGen[] GetSystemScripts()
        {
            return GetScriptsByType("SYSTEM");
        }

        private static ScriptGen[] GetScriptsByType(string type)
        {
            Session.CurrentSession.CheckLoggedIn();

            string sql = @"select S.*, T.scrAssemblyType,
    							dbo.GetCodeLookupDesc('SCRIPT', S.scrCode, @UI) as [scrdesc], 
    							dbo.GetCodeLookupDesc('SCRIPTTYPE', T.scrType, @UI) as [scrTypeDesc] 
    							from dbscript S 
    							inner join dbscripttype T on T.scrtype = S.scrType 
    							where S.scrtype = @CODE order by scrcode";

            System.Collections.Generic.List<ScriptGen> list = new System.Collections.Generic.List<ScriptGen>();

            using (DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "SCRIPTS", new IDataParameter[] 
            { 
                Session.CurrentSession.Connection.CreateParameter("Code", type), 
                Session.CurrentSession.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name) 
            }))
            {
                foreach (DataRow r in dt.Rows)
                {
                    using (DataView vw = new DataView(dt))
                    {
                        vw.RowFilter = String.Format("scrcode='{0}'", Convert.ToString(r["scrcode"]).Replace("'", "''"));
                        if (vw.Count > 0)
                            list.Add(new ScriptGen(vw.ToTable()));
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Returns a DataTable of Scripts that match a Script Type
        /// </summary>
        /// <param name="ScriptType">Script Type code e.g. Precedent</param>
        /// <returns>returns a DataTable</returns>
        public static DataTable GetDetailedScripts(string ScriptType)
        {
            Session.CurrentSession.CheckLoggedIn();
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT dbScript.*, dbUser.usrFullName AS CreatedByUser, dbUser_1.usrFullName AS UpdatedByUser FROM dbScript LEFT JOIN dbUser ON dbScript.CreatedBy = dbUser.usrID LEFT JOIN dbUser dbUser_1 ON dbScript.UpdatedBy = dbUser_1.usrID where scrType = @Code ORDER BY dbScript.Created", "EXISTS", new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, ScriptType) });
            return dt;
        }

        /// <summary>
        /// Deletes a Script
        /// </summary>
        /// <param name="Code">Script Name</param>
        /// <returns>True if Succesful;</returns>
        public static bool Delete(string Code)
        {
            Session.CurrentSession.CheckLoggedIn();
            try
            {
                Session.CurrentSession.Connection.ExecuteSQL("delete from dbScript where scrCode = @Code", new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code) });
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique script code from the database.
        /// </summary>
        public string Code
        {
            get
            {
                return Convert.ToString(GetExtraInfo("scrcode"));
            }
            set
            {
                if (_data.Rows[0].RowState == DataRowState.Added)
                {
                    SetExtraInfo("scrcode", value);
                    if (!string.IsNullOrEmpty(value))
                    {
                        OnNewScript();
                        this.ConvertToAdvanced();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current version of the script.
        /// </summary>
        public long Version
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt64(GetExtraInfo("scrversion"), 0);
            }
        }

        /// <summary>
        /// Gets the author name of the script.
        /// </summary>
        public string Author
        {
            get
            {
                return Convert.ToString(GetExtraInfo("scrauthor"));
            }
        }

        /// <summary>
        /// Gets the localized script description.
        /// </summary>
        public string ScriptDescription
        {
            get
            {
                if (_data.Columns.Contains("scrdesc"))
                    return Convert.ToString(GetExtraInfo("scrdesc"));
                else
                    return "";
            }
        }

        /// <summary>
        /// Gets the localized script description.
        /// </summary>
        public string ScriptTypeDescription
        {
            get
            {
                if (_data.Columns.Contains("scrtypedesc"))
                    return Convert.ToString(GetExtraInfo("scrtypedesc"));
                else
                    return "";
            }
        }

        /// <summary>
        /// Gets the scriptlet type object which will run the actual script.
        /// </summary>
        public ScriptType Scriptlet
        {
            get
            {
                return _scriptlet;
            }
        }

        /// <summary>
        /// Gets the scriptlet type object which is used to configure the admin kit.
        /// </summary>
        public ScriptType ScriptType
        {
            get
            {
                return _scriptType;
            }
        }

        public string RawXML
        {
            get
            {
                _settings.DocObject.PreserveWhitespace = true;
                _settings.DocObject.Normalize();
                return _settings.DocObject.InnerXml;
            }
            set
            {
                _settings.DocObject.InnerXml = value;
            }
        }

        /// <summary>
        /// Gets the script type, where the script is intended to be used within the system.
        /// </summary>
        public string Usage
        {
            get
            {
                try
                {
                    return Convert.ToString(GetExtraInfo("scrtype"));
                }
                catch
                {
                    return "SESSION";
                }
            }
        }

        /// <summary>
        /// Gets the output file path string.
        /// </summary>
        public string OutputName
        {
            get
            {
                FileInfo file = OutputDir.EnumerateFiles(string.Format("{0}.{1}.*.dll", Version, ClassName))
                    .OrderByDescending(f => f.CreationTimeUtc).FirstOrDefault();
                return (file != null) ? file.FullName : CreateOutputName();
            }
        }

        public string CreateOutputName()
        {
            return Path.Combine(OutputDir.FullName, new ScriptFileName(this).ToString());
        }

        public DirectoryInfo OutputDir
        {
            get
            {
                string dir = Path.Combine(Global.GetCachePath().ToString(), "Scriptlets", ClassName.ToLower());
                return System.IO.Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// Gets the class name to use.
        /// </summary>
        public string ClassName
        {
            get
            {
                return Code;
            }
        }

        public string Namespace
        {
            get
            {
                return ScriptType.Namespace;
            }
        }

        /// <summary>
        /// Gets or Sets the script type, whether CSharp or VB is preferred.
        /// </summary>
        public ScriptLanguage Language
        {
            // MNW - Enhanced to support VB :)
            get
            {
                return _lang;
            }
            set
            {
                _lang = value;
            }
        }

        public bool HasCode
        {
            get
            {
                _settings.Current = "/config/script/dynamicMethods";
                if (_settings.CurrentChildItems.Length > 0) return true;
                _settings.Current = "/config/script/methods";
                if (_settings.CurrentChildItems.Length > 0) return true;
                _settings.Current = "/config/script/staticMethods";
                if (_settings.CurrentChildItems.Length > 0) return true;
                return false;
            }
        }

        /// <summary>
        /// Updates the object and persists it to the database.
        /// </summary>	
        public override void Update()
        {
            if (string.IsNullOrEmpty(Code))
                return;

            if (IsDirty)
            {
                //Check if there are any changes made before setting the updated
                //and updated by properties then update.
                if (__settings != null)
                    SetExtraInfo("scrText", _settings.DocObject.OuterXml);
                SetExtraInfo("scrversion", IncrementVersionNumber(Convert.ToString(GetExtraInfo("scrCode")), ConvertDef.ToInt64(GetExtraInfo("scrversion"), 0), "dbScriptVersionData"));//Version + 1);
                SetExtraInfo("scrFormat", _lang);

                if (_flags.HasValue)
                {
                    SetExtraInfo("scrFlags", _flags);
                }

                base.Update();
            }
            this.IsDirty = false;
        }

        public long IncrementVersionNumber(string code, long currentversion, string versiontable)
        {
            long highestversion = GetHighestCheckedInVersion(code, versiontable);
            if (highestversion != 0 && highestversion > currentversion)
                return highestversion + 1;
            else
                return currentversion + 1;
        }

        private  long GetHighestCheckedInVersion(string code, string versiontable)
        {
            string sql = "select MAX(Version) as Version from " + versiontable + " where Code = '" + code + "'";
            List<IDataParameter> parList = new List<IDataParameter>();
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            DataTable dt = connection.ExecuteSQL(sql, parList);
            connection.Disconnect();
            return ConvertDef.ToInt64(dt.Rows[0]["Version"], 0);
        }

        #endregion

        public static string GenerateUniqueName(string name)
        {
            int t = 1;
            string acceptchars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";
            foreach (var c in name)
                if (acceptchars.Contains(c.ToString()) == false)
                    name = name.Replace(c.ToString(), "_");

            var newname = "_" + name.Replace("-", "_").Replace(" ", "_");
            if (newname.Length >= 15)
                newname = newname.Substring(0, newname.Length - (t.ToString().Length));
            while (FWBS.OMS.Script.ScriptGen.Exists(newname))
            {
                newname = name;
                if (newname.Length >= 14)
                    newname = newname.Substring(0, newname.Length - (t.ToString().Length + 1));
                newname += t.ToString();
                newname = "_" + newname;
                t++;
            }
            return newname;
        }
    }



}
