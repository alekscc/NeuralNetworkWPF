using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkWPF
{
    public static class Utils
    {
        public static int[] ArrayToIntTree<T>(T[][] array)
        {
            int[] result = new int[array.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = array[i].Length;

            return result;
        }
        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        public static void ConsoleDisplay2DArray<T>(T[][] array)
        {
            foreach(T[] x in array)
            {
                foreach(T y in x)
                {
                    Console.Write(y.ToString() + "\t");
                }

                Console.WriteLine("\n+++++++++++++++++++++++++++++");
            }
        }
    }
}
