using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection;
using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.Exceptions;
using RayanCNC.LSConnection.LsAddress.Contracts;

namespace RayanCNC.LSConnection.LsAddress
{
    public partial class LsAddress : ILsAddress
    {
        /// <summary>
        /// plc start address in bit
        /// </summary>
        public long StartAddressBit { set; get; } = 0;

        /// <summary>
        /// plc end address in bit
        /// </summary>
        public long EndAddressBit { get; set; } = 0;

        public long StartAddressByte => StartAddressBit / 8;
        public long EndAddressByte => EndAddressBit / 8;
        public long StartAddressWord => StartAddressBit / 16;
        public long EndAddressWord => EndAddressBit / 16;
        public long StartAddressDWord => StartAddressBit / 32;
        public long EndAddressDWord => EndAddressBit / 32;
        public string MemoryAddress { get; set; }
        public byte[] DataTypeInstructionHeaderBytes { get; private set; }// 2 bytes for data type header
        public byte[] AddressBytes => Encoding.ASCII.GetBytes(MemoryAddress);// address of the memory in bytes
        public byte[] ValueSizeInstructionHeaderBytes { get; private set; }// address of the memory in bytes
        public LsDataType LsDataType { get; set; }

        private LsAddress()
        {
        }

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
    }
}
