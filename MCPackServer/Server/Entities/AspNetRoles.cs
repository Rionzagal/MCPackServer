﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Index(nameof(NormalizedName), Name = "RoleNameIndex", IsUnique = true)]
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
        }

        [Key]
        [StringLength(450)]
        public string Id { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}