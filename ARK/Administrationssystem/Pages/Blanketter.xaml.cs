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

namespace ARK.Administrationssystem.Pages
{
    /// <summary>
    /// Interaction logic for Blanketter.xaml
    /// </summary>
    public partial class Blanketter : UserControl
    {
        public Blanketter()
        {
            InitializeComponent();

            for (int i = 0; i < 20; i++)
            {
                StackPanelLeft.Children.Add(new Label() { Content = "Søren er en nub" });
            }
        }
    }
}
