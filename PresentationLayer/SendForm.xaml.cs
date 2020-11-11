using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToolTip = System.Windows.Forms.ToolTip;

namespace PresentationLayer
{
    public partial class SendForm : Window
    {
        public string sender { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public string subject { get; set; }
        public string date { get; set; }
        public string sortCode { get; set; }
        public string nature { get; set; }
        public bool SIRChecked { get; set; }
        bool tooLong = false;

        public SendForm()
        {
            InitializeComponent();

            //this is initially set to 1 because 0 means infinite length
            //I set it to 1 so that a message couldn't be entered without identifying the sender, thus the type of message, first
            messageBox.MaxLength = 1;

            SIRChecked = false;

            natureCombo.Items.Insert(0, "ATM Theft");
            natureCombo.Items.Insert(1, "Bomb Threat");
            natureCombo.Items.Insert(2, "Cash Loss");
            natureCombo.Items.Insert(3, "Customer Attack");
            natureCombo.Items.Insert(4, "Intelligence");
            natureCombo.Items.Insert(5, "Raid");
            natureCombo.Items.Insert(6, "Staff Abuse");
            natureCombo.Items.Insert(7, "Staff Attack");
            natureCombo.Items.Insert(8, "Suspicious Incident");
            natureCombo.Items.Insert(9, "Terrorism");
            natureCombo.Items.Insert(10, "Theft");
        }

        private static bool isValidEmail(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        private static bool isValidTwitter(String twitter)
        {
            //return twitter[0] == '@' && !String.IsNullOrWhiteSpace(twitter.Substring(1)) && !int.TryParse(twitter[1].ToString(), out _) && Regex.IsMatch(twitter.Substring(1), @"^[a-z0-9-_]+$", RegexOptions.IgnoreCase);
            return Regex.IsMatch(twitter, @"^@+[a-z][a-z0-9-_]*$", RegexOptions.IgnoreCase);
        }

        private void senderBox_TextChanged(object s, TextChangedEventArgs e)
        {
            if (senderBox.Text.Equals(""))
            {
                type = null;
                invalidLabel.Visibility = Visibility.Hidden;
                if (SIRCheck.IsVisible)
                    SIRCheck.Visibility = Visibility.Collapsed;
                if (subjectBox.IsVisible)
                    subjectBox.Visibility = Visibility.Collapsed;
                messageBox.MaxLength = 1;
            }
            else
            {
                if (senderBox.Text[0] == '+' && int.TryParse(senderBox.Text.Substring(1), out _))
                {
                    type = "SMS";
                    senderBox.MaxLength = 12;
                    messageBox.MaxLength = 140;
                    invalidLabel.Visibility = Visibility.Hidden;
                }
                else if (isValidTwitter(senderBox.Text))
                {
                    type = "Tweet";
                    senderBox.MaxLength = 16;
                    messageBox.MaxLength = 140;
                    invalidLabel.Visibility = Visibility.Hidden;
                }
                else if (isValidEmail(senderBox.Text))
                {
                    type = "Email";
                    senderBox.MaxLength = 50;
                    messageBox.MaxLength = 1028;
                    SIRCheck.Visibility = Visibility.Visible;
                    subjectBox.Visibility = Visibility.Visible;
                    invalidLabel.Visibility = Visibility.Hidden;
                }
                else
                {
                    invalidLabel.Visibility = Visibility.Visible;
                    if (SIRCheck.IsVisible)
                        SIRCheck.Visibility = Visibility.Collapsed;
                    if (subjectBox.IsVisible)
                        subjectBox.Visibility = Visibility.Collapsed;
                    messageBox.MaxLength = 1;
                }
            }
            if (messageBox.Text.Length > messageBox.MaxLength)
            {
                tooLong = true;
                tooLongLabel.Visibility = Visibility.Visible;
            }
            else
            {
                tooLong = false;
                tooLongLabel.Visibility = Visibility.Hidden;
            }
            sender = senderBox.Text;
        }

        private void messageBox_KeyDown(object s, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                String[] str = {messageBox.Text.Substring(0, messageBox.SelectionStart), messageBox.Text.Substring(messageBox.SelectionStart)};
                String concat = str[0] + Environment.NewLine + str[1];
                messageBox.Clear();
                messageBox.AppendText(concat);
                messageBox.Select((str[0] + Environment.NewLine).Length, 0);
            }
        }

        private void SIRCheck_Clicked(object s, EventArgs e)
        {
            if (!SIRChecked)
            {
                SIRChecked = true;
                subjectBox.Visibility = Visibility.Collapsed;
                SIRDate.Visibility = Visibility.Visible;
                sortCodeLabel.Visibility = Visibility.Visible;
                sortCodeBox.Visibility = Visibility.Visible;
                natureCombo.Visibility = Visibility.Visible;
            }
            else
            {
                SIRChecked = false;
                subjectBox.Visibility = Visibility.Visible;
                SIRDate.Visibility = Visibility.Collapsed;
                sortCodeLabel.Visibility = Visibility.Collapsed;
                sortCodeBox.Visibility = Visibility.Collapsed;
                natureCombo.Visibility = Visibility.Collapsed;
            }
        }

        private void sendButton_Click(object s, EventArgs e)
        {
            if (sender != null && !sender.Equals(""))
            {
                if (!tooLong)
                {
                    if (type == "Email")
                    {
                        if (SIRChecked)
                        {
                            if (SIRDate.Text.Equals("") || sortCodeBox.Text.Equals("") || natureCombo.SelectedItem == null)
                            {
                                System.Windows.Forms.MessageBox.Show("SIRs must have a:\n\n1. Date of the incident\n2. Branch sort code\n3. Nature of the incident", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            date = SIRDate.Text;
                            sortCode = sortCodeBox.Text;
                            nature = natureCombo.SelectedItem.ToString();
                        }
                        else
                        {
                            if (subjectBox.Text.Equals(""))
                            {
                                System.Windows.Forms.MessageBox.Show("Please give the recipient a short subject description.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            subject = subjectBox.Text;
                        }
                    }
                    if (messageBox.Text.Equals(""))
                    {
                        System.Windows.Forms.MessageBox.Show("Cannot send an empty message.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    message = messageBox.Text;
                    Close();
                }
                else
                    System.Windows.Forms.MessageBox.Show("Message too long for the current message type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                System.Windows.Forms.MessageBox.Show("Messages must have a sender.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
