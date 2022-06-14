CREATE DATABASE  IF NOT EXISTS `mcpackdb` /*!40100 DEFAULT CHARACTER SET utf8 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `mcpackdb`;
-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: localhost    Database: mcpackdb
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `articlefamilies`
--

DROP TABLE IF EXISTS `articlefamilies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `articlefamilies` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(30) NOT NULL,
  `Description` varchar(100) NOT NULL,
  `Code` varchar(20) NOT NULL,
  `GroupId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_ArticleFamilies_ArticleGroups` (`GroupId`),
  CONSTRAINT `FK_ArticleFamilies_ArticleGroups` FOREIGN KEY (`GroupId`) REFERENCES `articlegroups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `articlegroups`
--

DROP TABLE IF EXISTS `articlegroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `articlegroups` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(30) NOT NULL,
  `Description` varchar(100) NOT NULL,
  `Code` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `HasVariablePrice` tinyint NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `articlestopurchase`
--

DROP TABLE IF EXISTS `articlestopurchase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `articlestopurchase` (
  `QuoteId` int NOT NULL,
  `PurchaseOrderId` int NOT NULL,
  `Quantity` int NOT NULL,
  `EntryDate` datetime(3) DEFAULT NULL,
  `DepartureDate` datetime(3) DEFAULT NULL,
  `SalePrice` double NOT NULL,
  PRIMARY KEY (`QuoteId`,`PurchaseOrderId`),
  KEY `FK_ArticlesToPurchase_PurchaseOrders` (`PurchaseOrderId`),
  CONSTRAINT `FK_ArticlesToPurchase_PurchaseOrders` FOREIGN KEY (`PurchaseOrderId`) REFERENCES `purchaseorders` (`Id`),
  CONSTRAINT `FK_ArticlesToPurchase_Quotes` FOREIGN KEY (`QuoteId`) REFERENCES `quotes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `articlesview`
--

DROP TABLE IF EXISTS `articlesview`;
/*!50001 DROP VIEW IF EXISTS `articlesview`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `articlesview` AS SELECT 
 1 AS `Id`,
 1 AS `Name`,
 1 AS `Description`,
 1 AS `Unit`,
 1 AS `TradeMark`,
 1 AS `Model`,
 1 AS `FamilyId`,
 1 AS `FamilyName`,
 1 AS `GroupId`,
 1 AS `GroupName`,
 1 AS `Code`,
 1 AS `MustQuoteDaily`,
 1 AS `Observations`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=387 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProviderKey` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProviderDisplayName` longtext,
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `RoleId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `EmailConfirmed` tinyint NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `ConcurrencyStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint NOT NULL,
  `TwoFactorEnabled` tinyint NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `LoginProvider` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Name` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `associatedcontactsview`
--

DROP TABLE IF EXISTS `associatedcontactsview`;
/*!50001 DROP VIEW IF EXISTS `associatedcontactsview`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `associatedcontactsview` AS SELECT 
 1 AS `Id`,
 1 AS `FullName`,
 1 AS `EmailAddress`,
 1 AS `MobilePhone`,
 1 AS `Position`,
 1 AS `CompanyId`,
 1 AS `status`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `clientcontacts`
--

DROP TABLE IF EXISTS `clientcontacts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clientcontacts` (
  `ClientId` int NOT NULL,
  `ContactId` int NOT NULL,
  PRIMARY KEY (`ClientId`,`ContactId`),
  KEY `IX_ClientContacts` (`ClientId`),
  KEY `FK_ClientContacts_Contacts` (`ContactId`),
  CONSTRAINT `FK_ClientContacts_Clients` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ClientContacts_Contacts` FOREIGN KEY (`ContactId`) REFERENCES `contacts` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clients` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketName` varchar(50) NOT NULL,
  `LegalName` varchar(50) NOT NULL,
  `FiscalAddress` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `City` varchar(50) NOT NULL,
  `Province` varchar(50) NOT NULL,
  `Country` varchar(50) NOT NULL,
  `PostalCode` varchar(50) NOT NULL,
  `PhoneNumber` varchar(20) NOT NULL,
  `PaymentCondition` varchar(50) DEFAULT NULL,
  `Website` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Clients` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `contacts`
--

DROP TABLE IF EXISTS `contacts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contacts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FullName` varchar(50) NOT NULL,
  `EmailAddress` varchar(50) NOT NULL,
  `MobilePhone` varchar(20) NOT NULL,
  `Position` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `devicecodes`
--

DROP TABLE IF EXISTS `devicecodes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `devicecodes` (
  `UserCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `DeviceCode` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `SubjectId` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `SessionId` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `ClientId` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Description` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) NOT NULL,
  `Data` longtext NOT NULL,
  PRIMARY KEY (`UserCode`),
  UNIQUE KEY `IX_DeviceCodes_DeviceCode` (`DeviceCode`),
  KEY `IX_DeviceCodes_Expiration` (`Expiration`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `keys`
--

DROP TABLE IF EXISTS `keys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `keys` (
  `Id` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Version` int NOT NULL,
  `Created` datetime(6) NOT NULL,
  `Use` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `Algorithm` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `IsX509Certificate` tinyint NOT NULL,
  `DataProtected` tinyint NOT NULL,
  `Data` longtext NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Keys_Use` (`Use`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `logs`
--

DROP TABLE IF EXISTS `logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `logs` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `Action` varchar(50) DEFAULT NULL,
  `TableName` varchar(50) DEFAULT NULL,
  `Succeeded` tinyint NOT NULL,
  `TimeOfAction` datetime DEFAULT NULL,
  `Message` longtext NOT NULL,
  `Exception` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_Logs_AspNetUsers_idx` (`UserId`),
  CONSTRAINT `FK_Logs_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `mcproducts`
--

DROP TABLE IF EXISTS `mcproducts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mcproducts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Description` varchar(100) DEFAULT NULL,
  `SugestedPrice` double NOT NULL,
  `Currency` varchar(5) NOT NULL,
  `Type` varchar(50) NOT NULL,
  `Code` varchar(50) NOT NULL,
  `Model` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Observations` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `persistedgrants`
--

DROP TABLE IF EXISTS `persistedgrants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `persistedgrants` (
  `Key` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Type` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `SubjectId` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `SessionId` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `ClientId` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Description` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) DEFAULT NULL,
  `ConsumedTime` datetime(6) DEFAULT NULL,
  `Data` longtext NOT NULL,
  PRIMARY KEY (`Key`),
  KEY `IX_PersistedGrants_ConsumedTime` (`ConsumedTime`),
  KEY `IX_PersistedGrants_Expiration` (`Expiration`),
  KEY `IX_PersistedGrants_SubjectId_ClientId_Type` (`SubjectId`,`ClientId`,`Type`),
  KEY `IX_PersistedGrants_SubjectId_SessionId_Type` (`SubjectId`,`SessionId`,`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `projectproducts`
--

DROP TABLE IF EXISTS `projectproducts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `projectproducts` (
  `ProductId` int NOT NULL,
  `ProjectId` int NOT NULL,
  `SalePrice` double NOT NULL,
  `Quantity` int NOT NULL,
  `Observations` longtext,
  PRIMARY KEY (`ProductId`,`ProjectId`),
  KEY `FK_ProjectProducts_Projects_idx` (`ProjectId`),
  CONSTRAINT `FK_ProjectProducts_MCProducts` FOREIGN KEY (`ProductId`) REFERENCES `mcproducts` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_ProjectProducts_Projects` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `projects`
--

DROP TABLE IF EXISTS `projects`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `projects` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ClientId` int NOT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `Description` longtext NOT NULL,
  `Discount` double NOT NULL,
  `AdmissionDate` datetime(3) DEFAULT NULL,
  `CommitmentDate` datetime(3) DEFAULT NULL,
  `DeliveryDate` datetime(3) DEFAULT NULL,
  `RealDeliveryDate` datetime(3) DEFAULT NULL,
  `DeliveryTime` varchar(100) NOT NULL,
  `AgreedCurrency` varchar(10) NOT NULL,
  `PaymentCurrency` varchar(10) NOT NULL,
  `PaymentConditions` varchar(100) NOT NULL,
  `SalesPerson` varchar(50) NOT NULL,
  `Comision` double NOT NULL,
  `HasTaxes` tinyint NOT NULL,
  `Observations` longtext,
  `Code` varchar(20) NOT NULL,
  `ProjectNumber` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Proyects_Clients` (`ClientId`),
  CONSTRAINT `FK_Proyects_Clients` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1538 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `providercontacts`
--

DROP TABLE IF EXISTS `providercontacts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providercontacts` (
  `ProviderId` int NOT NULL,
  `ContactId` int NOT NULL,
  PRIMARY KEY (`ProviderId`,`ContactId`),
  KEY `FK_ProviderContacts_Contacts` (`ContactId`),
  CONSTRAINT `FK_ProviderContacts_Contacts` FOREIGN KEY (`ContactId`) REFERENCES `contacts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ProviderContacts_Providers` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `providers`
--

DROP TABLE IF EXISTS `providers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MarketName` varchar(50) NOT NULL,
  `LegalName` varchar(50) NOT NULL,
  `FiscalAddress` varchar(50) NOT NULL,
  `City` varchar(50) NOT NULL,
  `Province` varchar(50) NOT NULL,
  `Country` varchar(50) NOT NULL,
  `PostalCode` varchar(50) NOT NULL,
  `PhoneNumber` varchar(20) NOT NULL,
  `Website` varchar(50) NOT NULL,
  `TypeOfPayment` varchar(50) NOT NULL,
  `PaymentCondition` varchar(50) NOT NULL,
  `CreditLimit` varchar(50) DEFAULT NULL,
  `Discount` double NOT NULL,
  `HomeDelivery` tinyint NOT NULL,
  `Observations` longtext,
  `HasTaxes` tinyint NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `purchasearticles`
--

DROP TABLE IF EXISTS `purchasearticles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchasearticles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(100) NOT NULL,
  `Unit` varchar(20) NOT NULL,
  `TradeMark` varchar(50) DEFAULT NULL,
  `Model` varchar(50) DEFAULT NULL,
  `FamilyId` int NOT NULL,
  `Code` varchar(20) NOT NULL,
  `Observations` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_ShoppingArticles_ArticleFamilies` (`FamilyId`),
  CONSTRAINT `FK_ShoppingArticles_ArticleFamilies` FOREIGN KEY (`FamilyId`) REFERENCES `articlefamilies` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `purchaseorders`
--

DROP TABLE IF EXISTS `purchaseorders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchaseorders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ProjectId` int NOT NULL,
  `IssuedDate` datetime(3) DEFAULT NULL,
  `ProviderId` int NOT NULL,
  `RequisitionId` int DEFAULT NULL,
  `DeliveryDate` datetime(3) DEFAULT NULL,
  `Currency` varchar(10) NOT NULL,
  `Discount` double NOT NULL,
  `Status` varchar(50) NOT NULL,
  `ReceptionDate` datetime(3) DEFAULT NULL,
  `InvoiceNumber` varchar(50) DEFAULT NULL,
  `Observations` longtext,
  `OrderNumber` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_PurchaseOrders_Providers` (`ProviderId`),
  KEY `FK_PurchaseOrders_Projects_idx` (`ProjectId`),
  CONSTRAINT `FK_PurchaseOrders_Projects` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_PurchaseOrders_Providers` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `purchaseordersview`
--

DROP TABLE IF EXISTS `purchaseordersview`;
/*!50001 DROP VIEW IF EXISTS `purchaseordersview`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `purchaseordersview` AS SELECT 
 1 AS `Id`,
 1 AS `IssuedDate`,
 1 AS `DeliveryDate`,
 1 AS `Currency`,
 1 AS `Discount`,
 1 AS `Status`,
 1 AS `ReceptionDate`,
 1 AS `InvoiceNumber`,
 1 AS `Observations`,
 1 AS `OrderNumber`,
 1 AS `ProviderId`,
 1 AS `ProviderLegalName`,
 1 AS `HasTaxes`,
 1 AS `ProjectId`,
 1 AS `ProjectNumber`,
 1 AS `ClientId`,
 1 AS `ClientMarketName`,
 1 AS `RequisitionId`,
 1 AS `RequisitionNumber`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `quotes`
--

DROP TABLE IF EXISTS `quotes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quotes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ArticleId` int NOT NULL,
  `ProviderId` int NOT NULL,
  `Price` double NOT NULL,
  `SKU` varchar(50) DEFAULT NULL,
  `DateUpdated` datetime(3) NOT NULL,
  `Currency` varchar(10) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Quotes_Providers` (`ProviderId`),
  KEY `FK_Quotes_PurchaseArticles` (`ArticleId`),
  CONSTRAINT `FK_Quotes_Providers` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Quotes_PurchaseArticles` FOREIGN KEY (`ArticleId`) REFERENCES `purchasearticles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `quotesview`
--

DROP TABLE IF EXISTS `quotesview`;
/*!50001 DROP VIEW IF EXISTS `quotesview`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `quotesview` AS SELECT 
 1 AS `Id`,
 1 AS `ProviderId`,
 1 AS `ProviderMarketName`,
 1 AS `ProviderLegalName`,
 1 AS `ArticleId`,
 1 AS `ArticleName`,
 1 AS `GroupId`,
 1 AS `GroupName`,
 1 AS `FamilyId`,
 1 AS `FamilyName`,
 1 AS `ArticleCode`,
 1 AS `SKU`,
 1 AS `Price`,
 1 AS `Currency`,
 1 AS `DateUpdated`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `requisitionarticles`
--

DROP TABLE IF EXISTS `requisitionarticles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `requisitionarticles` (
  `RequisitionId` int NOT NULL,
  `ArticleId` int NOT NULL,
  `ProjectId` int NOT NULL,
  `Quantity` int NOT NULL,
  PRIMARY KEY (`RequisitionId`,`ProjectId`,`ArticleId`),
  KEY `FK_RequisitionArticles_PurchaseArticles` (`ArticleId`),
  KEY `FK_RequisitionArticles_Projects_idx` (`ProjectId`),
  CONSTRAINT `FK_RequisitionArticles_Projects` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_RequisitionArticles_PurchaseArticles` FOREIGN KEY (`ArticleId`) REFERENCES `purchasearticles` (`Id`),
  CONSTRAINT `FK_RequisitionArticles_Requisitions` FOREIGN KEY (`RequisitionId`) REFERENCES `requisitions` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `requisitionarticlesview`
--

DROP TABLE IF EXISTS `requisitionarticlesview`;
/*!50001 DROP VIEW IF EXISTS `requisitionarticlesview`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `requisitionarticlesview` AS SELECT 
 1 AS `RequisitionId`,
 1 AS `RequisitionNumber`,
 1 AS `ArticleId`,
 1 AS `ArticleName`,
 1 AS `GroupId`,
 1 AS `GroupName`,
 1 AS `FamilyId`,
 1 AS `FamilyName`,
 1 AS `ArticleCode`,
 1 AS `ProjectId`,
 1 AS `ProjectNumber`,
 1 AS `Quantity`,
 1 AS `RequiredDate`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `requisitions`
--

DROP TABLE IF EXISTS `requisitions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `requisitions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RequisitionNumber` varchar(50) NOT NULL,
  `IssuedDate` datetime(3) DEFAULT NULL,
  `UserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `RequiredDate` datetime(3) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Requisitions` (`Id`),
  KEY `FK_Requisitions_AspNetUsers` (`UserId`),
  CONSTRAINT `FK_Requisitions_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `systemrolepermissions`
--

DROP TABLE IF EXISTS `systemrolepermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `systemrolepermissions` (
  `RoleId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `PermissionValue` longtext NOT NULL,
  `Id` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_SystemRolePermissions` (`RoleId`),
  CONSTRAINT `FK_SystemRolePermissions_AspNetRoles` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `userinformation`
--

DROP TABLE IF EXISTS `userinformation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userinformation` (
  `AspNetUserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `MiddleName` varchar(50) DEFAULT NULL,
  `FatherSurname` varchar(50) NOT NULL,
  `MotherSurname` varchar(50) NOT NULL,
  `BirthDate` datetime(3) DEFAULT NULL,
  `Gender` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`AspNetUserId`),
  CONSTRAINT `FK_UserInformation_AspNetUsers` FOREIGN KEY (`AspNetUserId`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `userinformationview`
--

DROP TABLE IF EXISTS `userinformationview`;
/*!50001 DROP VIEW IF EXISTS `userinformationview`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `userinformationview` AS SELECT 
 1 AS `Id`,
 1 AS `UserName`,
 1 AS `Email`,
 1 AS `Active`,
 1 AS `ShortName`,
 1 AS `FullName`,
 1 AS `BirthDate`,
 1 AS `Gender`*/;
