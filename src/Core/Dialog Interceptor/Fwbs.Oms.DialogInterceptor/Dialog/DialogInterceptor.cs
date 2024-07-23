namespace Fwbs.Oms.DialogInterceptor
{
    public static class Interceptor
    {
        #region Fields

        private static HiddenWindow hiddenwin;
        private static bool hooked;

        #endregion

        #region Events

        private static event DialogCapturedEventHandler dialogcaptured;
       

        public static event DialogCapturedEventHandler DialogCaptured
        {
            add
            {
                dialogcaptured += value;
                EventHook();    
            }
            remove
            {
                dialogcaptured -= value;
                EventUnHook();
            }
        }


        internal static void OnDialogCaptured(DialogCapturedEventArgs e)
        {
            DialogCapturedEventHandler ev = dialogcaptured;
            if (ev != null)
                dialogcaptured(e.Dialog, e);
        }

   

        #endregion

        private static void EventHook()
        {
            if (!hooked && 
                (
                dialogcaptured != null))
                Hook();
        }

        private static void EventUnHook()
        {
            if (hooked &&
                (
                dialogcaptured == null))
                UnHook();
        }

        private static void Hook()
        {
            DialogFactory.BuildDialogConfigurations();

            if (DialogFactory.IsDisabled)
                throw new HookException(Properties.Resources.ExceptionHookIsDisabled);

            if (hiddenwin == null)
            {
                hiddenwin = new HiddenWindow();
                try
                {
                    hiddenwin.Hook();
                    if (!DialogFactory.IsDisabledSecondary)
                        hiddenwin.SecondaryHook();
                }
                catch (HookException)
                {
                    UnHook();
                    throw;
                }
            }

            hooked = true;
        }

        private static void UnHook()
        {
            hooked = false;

            if (hiddenwin != null)
            {
                hiddenwin.Close();
                hiddenwin.Dispose();
                hiddenwin = null;
            }

            DialogFactory.ClearDialogConfiguration();
        }

        public static bool IsHooked
        {
            get
            {
                return hooked;
            }
        }
    }
}
