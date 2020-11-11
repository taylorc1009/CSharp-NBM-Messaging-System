
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BusinessLayer
{
    public class MessagesFacade
    {

        public MessagesFacade()
        {
        }

        private Dictionary<String, SMS> sms = new Dictionary<String, SMS>();

        private Dictionary<String, StandardEmailMessage> SEMEmails = new Dictionary<String, StandardEmailMessage>();

        private Dictionary<String, SignificantIncidentReport> SIREmails = new Dictionary<String, SignificantIncidentReport>();

        private Dictionary<String, Tweet> tweets = new Dictionary<String, Tweet>();

        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public void addSMS(String sender, String text)
        {
            SMS message = new SMS(sender, text, 'S');

            StringBuilder id = new StringBuilder("S000000000");
            String count = sms.Count().ToString();

            if (count.Length < 10)
                for (int i = 0; i < count.Length; i++)
                    id[id.Length - count.Length + i] = count[i];

            sms.Add(id.ToString(), message);
        }

        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public void addSEM(String sender, String subject, String text)
        {
            StandardEmailMessage message = new StandardEmailMessage(sender, subject, text, 'E');

            message.quarantineURLs();

            StringBuilder id = new StringBuilder("E000000000");
            String count = (SEMEmails.Count() + SIREmails.Count()).ToString();

            if (count.Length < 10)
                for (int i = 0; i < count.Length; i++)
                    id[id.Length - count.Length + i] = count[i];

            SEMEmails.Add(id.ToString(), message);
        }

        public void addSIR(String sender, DateTime date, String sortCode, String nature, String text)
        {
            /*String[] toks = date.Split('/');
            String dateShort = toks[0] + '/' + toks[1] + '/' + toks[2].Substring(2);*/

            SignificantIncidentReport message = new SignificantIncidentReport(sender, date, sortCode, nature, text, 'E');

            message.quarantineURLs();

            StringBuilder id = new StringBuilder("E000000000");
            String count = (SIREmails.Count() + SEMEmails.Count()).ToString();

            if (count.Length < 10)
                for (int i = 0; i < count.Length; i++)
                    id[id.Length - count.Length + i] = count[i];

            SIREmails.Add(id.ToString(), message);
        }

        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public void addTweet(String sender, String text)
        {
            Tweet message = new Tweet(sender, text, 'T');

            StringBuilder id = new StringBuilder("T000000000");
            String count = tweets.Count().ToString();

            if (count.Length < 10)
                for (int i = 0; i < count.Length; i++)
                    id[id.Length - count.Length + i] = count[i];

            tweets.Add(id.ToString(), message);
        }

        /// <summary>
        /// @param file 
        /// @return
        /// </summary>
        public void importMessages(String file)
        {
            // TODO implement here
        }

        /// <summary>
        /// @return
        /// </summary>
        public void listTrending()
        {
            // TODO implement here
        }

        /// <summary>
        /// @return
        /// </summary>
        public void listMentions()
        {
            // TODO implement here
        }

        /// <summary>
        /// @return
        /// </summary>
        public void listSIRs()
        {
            // TODO implement here
        }

        /// <summary>
        /// @return
        /// </summary>
        private void outputMessages()
        {
            // TODO implement here
        }

    }
}