﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MCPackServer.Entities;

#nullable disable

namespace MCPackServer.Data.Entity
{
    public partial class MCPACKDBContext : DbContext
    {
        public MCPACKDBContext()
        {
        }

        public MCPACKDBContext(DbContextOptions<MCPACKDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArticleFamilies> ArticleFamilies { get; set; }
        public virtual DbSet<ArticleGroups> ArticleGroups { get; set; }
        public virtual DbSet<ArticlesToPurchase> ArticlesToPurchase { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<ClientContacts> ClientContacts { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<ContactsWithCompanies> ContactsWithCompanies { get; set; }
        public virtual DbSet<DeviceCodes> DeviceCodes { get; set; }
        public virtual DbSet<Keys> Keys { get; set; }
        public virtual DbSet<MCProducts> MCProducts { get; set; }
        public virtual DbSet<PersistedGrants> PersistedGrants { get; set; }
        public virtual DbSet<ProjectProducts> ProjectProducts { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<ProviderContacts> ProviderContacts { get; set; }
        public virtual DbSet<Providers> Providers { get; set; }
        public virtual DbSet<PurchaseArticles> PurchaseArticles { get; set; }
        public virtual DbSet<PurchaseOrders> PurchaseOrders { get; set; }
        public virtual DbSet<Quotes> Quotes { get; set; }
        public virtual DbSet<RequisitionArticles> RequisitionArticles { get; set; }
        public virtual DbSet<Requisitions> Requisitions { get; set; }
        public virtual DbSet<SystemRolePermissions> SystemRolePermissions { get; set; }
        public virtual DbSet<UserInformation> UserInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<ArticleFamilies>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ArticleFamilies)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_ArticleFamilies_ArticleGroups");
            });

            modelBuilder.Entity<ArticleGroups>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<ArticlesToPurchase>(entity =>
            {
                entity.HasKey(e => new { e.QuoteId, e.PurchaseOrderId });

                entity.HasOne(d => d.PurchaseOrder)
                    .WithMany(p => p.ArticlesToPurchase)
                    .HasForeignKey(d => d.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticlesToPurchase_PurchaseOrders");

                entity.HasOne(d => d.Quote)
                    .WithMany(p => p.ArticlesToPurchase)
                    .HasForeignKey(d => d.QuoteId)
                    .HasConstraintName("FK_ArticlesToPurchase_Quotes");
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");
            });

            modelBuilder.Entity<ClientContacts>(entity =>
            {
                entity.HasKey(e => new { e.ClientId, e.ContactId });

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientContacts)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_ClientContacts_Clients");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ClientContacts)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_ClientContacts_Contacts");
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.LegalName).IsUnicode(false);

                entity.Property(e => e.MarketName).IsUnicode(false);

                entity.Property(e => e.PaymentCondition).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.PostalCode).IsUnicode(false);

                entity.Property(e => e.Province).IsUnicode(false);

