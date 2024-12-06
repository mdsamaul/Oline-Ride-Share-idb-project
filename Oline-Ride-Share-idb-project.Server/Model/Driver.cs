using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ride_Sharing_Project_isdb_bisew.Models;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Driver : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string DrivingLicenseNo { get; set; }
        public required string DriverNid { get; set; }
        public string? DriverImage { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company? Companys { get; set; }
        public float DriverLaitude { get; set; }
        public float DriverLongtiude { get; set; }
        public virtual ICollection<DriverVehicle>? DriverVehicles { get; set; }
    }

}
