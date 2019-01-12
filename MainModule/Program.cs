using System;
using RelevantFunction;
using System.Diagnostics;

namespace Non_ParallelVersion
{
    public class Program
    {
        private static int _var4Swap;

        static void Main(string[] args)
        {
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
            GetMaxValueThenPlaceToEnd(array);
            Sort(array, 0, array.Length - 1);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            bool result = Relevant.VerifySequence(array);
            Console.WriteLine(result);
        }

        private static void GetMaxValueThenPlaceToEnd(int[] array)
        {
            int maxEleIndex = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > array[maxEleIndex])
                    maxEleIndex = i;
            }

            Swap(array, maxEleIndex, array.Length - 1);
        }

        private static void Sort(int[] array, int lo, int hi)
        {
            if (hi <= lo) return;
            int middleEleIndex = Partition(array, lo, hi);
            Sort(array, lo, middleEleIndex - 1);
            Sort(array, middleEleIndex + 1, hi);
        }

        private static int Partition(int[] array, int lo, int hi)
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

        private static void Swap(int[] array, int e1, int e2)
        {
            _var4Swap = array[e1];
            array[e1] = array[e2];
            array[e2] = _var4Swap;
        }

    }
}
