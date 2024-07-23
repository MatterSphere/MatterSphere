using System.Windows.Forms;

namespace FWBS.OMS.UI
{
    internal class PrecedentBuilder
    {
        private Form _owner;
        private FWBS.OMS.Associate _associate;

        public PrecedentBuilder(Form owner, FWBS.OMS.Associate associate = null)
        {
            _owner = owner;
            _associate = associate;
        }

        public void Build(long precId)
        {
            Precedent precedent = FWBS.OMS.Precedent.GetPrecedent(precId);
            if (precedent.IsMultiPrecedent)
            {
                OMSFile of = FWBS.OMS.UI.Windows.Services.SelectFile();
                if (of == null)
                {
                    return;
                }

                _associate = of.DefaultAssociate;
                if (_associate == null)
                {
                    return;
                }

                precedent.GenerateJobList(_associate.OMSFile);
            }
            else
            {
                //Only ask for the associate if it is a standard precedent.
                if (_associate == null)
                {
                    _associate = FWBS.OMS.UI.Windows.Services.SelectAssociate(_owner);
                    if (_associate == null)
                    {
                        return;
                    }
                }

                PrecedentJob pj = new PrecedentJob(precedent)
                {
                    AsNewTemplate = false,
                    Associate = _associate
                };

                if (pj.Associate == null)
                {
                    return;
                }

                FWBS.OMS.Session.CurrentSession.CurrentPrecedentJobList.Add(pj);
            }
        }
    }
}
