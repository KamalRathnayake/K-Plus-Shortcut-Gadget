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
    public static class ExtensionMethods
    {
        public static string LastSlashContent(this MenuItem item)
        {
            return item.Text.Split('\\').Last();
        }
    }
    public class AppEngine
    {
        private Form1 handle;
        private WindowsLinkManager linkManager;
        public AppEngine(Form1 handle)
        {
            this.handle = handle;
        }
        public void Initialize()
        {

            handle.serializationEngine = new BinarySerializationEngine<BinaryStorage>(SystemUtility.StorageFileInAppDataPath);

            handle.binStorage = BinaryStorage.Get(handle.serializationEngine);
            handle.ShortcutRepository = new BinaryShortcutsRepository(handle.binStorage);
            handle.SettingsRepository = new BinarySettingsRepository(handle.binStorage);


            //IShortcutRepository srep = new XMLShortcutRepository();
            //srep.Shortcuts.ToList().ForEach(x => handle.ShortcutRepository.Add(x));

            handle.SystemInformationProvider = new SimpleSystemInformationProvider();
            handle.PanelProcessingManager = new SimplePanelProcessingManager(handle, new SimpleDriveDetector(1000), handle.SettingsRepository);
            handle.UIArranger = new SimpleUIArranger(handle);
            handle.effects = new Effects(handle);
            handle.Panels = new List<Panel>();
            handle.startupShortcut = StartupShortcut.Get();

            handle.Panels.Add(handle.main_panel_1);
            handle.Panels.Add(handle.main_panel_2);
            handle.Top = 0;
            handle.Left = 0;

            handle.UIArranger.StartingArrangement();
            ViewShortcuts();
            handle.pictureBox1.Cursor = Cursors.Hand;
            handle.CreateGlobalMousePositionListenerThread();

            Color bgColor = Color.FromArgb(int.Parse(handle.SettingsRepository.Receive("APP_BACKGROUND_RED").Value), int.Parse(handle.SettingsRepository.Receive("APP_BACKGROUND_GREEN").Value), int.Parse(handle.SettingsRepository.Receive("APP_BACKGROUND_BLUE").Value));
            handle.BackColor = bgColor;

            //Initializing Link Manager
            linkManager = new WindowsLinkManager(handle);
            linkManager.Initialize();
        }

        public void BindEvents()
        {
            //RIGHT CLICK CONTEXT MENU EVENTS
            MenuItem[] menuitems = new MenuItem[3];
            menuitems[0] = new MenuItem("Close", (s, x) => { handle.Close(); });
            Func<bool, string> startupMessage = (b) => { return b ? "Enable : Run on Startup" : "Disable : Run on Startup"; };
            menuitems[1] = new MenuItem(startupMessage(!handle.startupShortcut.Exists()), (s, ev) =>
            {
                var thismenu = (MenuItem)s;
                if (handle.startupShortcut.Exists()) handle.startupShortcut.Remove();
                else handle.startupShortcut.Create();
                thismenu.Text = startupMessage(!handle.startupShortcut.Exists());
            });
            menuitems[2] = new MenuItem("About", (s, x) => { Form1.msg("Developer : Kamal Rathnayake 2015 © [kamalrathnayake@outlook.com], [BETA RELEASE]"); });
            handle.ContextMenu = new ContextMenu(menuitems);

            //SETTINGS ICON EVENT
            handle.pictureBox1.MouseClick += (sender1, e1) => { handle.Set = new Settings(handle); handle.Set.ShowDialog(); };

            //PANELS ACTIVATION EVENTS
            if (bool.Parse(handle.SettingsRepository.Receive("PANEL_ACTIVE").Value)) handle.Height = 442;
            else handle.Height = 303;
            
            handle.ActivatePanel(handle.activePanel);
            handle.panel_activator_1.Click += (s, ex) => { handle.activePanel = 0; };
            handle.panel_activator_2.Click += (s, ex) => { handle.activePanel = 1; };
        }

        public void ViewShortcuts()
        {
            int thisHeightControlled = handle.panel2.Height;
            handle.panel2.Controls.Clear();
            for (int k = 0; k < handle.ShortcutRepository.Shortcuts.Count(); k++)
            {

                Model.FolderLink v = null;
                if (bool.Parse(handle.SettingsRepository.Receive("SORT_BY_USAGE").Value))
                    v = handle.ShortcutRepository.Shortcuts.OrderByDescending(x => x.AccessCount).ToList()[k];
                else
                    v = handle.ShortcutRepository.Shortcuts.ToList()[k];

                FolderLinkButton btn = new FolderLinkButton() { Text = v.Name.ToUpper().Substring(0,(v.Name.Length>16)?16:v.Name.Length) };
                btn.MouseDown += (sender, ev) =>
                {
                    if(ev.Button==MouseButtons.Right)
                        try
                        {
                            btn.ContextMenu = GetContextMenu(v.ExplorerLink);
                            btn.ContextMenu.Show(btn, new Point(btn.Width,btn.Height),LeftRightAlignment.Right);
                        }
                        catch (Exception ex)
                        {
                            btn.BackColor = Color.Red;
                        }
                };
                btn.MouseClick += (sender, e) =>
                {
                    if (Directory.Exists(v.ExplorerLink))
                    {
                        System.Diagnostics.Process.Start(@"C:\Windows\explorer.exe", v.ExplorerLink); if (handle.state == AppState.JustOpened || handle.state == AppState.MouseOKFadedInNotClicked) handle.state = AppState.ClickedALinkNotFaded; handle.effects.FadeOut(handle.FadingMilli); if (handle.state == AppState.ClickedALinkNotFaded || handle.state == AppState.MouseNotOKFadedOutNotClicked) handle.state = AppState.ClickedALinkFaded;
                        FolderLink shortcutx = handle.ShortcutRepository.Shortcuts.Where(x => x.ID == v.ID).First();
                        shortcutx.AccessCount++;
                        handle.ShortcutRepository.Update(shortcutx);
                    }
                    else Form1.msg("The Directory doesn't Exists!", MessageBoxIcon.Exclamation);
                };

                btn.MouseEnter += (sender, e) => {
                    try
                    {
                        FolderLink shortcutx = handle.ShortcutRepository.Shortcuts.Where(x => x.ID == v.ID).First();
                        int ac = shortcutx.AccessCount;
                        handle.label3.Text = ((shortcutx.AccessCount == 0) ? "NO" : "" + (((shortcutx.AccessCount < 10 && shortcutx.AccessCount >= 1) ? "0" + shortcutx.AccessCount : shortcutx.AccessCount.ToString()))) + " ACCESSES"; 
                        handle.label4.Text = shortcutx.Name; 
                        handle.label5.Text = shortcutx.ExplorerLink;

                        int dircount = Directory.EnumerateDirectories(shortcutx.ExplorerLink).Count();
                        int filecount = Directory.EnumerateFiles(shortcutx.ExplorerLink).Count();

                        handle.folder_count_lbl.Text = ((dircount == 0) ? "NO" : (((dircount < 10 && dircount >= 1) ? "0" + dircount : dircount.ToString()))) + " FOLDERS";
                        handle.file_count_lbl.Text = ((filecount == 0) ? "NO" : (((filecount < 10 && filecount >= 1) ? "0" + filecount : filecount.ToString()))) + " FILES"; 
                    }
                    catch (Exception ex)
                    {
                        btn.BackColor = Color.Red;
                    }
                };

                btn.Left = 5 + ((k % 2 == 0) ? 0 : 120);
                int scount = handle.ShortcutRepository.Shortcuts.Count();
                if (scount % 2 == 1) scount++;
                scount = (scount == 0 || scount == 1) ? 2 : scount;
                btn.Height = (thisHeightControlled - 30) / ((scount == 0) ? 1 : scount / 2);
                btn.Top = 20 + (thisHeightControlled - 30) / (scount / 2) * (int)Math.Floor(k / (double)2);
                btn.Width = 120;
                btn.BackColor = Color.Transparent;
                handle.panel2.Controls.Add(btn);

            }
        }

        private ContextMenu GetContextMenu(string p)
        {
            ContextMenu re = new ContextMenu();
            List<MenuItem> re_menus = new List<MenuItem>();
            foreach (var v in AddAttributes(ItemsLastAccessSorted(p)))
            {
                var mi = new MenuItem(v.Split('>', '|').First().Split('\\').Last() + " > " + v.Split('>', '|').Last(), (s, e) => { System.Diagnostics.Process.Start("explorer.exe", v.Split('>', '|').First()); });
                re_menus.Add(mi);
            }
            re.MenuItems.AddRange(re_menus.ToArray());
            MenuItem removeItem = new MenuItem();
            removeItem.Text = "Remove This";
            removeItem.Click += (xn, ev) => { /*if (Form1.msgask("Are you sure you want this removed?") == DialogResult.Yes)*/ { handle.ShortcutRepository.Remove(handle.ShortcutRepository.Shortcuts.Where(x => x.ExplorerLink.ToLower() == p.ToLower()).First().ID); ViewShortcuts();  } };
            re.MenuItems.Add(removeItem);
            return re;
        }
        private List<string> AddAttributes(List<string> paths)
        {
            List<string> re = new List<string>();

            for (int k = 0; k < paths.Count; k++)
            {
                var v = paths[k];
                if (Directory.Exists(v))
                {
                    string infostr = "";
                    infostr += (Directory.EnumerateDirectories(v).Count() + Directory.EnumerateFiles(v).Count())+ " Items Inside";
                    infostr += "";
                    paths[k] += " > " + infostr;
                }
                else if (File.Exists(v))
                {
                    string infostr = "";
                    infostr += (new FileInfo(v).Length)/(1024*1024)+" MB";
                    infostr += "";
                    paths[k] += " | " + infostr;
                }
            }
            return paths.ToList() ;
        }

        private List<string> ItemsLastAccessSorted(string path)
        {
            List<string> re = new List<string>();

            Directory.GetDirectories(path).ToList().ForEach(x => re.Add(x));
            Directory.GetFiles(path).ToList().ForEach(x => re.Add(x));
            Dictionary<string, DateTime> items_sortlist = new Dictionary<string, DateTime>();
            foreach (var v in re)
                if (File.Exists(v)) items_sortlist.Add(v, File.GetLastWriteTime(v));
                else items_sortlist.Add(v, Directory.GetLastWriteTime(v));
            var sorted = items_sortlist.OrderByDescending(x => x.Value);
            return sorted.Select(x => x.Key).ToList();
        }
    }
}
