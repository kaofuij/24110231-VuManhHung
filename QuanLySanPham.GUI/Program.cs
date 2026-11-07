using System;
using System.Windows.Forms;

namespace QuanLySanPham.GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormQuanLySanPham());
        }
    }
}
