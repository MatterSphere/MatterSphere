namespace Models.Common
{
    public class Message
    {
        private static readonly byte[][] _contentablePrefixes = new byte[3][]
        {
            System.Text.Encoding.Unicode.GetBytes("\uFEFF<root objecttype=\"DOCUMENT\">"),
            System.Text.Encoding.Unicode.GetBytes("\uFEFF<root objecttype=\"PRECEDENT\">"),
            System.Text.Encoding.Unicode.GetBytes("\uFEFF<root objecttype=\"EMAIL\">")
        };

        public Message(byte[] data, string id)
        {
            ID = id;
            Data = data;
        }

        public string ID { get; private set; }

        public byte[] Data { get; private set; }

        public int RetryCount { get; set; }

        public bool IsContentable
        {
            get
            {
                for (int i = 0; i < 3; i++)
                {
                    if (MatchPrefix(_contentablePrefixes[i]))
                        return true;
                }

                return false;
            }
        }

        private bool MatchPrefix(byte[] prefix)
        {
            if (Data.Length < prefix.Length)
                return false;

            for (int i = 0; i < prefix.Length; i++)
            {
                if (Data[i] != prefix[i])
                    return false;
            }

            return true;
        }
    }
}
