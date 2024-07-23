using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FWBS.OMS.Licensing
{
    using System.Diagnostics;
    using Fwbs.Framework;
    using Fwbs.Framework.Licensing.API;

    internal sealed class APILicensingProvider : IConsumerProvider
    {
        #region Fields

        private readonly ConsumerProvider apicp = new ConsumerProvider();
        private readonly Dictionary<Assembly, IConsumerInfo> cachedassemblies = new Dictionary<Assembly, IConsumerInfo>();
        private readonly List<Assembly> disqualifiedassemblies = new List<Assembly>();
        private readonly ConsumerProvider defaultProvider;
        private readonly List<IConsumerInfo> cache = new List<IConsumerInfo>();
        private bool requiresloading = true;

        #endregion

        #region Contructors

        public APILicensingProvider(ConsumerProvider defaultProvider)
        {
            if (defaultProvider == null)
                throw new ArgumentNullException("defaultProvider");

            this.defaultProvider = defaultProvider;
        }

        #endregion

        #region IConsumerProvider

        public bool IsAllowed(Assembly assembly)
        {
            IConsumerInfo consumer;
            return IsAllowed(assembly, out consumer);
        }

        public bool IsAllowed(Assembly assembly, out IConsumerInfo consumer)
        {

            if (assembly == null)
                throw new ArgumentNullException("assembly");

            consumer = null;

            if (disqualifiedassemblies.Contains(assembly))
                return false;

            consumer = GetConsumer(assembly);

            if (IsScriptAssembly(assembly))
            {
                return true;
            }


            if (String.IsNullOrWhiteSpace(consumer.PublicKeyToken))
            {
                if (FWBS.OMS.Session.CurrentSession.OnlyAllowStrongNamedAssemblies)
                {
                    DisableAssembly(assembly, consumer);
                    return false;
                }
            }

            if (ConsumerProvider.IsExpired(consumer))
            {
                DisableAssembly(assembly, consumer);
                return false;
            }

            if (consumer.State == ConsumerState.UnRegistered)
            {
                if (FWBS.OMS.Session.CurrentSession.OnlyAllowRegisteredAssemblies)
                {
                    DisableAssembly(assembly, consumer);
                    return false;
                }

                return true;
            }

            if (!consumer.State.HasFlag(ConsumerState.Registered))
            {
                if (!IsTrustedCompany(consumer) && FWBS.OMS.Session.CurrentSession.OnlyAllowRegisteredAssemblies)
                {
                    DisableAssembly(assembly, consumer);
                    return false;
                }
            }

            if (!consumer.State.HasFlag(ConsumerState.Enabled))
            {
                DisableAssembly(assembly, consumer);
                return false;
            }

       
            return true;
        }

        private bool IsTrustedCompany(IConsumerInfo consumer)
        {
            return apicp.InstalledCompanies.Count(c => String.Equals(c.PublicKeyToken, consumer.PublicKeyToken)) > 0;
        }

        private void DisableAssembly(Assembly assembly, IConsumerInfo consumer)
        {
            disqualifiedassemblies.Add(assembly);

            Trace.WriteLine(String.Format("Assembly '{0}' is not licensed to use the API and therefore will not be loaded.  The assembly is owned by '{1}'", assembly, consumer.CompanyName));
        }

        public IConsumerInfo Validate(Assembly assembly)
        {
            IConsumerInfo consumer;

            if (assembly == null)
                assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
                assembly = Assembly.GetCallingAssembly();

            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            if (!IsAllowed(assembly, out consumer))
                throw new InvalidOperationException(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("APNMNTLSDTACS", "The application / assembly with the name ''%1%'' is not licensed to access the %2% API.", "", assembly.FullName, FWBS.OMS.Global.ApplicationName).Text);

            return consumer;
        }

        public IConsumerInfo GetConsumer(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            IConsumerInfo consumer;

            if (cachedassemblies.TryGetValue(assembly, out consumer))
                return consumer;

            CompanyInfo company = new CompanyInfo(assembly);

            defaultProvider.ApplyPolicy(ref company);

            consumer = new ConsumerInfo(company, assembly);

            defaultProvider.ApplyPolicy(ref consumer);

            var apiattr = assembly.Attribute<AssemblyAPIClientAttribute>();

            foreach (var item in GetConsumers())
            {
                if (String.Compare(item.Name, consumer.Name, true) == 0 && (item.ID == consumer.ID || (apiattr != null && apiattr.ApplicationKey == item.ID)))
                {
                    IConsumerInfo info;
                    if (cachedassemblies.TryGetValue(assembly, out info))
                        return info;

                    cachedassemblies.Add(assembly, item);
                    return item;
                }
            }

            consumer = defaultProvider.GetConsumer(assembly);

            IConsumerInfo inf;
            if (cachedassemblies.TryGetValue(assembly, out inf))
                return inf;

            cachedassemblies.Add(assembly, consumer);
            return consumer;
        }

        public IEnumerable<IConsumerInfo> GetConsumers()
        {
            if (!requiresloading)
                return cache;

            cache.Clear();
            cache.AddRange(defaultProvider.GetConsumers());

            if (FWBS.OMS.Session.CurrentSession.IsProcedureInstalled("GetAPIConsumers"))
            {
                using (var dt = FWBS.OMS.Session.CurrentSession.Connection.ExecuteProcedureTable("GetAPIConsumers", "CONSUMERS", null))
                {
                    foreach (System.Data.DataRow row in dt.Rows)
                    {

                        var consumerinfo = new ConsumerInfo(
                            ConvertDef.To<Guid?>(row["ID"]),
                            ConvertDef.To<string>(row["Name"]),
                            ConvertDef.To<string>(row["Description"]),
                            ConvertDef.To<long?>(row["CompanyID"]),
                            ConvertDef.To<string>(row["CompanyName"]),
                            ConvertDef.To<string>(row["PublicKeyToken"]),
                            UsageMode.Designtime | UsageMode.Runtime,
                            ConsumerState.Registered | (ConvertDef.To<bool>(row["Enabled"]) ? ConsumerState.Enabled : ConsumerState.Registered),
                            ConvertDef.To<Priority>(row["DefaultPriority"]),
                            ConvertDef.To<Priority>(row["MaximumPriority"]),
                            ConvertDef.To<DateTime?>(row["ValidFrom"]),
                            ConvertDef.To<DateTime?>(row["Expires"]),
                            ConvertDef.To<ConsumerType>(row["ConsumerType"]),
                            ConvertDef.To<ConsumerTarget>(row["ConsumerTarget"])
                            );

                        cache.Add(consumerinfo);

                    }
                }
            }

            requiresloading = false;

            return cache;
        }


        #endregion

        #region Methods

        private static bool IsScriptAssembly(Assembly assembly)
        {
            return assembly.Attribute<FWBS.OMS.Script.ScriptGenAssemblyAttribute>() != null;
        }

        #endregion

    }
}
