using System;
using SingleThreadingVersion;
using MultiThreadingVersion;
using RelevantFunction;
using System.Diagnostics;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] array = Relevant.GenerateRandomIntergers(600_000_000, 0, 1_000_000);
            //array[array.Length - 1] = int.MaxValue;
            //
            //int[] array2 = new int[array.Length];
            //Array.Copy(array, array2, array.Length);
            //
            //Stopwatch sw = new Stopwatch();
            //Console.WriteLine("SingleThreadingVersion:");
            //sw.Start();
            //SingleThreadingVersion.Program.test(array);
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);
            //bool result = Relevant.VerifySequence(array);
            //Console.WriteLine(result);
            //
            //GC.Collect();
            //
            //sw.Reset();
            //Console.WriteLine("MultiThreadingVersion:");
            //sw.Start();
            //MultiThreadingVersion.Program.Process(array2);
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);
            //result = Relevant.VerifySequence(array);
            //Console.WriteLine(result);
            //
            //Console.ReadKey();
        }
    }
}
