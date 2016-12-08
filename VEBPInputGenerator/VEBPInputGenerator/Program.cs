﻿using System;
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
        public const int M = 6;
        public const int N = 6;
        public const int numberOfSameBits = 18;
        public static UInt32 numbOfElements=0;
    }
    class Program
    {
        static int inverseEBP(int EBPInt,ref int[] maskArray)
        {
            int resultInt=0;
            BitVector32 EBPBits = new BitVector32(EBPInt);
            BitVector32 tempEBPBits = new BitVector32(0);

            for (int index=0; index<(GlobalVar.M-1)*GlobalVar.N; index++)
            {
                int i = index / GlobalVar.M;
                int j = index % GlobalVar.M;
                int tempIndex = GlobalVar.M * (1 + i) - j - 1; //GlobalVar.M - 1 - j + GlobalVar.M * i;
                //int originalIndex = (int)Math.Pow(2, (double)index);
                //int newIndex = (int)Math.Pow(2, (double)tempIndex);
                tempEBPBits[maskArray[tempIndex]] = EBPBits[maskArray[index]];            
            }
            resultInt = tempEBPBits.Data;       
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
                Console.WriteLine(length/4);
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



            SortedDictionary<int, int> histogramDict = new SortedDictionary<int, int>();
            using (StreamWriter summaryFile = new StreamWriter("summaryFile_" + GlobalVar.M + ".txt"))
            {
                int lineNumber = 1;
                //int maxNumb = -1;
                summaryFile.WriteLine("GridSize: "+GlobalVar.M+"  fixedNumberOfBits: "+GlobalVar.numberOfSameBits);
                foreach (var eachBits in tempDict)
                {
                    summaryFile.Write(lineNumber+".  ");
                    lineNumber++;
                    BitVector32 writeBits = new BitVector32((int)eachBits.Key);
                    for (int i = 0; i < GlobalVar.numberOfSameBits; i++)
                    {
                        if (writeBits[maskArray[i]])
                            summaryFile.Write(1);
                        else
                            summaryFile.Write(0);
                    }
                    //summaryFile.Write(eachBits.Key);    
                    summaryFile.WriteLine(" : "+eachBits.Value.Count);


                    if (histogramDict.ContainsKey(eachBits.Value.Count))
                        histogramDict[eachBits.Value.Count]++;
                    else
                        histogramDict.Add(eachBits.Value.Count, 1);
                    //if (maxNumb < eachBits.Value.Count) maxNumb = eachBits.Value.Count;
                }
                //Console.WriteLine("maxNumb: " + maxNumb);
            }

            using (StreamWriter histogramFile = new StreamWriter("histogramFile_" + GlobalVar.M + ".txt"))
            {
                foreach (var eachEle in histogramDict)
                    histogramFile.WriteLine(eachEle.Key+" "+eachEle.Value);
            }


                Console.WriteLine("Press enterkey to finish...");
            Console.ReadLine();
        }
    }
}
