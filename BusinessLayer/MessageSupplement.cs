using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class MessageSupplement : Message
    {
        public void findAbbreviations(List<String> abbreviations)
        {
            String[] tokenized = this.text.Split(' ');

            /*for (int i = 0; i < tokenized.Length; i++)
            {
                int s = 0, e = tokenized[i].Length;
                while (!Regex.IsMatch(tokenized[i][s].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && s < tokenized[i].Length)
                    s++;
                while (!Regex.IsMatch(tokenized[i][e - 1].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && e > 0)
                    e--;

                if (Regex.IsMatch(tokenized[i].Substring(s, e - s), @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$", RegexOptions.IgnoreCase))
                {
                    StringBuilder merger = new StringBuilder();
                    merger.Append(tokenized[i].Substring(0, s) + "<URL Quarantined>" + tokenized[i].Substring(e));
                    tokenized[i] = merger.ToString();
                }
            }

            StringBuilder message = new StringBuilder();
            foreach (String tok in tokenized)
                message.Append(tok + ' ');

            this.text = message.ToString().Trim();*/
        }
    }
}
