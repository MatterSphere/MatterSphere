using System;

namespace FWBS.OMS.Script
{
    internal sealed class ScriptFileName
    {
        private ScriptFileName()
        {
        }

        internal ScriptFileName(ScriptGen gen)
        {
            if (gen == null)
                throw new ArgumentNullException("gen");
             
            this.Version = gen.Version;
            this.Name = gen.Code;
            this.Timestamp = DateTime.Now.ToFileTimeUtc();        
        }

        public ScriptFileName(IScriptDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            Version = definition.Version;
            Name = definition.Name;
            Timestamp = DateTime.Now.ToFileTimeUtc();
        }

        public long Version { get; private set; }
        public string Name { get; private set; }
        public long Timestamp { get; private set; }


        public static ScriptFileName Parse(string s)
        {
            ScriptFileName sfn;
            if (!TryParse(s, out sfn))
                throw new FormatException(String.Format("Cannot convert '{0}' into a ScriptFileName object.", s));
            return sfn;
        }

        public static bool TryParse(string s, out ScriptFileName sfn)
        {
            sfn = null;

            if (String.IsNullOrWhiteSpace(s))
                return false;

            var assparts = s.Split(',');

            if (assparts.Length < 1)
                return false;

            var vals = assparts[0].Split('.');

            if (!(vals.Length >= 3))
                return false;

            long ver;

            if (!long.TryParse(vals[0], out ver))
                return false;

            long ts;

            if (!long.TryParse(vals[2], out ts))
                return false;

            sfn = new ScriptFileName();

            sfn.Version = ver;
            sfn.Name = vals[1];
            sfn.Timestamp = ts;

            return true;
        }

        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}.dll", Version, Name, Timestamp);
        }
    }
}
