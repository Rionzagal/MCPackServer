﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Index(nameof(ProjectId), Name = "FK_RequisitionArticles_Projects_idx")]
    [Index(nameof(ArticleId), Name = "FK_RequisitionArticles_PurchaseArticles")]
    public partial class RequisitionArticles
    {
        [Key]
        public int RequisitionId { get; set; }
        [Key]
        public int ArticleId { get; set; }
        [Key]
        public int ProjectId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(ArticleId))]
        [InverseProperty(nameof(PurchaseArticles.RequisitionArticles))]
        public virtual PurchaseArticles Article { get; set; }
        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(Projects.RequisitionArticles))]
        public virtual Projects Project { get; set; }
        [ForeignKey(nameof(RequisitionId))]
        [InverseProperty(nameof(Requisitions.RequisitionArticles))]
        public virtual Requisitions Requisition { get; set; }
    }
}