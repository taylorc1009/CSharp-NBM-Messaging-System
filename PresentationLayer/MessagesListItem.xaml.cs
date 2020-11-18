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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer
{
    public partial class MessagesListItem : UserControl
    {

        public DateTime messageDate { get; set; }
        public String messageID { get; set; }

        public MessagesListItem(string id, string sender, string sub, string breif, DateTime dateTime, char header)
        {
            InitializeComponent();

            messageID = id;
            head.Text = sender;
            if (sub != null)
            {
                subject.Visibility = Visibility.Visible;
                subject.Text = sub;
                /*grid.RowDefinitions.ElementAt(grid.Children.IndexOf(type)).
                grid.Children[grid.Children.IndexOf(type)].*/
                //type.SetValue(Grid.RowSpanProperty, 4);
            }
            body.Text = breif;
            messageDate = dateTime;
            date.Text = messageDate.ToString("HH:mm dd/MM/yy");
            /*switch(header)
            {
                case 'S':
                    type.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/image-cache/sms.png", UriKind.Relative));
                    break;
                case 'E':
                    type.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/image-cache/email.png", UriKind.Relative));
                    break;
                case 'T':
                    type.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/image-cache/twitter.png", UriKind.Relative));
                    break;
            }*/
        }
    }
}
