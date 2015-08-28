using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using ShortcutGadget.Model;
using ShortcutGadget.Model.Concrete;

namespace ShortcutGadget.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            #region fileproc
            //string path = @"C:\Users\Kamal Rathnayake\Desktop\controlPanelShortcuts.txt";
            //FileStream fs = new FileStream(path, FileMode.Open);
            //List<string> lines = new List<string>();
            //List<byte> lineBuffer = new List<byte>();

            //for (int k = 0; k < 10; k++)
            //{
            //    int value = fs.ReadByte();
            //    System.Console.WriteLine(value);
            //    if (value == 13 && fs.Seek(k+1, SeekOrigin.Begin) == 10)
            //    {
            //        System.Console.WriteLine("de");
            //        lines.Add(System.Text.Encoding.ASCII.GetString(lineBuffer.ToArray()));
            //        lineBuffer = new List<byte>();
            //        value = fs.ReadByte();
            //    }
            //    else
            //        lineBuffer.Add((byte)value);
            //}

            //lines.ForEach(x => System.Console.WriteLine(x));
            //    System.Console.Read();
            #endregion

            int sum = 0;
            for (int k = 1; k <= 10; k++)
            {
                sum += k;
            }
             
            System.Console.WriteLine("Done!");
            System.Console.Read();
        }
    }
}
