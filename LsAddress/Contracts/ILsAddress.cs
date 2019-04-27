using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Models;

namespace RayanCNC.LSConnection.LsAddress.Contracts
{
    public interface ILsAddress
    {
        long StartAddressBit { set; get; }
        long EndAddressBit { set; get; }
        long StartAddressByte { get; }
        long EndAddressByte { get; }
        long StartAddressWord { get; }
        long EndAddressWord { get; }
        long StartAddressDWord { get; }
        long EndAddressDWord { get; }
        string MemoryAddress { set; get; }
        byte[] DataTypeInstructionHeaderBytes { get; }
        byte[] AddressBytes { get; }
        byte[] ValueSizeInstructionHeaderBytes { get; }
        LsDataType LsDataType { set; get; }
    }
}