using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.Exceptions;
using RayanCNC.LSConnection.Extensions;
using System;
using System.Linq;

namespace RayanCNC.LSConnection.LsAddress
{
    public partial class LsAddress
    {
        private static readonly char[] MemoryTypeChars = { 'X', 'B', 'W', 'D' };

        public static long BaseMemorySize { get; set; } = RayanCnc.LSConnection.LsConnection.DefaultPlcModel.MemorySize;

        public static LsAddress Parse(string addressString)
        {
            if (!ValidateAddressString(ref addressString)) throw new BadLsAddressException();
            return ParseString(addressString);
        }

        public static bool TryParse(string addressString, out LsAddress lsAddress)
        {
            lsAddress = null;
            if (!ValidateAddressString(ref addressString)) return false;
            lsAddress = ParseString(addressString);
            return true;
        }

        public static bool ValidateAddressString(ref string addressString)
        {
            addressString = addressString.ToUpper();
            if (!addressString.StartWith(new[] { "%", "M", "X", "B", "W", "D" }) || !addressString.ShouldHaveJustOne(MemoryTypeChars)) return false;
            if (addressString.StartsWith("M"))
                addressString = "%" + addressString;
            if (addressString.StartWith(MemoryTypeChars.ToStringArray()))
                addressString = "%M" + addressString;
            string[] subStrings = addressString.Split(',');
            if (subStrings.Length > 2 || subStrings.Length == 0) return false;
            try
            {
                string[] chunks = subStrings[0].SplitWithOne(MemoryTypeChars);
                long test = 0;
                if (subStrings.Length == 2 && !long.TryParse(subStrings[1], out test)) return false;
                if (!long.TryParse(chunks[1], out test)) return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static Tuple<long, long?> GetAddressTuple(string address)
        {
            string[] subStrings = address.Split(',');
            string[] chunks = subStrings[0].SplitWithOne(new[] { 'X', 'B', 'W', 'D' });
            return new Tuple<long, long?>(long.Parse(chunks[1]), (subStrings.Length == 2) ? (long?)long.Parse(subStrings[1]) : null);
        }

        private static LsDataType GetDataType(string addressString)
        {
            switch (MemoryTypeChars.First(c => addressString.Contains(c.ToString())))
            {
                case 'X':
                    return RayanCnc.LSConnection.Models.LsDataType.Bit;

                case 'B':
                    if (!addressString.Contains(",")) return RayanCnc.LSConnection.Models.LsDataType.Byte;
                    var addressTuple = GetAddressTuple(addressString);
                    if (!addressTuple.Item2.HasValue) return RayanCnc.LSConnection.Models.LsDataType.Byte;
                    return addressTuple.Item2.Value - addressTuple.Item1 == 1 ? RayanCnc.LSConnection.Models.LsDataType.Byte : RayanCnc.LSConnection.Models.LsDataType.Continuous;

                case 'W':
                    return RayanCnc.LSConnection.Models.LsDataType.Word;

                case 'D':
                    return RayanCnc.LSConnection.Models.LsDataType.Dword;

                default: return RayanCnc.LSConnection.Models.LsDataType.Byte;
            }
        }

        private static byte[] GetDataTypeInstructionHeaderBytes(LsDataType lsDataType)
        {
            switch (lsDataType)
            {
                case LsDataType.Bit:
                    return new byte[] { 0x00, 0x00 };

                case LsDataType.Byte:
                    return new byte[] { 0x01, 0x00 };

                case LsDataType.Word:
                    return new byte[] { 0x02, 0x00 };

                case LsDataType.Dword:
                    return new byte[] { 0x03, 0x00 };

                case LsDataType.Continuous:
                    return new byte[] { 0x14, 0x00 };

                default:
                    throw new Exception("Unmanaged data type");
            }
        }

        private static string GetDataTypeString(LsDataType dataType)
        {
            switch (dataType)
            {
                case LsDataType.Bit:
                    return "X";

                case LsDataType.Byte:
                    return "B";

                case LsDataType.Word:
                    return "W";

                case LsDataType.Dword:
                    return "D";

                case LsDataType.Continuous:
                    return "B";

                default:
                    return "B";
            }
        }

        private static long GetValidAddressInPlcRange(long address, LsDataType dataType = LsDataType.Bit)
        {
            switch (dataType)
            {
                case LsDataType.Continuous:
                case LsDataType.Byte:
                    address *= 8;
                    break;

                case LsDataType.Word:
                    address *= 16;
                    break;

                case LsDataType.Dword:
                    address *= 32;
                    break;
            }

            while (address > BaseMemorySize) address -= BaseMemorySize;
            while (address < 0) address += BaseMemorySize;
            return address;
        }

        private static LsAddress ParseString(string addressString)
        {
            var dataType = GetDataType(addressString);
            var addressTuple = GetAddressTuple(addressString);
            LsAddress address = new LsAddress
            {
                LsDataType = dataType,
                StartAddressBit = GetValidAddressInPlcRange(addressTuple.Item1, dataType),
                EndAddressBit = addressTuple.Item2 != null
                    ? GetValidAddressInPlcRange(addressTuple.Item2.Value, dataType)
                    : GetValidAddressInPlcRange(addressTuple.Item1 + 1, dataType),
                MemoryAddress = addressString.Split(',')[0]
            };
            address.DataTypeInstructionHeaderBytes = GetDataTypeInstructionHeaderBytes(address.LsDataType);
            return address;
        }

        private byte[] GetDataTypeValueBytes()
        {
            switch (LsDataType)
            {
                case LsDataType.Bit:
                    return new byte[] { 0x1, 0x00 };

                case LsDataType.Byte:
                    return new byte[] { 0x01, 0x00 };

                case LsDataType.Word:
                    return new byte[] { 0x02, 0x00 };

                case LsDataType.Dword:
                    return new byte[] { 0x04, 0x00 };

                case LsDataType.Continuous:
                    byte[] valueSize = BitConverter.GetBytes(EndAddressByte - StartAddressByte);
                    return new byte[] { valueSize[0], valueSize[1] };

                default:
                    throw new Exception("Unmanaged data type");
            }
        }
    }
}
