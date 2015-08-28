using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ShortcutGadget.Model
{


    [Serializable]
    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class FolderLink
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ExplorerLink { get; set; }
        public int AccessCount { get; set; }
    }

    public class FolderLinkButton : Button
    {
        public FolderLinkButton()
        {
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Font = new Font(new FontFamily("segoe ui"), 8, FontStyle.Bold);
            this.Height = 25;
            this.MouseEnter += (s, x) => { this.BackColor = Color.Gray; };
            this.MouseLeave += (s, x) => { this.BackColor = Color.Transparent; };
        }
    }

    public class Effects
    {
        Form1 frm;

        public Effects(Form1 f)
        {
            frm = f;
        }
        public void FadeIn(double timeMilli)
        {
            frm.Invoke(new Action(() => {

                frm.Top = 0;
                frm.Left = 0;

            }));
            for (double d = 0; d <= 1; d += 0.06)
            {
                frm.Invoke(new Action(() => { frm.Opacity = d; }));
                Thread.Sleep((int)(timeMilli * 0.06));
            }
            frm.Invoke(new Action(() => { frm.TopMost = true; }));

        }
        public void FadeOut(double timeMilli)
        {

            for (double d = 1; d >= 0; d -= 0.06)
            {
                frm.Invoke(new Action(() => { frm.Opacity = d; }));
                Thread.Sleep((int)(timeMilli * 0.06));
            }

            frm.Invoke(new Action(() => {

                frm.Top = -frm.Height + 1;
                frm.Left = -frm.Width + 1;

            }));
        }
    }

    public enum AppState
    {
        JustOpened,
        ClickedALinkNotFaded,
        ClickedALinkFaded,
        MouseOKFadedInNotClicked,
        MouseNotOKFadedOutNotClicked,
    }
}