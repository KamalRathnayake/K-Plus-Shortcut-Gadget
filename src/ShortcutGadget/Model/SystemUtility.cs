using Microsoft.Win32;
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
    public class SystemUtility
    {
        private static bool initialized = false;
        public static string DirectoryInAppDataPath
        {
            get
            {
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appdata, "KPLUS_SHORTCUT_GADGET");
            }
        }
        public static string StorageFileInAppDataPath
        {
            get
            {
#if DEBUG
                return "storage.bin";
#else
                return Path.Combine(DirectoryInAppDataPath, "storage.bin");
#endif
            }
        }

        public static void InitializeApplication()
        {
            if (!Directory.Exists(DirectoryInAppDataPath))
            {
                Directory.CreateDirectory(DirectoryInAppDataPath);
                SystemUtility.RegisterInStartup(true);
            }
            if (!File.Exists(StorageFileInAppDataPath)) File.Create(StorageFileInAppDataPath);
            initialized = true;
        }

        public static void RegisterInStartup(bool isChecked)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
            {
                registryKey.SetValue("KPlus_Shortcut_Gadget", Application.ExecutablePath);
            }
            else
            {
                registryKey.DeleteValue("KPlus_Shortcut_Gadget");
            }
        }

        public static void Catch(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Form1.msg(ex.Message);
            }
        }
    }
}
