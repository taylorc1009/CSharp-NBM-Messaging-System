using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BusinessLayer;
using PresentationLayer;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UnitTest
{
    [TestClass]
    public class UnitTests
    {
        MessagesFacade messagesFacade = new MessagesFacade();

        [TestMethod]
        public void storeAndValidate()
        {
            //lets simulate data a user may enter for each type of message, one valid and another invalid
            //if a message is valid, data will be stored in their respective dictionary in MessagesFacade
            //if the message is recognised as invalid, the KeyValuePair returned will contain null values

            //first of all, I'm going to use a List of boolean to collect the results
            List<bool[]> results = new List<bool[]>();
            bool[] smsResults = new bool[2];
            bool[] semResults = new bool[2];
            bool[] sirResults = new bool[2];
            bool[] tweetResults = new bool[2];

            //SMS tests

            String[] smsValid = { "+09878765274", "SMS message body." };
            messagesFacade.addSMS(smsValid[0], smsValid[1]);
            smsResults[0] = messagesFacade.getSMS()["S000000000"].sender == "+09878765274" && messagesFacade.getSMS()["S000000000"].text == "SMS message body.";

            String[] smsInvalid = { "07839274826", "SMS message body." }; //the phone number format should be recognised as invalid (lack of an international number '+')
            KeyValuePair<String, SMS> smsPair = messagesFacade.addSMS(smsInvalid[0], smsInvalid[1]);
            smsResults[1] = smsPair.Key == null; //if it's not null, the invalid message was stored, which shouldn't happen

            results.Add(smsResults);

            //Standard Email Message tests

            String[] semValid = { "example@domain.com", "SEM email subject", "SEM email body." };
            messagesFacade.addSEM(semValid[0], semValid[1], semValid[2]);
            semResults[0] = messagesFacade.getSEMEmails()["E000000000"].sender == "example@domain.com" && messagesFacade.getSEMEmails()["E000000000"].subject == "SEM email subject" && messagesFacade.getSEMEmails()["E000000000"].text == "SEM email body.";

            String[] semInvalid = { "my email", "", "SEM email body." }; //the email format should be invalid
            KeyValuePair<String, StandardEmailMessage> semPair = messagesFacade.addSEM(semInvalid[0], semInvalid[1], semInvalid[2]);
            semResults[1] = semPair.Key == null; //again, if it's not null, the invalid message was stored, which shouldn't happen

            results.Add(semResults);

            //Significant Incident Report tests

            String[] sirValid = { "example@napierbank.com", "09-47-10", "Raid", "SIR email body." };
            messagesFacade.addSIR(sirValid[0], DateTime.Now, sirValid[1], sirValid[2], sirValid[3]);
            //also, the email ID should now be "E000000001" if the ID generator works correctly
            sirResults[0] = messagesFacade.getSIREmails()["E000000001"].sender == "example@napierbank.com" && messagesFacade.getSIREmails()["E000000001"].subject == "SIR " + DateTime.Now.ToString("dd/MM/yy") && messagesFacade.getSIREmails()["E000000001"].sortCode == "09-47-10" && messagesFacade.getSIREmails()["E000000001"].nature == "Raid";

            String[] sirInvalid = { "example1@napierbank.com", "11/2/1990", "my sortcode", "Raid", "SIR email body." }; //sort code and date should be invalid (SIRs can only be created in the space of a year)
            KeyValuePair<String, SignificantIncidentReport> sirPair = messagesFacade.addSIR(sirInvalid[0], DateTime.Parse(sirInvalid[1]), sirInvalid[2], sirInvalid[3], sirInvalid[4]);
            sirResults[1] = sirPair.Key == null;

            results.Add(sirResults);

            //Tweet tests

            String[] tweetValid = { "@example", "Tweet body." };
            messagesFacade.addTweet(tweetValid[0], tweetValid[1]);
            tweetResults[0] = messagesFacade.getTweets()["T000000000"].sender == "@example" && messagesFacade.getTweets()["T000000000"].text == "Tweet body.";

            String[] tweetInvalid = { "twitterID", "SMS message body." }; //the Twitter ID format should be recognised as invalid (lack of an '@')
            KeyValuePair<String, Tweet> tweetPair = messagesFacade.addTweet(tweetInvalid[0], tweetInvalid[1]);
            tweetResults[1] = tweetPair.Key == null;

            results.Add(tweetResults);

            bool result = true;
            foreach (bool[] r in results)
                if (!(r[0] && r[1])) //if all values were stored and validated as expected, result will remain true
                    result = false;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void expandAbbreviations()
        {
            //both Tweets and SMS messages use the same abbreviation expansion method, so if it works for one it will for for the other

            String[] smsAbbrevated = { "+08927382", "I hope this expands lol." };
            KeyValuePair<String, SMS> sms = messagesFacade.addSMS(smsAbbrevated[0], smsAbbrevated[1]);
            Assert.IsTrue(sms.Value.text == "I hope this expands lol <Laughing out loud>.");
        }

        [TestMethod]
        public void acquireHashtagAndMention()
        {
            //here we try to find if the Tweet.hashtags and Tweet.mentions lists contain the hashtags/mentions we enter in a message body, indicating a successful classification

            String[] tweetWithItems = { "@example", "Can it #find either the # or the @mention?" }; //the code should also not crash because of the single #
            KeyValuePair<String, Tweet> tweet = messagesFacade.addTweet(tweetWithItems[0], tweetWithItems[1]);
            Assert.IsTrue(tweet.Value.getHashtags().Contains("#find") && tweet.Value.getMentions().Contains("@mention"));
        }

        [TestMethod]
        public void quarantineURL()
        {

        }
    }
}
