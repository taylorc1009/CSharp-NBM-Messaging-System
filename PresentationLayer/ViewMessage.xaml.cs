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
using System.Windows.Shapes;

namespace PresentationLayer
{
    public partial class ViewMessage : Window
    {
        public ViewMessage(Tuple<String, String, String, DateTime> message)
        {
            InitializeComponent();
            fromBox.Text = message.Item1;
            if (message.Item2 != String.Empty)
            {
                subjectBox.Text = message.Item2;
                subjectBox.Visibility = Visibility.Visible;
            }
            messageBox.Text = message.Item3;
            dateBlock.Text = "Sent " + message.Item4.ToString("dd/MM/yy") + " at " + message.Item4.ToString("HH:mm");
        }
    }
}
