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
using SOSQL;

namespace ShippingSpaceSolutions
{
    /// <summary>
    /// Interaction logic for ItemEntry.xaml
    /// </summary>
    public partial class ItemEntry : UserControl
    {
        DeviceSelector _Container;
        int number;
        public ItemEntry(DeviceSelector container, int _number)
        {
            number = _number;
            _Container = container;
            InitializeComponent();
        }

        private void TypeComboBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void DescComboBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ManufacturerComboBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ModelComboBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var output = new List<Package>();
                int n;
                if (!QuantityTextBox.Text.Equals("") && Int32.Parse(QuantityTextBox.Text) >= 0 )
                    n = Int32.Parse(QuantityTextBox.Text);
                else
                    n = 1;
                for (int i = 0; i < n; i++)
                    output.Add(new MedicalDevice(Int32.Parse(WidthTextBox.Text), Int32.Parse(HeightTextBox.Text), Int32.Parse(DepthTextBox.Text)));
                _Container.AddPackage(output, number);
                AddButton.Visibility = Visibility.Hidden;
                RemoveButton.Visibility = Visibility.Visible;
            } catch (Exception ex)
            {
                
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _Container.RemovePackages(number);
        }
    }
}
