using System.Net;

namespace RayanCnc.LSConnection
{
    public static class LsConnectionStatics
    {
        public static long DefaultMemorySize => 256000;
        public static byte[] DefaultPacketHeader { get; } = { 0x4C, 0x47, 0x49, 0x53, 0x2D, 0x47, 0x4C, 0x4F, 0x46, 0x41, 0x00, 0x00, 0x00, 0x33 };
        public static IPAddress DefaultPlcIpAddress => IPAddress.Parse("127.0.0.1");
        public static string DefaultPlcName => "TestName";
        public static int DefaultPlcPortNumber => 2004;
        public static int MaxPlcResponseLength => 40000;//bytes
        //in bit => 8 * 4 * 8000
    }
}
