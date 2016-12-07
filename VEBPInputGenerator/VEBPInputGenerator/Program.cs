using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//it is for retanglar grid (M==N)
namespace VEBPInputGenerator
{
    public static class GlobalVar
    {
        public const int M = 4;
        public const int N = 4;
        public static UInt32 numbOfElements=0;
    }
    class Program
    {
        static int inverseEBP(int EBPInt)
        {
            int resultInt=0;
            BitVector32 EBPBits = new BitVector32(EBPInt);
            BitVector32 tempEBPBits = new BitVector32(0);
            for (int index=0; index<(GlobalVar.M-1)*GlobalVar.N; index++)
            {
                int i = index / GlobalVar.M;
                int j = index % GlobalVar.M;
                int newIndex = GlobalVar.M * (1 + i) - j - 1; //GlobalVar.M - 1 - j + GlobalVar.M * i;

                tempEBPBits[newIndex] = EBPBits[index];
                Console.WriteLine(index);
            }
            resultInt = tempEBPBits.Data;
            Console.WriteLine(EBPBits);
            Console.WriteLine(tempEBPBits);
            return resultInt;
        }

        static int switchEBP(int EBPInt)
        {
            int resultInt = 0;


            return resultInt;
        }

        static int invSwitchEBP(int EBPInt)
        {
            int resultInt = 0;


            return resultInt;
        }

        static int rotateEBP(int EBPInt)
        {
            int resultInt = 0;


            return resultInt;
        }

        static void Main(string[] args)
        {
            
            HashSet<UInt32> testHash = new HashSet<UInt32>();
            
            UInt32 vebpInt;
            UInt32 hebpInt;
            Dictionary<UInt32, HashSet<UInt32>> tempDict = new Dictionary<UInt32, HashSet<UInt32>>();
            //using (BinaryReader reader = new BinaryReader(File.Open("VEBPHEBPSet_3.bin",FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(File.Open("VEBPHEBPSet_"+GlobalVar.M+".bin", FileMode.Open)))
            {
                long length = reader.BaseStream.Length;
                
                while(reader.BaseStream.Position != length)
                {
                    vebpInt = reader.ReadUInt32();
                    hebpInt = reader.ReadUInt32();
                    Console.WriteLine(vebpInt + " " + hebpInt);
                    inverseEBP((int)vebpInt);
                    Console.ReadLine();

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
