using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShortcutGadget.Model
{
    public class XMLShortcutRepository:IShortcutRepository
    {
        private string xmlfile = "storage.xml";
        public IQueryable<FolderLink> Shortcuts
        {
            get
            {
                List<FolderLink> re = new List<FolderLink>();
                var ele = GetDoc().Root.Element(XName.Get("shortcuts")).Elements();
                foreach (var v in ele)
                    re.Add(new FolderLink()
                    {
                        ID = int.Parse(v.Attribute(XName.Get("id")).Value),
                        Name=v.Element("name").Value,
                        ExplorerLink=v.Element("link").Value,
                        AccessCount=int.Parse(v.Element("accessCount").Value),
                    });
                return re.AsQueryable();
            }
        }
        public XDocument GetDoc()
        {
            return XDocument.Load(xmlfile);
        }
        public void Add(FolderLink s)
        {
            var doc = GetDoc();
            XElement ele = new XElement("shortcut");
            ele.Add(new XAttribute("id", (Shortcuts.Count()==0)?1:(Shortcuts.Select(x => x.ID).Max() + 1)));
            ele.Add(new XElement("name") { Value = s.Name });
            ele.Add(new XElement("link") { Value = s.ExplorerLink });
            ele.Add(new XElement("accessCount") { Value = s.AccessCount+"" });
            doc.Root.Element(XName.Get("shortcuts")).Add(ele);
            doc.Save(xmlfile);
        }

        public void Remove(int ID)
        {
            var doc = GetDoc();
            doc.Root.Element(XName.Get("shortcuts")).Elements().Where(x => x.Attribute(XName.Get("id")).Value == ID.ToString()).First().Remove();
            doc.Save(xmlfile);
        }

        public void Update(FolderLink s)
        {
            var doc = GetDoc();
            var ele = doc.Root.Element(XName.Get("shortcuts")).Elements().Where(x => x.Attribute(XName.Get("id")).Value == s.ID.ToString()).First();
            ele.Element("name").Value = s.Name;
            ele.Element("link").Value = s.ExplorerLink;
            ele.Element("accessCount").Value = s.AccessCount + "";
            doc.Save(xmlfile);
        }

        public void MoveUp(FolderLink s)
        {
            var doc = GetDoc();
            doc.Save(xmlfile);
            throw new NotImplementedException();
        }

        public void MoveDown(FolderLink s)
        {
            throw new NotImplementedException();
        }
    }
}
