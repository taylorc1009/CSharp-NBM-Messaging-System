using System;
using System.Collections.Generic;
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

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            var form = new SendForm();
            form.ShowDialog();
            String message = form.message, sender = form.sender, type = form.type;
            if (form.sent)
            {
                DateTime sentAt = DateTime.Now;
                if (type.Equals("Tweet"))
                    messagesFacade.addTweet(sender, message, sentAt);
                else if (type.Equals("SMS"))
                    messagesFacade.addSMS(sender, message, sentAt);
                else if (type.Equals("Email"))
                {
                    if (form.SIRChecked)
                        messagesFacade.addSIR(sender, DateTime.Parse(form.date), form.sortCode, form.nature, message, sentAt);
                    else
                        messagesFacade.addSEM(sender, form.subject, message, sentAt);
                }
            }
            updateList();
        }

        private void updateList()
        {
            List<MessagesListItem> items = new List<MessagesListItem>();
            foreach(KeyValuePair<String, SMS> sms in messagesFacade.getSMS())
            {
                SMS value = sms.Value;
                String brief = makeBrief(value.text);
                items.Add(new MessagesListItem(sms.Key, value.sender, null, brief, value.sentAt, value.header));
            }
            foreach (KeyValuePair<String, StandardEmailMessage> sem in messagesFacade.getSEMEmails())
            {
                StandardEmailMessage value = sem.Value;
                String brief = makeBrief(value.text);
                items.Add(new MessagesListItem(sem.Key, value.sender, value.subject, brief, value.sentAt, value.header));
            }
            foreach (KeyValuePair<String, SignificantIncidentReport> sir in messagesFacade.getSIREmails())
            {
                SignificantIncidentReport value = sir.Value;
                String brief = makeBrief(value.text);
                items.Add(new MessagesListItem(sir.Key, value.sender, value.subject, brief, value.sentAt, value.header));
            }
            foreach (KeyValuePair<String, Tweet> tweet in messagesFacade.getTweets())
            {
                Tweet value = tweet.Value;
                String brief = makeBrief(value.text);
                items.Add(new MessagesListItem(tweet.Key, value.sender, null, brief, value.sentAt, value.header));
            }
            items.Sort((y, x) => DateTime.Compare(x.messageDate, y.messageDate));

            fullList.Items.Clear();
            foreach(MessagesListItem item in items)
                fullList.Items.Add(item);
        }

        public String makeBrief(String text)
        {
            if (text.Length > 20)
                return text.Substring(0, 16) + "...";
            return text;
        }

        private void fullList_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (fullList.SelectedIndex == -1)
                return;

            MessagesListItem listItem = fullList.SelectedItem as MessagesListItem;
            /* Other ways to do this
            MessagesListItem second = (MessagesListItem)fullList.Items[fullList.SelectedIndex];
            MessagesListItem third = (sender as ListBox).SelectedItem as MessagesListItem;
            MessagesListItem fourth = args.AddedItems[0] as MessagesListItem;*/

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
    }
}
