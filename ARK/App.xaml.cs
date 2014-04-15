using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ARK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var thr = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    Debug.WriteLine("Mads er noob");
                    Thread.Sleep(1000);
                }
            }));
            thr.Start();
        }
    }
}
