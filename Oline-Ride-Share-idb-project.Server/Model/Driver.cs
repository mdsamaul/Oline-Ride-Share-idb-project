using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Driver : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }
        public string? DriverName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string DrivingLicenseNo { get; set; }
        [Required]
        public string DriverNid { get; set; }
        public string? DriverImage { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        [Range(-90, 90)]
        public float DriverLatitude { get; set; }
        [Range(-180, 180)]
        public float DriverLongitude { get; set; }
        public bool IsAvailable { get; set; }
        public string? FcmToken { get; set; }
        public virtual ICollection<DriverVehicle>? DriverVehicles { get; set; }
    }
}
