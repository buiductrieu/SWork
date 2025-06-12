
using System.ComponentModel.DataAnnotations;

namespace SWork.Data.DTO.Wallet.ManagementWalletDTO
{
    public class WalletCreateDTO
    {
        [Required]
        [StringLength(450)]
        public string UserID { get; set; }
    }
}
