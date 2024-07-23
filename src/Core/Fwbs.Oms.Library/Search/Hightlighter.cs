using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FWBS.OMS.Search
{
    public class Highlighter
    {
        private const string HIGHLIGHT_TAGS = "<highlight>$1</highlight>";
        private readonly Regex _regex;

        public Highlighter(string[] words)
        {
            int count = words.Length;
            if (count > 0)
            {
                List<string> tokens = new List<string>(count);
                for (int i = 0; i < count; i++)
                {
                    tokens.Add(Regex.Escape(words[i]));
                }
                tokens.Sort(StringComparer.CurrentCultureIgnoreCase);
                tokens.Reverse();

                _regex = new Regex(@"\b(" + string.Join("|", tokens) + @")", RegexOptions.IgnoreCase);
            }
        }

        public string SetHighlights(object source)
        {
            return SetHighlights(Convert.ToString(source));
        }

        public string SetHighlights(string text)
        {
            if (_regex != null && !string.IsNullOrWhiteSpace(text))
            {
                text = _regex.Replace(text, HIGHLIGHT_TAGS);
            }
            return text;
        }
    }
}
