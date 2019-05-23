using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;

namespace RayanCnc.LSConnection.DataTypeStrategy
{
    internal class DwordTypeStrategy : TypeStrategy
    {
        public DwordTypeStrategy(Type valueType) => ValueType = valueType;

        public override LsDataType DataType => LsDataType.Dword;
        public override Type ValueType { get; }

        public override byte[] CreateWriteInstructionBytes(ILsAddress address, object value)
        {
            var insBytes = new byte[6];
            insBytes[0] = address.ValueSizeInstructionHeaderBytes[0];
            insBytes[1] = address.ValueSizeInstructionHeaderBytes[1];
            var convertedDWordBytes = BitConverter.GetBytes((uint)value);
            insBytes[2] = convertedDWordBytes[0];
            insBytes[3] = convertedDWordBytes[1];
            insBytes[4] = convertedDWordBytes[2];
            insBytes[5] = convertedDWordBytes[3];
            return insBytes;
        }

        public override object HandleReadOperation(byte[] raw) => ParseReadData(raw);

        public override object ParseReadData(byte[] data) =>
            TypeStrategyValueConverter.Convert(ValueType, data);
    }
}
