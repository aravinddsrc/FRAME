using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Windows.Media.Imaging;

namespace FrameWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Item _ob = new Item();
        private List<string> _getStudentList;
        private string _mode;
        public string Pass { get; set; }
        public MainWindow()
        {
            InitializeComponent();



            //Spinner.Visibility = Visibility.Collapsed;
            SingleTextbox.Visibility = Visibility.Collapsed;
            Number.Visibility = Visibility.Collapsed;
            //Background = Brushes.WhiteSmoke;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            pbStatus.Visibility = Visibility.Collapsed;
            pbStatusSub.Visibility = Visibility.Collapsed;
            _ob.CommonList = Utility.ExcelValues.Column();
            var name = _ob.CommonList.Select(o => o.NameModel).Distinct();
            ClassName.ItemsSource = name;
            _mode = "Class";



            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (appPath == null) return;
            var configFile = Path.Combine(appPath, "Frame.exe.config");
            var configFileMaps = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            var configs = ConfigurationManager.OpenMappedExeConfiguration(configFileMaps, ConfigurationUserLevel.None);
            var val = configs.AppSettings.Settings["P"].Value;
            var obs = new Utility.Crypto();
            Pass = obs.Decrypt(val);
        }




        private void ClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _getStudentList = null;
            pbStatus.Visibility = Visibility.Collapsed;
            pbStatusSub.Visibility = Visibility.Collapsed;
            var clsNanme = ClassName.SelectedItem.ToString();
            _getStudentList = clsNanme == "Select All" ? _ob.CommonList.Select(o => o.NumberModel).ToList() : _ob.CommonList.Where(p => p.NameModel == clsNanme).Select(o => o.NumberModel).ToList();
            label.Content = _getStudentList.Count + " Members";
        }

        private bool IsCancel = false;

        private void Send_Click(object sender, RoutedEventArgs e)
        {
           




            //System.Diagnostics.Process.Start(@"C:\Stackapps\Data.xlsx");
            if (_mode != "Class")
            {
                if (_mode == "Manual" && SingleTextbox.Text == "")
                {
                    MessageBox.Show("Please enter Number");
                    return;
                }
                var number = SingleTextbox.Text;
                var numberWithoutspace = Regex.Replace(number, @"\s+", "");
                var numbers = numberWithoutspace.Split(';');
                _getStudentList = null;
                _getStudentList = numbers.ToList();
            }



            if (msgbox.Text == "")
            {
                MessageBox.Show("Please enter Message");
                return;
            }





            //if (Send.Content.ToString() == "Cancel")
            //{
            //    Process.Start(Application.ResourceAssembly.Location);
            //    Application.Current.Shutdown();
            //}

            //Send.Content = "Cancel";

            if (IsCancel == true)
            {
                IsCancel = false;
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            IsCancel = true;
            updateimage.Source = new BitmapImage(new Uri(@"C:\Users\aravind.a\Documents\Visual Studio 2015\Projects\Frame\FrameWPF\CANCEL01.png"));




            Dispatcher.Invoke(() =>
            {
                textBox.Text = "";
            });
            pbStatus.Visibility = Visibility.Visible;
            pbStatusSub.Visibility = Visibility.Visible;
            //Spinner.Visibility = Visibility.Visible;

            var worker = new BackgroundWorker { WorkerReportsProgress = true };
            worker.DoWork += RequestApi;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void RequestApi(object sender, DoWorkEventArgs e)
        {
            try
            {
                var pBarcount = (double)100 / _getStudentList.Count;

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                for (var i = 0; i < _getStudentList.Count; i++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        textBox.Text = _getStudentList[i] + " " + textBox.Text;

                    });
                    var provalue = (i + 1) * (int)pBarcount;

                    (sender as BackgroundWorker)?.ReportProgress(provalue);
                    var url = "https://stackapps.000webhostapp.com/Send.php?uid=8148681001&&pwd=" + Pass + "&&phone=" + _getStudentList[i] + "&&msg=" + DateTime.Now + "";
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    try
                    {
                        var response = request.GetResponse();
                        using (var responseStream = response.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                var reader = new StreamReader(responseStream, Encoding.UTF8);
                                reader.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        var errorResponse = ex.Response;
                        using (var responseStream = errorResponse.GetResponseStream())
                        {
                            if (responseStream == null) throw;
                            var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                            reader.ReadToEnd();
                        }
                        throw;
                    }
                }
                (sender as BackgroundWorker)?.ReportProgress(100);
                stopwatch.Stop();
                Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", stopwatch.Elapsed);
                Console.ReadLine();
                this.Dispatcher.Invoke(() =>
                {
                    textBox.Text = "Completed";
                    pbStatus.Visibility = Visibility.Collapsed;
                    pbStatusSub.Visibility = Visibility.Collapsed;
                    //Spinner.Visibility = Visibility.Collapsed;
                    //Send.Content = "Send";
                    //var bc = new BrushConverter();
                    //Send.Background = (Brush)bc.ConvertFrom("#10BF7A");
                    updateimage.Source = new BitmapImage(new Uri(@"C:\Users\aravind.a\Documents\Visual Studio 2015\Projects\Frame\FrameWPF\SEND01.png"));
                    IsCancel = false;

                });
            }
            catch (AggregateException)
            {
            }
        }

        private int charcount = 0;
        public void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

            charcount = 160 - msgbox.Text.Length;
            if (charcount < 0)
            {
                MessageBox.Show("Maximum Character reached!");
                return;
            }
            Charctercount.Foreground = charcount < 50 ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
            Charctercount.Text = charcount + " Remaining";



        }

        private void Single_Click(object sender, RoutedEventArgs e)
        {
            Single.IsEnabled = false;
            Byclass.IsEnabled = true;
            SingleTextbox.Visibility = Visibility.Visible;
            Number.Visibility = Visibility.Visible;
            ClassName.Visibility = Visibility.Collapsed;
            label.Visibility = Visibility.Collapsed;
            _mode = "Manual";
        }

        private void Byclass_Click(object sender, RoutedEventArgs e)
        {
            Single.IsEnabled = true;
            Byclass.IsEnabled = false;
            label.Visibility = Visibility.Visible;
            ClassName.Visibility = Visibility.Visible;
            SingleTextbox.Visibility = Visibility.Collapsed;
            Number.Visibility = Visibility.Collapsed;
            _mode = "Class";
        }
        private void Spinner_Ended(object sender, EventArgs e)
        {
            //Spinner.Position = TimeSpan.Zero;
            //Spinner.Play();
        }

        private void Spinner_MediaEnded(object sender, RoutedEventArgs e)
        {
            //Spinner.Position = new TimeSpan(0, 0, 1);
            //Spinner.Play();
        }
        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var p = AppDomain.CurrentDomain.BaseDirectory;



                var path1 = p + @"\Data.xlsx";

                Process.Start(path1);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Environment.Exit(0);

            }
            catch (Exception)
            {
                System.Windows.Application.Current.Shutdown();

            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }

    public class Item
    {
        public List<Utility.ColumnName> CommonList { get; set; }
    }
}


