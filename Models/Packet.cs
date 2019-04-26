using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;
using RayanCNC.LSConnection.LsAddress;
using RayanCNC.LSConnection.LsAddress.Contracts;

namespace RayanCnc.LSConnection.Models
{
    public class Packet<T> : IPacket<T>
    {
        public Packet(T value, ILsAddress address)
        {
            Init();
            Value = value;
            Address = address;
            ActionType = ActionType.Write;
        }

        public Packet(ILsAddress address)
        {
            Init();
            Address = address;
            ActionType = ActionType.Read;
        }

        public Packet(T value, string address)
        {
            Init();
            Value = value;
            Address = new LsAddress(address);
            ActionType = ActionType.Write;
        }

        public Packet(string address)
        {
            Init();
            Address = new LsAddress(address);
            ActionType = ActionType.Read;
        }

        private void Init()
        {
            if (typeof(T) == typeof(bool)) DataTpe = LsDataType.Bit;
            else if (typeof(T) == typeof(byte)) DataTpe = LsDataType.Byte;
            else if (typeof(T) == typeof(ushort)) DataTpe = LsDataType.Word;
            else if (typeof(T) == typeof(uint)) DataTpe = LsDataType.Dword;
            else if (typeof(T) == typeof(byte[])) DataTpe = LsDataType.Continuous;
            else throw new ArgumentException("The type of packet is not valid .please check your type first");
            Id = Guid.NewGuid();
            Order = s_orderCounter++;
            CreateByteArray();
        }

        private static ushort s_orderCounter = 0;//!DONE!//
        public Guid Id { set; get; }
        public T Value { set; get; }
        public ILsAddress Address { get; set; }
        public LsDataType DataTpe { set; get; }
        public ushort Order { get; set; }
        public ActionType ActionType { get; set; }
        public byte[] RequestPacketHeader { get; set; }
        public byte[] RequestPacketInformation { get; set; }
        public byte[] RequestPackeInstruction { get; set; }

        public byte[] RawData
        {
            get
            {
                byte[] rv = new byte[RequestPacketHeader.Length + RequestPackeInstruction.Length + RequestPacketInformation.Length];
                Buffer.BlockCopy(RequestPacketHeader, 0, rv, 0, RequestPacketHeader.Length);
                Buffer.BlockCopy(RequestPacketInformation, 0, rv, RequestPacketHeader.Length, RequestPacketInformation.Length);
                Buffer.BlockCopy(RequestPackeInstruction, 0, rv, RequestPacketHeader.Length + RequestPacketInformation.Length, RequestPackeInstruction.Length);
                return rv;
            }
        }

        private void CreateByteArray()
        {
            CreatePacketInstruction();
            CreatePacketInformation();
            RequestPacketHeader = LSConnection.DefaultPlcModel.PacketHeader;
        }

        private void CreatePacketInstruction()
        {
            byte[] insHead = CreateInstructionBasicHeader();

            switch (DataTpe)
            {
                case LsDataType.Bit:
                    break;

                case LsDataType.Byte:
                    break;

                case LsDataType.Word:
                    break;

                case LsDataType.Dword:
                    break;

                case LsDataType.Continuous:
                    break;

                case LsDataType.NotSeted:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private byte[] CreateInstructionBasicHeader()
        {
            byte[] insHead = new byte[10];

            if (ActionType == ActionType.Read)
            {
                insHead[0] = 0x54;//read
                insHead[1] = 0x00;//read
            }
            else
            {
                insHead[0] = 0x58;//write
                insHead[1] = 0x00;//write
            }
            insHead[2] = Address.DataTypeInstructionHeaderBytes[0];
            insHead[3] = Address.DataTypeInstructionHeaderBytes[1];
            insHead[4] = 0x00;
            insHead[5] = 0x00;
            insHead[6] = 0x01;
            insHead[7] = 0x00;
            insHead[8] = (byte)Address.MemoryAddress.Length;
            insHead[9] = 0x00;
            return insHead;
        }

        private void CreatePacketInformation()
        {
            RequestPacketInformation = new byte[6];
            byte[] byteArray = BitConverter.GetBytes(s_orderCounter);
            RequestPacketInformation[0] = byteArray[0];
            RequestPacketInformation[1] = byteArray[1];
            byteArray = BitConverter.GetBytes((ushort)RequestPackeInstruction.Length);
            RequestPacketInformation[2] = byteArray[0];
            RequestPacketInformation[3] = byteArray[1];
            int checkSum = CalculateCheckSum(RequestPacketInformation);
            byteArray = BitConverter.GetBytes(checkSum);
            RequestPacketInformation[4] = byteArray[0];
            RequestPacketInformation[5] = byteArray[1];
        }

        private int CalculateCheckSum(byte[] header)
        {
            int sum = 0;
            for (int i = 0; i < 5; i++)
                sum += header[i];
            sum += 760;
            return sum;
        }

        public DateTime CreatedOn { get; set; }
    }
}