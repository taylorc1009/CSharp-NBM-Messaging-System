/* TODO List
 * 
 * Implement:
 *      decorator
 *      JSON serialization
 *      import messages
 *      export message (?)
 *      message filter (?)
 *      credentials/log-in system (?)
 *      make list items width match parent (?)
 *      change list item type text to bitmap and collapse in in lists other than 'fullList' (?)
 * 
 * Testing:
 *      Unit Tests
 *      Compare with Use Cases
 *      
 * Other:
 *      Finish Class Diagram
 *      Double check Use Case Diagram
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BusinessLayer
{
    public class MessagesFacade
    {
        public MessagesFacade()
        {
            importAbbreviations();
        }

        private Dictionary<String, SMS> sms = new Dictionary<String, SMS>();
        private Dictionary<String, StandardEmailMessage> SEMEmails = new Dictionary<String, StandardEmailMessage>();
        private Dictionary<String, SignificantIncidentReport> SIREmails = new Dictionary<String, SignificantIncidentReport>();
        private Dictionary<String, Tweet> tweets = new Dictionary<String, Tweet>();
        private Dictionary<String, String> abbreviations;
        private Dictionary<String, int> trending;

        private void importAbbreviations()
        {
            using (var reader = new StreamReader(Directory.GetCurrentDirectory() + "\\textwords.csv"))
            {
                abbreviations = new Dictionary<String, String>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    abbreviations.Add(values[0], values[1]);
                }
            }
        }

        private String generateID(char header, int count)
        {
            StringBuilder id = new StringBuilder(header + "000000000");
            String countStr = count.ToString();

            if (countStr.Length < 10)
                for (int i = 0; i < countStr.Length; i++)
                    id[id.Length - countStr.Length + i] = countStr[i];

            return id.ToString();
        }

        public KeyValuePair<String, SMS> addSMS(String sender, String text)
        {
            SMS message = new SMS(sender, text.Trim());

            message.findAbbreviations(abbreviations);

            String id = generateID('S', sms.Count());
            sms.Add(id, message);
            return new KeyValuePair<String, SMS>(id, sms[id]);
        }

        public KeyValuePair<String, StandardEmailMessage> addSEM(String sender, String subject, String text)
        {
            StandardEmailMessage message = new StandardEmailMessage(sender, subject.Trim(), text.Trim());
            
            String id = generateID('E', SEMEmails.Count() + SIREmails.Count());
            SEMEmails.Add(id, message);
            return new KeyValuePair<String, StandardEmailMessage>(id, SEMEmails[id]);
        }

        public KeyValuePair<String, SignificantIncidentReport> addSIR(String sender, DateTime date, String sortCode, String nature, String text)
        {
            SignificantIncidentReport message = new SignificantIncidentReport(sender, date, sortCode, nature, text.Trim());

            String id = generateID('E', SEMEmails.Count() + SIREmails.Count());
            SIREmails.Add(id, message);
            return new KeyValuePair<String, SignificantIncidentReport>(id, SIREmails[id]);
        }

        private void analyseTweet(Tweet message)
        {
            message.findAbbreviations(abbreviations);
            if (message.text.Contains('#'))
            {
                if (trending == null)
                    trending = new Dictionary<String, int>();
                message.findHashtags(trending);
            }
            if (message.text.Contains('@'))
                message.findMentions();
        }

        public KeyValuePair<String, Tweet> addTweet(String sender, String text)
        {
            Tweet message = new Tweet(sender, text.Trim());

            analyseTweet(message);

            String id = generateID('T', tweets.Count());
            tweets.Add(id, message);
            return new KeyValuePair<String, Tweet>(id, tweets[id]);
        }

        public Dictionary<String, SMS> getSMS()
        {
            return sms;
        }

        public Dictionary<String, StandardEmailMessage> getSEMEmails()
        {
            return SEMEmails;
        }

        public Dictionary<String, SignificantIncidentReport> getSIREmails()
        {
            return SIREmails;
        }

        public Dictionary<String, Tweet> getTweets()
        {
            return tweets;
        }

        public Dictionary<String, int> getTrending()
        {
            return trending;
        }

        public void importMessages(String file)
        {
            // TODO implement here
        }

        public void listAll()
        {
            // TODO implement here
        }

        public void listTrending()
        {
            // TODO implement here
        }

        public void listMentions()
        {
            // TODO implement here
        }

        public void listSIRs()
        {
            // TODO implement here
        }

        private void outputMessages()
        {
            // TODO implement here
        }
    }
}