﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Index(nameof(Id), Name = "IX_Clients")]
    public partial class Clients
    {
        public Clients()
        {
            ClientContacts = new HashSet<ClientContacts>();
            Projects = new HashSet<Projects>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string MarketName { get; set; }
        [Required]
        [StringLength(100)]
        public string LegalName { get; set; }
        [Required]
        [StringLength(200)]
        public string FiscalAddress { get; set; }
        [Required]
        [StringLength(100)]
        public string City { get; set; }
        [Required]
        [StringLength(100)]
        public string Province { get; set; }
        [Required]
        [StringLength(100)]
        public string Country { get; set; }
        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [StringLength(200)]
        public string PaymentCondition { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(20)]
        public string RFC { get; set; }

        [InverseProperty("Client")]
        public virtual ICollection<ClientContacts> ClientContacts { get; set; }
        [InverseProperty("Client")]
        public virtual ICollection<Projects> Projects { get; set; }
    }
}