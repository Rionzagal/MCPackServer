CREATE DATABASE  MCPackDb /* SQLINES DEMO *** RACTER SET utf8 */ /* SQLINES DEMO *** RYPTION='N' */;
USE [MCPackDb];

DROP TABLE IF EXISTS [__EFMigrationsHistory];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE __EFMigrationsHistory (
  [MigrationId] varchar(150) NOT NULL,
  [ProductVersion] varchar(32) NOT NULL,
  PRIMARY KEY ([MigrationId])
) ;
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `ArticleFamilies`
--

DROP TABLE IF EXISTS [ArticleGroups];

CREATE TABLE ArticleGroups (
  [Id] int NOT NULL IDENTITY,
  [Name] varchar(30) NOT NULL,
  [Description] varchar(100) NOT NULL,
  [Code] varchar(20) NOT NULL,
  [HasVariablePrice] smallint NOT NULL,
  PRIMARY KEY ([Id])
)  ;

DROP TABLE IF EXISTS [ArticleFamilies];

CREATE TABLE ArticleFamilies (
  [Id] int NOT NULL IDENTITY,
  [Name] varchar(30) NOT NULL,
  [Description] varchar(100) NOT NULL,
  [Code] varchar(20) NOT NULL,
  [GroupId] int NOT NULL,
  PRIMARY KEY ([Id])
 ,
  CONSTRAINT [FK_ArticleFamilies_ArticleGroups] FOREIGN KEY ([GroupId]) REFERENCES ArticleGroups ([Id]) ON DELETE CASCADE
)  ;

CREATE INDEX [FK_ArticleFamilies_ArticleGroups] ON ArticleFamilies ([GroupId]);

DROP TABLE IF EXISTS [ArticlesToPurchase];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE ArticlesToPurchase (
  [QuoteId] int NOT NULL,
  [PurchaseOrderId] int NOT NULL,
  [Quantity] int NOT NULL,
  [EntryDate] datetime2(3) DEFAULT NULL,
  [DepartureDate] datetime2(3) DEFAULT NULL,
  [SalePrice] float NOT NULL,
  PRIMARY KEY ([QuoteId],[PurchaseOrderId])
 ,
  CONSTRAINT [FK_ArticlesToPurchase_PurchaseOrders] FOREIGN KEY ([PurchaseOrderId]) REFERENCES PurchaseOrders ([Id]),
  CONSTRAINT [FK_ArticlesToPurchase_Quotes] FOREIGN KEY ([QuoteId]) REFERENCES Quotes ([Id]) ON DELETE CASCADE
) ;

CREATE INDEX [FK_ArticlesToPurchase_PurchaseOrders] ON ArticlesToPurchase ([PurchaseOrderId]);

