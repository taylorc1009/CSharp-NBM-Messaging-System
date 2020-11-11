using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Email : Message
    {

        public Email()
        {
        }

        private Dictionary<int, URL> urlsQuarantined;


        /// <summary>
        /// @return
        /// </summary>
        public String quarantineURLs()
        {
            String[] tokenized = this.getText().Split(' ');

            for (int i = 0; i < tokenized.Length; i++) {

                //this is used to work around non-alphabetical chars, for rexmple if we had a URL in the form ",(http://example.com)."
                int s = 0, e = tokenized[i].Length;
                while (!Regex.IsMatch(tokenized[i][s].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && s != tokenized[i].Length)
                    s++;
                while (!Regex.IsMatch(tokenized[i][e].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && e > 0)
                    e--;

                if (Regex.IsMatch(tokenized[i].Substring(s, e), @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$", RegexOptions.IgnoreCase))
                {
                    if (urlsQuarantined == null)
                        urlsQuarantined = new Dictionary<int, URL>();
                    urlsQuarantined.Add(urlsQuarantined.Count(), new URL(tokenized[i], false));
                    tokenized[i] = "<URL Quarantined>";
                }
            }

            StringBuilder s = new StringBuilder();
            foreach (String tok in tokenized)
                s.Append(tok + ' ');

            return s.ToString().Trim();
        }

    }
}