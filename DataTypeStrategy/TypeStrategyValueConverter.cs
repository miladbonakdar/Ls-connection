using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayanCnc.LSConnection.DataTypeStrategy
{
    public static class TypeStrategyValueConverter
    {
        private static readonly Dictionary<Type, Func<byte[], object>> TypeStrategyValueConverterDictionary
            = new Dictionary<Type, Func<byte[], object>>
            {
                { typeof(bool),
                    (bytes) => bytes != null && bytes[0] == 0x01 },
                { typeof(byte),
                    (bytes) => bytes?[0] ?? 0x00},
                { typeof(ushort),
                    (bytes) => bytes != null? BitConverter.ToUInt16(bytes, bytes.Length - 2):0 },
                { typeof(short),
                    (bytes) => bytes != null? BitConverter.ToInt16(bytes, bytes.Length - 2):0 },
                { typeof(int),
                    (bytes) =>  bytes != null? BitConverter.ToInt32(bytes, bytes.Length - 4):0 },
                { typeof(uint),
                    (bytes) =>  bytes != null? BitConverter.ToUInt32(bytes, bytes.Length - 4):0 }
            };

        public static object Convert(Type valueType, byte[] valueBytes)
        {
            if (TypeStrategyValueConverterDictionary.Keys.All(k => k != valueType))
                throw new Exception("Type Strategy Value Converter not found");
            return System.Convert.ChangeType(TypeStrategyValueConverterDictionary[valueType](valueBytes),
                                            valueType);
        }
    }
}
