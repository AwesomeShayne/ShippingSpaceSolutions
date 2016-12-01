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
        internal Container LoadedContainer = new Container(480, 114, 96, 480, 114, 96, 0, 0);
        private PerspectiveCamera myPerspectiveCamera = new PerspectiveCamera();
        private int CameraDistance = 600;
        private int HorizontalAngle = 315;
        private int VerticalAngle = 45;
        private List<Package> Packages = new List<Package>();
        private Model3DGroup ModelGroup = new Model3DGroup();
        private GeometryModel3D ContainerObject;
        private List<GeometryModel3D> AllPackageVisuals = new List<GeometryModel3D>();
        private int ShownStep = 1;
        private Random Rand = new Random();

        public OutputView(Container container)
        {
            InitializeComponent();
            ContainerObject = LoadedContainer.GetModel(Rand);
            LoadedContainer = container;
            Packages = LoadedContainer.Packages;
            foreach (Package box in Packages.ToArray())
            {
                StepListBox.Items.Add(string.Concat("Load ", box.Name, " at the shown location"));
                AllPackageVisuals.Add(box.GetModel(Rand));
            }
            ModelGroup.Children.Add(AllPackageVisuals[0]);
            ModelGroup.Children.Add(ContainerObject);
            ShownStepModel.Content = ModelGroup;
            StepSlider.Maximum = Packages.Count();
            StepSlider.Minimum = 1;
            StepSlider.IsSnapToTickEnabled = true;
            StepSlider.TickFrequency = 1;


            HorizontalAngleSlider.Value = HorizontalAngle;
            VerticalAngleSlider.Value = VerticalAngle;
            DistanceSlider.Value = CameraDistance;

            myPerspectiveCamera.FieldOfView = 60;

            ViewArea.Camera = myPerspectiveCamera;

            SetCameraLocation();
        }

        public OutputView(List<Package> _Packages)
        {
            Packages.AddRange(_Packages);
            ContainerObject = LoadedContainer.GetModel(Rand);
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
                AllPackageVisuals.Add(box.GetModel(Rand));
            }
            ModelGroup.Children.Add(AllPackageVisuals[0]);
            ModelGroup.Children.Add(ContainerObject);
            ShownStepModel.Content = ModelGroup;
            StepSlider.Maximum = Packages.Count();
            StepSlider.Minimum = 1;
            StepSlider.IsSnapToTickEnabled = true;
            StepSlider.TickFrequency = 1;


            HorizontalAngleSlider.Value = HorizontalAngle;
            VerticalAngleSlider.Value = VerticalAngle;
            DistanceSlider.Value = CameraDistance;

            myPerspectiveCamera.FieldOfView = 60;

            ViewArea.Camera = myPerspectiveCamera;

            SetCameraLocation();
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

        private void SetCameraLocation()
        {
            double vRadians = ((float)VerticalAngle / 180) * (Math.PI);
            double hRadians = ((float)HorizontalAngle / 180) * (Math.PI);
            var locationX = CameraDistance * Math.Sin(vRadians) * Math.Cos(hRadians);
            var locationZ = CameraDistance * Math.Sin(vRadians) * Math.Sin(hRadians);
            var locationY = CameraDistance * Math.Cos(vRadians);

            myPerspectiveCamera.Position = new Point3D(locationX, locationY, locationZ);
            myPerspectiveCamera.LookDirection = new Vector3D(-locationX, -locationY, -locationZ);
        }

        private void DistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CameraDistance = (int)e.NewValue;
            SetCameraLocation();
        }

        private void VerticalAngleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VerticalAngle = (int)e.NewValue;
            SetCameraLocation();
        }

        private void HorizontalAngleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            HorizontalAngle = (int)e.NewValue;
            SetCameraLocation();
        }

        private void ViewArea_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ViewArea_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ViewArea_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int newCamDist = CameraDistance - e.Delta;
            if (newCamDist > DistanceSlider.Maximum)
            {
                newCamDist = (int)DistanceSlider.Maximum;
            }
            if (newCamDist < DistanceSlider.Minimum)
            {
                newCamDist = (int)DistanceSlider.Minimum;
            }
            CameraDistance = newCamDist;
            DistanceSlider.Value = CameraDistance;
            SetCameraLocation();
        }
    }
}