                entity.Property(e => e.Website).IsUnicode(false);
            });

            modelBuilder.Entity<Contacts>(entity =>
            {
                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.FullName).IsUnicode(false);

                entity.Property(e => e.MobilePhone).IsUnicode(false);

                entity.Property(e => e.Position).IsUnicode(false);
            });

            modelBuilder.Entity<ContactsWithCompanies>(entity =>
            {
                entity.ToView("ContactsWithCompanies");

                entity.Property(e => e.CompanyId).IsUnicode(false);

                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.FullName).IsUnicode(false);

                entity.Property(e => e.MobilePhone).IsUnicode(false);

                entity.Property(e => e.Position).IsUnicode(false);

                entity.Property(e => e.status).IsUnicode(false);
            });

            modelBuilder.Entity<MCProducts>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.Currency).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);
            });

            modelBuilder.Entity<ProjectProducts>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ProjectId });

                entity.Property(e => e.Observations).IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProjectProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProyectProducts_MCProducts");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectProducts)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProyectProducts_Proyects");
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AgreedCurrency).IsUnicode(false);

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.DeliveryTime).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Observations).IsUnicode(false);

                entity.Property(e => e.PaymentConditions).IsUnicode(false);

                entity.Property(e => e.PaymentCurrency).IsUnicode(false);

                entity.Property(e => e.SalesPerson).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Proyects_Clients");
            });

            modelBuilder.Entity<ProviderContacts>(entity =>
            {
                entity.HasKey(e => new { e.ProviderId, e.ContactId });

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ProviderContacts)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_ProviderContacts_Contacts");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.ProviderContacts)
                    .HasForeignKey(d => d.ProviderId)
                    .HasConstraintName("FK_ProviderContacts_Providers");
            });

            modelBuilder.Entity<Providers>(entity =>
            {
                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.CreditLimit).IsUnicode(false);

                entity.Property(e => e.FiscalAddress).IsUnicode(false);

                entity.Property(e => e.LegalName).IsUnicode(false);

                entity.Property(e => e.MarketName).IsUnicode(false);

                entity.Property(e => e.Observations).IsUnicode(false);

                entity.Property(e => e.PaymentCondition).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.PostalCode).IsUnicode(false);

                entity.Property(e => e.Province).IsUnicode(false);

                entity.Property(e => e.TypeOfPayment).IsUnicode(false);

                entity.Property(e => e.Website).IsUnicode(false);
            });

            modelBuilder.Entity<PurchaseArticles>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Model).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.TradeMark).IsUnicode(false);

                entity.Property(e => e.Unit).IsUnicode(false);

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.PurchaseArticles)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("FK_ShoppingArticles_ArticleFamilies");
            });

            modelBuilder.Entity<PurchaseOrders>(entity =>
            {
                entity.Property(e => e.Currency).IsUnicode(false);

                entity.Property(e => e.InvoiceNumber).IsUnicode(false);

                entity.Property(e => e.Observations).IsUnicode(false);

                entity.Property(e => e.Status).IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_PurchaseOrders_Proyects");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.ProviderId)
                    .HasConstraintName("FK_PurchaseOrders_Providers");

                entity.HasOne(d => d.Requisition)
                    .WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.RequisitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrders_Requisitions");
            });

            modelBuilder.Entity<Quotes>(entity =>
            {
                entity.Property(e => e.Currency).IsUnicode(false);

                entity.Property(e => e.SKU).IsUnicode(false);

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Quotes)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK_Quotes_PurchaseArticles");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.Quotes)
                    .HasForeignKey(d => d.ProviderId)
                    .HasConstraintName("FK_Quotes_Providers");
            });

            modelBuilder.Entity<RequisitionArticles>(entity =>
            {
                entity.HasKey(e => new { e.RequisitionId, e.ProjectId, e.ArticleId });

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.RequisitionArticles)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequisitionArticles_PurchaseArticles");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.RequisitionArticles)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequisitionArticles_Projects");

                entity.HasOne(d => d.Requisition)
                    .WithMany(p => p.RequisitionArticles)
                    .HasForeignKey(d => d.RequisitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequisitionArticles_Requisitions");
            });

            modelBuilder.Entity<Requisitions>(entity =>
            {
                entity.Property(e => e.RequisitionNumber).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Requisitions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Requisitions_AspNetUsers");
            });

            modelBuilder.Entity<SystemRolePermissions>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithOne(p => p.SystemRolePermissions)
                    .HasForeignKey<SystemRolePermissions>(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SystemRolePermissions_AspNetRoles");
            });

            modelBuilder.Entity<UserInformation>(entity =>
            {
                entity.Property(e => e.FatherSurname).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.MiddleName).IsUnicode(false);

                entity.Property(e => e.MotherSurname).IsUnicode(false);

                entity.HasOne(d => d.AspNetUser)
                    .WithOne(p => p.UserInformation)
                    .HasForeignKey<UserInformation>(d => d.AspNetUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserInformation_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}