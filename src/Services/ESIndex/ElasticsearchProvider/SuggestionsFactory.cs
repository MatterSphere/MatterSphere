using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Models.Interfaces;

namespace ElasticsearchProvider
{
    public class SuggestionsFactory : ISuggestionsFactory
    {
        public string[] CreateSuggestions(string value)
        {
            if (value == null)
            {
                return new string[0];
            }

            value = Regex.Replace(value, "[^\\w\\s]", "").Trim();
            var spacesRegex = new Regex("[ ]{2,}", RegexOptions.None);
            value = spacesRegex.Replace(value, " ");
            var words = value.Split(' ');
            var phrases = new List<string>();
            for (int i = 0; i < words.Length; i++)
            {
                StringBuilder phrase = new StringBuilder(words[i]);
                for (int j = i + 1; j < words.Length; j++)
                {
                    phrase.Append(' ');
                    phrase.Append(words[j]);
                    phrases.Add(phrase.ToString());
                }
            }

            return words.Concat(phrases).Distinct().ToArray();
        }
    }
}