DROP TABLE IF EXISTS [AspNetRoleClaims];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE AspNetRoleClaims (
  [Id] int NOT NULL IDENTITY,
  [RoleId] varchar(450) NOT NULL,
  [ClaimType] varchar(max),
  [ClaimValue] varchar(max),
  PRIMARY KEY ([Id])
 ,
  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES AspNetRoles ([Id]) ON DELETE CASCADE
)  ;

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON AspNetRoleClaims ([RoleId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `AspNetRoles`
--

DROP TABLE IF EXISTS [AspNetRoles];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE AspNetRoles (
  [Id] varchar(450) NOT NULL,
  [Name] varchar(256) DEFAULT NULL,
  [NormalizedName] varchar(256) DEFAULT NULL,
  [ConcurrencyStamp] varchar(max),
  PRIMARY KEY ([Id]),
  CONSTRAINT [RoleNameIndex] UNIQUE  ([NormalizedName])
) ;
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `AspNetUserClaims`
--

DROP TABLE IF EXISTS [AspNetUserClaims];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE AspNetUserClaims (
  [Id] int NOT NULL IDENTITY,
  [UserId] varchar(450) NOT NULL,
  [ClaimType] varchar(max),
  [ClaimValue] varchar(max),
  PRIMARY KEY ([Id])
 ,
  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES AspNetUsers ([Id]) ON DELETE CASCADE
) ;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON AspNetUserClaims ([UserId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `aspnetuserlogins`
--

DROP TABLE IF EXISTS [aspnetuserlogins];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE aspnetuserlogins (
  [LoginProvider] varchar(128) NOT NULL,
  [ProviderKey] varchar(128) NOT NULL,
  [ProviderDisplayName] varchar(max),
  [UserId] varchar(450) NOT NULL,
  PRIMARY KEY ([LoginProvider],[ProviderKey])
 ,
  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES AspNetUsers ([Id]) ON DELETE CASCADE
) ;

CREATE INDEX [IX_AspNetUserLogins_UserId] ON aspnetuserlogins ([UserId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `aspnetuserroles`
--

DROP TABLE IF EXISTS [aspnetuserroles];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE aspnetuserroles (
  [UserId] varchar(450) NOT NULL,
  [RoleId] varchar(450) NOT NULL,
  PRIMARY KEY ([UserId],[RoleId])
 ,
  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES AspNetRoles ([Id]) ON DELETE CASCADE,
  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES AspNetUsers ([Id]) ON DELETE CASCADE
) ;

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON aspnetuserroles ([RoleId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `AspNetUsers`
--

DROP TABLE IF EXISTS [AspNetUsers];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE AspNetUsers (
  [Id] varchar(450) NOT NULL,
  [UserName] varchar(256) DEFAULT NULL,
  [NormalizedUserName] varchar(256) DEFAULT NULL,
  [Email] varchar(256) DEFAULT NULL,
  [NormalizedEmail] varchar(256) DEFAULT NULL,
  [EmailConfirmed] smallint NOT NULL,
  [PasswordHash] varchar(max),
  [SecurityStamp] varchar(max),
  [ConcurrencyStamp] varchar(max),
  [PhoneNumber] varchar(max),
  [PhoneNumberConfirmed] smallint NOT NULL,
  [TwoFactorEnabled] smallint NOT NULL,
  [LockoutEnd] datetime2(6) DEFAULT NULL,
  [LockoutEnabled] smallint NOT NULL,
  [AccessFailedCount] int NOT NULL,
  PRIMARY KEY ([Id]),
  CONSTRAINT [UserNameIndex] UNIQUE  ([NormalizedUserName])
) ;

CREATE INDEX [EmailIndex] ON AspNetUsers ([NormalizedEmail]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `AspNetUserTokens`
--

DROP TABLE IF EXISTS [AspNetUserTokens];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE AspNetUserTokens (
  [UserId] varchar(450) NOT NULL,
  [LoginProvider] varchar(128) NOT NULL,
  [Name] varchar(128) NOT NULL,
  [Value] varchar(max),
  PRIMARY KEY ([UserId],[LoginProvider],[Name]),
  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES AspNetUsers ([Id]) ON DELETE CASCADE
) ;

DROP TABLE IF EXISTS [ClientContacts];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE ClientContacts (
  [ClientId] int NOT NULL,
  [ContactId] int NOT NULL,
  PRIMARY KEY ([ClientId],[ContactId])
 ,
  CONSTRAINT [FK_ClientContacts_Clients] FOREIGN KEY ([ClientId]) REFERENCES Clients ([Id]) ON DELETE CASCADE,
  CONSTRAINT [FK_ClientContacts_Contacts] FOREIGN KEY ([ContactId]) REFERENCES Contacts ([Id]) ON DELETE CASCADE
) ;

CREATE INDEX [IX_ClientContacts] ON ClientContacts ([ClientId]);
CREATE INDEX [FK_ClientContacts_Contacts] ON ClientContacts ([ContactId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `Clients`
--

DROP TABLE IF EXISTS [Clients];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE Clients (
  [Id] int NOT NULL IDENTITY,
  [MarketName] varchar(50) NOT NULL,
  [LegalName] varchar(50) NOT NULL,
  [FiscalAddress] varchar(100) NOT NULL,
  [City] varchar(50) NOT NULL,
  [Province] varchar(50) NOT NULL,
  [Country] varchar(50) NOT NULL,
  [PostalCode] varchar(50) NOT NULL,
  [PhoneNumber] varchar(20) NOT NULL,
  [PaymentCondition] varchar(50) DEFAULT NULL,
  [Website] varchar(50) DEFAULT NULL,
  PRIMARY KEY ([Id])
)  ;

CREATE INDEX [IX_Clients] ON Clients ([Id]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `Contacts`
--

DROP TABLE IF EXISTS [Contacts];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE Contacts (
  [Id] int NOT NULL IDENTITY,
  [FullName] varchar(50) NOT NULL,
  [EmailAddress] varchar(50) NOT NULL,
  [MobilePhone] varchar(20) NOT NULL,
  [Position] varchar(50) DEFAULT NULL,
  PRIMARY KEY ([Id])
)  ;
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `DeviceCodes`
--

DROP TABLE IF EXISTS [DeviceCodes];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE DeviceCodes (
  [UserCode] varchar(200) NOT NULL,
  [DeviceCode] varchar(200) NOT NULL,
  [SubjectId] varchar(200) DEFAULT NULL,
  [SessionId] varchar(100) DEFAULT NULL,
  [ClientId] varchar(200) NOT NULL,
  [Description] varchar(200) DEFAULT NULL,
  [CreationTime] datetime2(6) NOT NULL,
  [Expiration] datetime2(6) NOT NULL,
  [Data] varchar(max) NOT NULL,
  PRIMARY KEY ([UserCode]),
  CONSTRAINT [IX_DeviceCodes_DeviceCode] UNIQUE  ([DeviceCode])
) ;

CREATE INDEX [IX_DeviceCodes_Expiration] ON DeviceCodes ([Expiration]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `Keys`
--

DROP TABLE IF EXISTS [Keys];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE Keys (
  [Id] varchar(450) NOT NULL,
  [Version] int NOT NULL,
  [Created] datetime2(6) NOT NULL,
  [Use] varchar(450) DEFAULT NULL,
  [Algorithm] varchar(100) NOT NULL,
  [IsX509Certificate] smallint NOT NULL,
  [DataProtected] smallint NOT NULL,
  [Data] varchar(max) NOT NULL,
  PRIMARY KEY ([Id])
) ;

CREATE INDEX [IX_Keys_Use] ON Keys ([Use]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `Logs`
--

DROP TABLE IF EXISTS [Logs];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE Logs (
  [Id] int NOT NULL IDENTITY,
  [UserId] varchar(450) DEFAULT NULL,
  [Action] varchar(50) DEFAULT NULL,
  [TableName] varchar(50) DEFAULT NULL,
  [Succeeded] smallint NOT NULL,
  [TimeOfAction] datetime2(0) DEFAULT NULL,
  [Message] varchar(max) NOT NULL,
  [Exception] varchar(max),
  PRIMARY KEY ([Id])
 ,
  CONSTRAINT [FK_Logs_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES AspNetUsers ([Id]) ON UPDATE CASCADE
)  ;

CREATE INDEX [FK_Logs_AspNetUsers_idx] ON Logs ([UserId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `MCProducts`
--

DROP TABLE IF EXISTS [MCProducts];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE MCProducts (
  [Id] int NOT NULL IDENTITY,
  [Description] varchar(100) DEFAULT NULL,
  [SugestedPrice] float NOT NULL,
  [Currency] varchar(5) NOT NULL,
  [Type] varchar(50) NOT NULL,
  [Code] varchar(50) NOT NULL,
  [Model] varchar(50) NOT NULL,
  [Observations] varchar(200) DEFAULT NULL,
  PRIMARY KEY ([Id])
)  ;
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `PersistedGrants`
--

DROP TABLE IF EXISTS [PersistedGrants];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE PersistedGrants (
  [Key] varchar(200) NOT NULL,
  [Type] varchar(50) NOT NULL,
  [SubjectId] varchar(200) DEFAULT NULL,
  [SessionId] varchar(100) DEFAULT NULL,
  [ClientId] varchar(200) NOT NULL,
  [Description] varchar(200) DEFAULT NULL,
  [CreationTime] datetime2(6) NOT NULL,
  [Expiration] datetime2(6) DEFAULT NULL,
  [ConsumedTime] datetime2(6) DEFAULT NULL,
  [Data] varchar(max) NOT NULL,
  PRIMARY KEY ([Key])
) ;

CREATE INDEX [IX_PersistedGrants_ConsumedTime] ON PersistedGrants ([ConsumedTime]);
CREATE INDEX [IX_PersistedGrants_Expiration] ON PersistedGrants ([Expiration]);
CREATE INDEX [IX_PersistedGrants_SubjectId_ClientId_Type] ON PersistedGrants ([SubjectId],[ClientId],[Type]);
CREATE INDEX [IX_PersistedGrants_SubjectId_SessionId_Type] ON PersistedGrants ([SubjectId],[SessionId],[Type]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `ProjectProducts`
--

DROP TABLE IF EXISTS [ProjectProducts];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE ProjectProducts (
  [ProductId] int NOT NULL,
  [ProjectId] int NOT NULL,
  [SalePrice] float NOT NULL,
  [Quantity] int NOT NULL,
  [Observations] varchar(max),
  PRIMARY KEY ([ProductId],[ProjectId])
 ,
  CONSTRAINT [FK_ProjectProducts_MCProducts] FOREIGN KEY ([ProductId]) REFERENCES MCProducts ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT [FK_ProjectProducts_Projects] FOREIGN KEY ([ProjectId]) REFERENCES Projects ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
) ;

CREATE INDEX [FK_ProjectProducts_Projects_idx] ON ProjectProducts ([ProjectId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `Projects`
--

DROP TABLE IF EXISTS [Projects];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE Projects (
  [Id] int NOT NULL IDENTITY,
  [ClientId] int NOT NULL,
  [Type] varchar(50) DEFAULT NULL,
  [Description] varchar(max) NOT NULL,
  [Discount] float NOT NULL,
  [AdmissionDate] datetime2(3) DEFAULT NULL,
  [CommitmentDate] datetime2(3) DEFAULT NULL,
  [DeliveryDate] datetime2(3) DEFAULT NULL,
  [RealDeliveryDate] datetime2(3) DEFAULT NULL,
  [DeliveryTime] varchar(100) NOT NULL,
  [AgreedCurrency] varchar(10) NOT NULL,
  [PaymentCurrency] varchar(10) NOT NULL,
  [PaymentConditions] varchar(100) NOT NULL,
  [SalesPerson] varchar(50) NOT NULL,
  [Comision] float NOT NULL,
  [HasTaxes] smallint NOT NULL,
  [Observations] varchar(max),
  [Code] varchar(20) NOT NULL,
  [ProjectNumber] varchar(20) DEFAULT NULL,
  PRIMARY KEY ([Id])
 ,
  CONSTRAINT [FK_Proyects_Clients] FOREIGN KEY ([ClientId]) REFERENCES Clients ([Id]) ON DELETE CASCADE
)  ;

CREATE INDEX [FK_Proyects_Clients] ON Projects ([ClientId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `providerContacts`
--

DROP TABLE IF EXISTS [providerContacts];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE providerContacts (
  [ProviderId] int NOT NULL,
  [ContactId] int NOT NULL,
  PRIMARY KEY ([ProviderId],[ContactId])
 ,
  CONSTRAINT [FK_ProviderContacts_Contacts] FOREIGN KEY ([ContactId]) REFERENCES Contacts ([Id]) ON DELETE CASCADE,
  CONSTRAINT [FK_ProviderContacts_Providers] FOREIGN KEY ([ProviderId]) REFERENCES Providers ([Id]) ON DELETE CASCADE
) ;

CREATE INDEX [FK_ProviderContacts_Contacts] ON providerContacts ([ContactId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `Providers`
--

DROP TABLE IF EXISTS [Providers];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE Providers (
  [Id] int NOT NULL IDENTITY,
  [MarketName] varchar(50) NOT NULL,
  [LegalName] varchar(50) NOT NULL,
  [FiscalAddress] varchar(50) NOT NULL,
  [City] varchar(50) NOT NULL,
  [Province] varchar(50) NOT NULL,
  [Country] varchar(50) NOT NULL,
  [PostalCode] varchar(50) NOT NULL,
  [PhoneNumber] varchar(20) NOT NULL,
  [Website] varchar(50) NOT NULL,
  [TypeOfPayment] varchar(50) NOT NULL,
  [PaymentCondition] varchar(50) NOT NULL,
  [CreditLimit] varchar(50) DEFAULT NULL,
  [Discount] float NOT NULL,
  [HomeDelivery] smallint NOT NULL,
  [Observations] varchar(max),
  [HasTaxes] smallint NOT NULL,
  PRIMARY KEY ([Id])
)  ;
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `PurchaseArticles`
--

DROP TABLE IF EXISTS [PurchaseArticles];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE PurchaseArticles (
  [Id] int NOT NULL IDENTITY,
  [Name] varchar(50) NOT NULL,
  [Description] varchar(100) NOT NULL,
  [Unit] varchar(20) NOT NULL,
  [TradeMark] varchar(50) DEFAULT NULL,
  [Model] varchar(50) DEFAULT NULL,
  [FamilyId] int NOT NULL,
  [Code] varchar(20) NOT NULL,
  [Observations] varchar(100) DEFAULT NULL,
  PRIMARY KEY ([Id])
 ,
  CONSTRAINT [FK_ShoppingArticles_ArticleFamilies] FOREIGN KEY ([FamilyId]) REFERENCES ArticleFamilies ([Id]) ON DELETE CASCADE
)  ;

CREATE INDEX [FK_ShoppingArticles_ArticleFamilies] ON PurchaseArticles ([FamilyId]);
/* SQLINES DEMO *** er_set_client = @saved_cs_client */;

--
-- SQLINES DEMO *** or table `PurchaseOrders`
--

DROP TABLE IF EXISTS [PurchaseOrders];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE PurchaseOrders (
  [Id] int NOT NULL IDENTITY,
  [ProjectId] int NOT NULL,
  [IssuedDate] datetime2(3) DEFAULT NULL,
  [ProviderId] int NOT NULL,
  [RequisitionId] int DEFAULT NULL,
  [DeliveryDate] datetime2(3) DEFAULT NULL,
  [Currency] varchar(10) NOT NULL,
  [Discount] float NOT NULL,
  [Status] varchar(50) NOT NULL,
  [ReceptionDate] datetime2(3) DEFAULT NULL,
  [InvoiceNumber] varchar(50) DEFAULT NULL,
  [Observations] varchar(max),
  [OrderNumber] varchar(50) DEFAULT NULL,
  PRIMARY KEY ([Id])
 ,
  CONSTRAINT [FK_PurchaseOrders_Projects] FOREIGN KEY ([ProjectId]) REFERENCES Projects ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT [FK_PurchaseOrders_Providers] FOREIGN KEY ([ProviderId]) REFERENCES Providers ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
)  ;

CREATE INDEX [FK_PurchaseOrders_Providers] ON PurchaseOrders ([ProviderId]);
CREATE INDEX [FK_PurchaseOrders_Projects_idx] ON PurchaseOrders ([ProjectId]);


DROP TABLE IF EXISTS [Quotes];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE Quotes (
  [Id] int NOT NULL IDENTITY,
  [ArticleId] int NOT NULL,
  [ProviderId] int NOT NULL,
  [Price] float NOT NULL,
  [SKU] varchar(50) DEFAULT NULL,
  [DateUpdated] datetime2(3) NOT NULL,
  [Currency] varchar(10) NOT NULL,
  PRIMARY KEY ([Id])
 ,
  CONSTRAINT [FK_Quotes_Providers] FOREIGN KEY ([ProviderId]) REFERENCES Providers ([Id]) ON DELETE CASCADE,
  CONSTRAINT [FK_Quotes_PurchaseArticles] FOREIGN KEY ([ArticleId]) REFERENCES PurchaseArticles ([Id]) ON DELETE CASCADE
)  ;
CREATE INDEX [FK_Quotes_Providers] ON Quotes ([ProviderId]);
CREATE INDEX [FK_Quotes_PurchaseArticles] ON Quotes ([ArticleId]);

DROP TABLE IF EXISTS [RequisitionArticles];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE RequisitionArticles (
  [RequisitionId] int NOT NULL,
  [ArticleId] int NOT NULL,
  [ProjectId] int NOT NULL,
  [Quantity] int NOT NULL,
  PRIMARY KEY ([RequisitionId],[ProjectId],[ArticleId])
 ,
  CONSTRAINT [FK_RequisitionArticles_Projects] FOREIGN KEY ([ProjectId]) REFERENCES Projects ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT [FK_RequisitionArticles_PurchaseArticles] FOREIGN KEY ([ArticleId]) REFERENCES PurchaseArticles ([Id]),
  CONSTRAINT [FK_RequisitionArticles_Requisitions] FOREIGN KEY ([RequisitionId]) REFERENCES Requisitions ([Id])
) ;

CREATE INDEX [FK_RequisitionArticles_PurchaseArticles] ON RequisitionArticles ([ArticleId]);
CREATE INDEX [FK_RequisitionArticles_Projects_idx] ON RequisitionArticles ([ProjectId]);

DROP TABLE IF EXISTS [Requisitions];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE Requisitions (
  [Id] int NOT NULL IDENTITY,
  [RequisitionNumber] varchar(50) NOT NULL,
  [IssuedDate] datetime2(3) DEFAULT NULL,
  [UserId] varchar(450) NOT NULL,
  [RequiredDate] datetime2(3) DEFAULT NULL,
  PRIMARY KEY ([Id]),
  CONSTRAINT [IX_Requisitions] UNIQUE  ([Id])
 ,
  CONSTRAINT [FK_Requisitions_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES AspNetUsers ([Id])
)  ;

CREATE INDEX [FK_Requisitions_AspNetUsers] ON Requisitions ([UserId]);


DROP TABLE IF EXISTS [UserInformation];
/* SQLINES DEMO *** cs_client     = @@character_set_client */;
/* SQLINES DEMO *** er_set_client = utf8mb4 */;
-- SQLINES LICENSE FOR EVALUATION USE ONLY
CREATE TABLE UserInformation (
  [AspNetUserId] varchar(450) NOT NULL,
  [FirstName] varchar(50) NOT NULL,
  [MiddleName] varchar(50) DEFAULT NULL,
  [FatherSurname] varchar(50) NOT NULL,
  [MotherSurname] varchar(50) NOT NULL,
  [BirthDate] datetime2(3) DEFAULT NULL,
  [Gender] varchar(50) DEFAULT NULL,
  PRIMARY KEY ([AspNetUserId]),
  CONSTRAINT [FK_UserInformation_AspNetUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES AspNetUsers ([Id])
) ;