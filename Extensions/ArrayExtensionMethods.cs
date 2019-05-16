using System;

namespace RayanCNC.LSConnection.Extensions
{
    public static class ArrayExtensionMethods
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static string[] ToStringArray<T>(this T[] data)
        {
            string[] stringArray = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
                stringArray[i] = data[i].ToString();
            return stringArray;
        }
    }
}
