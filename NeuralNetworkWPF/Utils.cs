﻿using System;
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
        public static string IntArrayToString(int[] array)
        {
            StringBuilder strBuilder = new StringBuilder();

            foreach (int i in array)
                strBuilder.Append(i + ",");

            return strBuilder.ToString();

        }
        public static int[] StringToIntArray(string str)
        {
            string[] data = str.Split(',').ToArray();

            int[] array = new int[data.Length-1];

            for(int i =0;i<array.Length;i++)
            {
                array[i] = int.Parse(data[i]);
            }

            return array;

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
        public static T[] SubArray2<T>(T[] array, int i, int n, bool isInverted = false)
        {
            int len = array.Length / n;

            T[] sub = new T[len];
            T[] sub22 = new T[array.Length - len];

            int r1 = len * i,
                r2 = len * (i + 1);

            for (int j = 0, x = 0, y = 0; j < array.Length; j++)
            {
                if (j >= r1 && j < r2)
                    sub[x++] = array[j];
                else
                    sub22[y++] = array[j];
            }

            return (!isInverted) ? sub : sub22;
        }
    }
}
