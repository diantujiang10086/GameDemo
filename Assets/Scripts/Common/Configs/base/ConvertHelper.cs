using System;
using Unity.Mathematics;

public class ConvertHelper
{
    public static object ChangeType<T>(string value)
    {
        return ChangeType(value, typeof(T));
    }

    public static object ChangeType(string value, Type conversionType)
    {
        if (string.IsNullOrEmpty(value))
            return default;

        if (conversionType.IsEnum)
        {
            if (Enum.TryParse(conversionType, value, out var result))
            {
                return result;
            }
            else
            {
                return default;
            }
        }
        
        if (conversionType == typeof(string))
            return value;

        if (conversionType.IsArray)
        {
            string[] array1 = value.Split('#');
            if (conversionType.Name == "String[]")
            {
                return array1;
            }
            if (conversionType.Name == "Int32[]")
            {
                int[] array2 = new int[array1.Length];
                for (int n = 0; n < array1.Length; n++)
                {
                    array2[n] = int.Parse(array1[n]);
                }
                return array2;
            }
            if (conversionType.Name == "Single[]")
            {
                float[] array2 = new float[array1.Length];
                for (int n = 0; n < array1.Length; n++)
                {
                    array2[n] = float.Parse(array1[n]);
                }
                return array2;
            }
            if (conversionType.Name == "Int16[]")
            {
                short[] array2 = new short[array1.Length];
                for (int n = 0; n < array1.Length; n++)
                {
                    array2[n] = short.Parse(array1[n]);
                }
                return array2;
            }
            if (conversionType.Name == "Byte[]")
            {
                byte[] array2 = new byte[array1.Length];
                for (int n = 0; n < array1.Length; n++)
                {
                    array2[n] = byte.Parse(array1[n]);
                }
                return array2;
            }
            if (conversionType.Name == "Int64[]")
            {
                long[] array2 = new long[array1.Length];
                for (int n = 0; n < array1.Length; n++)
                {
                    array2[n] = long.Parse(array1[n]);
                }
                return array2;
            }
            if (conversionType.Name == "Double[]")
            {
                double[] array2 = new double[array1.Length];
                for (int n = 0; n < array1.Length; n++)
                {
                    array2[n] = double.Parse(array1[n]);
                }
                return array2;
            }
        }

        if (conversionType.Name == "Vector2")
        {
            var array = value.Split('#');
            return value.Split('#').Length switch
            {
                1 => new float2(float.Parse(array[0]), 0),
                2 => new float2(float.Parse(array[0]), float.Parse(array[1])),
                _ => float2.zero
            };
        }
        else if (conversionType.Name == "Vector3")
        {
            var array = value.Split('#');
            return value.Split('#').Length switch
            {
                1 => new float3(float.Parse(array[0]), 0, 0),
                2 => new float3(float.Parse(array[0]), float.Parse(array[1]), 0),
                3 => new float3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2])),
                _ => float3.zero
            };
        }
        else if (conversionType.Name == "Vector2Int")
        {
            var array = value.Split('#');
            return value.Split('#').Length switch
            {
                1 => new int2(int.Parse(array[0]), 0),
                2 => new int2(int.Parse(array[0]), int.Parse(array[1])),
                _ => int2.zero
            };
        }
        else if (conversionType.Name == "Vector3Int")
        {
            var array = value.Split('#');
            return value.Split('#').Length switch
            {
                1 => new int3(int.Parse(array[0]), 0, 0),
                2 => new int3(int.Parse(array[0]), int.Parse(array[1]), 0),
                3 => new int3(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2])),
                _ => int3.zero
            };
        }
        else if (conversionType.Name == "Quaternion")
        {
            var array = value.Split('#');
            return value.Split('#').Length switch
            {
                1 => quaternion.Euler(float.Parse(array[0]), 0, 0),
                2 => quaternion.Euler(float.Parse(array[0]), float.Parse(array[1]), 0),
                3 => quaternion.Euler(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2])),
                4 => new quaternion(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3])),
                _ => quaternion.identity
            };
        }

        return Convert.ChangeType(value, conversionType);
    }
}