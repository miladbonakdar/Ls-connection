using System;
using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System.Text;

namespace RayanCNC.LSConnection.LsAddress
{
    public partial class LsAddress : ILsAddress
    {
        public LsAddress(string addressString)
        {
            var address = Parse(addressString);
            LsDataType = address.LsDataType;
            StartAddressBit = address.StartAddressBit;
            EndAddressBit = address.EndAddressBit;
            MemoryAddress = address.MemoryAddress;
            DataTypeInstructionHeaderBytes = address.DataTypeInstructionHeaderBytes;
            ValueSizeInstructionHeaderBytes = GetDataTypeValueBytes();
        }

        public LsAddress(long address, LsDataType dataType)
        {
            LsDataType = dataType;
            StartAddressBit = GetValidAddressInPlcRange(address, dataType);
            EndAddressBit = GetValidAddressInPlcRange(address + 1, dataType);
            MemoryAddress = "%M" + GetDataTypeString(dataType) + address.ToString();
            DataTypeInstructionHeaderBytes = GetDataTypeInstructionHeaderBytes(dataType);
            ValueSizeInstructionHeaderBytes = GetDataTypeValueBytes();
        }

        public LsAddress(long startByteAddress, long endByteAddress)
        {
            LsDataType = LsDataType.Continuous;
            StartAddressBit = GetValidAddressInPlcRange(startByteAddress, LsDataType);
            EndAddressBit = GetValidAddressInPlcRange(endByteAddress, LsDataType);
            MemoryAddress = "%M" + GetDataTypeString(LsDataType) + startByteAddress.ToString();
            DataTypeInstructionHeaderBytes = GetDataTypeInstructionHeaderBytes(LsDataType);
            ValueSizeInstructionHeaderBytes = GetDataTypeValueBytes();
        }

        private LsAddress()
        {
        }

        public byte[] AddressBytes => Encoding.ASCII.GetBytes(MemoryAddress);

        public byte[] ContinuousAddressDifference => BitConverter.GetBytes(EndAddressByte - StartAddressByte);

        public byte[] DataTypeInstructionHeaderBytes { get; private set; }

        /// <summary>
        /// plc end address in bit
        /// </summary>
        public long EndAddressBit { get; set; }

        public long EndAddressByte => EndAddressBit / 8;

        public long EndAddressDWord => EndAddressBit / 32;

        public long EndAddressWord => EndAddressBit / 16;

        public LsDataType LsDataType { get; set; }

        public string MemoryAddress { get; set; }

        /// <summary>
        /// plc start address in bit
        /// </summary>
        public long StartAddressBit { set; get; }

        public long StartAddressByte => StartAddressBit / 8;
        public long StartAddressDWord => StartAddressBit / 32;
        public long StartAddressWord => StartAddressBit / 16;

        // 2 bytes for data type header
        // address of the memory in bytes
        public byte[] ValueSizeInstructionHeaderBytes { get; private set; }// address of the memory in bytes
    }
}
