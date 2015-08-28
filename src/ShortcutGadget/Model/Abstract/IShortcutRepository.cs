using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutGadget.Model
{
    public interface IShortcutRepository
    {
        IQueryable<FolderLink> Shortcuts { get; }
        void Add(FolderLink s);
        void Remove(int ID);
        void Update(FolderLink s);
        void MoveUp(FolderLink s);
        void MoveDown(FolderLink s);
    }
}
