using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLayer;
using DataLayer;
using ListBox = System.Windows.Controls.ListBox;

namespace PresentationLayer
{
    public partial class MainWindow : Window
    {
        List<MessagesListItem> items = new List<MessagesListItem>(), sirs = new List<MessagesListItem>(), mentions = new List<MessagesListItem>();
        Dictionary<MessagesListItem, int> trending = new Dictionary<MessagesListItem, int>();
        MessagesFacade messagesFacade;

        public MainWindow()
        {
            InitializeComponent();
            messagesFacade = new MessagesFacade();
            messagesFacade.importMessages();
            importList();
        }

        private MessagesListItem createListItem(String id, String sender, String subject, String text, DateTime sentAt, char header)
        {
            String brief = makeBrief(text);
            MessagesListItem item = new MessagesListItem(id, sender, subject, brief, sentAt, header);
            items.Add(item);
            return item;
        }

        public bool addMessage(char type, String sender, String subject, String message, bool SIRChecked, String date, String sortCode, String nature)
        {
            if (type == 'T')
            {
                KeyValuePair<String, Tweet> tweet = messagesFacade.addTweet(sender, message);
                if (tweet.Key == null)
                    return false;
                MessagesListItem item = createListItem(tweet.Key, tweet.Value.sender, null, tweet.Value.text, tweet.Value.sentAt, 'T');
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
            messagesFacade.outputMessages();
            return true;
        }

        private void sendMessage_Click(object s, EventArgs e)
        {
            String sender = String.Empty, subject = String.Empty, message = String.Empty, date = String.Empty, sortCode = String.Empty, nature = String.Empty;
            bool SIRChecked = false, valid = false;

            while (!valid)
            {
                SendForm form = new SendForm(sender, subject, message, SIRChecked, date, sortCode, nature);
                form.ShowDialog();
                char header = form.type;

                if (form.sent)
                {
                    valid = addMessage(header, form.sender, form.subject, form.message, form.SIRChecked, form.date, form.sortCode, form.nature);
                    if (!valid)
                    {
                        sender = form.sender;
                        subject = form.subject;
                        message = form.message;
                        SIRChecked = form.SIRChecked;
                        date = form.date;
                        sortCode = form.sortCode;
                        nature = form.nature;
                        System.Windows.Forms.MessageBox.Show("Message was not valid to be sent.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        refreshList(header, form.SIRChecked);
                }
                else
                    valid = true;
            }
        }

        private void categoriseTweetItem(MessagesListItem item, Tweet tweet)
        {
            Dictionary<String, int> trendingData = messagesFacade.getTrending();
            List<String> hashtagsData = tweet.hashtags;
            if (trendingData != null && hashtagsData != null)
                foreach (KeyValuePair<String, int> hashtag in trendingData)
                    if (hashtagsData.Contains(hashtag.Key))
                        trending.Add(item, hashtag.Value);

            List<String> mentionsData = tweet.mentions;
            if (mentionsData != null)
                if (mentionsData.Any())
                    mentions.Add(item);
        }

        private void addDuplicateFix(MessagesListItem item, ListBox control) //if the message exists in another list, we need to create a duplicate as objects cannot have more than one parent
        {
            if (item.Parent != null)
            {
                char header = item.messageID[0];
                MessagesListItem dupe = new MessagesListItem(item.messageID, item.head.Text, header == 'E' ? item.subject.Text : null, item.body.Text, item.messageDate, header);
                control.Items.Add(dupe);
            }
            else
                control.Items.Add(item);
        }

        private void refreshList(char header, bool isSIR)
        {
            if (header == '0' || (header == 'E' && isSIR))
            {
                sirs.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));
                SIRList.Items.Clear();
                foreach (MessagesListItem sir in sirs)
                    addDuplicateFix(sir, SIRList);
            }
            if (header == '0' || header == 'T')
            {
                trendingList.Items.Clear();
                foreach (KeyValuePair<MessagesListItem, int> hashtag in trending.OrderBy(i => i.Value))
                    addDuplicateFix(hashtag.Key, trendingList);

                mentions.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));
                mentionsList.Items.Clear();
                foreach (MessagesListItem mention in mentions)
                    addDuplicateFix(mention, mentionsList);
            }

            items.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));
            fullList.Items.Clear();
            foreach (MessagesListItem item in items)
                addDuplicateFix(item, fullList);
        }

        private void importList()
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
            refreshList('0', false);
        }

        public String makeBrief(String text)
        {
            if (text.Length > 20)
                return text.Substring(0, 17) + "...";
            return text;
        }

        /* This was an attempt to allow the user to click the MessagesListItem control to open the message, but it doesn't work
        
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
            ListBox l = null;
            if (sender.GetType() == fullList.GetType())
                l = (ListBox)sender;
            if (l == null || l.SelectedIndex == -1)
                return;

            MessagesListItem listItem = args.AddedItems[0] as MessagesListItem;

            openMessage(listItem.messageID);
        }

        private void openMessage(String id)
        {
            char header = id[0];
            if (header == 'S')
            {
                Dictionary<String, SMS> messages = messagesFacade.getSMS();
                SMS message = messages[id];
                var form = new ViewMessage(Tuple.Create(message.sender, String.Empty, message.text, message.sentAt));
                form.ShowDialog();
            }
            else if (header == 'T')
            {
                Dictionary<String, Tweet> messages = messagesFacade.getTweets();
                Tweet message = messages[id];
                var form = new ViewMessage(Tuple.Create(message.sender, String.Empty, message.text, message.sentAt));
                form.ShowDialog();
            }
            else if (header == 'E')
            {
                if (messagesFacade.getSEMEmails().ContainsKey(id))
                {
                    Dictionary<String, StandardEmailMessage> messages = messagesFacade.getSEMEmails();
                    StandardEmailMessage message = messages[id];
                    var form = new ViewMessage(Tuple.Create(message.sender, message.subject, message.text, message.sentAt));
                    form.ShowDialog();
                }
                else if (messagesFacade.getSIREmails().ContainsKey(id))
                {
                    Dictionary<String, SignificantIncidentReport> messages = messagesFacade.getSIREmails();
                    SignificantIncidentReport message = messages[id];
                    var form = new ViewMessage(Tuple.Create(message.sender, message.subject, message.text, message.sentAt));
                    form.ShowDialog();
                }
                else
                    System.Windows.Forms.MessageBox.Show("Message was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void importMessage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.Filter = "Text file (*.txt)|*.txt";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IOSystem import = new IOSystem();
                String[] values = import.importFile(dialog.FileName);
                if (values != null)
                    if(addMessage(import.header, values[0], values[1], values[2], values[3] == "true", values[4], values[5], values[6]))
                        refreshList(import.header, values[3] == "true");
                else
                    System.Windows.Forms.MessageBox.Show("Message was not valid to be imported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
