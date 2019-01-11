using System;

namespace RelevantFunction
{
    public class Relevant
    {
        // [min,max]
        public static int[] GenerateRandomIntergers(int count, int min, int max)
        {
            Random rand = new Random();
            int[] array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = rand.Next(min, max + 1);
            }
            return array;
        }

        public static bool VerifySequence(int[] sourceArray)
        {
            int[] array = new int[sourceArray.Length + 1];      //use sentinel
            Array.Copy(sourceArray, array, sourceArray.Length);
            array[array.Length - 1] = int.MinValue;

            int i = 0;
            while (array[i] <= array[i + 1])
            {
                i++;
            }

            return i == array.Length - 2;
        }
    }
}
