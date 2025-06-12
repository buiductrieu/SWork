
using SWork.Data.DTO.Wallet.ManagementWalletDTO;

namespace SWork.ServiceContract.Interfaces
{
    public interface IWalletService
    {
        // --- Wallet Operations ---
        Task<WalletResponseDTO> CreateWalletAsync(WalletCreateDTO createDto);
        Task<WalletResponseDTO?> GetWalletByUserIdAsync(string userId);
        Task<WalletResponseDTO?> GetWalletByIdAsync(int walletId);
        Task<IEnumerable<WalletResponseDTO>> GetAllWalletsAsync();
        Task<WalletResponseDTO> UpdateWalletAsync(int walletId, WalletUpdateDTO updateDto);
        Task<bool> DeleteWalletAsync(int walletId); 

        // Các thao tác thay đổi số dư cụ thể
        Task<bool> AddToWalletAsync(string userId, decimal amount, string description, string transactionType);
        Task<bool> DeductFromWalletAsync(string userId, decimal amount, string description, string transactionType);

    }
}
