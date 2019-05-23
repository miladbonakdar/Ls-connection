using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;
using RayanCNC.LSConnection.Exceptions;

namespace RayanCnc.LSConnection.DataTypeStrategy
{
    internal class BitTypeStrategy : TypeStrategy
    {
        public BitTypeStrategy(Type valueType) => ValueType = valueType;

        public override LsDataType DataType => LsDataType.Bit;
        public override Type ValueType { get; }

        public override byte[] CreateWriteInstructionBytes(ILsAddress address, object value)
        {
            var insBytes = new byte[3];
            insBytes[0] = address.ValueSizeInstructionHeaderBytes[0];
            insBytes[1] = address.ValueSizeInstructionHeaderBytes[1];
            insBytes[2] = BitConverter.GetBytes((bool)value)[0];
            return insBytes;
        }

        public override object HandleReadOperation(byte[] raw) => ParseReadData(raw);

        public override object ParseReadData(byte[] data) => data[data.Length - 1] == 0x01;
    }
}
