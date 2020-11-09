using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using System.Windows.Shapes;

namespace PresentationLayer
{
    public partial class SendForm : Window
    {
        public string type { get; set; }
        public string message { get; set; }
        bool SIRChecked = false;

        public SendForm()
        {
            InitializeComponent();
        }

        private void senderBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (senderBox.Text.Equals(""))
            {
                type = null;
                invalidLabel.Visibility = Visibility.Hidden;
                if (SIRCheck.IsVisible)
                    SIRCheck.Visibility = Visibility.Collapsed;
                if (subjectBox.IsVisible)
                    subjectBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                try
                {
                    char d = senderBox.Text[0];
                    if (d == '+')
                        type = "SMS";
                    else if (d == '@')
                        type = "Tweet";
                    else if (!senderBox.Text.Equals(""))
                    {
                        new MailAddress(senderBox.Text.ToString());
                        type = "Email";
                        SIRCheck.Visibility = Visibility.Visible;
                        subjectBox.Visibility = Visibility.Visible;
                    }
                    invalidLabel.Visibility = Visibility.Hidden;
                }
                catch (FormatException)
                {
                    invalidLabel.Visibility = Visibility.Visible;
                    if(SIRCheck.IsVisible)
                        SIRCheck.Visibility = Visibility.Collapsed;
                    if(subjectBox.IsVisible)
                        subjectBox.Visibility = Visibility.Collapsed;
                }
            }
            /*if (subjectBox.Text.Length > 20)
            {
                System.Windows.Forms.MessageBox.Show("Text in textBox must have less than 20 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
            }*/
        }

        private void SIRCheck_Clicked(object sender, EventArgs e)
        {
            if (!SIRChecked)
            {
                SIRChecked = true;
                subjectBox.Visibility = Visibility.Collapsed;
                SIRDate.Visibility = Visibility.Visible;
            }
            else
            {
                SIRChecked = false;
                subjectBox.Visibility = Visibility.Visible;
                SIRDate.Visibility = Visibility.Collapsed;
            }
        }

        private void SIRCheck_Checked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            message = messageBox.Text;
            Close();
        }
    }
}
