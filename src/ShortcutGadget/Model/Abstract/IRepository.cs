using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutGadget.Model.Abstract
{
    public interface IRepository<G>
    {
        IQueryable<G> items { get; }
        void Insert(G item);
        void Remove(G item);
        void Update(G item);
    }
}
