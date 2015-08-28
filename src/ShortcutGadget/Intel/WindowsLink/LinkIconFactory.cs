using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutGadget.Intel.WindowsLinks
{
    public class LinkIconFactory
    {
        public PictureBox GetPictureBox(WindowsLink winlink)
        {
            PictureBox re = new PictureBox();
            re.BackColor = System.Drawing.Color.Transparent;
            re.Image = winlink.Icon;
            re.Location = new System.Drawing.Point(0, 0);
            re.Margin = new System.Windows.Forms.Padding(10);
            re.Name = "pictureBox4";
            re.Size = new System.Drawing.Size(47, 47);
            re.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            re.TabIndex = 7;
            re.TabStop = false;
            re.MouseEnter += (sn, ev) => { var obj = (PictureBox)sn; obj.BackColor = Color.Black; };
            re.MouseLeave += (sn, ev) => { var obj = (PictureBox)sn; obj.BackColor = Color.Transparent; };
            re.MouseClick += (sn, ev) =>
            {
                if (ev.Button == System.Windows.Forms.MouseButtons.Left)
                    ExecuteCommandMiddleware(winlink.MainCommand, winlink.ExecutionMode);
            };
            List<MenuItem> subItems = new List<MenuItem>();
            foreach (var v in winlink.SubCommands)
            {
                MenuItem item = new MenuItem();
                item.Text = v.Item1;
                item.Click += (sn, ev) => ExecuteCommandMiddleware(v.Item2, v.Item3);
                subItems.Add(item);
            }

            if (winlink.ExtraList != null && winlink.ExtraList.Count() > 0)
            {
                MenuItem extra = new MenuItem();
                extra.Text = "View All";
                List<MenuItem> extralist = new List<MenuItem>();
                foreach (var v in winlink.ExtraList)
                {
                    MenuItem ir = new MenuItem();
                    ir.Text = v.Item1;
                    ir.Click += (sn, ev) => ExecuteCommandMiddleware(v.Item2, v.Item3);
                    extralist.Add(ir);
                }
            }

            re.ContextMenu = new ContextMenu(subItems.ToArray());
            return re;
        }

        private void ExecuteCommandMiddleware(string command, ExecutionMode mode)
        {
            switch(mode)
            {
                case ExecutionMode.CommandLine: ExecuteCommand(command, 1, true);
                    break;
                case ExecutionMode.GUID: Process.Start(command);
                    break;
                case ExecutionMode.ProcessStart:Process.Start(command);
                    break;
                default:
                    break;
            }
        }

        private void ExecuteCommand(string Command, int Timeout, Boolean closeProcess)
        {
            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + Command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;
            Process = Process.Start(ProcessInfo);
            Process.WaitForExit(Timeout);

            if (closeProcess == true) { Process.Close(); }
        }
    }
}
