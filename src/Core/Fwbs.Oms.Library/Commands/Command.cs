namespace FWBS.OMS.Commands
{

    public abstract class Command : ICommand
    {

        #region ICommand Members

        public bool AllowUI { get; set; }

        public bool ContinueOnError { get; set; }

        public abstract ExecuteResult Execute();


        #endregion
    }
}
