using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShortcutGadget.Model
{
    public class SimpleDriveDetector : IDriveDetector
    {
        private List<DriveInfo> insertedDrives = new List<DriveInfo>();
        private int ScanningInterval = 1000;
        private bool stopCommand = false;
        public SimpleDriveDetector(int ScanningInterval)
        {
            this.ScanningInterval = ScanningInterval;
        }
        public void StartScanning(DriveInsertedEvent insertedEvent, DriveRemovedEvent removedEvent, bool displayFixedDrives)
        {
            stopCommand = false;
            this.DisplayFixedDrives = displayFixedDrives;
            Task.Run(new Action(() =>
            {
                while (true)
                {
                    List<DriveInfo> drives = new List<DriveInfo>();
                    foreach (var v in FilterDrives(DriveInfo.GetDrives().ToList()))
                    {
                        try
                        {
                            var dae = v.RootDirectory.EnumerateFiles();
                            drives.Add(v);
                        }
                        catch (Exception ex) { }
                    }
                    foreach (var v in drives)
                    {
                        if (!insertedDrives.Select(x => x.Name).Contains(v.Name)) { insertedEvent(v.Name); insertedDrives.Add(v); }
                    }
                    if (stopCommand) break;
                    Thread.Sleep(ScanningInterval);
                }
            }));

            Task.Run(new Action(() =>
            {
                while (true)
                {
                    List<DriveInfo> drives = new List<DriveInfo>();
                    foreach (var v in FilterDrives(DriveInfo.GetDrives().ToList()))
                    {
                        try
                        {
                            var dae = v.RootDirectory.EnumerateFiles();
                            drives.Add(v);
                        }
                        catch (Exception ex) { }
                    }
                    var temp = insertedDrives.ToArray();
                    foreach (var v in temp)
                    {
                        if (!drives.Select(x => x.Name).Contains(v.Name))
                        {
                            removedEvent(v.Name); insertedDrives.Remove(v);
                        }
                    }

                    if (stopCommand) break;
                    Thread.Sleep(ScanningInterval);
                }
            }));
        }

        private List<DriveInfo> FilterDrives(List<DriveInfo> list)
        {
            List<DriveInfo> re = list.ToArray().ToList();
            if (DisplayFixedDrives) re = list.Where(x => x.DriveType == DriveType.Removable || x.DriveType == DriveType.CDRom || x.DriveType == DriveType.Fixed).ToList();
            else re = list.Where(x => x.DriveType == DriveType.Removable || x.DriveType == DriveType.CDRom).ToList();
            return re;
        }

        public void StopScanning()
        {
            stopCommand = true;
        }
        private bool _displayFixedDrives;
        public bool DisplayFixedDrives
        {
            get
            {
                return _displayFixedDrives;
            }
            set
            {
                _displayFixedDrives = value;
            }
        }
    }
}
