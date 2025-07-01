using System;
using System.Windows.Forms;
using Hnefatafl.Views;

namespace Hnefatafl
{
    /// <summary>
    /// Entry point of the Hnefatafl application.
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
