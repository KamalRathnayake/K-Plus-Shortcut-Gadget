using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutGadget.Intel.WindowsLinks
{
    public enum ExecutionMode { CommandLine, ProcessStart, GUID };
    public class WindowsLink
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Bitmap Icon { get; set; }
        public string MainCommand { get; set; }
        public ExecutionMode ExecutionMode { get; set; }
        public List<Tuple<string, string, ExecutionMode>> SubCommands { get; set; }
        public List<Tuple<string, string, ExecutionMode>> ExtraList { get; set; }
    }
}
