using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace PreviewTest
{
    public class cInfo : Fwbs.Framework.Licensing.API.IConsumerInfo
    {

        public long? CompanyID
        {
            get { throw new NotImplementedException(); }
        }

        public string CompanyName
        {
            get { throw new NotImplementedException(); }
        }

        public Fwbs.Framework.Priority DefaultPriority
        {
            get { throw new NotImplementedException(); }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime? Expires
        {
            get { throw new NotImplementedException(); }
        }

        public Guid? ID
        {
            get { throw new NotImplementedException(); }
        }

        public Fwbs.Framework.Priority MaximumPriority
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string PublicKeyToken
        {
            get { throw new NotImplementedException(); }
        }

        public Fwbs.Framework.Licensing.API.ConsumerState State
        {
            get { throw new NotImplementedException(); }
        }

        public Fwbs.Framework.Licensing.API.ConsumerTarget Target
        {
            get { throw new NotImplementedException(); }
        }

        public Fwbs.Framework.Licensing.API.ConsumerType Type
        {
            get { throw new NotImplementedException(); }
        }

        public Fwbs.Framework.Licensing.API.UsageMode UsageMode
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime? ValidFrom
        {
            get { throw new NotImplementedException(); }
        }
    }

    [Export(typeof(Fwbs.Framework.Licensing.ILicensingManager))]
    public class LicMan : Fwbs.Framework.Licensing.ILicensingManager
    {

        public Fwbs.Framework.Licensing.API.IConsumerInfo GetConsumer(System.Reflection.Assembly assembly)
        {
            return new cInfo();
        }

        public IEnumerable<Fwbs.Framework.Licensing.API.IConsumerInfo> GetConsumers()
        {
            throw new NotImplementedException();
        }

        public bool IsAllowed(System.Reflection.Assembly assembly, out Fwbs.Framework.Licensing.API.IConsumerInfo consumer)
        {
            consumer = new cInfo();

            return true;
        }

        public bool IsAllowed(System.Reflection.Assembly assembly)
        {
            return true;
        }

        public Fwbs.Framework.Licensing.API.IConsumerInfo Validate(System.Reflection.Assembly assembly)
        {
            return new cInfo();
        }
    }
}
