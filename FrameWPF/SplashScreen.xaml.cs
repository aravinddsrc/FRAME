using System;
using System.Windows;

namespace FrameWPF
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int count = 0;
        public SplashScreen()
        {
            InitializeComponent();
            Log.Info("SplashScreen Initialized");
        }

        private void Splash_MediaEnded(object sender, RoutedEventArgs e)
        {
            Log.Info("Callactivation");
            Callactivation();
        }

        private void Callactivation()
        {
            
            try
            {
                var obj = new Utility.CheckLicense();

                var ismac = obj.mac();
                if (ismac)
                {
                    var trig = new MainWindow();
                    Log.Info("MainWindow");
                    trig.Show();
                    Close();
                }
                else
                {
                    var trig = new Activation();
                    Log.Info("Activation");
                    trig.Show();
                    Close();
                }
            }
            catch (Exception e)
            {
                Log.Info(e.Message);
                throw;
            }
           
        }

    }
}
