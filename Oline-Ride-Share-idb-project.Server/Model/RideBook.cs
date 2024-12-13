using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class RideBook
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RideBookId { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        [ForeignKey("DriverVehicle")]
        public int DriverVehicleId { get; set; }
        public virtual DriverVehicle? DriverVehicle { get; set; }
        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
        public float SourceLaitude { get; set; }
        public float SourceLongtiude { get; set; }
        public float DestinationLatitude { get; set; }
        public float DestinationLongtiude { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Status { get; set; }
        public decimal TotalFare { get; set; }
        public bool IsPaid { get; set; }
        public string? DriverRating { get; set; }
        public string? CustomerRating { get; set; }
        public float DistanceInMeter { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<RideTrack>? RideTracks { get; set; }
    }
}
