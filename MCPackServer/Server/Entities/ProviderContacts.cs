﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Index(nameof(ContactId), Name = "FK_ProviderContacts_Contacts")]
    public partial class ProviderContacts
    {
        [Key]
        public int ProviderId { get; set; }
        [Key]
        public int ContactId { get; set; }

        [ForeignKey(nameof(ContactId))]
        [InverseProperty(nameof(Contacts.ProviderContacts))]
        public virtual Contacts Contact { get; set; }
        [ForeignKey(nameof(ProviderId))]
        [InverseProperty(nameof(Providers.ProviderContacts))]
        public virtual Providers Provider { get; set; }
    }
}