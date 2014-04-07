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

namespace ARK.Protokolsystem
{
    /// <summary>
    /// Interaction logic for HeadlineControl.xaml
    /// </summary>
    public partial class HeadlineControl : UserControl
    {
        public HeadlineControl()
        {
            InitializeComponent();
        }
		
		// Property for binding
        static private string _HeadlineText;
        static public string HeadlineText
        {
            get { return _HeadlineText; }
            set { _HeadlineText = value; }
        }
    }
}
