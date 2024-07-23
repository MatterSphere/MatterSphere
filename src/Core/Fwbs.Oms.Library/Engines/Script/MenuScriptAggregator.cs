using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FWBS.OMS.Script
{
    public class MenuItem
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Tooltip { get; set; }
        public string Filter { get; set; }
        public string EnabledFilter { get; set; }
        public string Command { get; set; }

        public bool Visible { get; set; }
        public bool Enabled{get;set;}
    }

    public class MenuScriptAggregator : IDisposable
    {
        public MenuScriptAggregator() : this(null, null)
        {
        }

        public MenuScriptAggregator(object application, FWBS.OMS.Interfaces.IOMSApp omsApp)
        {
            Initialise(application, omsApp);
        }

        private List<MenuScriptType> menuScripts;

        public System.Collections.ObjectModel.ReadOnlyCollection<MenuScriptType> MenuScripts
        {
            get { return menuScripts.AsReadOnly(); }
        }

        private void Initialise(object application, FWBS.OMS.Interfaces.IOMSApp omsApp)
        {
            if (menuScripts != null)
                return;

            menuScripts = new List<MenuScriptType>();

            StringBuilder errors = new StringBuilder();

            List<ScriptGen> scripts = new List<ScriptGen>(ScriptGen.GetMenuScripts());

            foreach (ScriptGen script in scripts)
            {
                try
                {
                    script.Load();
                    MenuScriptType ms = script.Scriptlet as MenuScriptType;
                    if (ms != null)
                    {
                        ms.SetAppObject(application, omsApp);
                        menuScripts.Add(ms);
                    }
                }
                catch(Exception ex)
                {
                    errors.AppendLine(ex.Message);
                }
            }

            if (errors.Length > 0)
                throw new Exception(errors.ToString());

        }

        public void Dispose()
        {
            if (menuScripts == null)
                return;

            foreach (MenuScriptType menu in menuScripts)
            {
                if (menu != null)
                    menu.Dispose();
            }
            
            menuScripts.Clear();
            menuScripts = null;

        }

        public bool ParseCommand(object doc, ref string command)
        {
            if (menuScripts != null)
            {
                foreach (FWBS.OMS.Script.MenuScriptType ms in menuScripts)
                {
                    if (ms == null)
                        continue;

                    if (ms.ParseCommand(doc, ref command))
                        return true;
                }
            }

            return false;
        }

        public bool Validate(MenuItem item, object doc)
        {
            if (menuScripts != null)
            {
                foreach (FWBS.OMS.Script.MenuScriptType ms in menuScripts)
                {
                    if (ms == null)
                        continue;

                    if (ms.Validate(item, doc))
                        return true;
                }
            }

            return false;
        }

        [Obsolete("Please use the Constructor to specify the omsapp and use Invoke.")]
        public bool Execute(object application, FWBS.OMS.Interfaces.IOMSApp omsApp, string command)
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return false;

            if (menuScripts == null || menuScripts.Count <= 0)
                return false;

            foreach (MenuScriptType menuScript in menuScripts)
            {
                if (menuScript == null)
                    continue;

                menuScript.SetAppObject(application, omsApp);


                try
                {

                    MethodInfo info = menuScript.GetType().GetMethod(command, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (info != null)
                    {
                        info.Invoke(menuScript, null);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                    else
                        throw;
                }
            }

            return false;
        }

        public bool Execute(object doc, string command)
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return false;

            if (menuScripts == null || menuScripts.Count <= 0)
                return false;

            foreach (MenuScriptType menuScript in menuScripts)
            {
                if (menuScript == null)
                    continue;

                try
                {
                    if (menuScript.Execute(doc, command))
                        return true;

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                    else
                        throw;
                }


            }

            return false;
        }

        public bool Invoke(string command)
        {
            object returnValue;
            return Invoke(command, out returnValue);
        }

        public bool Invoke(string command, out object returnValue,  params object[] parameters)
        {
            returnValue = null;

            if (!Session.CurrentSession.IsLoggedIn)
                return false;

            if (menuScripts == null || menuScripts.Count <= 0)
                return false;

            Type[] types = null;
            if (parameters != null)
            {
                List<Type> paramtypes = new List<Type>();
                foreach(var param in parameters)
                {
                    paramtypes.Add(param.GetType());
                }

                types = paramtypes.ToArray();
            }

            foreach (MenuScriptType menuScript in menuScripts)
            {
                if (menuScript == null)
                    continue;

                try
                {
                    MethodInfo info = menuScript.GetType().GetMethod(command, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase, null, types, null);
                    if (info != null)
                    {
                        returnValue = info.Invoke(menuScript, parameters);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                    else
                        throw;
                }


            }


            return false;
        }

    }
}
