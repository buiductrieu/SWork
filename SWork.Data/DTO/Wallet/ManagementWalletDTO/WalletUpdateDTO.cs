

using System.ComponentModel.DataAnnotations;

namespace SWork.Data.DTO.Wallet.ManagementWalletDTO
{
    public class WalletUpdateDTO
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a non-negative value.")]
        public decimal Balance { get; set; }
        public string Description { get; set; }
    }

}
