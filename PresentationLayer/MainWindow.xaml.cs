using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using BusinessLayer;
using DataLayer;
using ListBox = System.Windows.Controls.ListBox;

namespace PresentationLayer
{
    public partial class MainWindow : Window
    {
        private List<MessagesListItem> items = new List<MessagesListItem>(), sirs = new List<MessagesListItem>(), mentions = new List<MessagesListItem>();
        private Dictionary<MessagesListItem, int> trending = new Dictionary<MessagesListItem, int>();
        private MessagesFacade messagesFacade;

        public MainWindow()
        {
            InitializeComponent();
            messagesFacade = new MessagesFacade("messages.json");
            importLists();
        }

        //makes the message body brief, as we don't want to show the entire body in the listed preview
        private String makeBrief(String text)
        {
            if (text.Length > 40)
                return text.Substring(0, 37) + "...";
            return text;
        }

        private MessagesListItem createListItem(String id, String sender, String subject, String text, DateTime sentAt, char header)
        {
            //adds a message, with a brief of the message body, to the list of all messages
            String brief = makeBrief(text);
            MessagesListItem item = new MessagesListItem(id, sender, subject, brief, sentAt, header);
            items.Add(item);

            return item;
        }

        private bool addMessage(char type, String sender, String subject, String message, bool SIRChecked, String date, String sortCode, String nature)
        {
            //adds a message to one of the dictionaries of stored messages in 'messageFacade', based on the type
            if (type == 'T')
            {
                KeyValuePair<String, Tweet> tweet = messagesFacade.addTweet(sender, message);

                //if the message was an invalid message of its type, the add method in 'messagesFacade' will return a null object
                if (tweet.Key == null)
                    return false;

                //if it was valid, we'll add it to the list of messages
                MessagesListItem item = createListItem(tweet.Key, tweet.Value.sender, null, tweet.Value.text, tweet.Value.sentAt, 'T');

                //since the message is a Tweet, we need to categorise it in the ListBoxes correctly
                categoriseTweetItem(item, tweet.Value);
            }
            else if (type == 'S')
            {
                KeyValuePair<String, SMS> sms = messagesFacade.addSMS(sender, message);

                if (sms.Key == null)
                    return false;

                createListItem(sms.Key, sms.Value.sender, null, sms.Value.text, sms.Value.sentAt, 'S');
            }
            else if (type == 'E')
            {
                if (SIRChecked)
                {
                    KeyValuePair<String, SignificantIncidentReport> sir = messagesFacade.addSIR(sender, DateTime.Parse(date), sortCode, nature, message);

                    if (sir.Key == null)
                        return false;

                    MessagesListItem item = createListItem(sir.Key, sir.Value.sender, sir.Value.subject, sir.Value.text, sir.Value.sentAt, 'E');
                    sirs.Add(item);
                }
                else
                {
                    KeyValuePair<String, StandardEmailMessage> sem = messagesFacade.addSEM(sender, subject, message);

                    if (sem.Key == null)
                        return false;

                    createListItem(sem.Key, sem.Value.sender, sem.Value.subject, sem.Value.text, sem.Value.sentAt, 'E');
                }
            }

            //if we reach this point, the attempt to add the message was successful, now attempt to serialize the new collection of messages
            messagesFacade.outputMessages("messages.json");

            return true;
        }

