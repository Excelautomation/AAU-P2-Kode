﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                window = new ARK.Administrationssystem.AdminSystem();
            }
            else if (args.Select(e => e.ToLower()).Contains("protokolsystem"))
            {
                window = new Protokolsystem.Protokolsystem();
            }
            else
            {
                var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
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

            var app = new Application();
            app.Run(window);
        }
    }
}
