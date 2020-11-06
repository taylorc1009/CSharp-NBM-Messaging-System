
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
    public class MessagesFacade
    {

        public MessagesFacade()
        {
        }

        private Dictionary<String, SMS> sms;

        private Dictionary<String, StandardEmailMessage> SEMEmails;

        private Dictionary<String, SignificantIncidentReport> SIREmails;

        private Dictionary<String, Tweet> tweets;


        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public void addSMS(String sender, String text)
        {
            // TODO implement here
        }

        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public void addEmail(String sender, String text)
        {
            // TODO implement here
        }

        /// <summary>
        /// @param sender 
        /// @param text 
        /// @return
        /// </summary>
        public void addTweet(String sender, String text)
        {
            // TODO implement here
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