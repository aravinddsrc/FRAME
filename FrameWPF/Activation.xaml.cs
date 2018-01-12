using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;

namespace FrameWPF
{
    /// <summary>
    /// Interaction logic for Activation.xaml
    /// </summary>
    public partial class Activation
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Activation()
        {
            var obj = new Utility.CheckLicense();
            var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            var mac = obj.CheckMac(macAddr);
            if (mac)
            {
                var trig = new MainWindow();
                Log.Info("MainWindow");
                trig.Show();
                Close();
            }
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (macAddr != null) textBox.Text = macAddr;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Log.Info("Shutdown");
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
