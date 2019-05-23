using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;
using RayanCNC.LSConnection.Exceptions;

namespace RayanCnc.LSConnection.DataTypeStrategy
{
    public abstract class TypeStrategy : ITypeStrategy
    {
        public abstract LsDataType DataType { get; }
        public abstract Type ValueType { get; }

        public static TypeStrategy GetDataTypeStrategy(Type valueType)
        {
            if (valueType == typeof(bool)) return new BitTypeStrategy(valueType);
            if (valueType == typeof(byte)) return new ByteTypeStrategy(valueType);
            if (valueType == typeof(ushort) || valueType == typeof(short)) return new WordTypeStrategy(valueType);
            if (valueType == typeof(uint) || valueType == typeof(int)) return new DwordTypeStrategy(valueType);
            if (valueType == typeof(byte[])/* || valueType == typeof(List<byte>)*/) return new ContinuousTypeStrategy(valueType);
            throw new ArgumentException("The type of packet is not valid .please check your type first");
        }

        public abstract byte[] CreateWriteInstructionBytes(ILsAddress address, object value);

        public abstract object HandleReadOperation(byte[] raw);

        public virtual void HandleWriteOperation(byte[] raw)
        {
            if (raw[raw.Length - 2] != 1)
                throw new LsWriteOperationException();
        }

        public abstract object ParseReadData(byte[] data);
    }
}
