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

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MessagesListBox.xaml
    /// </summary>
    public partial class MessagesListItem : UserControl
    {
        public DateTime messageDate { get; set; }
        public String id { get; set; }

        public MessagesListItem(string id, string sender, string breif, DateTime dateTime, char header)
        {
            InitializeComponent();
            head.Text = sender;
            body.Text = breif;
            messageDate = dateTime;
            date.Text = messageDate.ToString();
            switch(header)
            {
                case 'S':
                    type.Text = "SMS";
                    break;
                case 'E':
                    type.Text = "Email";
                    break;
                case 'T':
                    type.Text = "Tweet";
                    break;
            }
        }
    }
}
