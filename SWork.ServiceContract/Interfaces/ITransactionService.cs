
using SWork.Data.DTO.Wallet.TransactionDTO;

namespace SWork.ServiceContract.Interfaces
{
    public interface ITransactionService
    {
        Task<WalletTransactionResponseDTO?> GetTransactionByIdAsync(int transactionId);
        Task<IEnumerable<WalletTransactionResponseDTO>> GetTransactionsByWalletIdAsync(int walletId, WalletTransactionFilterDTO? filter = null);
        Task<WalletTransactionResponseDTO> CreateTransactionAsync(WalletTransactionCreateDTO createDto);
        Task<bool> DeleteTransactionAsync(int transactionId);
    }
}
