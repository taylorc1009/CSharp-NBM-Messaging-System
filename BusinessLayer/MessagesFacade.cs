/* TODO List
 * 
 * Implement (? - signifies a non-essential proposal; one of my own proposed additions):
 *      (?) search query
 *      (?) export message
 *      (?) credentials/log-in system
 *      (?) make list items width match parent
 *      (?) change list item type text to bitmap and collapse in in lists other than 'fullList'
 *      (?) line separator to ListBoxes
 *      (?) allow logins with different authorization levels;
 *              either:
 *                  user
 *                  manager
 *              and allow the user to link their email, Twitter and phone number all to the same login:
 *                  for example, in a credentials.json, store email, phone number and Twitter ID together for accounts that have linked them.
 *              display messages sent only to the current login.
 *                  store messages with recipient info also.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using DataLayer;

namespace BusinessLayer
{
    //basically the centre program for what would be considered 'back-end' work
    public class MessagesFacade
    {
        public MessagesFacade(String file)
        {
            //imports the list of abbreviations that SMS and Tweets can contain and the serialized messages, as soon as the Facade is declared
            importAbbreviations();
            importMessages(file);
        }
        
        private Dictionary<String, SMS> sms = new Dictionary<String, SMS>();
        private Dictionary<String, StandardEmailMessage> SEMEmails = new Dictionary<String, StandardEmailMessage>();
        private Dictionary<String, SignificantIncidentReport> SIREmails = new Dictionary<String, SignificantIncidentReport>();
        private Dictionary<String, Tweet> tweets = new Dictionary<String, Tweet>();
        private Dictionary<String, String> abbreviations;
        private Dictionary<String, int> trending;

        //imports the abbreviations from the 'textwords.csv' in the current directory
        private void importAbbreviations()
        {
            //'using' automatically closes the file after we're done, so there's no need to manually close it
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

        //generates a 10-digit ID for each message based on the message header (type) and the count of the currently present messages of the same type
        private String generateID(char header, int count)
        {
            StringBuilder id = new StringBuilder(header + "000000000");
            
            //replaces the last n characters with the count, based on the amount of numbers in the count
            String countStr = count.ToString();
            if (countStr.Length < 10)
                for (int i = 0; i < countStr.Length; i++)
                    id[id.Length - countStr.Length + i] = countStr[i];

            return id.ToString();
        }

        public KeyValuePair<String, SMS> addSMS(String sender, String text)
        {
            SMS message = new SMS(sender, text.Trim());

            //if the message being added is valid, add it to the dictionary, otherwise return an empty outcome off the attempt
            if (message.validate(message.sender, null, text, DateTime.MinValue, null, null))
            {
                //check for any abbreviations in the message body, using the dictionary of abbreviations collected
                message.findAbbreviations(abbreviations);

                String id = generateID('S', sms.Count());
                sms.Add(id, message);

                return new KeyValuePair<String, SMS>(id, sms[id]);
            }
            else
                return new KeyValuePair<string, SMS>();
        }

        public KeyValuePair<String, StandardEmailMessage> addSEM(String sender, String subject, String text)
        {
            StandardEmailMessage message = new StandardEmailMessage(sender, subject.Trim(), text.Trim());

            if (message.validate(message.sender, message.subject, text, DateTime.MinValue, null, null))
            {
                //check for any URLs present in the message body and quarantine them for security checks
                message.quarantineURLs();

                String id = generateID('E', SEMEmails.Count() + SIREmails.Count());
                SEMEmails.Add(id, message);

                return new KeyValuePair<String, StandardEmailMessage>(id, SEMEmails[id]);
            }
            else
                return new KeyValuePair<String, StandardEmailMessage>();
        }

        public KeyValuePair<String, SignificantIncidentReport> addSIR(String sender, DateTime date, String sortCode, String nature, String text)
        {
            SignificantIncidentReport message = new SignificantIncidentReport(sender, date, sortCode, nature, text.Trim());

            if (message.validate(message.sender, null, text, message.date, message.sortCode, message.nature))
            {
                message.quarantineURLs();

                String id = generateID('E', SEMEmails.Count() + SIREmails.Count());
                SIREmails.Add(id, message);

                return new KeyValuePair<String, SignificantIncidentReport>(id, SIREmails[id]);
            }
            else
                return new KeyValuePair<String, SignificantIncidentReport>();
        }

        //Tweets have a few different classifications to do, so lets do them here
        private void analyseTweet(Tweet message)
        {
            message.findAbbreviations(abbreviations);

            //prevents checking for hashtags if the message body doesn't contain any
            if (message.text.Contains('#'))
            {
                //initialises trending only if we have a hashtag to add to the trending list
                if (trending == null)
                    trending = new Dictionary<String, int>();

                message.findHashtags(trending);
            }

            //again, prevents checking if the body doesn't contain a mention, signified by '@'
            if (message.text.Contains('@'))
                message.findMentions();
        }

        public KeyValuePair<String, Tweet> addTweet(String sender, String text)
        {
            Tweet message = new Tweet(sender, text.Trim());

            if (message.validate(message.sender, null, text, DateTime.MinValue, null, null))
            {
                analyseTweet(message);

                String id = generateID('T', tweets.Count());
                tweets.Add(id, message);

                return new KeyValuePair<String, Tweet>(id, tweets[id]);
            }
            else
                return new KeyValuePair<String, Tweet>();
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
            //the directory normally won't be empty
            //so far the only instance of an empty path is present to prevent any messages from being imported during testing
            if (!String.IsNullOrEmpty(file))
            {
                String[] import = IOSystem.importMessages(file);

                if (import != null)
                {
                    //deserializes the strings containing the serialized dictionaries
                    //we need to deserialize them using the anonymous types method; as we have lists of object within objects, we need to signify what they are
                    sms = JsonConvert.DeserializeAnonymousType(import[0], sms);
                    tweets = JsonConvert.DeserializeAnonymousType(import[1], tweets);
                    SEMEmails = JsonConvert.DeserializeAnonymousType(import[2], SEMEmails);
                    SIREmails = JsonConvert.DeserializeAnonymousType(import[3], SIREmails);
                    trending = JsonConvert.DeserializeAnonymousType(import[4], trending);
                }
            }
        }

        public void outputMessages(String file)
        {
            if (!String.IsNullOrEmpty(file))
            {
                //serializes the dictionaries into a string[], to then be exported to a JSON
                string[] output = { JsonConvert.SerializeObject(sms), JsonConvert.SerializeObject(tweets), JsonConvert.SerializeObject(SEMEmails), JsonConvert.SerializeObject(SIREmails), JsonConvert.SerializeObject(trending) };
                IOSystem.exportMessages(file, output);
            }
        }
    }
}