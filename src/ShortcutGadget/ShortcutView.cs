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
using ShortcutGadget.Model;

namespace ShortcutGadget
{
    public enum SVMode{
        CreateNew,Update
    }
    public partial class ShortcutView : Form
    {
        Settings parent;
        FolderLink shortcut;
        SVMode Mode;
        public ShortcutView(FolderLink s,Settings sv,SVMode svMode)
        {
            shortcut = s;
            InitializeComponent();
            parent = sv;
            Mode = svMode;
        }

        private void ShortcutView_Load(object sender, EventArgs e)
        {
            updateUI();
            button1.MouseClick += (s, e1) => { this.Close(); };
            button2.MouseClick += (s, e1) =>
            {
                if (shortcut.Name!=null && shortcut.Name.Length != 0)
                {
                    if (Directory.Exists(shortcut.ExplorerLink))
                    {
                        if (Mode == SVMode.CreateNew) { /*MessageBox.Show("CREATE NEW");*/ parent.ShortcutRepository.Add(shortcut); parent.ViewSettings(); this.Close(); }
                        else if (Mode == SVMode.Update) { MessageBox.Show("Update"); parent.ShortcutRepository.Update(shortcut); }
                    }
                    else Form1.msg("The directory does not exist!", MessageBoxIcon.Exclamation);
                }
                else Form1.msg("The name should not be empty!", MessageBoxIcon.Exclamation);
            };
            button3.MouseClick += (s, e1) =>
            {
                FolderBrowserDialog fb = new FolderBrowserDialog();
                if (fb.ShowDialog() == DialogResult.OK)
                {
                    textBox3.Text = fb.SelectedPath;
                }
            };
            textBox2.TextChanged += (s, e1) => { shortcut.Name = textBox2.Text; };
            textBox3.TextChanged += (s, e1) => { shortcut.ExplorerLink = textBox3.Text; };
        }
        public void updateUI()
        {
            textBox1.Text = shortcut.ID.ToString();
            textBox2.Text = shortcut.Name;
            textBox3.Text = shortcut.ExplorerLink;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
