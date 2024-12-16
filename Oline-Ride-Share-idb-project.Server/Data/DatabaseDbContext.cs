using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Model;

namespace Oline_Ride_Share_idb_project.Server.Data
{
    public class DatabaseDbContext: DbContext
    {
        public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options):base(options) { }
        public virtual DbSet<VehicleType>? VehicleTypes{ get; set; }
        public virtual DbSet<Vehicle>? Vehicles { get; set; }
        public virtual DbSet<Driver>? Drivers { get; set; }
        public virtual DbSet<DriverVehicle>? DriverVehicles { get; set; }
        public virtual DbSet<Customer>? Customers { get; set; }
        public virtual DbSet<RideBook>? RideBooks { get; set; }
        public virtual DbSet<RideTrack>? RideTracks { get; set; }
        public virtual DbSet<FareDetail>? FareDetails { get; set; }
        public virtual DbSet<Invoice>? Invoices { get; set; }
        public virtual DbSet<Bank>? Banks { get; set; }
        public virtual DbSet<PaymentMethod>? PaymentMethods { get; set; }
        public virtual DbSet<Payment>? Payments { get; set; }
        public virtual DbSet<Chat>? Chats { get; set; }
        public virtual DbSet<Company>? Companys { get; set; }
        public virtual DbSet<Employee>? Employees{ get; set; }

    }
}
