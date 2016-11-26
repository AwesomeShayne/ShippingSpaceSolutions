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
using System.Configuration;
using MySql.Data.MySqlClient;

namespace ShippingSpaceSolutions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string sshHost = ConfigurationManager.AppSettings["sshHost"];
        static int sshPort = Int32.Parse(ConfigurationManager.AppSettings["sshPort"]);
        static string sshUser = ConfigurationManager.AppSettings["sshUser"];
        static string sshPassword = ConfigurationManager.AppSettings["sshPassword"];
        static string db = ConfigurationManager.AppSettings["db"];
        static string dbUser = ConfigurationManager.AppSettings["dbUser"];
        static string dbPassword = ConfigurationManager.AppSettings["dbPassword"];
        static string cs = @"SERVER=127.0.0.1;PORT=3306;UID=" + dbUser + ";PASSWORD=" + dbPassword + ";DATABASE=" + db; // assemble connectionString

        public SshClient Client;
        public MySqlConnection Connection;

        private bool Set = false;
        public MainWindow()
        {
            Client = new SshClient(sshHost, sshPort, sshUser, sshPassword);
            Client.KeepAliveInterval = new TimeSpan(0, 0, 30); // set interval to keep ssh thread alive.
            Client.ConnectionInfo.Timeout = new TimeSpan(0, 0, 20); // set timeout on trying the ssh connection
            Client.Connect();

            var port = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306); // local IP, local port, remote IP, remote port (need to explicitly state local IP here because program was using the wrong network adapter)
            Client.AddForwardedPort(port);

            port.Start();

            Connection = new MySqlConnection(cs);
            Connection.Open();

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
