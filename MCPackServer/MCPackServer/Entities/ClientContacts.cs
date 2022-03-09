﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCPackServer.Entities
{
    [Index(nameof(ClientId), Name = "IX_ClientContacts")]
    public partial class ClientContacts
    {
        [Key]
        public int ClientId { get; set; }
        [Key]
        public int ContactId { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty(nameof(Clients.ClientContacts))]
        public virtual Clients Client { get; set; }
        [ForeignKey(nameof(ContactId))]
        [InverseProperty(nameof(Contacts.ClientContacts))]
        public virtual Contacts Contact { get; set; }
    }
}