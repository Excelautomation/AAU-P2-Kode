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

namespace Administrationssystem
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class Filter : UserControl
    {
        List<FilterControl> filterControls = new List<FilterControl>();
        public Filter()
        {
            InitializeComponent();

            for (int i = 0; i < 4; i++)
                ListBox.Items.Add(new FilterControl("FilterNavn"));
        }
    }
}
