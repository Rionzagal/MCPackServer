﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    public partial class PurchaseOrders
    {
        public PurchaseOrders()
        {
            ArticlesToPurchase = new HashSet<ArticlesToPurchase>();
        }

        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IssuedDate { get; set; }
        public int ProviderId { get; set; }
        public int RequisitionId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeliveryDate { get; set; }
        [Required]
        [StringLength(10)]
        public string Currency { get; set; }
        public float Discount { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ReceptionDate { get; set; }
        [StringLength(50)]
        public string InvoiceNumber { get; set; }
        public string Observations { get; set; }
        [StringLength(50)]
        public string OrderNumber { get; set; }

        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(Projects.PurchaseOrders))]
        public virtual Projects Project { get; set; }
        [ForeignKey(nameof(ProviderId))]
        [InverseProperty(nameof(Providers.PurchaseOrders))]
        public virtual Providers Provider { get; set; }
        [ForeignKey(nameof(RequisitionId))]
        [InverseProperty(nameof(Requisitions.PurchaseOrders))]
        public virtual Requisitions Requisition { get; set; }
        [InverseProperty("PurchaseOrder")]
        public virtual ICollection<ArticlesToPurchase> ArticlesToPurchase { get; set; }
    }
}