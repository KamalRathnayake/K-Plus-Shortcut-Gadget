using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShortcutGadget.Model
{
    [Serializable]
    public class BinarySerializationEngine<T>
    {
        private string path;
        public BinarySerializationEngine(string XmlFilePath)
        {
            path = XmlFilePath;
        }
        public void Set(T item)
        {
            Stream stream = File.Open(path, FileMode.Create,FileAccess.Write,FileShare.ReadWrite);
            BinaryFormatter bform = new BinaryFormatter();
            bform.Serialize(stream, item);
            stream.Close();
        }
        public T Get()
        {
            int index = 0;
            Stream stream;
            while (true)
            {
                index++;
                try
                {
                    stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    break;
                }
                catch (IOException ex)
                {
                    if (index > 10) throw ex;
                    else Thread.Sleep(200);
                }
            }
            BinaryFormatter bform = new BinaryFormatter();
            T re;
            try
            {
                re = (T)bform.Deserialize(stream);
            }
            catch (SerializationException sex)
            {
                re = default(T);
            }
            stream.Close();
            return re;
        }
    }
}
