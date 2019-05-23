using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;

namespace RayanCnc.LSConnection.DataTypeStrategy
{
    internal class ByteTypeStrategy : TypeStrategy
    {
        public ByteTypeStrategy(Type valueType) => ValueType = valueType;

        public override LsDataType DataType => LsDataType.Byte;
        public override Type ValueType { get; }

        public override byte[] CreateWriteInstructionBytes(ILsAddress address, object value)
        {
            var insBytes = new byte[3];
            insBytes[0] = address.ValueSizeInstructionHeaderBytes[0];
            insBytes[1] = address.ValueSizeInstructionHeaderBytes[1];
            insBytes[2] = BitConverter.GetBytes((byte)value)[0];
            return insBytes;
        }

        public override object HandleReadOperation(byte[] raw) => ParseReadData(raw);

        public override object ParseReadData(byte[] data) => data[data.Length - 1];
    }
}
