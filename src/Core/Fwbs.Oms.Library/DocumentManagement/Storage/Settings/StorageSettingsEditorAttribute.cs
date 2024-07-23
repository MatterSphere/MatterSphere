using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public sealed class StorageSettingsEditorAttribute : Attribute
    {
        private Type type;
        private SettingsType settingsType;

        public StorageSettingsEditorAttribute(string typeName, SettingsType settingsType)
        {
            if (String.IsNullOrEmpty(typeName))
                throw new ArgumentNullException("typeName");

            type = Session.CurrentSession.TypeManager.Load(typeName);
            this.settingsType = settingsType;
        }

        public StorageSettingsEditorAttribute(Type type, SettingsType settingsType)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.type = type;
            this.settingsType = settingsType;
        }

        public Type Type
        {
            get
            {
                return type;
            }
        }

        public SettingsType SettingsType
        {
            get
            {
                return settingsType;
            }
        }

    }
}
