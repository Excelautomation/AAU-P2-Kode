using System;
using System.Linq;
using System.Security.Principal;
using System.Windows;
using ARK.Administrationssystem;

namespace ARK
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Window window;

            if (args.Select(e => e.ToLower()).Contains("administrationssystem"))
            {
                window = new AdminSystem();
            }
            else if (args.Select(e => e.ToLower()).Contains("protokolsystem"))
            {
                window = new Protokolsystem.Protokolsystem();
            }
            else
            {
                WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
                if (windowsIdentity != null && windowsIdentity.Name == "SAHB-WIN7\\sahb")
                {
                    // window = new ARK.Administrationssystem.Administrationssystem();

                    window = new MainWindow();
                }
                else
                {
                    window = new MainWindow();
                }
            }

            Application app = new Application();
            app.Run(window);
        }
    }
}
