using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutGadget.Intel.WindowsLinks
{
    public class WindowsLinkFactory
    {
        public static List<WindowsLink> WindowsLinks { get { return links(); } }
        private static List<WindowsLink> links()
        {
            List<WindowsLink> re = new List<WindowsLink>();
            List<Tuple<string, string, ExecutionMode>> d1 = new List<Tuple<string, string, ExecutionMode>>();

            d1.Add(new Tuple<string, string, ExecutionMode>("Display Settings", "control desktop", ExecutionMode.CommandLine));
            d1.Add(new Tuple<string, string, ExecutionMode>("Add/Remove Programs", "control appwiz.cpl", ExecutionMode.CommandLine));
            d1.Add(new Tuple<string, string, ExecutionMode>("Administrative Tools", "control admintools", ExecutionMode.CommandLine));
            d1.Add(new Tuple<string, string, ExecutionMode>("Task Scheduler", "control schedtasks", ExecutionMode.CommandLine));
            d1.Add(new Tuple<string, string, ExecutionMode>("User Accounts", "control userpasswords", ExecutionMode.CommandLine));
            d1.Add(new Tuple<string, string, ExecutionMode>("Mouse Settings", "control mouse", ExecutionMode.CommandLine));
            d1.Add(new Tuple<string, string, ExecutionMode>("Keyboard Settings", "control keyboard", ExecutionMode.CommandLine));
            d1.Add(new Tuple<string, string, ExecutionMode>("Network Connections", "control netconnections", ExecutionMode.CommandLine));
            d1.Add(new Tuple<string, string, ExecutionMode>("Default Programs", "control /name Microsoft.DefaultPrograms", ExecutionMode.CommandLine));
            d1.Add(new Tuple<string, string, ExecutionMode>("System Date & Time", "control /name Microsoft.DateAndTime", ExecutionMode.CommandLine));
            re.Add(new WindowsLink()
            {
                ID = 1,
                Name = "Control Panel",
                Icon = global::ShortcutGadget.Properties.Resources.cpanel,
                MainCommand = "control",
                ExecutionMode = ExecutionMode.CommandLine,
                SubCommands = d1
            });


            List<Tuple<string, string, ExecutionMode>> d2 = new List<Tuple<string, string, ExecutionMode>>();
            d2.Add(new Tuple<string, string, ExecutionMode>("Device Manager", "control /name Microsoft.DeviceManager", ExecutionMode.CommandLine));
            d2.Add(new Tuple<string, string, ExecutionMode>("Properties", "control sysdm.cpl", ExecutionMode.CommandLine));

            re.Add(new WindowsLink()
            {
                ID = 2,
                Name = "My Computer",
                Icon = global::ShortcutGadget.Properties.Resources.mcomp,
                ExecutionMode = ExecutionMode.GUID,
                MainCommand = "::{20d04fe0-3aea-1069-a2d8-08002b30309d}",
                SubCommands = d2,
            });

            List<Tuple<string, string, ExecutionMode>> d3 = new List<Tuple<string, string, ExecutionMode>>();
            d3.Add(new Tuple<string, string, ExecutionMode>("Shutdown", "shutdown /s", ExecutionMode.CommandLine));
            d3.Add(new Tuple<string, string, ExecutionMode>("Restart", "shutdown /r", ExecutionMode.CommandLine));
            d3.Add(new Tuple<string, string, ExecutionMode>("Sleep", "shutdown /", ExecutionMode.CommandLine));
            d3.Add(new Tuple<string, string, ExecutionMode>("Hibernate", "shutdown /h", ExecutionMode.CommandLine));
            d3.Add(new Tuple<string, string, ExecutionMode>("Sign Out", "shutdown /l", ExecutionMode.CommandLine));

            re.Add(new WindowsLink()
            {
                ID = 3,
                Name = "System Power",
                Icon = global::ShortcutGadget.Properties.Resources.Power,
                ExecutionMode = ExecutionMode.CommandLine,
                MainCommand = "taskmgr.exe",
                SubCommands = d3,
            });
            return re;
        }
    }
}
