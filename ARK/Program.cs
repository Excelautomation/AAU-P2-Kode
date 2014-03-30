using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ARK
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Window window;

            if (args.Select(e => e.ToLower()).Contains("administrationssystem"))
            {
                window = new Administrationssystem();
            }
            else if (args.Select(e => e.ToLower()).Contains("protokolsystem"))
            {
                window = new Protokolsystem();
            }
            else
            {
                window = new Administrationssystem();
            }

            var app = new Application();
            app.Run(window);
        }
    }
}
