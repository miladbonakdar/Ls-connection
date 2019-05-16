using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;

namespace RayanCnc.LSConnection.DataTypeStrategy
{
    internal class WordTypeStrategy : TypeStrategy
    {
        public override LsDataType DataType => LsDataType.Word;

        public override byte[] CreateWriteInstructionBytes(ILsAddress address, object value)
        {
            var insBytes = new byte[4];
            insBytes[0] = address.ValueSizeInstructionHeaderBytes[0];
            insBytes[1] = address.ValueSizeInstructionHeaderBytes[1];
            var convertedWordBytes = BitConverter.GetBytes((ushort)value);
            insBytes[2] = convertedWordBytes[0];
            insBytes[3] = convertedWordBytes[1];
            return insBytes;
        }

        public override object HandleReadOperation(byte[] raw) => ParseReadData(raw);

        public override object ParseReadData(byte[] data) => BitConverter.ToUInt16(data, data.Length - 2);
    }
}
