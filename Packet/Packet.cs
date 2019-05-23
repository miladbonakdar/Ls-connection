using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.DataTypeStrategy;
using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;
using RayanCnc.LSConnection.Extensions;

namespace RayanCnc.LSConnection.Packet
{
    public partial class Packet<T> : IPacket<T>
    {
        public Packet(T value, ILsAddress address, byte[] requestPacketHeader)
        {
            Value = value;
            Address = address;
            ActionType = ActionType.Write;
            Init(requestPacketHeader);
        }

        public Packet(T value, string address, byte[] requestPacketHeader)
        {
            Value = value;
            Address = new LsAddress(address);
            ActionType = ActionType.Write;
            Init(requestPacketHeader);
        }

        public Packet(ILsAddress address, byte[] requestPacketHeader)
        {
            Address = address;
            ActionType = ActionType.Read;
            Init(requestPacketHeader);
        }

        public Packet(string address, byte[] requestPacketHeader)
        {
            Address = new LsAddress(address);
            ActionType = ActionType.Read;
            Init(requestPacketHeader);
        }

        public ActionType ActionType { get; }

        public ILsAddress Address { get; }

        public DateTime CreatedOn { get; private set; }

        public LsDataType DataType => DataTypeStrategy.DataType;

        public ITypeStrategy DataTypeStrategy { get; private set; }

        //!DONE!//
        public Guid Id { get; private set; }

        public ushort Order { get; private set; }

        public byte[] RawRequest
        {
            get
            {
                byte[] rv = new byte[RequestPacketHeader.Length + RequestPacketInstruction.Length + RequestPacketInformation.Length];
                Buffer.BlockCopy(RequestPacketHeader, 0, rv, 0, RequestPacketHeader.Length);
                Buffer.BlockCopy(RequestPacketInformation, 0, rv, RequestPacketHeader.Length, RequestPacketInformation.Length);
                Buffer.BlockCopy(RequestPacketInstruction, 0, rv, RequestPacketHeader.Length + RequestPacketInformation.Length, RequestPacketInstruction.Length);
                return rv;
            }
        }

        public byte[] RawResponse { get; private set; }

        public byte[] RequestPacketHeader { get; private set; }

        public byte[] RequestPacketInformation { get; private set; }

        public byte[] RequestPacketInstruction { get; private set; }

        public DateTime ResponseOn { get; private set; }

        public T Value { get; private set; }

        public void ParseRawBytes(byte[] raw)
        {
            RawResponse = raw;
            ResponseOn = DateTime.Now;
            if (raw[20] == ReadResponseFlag)
            {
                Value = (T)DataTypeStrategy.HandleReadOperation(raw);
            }
            if (raw[20] == WriteResponseFlag)
                DataTypeStrategy.HandleWriteOperation(raw);
        }

        private ushort CalculateCheckSum(byte[] header)
        {
            ushort sum = 0;
            for (ushort i = 0; i < 5; i++)
                sum += header[i];
            sum += 760;
            return sum;
        }

        private void CreateByteArray()
        {
            CreatePacketInstruction();//it should be first. because we need the array length
            CreatePacketInformation();
        }

        private byte[] CreateInstructionBasicHeader()
        {
            byte[] insHead = new byte[10];

            if (ActionType == ActionType.Read)
            {
                insHead[0] = ReadInstructionHeader[0];
                insHead[1] = ReadInstructionHeader[1];
            }
            else
            {
                insHead[0] = WriteInstructionHeader[0];
                insHead[1] = WriteInstructionHeader[1];
            }
            insHead[2] = Address.DataTypeInstructionHeaderBytes[0];
            insHead[3] = Address.DataTypeInstructionHeaderBytes[1];
            insHead[4] = InstructionHeader[0];
            insHead[5] = InstructionHeader[1];
            insHead[6] = InstructionHeader[2];
            insHead[7] = InstructionHeader[3];
            insHead[8] = (byte)Address.MemoryAddress.Length;
            insHead[9] = InstructionHeader[4];
            return insHead;
        }

        private void CreatePacketInformation()
        {
            RequestPacketInformation = new byte[6];
            byte[] byteArray = BitConverter.GetBytes(_orderCounter);
            RequestPacketInformation[0] = byteArray[0];
            RequestPacketInformation[1] = byteArray[1];
            byteArray = BitConverter.GetBytes((ushort)RequestPacketInstruction.Length);
            RequestPacketInformation[2] = byteArray[0];
            RequestPacketInformation[3] = byteArray[1];
            int checkSum = CalculateCheckSum(RequestPacketInformation);
            byteArray = BitConverter.GetBytes(checkSum);
            RequestPacketInformation[4] = byteArray[0];
            RequestPacketInformation[5] = byteArray[1];
        }

        private void CreatePacketInstruction()
        {
            byte[] insHead = CreateInstructionBasicHeader();
            if (ActionType == ActionType.Read)
            {
                RequestPacketInstruction = new byte[insHead.Length + Address.AddressBytes.Length +
                                                    (DataType == LsDataType.Continuous ? Address.ContinuousAddressDifference.Length : 0)];
                Buffer.BlockCopy(insHead, 0, RequestPacketInstruction, 0, insHead.Length);
                Buffer.BlockCopy(Address.AddressBytes, 0, RequestPacketInstruction,
                    insHead.Length, Address.AddressBytes.Length);
                if (DataType == LsDataType.Continuous)
                {
                    Buffer.BlockCopy(Address.ContinuousAddressDifference, 0, RequestPacketInstruction,
                        insHead.Length + Address.AddressBytes.Length, Address.ContinuousAddressDifference.Length);
                }
            }
            else
            {
                var writeInstructionBytes = CreateWriteInstructionBytes();
                RequestPacketInstruction = new byte[insHead.Length + Address.AddressBytes.Length + writeInstructionBytes.Length];
                Buffer.BlockCopy(insHead, 0, RequestPacketInstruction, 0, insHead.Length);
                Buffer.BlockCopy(Address.AddressBytes, 0, RequestPacketInstruction, insHead.Length, Address.AddressBytes.Length);
                Buffer.BlockCopy(writeInstructionBytes, 0, RequestPacketInstruction, Address.AddressBytes.Length + insHead.Length, writeInstructionBytes.Length);
            }
        }

        private byte[] CreateWriteInstructionBytes() => DataTypeStrategy.CreateWriteInstructionBytes(Address, Value);

        private void Init(byte[] requestPacketHeader)
        {
            DataTypeStrategy = TypeStrategy.GetDataTypeStrategy(typeof(T));
            if (DataTypeStrategy.DataType != Address.LsDataType)
                throw new LsDataTypeConflictException(DataTypeStrategy.DataType, Address.LsDataType);
            Id = Guid.NewGuid();
            Order = _orderCounter++;
            CreateByteArray();
            CreatedOn = DateTime.Now;
            RequestPacketHeader = requestPacketHeader;
        }
    }
}
