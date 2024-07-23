using System;
using System.IO;

namespace FWBS.OMS.Script
{
    internal sealed class AdvancedScriptDefinition : ScriptDefinition
    {
        public const string SourceFileName = "Editor";

        #region Fields

        private string code;
        private string sourceFile;
        private DateTime? modified = null;

        #endregion

        #region Constructors

        public AdvancedScriptDefinition(ScriptGen gen)
            : base(gen)
        {
        }

        #endregion

        #region Properties

        public string Code
        {
            get
            {
                Download();

                return code;
            }
        }

        public string SourceFile
        {
            get
            {
                Download();

                return sourceFile;
            }
        }

        public DateTime? LastModified
        {
            get
            {
                Download();

                return modified;
            }
        }

        #endregion

        #region Methods

        protected override void Download()
        {
            if (IsDownloaded)
                return;

            base.Download();

            BuildCode();
        }

        private string Deserialize(string data)
        {
            string str = ScriptUtils.FromBase64(data);

            if (Language == ScriptLanguage.CSharp)
            {
                var val = String.Format("partial class {0}", Name);
                if (str.IndexOf(val) < 0)
                    return str.Replace(String.Format("class {0}", Name), val);
            }

            return str;
        }



        private void BuildCode()
        {
            var el_script = Xml.Element("script");
            if (el_script == null)
                throw ThrowInvalidScriptDefinition();

            var el_units = el_script.Element("units");
            if (el_units == null)
                throw ThrowInvalidScriptDefinition();

            var el_unit = el_units.Element("units");
            if (el_unit == null)
                throw ThrowInvalidScriptDefinition();


            FileInfo file = null;

            if (el_units == null)
                code = String.Empty;
            else
                code = Deserialize(el_unit.Value ?? String.Empty);

            var attr_file = el_unit.Attribute("filename");

            string fileExtension = ScriptCompiler.CreateProvider(this).FileExtension;


            string directory = ScriptUtils.GetDirectory(this);
            string fileName = Path.Combine(directory, SourceFileName);
            fileName = Path.ChangeExtension(fileName, fileExtension);
            file = new FileInfo(fileName);
            if (!file.Exists)
            {
                file = new FileInfo(fileName);
            }

            this.sourceFile = file.FullName;

            var attr_modified = el_unit.Attribute("modified");

            if (attr_modified != null && !String.IsNullOrWhiteSpace(attr_modified.Value))
            {
                DateTime d;
                if (DateTime.TryParse(attr_modified.Value, out d))
                    modified = d;
            }

            if (!modified.HasValue || !file.Exists || (file.LastWriteTime != modified.Value))
            {
                ScriptUtils.SaveToFile(file, el_unit.Value ?? String.Empty, modified);
            }
        }

        private static Exception ThrowInvalidScriptDefinition()
        {
            return new InvalidScriptDefinitionException(Session.CurrentSession.Resources.GetMessage("ADVSCRDTRLTDT", "Cannot be an advanced security script does not have the relevant data.", "").Text);
        }

        public override IScriptBuilder CreateDefaultBuilder()
        {
            return new AdvancedScriptBuilder();
        }

        #endregion
    }
}
