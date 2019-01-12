using RelevantFunction;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MultiThreadingVersion
{
    public class Program
    {
        static int partitionCount = 0;
        static int endCount = 0;

        static void Main(string[] args)
        {
            //int[] array = Relevant.GenerateRandomIntergers(40_000_000, 0, 10000);
            //array[array.Length - 1] = int.MaxValue;
            //
            //
            //
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
            Console.WriteLine("MultiThreadingVersion:");
            sw.Start();

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
