using System;
using System.Windows.Forms;

namespace Cau2_QuanCaPhe
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormQuanCaPhe());
        }
    }
}