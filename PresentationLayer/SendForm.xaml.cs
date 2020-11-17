using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using ToolTip = System.Windows.Forms.ToolTip;
using BusinessLayer;

namespace PresentationLayer
{
    public partial class SendForm : Window
    {
        public string sender { get; set; }
        public char type { get; set; }
        public string message { get; set; }
        public string subject { get; set; } = String.Empty;
        public string date { get; set; } = String.Empty;
        public string sortCode { get; set; } = String.Empty;
        public string nature { get; set; } = String.Empty;
        public bool SIRChecked { get; set; } = false;
        public bool sent { get; set; } = false;

        bool tooLong = false;
        bool validSort = true;

        public SendForm(String pSender, String pSubject, String pMessage, bool pSIRChecked, String pDate, String pSortCode, String pNature)
        {
            InitializeComponent();

            //this is initially set to 1 because 0 means infinite length
            //I set it to 1 so that a message couldn't be entered without identifying the sender, thus the type of message, first
            messageBox.MaxLength = 1;

            SIRDate.DisplayDateStart = DateTime.Now.AddYears(-1);
            SIRDate.DisplayDateEnd = DateTime.Now;

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

            //reopens an invalid message to be edited
            if (!String.IsNullOrEmpty(pSender))
                senderBox.Text = pSender;
            if (!String.IsNullOrEmpty(pSubject))
                subjectBox.Text = pSubject;
            if (!String.IsNullOrEmpty(pMessage))
                messageBox.Text = pMessage;
            if (!String.IsNullOrEmpty(pDate))
                SIRDate.SelectedDate = DateTime.Parse(pDate);
            if (!String.IsNullOrEmpty(pSortCode))
                sortCodeBox.Text = pSortCode;
            if (!String.IsNullOrEmpty(pNature))
                natureCombo.SelectedItem = pNature;
            if (pSIRChecked)
            {
                SIRCheck.IsChecked = pSIRChecked;
                SIRChecked = pSIRChecked;
                subjectBox.Visibility = Visibility.Collapsed;
                SIRDate.Visibility = Visibility.Visible;
                sortCodeLabel.Visibility = Visibility.Visible;
                sortCodeBox.Visibility = Visibility.Visible;
                natureCombo.Visibility = Visibility.Visible;
            }
        }

        private void senderBox_TextChanged(object s, TextChangedEventArgs e)
        {
            if (!senderBox.Text.Equals(""))
            {
                if (Utilities.isValidPhoneNumber(senderBox.Text))
                {
                    type = 'S';
                    senderBox.MaxLength = 12;
                    messageBox.MaxLength = 140;
                    invalidLabel.Visibility = Visibility.Hidden;
                }
                else if (Utilities.isValidTwitter(senderBox.Text))
                {
                    type = 'T';
                    senderBox.MaxLength = 16;
                    messageBox.MaxLength = 140;
                    invalidLabel.Visibility = Visibility.Hidden;
                }
                else if (Utilities.isValidEmail(senderBox.Text))
                {
                    type = 'E';
                    senderBox.MaxLength = 40;
                    messageBox.MaxLength = 1028;
                    SIRCheck.Visibility = Visibility.Visible;
                    if (SIRChecked)
                    {
                        SIRDate.Visibility = Visibility.Visible;
                        sortCodeLabel.Visibility = Visibility.Visible;
                        sortCodeBox.Visibility = Visibility.Visible;
                        natureCombo.Visibility = Visibility.Visible;
                    }
                    else
                        subjectBox.Visibility = Visibility.Visible;
                    invalidLabel.Visibility = Visibility.Hidden;
                }
                else
                    type = '0';
            }
            else
                type = '0';

            if (type == '0')
            {
                senderBox.MaxLength = 40;
                invalidLabel.Visibility = Visibility.Visible;
                if (SIRCheck.IsVisible)
                    SIRCheck.Visibility = Visibility.Collapsed;
                if (subjectBox.IsVisible)
                    subjectBox.Visibility = Visibility.Collapsed;
                if (SIRDate.IsVisible)
                    SIRDate.Visibility = Visibility.Collapsed;
                if (sortCodeLabel.IsVisible)
                    sortCodeLabel.Visibility = Visibility.Collapsed;
                if (sortCodeBox.IsVisible)
                    sortCodeBox.Visibility = Visibility.Collapsed;
                if (natureCombo.IsVisible)
                    natureCombo.Visibility = Visibility.Collapsed;
                messageBox.MaxLength = 1;
            }
            else
            {
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

        private void sortCodeBox_TextChanged(object s, TextChangedEventArgs e)
        {
            if (sortCodeBox.Text.Equals(""))
            {
                validSort = false;
                invalidSortLabel.Visibility = Visibility.Collapsed;
            }
            else if(Utilities.isValidSortCode(sortCodeBox.Text))
            {
                validSort = true;
                invalidSortLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                validSort = false;
                invalidSortLabel.Visibility = Visibility.Visible;
            }
        }

        private void sendButton_Click(object s, EventArgs e)
        {
            if (sender != null && !sender.Equals("") && type != '0')
            {
                if (!tooLong)
                {
                    if (!messageBox.Text.Equals(""))
                    {
                        if (type == 'E')
                        {
                            if (SIRChecked)
                            {
                                if (SIRDate.Text.Equals("") || sortCodeBox.Text.Equals("") || natureCombo.SelectedItem == null)
                                {
                                    System.Windows.Forms.MessageBox.Show("SIRs must have a:\n\n1. Date of the incident\n2. Branch sort code\n3. Nature of the incident", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                else if (!validSort)
                                {
                                    System.Windows.Forms.MessageBox.Show("Branch sort code is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        message = messageBox.Text;
                        sent = true;
                        if (!SIRCheck.IsVisible)
                            SIRChecked = false;
                        Close();
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Cannot send an empty message.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    System.Windows.Forms.MessageBox.Show("Message too long for the current message type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                System.Windows.Forms.MessageBox.Show("Messages must have a valid sender.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}