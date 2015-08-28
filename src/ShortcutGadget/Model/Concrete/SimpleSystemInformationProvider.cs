using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutGadget.Model
{
    public class SimpleSystemInformationProvider:ISystemInformationProvider
    {
        public double GetProcessorUsagePerecentge()
        {
            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter(
            "Processor",
            "% Processor Time",
            "_Total",
            true
            );
            return cpuCounter.NextValue();
        }

        public double GetMemoryUsagePercentage()
        {
            PerformanceCounter cpuCounter;
            PerformanceCounter ramCounter;
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            return ramCounter.NextValue();
        }

        public double GetHardDriveUsagePercentage()
        {
            double free=0;
            double all = 0;
            foreach (var v in System.IO.DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Fixed))
            {
                free += v.AvailableFreeSpace;
                all += v.TotalSize;
            }
            return free / all * 100;
        }

        public TimeSpan GetSystemUptime()
        {
            throw new NotImplementedException();
        }

        public double GetHardDriveUsagePercentage(string driveLetter)
        {
            throw new NotImplementedException();
        }
    }
}
