using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vector3 = CommonLibrary.Vector3;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var vec = new Vector3(9, 2, 3);
            // var properties = typeof(Vector3).GetFields();
            //
            // foreach (var property in properties)
            // {
            //     Console.WriteLine(property.FieldType);
            //     var value = property;
            //     //var x = propertya.GetRawConstantValue();
            //     Assembly.GetAssembly(typeof(Vector3))
            //     var x = property.GetValue();
            //
            //     Console.WriteLine(x.ToString());
            // }
            var properties = typeof(Vector3).GetFields();
            

            // var buf = PacketSerializer.Serialize(new Vector3(0, 2, 4));
       //var vec = PacketSerializer.Deserialize(buf);
       //Console.WriteLine($"{vec.x}, {vec.y}, {vec.z}");
            return;

        }

        public static byte[] Serialize<T>(T t)
        {
            var info = typeof(T).GetTypeInfo();
            if (info.IsNullable())
            {
                info = info.GenericTypeArguments[0].GetTypeInfo();
                if (!info.IsEnum) {return null;}
            }
        }
    }

    public static class ReflectionExtensions
    {
        public static bool IsNullable(this TypeInfo typeInfo) => typeInfo.IsGenericType &&
                                                                 typeInfo.GetGenericTypeDefinition() ==
                                                                 typeof(System.Nullable<>);
    }
}