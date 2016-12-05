using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEBPInputGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            UInt32 vebpInt;
            UInt32 hebpInt;
            
            using (BinaryReader reader = new BinaryReader(File.Open("VEBPHEBPSet_3.bin",FileMode.Open)))
            {
                long length = reader.BaseStream.Length;
                while(reader.BaseStream.Position != length)
                {
                    vebpInt = reader.ReadUInt32();
                    hebpInt = reader.ReadUInt32();
                    Console.WriteLine(vebpInt + " " + hebpInt);
                }
                
            }
            Console.WriteLine("Press enterkey to finish...");
            Console.ReadLine();
        }
    }
}
