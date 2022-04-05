using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleApp
{
    internal static class Program
    {
        private static Form1 form = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();
            Application.Run(new Form1());
        }

        internal static void Log(string msg)
        {
            var f = Form1.ActiveForm;
            (f as Form1)?.Log(msg);
        }
    }
}
