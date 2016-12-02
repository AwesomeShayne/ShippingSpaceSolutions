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
using Renci.SshNet;
using MySql.Data.MySqlClient;

namespace ShippingSpaceSolutions
{
    /// <summary>
    /// Interaction logic for ItemEntry.xaml
    /// </summary>
    public partial class ItemEntry : UserControl
    {
        DeviceSelector _Container;
        int number;
        SshClient Client;
        MySqlConnection Connection;
        
        List<String> categories = new List<string>();

        public ItemEntry(DeviceSelector container, int _number)
        {
            
            number = _number;
            _Container = container;

            Client = _Container.parent.Client;
            MySqlConnection conn = _Container.parent.Connection;
            MySqlCommand selDistinctCmd = new MySqlCommand("SELECT DISTINCT category FROM devices ", conn);
            MySqlDataReader rdr = selDistinctCmd.ExecuteReader();
            
            
            InitializeComponent();
            if (rdr != null)
            {
                while (rdr.Read())
                {
                    TypeComboBox.Items.Add(rdr.GetString(0));
                }
                rdr.Close();
            }
        }

        private void TypeComboBox_Selected(object sender, RoutedEventArgs e)
        {
            if (!(TypeComboBox.SelectedValue == null))
            {
                MySqlConnection conn = _Container.parent.Connection;
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT description FROM devices WHERE category =@category", conn);
                cmd.Parameters.AddWithValue("@category", TypeComboBox.SelectedValue);
                MySqlDataReader rdr = cmd.ExecuteReader();
                DescComboBox.Items.Clear();
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        DescComboBox.Items.Add(rdr.GetString(0));
                    }
                    rdr.Close();
                }
            }
        }

        private void DescComboBox_Selected(object sender, RoutedEventArgs e)
        {
            if (!(DescComboBox.SelectedValue == null))
            {
                MySqlConnection conn = _Container.parent.Connection;
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT manufacturer FROM devices WHERE category =@category AND description =@description", conn);
                cmd.Parameters.AddWithValue("@category", TypeComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@description", DescComboBox.SelectedValue);
                MySqlDataReader rdr = cmd.ExecuteReader();
                ManufacturerComboBox.Items.Clear();
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        ManufacturerComboBox.Items.Add(rdr.GetString(0));
                    }
                    rdr.Close();
                }
            }
        }

        private void ManufacturerComboBox_Selected(object sender, RoutedEventArgs e)
        {
            if (!(ManufacturerComboBox.SelectedValue == null))
            {
                MySqlConnection conn = _Container.parent.Connection;
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT model_number FROM devices WHERE category =@category AND description =@description AND manufacturer =@manufacturer", conn);
                cmd.Parameters.AddWithValue("@category", TypeComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@description", DescComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@manufacturer", ManufacturerComboBox.SelectedValue);
                MySqlDataReader rdr = cmd.ExecuteReader();
                ModelComboBox.Items.Clear();
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        ModelComboBox.Items.Add(rdr.GetString(0));
                    }
                    rdr.Close();
                }
            }
        }

        private void ModelComboBox_Selected(object sender, RoutedEventArgs e)
        {
            if (!(ModelComboBox.SelectedValue == null))
            {
                MySqlConnection conn = _Container.parent.Connection;
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT test_status FROM devices WHERE category =@category AND description =@description AND manufacturer =@manufacturer AND model_number =@model", conn);
                cmd.Parameters.AddWithValue("@category", TypeComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@description", DescComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@manufacturer", ManufacturerComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@model", ModelComboBox.SelectedValue);
                MySqlDataReader rdr = cmd.ExecuteReader();
                TestStatusComboBox.Items.Clear();
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        TestStatusComboBox.Items.Add(rdr.GetString(0));
                    }
                    rdr.Close();
                }
            }
        }

        private void TestStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(DescComboBox.SelectedValue == null))
            {
                MySqlConnection conn = _Container.parent.Connection;
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT sos_number FROM devices WHERE category =@category AND description =@description AND manufacturer =@manufacturer AND model_number =@model AND test_status  =@status", conn);
                cmd.Parameters.AddWithValue("@category", TypeComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@description", DescComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@manufacturer", ManufacturerComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@model", ModelComboBox.SelectedValue);
                cmd.Parameters.AddWithValue("@status", TestStatusComboBox.SelectedValue);
                MySqlDataReader rdr = cmd.ExecuteReader();
                NumberComboBox.Items.Clear();
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        NumberComboBox.Items.Add(rdr.GetString(0));
                    }
                    rdr.Close();
                }
            }
        }

        private void NumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestStatusComboBox.SelectedIndex > -1)
            {
                MySqlConnection conn = _Container.parent.Connection;
                MySqlCommand cmd = new MySqlCommand("SELECT dimension_x,dimension_y,dimension_z,orientable_x,orientable_y,orientable_z,stackable_x,stackable_y,stackable_z FROM devices WHERE sos_number =@number", conn);
                cmd.Parameters.AddWithValue("@number", NumberComboBox.SelectedValue);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        WidthTextBox.Text = rdr.GetInt32(0).ToString();
                        HeightTextBox.Text = rdr.GetInt32(1).ToString();
                        DepthTextBox.Text = rdr.GetInt32(2).ToString();
                        OrientableX.IsChecked = rdr.GetBoolean(3);
                        OrientableY.IsChecked = rdr.GetBoolean(4);
                        OrientableZ.IsChecked = rdr.GetBoolean(5);
                        StackableX.IsChecked = rdr.GetBoolean(6);
                        StackableY.IsChecked = rdr.GetBoolean(7);
                        StackableZ.IsChecked = rdr.GetBoolean(8);
                        break;
                    }
                    rdr.Close();
                }
            }
            else
            {
                MySqlConnection conn = _Container.parent.Connection;
                MySqlCommand cmd = new MySqlCommand("SELECT category,description,manufacturer," +
                                                    "       model_number,test_status,dimension_x," +
                                                    "       dimension_y,dimension_z,orientable_x," +
                                                    "       orientable_y,orientable_z,stackable_x," +
                                                    "       stackable_z" +
                                                    " FROM devices" +
                                                    " WHERE sos_number = @number", conn);
                cmd.Parameters.AddWithValue("@number", NumberComboBox.SelectedValue);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        TypeComboBox.Text = rdr.GetString(0);
                        DescComboBox.Text = rdr.GetString(1);
                        ManufacturerComboBox.Text = rdr.GetString(2);
                        ModelComboBox.Text = rdr.GetString(3);
                        TestStatusComboBox.Text = rdr.GetString(4);
                        WidthTextBox.Text = rdr.GetInt32(5).ToString();
                        HeightTextBox.Text = rdr.GetInt32(6).ToString();
                        DepthTextBox.Text = rdr.GetInt32(7).ToString();
                        OrientableX.IsChecked = rdr.GetBoolean(8);
                        OrientableY.IsChecked = rdr.GetBoolean(9);
                        OrientableZ.IsChecked = rdr.GetBoolean(10);
                        StackableX.IsChecked = rdr.GetBoolean(11);
                        StackableY.IsChecked = rdr.GetBoolean(12);
                        StackableZ.IsChecked = rdr.GetBoolean(13);
                        break;
                    }
                }
            }

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
                {
                    if (DescComboBox.SelectedIndex > -1)
                        output.Add(new MedicalDevice(DescComboBox.SelectedValue.ToString(), Int32.Parse(WidthTextBox.Text), Int32.Parse(HeightTextBox.Text), Int32.Parse(DepthTextBox.Text),
                                                                                            (bool)OrientableX.IsChecked, (bool)OrientableY.IsChecked, (bool)OrientableZ.IsChecked,
                                                                                            (bool)StackableX.IsChecked, (bool)StackableY.IsChecked, (bool)StackableZ.IsChecked));
                    else
                        output.Add(new MedicalDevice(Int32.Parse(WidthTextBox.Text), Int32.Parse(HeightTextBox.Text), Int32.Parse(DepthTextBox.Text)));
                }
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

        private void UpdateDBButton_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = _Container.parent.Connection;
            MySqlCommand cmd = new MySqlCommand("UPDATE devices SET dimension_x=@dimx, "+
                                                "       dimension_y=@dimy,dimension_z=@dimz, "+
                                                "       orientable_x=@orx,orientable_y=@ory, "+
                                                "       orientable_z=@orz,stackable_x=@stx, "+
                                                "       stackable_y=@sty,stackable_z=@stz "+
                                                "WHERE sos_number = @sosnumber");
            cmd.ExecuteNonQuery();

        }

    }
}
