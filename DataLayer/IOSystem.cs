using System;
using System.IO;
using BusinessLayer;

namespace DataLayer
{
    public class IOSystem
    {
        public SMS sms { get; set; }
        public Tweet tweet { get; set; }
        public StandardEmailMessage sem { get; set; }
        public SignificantIncidentReport sir { get; set; }
        public char header { get; set; }

        public IOSystem() { }

        public bool importFile(String file)
        {
            try
            {
                string contents = File.ReadAllText(file);
                string[] values = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                header = values[0][0];
                switch (header) {
                    case 'S':
                        sms = new SMS(values[1], values[2]);
                        return sms.validate(sms.sender, String.Empty, sms.text, DateTime.MinValue, String.Empty, String.Empty);
                    case 'T':
                        tweet = new Tweet(values[1], values[2]);
                        return tweet.validate(tweet.sender, String.Empty, tweet.text, DateTime.MinValue, String.Empty, String.Empty);
                    case 'E':
                        if (values[2] == "true")
                        {
                            String[] sirData = values[3].Split(':');
                            sir = new SignificantIncidentReport(values[1], DateTime.Parse(sirData[0]), sirData[1], sirData[2], values[4]);
                            return sir.validate(sir.sender, sir.subject, sir.text, sir.date, sir.sortCode, sir.nature);
                        }
                        else if (values[2] == "false")
                        {
                            sem = new StandardEmailMessage(values[1], values[3], values[4]);
                            return sem.validate(sem.sender, sem.subject, sem.text, DateTime.MinValue, String.Empty, String.Empty);
                        }
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void exportFile(String file)
        {
            // TODO implement here
        }
    }
}
