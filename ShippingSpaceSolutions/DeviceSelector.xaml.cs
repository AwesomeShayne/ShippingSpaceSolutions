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
    /// Interaction logic for DeviceSelector.xaml
    /// </summary>
    public partial class DeviceSelector : UserControl
    {
        public MainWindow parent;
        private List<Package> Packages = new List<Package>();
        private List<List<Package>> orders = new List<List<Package>>();
        int rowCount = 0;
        public DeviceSelector(MainWindow _parent)
        {
            parent = _parent;
            InitializeComponent();
            ItemEntryGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            var row = new ItemEntry(this, rowCount);
            Grid.SetRow(row, rowCount);
            ItemEntryGrid.Children.Add(row);
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            parent.SetContent(new OutputView(Packages, this, parent));
        }

        internal void AddPackage(List<Package> packages, int index)
        {
            Packages.AddRange(packages);
            orders.Insert(index, packages);
            ItemEntryGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            rowCount++;
            var row = new ItemEntry(this, rowCount);
            Grid.SetRow(row, rowCount);
            ItemEntryGrid.Children.Add(row);
        }

        internal void RemovePackages(int index)
        {
            foreach (var rem in orders[index])
            {
                Packages.Remove(rem);
            }
            var row = new ItemEntry(this, index);
            Grid.SetRow(row, index);
            ItemEntryGrid.Children.RemoveAt(index);
            ItemEntryGrid.Children.Add(row);

        }
    }
}
