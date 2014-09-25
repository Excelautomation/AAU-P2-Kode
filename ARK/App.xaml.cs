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

using System.Net.Mail;

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
            this.StartupUri = new Uri("/View/Administrationssystem/AdminLogin.xaml", UriKind.Relative);
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

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Der opstod en fejl. Hvis fejlen er gentagende, ring venligst til Mads Gadeberg på 53552125 eller Mads Edelskjold på 3154013", "Kritisk fejl", MessageBoxButton.OK);
            
            MailService.sendUnhandledException(e);

            System.Windows.Forms.Application.Restart();

            Application.Current.Shutdown();
                //e.Exception.
        }
    }
}