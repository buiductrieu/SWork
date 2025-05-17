using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWork.Data.Entities
{
    public class Wallet
    {
        [Key]
        public int WalletID { get; set; }
        public int UserID { get; set; }
        public decimal Balance { get; set; } = 0;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        public virtual ICollection<WalletTransaction> Transactions { get; set; }
    }
}
