using RelevantFunction;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MultiThreadingVersion
{
    class Program
    {
        static ConcurrentBag<Task> bag = new ConcurrentBag<Task>();

        static void Main(string[] args)
        {
            int[] array = Relevant.GenerateRandomIntergers(20_000_000, 0, 10000);
            array[array.Length - 1] = int.MaxValue;

            Task mainTask = null;
            mainTask = new Task(() => Sort(array, 0, array.Length - 1, mainTask));
            mainTask.Start();


            //Task.Factory.StartNew(() => Sort(array, 0, array.Length - 1)).Wait();


            //bool result = Relevant.VerifySequence(array);
            //Console.WriteLine(result);
            //Console.ReadKey();

            //int taskCount = 100;
            while (true)
            {  //
               //if (taskCount == bag.Count)
               //{
               //    break;
               //}
               //else
               //{
               //    taskCount = bag.Count;
               //}
                SpinWait.SpinUntil(() => false, 500);
            }
        }

        static void Sort(int[] array, int lo, int hi, Task t)
        {
            if (hi <= lo) return;
            int middleEleIndex = Partition(array, lo, hi);

            //Task.Factory.StartNew(() => Sort(array, lo, middleEleIndex - 1)).Wait();
            //Task.Factory.StartNew(() => Sort(array, middleEleIndex + 1, hi)).Wait();

            //new Task(() => Sort(array, lo, middleEleIndex - 1), TaskCreationOptions.AttachedToParent).Start();
            //new Task(() => Sort(array, middleEleIndex + 1, hi), TaskCreationOptions.AttachedToParent).Start();

            //Task tLeft = 
            t.ContinueWith((task) => Sort(array, lo, middleEleIndex - 1, task));
            //Task tRight = 
            t.ContinueWith((task) => Sort(array, middleEleIndex + 1, hi, task));
            //bag.Add(tLeft);
            //bag.Add(tRight);
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
