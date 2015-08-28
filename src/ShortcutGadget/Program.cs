using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutGadget
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                Form1.msg("Ooops! Something is not right!. If this error persists try reinstalling the app. You are welcome to send feedbacks for this app to [kamalrathnayake@outlook.com]. +By the way the error is : : : : " + ex.Message, MessageBoxIcon.Error);
            }
        }
    }
}
