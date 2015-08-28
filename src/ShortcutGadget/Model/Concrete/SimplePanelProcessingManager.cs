using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutGadget.Model
{

    public class SimplePanelProcessingManager:IPanelProcessingManager
    {
        Form1 handle;
        IDriveDetector DriveDetector;
        ISettingsRepository SettingsRepository;
        public SimplePanelProcessingManager(Form1 handle,IDriveDetector DriveDetector,ISettingsRepository SettingsRepository)
        {
            this.handle = handle;
            this.DriveDetector=DriveDetector;
            this.SettingsRepository = SettingsRepository;
        }
        public void ActivatePanel(int id)
        {
            handle.SettingsRepository.Update(new Setting() { Key = "PANEL_TYPE", Value = id + "" });
            for (int k = 0; k < handle.Panels.Count; k++)
                if (k != id) handle.Panels[k].Visible = false;
                else handle.Panels[k].Visible = true;

                start_panel_0_processing();

                start_panel_3_processing();
        }

        public void start_panel_0_processing()
        {
            Task.Run(new Action(() =>
            {
                while (true)
                {
                    handle.label1.Invoke(new Action(() =>
                    {
                        //label1.Text = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        handle.label1.Text = (DateTime.Now.TimeOfDay + "").Substring(0, 8);
                    }));
                    handle.label2.Invoke(new Action(() =>
                    {
                        handle.label2.Text = DateTime.Today.ToLongDateString() + "";
                    }));
                    Thread.Sleep(1000);
                    if (handle.activePanel != 1) break;
                }
            }));
        }

        public void start_panel_3_processing()
        {

            DriveInsertedEvent inserted = null;
            DriveRemovedEvent removed = null;
            List<string> drives = new List<string>();
            inserted += (s) =>
            {
                drives.Add(s);
                ViewIcons(drives);
            };
            removed += (s) =>
            {
                drives.Remove(s);
                ViewIcons(drives);
            };
            DriveDetector.StartScanning(inserted, removed, bool.Parse(SettingsRepository.Receive("PANEL_DRIVE_INFO_VIEW_FIXED").Value));
        }
        private void ViewIcons(List<string> drives)
        {
        }
    }

    class DriveIcon : Panel
    {
        public DriveIcon(string DriveName, string DriveLetter, string IconString)
        {
            Color mouseLeftColor = Color.Black;
            Color mouseOverColor = Color.FromArgb(82, 105, 215);
            Width = 64; Height = 64; BackColor = mouseLeftColor;
            Cursor = Cursors.Hand;
            MouseEnter += (s, e) => { BackColor = mouseOverColor; };
            MouseLeave += (s, e) => { BackColor = mouseLeftColor; };
            MouseUp += (s, e) => { System.Diagnostics.Process.Start("explorer.exe", DriveLetter); };
            Label lbl = new Label() { Font = new Font("segoe ui", 9), Top = 45, Left = 4, Text = DriveName + " " + DriveLetter, ForeColor = Color.White, };
            lbl.MouseEnter += (s, e) => { this.BackColor = mouseOverColor; };
            lbl.MouseUp += (s, e) => { System.Diagnostics.Process.Start("explorer.exe", DriveLetter); };
            Controls.Add(lbl);

            PictureBox iconBox = new PictureBox() { Top = 16, Left = 20, Width = 24, Height = 24 };
            iconBox.Image = Image.FromFile(@"Ico/USB.png");
            Controls.Add(iconBox);
        }
    }
}
