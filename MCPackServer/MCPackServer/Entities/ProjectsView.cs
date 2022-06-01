﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Keyless]
    public partial class ProjectsView
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        public string Description { get; set; }
        public double Discount { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? CommitmentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }
        [Required]
        [StringLength(100)]
        public string DeliveryTime { get; set; }
        [Required]
        [StringLength(10)]
        public string AgreedCurrency { get; set; }
        [Required]
        [StringLength(10)]
        public string PaymentCurrency { get; set; }
        [Required]
        [StringLength(100)]
        public string PaymentConditions { get; set; }
        [Required]
        [StringLength(50)]
        public string SalesPerson { get; set; }
        public double Comision { get; set; }
        public short HasTaxes { get; set; }
        public string Observations { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(20)]
        public string ProjectNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string ClientMarketName { get; set; }
        [Required]
        [StringLength(50)]
        public string ClientLegalName { get; set; }
    }
}