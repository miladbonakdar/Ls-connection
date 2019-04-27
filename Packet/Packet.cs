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
            Value = value;
            Address = address;
            ActionType = ActionType.Write;
            Init();
        }

        public Packet(ILsAddress address)
        {
            Address = address;
            ActionType = ActionType.Read;
            Init();
        }

        public Packet(T value, string address)
        {
            Value = value;
            Address = new LsAddress(address);
            ActionType = ActionType.Write;
            Init();
        }

        public Packet(string address)
        {
            Address = new LsAddress(address);
            ActionType = ActionType.Read;
            Init();
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
            CreatePacketInstruction();//it should be first. because we need the array length
            CreatePacketInformation();
            RequestPacketHeader = LSConnection.DefaultPlcModel.PacketHeader;
        }

        private void CreatePacketInstruction()
        {
            byte[] insHead = CreateInstructionBasicHeader();
            if (ActionType == ActionType.Read)
            {
                RequestPackeInstruction = new byte[insHead.Length + Address.AddressBytes.Length];
                Buffer.BlockCopy(insHead, 0, RequestPackeInstruction, 0, insHead.Length);
                Buffer.BlockCopy(Address.AddressBytes, 0, RequestPackeInstruction, insHead.Length, Address.AddressBytes.Length);
            }
            else
            {
                var writeInstructionBytes = CreateWriteInstructionBytes();
                RequestPackeInstruction = new byte[insHead.Length + Address.AddressBytes.Length + writeInstructionBytes.Length];
                Buffer.BlockCopy(insHead, 0, RequestPackeInstruction, 0, insHead.Length);
                Buffer.BlockCopy(Address.AddressBytes, 0, RequestPackeInstruction, insHead.Length, Address.AddressBytes.Length);
                Buffer.BlockCopy(writeInstructionBytes, 0, RequestPackeInstruction, Address.AddressBytes.Length + insHead.Length, writeInstructionBytes.Length);
            }
        }

        private byte[] CreateWriteInstructionBytes()
        {
            byte[] valueBytes;
            switch (DataTpe)
            {
                case LsDataType.Bit:
                    valueBytes = new byte[3];
                    valueBytes[0] = Address.ValueSizeInstructionHeaderBytes[0];
                    valueBytes[1] = Address.ValueSizeInstructionHeaderBytes[1];
                    valueBytes[2] = BitConverter.GetBytes((bool)(object)Value)[0];
                    return valueBytes;

                case LsDataType.Byte:
                    valueBytes = new byte[3];
                    valueBytes[0] = Address.ValueSizeInstructionHeaderBytes[0];
                    valueBytes[1] = Address.ValueSizeInstructionHeaderBytes[1];
                    valueBytes[2] = BitConverter.GetBytes((byte)(object)Value)[0];
                    return valueBytes;

                case LsDataType.Word:
                    valueBytes = new byte[4];
                    valueBytes[0] = Address.ValueSizeInstructionHeaderBytes[0];
                    valueBytes[1] = Address.ValueSizeInstructionHeaderBytes[1];
                    var convertedWordBytes = BitConverter.GetBytes((ushort)(object)Value);
                    valueBytes[2] = convertedWordBytes[0];
                    valueBytes[3] = convertedWordBytes[1];
                    return valueBytes;

                case LsDataType.Dword:
                    valueBytes = new byte[6];
                    valueBytes[0] = Address.ValueSizeInstructionHeaderBytes[0];
                    valueBytes[1] = Address.ValueSizeInstructionHeaderBytes[1];
                    var convertedDWordBytes = BitConverter.GetBytes((uint)(object)Value);
                    valueBytes[2] = convertedDWordBytes[0];
                    valueBytes[3] = convertedDWordBytes[1];
                    valueBytes[4] = convertedDWordBytes[2];
                    valueBytes[5] = convertedDWordBytes[3];
                    return valueBytes;

                case LsDataType.Continuous:
                    valueBytes = new byte[2];
                    valueBytes[0] = Address.ValueSizeInstructionHeaderBytes[0];
                    valueBytes[1] = Address.ValueSizeInstructionHeaderBytes[1];
                    return valueBytes;

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

        public void SetRawValue(byte[] raw)
        {
            RawResponse = raw;
            throw new NotImplementedException();
        }

        public DateTime CreatedOn { get; set; }

        public byte[] RawResponse { get; private set; }
    }
}
