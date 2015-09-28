using Microsoft.Win32;
using ShortcutGadget.Model;
using ShortcutGadget.Model.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutGadget
{
    public partial class Settings : Form
    {
        public IShortcutRepository ShortcutRepository;
        public ISettingsRepository SettingsRepository;
        Form1 frm;
        List<Shortcut> Shortcuts;

        public Settings(Form1 frm1)
        {
            InitializeComponent();
            Shortcuts = new List<Shortcut>();
            frm = frm1;
            ShortcutRepository = frm.ShortcutRepository;
            SettingsRepository = frm.SettingsRepository;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.Left = 350;

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\Windows\\CurrentVersion\\Run", true);
            if (rk.GetValue("KPlus_Shortcut_Gadget") == null)
                checkBox1.Checked = false;
            else checkBox1.Checked = true;

            button1.MouseClick += (s, e1) => { ShortcutView sv = new ShortcutView(new FolderLink() { ID = ShortcutRepository.Shortcuts.Count() },this,SVMode.CreateNew); sv.ShowDialog(); };
            button3.MouseClick += (s, e1) =>
            {
                frm.engine.ViewShortcuts(); label5.Text = "PLEASE RESTART THE APP";
                frm.BackColor = panel1.BackColor;
            };
            trackBar1.Value = frm.FadingMilli;
            label7.Text = trackBar1.Value + "ms";
            trackBar1.Scroll += (s, ev) => { label7.Text = trackBar1.Value + "ms"; frm.FadingMilli = trackBar1.Value; };
            ViewSettings();
        }
        public void ViewSettings()
        {
            listBox1.Items.Clear();
            foreach (var v in ShortcutRepository.Shortcuts) listBox1.Items.Add(v.Name);

            Color bgColor = Color.FromArgb(
                int.Parse(SettingsRepository.Receive("APP_BACKGROUND_RED").Value),
                int.Parse(SettingsRepository.Receive("APP_BACKGROUND_GREEN").Value),
                int.Parse(SettingsRepository.Receive("APP_BACKGROUND_BLUE").Value)
                );
            panel1.BackColor = bgColor;
            checkBox2.Checked = bool.Parse(SettingsRepository.Receive("SORT_BY_USAGE").Value);
            //panels


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         //   MessageBox.Show(ShortcutRepository.Shortcuts.Where(x => x.ID == sendr.SelectedIndex).First().ExplorerLink);
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ListBox sendr = (ListBox)sender;
            //MessageBox.Show(sendr.SelectedIndex+"");
            if (sendr.SelectedIndex > -1)
            {
                label2.Text = ShortcutRepository.Shortcuts.ToList()[sendr.SelectedIndex].ID + "";
                label3.Text = ShortcutRepository.Shortcuts.ToList()[sendr.SelectedIndex].Name + "";
                label4.Text = ShortcutRepository.Shortcuts.ToList()[sendr.SelectedIndex].ExplorerLink + "";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1)
            ShortcutRepository.Remove(ShortcutRepository.Shortcuts.ToList()[listBox1.SelectedIndex].ID);
            ViewSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (checkBox2.Checked)
            {
                SettingsRepository.Update(new Setting() { Key = "SORT_BY_USAGE", Value = "true" });
            }
            else
            {
                SettingsRepository.Update(new Setting() { Key = "SORT_BY_USAGE", Value = "false" });
            }

            frm.engine.ViewShortcuts();

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Windows\explorer.exe", label4.Text);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1.msg(new NotImplementedException().Message);
            return;
            if (listBox1.SelectedIndex > -1)
            {
                ShortcutRepository.MoveUp(ShortcutRepository.Shortcuts.ToList()[listBox1.SelectedIndex]);
                ViewSettings();
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {

            Form1.msg(new NotImplementedException().Message);
            return;
            if (listBox1.SelectedIndex > -1)
                ShortcutRepository.MoveDown(ShortcutRepository.Shortcuts.ToList()[listBox1.SelectedIndex]);
            ViewSettings();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {

            ColorDialog colorDialog = new ColorDialog();
            if (DialogResult.OK == colorDialog.ShowDialog())
            {
                Color color = colorDialog.Color;
                panel1.BackColor = color;
                SettingsRepository.Update(new Setting() { Key = "APP_BACKGROUND_RED", Value = "" + color.R });
                SettingsRepository.Update(new Setting() { Key = "APP_BACKGROUND_GREEN", Value = "" + color.G });
                SettingsRepository.Update(new Setting() { Key = "APP_BACKGROUND_BLUE", Value = "" + color.B });
                //StreamWriter sw = new StreamWriter(@"color.config");
                //sw.WriteLine(color.R + "");
                //sw.WriteLine(color.G + "");
                //sw.WriteLine(color.B + "");
                //sw.Flush();
                //sw.Close();
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfile = new SaveFileDialog();
            if (sfile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.Copy(SystemUtility.StorageFileInAppDataPath, sfile.FileName);
                    Form1.msg("Your application settings are saved in the file '" + sfile.FileName + "'");
                }
                catch (Exception ex)
                {
                    Form1.msg(ex.Message + " " + "Please try again!", MessageBoxIcon.Exclamation);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "K+ Shortcut SaveFile|*.bin";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BinarySerializationEngine<BinaryStorage> b = new BinarySerializationEngine<BinaryStorage>(ofd.FileName);
                    var v = b.Get();

                    try
                    {
                        if(File.Exists(SystemUtility.StorageFileInAppDataPath))
                        {
                            File.Delete(SystemUtility.StorageFileInAppDataPath);
                        }
                        File.Copy(ofd.FileName, SystemUtility.StorageFileInAppDataPath);
                        Form1.msg("Your application settings were restored successfully!");
                    }
                    catch (Exception ex)
                    {
                        Form1.msg(ex.Message + " " + "Please try again!", MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception ex) {Form1.msg("The save file is currupted!. " + ex.Message, MessageBoxIcon.Exclamation);}
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SystemUtility.Catch(() =>
            {
                SystemUtility.RegisterInStartup(checkBox1.Checked);
            });
        }
    }
}
