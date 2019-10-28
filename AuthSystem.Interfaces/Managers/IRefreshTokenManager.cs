using AuthSystem.Data;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Managers
{
    public interface IRefreshTokenManager
    {
        Task<bool> IsValidRefreshTokenPresentAsync(RefreshTokenId tokenId);
        Task<RefreshTokenId> CreateRefreshTokenAsync(UserId userId);
    }
}
