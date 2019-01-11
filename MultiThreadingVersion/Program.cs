using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MultiThreadingVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        static void Swap(int[] array, int e1, int e2)
        {
            array[e1] = array[e1] + array[e2];
            array[e2] = array[e1] - array[e2];
            array[e1] = array[e1] - array[e2];
        }
    }
}
