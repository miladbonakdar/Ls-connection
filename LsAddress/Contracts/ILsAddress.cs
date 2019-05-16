using RayanCnc.LSConnection.Models;

namespace RayanCNC.LSConnection.LsAddress.Contracts
{
    public interface ILsAddress
    {
        byte[] AddressBytes { get; }
        byte[] ContinuousAddressDifference { get; }
        byte[] DataTypeInstructionHeaderBytes { get; }
        long EndAddressBit { set; get; }
        long EndAddressByte { get; }
        long EndAddressDWord { get; }
        long EndAddressWord { get; }
        LsDataType LsDataType { set; get; }
        string MemoryAddress { set; get; }
        long StartAddressBit { set; get; }
        long StartAddressByte { get; }
        long StartAddressDWord { get; }
        long StartAddressWord { get; }
        byte[] ValueSizeInstructionHeaderBytes { get; }
    }
}
