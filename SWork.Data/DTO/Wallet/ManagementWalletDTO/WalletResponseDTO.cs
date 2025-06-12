
namespace SWork.Data.DTO.Wallet.ManagementWalletDTO
{
    public class WalletResponseDTO
    {
        public int WalletID { get; set; }
        public string UserID { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
