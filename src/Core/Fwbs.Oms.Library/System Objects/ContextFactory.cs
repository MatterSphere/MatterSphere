using FWBS.OMS.DocumentManagement;

namespace FWBS.OMS
{
    public class ContextFactory
    {
        public ContextFactory()
        {

        }


        public Context CreateContext(object obj, object parent = null)
        {
            var ctx = Create(obj);
            if (parent != null)
                ctx.Parent = Create(parent);
            return ctx;
		}


        private static Context Create(object obj)
        {
            var _context = new Context();

            _context.Session = Session.CurrentSession;
            _context.User = _context.Session.CurrentUser;
            _context.FeeEarner = _context.Session.CurrentFeeEarner;
            _context.NumberFormat = _context.Session.DefaultCurrencyFormat;
            _context.DateTimeFormat = _context.Session.DefaultDateTimeFormat;

            _context.Associate = null;
            _context.Phase = null;
            _context.File = null;
            _context.Client = null;
            _context.Contact = null;
            _context.Document = null;
            _context.DocumentVersion = null;

            if (obj is Contact
                || obj is Client
                || obj is OMSFile
                || obj is Associate
                || obj is OMSDocument
                || obj is User
                || obj is FeeEarner
                || obj is DocumentVersion 
                || obj is PrecedentJob
                )
            {
                _context.Data = obj;
            }
            else
            {
                if (obj is Interfaces.IParent)
                    _context.Data = ((Interfaces.IParent)obj).Parent;
                else
                    _context.Data = obj;
            }

            if (_context.Data != null)
            {
                BuildContextForMainBusinessObjects(_context);
            }
            
            _context.Data = obj;

            return _context;
        }


        private static void BuildContextForMainBusinessObjects(Context _context)
        {
            if (_context.Data is Contact)
            {
                BuildContactBasedContext(_context);
            }
            else if (_context.Data is Client)
            {
                BuildClientBasedContext(_context);
            }
            else if (_context.Data is OMSFile)
            {
                BuildFileBasedContext(_context);
            }
            else if (_context.Data is Associate)
            {
                BuildAssociateBasedContext(_context, (Associate)_context.Data);
            }
            else if (_context.Data is FeeEarner)
            {
                BuildFeeEarnerBasedContext(_context);
            }
            else if (_context.Data is User)
            {
                BuildUserBasedContext(_context);
            }
            else if (_context.Data is OMSDocument)
            {
                BuildDocumentBasedContext(_context);
            }
            else if (_context.Data is FWBS.OMS.DocumentManagement.DocumentVersion)
            {
                BuildDocumentVersionBasedContext(_context);
            }
            else if (_context.Data is PrecedentJob)
            {
                BuildPrecedentJobBasedContext(_context);
            }
        }


        private static void BuildContactBasedContext(Context _context)
        {
            _context.Contact = (Contact)_context.Data;
        }


        private static void BuildClientBasedContext(Context _context)
        {
            _context.Client = (Client)_context.Data;
            _context.Contact = _context.Client.DefaultContact;
        }


        private static void BuildFileBasedContext(Context _context)
        {
            _context.File = (OMSFile)_context.Data;
            _context.Phase = _context.File.CurrentPhase;
            _context.Client = _context.File.Client;
            _context.Contact = _context.Client.DefaultContact;
        }


        private static void BuildFeeEarnerBasedContext(Context _context)
        {
            _context.FeeEarner = (FeeEarner)_context.Data;
        }


        private static void BuildUserBasedContext(Context _context)
        {
            _context.User = (User)_context.Data;
        }


        private static void BuildDocumentBasedContext(Context _context)
        {
            _context.Document = (OMSDocument)_context.Data;
            _context.Precedent = _context.Document.LastPrecedent ?? _context.Document.BasePrecedent;
            _context.Associate = _context.Document.Associate;
            _context.File = _context.Associate.OMSFile;
            _context.Phase = _context.File.CurrentPhase;
            _context.Client = _context.File.Client;
            _context.Contact = _context.Client.DefaultContact;
        }


        private static void BuildDocumentVersionBasedContext(Context _context)
        {
            _context.DocumentVersion = (FWBS.OMS.DocumentManagement.DocumentVersion)_context.Data;
            _context.Document = _context.DocumentVersion.ParentDocument;
            _context.Precedent = _context.Document.LastPrecedent ?? _context.Document.BasePrecedent;
            _context.Associate = _context.Document.Associate;
            _context.File = _context.Associate.OMSFile;
            _context.Phase = _context.File.CurrentPhase;
            _context.Client = _context.File.Client;
            _context.Contact = _context.Client.DefaultContact;
        }


        private static void BuildPrecedentJobBasedContext(Context _context)
        {
            var contextData = (PrecedentJob)_context.Data;

            if (contextData.Precedent != null)
            {
                _context.Precedent = contextData.Precedent;
            }

            if (contextData.Associate != null)
            {
                BuildAssociateBasedContext(_context, contextData.Associate);
            }
        }


        private static void BuildAssociateBasedContext(Context _context, Associate associate)
        {
            _context.Associate = associate;
            _context.File = _context.Associate.OMSFile;
            if (_context.File != null)
            {
                _context.Phase = _context.File.CurrentPhase;
                _context.Client = _context.File.Client;
                _context.Contact = _context.Client.DefaultContact;
            }
        }
    }
}
