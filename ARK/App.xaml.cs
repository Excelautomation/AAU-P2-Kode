using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using ARK.HelperFunctions;
using ARK.HelperFunctions.SMSGateway;
using ARK.Model;
using ARK.Model.DB;

namespace ARK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
#if RELEASE || DEBUG

            // Start thread which downloads the sunset time every day
            var sunsetTokenSource = new CancellationTokenSource();
            SunsetClass.StartSunsetTask(sunsetTokenSource.Token);

            // Start thread which synchronizes the database with the FTP every hour
            var xmlTokenSource = new CancellationTokenSource();
            XmlSynchronizer.StartXmlSynchronizerTask(xmlTokenSource.Token);

            // Start thread which sends SMS warnings
            var smsWarningTokenSource = new CancellationTokenSource();
            SmsWarnings.RunTask(smsWarningTokenSource.Token);

            // Start thread which checks if a new season needs to be started
            var seasonTokenSource = new CancellationTokenSource();
            SeasonClass.StartCheckCurrentSeasonEndTask(seasonTokenSource.Token);

            Current.Exit += (sender, e) =>
            {
                sunsetTokenSource.Cancel();
                xmlTokenSource.Cancel();
                smsWarningTokenSource.Cancel();
                seasonTokenSource.Cancel();
            };
#endif

            // Thread that checks if a new season needs to be started
            /*var checkForNewSeasonThread = new Thread(
                () =>
                    {
                        while (true)
                        {
                            DateTime tomorrowAtTree = new DateTime(
                                DateTime.Now.Year, 
                                DateTime.Now.Month, 
                                DateTime.Now.Day, 
                                3, 
                                00, 
                                00);

                            tomorrowAtTree = tomorrowAtTree.AddDays(1);

                            TimeSpan TimeToTreeOCloc = tomorrowAtTree - DateTime.Now;

                            try
                            {
                                Thread.Sleep((int)TimeToTreeOCloc.TotalMilliseconds); // sleep until 3.00 hours
                            }
                            catch (ThreadInterruptedException)
                            {
                                break;
                            }

                            CheckCurrentSeasonEnd();
                        }
                    });
            checkForNewSeasonThread.Start();*/

            Current.ShutdownMode = System.Windows.ShutdownMode.OnLastWindowClose;
        }

        public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            EventManager.RegisterClassHandler(
                typeof(DatePicker), 
                DatePicker.LoadedEvent, 
                new RoutedEventHandler(DatePicker_Loaded));
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if AdministrationsSystem
            this.StartupUri = new Uri("/View/Administrationssystem/AdminSystem.xaml", UriKind.Relative);
#else
    #if DEBUG
            StartupUri = new Uri("/MainWindow.xaml", UriKind.Relative);
    #else
            this.StartupUri = new Uri("/View/Protokolsystem/ProtocolSystem.xaml", UriKind.Relative);
    #endif
#endif
        }

        // Opdater Watermark
        // http://matthamilton.net/datepicker-watermark
        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var dp = sender as DatePicker;
            if (dp == null)
            {
                return;
            }

            var tb = GetChildOfType<DatePickerTextBox>(dp);
            if (tb == null)
            {
                return;
            }

            var wm = tb.Template.FindName("PART_Watermark", tb) as ContentControl;
            if (wm == null)
            {
                return;
            }

            wm.Content = "Vælg en dato";
        }
    }
}