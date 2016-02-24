using ShortcutGadget.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using ShortcutGadget.Model.Concrete;

namespace ShortcutGadget
{
    public partial class Form1 : Form
    {
        public IShortcutRepository ShortcutRepository;
        public ISettingsRepository SettingsRepository;
        public ISystemInformationProvider SystemInformationProvider;
        public IPanelProcessingManager PanelProcessingManager;
        public IUIArranger UIArranger;
        public List<Panel> Panels;
        public int FadingMilli
        {
            get
            {
                return int.Parse(SettingsRepository.Receive("FADE_IN_OUT_ANIMATION_TIME_MS").Value);
            }
            set
            {
                SettingsRepository.Update(new Setting() { Key = "FADE_IN_OUT_ANIMATION_TIME_MS", Value = value + "" });
            }
        }
        public int activePanel
        {
            get
            {
                return int.Parse(SettingsRepository.Receive("PANEL_TYPE").Value);
            }
            set
            {
                ActivatePanel(value);
            }
        }
        public int MouseUpdateResponsiveness = 170;
        public Effects effects;
        public Settings Set;
        public AppState state = AppState.JustOpened;
        public StartupShortcut startupShortcut;
        public AppEngine engine;
        public BinaryStorage binStorage;
        public BinarySerializationEngine<BinaryStorage> serializationEngine;
        public Form1()
        {
            InitializeComponent();
#if DEBUG 
#else
            SystemUtility.InitializeApplication();
#endif
            engine = new AppEngine(this);
            this.AllowDrop = true;
            this.DragOver += (sn, ev) => {
                string[] files = (string[])ev.Data.GetData(DataFormats.FileDrop);
                if (files.Count() == 0) return;
                string path = files[0];
                if (!Directory.Exists(path)) { msg("The input should be a valid directory", MessageBoxIcon.Exclamation); return; }
                string name = path.Split('\\').Last();
                //if(msgask("Are you sure you want '"+path+"' as a shortcut?")==DialogResult.Yes)
                        ShortcutRepository.Add(new FolderLink() { Name = name, ExplorerLink = path });

                engine.ViewShortcuts();
            };
        }

        public static void msg(object text,MessageBoxIcon icon=MessageBoxIcon.Asterisk)
        {
            MessageBox.Show(text.ToString(), "K+ Shortcut Gadget", MessageBoxButtons.OK, icon);
        }
        public static DialogResult msgask(object text,MessageBoxButtons btns=MessageBoxButtons.YesNo)
        {
            return MessageBox.Show(text.ToString(), "K+ Shortcut Gadget", btns, MessageBoxIcon.Information);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            engine.Initialize();
            engine.BindEvents();
        }

        public bool justOpenedFaded = false;
        public void CreateGlobalMousePositionListenerThread()
        {
            Task.Run(new Action(() => {
                while (true)
                {
                    int x = MousePosition.X; int y = MousePosition.Y;
                    //Console.WriteLine(MousePosition.X + " " + MousePosition.Y);
                    if (state == AppState.ClickedALinkFaded)
                    {
                        if (x < 4 && y < 4)
                        {
                            this.Invoke(new Action(() => { pictureBox2.Visible = false; }));
                            effects.FadeIn(FadingMilli);
                            state = AppState.MouseOKFadedInNotClicked;
                        }
                    }
                    if (state == AppState.MouseOKFadedInNotClicked||state==AppState.JustOpened)
                    {
                        if (x > this.Width || y > this.Height)
                        {
                            if (!justOpenedFaded) { Thread.Sleep(0); justOpenedFaded = true; }
                                effects.FadeOut(FadingMilli);
                            state = AppState.ClickedALinkFaded;
                        }
                    }
                    //Console.WriteLine(state);
                    Thread.Sleep(MouseUpdateResponsiveness);
                }
            }));
        }


        public void AddMenuDirItems(ref MenuItem menuItem)
        {
            MenuItem re=new MenuItem();
            foreach (var v in Directory.EnumerateDirectories(menuItem.Text))
            {
                MenuItem mi = new MenuItem(v, (s, e) => { System.Diagnostics.Process.Start("explorer.exe", v); });
                mi.Select += (s, e) => { AddMenuDirItems(ref mi); };
                re.MenuItems.Add(mi);
            }
            menuItem = re;
        }
        
        public void ActivatePanel(int id)
        {
            PanelProcessingManager.ActivatePanel(id);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void folder_count_lbl_Click(object sender, EventArgs e)
        {

        }
    }

}
