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
            importList();
        }

        private MessagesListItem createListItem(String id, String sender, String subject, String text, DateTime sentAt, char header)
        {
            String brief = makeBrief(text);
            MessagesListItem item = new MessagesListItem(id, sender, subject, brief, sentAt, header);
            items.Add(item);
            return item;
        }

        public void addMessage(char type, String sender, String subject, String message, bool SIRChecked, String date, String sortCode, String nature)
        {
            if (type == 'T')
            {
                KeyValuePair<String, Tweet> tweet = messagesFacade.addTweet(sender, message);
                MessagesListItem item = createListItem(tweet.Key, tweet.Value.sender, null, tweet.Value.text, tweet.Value.sentAt, 'T');
                categoriseTweetItem(item, tweet.Value);
            }
            else if (type == 'S')
            {
                KeyValuePair<String, SMS> sms = messagesFacade.addSMS(sender, message);
                createListItem(sms.Key, sms.Value.sender, null, sms.Value.text, sms.Value.sentAt, 'S');
            }
            else if (type == 'E')
            {
                if (SIRChecked)
                {
                    KeyValuePair<String, SignificantIncidentReport> sir = messagesFacade.addSIR(sender, DateTime.Parse(date), sortCode, nature, message);
                    MessagesListItem item = createListItem(sir.Key, sir.Value.sender, sir.Value.subject, sir.Value.text, sir.Value.sentAt, 'E');
                    sirs.Add(item);
                }
                else
                {
                    KeyValuePair<String, StandardEmailMessage> sem = messagesFacade.addSEM(sender, subject, message);
                    createListItem(sem.Key, sem.Value.sender, sem.Value.subject, sem.Value.text, sem.Value.sentAt, 'E');
                }
            }
        }

        private void sendMessage_Click(object s, EventArgs e)
        {
            SendForm form = new SendForm();
            form.ShowDialog();
            char header = form.type;

            if (form.sent)
            {
                addMessage(header, form.sender, form.subject, form.message, form.SIRChecked, form.date, form.sortCode, form.nature);
                refreshList(header, form.SIRChecked);
            }
        }

        private void categoriseTweetItem(MessagesListItem item, Tweet tweet)
        {
            Dictionary<String, int> trendingData = messagesFacade.getTrending();
            List<String> hashtagsData = tweet.getHashtags();
            if (trendingData != null && hashtagsData != null)
                foreach (KeyValuePair<String, int> hashtag in trendingData)
                    if (hashtagsData.Contains(hashtag.Key))
                        trending.Add(item, hashtag.Value);

            List<String> mentionsData = tweet.getMentions();
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
            else if (header == '0' || header == 'T')
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
                MessagesListItem item = new MessagesListItem(tweet.Key, tweet.Value.sender, null, tweet.Value.text, tweet.Value.sentAt, 'T');
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
                if(import.importFile(dialog.FileName))
                {
                    String[] values = import.getCollection();
                    addMessage(import.header, values[0], values[1], values[2], values[3] == "1", values[4], values[5], values[6]);
                    refreshList(import.header, values[3] == "1");
                }
                else
                    System.Windows.Forms.MessageBox.Show("Message was not valid to be imported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
