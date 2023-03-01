﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Index(nameof(ClientId), Name = "FK_Proyects_Clients")]
    public partial class Projects
    {
        public Projects()
        {
            ProjectProducts = new HashSet<ProjectProducts>();
            PurchaseOrders = new HashSet<PurchaseOrders>();
            RequisitionArticles = new HashSet<RequisitionArticles>();
        }

        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        public float Discount { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? CommitmentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }
        [Required]
        [StringLength(100)]
        public string DeliveryTime { get; set; }
        [Required]
        [StringLength(5)]
        public string AgreedCurrency { get; set; }
        [Required]
        [StringLength(5)]
        public string PaymentCurrency { get; set; }
        [Required]
        [StringLength(200)]
        public string PaymentConditions { get; set; }
        [Required]
        [StringLength(50)]
        public string SalesPerson { get; set; }
        public float Comision { get; set; }
        public bool HasTaxes { get; set; }
        [StringLength(200)]
        public string Observations { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        [StringLength(20)]
        public string ProjectNumber { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty(nameof(Clients.Projects))]
        public virtual Clients Client { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<ProjectProducts> ProjectProducts { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<PurchaseOrders> PurchaseOrders { get; set; }
        [InverseProperty("Project")]
        public virtual ICollection<RequisitionArticles> RequisitionArticles { get; set; }
    }
}