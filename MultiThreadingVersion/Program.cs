using RelevantFunction;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelVersion
{
    public class Program
    {
        private static int _partitionCounter = 0;
        private static int _finishCounter = 0;
        private static int _lengthRequireNewThread;
        private static ConcurrentDictionary<int, int> _pair4MaxValues = new ConcurrentDictionary<int, int>();

        static void Main(string[] args)
        {
            Testing();
            Console.ReadKey();
        }

        public static void Testing()
        {
            int[] array = Relevant.GenerateRandomIntergers(300_000_000, 0, 1_000_000);

            Stopwatch sw = new Stopwatch();
            Console.WriteLine("MultiThreadingVersion:");
            sw.Start();

            GetMaxValueThenPlaceToEnd(array);
            _lengthRequireNewThread = array.Length / 256;
            Task mainTask = null;
            mainTask = new Task(() => Sort_Continuation(array, 0, array.Length - 1, mainTask));
            mainTask.Start();
            while (true)
            {
                Thread.Sleep(500);
                if (_partitionCounter == _finishCounter - 1)
                {
                    break;
                }
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            bool result = Relevant.VerifySequence(array);
            Console.WriteLine(result);
        }

        //[lo,hi)
        private static void GetMaxValueFromSubArray(int[] array, int lo, int hi)
        {
            int maxEleIndex = lo;
            for (int i = lo; i < hi; i++)
            {
                if (array[i] > array[maxEleIndex])
                    maxEleIndex = i;
            }

            bool result = _pair4MaxValues.TryAdd(maxEleIndex, array[maxEleIndex]);
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

            int maxEleIndex = _pair4MaxValues.OrderByDescending(t => t.Value).First().Key;
            Swap(array, maxEleIndex, array.Length - 1);
        }

        private static void Sort_Continuation(int[] array, int lo, int hi, Task t)
        {
            if (hi <= lo)
            {
                Interlocked.Increment(ref _finishCounter);
                return;
            }

            Interlocked.Increment(ref _partitionCounter);
            int middleEleIndex = Partition(array, lo, hi);

            if (middleEleIndex - lo > _lengthRequireNewThread)
                t.ContinueWith((task) => Sort_Continuation(array, lo, middleEleIndex - 1, task));
            else
                Sort_Recursion(array, lo, middleEleIndex - 1);

            if (hi - middleEleIndex > _lengthRequireNewThread)
                t.ContinueWith((task) => Sort_Continuation(array, middleEleIndex + 1, hi, task));
            else
                Sort_Recursion(array, middleEleIndex + 1, hi);
        }

        private static void Sort_Recursion(int[] array, int lo, int hi)
        {
            if (hi <= lo)
            {
                Interlocked.Increment(ref _finishCounter);
                return;
            }

            Interlocked.Increment(ref _partitionCounter);
            int middleEleIndex = Partition(array, lo, hi);

            Sort_Recursion(array, lo, middleEleIndex - 1);
            Sort_Recursion(array, middleEleIndex + 1, hi);
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
            if (e1 != e2)
            {
                array[e1] = array[e1] + array[e2];
                array[e2] = array[e1] - array[e2];
                array[e1] = array[e1] - array[e2];
            }
        }
    }
}
