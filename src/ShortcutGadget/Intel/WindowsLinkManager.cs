using ShortcutGadget.Intel.WindowsLinks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutGadget.Intel
{
    public class WindowsLinkManager
    {
        Form1 handle;
        private LinkIconFactory iconFactory;
        private List<WindowsLink> winLinks;
        public WindowsLinkManager(Form1 handle)
        {
            this.handle = handle;
            winLinks = WindowsLinkFactory.WindowsLinks;
            iconFactory = new LinkIconFactory();
        }
        public void Initialize()
        {
            int posx = 200;
            int posy = 60;
            for (int k = 0; k < winLinks.Count(); k++)
            {
                var pbox = iconFactory.GetPictureBox(winLinks[k]);
                pbox.Top = posy + k * pbox.Height;
                pbox.Left = handle.Width - pbox.Width;
                handle.Controls.Add(pbox);
            }
        }

    }
}