        private String[] sendFormValidation(String header, String sender, String subject, String text, bool SIRChecked, String date, String sortCode, String nature)
        {
            String[] message = { header, sender, subject, text, SIRChecked ? "true" : "false", date, sortCode, nature };
            bool valid = false;

            //continues to open the send form until a valid message is entered
            while (!valid)
            {
                //(re)opens the SendForm, passing it any information acquired from the previous invalid one
                SendForm form = new SendForm(message[1], message[2], message[3], message[4] == "true" ? true : false, message[5], message[6], message[7]);
                form.ShowDialog();

                //if the user clicked send, check if it's valid by tring to store it
                //else, the user didn't click send (and otherwise force closed the form), '.sent' will be false so we can use this to exit the loop
                if (form.sent)
                {
                    //if valid is false (the add method returned null), the message wasn't added
                    valid = addMessage(form.type, form.sender, form.subject, form.message, form.SIRChecked, form.date, form.sortCode, form.nature);

                    message[0] = form.type.ToString();
                    message[1] = form.sender;
                    message[2] = form.subject;
                    message[3] = form.message;
                    message[4] = form.SIRChecked ? "true" : "false";
                    message[5] = form.date;
                    message[6] = form.sortCode;
                    message[7] = form.nature;

                    //if it's not valid, show an error then loop again
                    //else, return the message data
                    if (!valid)
                        System.Windows.Forms.MessageBox.Show("Message was not valid to be sent.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        return message;
                }
                else
                    valid = true;
            }
            return null;
        }

        private void sendMessage_Click(object s, EventArgs e)
        {
            String[] message = sendFormValidation(String.Empty, String.Empty, String.Empty, String.Empty, false, String.Empty, String.Empty, String.Empty);

            if (message != null)
                refreshLists(message[0][0], message[4] == "true");
        }

        private void refresh_Click(object s, EventArgs e)
        {
            //clear all ListBoxes
            fullList.Items.Clear();
            SIRList.Items.Clear();
            trendingList.Items.Clear();
            mentionsList.Items.Clear();

            //clear all Lists containing ListBox's data
            items.Clear();
            sirs.Clear();
            mentions.Clear();
            trending.Clear();

            //attempt to import the serialized messages again
            messagesFacade.importMessages("messages.json");

            importLists();
        }

        private void categoriseTweetItem(MessagesListItem item, Tweet tweet)
        {
            Dictionary<String, int> trendingData = messagesFacade.getTrending();
            List<String> hashtagsData = tweet.hashtags;

            //if there are hashtags present in any tweet, these won't be null
            if (trendingData != null && hashtagsData != null)
                foreach (KeyValuePair<String, int> hashtag in trendingData)
                    if (hashtagsData.Contains(hashtag.Key)) //for every unique hashtag, if the 'tweet' contains one of them, add it to the ListBox of Tweets with trending hashtags
                        trending.Add(item, hashtag.Value);

            List<String> mentionsData = tweet.mentions;
            if (mentionsData != null)
                if (mentionsData.Any()) //if the 'tweet' has any hashtags, add it to the ListBox of Tweets with mentions
                    mentions.Add(item);
        }

        private void addDuplicateFix(MessagesListItem item, ListBox control)
        {
            //if the message exists in another list, we need to create a duplicate as objects cannot have more than one parent
            if (item.Parent != null)
            {
                char header = item.messageID[0];
                MessagesListItem dupe = new MessagesListItem(item.messageID, item.head.Text, header == 'E' ? item.subject.Text : null, item.body.Text, item.messageDate, header);
                control.Items.Add(dupe);
            }
            else
                control.Items.Add(item);
        }

        private void refreshLists(char header, bool isSIR)
        {
            //if we're not adding a single message, we're importing every message that was serialized
            //since we need to refresh every list in this situation, we use a header of '0' to signify there's no specific type of new message
            //or we may be adding a specific type; if it's a SIR or a Tweet, we'll need to attempt to categorise it
            if (header == '0' || (header == 'E' && isSIR))
            {
                //sorts the SIRs by date
                sirs.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));

                SIRList.Items.Clear();
                if (sirs.Any())
                    foreach (MessagesListItem sir in sirs)
                        addDuplicateFix(sir, SIRList);
                else
                    SIRList.Items.Add("No incidents to show...");
            }
            if (header == '0' || header == 'T')
            {
                trendingList.Items.Clear();
                if (trending.Any())
                    //iterates the list of hashtags in descending order of most occurences
                    foreach (KeyValuePair<MessagesListItem, int> hashtag in trending.OrderBy(i => i.Value))
                        addDuplicateFix(hashtag.Key, trendingList);
                else
                    trendingList.Items.Add("No hashtags to show...");

                mentions.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));

