
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Message
    {

        public Message()
        {
        }

        public String sender { get; set; }

        public String text { get; set; }

        public char header { get; set; }
        public DateTime sentAt { get; set; }

        protected Tuple<String, int, int> trimNonAlphabeticals(String str)
        {
            //this is used to work around non-alphabetical chars, for rexmple if we had a URL in the form ",(http://example.com)."
            int s = 0, e = str.Length;
            while (!Regex.IsMatch(str[s].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && s < str.Length)
                s++;
            while (!Regex.IsMatch(str[e - 1].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && e > 0)
                e--;

            return Tuple.Create(str.Substring(s, e - s), s, e);
        }

        /// <summary>
        /// @return
        /// </summary>
        public bool validate()
        {
            // TODO implement here
            return false;
        }

    }
}