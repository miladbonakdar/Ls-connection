using System.Threading.Tasks;

namespace RayanCnc.LSConnection.Contracts
{
    public interface IPing
    {
        Task<bool> PingAsync();
    }
}