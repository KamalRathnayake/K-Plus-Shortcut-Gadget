using ShortcutGadget.Model;
using ShortcutGadget.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        static void Main(string[] args)
        {
            if (args.Length == 0)
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
            else
            {
                if (args[0].ToLower() == "changedrive"){
                    Console.WriteLine("chan");
                    var bsto=BinaryStorage.Get(new BinarySerializationEngine<BinaryStorage>(SystemUtility.StorageFileInAppDataPath));
                    var bin=new BinaryShortcutsRepository(bsto);
                    ArgumentHandler.ChangeDriveOfContent(bin, args[1][0], args[2][0]);
                }
            }
        }
    }
    public class ArgumentHandler
    {
        public static void ChangeDriveOfContent(IShortcutRepository repo, char currentDriveLetter, char newDriveLetter)
        {
            foreach (var v in repo.Shortcuts)
            {
                if (v.ExplorerLink.ToLower().StartsWith(currentDriveLetter.ToString().ToLower()+ ""))
                {
                    StringBuilder sb = new StringBuilder(v.ExplorerLink);
                    sb[0] = newDriveLetter.ToString().ToUpper()[0];
                    Console.WriteLine("Changing '{0}'\n\tTo '{1}'", v.ExplorerLink, sb.ToString());
                    v.ExplorerLink = sb.ToString();
                    repo.Update(v);
                }
            }
            Console.WriteLine("Done");
        }
    }
}
