namespace FWBS.OMS.Script
{
    public interface IScriptLoader
    {
        IScriptType Load(LoadOptions options);
    }


    public class LoadOptions
    {
        public LoadOptions()
        {
            ThrowException = true;
        }

        public LoadCompileOption Compile{get;set;}

        public bool ThrowException{get;set;}
    }

    public enum LoadCompileOption
    {
        Default = 0,
        Always,
        OnError,
        Never,
    }
}
