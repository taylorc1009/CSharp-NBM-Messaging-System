using System;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    //base message class, all message classes will inherit this
    public class Message : ValidateDecorator
    {
        //attributes are public for serialization purposes
        public String sender { get; set; }
        public String text { get; set; }
        public DateTime sentAt { get; set; } = DateTime.Now;

        protected Tuple<String, int, int> trimNonAlphabeticals(String str)
        {
            //this is used to work around non-alphabetical chars - for example, if we had a URL in the form ",(http://example.com).", we use this to extract only the URL from the string
            if (!str.Equals(""))
            {
                int s = 0, e = str.Length;

                //while the increment is less than the string length and not at the point of an alphabetical character
                while (s < str.Length && !Regex.IsMatch(str[s].ToString(), @"[a-z]", RegexOptions.IgnoreCase))
                    s++;

                //same as above, but in reverse
                while (e > 0 && !Regex.IsMatch(str[e - 1].ToString(), @"[a-z]", RegexOptions.IgnoreCase))
                    e--;

                //returns the extracted substring and the indexes of the text start and finish
                return Tuple.Create(str.Substring(s, e - s < 0 ? 0 : e - s), s, e);
            }
            return null;
        }

        protected void decorate(int type)
        {
            //'Message' inherits from 'ValidateDecorator', which contains a method that will be defined ("decorated"), based on the message type, to validate that message type with the validations it needs
            //this method determines which validation type to decorate the 'validate' method with
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