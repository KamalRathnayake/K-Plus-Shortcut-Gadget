using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutGadget.Model
{
    public class SimpleUIArranger:IUIArranger
    {
        Form1 handle;
        public SimpleUIArranger(Form1 handle)
        {
            this.handle = handle;
        }

        public void StartingArrangement()
        {
            handle.Width = 309;
            handle.Height = 442;
            foreach (var v in handle.Panels) { v.Top = 275; v.Left = 0; }
        }
    }
}