SET character_set_client = @saved_cs_client;

--
-- Final view structure for view `articlesview`
--

/*!50001 DROP VIEW IF EXISTS `articlesview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `articlesview` AS select `article`.`Id` AS `Id`,`article`.`Name` AS `Name`,`article`.`Description` AS `Description`,`article`.`Unit` AS `Unit`,`article`.`TradeMark` AS `TradeMark`,`article`.`Model` AS `Model`,`article`.`FamilyId` AS `FamilyId`,`family`.`Name` AS `FamilyName`,`family`.`GroupId` AS `GroupId`,`group`.`Name` AS `GroupName`,concat(`group`.`Code`,'-',`family`.`Code`,'-',`article`.`Code`) AS `Code`,`group`.`HasVariablePrice` AS `MustQuoteDaily`,`article`.`Observations` AS `Observations` from ((`purchasearticles` `article` join `articlefamilies` `family` on((`article`.`FamilyId` = `family`.`Id`))) join `articlegroups` `group` on((`family`.`GroupId` = `group`.`Id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `associatedcontactsview`
--

/*!50001 DROP VIEW IF EXISTS `associatedcontactsview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `associatedcontactsview` AS select `co`.`Id` AS `Id`,`co`.`FullName` AS `FullName`,`co`.`EmailAddress` AS `EmailAddress`,`co`.`MobilePhone` AS `MobilePhone`,`co`.`Position` AS `Position`,concat(`ci`.`Id`,'-c') AS `CompanyId`,'Client' AS `status` from ((`contacts` `co` join `clientcontacts` `cc` on((`co`.`Id` = `cc`.`ContactId`))) join `clients` `ci` on((`cc`.`ClientId` = `ci`.`Id`))) union select `co`.`Id` AS `Id`,`co`.`FullName` AS `FullName`,`co`.`EmailAddress` AS `EmailAddress`,`co`.`MobilePhone` AS `MobilePhone`,`co`.`Position` AS `Position`,concat(`po`.`Id`,'-p') AS `CompanyId`,'Provider' AS `status` from ((`contacts` `co` join `providercontacts` `pc` on((`co`.`Id` = `pc`.`ContactId`))) join `providers` `po` on((`pc`.`ProviderId` = `po`.`Id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `purchaseordersview`
--

/*!50001 DROP VIEW IF EXISTS `purchaseordersview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `purchaseordersview` AS select `po`.`Id` AS `Id`,`po`.`IssuedDate` AS `IssuedDate`,`po`.`DeliveryDate` AS `DeliveryDate`,`po`.`Currency` AS `Currency`,`po`.`Discount` AS `Discount`,`po`.`Status` AS `Status`,`po`.`ReceptionDate` AS `ReceptionDate`,`po`.`InvoiceNumber` AS `InvoiceNumber`,`po`.`Observations` AS `Observations`,`po`.`OrderNumber` AS `OrderNumber`,`po`.`ProviderId` AS `ProviderId`,`provider`.`LegalName` AS `ProviderLegalName`,`provider`.`HasTaxes` AS `HasTaxes`,`po`.`ProjectId` AS `ProjectId`,`project`.`ProjectNumber` AS `ProjectNumber`,`project`.`ClientId` AS `ClientId`,`client`.`MarketName` AS `ClientMarketName`,`po`.`RequisitionId` AS `RequisitionId`,if((NULL <> `po`.`RequisitionId`),`requisition`.`RequisitionNumber`,'N/A') AS `RequisitionNumber` from ((((`purchaseorders` `po` join `providers` `provider` on((`po`.`ProviderId` = `provider`.`Id`))) join `projects` `project` on((`po`.`ProjectId` = `project`.`Id`))) join `clients` `client` on((`project`.`ClientId` = `client`.`Id`))) left join `requisitions` `requisition` on((`po`.`RequisitionId` = `requisition`.`Id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `quotesview`
--

/*!50001 DROP VIEW IF EXISTS `quotesview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `quotesview` AS select `quote`.`Id` AS `Id`,`quote`.`ProviderId` AS `ProviderId`,`provider`.`MarketName` AS `ProviderMarketName`,`provider`.`LegalName` AS `ProviderLegalName`,`quote`.`ArticleId` AS `ArticleId`,`article`.`Name` AS `ArticleName`,`article`.`GroupId` AS `GroupId`,`article`.`GroupName` AS `GroupName`,`article`.`FamilyId` AS `FamilyId`,`article`.`FamilyName` AS `FamilyName`,`article`.`Code` AS `ArticleCode`,`quote`.`SKU` AS `SKU`,`quote`.`Price` AS `Price`,`quote`.`Currency` AS `Currency`,`quote`.`DateUpdated` AS `DateUpdated` from ((`quotes` `quote` join `articlesview` `article` on((`quote`.`ArticleId` = `article`.`Id`))) join `providers` `provider` on((`quote`.`ProviderId` = `provider`.`Id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `requisitionarticlesview`
--

/*!50001 DROP VIEW IF EXISTS `requisitionarticlesview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `requisitionarticlesview` AS select `reqarticles`.`RequisitionId` AS `RequisitionId`,`requisition`.`RequisitionNumber` AS `RequisitionNumber`,`reqarticles`.`ArticleId` AS `ArticleId`,`article`.`Name` AS `ArticleName`,`article`.`GroupId` AS `GroupId`,`article`.`GroupName` AS `GroupName`,`article`.`FamilyId` AS `FamilyId`,`article`.`FamilyName` AS `FamilyName`,`article`.`Code` AS `ArticleCode`,`reqarticles`.`ProjectId` AS `ProjectId`,`project`.`ProjectNumber` AS `ProjectNumber`,`reqarticles`.`Quantity` AS `Quantity`,`requisition`.`RequiredDate` AS `RequiredDate` from (((`requisitionarticles` `reqarticles` join `requisitions` `requisition` on((`requisition`.`Id` = `reqarticles`.`RequisitionId`))) join `articlesview` `article` on((`article`.`Id` = `reqarticles`.`ArticleId`))) join `projects` `project` on((`project`.`Id` = `reqarticles`.`ProjectId`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `userinformationview`
--

/*!50001 DROP VIEW IF EXISTS `userinformationview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `userinformationview` AS select `a`.`Id` AS `Id`,`a`.`UserName` AS `UserName`,`a`.`Email` AS `Email`,`a`.`EmailConfirmed` AS `Active`,concat(`i`.`FirstName`,' ',`i`.`FatherSurname`) AS `ShortName`,concat(`i`.`FirstName`,' ',`i`.`MiddleName`,' ',`i`.`FatherSurname`,' ',`i`.`MotherSurname`) AS `FullName`,`i`.`BirthDate` AS `BirthDate`,`i`.`Gender` AS `Gender` from (`aspnetusers` `a` join `userinformation` `i` on((`a`.`Id` = `i`.`AspNetUserId`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-05-31 13:36:59
