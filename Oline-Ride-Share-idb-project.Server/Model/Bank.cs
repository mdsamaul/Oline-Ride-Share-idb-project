using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Bank
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankId { get; set; }
        public string? BankName { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public string? Address { get; set; }
        public string? BranchName { get; set; }
        public int AccountNumber { get; set; }
        public virtual ICollection<Company>? Companys { get; set; }
    }
}
