using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutGadget.Model
{
    public interface ISystemInformationProvider
    {
        double GetProcessorUsagePerecentge();
        double GetMemoryUsagePercentage();
        double GetHardDriveUsagePercentage();
        TimeSpan GetSystemUptime();
        double GetHardDriveUsagePercentage(string driveLetter);
    }
}
