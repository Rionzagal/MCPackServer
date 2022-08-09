﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    public partial class MCProducts
    {
        public MCProducts()
        {
            ProjectProducts = new HashSet<ProjectProducts>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public float SugestedPrice { get; set; }
        [Required]
        [StringLength(5)]
        public string Currency { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string Model { get; set; }
        [StringLength(100)]
        public string Observations { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<ProjectProducts> ProjectProducts { get; set; }
    }
}