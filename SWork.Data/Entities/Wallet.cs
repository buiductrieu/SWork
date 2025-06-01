
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWork.Data.Entities
{
    public class Wallet
    {
        [Key]
        public int WalletID { get; set; }
        public string UserID { get; set; }
        public decimal Balance { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }
        public virtual ICollection<WalletTransaction> Transactions { get; set; }
    }
}
