using System;

namespace FWBS.OMS.Utils.Commands
{
    using System.Windows.Forms;
    using Fwbs.WinFinder;

    public abstract class RunCommand
    {
        private string param;
        public string Param
        {
            get
            {
                return this.param ?? String.Empty;
            }
            set
            {
                this.param = value ?? String.Empty;
            }
        }

        private string param2;
        public string Param2
        {
            get
            {
                return this.param2 ?? String.Empty;
            }
            set
            {
                this.param2 = value ?? String.Empty;
            }
        }

        public abstract string Name { get;}


        public virtual bool RequiresLogin
        {
            get
            {
                return true;
            }
        }


        public override string ToString()
        {
            return Name ?? String.Empty;
        }

        public abstract void Execute(MainWindow main);

        protected void OpenFile(string file, string verb)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(file);
            psi.Verb = verb;
            System.Diagnostics.Process.Start(psi);
        }

        private Window parent;
        public Window Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public virtual void OnBusy(NotifyIcon notification)
        {
        }

    }

   
}
