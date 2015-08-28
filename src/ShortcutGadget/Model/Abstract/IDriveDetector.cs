using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutGadget.Model
{
    public delegate void DriveInsertedEvent(string driveLetter);
    public delegate void DriveRemovedEvent(string driveLetter);
    public interface IDriveDetector
    {
        bool DisplayFixedDrives { get; set; }
        void StartScanning(DriveInsertedEvent insertedEvent, DriveRemovedEvent removedEvent, bool displayFixedDrives);
        void StopScanning();
    }
}
