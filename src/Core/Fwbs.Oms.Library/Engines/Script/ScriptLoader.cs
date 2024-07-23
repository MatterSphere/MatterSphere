using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FWBS.OMS.Script
{
    internal sealed class ScriptLoader : IScriptLoader
    {
        #region Fields

        private static readonly Dictionary<string, Type> LoadedScriptlets = new Dictionary<string, Type>();

        private readonly IScriptDefinition definition;
        private readonly IScriptCompiler compiler;

        #endregion

        #region Constructors

        public ScriptLoader(IScriptDefinition definition, IScriptCompiler compiler)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            if (compiler == null)
                throw new ArgumentNullException("compiler");

            this.definition = definition;
            this.compiler = compiler;
        }

        #endregion

        #region Methods

        private IScriptType CreateType(Type t)
        {
            return (IScriptType)Activator.CreateInstance(t);
        }

        #endregion

        #region IScriptLoader

        public IScriptType Load(LoadOptions options)
        {
            try
            {
                options = options ?? new LoadOptions();

                options.Compile = options.Compile == LoadCompileOption.Default ? LoadCompileOption.OnError : options.Compile;

                string name = String.Format("{0}.{1}", definition.Namespace, definition.Name);

                string location = ScriptUtils.GetOutputName(definition);

    
                switch (options.Compile)
                {
                    case LoadCompileOption.Always:
                        {
                            return LoadWithCompileAlways(options, name, location);
                        }
                    case LoadCompileOption.Never:
                        {
                            return LoadWithCompileNever(options, name, location);
                        }
                    case LoadCompileOption.OnError:
                        {
                            return LoadWithCompileOnError(options, name, location);
                        }
                    default:
                        throw new NotSupportedException();
                }
            }
            catch (InvalidScriptDefinitionException)
            {
                throw;
            }
            catch
            {
                if (options.ThrowException)
                    throw;

                return null;
            }
        }

        private IScriptType LoadWithCompileNever(LoadOptions options, string name, string location)
        {
            Assembly ass = null;
            Type type = null;

            if (!File.Exists(location))
            {
                if (options.ThrowException)
                    throw new FileNotFoundException(Session.CurrentSession.Resources.GetMessage("SCRASCNTBEFND", "Script Assembly cannot be found.", "").Text, location);
                else
                    return null;
            }
            else
            {
                GetTypeFromCache(name, location, out type, out ass);

            }

            return LoadType(ass, type, name, location);
        }

        private IScriptType LoadWithCompileAlways(LoadOptions options, string name, string location)
        {
            Assembly ass = null;
            
            Type type = null;

            var result = compiler.Compile(new CompileOptions(){Force = true});

            location = result.PathToAssembly;

            return LoadType(ass, type, name, location);
        }

        private IScriptType LoadWithCompileOnError(LoadOptions options, string name, string location)
        {
            Assembly ass = null;

            Type type = null;

            try
            {
                if (!File.Exists(location))
                {
                    var result = compiler.Compile(null);

                    location = result.PathToAssembly;
                }
                else
                {
                    GetTypeFromCache(name, location, out type, out ass);
                }

                return LoadType(ass, type, name, location);
            }
            catch (InvalidScriptDefinitionException)
            {
                throw;
            }
            catch
            {
                if (options.Compile != LoadCompileOption.OnError)
                {
                    if (options.ThrowException)
                        throw;
                    else
                        return null;
                }

                try
                {
                    var result = compiler.Compile(new CompileOptions() { Force = true });

                    location = result.PathToAssembly;

                    ass = Session.CurrentSession.AssemblyManager.LoadFrom(location);

                    type = ass.GetType(name, true, true);

                    if (LoadedScriptlets.Contains(name))
                        LoadedScriptlets.Remove(name);

                    LoadedScriptlets.Add(name, type);

                    return CreateType(type);
                }
                catch (Exception subex)
                {
                    if (options.ThrowException)
                        throw new TypeLoadException(Session.CurrentSession.Resources.GetMessage("NBLTLDSCRERCRCS", "Unable to Load Script Error creating Class ''%1%''", "", name).Text, subex);
                    else
                        return null;
                }
            }
        }

        private void GetTypeFromCache(string name, string location, out Type type, out Assembly ass)
        {
            type = null;
            ass = null;

            if (LoadedScriptlets.TryGetValue(name, out type))
            {
                if (type.Assembly.Location.Equals(location, StringComparison.OrdinalIgnoreCase))
                {
                    ass = Assembly.GetAssembly(type);
                }
                else
                {
                    ass = null;
                    type = null;
                }
            }
        }

        private IScriptType LoadType(Assembly ass, Type type, string name, string location)
        {
            if (ass == null) // Removed ev param for .NET4 support
                ass = Session.CurrentSession.AssemblyManager.LoadFrom(location);

            if (type == null)
                type = ass.GetType(name, true);

            LoadedScriptlets[name] = type;

            return CreateType(type);
        }

        #endregion
    }

}
