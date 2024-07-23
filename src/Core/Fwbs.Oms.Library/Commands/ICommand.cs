namespace FWBS.OMS.Commands
{
    public interface ICommand
    {
        bool AllowUI { get; set; }
        ExecuteResult Execute();
    }

}
