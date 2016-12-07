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

            HashSet<UInt32> testHash = new HashSet<UInt32>();
            
            UInt32 vebpInt;
            UInt32 hebpInt;
            Dictionary<UInt32, HashSet<UInt32>> tempDict = new Dictionary<UInt32, HashSet<UInt32>>();
            using (BinaryReader reader = new BinaryReader(File.Open("VEBPHEBPSet_3.bin",FileMode.Open)))
            {
                long length = reader.BaseStream.Length;
                
                while(reader.BaseStream.Position != length)
                {
                    vebpInt = reader.ReadUInt32();
                    hebpInt = reader.ReadUInt32();
                    //Console.WriteLine(vebpInt + " " + hebpInt);
                    
                    if (!tempDict.ContainsKey(vebpInt))
                    {
                        HashSet<UInt32> tempHashSet = new HashSet<UInt32>();
                        tempHashSet.Add(hebpInt);
                        tempDict.Add(vebpInt, tempHashSet);
                    }
                    else
                    {
                        bool duplicateCheck = tempDict[vebpInt].Add(hebpInt);
                        if (!duplicateCheck) Console.WriteLine("there is duplicate!");
                    }
                }
                
            }
            Console.WriteLine("Press enterkey to finish...");
            Console.ReadLine();
        }
    }
}
