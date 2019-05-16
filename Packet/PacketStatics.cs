using System.Diagnostics.CodeAnalysis;

namespace RayanCnc.LSConnection.Packet
{
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public partial class Packet<T>
    {
        private static readonly byte[] InstructionHeader = { 0x00, 0x00, 0x01, 0x00, 0x00 };
        private static readonly byte[] ReadInstructionHeader = { 0x54, 0x00 };
        private static readonly byte[] WriteInstructionHeader = { 0x58, 0x00 };
        private static readonly byte WriteResponseFlag = 0x59;
        private static readonly byte ReadResponseFlag = 0x55;
        private static ushort _orderCounter;
    }
}
