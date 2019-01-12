using System;
using RelevantFunction;
using System.Diagnostics;

namespace SingleThreadingVersion
{
    public class Program
    {
        static int var4Swap;

        static void Main(string[] args)
        {
            //int[] array = Relevant.GenerateRandomIntergers(40_000_000, 0, 10000);
            //GetMaxValueThenPlaceToEnd(array);
            //Sort(array, 0, array.Length - 1);
            //bool result = Relevant.VerifySequence(array);
            //Console.WriteLine(result);
            Testing();
            Console.ReadKey();
        }

        public static void Testing()
        {
            int[] array = Relevant.GenerateRandomIntergers(300_000_000, 0, 1_000_000);
            array[array.Length - 1] = int.MaxValue;

            Stopwatch sw = new Stopwatch();
            Console.WriteLine("SingleThreadingVersion:");
            sw.Start();
            Sort(array, 0, array.Length - 1);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            bool result = Relevant.VerifySequence(array);
            Console.WriteLine(result);
        }

        static void GetMaxValueThenPlaceToEnd(int[] array)
        {
            int maxEleIndex = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > array[maxEleIndex])
                    maxEleIndex = i;
            }

            Swap(array, maxEleIndex, array.Length - 1);
        }

        static void Sort(int[] array, int lo, int hi)
        {
            if (hi <= lo) return;
            int middleEleIndex = Partition(array, lo, hi);
            Sort(array, lo, middleEleIndex - 1);
            Sort(array, middleEleIndex + 1, hi);
        }

        static int Partition(int[] array, int lo, int hi)
        {
            int middleEle = array[lo];
            int i = lo;
            int j = hi + 1;
            while (true)
            {
                while (array[++i] < middleEle) ;
                while (array[--j] > middleEle) ;
                if (i >= j) break;

                Swap(array, i, j);
            }
            Swap(array, lo, j);
            return j;
        }

        static void Swap(int[] array, int e1, int e2)
        {
            var4Swap = array[e1];
            array[e1] = array[e2];
            array[e2] = var4Swap;
        }

    }
}
