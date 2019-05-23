using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;

namespace RayanCnc.LSConnection.DataTypeStrategy
{
    internal class ContinuousTypeStrategy : TypeStrategy
    {
        public ContinuousTypeStrategy(Type valueType) => ValueType = valueType;

        public override LsDataType DataType => LsDataType.Continuous;
        public override Type ValueType { get; }

        public override byte[] CreateWriteInstructionBytes(ILsAddress address, object value)
        {
            var byteValue = (byte[])value;
            var insBytes = new byte[2 + byteValue.Length];
            System.Buffer.BlockCopy(byteValue, 0, insBytes, 2, byteValue.Length);
            insBytes[0] = address.ValueSizeInstructionHeaderBytes[0];
            insBytes[1] = address.ValueSizeInstructionHeaderBytes[1];
            return insBytes;
        }

        public override object HandleReadOperation(byte[] raw) => ParseReadData(raw);

        public override object ParseReadData(byte[] data)
        {
            var array = new byte[data[30] + (data[31] * 256)];
            Buffer.BlockCopy(data, 32, array, 0, array.Length);
            return array;
        }
    }
}
