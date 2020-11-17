using System;
using System.IO;

namespace DataLayer
{
    public class IOSystem
    {
        /*public SMS sms { get; set; }
        public Tweet tweet { get; set; }
        public StandardEmailMessage sem { get; set; }
        public SignificantIncidentReport sir { get; set; }*/
        public char header { get; set; }

        public IOSystem() { }

        public String[] importFile(String file)
        {
            try
            {
                string contents = File.ReadAllText(file);
                string[] values = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                header = values[0][0];
                switch (header)
                {
                    case 'S':
                        return new String[] { values[1], String.Empty, values[2], "false", String.Empty, String.Empty, String.Empty };
                    case 'T':
                        return new String[] { values[1], String.Empty, values[2], "false", String.Empty, String.Empty, String.Empty };
                    case 'E':
                        if (values[2] == "true") {
                            String[] sirData = values[3].Split(':');
                            return new String[] { values[1], String.Empty, values[4], values[2], sirData[0], sirData[1], sirData[2] };
                        }
                        else
                            return new String[] { values[1], values[3], values[4], values[2], String.Empty, String.Empty, String.Empty };
                    default:
                        return null;
                }
                /*switch (header) {
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
                }*/
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /*public String[] getCollection()
        {
            switch (header)
            {
                case 'S':
                    return new String[] { sms.sender, String.Empty, sms.text, "0", String.Empty, String.Empty, String.Empty };
                case 'T':
                    return new String[] { tweet.sender, String.Empty, tweet.text, "0", String.Empty, String.Empty, String.Empty };
                case 'E':
                    if (sir != null)
                    {
                        String[] text = sir.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); //used to eliminate the generated lines so they aren't generated twice
                        return new String[] { sir.sender, String.Empty, String.Join(Environment.NewLine, text, 2, text.Length - 2), "1", sir.date.ToString(), sir.sortCode, sir.nature };
                    }
                    else
                        return new String[] { sem.sender, sem.subject, sir.text, "0", sir.date.ToString(), sir.sortCode, sir.nature };
                default:
                    return null;
            }
        }*/

        public void exportFile(String file)
        {
            // TODO implement here
        }
    }
}
