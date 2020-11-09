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
using System.Windows.Shapes;

namespace PresentationLayer
{
    public partial class SendForm : Window
    {
        public string returnMessage { get; set; }

        public SendForm()
        {
            InitializeComponent();
            formatCombo.Items.Insert(0, "SMS");
            formatCombo.Items.Insert(1, "Email");
            formatCombo.Items.Insert(2, "Tweet");
            emailCombo.Items.Insert(0, "Standard");
            emailCombo.Items.Insert(1, "Incident Report");
        }

        private void subjectBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (subjectBox.Text.Length > 20)
            {
                System.Windows.Forms.MessageBox.Show("Text in textBox must have less than 20 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
            }
        }
        
        private void sendButton_Click(object sender, EventArgs e)
        {
            returnMessage = textBox.Text;
            //this.ReturnValue2 = DateTime.Now.ToString(); //example
            Close();
        }
    }
}
