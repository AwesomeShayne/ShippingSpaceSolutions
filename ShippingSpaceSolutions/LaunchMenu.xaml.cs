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

namespace ShippingSpaceSolutions
{
    /// <summary>
    /// Interaction logic for LaunchMenu.xaml
    /// </summary>
    public partial class LaunchMenu : UserControl
    {
        MainWindow parent;
        public LaunchMenu(MainWindow _parent)
        {
            parent = _parent;
            InitializeComponent();
        }
        private void OpenContainerButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void OpenRequestButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void NewContainerButton_Click(object sender, RoutedEventArgs e)
        {
            parent.SetContent(new DeviceSelector(parent));
        }
    }
}
