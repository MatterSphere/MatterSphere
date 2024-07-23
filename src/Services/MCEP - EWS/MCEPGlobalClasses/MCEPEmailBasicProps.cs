using System;

namespace MCEPGlobalClasses
{
    class MCEPEmailBasicProps
    {
        public string Subject { get; set; }
        public DateTime EmailSent { get; set; }
        public DateTime EmailRecieved { get; set; }
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string EmailCC { get; set; }
        public DateTime EmailCreated { get; set; }
    }
}
