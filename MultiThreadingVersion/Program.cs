using RelevantFunction;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Collections.Concurrent;

namespace MultiThreadingVersion
{
    public class Program
    {
        static int partitionCount = 0;
        static int endCount = 0;

        static void Main(string[] args)
        {
            int[] array = Relevant.GenerateRandomIntergers(1_000_000, 0, 10000);
            //array[array.Length - 1] = int.MaxValue;
            //
            //
            //
            //bool result = Relevant.VerifySequence(array);
            //Console.WriteLine(result);

            GetMaxValueThenPlaceToEnd(array);
            //Testing();
            Console.ReadKey();
        }

        public static void Testing()
        {
            int[] array = Relevant.GenerateRandomIntergers(300_000_000, 0, 1_000_000);

            Stopwatch sw = new Stopwatch();
            Console.WriteLine("MultiThreadingVersion:");
            sw.Start();

            GetMaxValueThenPlaceToEnd(array);
            Task mainTask = null;
            mainTask = new Task(() => Sort_Continuation(array, 0, array.Length - 1, mainTask));
            mainTask.Start();
            while (true)
            {
                Thread.Sleep(500);
                if (partitionCount == endCount - 1)
                {
                    break;
                }
            }
            //Sort_Parallel(array, 0, array.Length - 1);

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            bool result = Relevant.VerifySequence(array);
            Console.WriteLine(result);
        }


        static ConcurrentDictionary<int, int> pair = new ConcurrentDictionary<int, int>();

        //[lo,hi)
        private static void GetMaxValueFromSubArray(int[] array, int lo, int hi)
        {
            int maxEleIndex = lo;
            for (int i = lo; i < hi; i++)
            {
                if (array[i] > array[maxEleIndex])
                    maxEleIndex = i;
            }

            bool result = pair.TryAdd(maxEleIndex, array[maxEleIndex]);
            if (!result)
            {
                Console.WriteLine("Error");
            }
        }

        private static void GetMaxValueThenPlaceToEnd(int[] array)
        {
            int pc = array.Length / 8;
            Parallel.Invoke(
                () => GetMaxValueFromSubArray(array, 0, pc),
                () => GetMaxValueFromSubArray(array, pc, 2 * pc),
                () => GetMaxValueFromSubArray(array, 2 * pc, 3 * pc),
                () => GetMaxValueFromSubArray(array, 3 * pc, 4 * pc),
                () => GetMaxValueFromSubArray(array, 4 * pc, 5 * pc),
                () => GetMaxValueFromSubArray(array, 5 * pc, 6 * pc),
                () => GetMaxValueFromSubArray(array, 6 * pc, 7 * pc),
                () => GetMaxValueFromSubArray(array, 7 * pc, array.Length));

            int maxEleNumber = pair.OrderByDescending(t => t.Value).First().Key;
            Swap(array, maxEleNumber, array.Length - 1);
        }



        static void Sort_Continuation(int[] array, int lo, int hi, Task t)
        {
            if (hi <= lo)
            {
                Interlocked.Increment(ref endCount);
                return;
            }

            Interlocked.Increment(ref partitionCount);
            int middleEleIndex = Partition(array, lo, hi);
            t.ContinueWith((task) => Sort_Continuation(array, lo, middleEleIndex - 1, task));
            Sort_Continuation(array, middleEleIndex + 1, hi, t);
        }

        static void Sort_Parallel(int[] array, int lo, int hi)
        {
            if (hi <= lo)
            {
                return;
            }

            int middleEleIndex = Partition(array, lo, hi);

            Parallel.Invoke(
                () => Sort_Parallel(array, lo, middleEleIndex - 1),
                () => Sort_Parallel(array, middleEleIndex + 1, hi));
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
