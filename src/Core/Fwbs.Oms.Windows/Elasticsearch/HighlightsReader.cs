using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class HighlightsReader
    {
        private const string _startMarker = "<highlight>";
        private const string _endMarker = "</highlight>";
        private const string _pattern = "<highlight(.*?)>(.*?)</highlight>";
        
        public ReadInfo Read(string input)
        {
            var matchResult = Regex.Matches(input, _pattern, RegexOptions.IgnoreCase);
            var output = input.Replace(_startMarker, "").Replace(_endMarker, "");
            var info = new ReadInfo(output);
            for (int i = 0; i < matchResult.Count; i++)
            {
                info.AddGroup(
                    matchResult[i].Index - (_startMarker.Length + _endMarker.Length) * i,
                    matchResult[i].Value.Replace(_startMarker, "").Replace(_endMarker, ""));
            }

            return info;
        }

        public class ReadInfo
        {
            public ReadInfo(string output)
            {
                Output = output;
                Groups = new List<Group>();
            }

            public string Output { get; set; }
            public List<Group> Groups { get; set; }

            public void AddGroup(int index, string phrase)
            {
                Groups.Add(new Group(index, phrase, Groups.Count));
            }

            public class Group
            {
                public Group(int index, string phrase, int number)
                {
                    Index = index;
                    Phrase = phrase;
                    Number = number;
                }

                public int Index { get; private set; }
                public string Phrase { get; private set; }
                public int Number { get; private set; }
            }
        }
    }
}
