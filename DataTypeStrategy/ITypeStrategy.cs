using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;

namespace RayanCnc.LSConnection.DataTypeStrategy
{
    public interface ITypeStrategy
    {
        LsDataType DataType { get; }

        byte[] CreateWriteInstructionBytes(ILsAddress address, object value);

        object HandleReadOperation(byte[] raw);

        void HandleWriteOperation(byte[] raw);

        object ParseReadData(byte[] data);
    }
}
