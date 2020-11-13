using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class MessageSupplement : Message
    {
        public void findAbbreviations(Dictionary<String, String> abbreviations)
        {
            String[] tokenized = this.text.Split(' ');

            for (int i = 0; i < tokenized.Length; i++)
            {
                Tuple<String, int, int> trimmed = trimNonAlphabeticals(tokenized[i]);
                if (abbreviations.ContainsKey(trimmed.Item1.ToUpper()))
                {
                    StringBuilder merger = new StringBuilder();
                    merger.Append(tokenized[i].Substring(0, trimmed.Item2 + trimmed.Item1.Length) + " <" + abbreviations[trimmed.Item1.ToUpper()] + "> " + tokenized[i].Substring(trimmed.Item3));
                    tokenized[i] = merger.ToString();
                }
            }

            StringBuilder message = new StringBuilder();
            foreach (String tok in tokenized)
                message.Append(tok + ' ');

            this.text = message.ToString().Trim();
        }
    }
}
