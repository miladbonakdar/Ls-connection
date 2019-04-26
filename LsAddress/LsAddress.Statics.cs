using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RayanCnc.LSConnection;
using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.Exceptions;
using RayanCNC.LSConnection.Extentions;

namespace RayanCNC.LSConnection.LsAddress
{
    public partial class LsAddress
    {
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

        private static LsAddress ParseString(string addressString)
        {
            var addressTuple = GetAddressTuple(addressString);
            LsAddress address = new LsAddress
            {
                LsDataType = GetDataType(addressString),
                StartAddressBit = addressTuple.Item1
            };
            address.EndAddressBit = addressTuple.Item2 ?? addressTuple.Item1 + GetDataTypeAddressDistance(address.LsDataType);
            address.MemoryAddress = addressString.Split(',')[0];
            address.DataTypeInstructionHeaderBytes = GetDataTypeInstructionHeaderBytes(address.LsDataType);
            return address;
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
                    throw new Exception("Unmanaged datatype");
            }
        }

        private static ushort GetDataTypeAddressDistance(LsDataType lsDataType)
        {
            switch (lsDataType)
            {
                case LsDataType.Bit:
                    return 1;

                case LsDataType.Byte:
                case LsDataType.Continuous:
                    return 8;

                case LsDataType.Word:
                    return 16;

                case LsDataType.Dword:
                    return 32;

                default:
                    throw new Exception("Unmanaged datatype");
            }
        }

        private static long GetValidAddressInPlcRange(long address, LsDataType dataType = LsDataType.NotSeted)
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

            while (address > SBaseMemorySize) address -= SBaseMemorySize;
            while (address < 0) address += SBaseMemorySize;
            return address;
        }

        private static LsDataType GetDataType(string addressString)
        {
            switch (_sMemoryTypeChars.First(c => addressString.Contains(c.ToString())))
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

        private static Tuple<long, long?> GetAddressTuple(string address)
        {
            string[] subStrings = address.Split(',');
            string[] chunks = subStrings[0].SplitWithOne(new[] { 'X', 'B', 'W', 'D' });
            return new Tuple<long, long?>(long.Parse(chunks[1]), (subStrings.Length == 2) ? (long?)long.Parse(subStrings[1]) : null);
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

                case LsDataType.NotSeted:
                    return "B";

                default:
                    return "B";
            }
        }

        //TODO test here
        public static bool ValidateAddressString(ref string addressString)
        {
            addressString = addressString.ToUpper();
            if (!addressString.StartWith(new[] { "%", "M", "X", "B", "W", "D" }) || !addressString.ShouldHaveJustOne(_sMemoryTypeChars)) return false;
            if (addressString.StartsWith("M"))
                addressString = "%" + addressString;
            if (!addressString.StartWith(_sMemoryTypeChars.ToStringArray()))
                addressString = "%M" + addressString;
            string[] subStrings = addressString.Split(',');
            if (subStrings.Length > 2 || subStrings.Length == 0) return false;
            try
            {
                string[] chunks = subStrings[0].SplitWithOne(_sMemoryTypeChars);
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

        private static readonly char[] _sMemoryTypeChars = new[] { 'X', 'B', 'W', 'D' };
        public static long SBaseMemorySize { get; set; } = RayanCnc.LSConnection.LSConnection.DefaultPlcModel.MemorySize;
    }
}