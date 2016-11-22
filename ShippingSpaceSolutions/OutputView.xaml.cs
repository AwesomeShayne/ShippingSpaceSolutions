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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SOSQL;

namespace ShippingSpaceSolutions
{
    /// <summary>
    /// Interaction logic for OutputView.xaml
    /// </summary>
    public partial class OutputView : UserControl
    {
        private Container LoadedContainer = new Container(480, 114, 96, 480, 114, 96, 0, 0);
        private List<Package> Packages = new List<Package>();
        private Model3DGroup ModelGroup = new Model3DGroup();
        private GeometryModel3D ContainerObject;
        private List<GeometryModel3D> AllPackageVisuals = new List<GeometryModel3D>();
        private int ShownStep = 1;
        public OutputView(List<Package> _Packages)
        {
            Packages.AddRange(_Packages);
            ContainerObject = LoadedContainer.GetModel();
            InitializeComponent();
            Packages = Packages.OrderByDescending(b => b.Volume()).ToList();

            foreach (Package box in Packages.ToArray())
            {
                if (!LoadedContainer.AddBox(box))
                {
                    StepListBox.Items.Add("This Container could not be loaded with these packages.");
                    return;
                }
            }
            foreach (Package box in Packages.ToArray())
            {
                StepListBox.Items.Add(string.Concat("Load ", box.Name, " at the shown location"));
                AllPackageVisuals.Add(box.GetModel());
            }
            ModelGroup.Children.Add(AllPackageVisuals[0]);
            ModelGroup.Children.Add(ContainerObject);
            ShownStepModel.Content = ModelGroup;
            StepSlider.Maximum = Packages.Count();
            StepSlider.Minimum = 1;
            StepSlider.IsSnapToTickEnabled = true;
            StepSlider.TickFrequency = 1;
        }

        private void StepListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StepSlider.Value = StepListBox.Items.IndexOf(e.AddedItems[0]);
        }

        private void StepSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            while (ShownStep < e.NewValue)
            {
                ShownStep++;
                ModelGroup.Children.Add(AllPackageVisuals[ShownStep - 1]);
                ModelGroup.Children.Remove(ContainerObject);
                ModelGroup.Children.Add(ContainerObject);
            }
            while (ShownStep > e.NewValue)
            {
                ModelGroup.Children.RemoveAt(ShownStep - 1);
                ModelGroup.Children.Remove(ContainerObject);
                ModelGroup.Children.Add(ContainerObject);
                ShownStep--;
            }
        }

        private void DistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void VerticalAngleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void HorizontalAngleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

       
    }
}
