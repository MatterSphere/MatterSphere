using System;
using System.Collections.Generic;

namespace FWBS.OMS.Utils
{
    internal static class Globals
    {
        public static readonly Queue<Commands.RunCommand> Commands = new Queue<FWBS.OMS.Utils.Commands.RunCommand>();
        public static readonly List<string> ChangedDocuments = new List<string>();

        public static readonly Dictionary<string, DateTime> JustSavedDocuments = new Dictionary<string, DateTime>();
        public static bool IsBusy = false;
        public static bool Cancel = false;
        public static Commands.RunCommand ProcessingCommand = null;
        public static readonly object Synch = new object();

        public static void AddSavedDocument(string file)
        {
            lock (Globals.Synch)
            {
                file = file.ToUpperInvariant();
                //UTCFIX: DM - 30/11/06 - Should be okay, local time used.
                if (Globals.JustSavedDocuments.ContainsKey(file))
                    Globals.JustSavedDocuments.Remove(file);
                Globals.JustSavedDocuments.Add(file, DateTime.UtcNow);
            }
        }

        public static void RemoveSavedDocument(string file)
        {
            lock (Globals.Synch)
            {
                file = file.ToUpperInvariant();
                if (Globals.JustSavedDocuments.ContainsKey(file))
                    Globals.JustSavedDocuments.Remove(file);
            }
        }


        public static void RemoveChangedDocument(string file)
        {
            lock (Globals.Synch)
            {
                file = file.ToUpperInvariant();
                if (Globals.ChangedDocuments.Contains(file))
                    Globals.ChangedDocuments.Remove(file);
            }
        }

       

    }
}
