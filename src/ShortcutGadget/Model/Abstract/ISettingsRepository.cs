using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShortcutGadget.Model
{
    public interface ISettingsRepository
    {
        void Insert(Setting setting);
        void Update(Setting setting);
        Setting Receive(string key);
    }

}
