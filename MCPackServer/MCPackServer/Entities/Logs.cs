﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Index(nameof(UserId), Name = "FK_Logs_AspNetUsers_idx")]
    public partial class Logs
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [StringLength(20)]
        public string Action { get; set; }
        [StringLength(50)]
        public string TableName { get; set; }
        public bool Succeeded { get; set; }
        public DateTime? TimeOfAction { get; set; }
        [Required]
        public string Message { get; set; }
        public string Exception { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUsers.Logs))]
        public virtual AspNetUsers User { get; set; }
    }
}