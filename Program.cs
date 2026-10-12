using System;
using System.Windows.Forms;

namespace Cau1_TiemTapHoa
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormHangHoa());
        }
    }
}