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
        MessagesFacade messagesFacade;

        public MainWindow()
        {
            InitializeComponent();
            messagesFacade = new MessagesFacade();
        }

        private void sendMessage_Click(object s, EventArgs e)
        {
            SendForm form = new SendForm();
            form.ShowDialog();
            String message = form.message, sender = form.sender, type = form.type;
            char header = '0';

            if (form.sent)
            {
                if (type.Equals("Tweet"))
                {
                    messagesFacade.addTweet(sender, message);
                    header = 'T';
                }
                else if (type.Equals("SMS"))
                {
                    messagesFacade.addSMS(sender, message);
                    header = 'S';
                }
                else if (type.Equals("Email"))
                {
                    if (form.SIRChecked)
                        messagesFacade.addSIR(sender, DateTime.Parse(form.date), form.sortCode, form.nature, message);
                    else
                        messagesFacade.addSEM(sender, form.subject, message);
                    header = 'E';
                }
            }

            updateList(Tuple.Create(true, header, form.SIRChecked));
        }

        private void updateList(Tuple<bool, char, bool> isAdd)
        {
            List<MessagesListItem> items = new List<MessagesListItem>(), sirs = new List<MessagesListItem>(), mentions = new List<MessagesListItem>();
            Dictionary<MessagesListItem, int> trending = new Dictionary<MessagesListItem, int>();

            if (isAdd.Item1) //this is used to add messages who's 'foreach' won't run, back into the items list
            {
                foreach (MessagesListItem item in fullList.Items)
                {
                    if (isAdd.Item2 == 'E')
                    {
                        if (item.subject.Text.Substring(0, 3) == "SIR" && !isAdd.Item3)
                            items.Add(item);
                        else if (item.subject.Text.Substring(0, 3) != "SIR" && isAdd.Item3)
                            items.Add(item);
                    }
                    else if (item.type.Text[0] != isAdd.Item2)
                        items.Add(item);
                }
            }

            if ((isAdd.Item1 && isAdd.Item2 == 'S') || !isAdd.Item1) //this is used to prevent having to repopulate lists unnecessarily, and determine if we're adding a message or importing all stored messages
            {
                foreach (KeyValuePair<String, SMS> sms in messagesFacade.getSMS())
                {
                    SMS value = sms.Value;
                    String brief = makeBrief(value.text);
                    items.Add(new MessagesListItem(sms.Key, value.sender, null, brief, value.sentAt, isAdd.Item2));
                }
            }
            if ((isAdd.Item1 && isAdd.Item2 == 'E' && !isAdd.Item3) || !isAdd.Item1)
            {
                foreach (KeyValuePair<String, StandardEmailMessage> sem in messagesFacade.getSEMEmails())
                {
                    StandardEmailMessage value = sem.Value;
                    String brief = makeBrief(value.text);
                    items.Add(new MessagesListItem(sem.Key, value.sender, value.subject, brief, value.sentAt, isAdd.Item2));
                }
            }
            if ((isAdd.Item1 && isAdd.Item2 == 'E' && isAdd.Item3) || !isAdd.Item1)
            {
                foreach (KeyValuePair<String, SignificantIncidentReport> sir in messagesFacade.getSIREmails())
                {
                    SignificantIncidentReport value = sir.Value;
                    String brief = makeBrief(value.text);
                    MessagesListItem item = new MessagesListItem(sir.Key, value.sender, value.subject, brief, value.sentAt, isAdd.Item2);
                    items.Add(item);
                    sirs.Add(item);
                }
                sirs.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));

                SIRList.Items.Clear();
                foreach (MessagesListItem sir in sirs)
                    SIRList.Items.Add(sir);
            }
            if ((isAdd.Item1 && isAdd.Item2 == 'T') || !isAdd.Item1)
            {
                Dictionary<String, int> trendingData = messagesFacade.getTrending();
                foreach (KeyValuePair<String, Tweet> tweet in messagesFacade.getTweets())
                {
                    Tweet value = tweet.Value;
                    String brief = makeBrief(value.text);
                    MessagesListItem item = new MessagesListItem(tweet.Key, value.sender, null, brief, value.sentAt, isAdd.Item2);
                    items.Add(item);

                    List<String> hashtagsData = value.getHashtags();
                    if (trendingData != null && hashtagsData != null)
                        foreach (KeyValuePair<String, int> hashtag in trendingData)
                            if (hashtagsData.Contains(hashtag.Key))
                                trending.Add(item, hashtag.Value);

                    List<String> mentionsData = value.getMentions();
                    if(mentionsData != null)
                        if (mentionsData.Any())
                            mentions.Add(item);
                }
                trendingList.Items.Clear();
                foreach (KeyValuePair<MessagesListItem, int> item in trending.OrderBy(i => i.Value))
                    trendingList.Items.Add(item.Key);

                mentions.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));
                mentionsList.Items.Clear();
                foreach (MessagesListItem mention in mentions)
                    mentionsList.Items.Add(mention);
            }
            items.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));
            fullList.Items.Clear();

            foreach (MessagesListItem item in items)
            {
                if (item.Parent != null) //if a message exists in another list, we need to create a duplicate as objects cannot have more than one parent
                {
                    char header = item.type.Text[0];
                    MessagesListItem dupe = new MessagesListItem(item.messageID, item.head.Text, header == 'E' ? item.subject.Text : null, item.body.Text, item.messageDate, header);
                    fullList.Items.Add(dupe);
                }
                else
                    fullList.Items.Add(item);
            }
        }

        public String makeBrief(String text)
        {
            if (text.Length > 20)
                return text.Substring(0, 16) + "...";
            return text;
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            ListBox l = null;
            if (sender.GetType() == fullList.GetType())
                l = (ListBox)sender;
            if (l == null || l.SelectedIndex == -1)
                return;

            MessagesListItem listItem = args.AddedItems[0] as MessagesListItem;

            String id = listItem.messageID;
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
                    //import
                }
                else
                    System.Windows.Forms.MessageBox.Show("Message was not valid to be imported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
