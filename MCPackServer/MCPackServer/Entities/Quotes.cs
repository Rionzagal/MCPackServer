﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Index(nameof(ProviderId), Name = "FK_Quotes_Providers")]
    [Index(nameof(ArticleId), Name = "FK_Quotes_PurchaseArticles")]
    public partial class Quotes
    {
        public Quotes()
        {
            ArticlesToPurchase = new HashSet<ArticlesToPurchase>();
        }

        [Key]
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int ProviderId { get; set; }
        public float Price { get; set; }
        [StringLength(100)]
        public string SKU { get; set; }
        public DateTime DateUpdated { get; set; }
        [Required]
        [StringLength(5)]
        public string Currency { get; set; }

        [ForeignKey(nameof(ArticleId))]
        [InverseProperty(nameof(PurchaseArticles.Quotes))]
        public virtual PurchaseArticles Article { get; set; }
        [ForeignKey(nameof(ProviderId))]
        [InverseProperty(nameof(Providers.Quotes))]
        public virtual Providers Provider { get; set; }
        [InverseProperty("Quote")]
        public virtual ICollection<ArticlesToPurchase> ArticlesToPurchase { get; set; }
    }
}