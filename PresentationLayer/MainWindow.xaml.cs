using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
            if (type != null)
            {
                if (type.Equals("Tweet"))
                    messagesFacade.addTweet(sender, message);
                else if (type.Equals("SMS"))
                    messagesFacade.addSMS(sender, message);
                else if (type.Equals("Email"))
                {
                    if (form.SIRChecked)
                        messagesFacade.addSIR(sender, DateTime.Parse(form.date), form.sortCode, form.nature, message);
                    else
                        messagesFacade.addSEM(sender, form.subject, message);
                }
            }
        }
    }
}
