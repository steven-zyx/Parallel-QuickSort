using RelevantFunction
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MultiThreadingVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = Relevant.GenerateRandomIntergers(10_000, 0, 10000);
            array[array.Length - 1] = int.MaxValue;
            //Sort(array, 0, array.Length - 1);
            bool result = Relevant.VerifySequence(array);
            Console.WriteLine(result);
            Console.ReadKey();
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
            if (e1 != e2)
            {
                array[e1] = array[e1] + array[e2];
                array[e2] = array[e1] - array[e2];
                array[e1] = array[e1] - array[e2];
            }
        }
    }
}
