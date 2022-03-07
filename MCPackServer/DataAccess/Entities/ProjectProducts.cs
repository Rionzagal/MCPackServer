﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities
{
    public partial class ProjectProducts
    {
        [Key]
        public int ProductId { get; set; }
        [Key]
        public int ProjectId { get; set; }
        public float SalePrice { get; set; }
        public int Quantity { get; set; }
        public string Observations { get; set; }

        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(MCProducts.ProjectProducts))]
        public virtual MCProducts Product { get; set; }
        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(Projects.ProjectProducts))]
        public virtual Projects Project { get; set; }
    }
}