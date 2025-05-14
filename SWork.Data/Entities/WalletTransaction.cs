using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class WalletTransaction
    {
        [Key]
        public int TransactionID { get; set; }
        public int WalletID { get; set; }
        public decimal Amount { get; set; }
        public string Transaction_type { get; set; } // 'DEPOSIT', 'WITHDRAWAL', 'PAYMENT'
        public string Description { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("WalletID")]
        public virtual Wallet Wallet { get; set; }
    }
}
