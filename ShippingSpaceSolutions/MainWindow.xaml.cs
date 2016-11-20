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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool Set = false;
        public MainWindow()
        {
            InitializeComponent();
            LaunchMenu Start = new LaunchMenu(this);
            MainContent.Content = Start;
        }

        public void SetContent(UserControl Content)
        {
            MainContent.Content = Content;
            if (WindowState != System.Windows.WindowState.Maximized && !Set)
            {
                WindowState = System.Windows.WindowState.Maximized;
                Set = true;
            }
        }
    }
}
