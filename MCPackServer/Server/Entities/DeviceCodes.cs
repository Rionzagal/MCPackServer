﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Index(nameof(DeviceCode), Name = "IX_DeviceCodes_DeviceCode", IsUnique = true)]
    [Index(nameof(Expiration), Name = "IX_DeviceCodes_Expiration")]
    public partial class DeviceCodes
    {
        [Key]
        [StringLength(200)]
        public string UserCode { get; set; }
        [Required]
        [StringLength(200)]
        public string DeviceCode { get; set; }
        [StringLength(200)]
        public string SubjectId { get; set; }
        [StringLength(100)]
        public string SessionId { get; set; }
        [Required]
        [StringLength(200)]
        public string ClientId { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime Expiration { get; set; }
        [Required]
        public string Data { get; set; }
    }
}