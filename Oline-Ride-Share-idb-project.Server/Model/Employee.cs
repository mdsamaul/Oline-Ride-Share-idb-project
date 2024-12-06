﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ride_Sharing_Project_isdb_bisew.Models;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Employee : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public required string PhoneNumber { get; set; }
        public bool IsLive { get; set; }
        public string? Email { get; set; }
        public virtual ICollection<Chat>? Chats { get; set; }
    }
}
