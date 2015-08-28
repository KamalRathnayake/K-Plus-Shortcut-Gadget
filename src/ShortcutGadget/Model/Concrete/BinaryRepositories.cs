using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShortcutGadget.Model.Concrete
{
    public class BinarySettingsRepository : ISettingsRepository
    {
        private BinaryStorage storage;
        public BinarySettingsRepository(BinaryStorage storage_)
        {
            storage = storage_;
        }
        public void Insert(Setting setting)
        {
            if (storage.Settings.Where(x => x.Key == setting.Key).Count() > 0)
                storage.Settings.Where(x => x.Key == setting.Key).First().Value = setting.Value;
            else storage.Settings.Add(setting);
            storage.SaveChanges();
        }

        public void Update(Setting setting)
        {
            storage.Settings.Where(x => x.Key == setting.Key).First().Value = setting.Value;
            storage.SaveChanges();
        }

        public Setting Receive(string key)
        {
            if (storage.Settings.Where(x => x.Key == key).Count() == 0)
            {
                storage.Settings.Add(new Setting() { Key = key, Value = (key == "PANEL_ACTIVE" || key == "SORT_BY_USAGE" || key == "PANEL_DRIVE_INFO_VIEW_FIXED") ? "true" : "0" });
                storage.SaveChanges();
            }
            return storage.Settings.Where(x => x.Key.ToLower() == key.ToLower()).First();
        }
    }
    public class BinaryShortcutsRepository : IShortcutRepository
    {
        private BinaryStorage storage;
        public BinaryShortcutsRepository(BinaryStorage storage_)
        {
            storage = storage_;
        }
        public IQueryable<FolderLink> Shortcuts
        {
            get { return storage.FolderLinks.AsQueryable(); }
        }

        public void Add(FolderLink s)
        {
            if (storage.FolderLinks.Count() > 0) s.ID = storage.FolderLinks.Select(x => x.ID).Max() + 1;
            else s.ID = 1;
            if (storage.FolderLinks.Where(x => x.ExplorerLink.ToLower() == s.ExplorerLink.ToLower()).Count() > 0) return;
            storage.FolderLinks.Add(s);
            storage.SaveChanges();
        }

        public void Remove(int ID)
        {
            storage.FolderLinks.Remove(storage.FolderLinks.Where(x => x.ID == ID).First());
            storage.SaveChanges();
        }

        public void Update(FolderLink s)
        {
            var item = storage.FolderLinks.Where(x => x.ID == s.ID).First();
            item = s;
            storage.SaveChanges();
        }

        public void MoveUp(FolderLink s)
        {
            throw new NotImplementedException();
        }

        public void MoveDown(FolderLink s)
        {
            throw new NotImplementedException();
        }
    }


}