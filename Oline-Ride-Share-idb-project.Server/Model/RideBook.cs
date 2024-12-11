﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ride_Sharing_Project_isdb_bisew.Models;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class RideBook : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RideBookId { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer? Customers { get; set; }
        [ForeignKey("DriverVehicle")]
        public int DriverVehicleId { get; set; }
        public virtual DriverVehicle? DriverVehicles { get; set; }
        public float SourceLaitude { get; set; }
        public float SourceLongtiude { get; set; }
        public float DestinationLatitude { get; set; }
        public float DestinationLongitude { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalFare { get; set; }
        public bool IsPaid { get; set; }
        public string? DriverRating { get; set; }
        public string? CustomerRating { get; set; }
        public float DistanceInMeter { get; set; }
        //public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<RideTrack>? RideTracks { get; set; }
    }
}
