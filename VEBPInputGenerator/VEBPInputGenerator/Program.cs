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
        public const int M = 5;
        public const int N = 5;
        public const int numberOfSameBits = 14;
        public static UInt32 numbOfElements=0;
    }
    class Program
    {
        
        static void Main(string[] args)
        {

            
            HashSet<UInt32> testHash = new HashSet<UInt32>();
            int[] maskArray = new int[GlobalVar.M * (GlobalVar.N - 1)];
            maskArray[0] = BitVector32.CreateMask();
            for(int i=1; i<GlobalVar.M*(GlobalVar.N-1); i++)
            {
                maskArray[i] = BitVector32.CreateMask(maskArray[i - 1]);
            }

            
            UInt32 vebpInt;
            //UInt32 hebpInt;
            Dictionary<UInt32, HashSet<UInt32>> tempDict = new Dictionary<UInt32, HashSet<UInt32>>();
            //using (BinaryReader reader = new BinaryReader(File.Open("VEBPHEBPSet_"+GlobalVar.M+".bin",FileMode.Open)))
            using (BinaryReader reader = new BinaryReader(File.Open("VEBPSet_"+GlobalVar.M+".bin", FileMode.Open)))
            {
                long length = reader.BaseStream.Length;
                //Console.WriteLine(length/4);
                
                while(reader.BaseStream.Position != length)
                {
                    vebpInt = reader.ReadUInt32();
                    //hebpInt = reader.ReadUInt32();
                    //Console.WriteLine(vebpInt + " " + hebpInt);
                    //int result = inverseEBP((int)vebpInt);
                    //Console.ReadKey();
                    BitVector32 VEBPBIts = new BitVector32((int)vebpInt);
                    BitVector32 firstVEBPBIts = new BitVector32();
                    for (int i = 0; i < GlobalVar.numberOfSameBits; i++)
                        firstVEBPBIts[maskArray[i]] = VEBPBIts[maskArray[i]];
                    //Console.WriteLine(VEBPBIts+" , "+vebpInt);
                    //Console.WriteLine(firstVEBPBIts+" , "+(UInt32)firstVEBPBIts.Data);
                    //if(firstVEBPBIts.Data==54)
                    //{
                    //    Console.WriteLine(VEBPBIts + " , " + vebpInt);
                    //    Console.WriteLine(firstVEBPBIts + " , " + (UInt32)firstVEBPBIts.Data);
                    //    Console.ReadKey();
                    //}

                    if (!tempDict.ContainsKey((UInt32)firstVEBPBIts.Data))
                    {
                        HashSet<UInt32> tempHashSet = new HashSet<UInt32>();
                        tempHashSet.Add(vebpInt);
                        tempDict.Add((UInt32)firstVEBPBIts.Data, tempHashSet);
                    }
                    else
                    {
                        bool duplicateCheck = tempDict[(UInt32)firstVEBPBIts.Data].Add(vebpInt);
                        if (!duplicateCheck) Console.WriteLine("there is duplicate!");
                    }
                }             
            }

            SortedDictionary<int, List<HashSet<uint>>> histogramDict = new SortedDictionary<int, List<HashSet<uint>>>();
            foreach (var eachHashSet in tempDict)
            {
                int NumbOfVEBPs = eachHashSet.Value.Count;
                HashSet<uint> tempHashSet = eachHashSet.Value;

                if (histogramDict.ContainsKey(NumbOfVEBPs))
                {
                    histogramDict[NumbOfVEBPs].Add(tempHashSet);
                }
                else
                {
                    List<HashSet<uint>> tempList = new List<HashSet<uint>>();
                    tempList.Add(tempHashSet);
                    histogramDict.Add(NumbOfVEBPs, tempList);
                }
            }

            int fileNumb = 0;
            foreach (var eachDict in histogramDict)
            {
                foreach (HashSet<uint> eachVEBPSet in eachDict.Value)
                {
                    using (BinaryWriter binaryFileWriter = new BinaryWriter(File.Open(GlobalVar.M+"x"+GlobalVar.M+"Files\\" + fileNumb + ".bin", FileMode.Create)))
                    {
                        foreach (uint eachVebp in eachVEBPSet)
                        {
                            binaryFileWriter.Write(eachVebp);
                        }
                    }
                    fileNumb++;
                }
            }


            //SortedDictionary<int, int> histogramDict = new SortedDictionary<int, int>();
            //using (StreamWriter summaryFile = new StreamWriter("summaryFile_" + GlobalVar.M + ".txt"))
            //{
            //    int lineNumber = 1;
            //    //int maxNumb = -1;
            //    summaryFile.WriteLine("GridSize: "+GlobalVar.M+"  fixedNumberOfBits: "+GlobalVar.numberOfSameBits);
            //    foreach (var eachBits in tempDict)
            //    {
            //        summaryFile.Write(lineNumber+".  ");
            //        lineNumber++;
            //        BitVector32 writeBits = new BitVector32((int)eachBits.Key);
            //        for (int i = 0; i < GlobalVar.numberOfSameBits; i++)
            //        {
            //            if (writeBits[maskArray[i]])
            //                summaryFile.Write(1);
            //            else
            //                summaryFile.Write(0);
            //        }
            //        //summaryFile.Write(eachBits.Key);    
            //        summaryFile.WriteLine(" : "+eachBits.Value.Count);


            //        if (histogramDict.ContainsKey(eachBits.Value.Count))
            //            histogramDict[eachBits.Value.Count]++;
            //        else
            //            histogramDict.Add(eachBits.Value.Count, 1);
            //        //if (maxNumb < eachBits.Value.Count) maxNumb = eachBits.Value.Count;
            //    }
            //    //Console.WriteLine("maxNumb: " + maxNumb);
            //}

            //using (StreamWriter histogramFile = new StreamWriter("histogramFile_" + GlobalVar.M + ".txt"))
            //{
            //    foreach (var eachEle in histogramDict)
            //        histogramFile.WriteLine(eachEle.Key+" "+eachEle.Value);
            //}


            Console.WriteLine("Press enterkey to finish...");
            Console.ReadLine();
        }
        static int inverseEBP(int EBPInt, ref int[] maskArray)
        {
            int resultInt = 0;
            BitVector32 EBPBits = new BitVector32(EBPInt);
            BitVector32 tempEBPBits = new BitVector32(0);

            for (int index = 0; index < (GlobalVar.M - 1) * GlobalVar.N; index++)
            {
                int i = index / GlobalVar.M;
                int j = index % GlobalVar.M;
                int newIndex = GlobalVar.M * (1 + i) - j - 1; //GlobalVar.M - 1 - j + GlobalVar.M * i;
                tempEBPBits[maskArray[newIndex]] = EBPBits[maskArray[index]];
            }

            resultInt = tempEBPBits.Data;
            return resultInt;
        }

        static int switchEBP(int EBPInt, ref int[] maskArray)
        {
            int resultInt = 0;
            BitVector32 EBPBits = new BitVector32(EBPInt);
            BitVector32 tempEBPBits = new BitVector32(0);

            for (int index = 0; index < (GlobalVar.M - 1) * GlobalVar.N; index++)
            {
                int i = index % GlobalVar.M;
                int j = index / GlobalVar.M;
                int newJ = GlobalVar.N - 1 - j;
                int newIndex = (newJ - 1) * GlobalVar.M + i;
                tempEBPBits[maskArray[newIndex]] = EBPBits[maskArray[index]];

            }

            resultInt = tempEBPBits.Data;
            return resultInt;
        }

        static int invSwitchEBP(int EBPInt, ref int[] maskArray)
        {
            int resultInt = 0;
            resultInt = inverseEBP(switchEBP(EBPInt, ref maskArray), ref maskArray);

            return resultInt;
        }

        static int rotateEBP(int EBPInt, ref int[] maskArray)
        {
            int resultInt = 0;
            resultInt = inverseEBP(EBPInt, ref maskArray);

            return resultInt;
        }
    }
}
