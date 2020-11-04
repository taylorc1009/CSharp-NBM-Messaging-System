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
    public partial class SendForm : Window
    {
        public SendForm()
        {
            InitializeComponent();
            formatCombo.Items.Insert(0, "SMS");
            formatCombo.Items.Insert(1, "Email");
            formatCombo.Items.Insert(2, "Tweet");
            emailCombo.Items.Insert(0, "Standard");
            emailCombo.Items.Insert(1, "Incident Report");
        }
    }
}
