using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutGadget.Model.Concrete
{
    [Serializable]
    public class BinaryStorage
    {
        private static BinaryStorage storage;
        private Model.BinarySerializationEngine<BinaryStorage> binStorage;
        private BinaryStorage() { }
        public static BinaryStorage Get(BinarySerializationEngine<BinaryStorage> serializationEngine)
        {
            if (storage == null)
            {
                storage = new BinaryStorage();
                storage.binStorage = serializationEngine;
                try
                {
                    storage.FolderLinks = storage.binStorage.Get().FolderLinks;
                    storage.Settings = storage.binStorage.Get().Settings;
                }
                catch (NullReferenceException nex)
                {
                    storage.FolderLinks = new List<FolderLink>();
                    storage.Settings = new List<Setting>();
                }
            }
            return storage;
        }
        public void SaveChanges()
        {
            storage.binStorage.Set(storage);
        }
        public List<FolderLink> FolderLinks;
        public List<Setting> Settings;
    }
}
