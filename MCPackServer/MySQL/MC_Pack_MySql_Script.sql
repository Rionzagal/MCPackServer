-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema mcpackdb
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `mcpackdb` ;

-- -----------------------------------------------------
-- Schema mcpackdb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mcpackdb` DEFAULT CHARACTER SET utf8 ;
USE `mcpackdb` ;

-- -----------------------------------------------------
-- Table `mcpackdb`.`__efmigrationshistory`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`__efmigrationshistory` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`__efmigrationshistory` (
  `MigrationId` VARCHAR(150) CHARACTER SET 'utf8' NOT NULL,
  `ProductVersion` VARCHAR(32) CHARACTER SET 'utf8' NOT NULL,
  PRIMARY KEY (`MigrationId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`articlegroups`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`articlegroups` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`articlegroups` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(30) NOT NULL,
  `Description` VARCHAR(100) NOT NULL,
  `Code` VARCHAR(20) CHARACTER SET 'utf8' NOT NULL,
  `HasVariablePrice` TINYINT NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`articlefamilies`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`articlefamilies` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`articlefamilies` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(30) NOT NULL,
  `Description` VARCHAR(100) NOT NULL,
  `Code` VARCHAR(20) NOT NULL,
  `GroupId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FK_ArticleFamilies_ArticleGroups` (`GroupId` ASC) ,
  CONSTRAINT `FK_ArticleFamilies_ArticleGroups`
    FOREIGN KEY (`GroupId`)
    REFERENCES `mcpackdb`.`articlegroups` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`clients`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`clients` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`clients` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `MarketName` VARCHAR(50) NOT NULL,
  `LegalName` VARCHAR(50) NOT NULL,
  `FiscalAddress` VARCHAR(100) CHARACTER SET 'utf8' NOT NULL,
  `City` VARCHAR(50) NOT NULL,
  `Province` VARCHAR(50) NOT NULL,
  `Country` VARCHAR(50) NOT NULL,
  `PostalCode` VARCHAR(50) NOT NULL,
  `PhoneNumber` VARCHAR(20) NOT NULL,
  `PaymentCondition` VARCHAR(50) NULL DEFAULT NULL,
  `Website` VARCHAR(50) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_Clients` (`Id` ASC) )
ENGINE = InnoDB
AUTO_INCREMENT = 10
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`projects`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`projects` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`projects` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `ClientId` INT NOT NULL,
  `Type` VARCHAR(50) NULL DEFAULT NULL,
  `Description` LONGTEXT NOT NULL,
  `Discount` DOUBLE NOT NULL,
  `AdmissionDate` DATETIME(3) NULL DEFAULT NULL,
  `CommitmentDate` DATETIME(3) NULL DEFAULT NULL,
  `DeliveryDate` DATETIME(3) NULL DEFAULT NULL,
  `RealDeliveryDate` DATETIME(3) NULL DEFAULT NULL,
  `DeliveryTime` VARCHAR(100) NOT NULL,
  `AgreedCurrency` VARCHAR(10) NOT NULL,
  `PaymentCurrency` VARCHAR(10) NOT NULL,
  `PaymentConditions` VARCHAR(100) NOT NULL,
  `SalesPerson` VARCHAR(50) NOT NULL,
  `Comision` DOUBLE NOT NULL,
  `HasTaxes` TINYINT NOT NULL,
  `Observations` LONGTEXT NULL DEFAULT NULL,
  `Code` VARCHAR(20) NOT NULL,
  `ProjectNumber` VARCHAR(20) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FK_Proyects_Clients` (`ClientId` ASC) ,
  CONSTRAINT `FK_Proyects_Clients`
    FOREIGN KEY (`ClientId`)
    REFERENCES `mcpackdb`.`clients` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1536
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`providers`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`providers` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`providers` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `MarketName` VARCHAR(50) NOT NULL,
  `LegalName` VARCHAR(50) NOT NULL,
  `FiscalAddress` VARCHAR(50) NOT NULL,
  `City` VARCHAR(50) NOT NULL,
  `Province` VARCHAR(50) NOT NULL,
  `Country` VARCHAR(50) NOT NULL,
  `PostalCode` VARCHAR(50) NOT NULL,
  `PhoneNumber` VARCHAR(20) NOT NULL,
  `Website` VARCHAR(50) NOT NULL,
  `TypeOfPayment` VARCHAR(50) NOT NULL,
  `PaymentCondition` VARCHAR(50) NOT NULL,
  `CreditLimit` VARCHAR(50) NULL DEFAULT NULL,
  `Discount` DOUBLE NOT NULL,
  `HomeDelivery` TINYINT NOT NULL,
  `Observations` LONGTEXT NULL DEFAULT NULL,
  `HasTaxes` TINYINT NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`purchaseorders`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`purchaseorders` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`purchaseorders` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `ProjectId` INT NOT NULL,
  `IssuedDate` DATETIME(3) NULL DEFAULT NULL,
  `ProviderId` INT NOT NULL,
  `RequisitionId` INT NULL DEFAULT NULL,
  `DeliveryDate` DATETIME(3) NULL DEFAULT NULL,
  `Currency` VARCHAR(10) NOT NULL,
  `Discount` DOUBLE NOT NULL,
  `Status` VARCHAR(50) NOT NULL,
  `ReceptionDate` DATETIME(3) NULL DEFAULT NULL,
  `InvoiceNumber` VARCHAR(50) NULL DEFAULT NULL,
  `Observations` LONGTEXT NULL DEFAULT NULL,
  `OrderNumber` VARCHAR(50) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FK_PurchaseOrders_Providers` (`ProviderId` ASC) ,
  INDEX `FK_PurchaseOrders_Projects_idx` (`ProjectId` ASC) ,
  CONSTRAINT `FK_PurchaseOrders_Projects`
    FOREIGN KEY (`ProjectId`)
    REFERENCES `mcpackdb`.`projects` (`Id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `FK_PurchaseOrders_Providers`
    FOREIGN KEY (`ProviderId`)
    REFERENCES `mcpackdb`.`providers` (`Id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`purchasearticles`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`purchasearticles` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`purchasearticles` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(50) NOT NULL,
  `Description` VARCHAR(100) NOT NULL,
  `Unit` VARCHAR(20) NOT NULL,
  `TradeMark` VARCHAR(50) NULL DEFAULT NULL,
  `Model` VARCHAR(50) NULL DEFAULT NULL,
  `FamilyId` INT NOT NULL,
  `Code` VARCHAR(20) NOT NULL,
  `Observations` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FK_ShoppingArticles_ArticleFamilies` (`FamilyId` ASC) ,
  CONSTRAINT `FK_ShoppingArticles_ArticleFamilies`
    FOREIGN KEY (`FamilyId`)
    REFERENCES `mcpackdb`.`articlefamilies` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`quotes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`quotes` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`quotes` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `ArticleId` INT NOT NULL,
  `ProviderId` INT NOT NULL,
  `Price` DOUBLE NOT NULL,
  `SKU` VARCHAR(50) NULL DEFAULT NULL,
  `DateUpdated` DATETIME(3) NOT NULL,
  `Currency` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FK_Quotes_Providers` (`ProviderId` ASC) ,
  INDEX `FK_Quotes_PurchaseArticles` (`ArticleId` ASC) ,
  CONSTRAINT `FK_Quotes_Providers`
    FOREIGN KEY (`ProviderId`)
    REFERENCES `mcpackdb`.`providers` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_Quotes_PurchaseArticles`
    FOREIGN KEY (`ArticleId`)
    REFERENCES `mcpackdb`.`purchasearticles` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`articlestopurchase`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`articlestopurchase` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`articlestopurchase` (
  `QuoteId` INT NOT NULL,
  `PurchaseOrderId` INT NOT NULL,
  `Quantity` INT NOT NULL,
  `EntryDate` DATETIME(3) NULL DEFAULT NULL,
  `DepartureDate` DATETIME(3) NULL DEFAULT NULL,
  `SalePrice` DOUBLE NOT NULL,
  PRIMARY KEY (`QuoteId`, `PurchaseOrderId`),
  INDEX `FK_ArticlesToPurchase_PurchaseOrders` (`PurchaseOrderId` ASC) ,
  CONSTRAINT `FK_ArticlesToPurchase_PurchaseOrders`
    FOREIGN KEY (`PurchaseOrderId`)
    REFERENCES `mcpackdb`.`purchaseorders` (`Id`),
  CONSTRAINT `FK_ArticlesToPurchase_Quotes`
    FOREIGN KEY (`QuoteId`)
    REFERENCES `mcpackdb`.`quotes` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`aspnetroles`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`aspnetroles` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`aspnetroles` (
  `Id` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `Name` VARCHAR(256) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `NormalizedName` VARCHAR(256) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `ConcurrencyStamp` LONGTEXT NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `RoleNameIndex` (`NormalizedName` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`aspnetroleclaims`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`aspnetroleclaims` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`aspnetroleclaims` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `RoleId` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `ClaimType` LONGTEXT NULL DEFAULT NULL,
  `ClaimValue` LONGTEXT NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_AspNetRoleClaims_RoleId` (`RoleId` ASC) ,
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId`
    FOREIGN KEY (`RoleId`)
    REFERENCES `mcpackdb`.`aspnetroles` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 249
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`aspnetusers`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`aspnetusers` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`aspnetusers` (
  `Id` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `UserName` VARCHAR(256) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `NormalizedUserName` VARCHAR(256) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `Email` VARCHAR(256) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `NormalizedEmail` VARCHAR(256) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `EmailConfirmed` TINYINT NOT NULL,
  `PasswordHash` LONGTEXT NULL DEFAULT NULL,
  `SecurityStamp` LONGTEXT NULL DEFAULT NULL,
  `ConcurrencyStamp` LONGTEXT NULL DEFAULT NULL,
  `PhoneNumber` LONGTEXT NULL DEFAULT NULL,
  `PhoneNumberConfirmed` TINYINT NOT NULL,
  `TwoFactorEnabled` TINYINT NOT NULL,
  `LockoutEnd` DATETIME(6) NULL DEFAULT NULL,
  `LockoutEnabled` TINYINT NOT NULL,
  `AccessFailedCount` INT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `UserNameIndex` (`NormalizedUserName` ASC) ,
  INDEX `EmailIndex` (`NormalizedEmail` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`aspnetuserclaims`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`aspnetuserclaims` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`aspnetuserclaims` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `UserId` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `ClaimType` LONGTEXT NULL DEFAULT NULL,
  `ClaimValue` LONGTEXT NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_AspNetUserClaims_UserId` (`UserId` ASC) ,
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId`
    FOREIGN KEY (`UserId`)
    REFERENCES `mcpackdb`.`aspnetusers` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`aspnetuserlogins`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`aspnetuserlogins` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`aspnetuserlogins` (
  `LoginProvider` VARCHAR(128) CHARACTER SET 'utf8' NOT NULL,
  `ProviderKey` VARCHAR(128) CHARACTER SET 'utf8' NOT NULL,
  `ProviderDisplayName` LONGTEXT NULL DEFAULT NULL,
  `UserId` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  PRIMARY KEY (`LoginProvider`, `ProviderKey`),
  INDEX `IX_AspNetUserLogins_UserId` (`UserId` ASC) ,
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId`
    FOREIGN KEY (`UserId`)
    REFERENCES `mcpackdb`.`aspnetusers` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`aspnetuserroles`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`aspnetuserroles` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`aspnetuserroles` (
  `UserId` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `RoleId` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  PRIMARY KEY (`UserId`, `RoleId`),
  INDEX `IX_AspNetUserRoles_RoleId` (`RoleId` ASC) ,
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId`
    FOREIGN KEY (`RoleId`)
    REFERENCES `mcpackdb`.`aspnetroles` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId`
    FOREIGN KEY (`UserId`)
    REFERENCES `mcpackdb`.`aspnetusers` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`aspnetusertokens`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`aspnetusertokens` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`aspnetusertokens` (
  `UserId` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `LoginProvider` VARCHAR(128) CHARACTER SET 'utf8' NOT NULL,
  `Name` VARCHAR(128) CHARACTER SET 'utf8' NOT NULL,
  `Value` LONGTEXT NULL DEFAULT NULL,
  PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId`
    FOREIGN KEY (`UserId`)
    REFERENCES `mcpackdb`.`aspnetusers` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`contacts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`contacts` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`contacts` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `FullName` VARCHAR(50) NOT NULL,
  `EmailAddress` VARCHAR(50) NOT NULL,
  `MobilePhone` VARCHAR(20) NOT NULL,
  `Position` VARCHAR(50) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`clientcontacts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`clientcontacts` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`clientcontacts` (
  `ClientId` INT NOT NULL,
  `ContactId` INT NOT NULL,
  PRIMARY KEY (`ClientId`, `ContactId`),
  INDEX `IX_ClientContacts` (`ClientId` ASC) ,
  INDEX `FK_ClientContacts_Contacts` (`ContactId` ASC) ,
  CONSTRAINT `FK_ClientContacts_Clients`
    FOREIGN KEY (`ClientId`)
    REFERENCES `mcpackdb`.`clients` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_ClientContacts_Contacts`
    FOREIGN KEY (`ContactId`)
    REFERENCES `mcpackdb`.`contacts` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`devicecodes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`devicecodes` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`devicecodes` (
  `UserCode` VARCHAR(200) CHARACTER SET 'utf8' NOT NULL,
  `DeviceCode` VARCHAR(200) CHARACTER SET 'utf8' NOT NULL,
  `SubjectId` VARCHAR(200) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `SessionId` VARCHAR(100) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `ClientId` VARCHAR(200) CHARACTER SET 'utf8' NOT NULL,
  `Description` VARCHAR(200) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `CreationTime` DATETIME(6) NOT NULL,
  `Expiration` DATETIME(6) NOT NULL,
  `Data` LONGTEXT NOT NULL,
  PRIMARY KEY (`UserCode`),
  UNIQUE INDEX `IX_DeviceCodes_DeviceCode` (`DeviceCode` ASC) ,
  INDEX `IX_DeviceCodes_Expiration` (`Expiration` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`keys`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`keys` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`keys` (
  `Id` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `Version` INT NOT NULL,
  `Created` DATETIME(6) NOT NULL,
  `Use` VARCHAR(450) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `Algorithm` VARCHAR(100) CHARACTER SET 'utf8' NOT NULL,
  `IsX509Certificate` TINYINT NOT NULL,
  `DataProtected` TINYINT NOT NULL,
  `Data` LONGTEXT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IX_Keys_Use` (`Use` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`logs`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`logs` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`logs` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `UserId` VARCHAR(450) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `Action` VARCHAR(50) NULL DEFAULT NULL,
  `TableName` VARCHAR(50) NULL DEFAULT NULL,
  `Succeded` TINYINT NOT NULL,
  `TimeOfAction` DATETIME NULL DEFAULT NULL,
  `Message` LONGTEXT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `FK_Logs_AspNetUsers_idx` (`UserId` ASC) ,
  CONSTRAINT `FK_Logs_AspNetUsers`
    FOREIGN KEY (`UserId`)
    REFERENCES `mcpackdb`.`aspnetusers` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`mcproducts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`mcproducts` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`mcproducts` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Description` VARCHAR(100) NULL DEFAULT NULL,
  `SugestedPrice` DOUBLE NOT NULL,
  `Currency` VARCHAR(5) NOT NULL,
  `Type` VARCHAR(50) NOT NULL,
  `Code` VARCHAR(50) NOT NULL,
  `Model` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL,
  `Observations` VARCHAR(200) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`persistedgrants`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`persistedgrants` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`persistedgrants` (
  `Key` VARCHAR(200) CHARACTER SET 'utf8' NOT NULL,
  `Type` VARCHAR(50) CHARACTER SET 'utf8' NOT NULL,
  `SubjectId` VARCHAR(200) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `SessionId` VARCHAR(100) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `ClientId` VARCHAR(200) CHARACTER SET 'utf8' NOT NULL,
  `Description` VARCHAR(200) CHARACTER SET 'utf8' NULL DEFAULT NULL,
  `CreationTime` DATETIME(6) NOT NULL,
  `Expiration` DATETIME(6) NULL DEFAULT NULL,
  `ConsumedTime` DATETIME(6) NULL DEFAULT NULL,
  `Data` LONGTEXT NOT NULL,
  PRIMARY KEY (`Key`),
  INDEX `IX_PersistedGrants_ConsumedTime` (`ConsumedTime` ASC) ,
  INDEX `IX_PersistedGrants_Expiration` (`Expiration` ASC) ,
  INDEX `IX_PersistedGrants_SubjectId_ClientId_Type` (`SubjectId` ASC, `ClientId` ASC, `Type` ASC) ,
  INDEX `IX_PersistedGrants_SubjectId_SessionId_Type` (`SubjectId` ASC, `SessionId` ASC, `Type` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`projectproducts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`projectproducts` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`projectproducts` (
  `ProductId` INT NOT NULL,
  `ProjectId` INT NOT NULL,
  `SalePrice` DOUBLE NOT NULL,
  `Quantity` INT NOT NULL,
  `Observations` LONGTEXT NULL DEFAULT NULL,
  PRIMARY KEY (`ProductId`, `ProjectId`),
  INDEX `FK_ProjectProducts_Projects_idx` (`ProjectId` ASC) ,
  CONSTRAINT `FK_ProjectProducts_MCProducts`
    FOREIGN KEY (`ProductId`)
    REFERENCES `mcpackdb`.`mcproducts` (`Id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `FK_ProjectProducts_Projects`
    FOREIGN KEY (`ProjectId`)
    REFERENCES `mcpackdb`.`projects` (`Id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`providercontacts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`providercontacts` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`providercontacts` (
  `ProviderId` INT NOT NULL,
  `ContactId` INT NOT NULL,
  PRIMARY KEY (`ProviderId`, `ContactId`),
  INDEX `FK_ProviderContacts_Contacts` (`ContactId` ASC) ,
  CONSTRAINT `FK_ProviderContacts_Contacts`
    FOREIGN KEY (`ContactId`)
    REFERENCES `mcpackdb`.`contacts` (`Id`)
    ON DELETE CASCADE,
  CONSTRAINT `FK_ProviderContacts_Providers`
    FOREIGN KEY (`ProviderId`)
    REFERENCES `mcpackdb`.`providers` (`Id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`requisitions`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`requisitions` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`requisitions` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `RequisitionNumber` VARCHAR(50) NOT NULL,
  `IssuedDate` DATETIME(3) NULL DEFAULT NULL,
  `UserId` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `RequiredDate` DATETIME(3) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `IX_Requisitions` (`Id` ASC) ,
  INDEX `FK_Requisitions_AspNetUsers` (`UserId` ASC) ,
  CONSTRAINT `FK_Requisitions_AspNetUsers`
    FOREIGN KEY (`UserId`)
    REFERENCES `mcpackdb`.`aspnetusers` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`requisitionarticles`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`requisitionarticles` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`requisitionarticles` (
  `RequisitionId` INT NOT NULL,
  `ArticleId` INT NOT NULL,
  `ProjectId` INT NOT NULL,
  `Quantity` INT NOT NULL,
  PRIMARY KEY (`RequisitionId`, `ProjectId`, `ArticleId`),
  INDEX `FK_RequisitionArticles_PurchaseArticles` (`ArticleId` ASC) ,
  INDEX `FK_RequisitionArticles_Projects_idx` (`ProjectId` ASC) ,
  CONSTRAINT `FK_RequisitionArticles_Projects`
    FOREIGN KEY (`ProjectId`)
    REFERENCES `mcpackdb`.`projects` (`Id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `FK_RequisitionArticles_PurchaseArticles`
    FOREIGN KEY (`ArticleId`)
    REFERENCES `mcpackdb`.`purchasearticles` (`Id`),
  CONSTRAINT `FK_RequisitionArticles_Requisitions`
    FOREIGN KEY (`RequisitionId`)
    REFERENCES `mcpackdb`.`requisitions` (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`systemrolepermissions`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`systemrolepermissions` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`systemrolepermissions` (
  `RoleId` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `PermissionValue` LONGTEXT NOT NULL,
  `Id` INT NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `IX_SystemRolePermissions` (`RoleId` ASC) ,
  CONSTRAINT `FK_SystemRolePermissions_AspNetRoles`
    FOREIGN KEY (`RoleId`)
    REFERENCES `mcpackdb`.`aspnetroles` (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;


-- -----------------------------------------------------
-- Table `mcpackdb`.`userinformation`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`userinformation` ;

CREATE TABLE IF NOT EXISTS `mcpackdb`.`userinformation` (
  `AspNetUserId` VARCHAR(450) CHARACTER SET 'utf8' NOT NULL,
  `FirstName` VARCHAR(50) NOT NULL,
  `MiddleName` VARCHAR(50) NULL DEFAULT NULL,
  `FatherSurname` VARCHAR(50) NOT NULL,
  `MotherSurname` VARCHAR(50) NOT NULL,
  `BirthDate` DATETIME(3) NULL DEFAULT NULL,
  `Gender` VARCHAR(50) NULL DEFAULT NULL,
  PRIMARY KEY (`AspNetUserId`),
  CONSTRAINT `FK_UserInformation_AspNetUsers`
    FOREIGN KEY (`AspNetUserId`)
    REFERENCES `mcpackdb`.`aspnetusers` (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci ;

USE `mcpackdb` ;

-- -----------------------------------------------------
-- Placeholder table for view `mcpackdb`.`associatedcontactsview`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mcpackdb`.`associatedcontactsview` (`Id` INT, `FullName` INT, `EmailAddress` INT, `MobilePhone` INT, `Position` INT, `CompanyId` INT, `status` INT);

-- -----------------------------------------------------
-- Placeholder table for view `mcpackdb`.`userinformationview`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mcpackdb`.`userinformationview` (`Id` INT, `UserName` INT, `Email` INT, `Active` INT, `ShortName` INT, `FullName` INT, `BirthDate` INT, `Gender` INT);

-- -----------------------------------------------------
-- View `mcpackdb`.`associatedcontactsview`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`associatedcontactsview`;
DROP VIEW IF EXISTS `mcpackdb`.`associatedcontactsview` ;
USE `mcpackdb`;
CREATE  OR REPLACE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `mcpackdb`.`associatedcontactsview` AS select `co`.`Id` AS `Id`,`co`.`FullName` AS `FullName`,`co`.`EmailAddress` AS `EmailAddress`,`co`.`MobilePhone` AS `MobilePhone`,`co`.`Position` AS `Position`,concat(`ci`.`Id`,'-c') AS `CompanyId`,'Client' AS `status` from ((`mcpackdb`.`contacts` `co` join `mcpackdb`.`clientcontacts` `cc` on((`co`.`Id` = `cc`.`ContactId`))) join `mcpackdb`.`clients` `ci` on((`cc`.`ClientId` = `ci`.`Id`))) union select `co`.`Id` AS `Id`,`co`.`FullName` AS `FullName`,`co`.`EmailAddress` AS `EmailAddress`,`co`.`MobilePhone` AS `MobilePhone`,`co`.`Position` AS `Position`,concat(`po`.`Id`,'-p') AS `CompanyId`,'Provider' AS `status` from ((`mcpackdb`.`contacts` `co` join `mcpackdb`.`providercontacts` `pc` on((`co`.`Id` = `pc`.`ContactId`))) join `mcpackdb`.`providers` `po` on((`pc`.`ProviderId` = `po`.`Id`)));

-- -----------------------------------------------------
-- View `mcpackdb`.`userinformationview`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mcpackdb`.`userinformationview`;
DROP VIEW IF EXISTS `mcpackdb`.`userinformationview` ;
USE `mcpackdb`;
CREATE  OR REPLACE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `mcpackdb`.`userinformationview` AS select `a`.`Id` AS `Id`,`a`.`UserName` AS `UserName`,`a`.`Email` AS `Email`,`a`.`EmailConfirmed` AS `Active`,concat(`i`.`FirstName`,' ',`i`.`FatherSurname`) AS `ShortName`,concat(`i`.`FirstName`,' ',`i`.`MiddleName`,' ',`i`.`FatherSurname`,' ',`i`.`MotherSurname`) AS `FullName`,`i`.`BirthDate` AS `BirthDate`,`i`.`Gender` AS `Gender` from (`mcpackdb`.`aspnetusers` `a` join `mcpackdb`.`userinformation` `i` on((`a`.`Id` = `i`.`AspNetUserId`)));

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