                mentionsList.Items.Clear();
                if (mentions.Any())
                    foreach (MessagesListItem mention in mentions)
                        addDuplicateFix(mention, mentionsList);
                else
                    mentionsList.Items.Add("No mentions to show...");
            }

            items.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));

            fullList.Items.Clear();
            if (items.Any())
                foreach (MessagesListItem item in items)
                    addDuplicateFix(item, fullList);
            else
                fullList.Items.Add("No messages to show...");
        }

        //iterates through every dictionary and adds them to their respective lists - this is a complete import situation, such as opening the app
        private void importLists()
        {
            foreach (KeyValuePair<String, SMS> sms in messagesFacade.getSMS())
                createListItem(sms.Key, sms.Value.sender, null, sms.Value.text, sms.Value.sentAt, 'S');

            foreach (KeyValuePair<String, StandardEmailMessage> sem in messagesFacade.getSEMEmails())
                createListItem(sem.Key, sem.Value.sender, sem.Value.subject, sem.Value.text, sem.Value.sentAt, 'E');

            foreach (KeyValuePair<String, SignificantIncidentReport> sir in messagesFacade.getSIREmails())
            {
                MessagesListItem item = createListItem(sir.Key, sir.Value.sender, sir.Value.subject, sir.Value.text, sir.Value.sentAt, 'E');
                sirs.Add(item);
            }

            foreach (KeyValuePair<String, Tweet> tweet in messagesFacade.getTweets())
            {
                MessagesListItem item = createListItem(tweet.Key, tweet.Value.sender, null, tweet.Value.text, tweet.Value.sentAt, 'T');
                categoriseTweetItem(item, tweet.Value);
            }

            refreshLists('0', false);
        }

        /* This was an attempt to allow the user to click the MessagesListItem object as well as the ListItem control (in the ListBoxes) to open the message, but it doesn't work
        
        private void listBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            ListBox parent = (ListBox)item.Parent;
            if (parent == null || parent.SelectedIndex == -1)
                return;
            MessagesListItem message = (MessagesListItem)parent.SelectedItem;
            openMessage(message.messageID);
        }*/

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            //makes sure the control invoking the method is definitely: a ListBox, initialised and using a valid selected item
            ListBox l = null;
            if (sender.GetType() == fullList.GetType())
                l = (ListBox)sender;
            if (l == null || l.SelectedIndex == -1)
                return;

            //gets the message item in the ListBox item (will always be at item index 0 as we only add the message to each item)
            MessagesListItem listItem = args.AddedItems[0] as MessagesListItem;

            //open the message using the message ID
            if (listItem != null)
                openMessage(listItem.messageID);
        }

        private void openMessage(String id)
        {
            char header = id[0];

            if (header == 'S')
            {
                Dictionary<String, SMS> messages = messagesFacade.getSMS();
                SMS message = messages[id];

                //give the OpenForm the selected message text in the form of a Tuple
                ViewMessage form = new ViewMessage(Tuple.Create(message.sender, String.Empty, message.text, message.sentAt));
                form.ShowDialog();
            }
            else if (header == 'T')
            {
                Dictionary<String, Tweet> messages = messagesFacade.getTweets();
                Tweet message = messages[id];

                ViewMessage form = new ViewMessage(Tuple.Create(message.sender, String.Empty, message.text, message.sentAt));
                form.ShowDialog();
            }
            else if (header == 'E')
            {
                //differentiates between a SIR and SEM by shecking which list contains the cuurrent ID
                if (messagesFacade.getSEMEmails().ContainsKey(id))
                {
                    Dictionary<String, StandardEmailMessage> messages = messagesFacade.getSEMEmails();
                    StandardEmailMessage message = messages[id];

                    ViewMessage form = new ViewMessage(Tuple.Create(message.sender, message.subject, message.text, message.sentAt));
                    form.ShowDialog();
                }
                else if (messagesFacade.getSIREmails().ContainsKey(id))
                {
                    Dictionary<String, SignificantIncidentReport> messages = messagesFacade.getSIREmails();
                    SignificantIncidentReport message = messages[id];

                    ViewMessage form = new ViewMessage(Tuple.Create(message.sender, message.subject, message.text, message.sentAt));
                    form.ShowDialog();
                }
            }
        }

        private void importMessage_Click(object s, RoutedEventArgs e)
        {
            //uses the Windows Explorer dialogue to allow the user to select a file
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.Filter = "Text file (*.txt)|*.txt";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //acquires the imported details
                IOSystem import = new IOSystem();
                String[] values = import.importFile(dialog.FileName);

                if (values != null)
                {
                    //uses the same loop contents as the 'sendMessage_Click' method to verify that the imported message is valid
                    String[] message = sendFormValidation(import.header.ToString(), values[0], values[1], values[2], values[3] == "true", values[4], values[5], values[6]);

                    if (message != null)
                        refreshLists(message[0][0], message[4] == "true");
                }
                else
                    System.Windows.Forms.MessageBox.Show("Message was not valid to be imported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
