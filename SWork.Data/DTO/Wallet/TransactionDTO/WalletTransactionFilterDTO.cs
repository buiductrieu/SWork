

namespace SWork.Data.DTO.Wallet.TransactionDTO
{
    public class WalletTransactionFilterDTO  //Lọc giao dịch theo WalletID, thời gian, loại giao dịch
    {
        public int? WalletID { get; set; } 
        public string? TransactionType { get; set; } 
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
