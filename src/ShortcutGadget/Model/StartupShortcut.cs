using ShortcutGadget.Intel;
using ShortcutGadget.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutGadget.Model
{
    public class StartupShortcut
    {
        private static StartupShortcut shortcut;
        private string startupFolder;
        private string assemblyShortcut;
        private string shortcutName;
        private StartupShortcut()
        {
            startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            assemblyShortcut = Assembly.GetExecutingAssembly().Location + " Shortcut.lnk";
            shortcutName = new FileInfo(assemblyShortcut).Name;
        }
        public static StartupShortcut Get()
        {
            if (shortcut == null) shortcut = new StartupShortcut();
            return shortcut;
        }
        public bool Exists()
        {
            return File.Exists(Path.Combine(startupFolder, shortcutName));
        }
        public bool Create()
        {
            try
            {
                File.Copy(assemblyShortcut, Path.Combine(startupFolder,shortcutName));
                return true;
            }
            catch (Exception ex) { Form1.msg(ex.Message); return false; }
        }
        public bool Remove()
        {
            try
            {
                if (Exists()) File.Delete(Path.Combine(startupFolder, shortcutName));
                return true;
            }
            catch(Exception ex) { Form1.msg(ex.Message);return false; }
        }
    }
}
