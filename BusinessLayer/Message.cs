
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Message : ValidateDecorator
    {
        public String sender { get; set; }
        public String text { get; set; }
        public DateTime sentAt { get; set; }

        protected Tuple<String, int, int> trimNonAlphabeticals(String str)
        {
            //this is used to work around non-alphabetical chars, for rexmple if we had a URL in the form ",(http://example.com)."
            if (!str.Equals(""))
            {
                int s = 0, e = str.Length;
                while (!Regex.IsMatch(str[s].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && s < str.Length)
                    s++;
                while (!Regex.IsMatch(str[e - 1].ToString(), @"[a-z]", RegexOptions.IgnoreCase) && e > 0)
                    e--;

                return Tuple.Create(str.Substring(s, e - s), s, e);
            }
            return null;
        }

        protected void decorate(int type)
        {
            switch (type) {
                case 1:
                    setComponent(new SMSDecorator());
                    break;
                case 2:
                    setComponent(new TweetDecorator());
                    break;
                case 3:
                    setComponent(new SEMDecorator());
                    break;
                case 4:
                    setComponent(new SIRDecorator());
                    break;
            }
        }
    }
}