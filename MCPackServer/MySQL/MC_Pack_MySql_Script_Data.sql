-- MySQL dump 10.13  Distrib 5.6.24, for Win64 (x86_64)
--
-- Host: localhost    Database: mcpackdb
-- ------------------------------------------------------
-- Server version	5.7.37-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
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
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `articlefamilies`
--

DROP TABLE IF EXISTS `articlefamilies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `articlefamilies` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(100) NOT NULL,
  `Code` varchar(20) NOT NULL,
  `GroupId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_ArticleFamilies_ArticleGroups` (`GroupId`),
  CONSTRAINT `FK_ArticleFamilies_ArticleGroups` FOREIGN KEY (`GroupId`) REFERENCES `articlegroups` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `articlefamilies`
--

LOCK TABLES `articlefamilies` WRITE;
/*!40000 ALTER TABLE `articlefamilies` DISABLE KEYS */;
INSERT INTO `articlefamilies` VALUES (2,'INOXIDABLE','ANGULO, CUADRADO, HEXAGONO, LAMINA, PLACA, PTR, REDONDO, SOLERA, TUBING, TUBO','IN',3),(3,'PANTALLAS','PANTALLAS','PA',4),(4,'CLEMAS','CLEMAS','CLE',4),(5,'BOTON','Dispositivo que tiene como objetivo vigilar y controlar las máquinas de un proceso','BOT',4);
/*!40000 ALTER TABLE `articlefamilies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `articlegroups`
--

DROP TABLE IF EXISTS `articlegroups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `articlegroups` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(100) NOT NULL,
  `Code` varchar(20) NOT NULL,
  `HasVariablePrice` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `articlegroups`
--

LOCK TABLES `articlegroups` WRITE;
/*!40000 ALTER TABLE `articlegroups` DISABLE KEYS */;
INSERT INTO `articlegroups` VALUES (3,'ACEROS Y METALES','TODO TIPO DE ACEROS Y METALES','ME',1),(4,'ELECTRICO','MATERIAL ELECTRICO Y ELECTRONICO','EL',0);
/*!40000 ALTER TABLE `articlegroups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `articlestopurchase`
--

DROP TABLE IF EXISTS `articlestopurchase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `articlestopurchase` (
  `QuoteId` int(11) NOT NULL,
  `PurchaseOrderId` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `ReceptionDate` datetime DEFAULT NULL,
  `DepartureDate` datetime(3) DEFAULT NULL,
  `SalePrice` double NOT NULL,
  PRIMARY KEY (`QuoteId`,`PurchaseOrderId`),
  KEY `FK_ArticlesToPurchase_PurchaseOrders` (`PurchaseOrderId`),
  CONSTRAINT `FK_ArticlesToPurchase_PurchaseOrders` FOREIGN KEY (`PurchaseOrderId`) REFERENCES `purchaseorders` (`Id`),
  CONSTRAINT `FK_ArticlesToPurchase_Quotes` FOREIGN KEY (`QuoteId`) REFERENCES `quotes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `articlestopurchase`
--

LOCK TABLES `articlestopurchase` WRITE;
/*!40000 ALTER TABLE `articlestopurchase` DISABLE KEYS */;
INSERT INTO `articlestopurchase` VALUES (1,2,0,NULL,NULL,300),(2,3,1,NULL,NULL,150);
/*!40000 ALTER TABLE `articlestopurchase` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `articlestopurchaseview`
--

DROP TABLE IF EXISTS `articlestopurchaseview`;
/*!50001 DROP VIEW IF EXISTS `articlestopurchaseview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `articlestopurchaseview` AS SELECT 
 1 AS `QuoteId`,
 1 AS `GroupId`,
 1 AS `GroupName`,
 1 AS `FamilyId`,
 1 AS `FamilyName`,
 1 AS `ArticleId`,
 1 AS `ArticleName`,
 1 AS `ArticleCode`,
 1 AS `TradeMark`,
 1 AS `Model`,
 1 AS `SKU`,
 1 AS `Unit`,
 1 AS `DateUpdated`,
 1 AS `PurchaseOrderId`,
 1 AS `Quantity`,
 1 AS `ReceptionDate`,
 1 AS `DepartureDate`,
 1 AS `SalePrice`,
 1 AS `Currency`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `articlesview`
--

DROP TABLE IF EXISTS `articlesview`;
/*!50001 DROP VIEW IF EXISTS `articlesview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
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
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(450) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=563 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
INSERT INTO `aspnetroleclaims` VALUES (249,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Users'),(250,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Users.View'),(251,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Users.Create'),(252,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Users.Edit'),(253,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Users.Delete'),(254,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Roles'),(255,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Roles.View'),(256,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Roles.Create'),(257,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Roles.Edit'),(258,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Roles.Delete'),(259,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Clients'),(260,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Clients.View'),(261,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Clients.Create'),(262,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Clients.Edit'),(263,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Clients.Delete'),(264,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Contacts'),(265,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Contacts.View'),(266,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Contacts.Create'),(267,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Contacts.Edit'),(268,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Contacts.Delete'),(269,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Providers'),(270,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Providers.View'),(271,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Providers.Create'),(272,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Providers.Edit'),(273,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Providers.Delete'),(274,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Products'),(275,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Products.View'),(276,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Products.Create'),(277,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Products.Edit'),(278,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Products.Delete'),(279,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Articles'),(280,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Articles.View'),(281,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Articles.Create'),(282,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Articles.Edit'),(283,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Articles.Delete'),(284,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleFamilies.View'),(285,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleFamilies.Create'),(286,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleFamilies.Edit'),(287,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleFamilies.Delete'),(288,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleGroups.View'),(289,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleGroups.Create'),(290,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleGroups.Edit'),(291,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleGroups.Delete'),(292,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Quotes.View'),(293,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Quotes.Create'),(294,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Quotes.Edit'),(295,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Quotes.Delete'),(296,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Projects'),(297,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Projects.View'),(298,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Projects.Create'),(299,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Projects.Edit'),(300,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Projects.Delete'),(301,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.PurchaseOrders'),(302,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.PurchaseOrders.View'),(303,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.PurchaseOrders.Create'),(304,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.PurchaseOrders.Edit'),(305,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.PurchaseOrders.Delete'),(306,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Requisitions'),(307,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Requisitions.View'),(308,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Requisitions.Create'),(309,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Requisitions.Edit'),(310,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Requisitions.Delete'),(311,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Users'),(312,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Users.View'),(313,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Users.Create'),(314,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Users.Edit'),(315,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Users.Delete'),(316,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Roles'),(317,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Roles.View'),(318,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Roles.Create'),(319,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Roles.Edit'),(320,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Roles.Delete'),(321,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Clients'),(322,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Clients.View'),(323,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Clients.Create'),(324,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Clients.Edit'),(325,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Clients.Delete'),(326,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Contacts'),(327,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Contacts.View'),(328,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Contacts.Create'),(329,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Contacts.Edit'),(330,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Contacts.Delete'),(331,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Providers'),(332,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Providers.View'),(333,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Providers.Create'),(334,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Providers.Edit'),(335,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Providers.Delete'),(336,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Products'),(337,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Products.View'),(338,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Products.Create'),(339,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Products.Edit'),(340,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Products.Delete'),(341,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Articles'),(342,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Articles.View'),(343,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Articles.Create'),(344,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Articles.Edit'),(345,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Articles.Delete'),(346,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleFamilies.View'),(347,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleFamilies.Create'),(348,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleFamilies.Edit'),(349,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleFamilies.Delete'),(350,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleGroups.View'),(351,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleGroups.Create'),(352,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleGroups.Edit'),(353,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleGroups.Delete'),(354,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Quotes.View'),(355,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Quotes.Create'),(356,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Quotes.Edit'),(357,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Quotes.Delete'),(358,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Projects'),(359,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Projects.View'),(360,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Projects.Create'),(361,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Projects.Edit'),(362,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Projects.Delete'),(363,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.PurchaseOrders'),(364,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.PurchaseOrders.View'),(365,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.PurchaseOrders.Create'),(366,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.PurchaseOrders.Edit'),(367,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.PurchaseOrders.Delete'),(368,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Requisitions'),(369,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Requisitions.View'),(370,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Requisitions.Create'),(371,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Requisitions.Edit'),(372,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Requisitions.Delete'),(373,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Reports.View'),(374,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Reports.Create'),(375,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ProjectSpecial.ClientChange'),(376,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.RoleClaims.View'),(377,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.RoleClaims.Create'),(378,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.RoleClaims.Edit'),(379,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.RoleClaims.Delete'),(380,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Reports.View'),(381,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Reports.Create'),(382,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ProjectSpecial.ClientChange'),(383,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.RoleClaims.View'),(384,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.RoleClaims.Create'),(385,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.RoleClaims.Edit'),(386,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.RoleClaims.Delete'),(387,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Menu.Providers'),(388,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Menu.Articles'),(389,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Menu.PurchaseOrders'),(390,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Menu.Requisitions'),(391,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Contacts.Create'),(392,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Contacts.View'),(393,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Contacts.Edit'),(394,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Contacts.Delete'),(395,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Providers.Create'),(396,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Providers.View'),(397,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Providers.Edit'),(399,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Articles.Create'),(400,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Articles.View'),(401,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Articles.Edit'),(403,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleFamilies.Create'),(404,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleFamilies.View'),(405,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleFamilies.Edit'),(407,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleGroups.Create'),(408,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleGroups.View'),(409,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleGroups.Edit'),(411,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Quotes.Create'),(412,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Quotes.View'),(413,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Quotes.Edit'),(415,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.PurchaseOrders.Create'),(416,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.PurchaseOrders.View'),(417,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.PurchaseOrders.Edit'),(419,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Requisitions.Create'),(420,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Requisitions.View'),(421,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Requisitions.Edit'),(423,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Reports.View'),(424,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Reports.Create'),(425,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Users'),(426,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Roles'),(427,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Clients'),(428,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Providers'),(429,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Products'),(430,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Articles'),(431,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Projects'),(432,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.PurchaseOrders'),(433,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Requisitions'),(434,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Users.View'),(435,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Roles.View'),(436,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Clients.View'),(437,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Articles.View'),(438,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Products.View'),(439,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Providers.View'),(440,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Contacts.View'),(441,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.ArticleFamilies.View'),(442,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.ArticleGroups.View'),(443,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Quotes.View'),(444,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Projects.View'),(445,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.PurchaseOrders.View'),(446,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Requisitions.View'),(447,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Reports.View'),(448,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Users'),(449,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Roles'),(450,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Clients'),(451,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Providers'),(452,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Products'),(453,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Articles'),(454,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Projects'),(455,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.PurchaseOrders'),(456,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Requisitions'),(457,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Clients.Create'),(458,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Clients.View'),(459,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Clients.Edit'),(460,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Clients.Delete'),(461,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Contacts.Create'),(462,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Contacts.View'),(463,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Contacts.Edit'),(464,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Contacts.Delete'),(465,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Products.Create'),(466,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Products.View'),(467,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Products.Edit'),(468,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Products.Delete'),(469,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Projects.Create'),(470,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Projects.View'),(471,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Projects.Edit'),(472,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Projects.Delete'),(473,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Reports.View'),(474,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Reports.Create'),(478,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.Providers'),(480,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.Articles'),(481,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.Projects'),(482,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.PurchaseOrders'),(483,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.Requisitions'),(484,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Contacts.Create'),(485,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Contacts.View'),(486,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Contacts.Edit'),(487,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Contacts.Delete'),(488,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Providers.Create'),(489,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Providers.View'),(490,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Providers.Edit'),(492,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Articles.Create'),(493,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Articles.View'),(494,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Articles.Edit'),(496,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleFamilies.Create'),(497,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleFamilies.View'),(498,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleFamilies.Edit'),(500,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleGroups.Create'),(501,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleGroups.View'),(502,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleGroups.Edit'),(504,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Quotes.Create'),(505,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Quotes.View'),(506,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Quotes.Edit'),(507,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Quotes.Delete'),(508,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.PurchaseOrders.Create'),(509,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.PurchaseOrders.View'),(510,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.PurchaseOrders.Edit'),(513,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Requisitions.View'),(516,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Reports.View'),(520,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Clients'),(521,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Providers'),(522,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Products'),(523,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Articles'),(524,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Projects'),(525,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.PurchaseOrders'),(526,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Requisitions'),(527,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Clients.Create'),(528,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Clients.View'),(529,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Clients.Edit'),(531,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Contacts.Create'),(532,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Contacts.View'),(533,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Contacts.Edit'),(534,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Contacts.Delete'),(535,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Products.Create'),(536,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Products.View'),(537,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Products.Edit'),(540,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Providers.View'),(544,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Quotes.View'),(548,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Projects.View'),(549,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Projects.Edit'),(552,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.PurchaseOrders.View'),(555,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Requisitions.Create'),(556,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Requisitions.View'),(557,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Requisitions.Edit'),(559,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Reports.View'),(560,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Reports.Create'),(561,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.History.View'),(562,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.History');
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(450) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(450) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `ProviderDisplayName` longtext,
  `UserId` varchar(450) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(450) NOT NULL,
  `RoleId` varchar(450) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` VALUES ('29467d9b-19d6-43cd-98ec-48164adf8462','1db0ca6c-4053-4d29-b2d7-604c37b27b5f'),('29467d9b-19d6-43cd-98ec-48164adf8462','35a25ec1-8631-4a65-98d6-7a6ecce83d32'),('eb8e18aa-90e6-48d1-a7a5-4f5f36552aee','3b9abf42-31f2-4d3d-8bb7-f3764be31b31'),('5518f7a4-2202-4598-8b27-a23f0864aedb','6E4134C5-FE58-478F-A6EE-DE4A1A87CC16'),('5c1a3262-c3d0-45d4-8df5-0f19901729b2','921a17ad-d6e3-4082-84fc-0c43b4a86f8d'),('81c4e175-5589-48e8-af61-b0e0218ed513','9a29c706-d755-421f-b15f-7a19b2e8fc20'),('24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','AABE1774-BA38-4EE6-89E6-0405E1F1A6A6'),('63f048db-8f22-492e-841d-298543e7a637','AABE1774-BA38-4EE6-89E6-0405E1F1A6A6');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(450) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(4) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `ConcurrencyStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(4) NOT NULL,
  `TwoFactorEnabled` tinyint(4) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(4) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','mario.gonzalez@mc-pack.com','MARIO.GONZALEZ@MC-PACK.COM','mario.gonzalez@mc-pack.com','MARIO.GONZALEZ@MC-PACK.COM',1,'AQAAAAEAACcQAAAAELHwgiT84OdkakvazK02mYEn2oBLgRJZZld8fkl5mYBY8yTrYiJaZviDVdNUAB1AUQ==','KG2ZNVHIR6JAO6UJ5NDT4S47SNEQ3WHQ','75d099e0-c736-404a-9e18-95c37c697125',NULL,0,0,NULL,1,0),('29467d9b-19d6-43cd-98ec-48164adf8462','administracion@mc-pack.com','ADMINISTRACION@MC-PACK.COM','administracion@mc-pack.com','ADMINISTRACION@MC-PACK.COM',1,'AQAAAAEAACcQAAAAEFil6Tim7PVE1i15A1vTx0Bpt6zI1fBOq3jIagfhSuS87eOXWum2RMsqpm+Vt73hCw==','UP5HINYJ2QF7YW557B6GRTPGMDBSSONN','9a99173f-836b-4838-b720-3cf23183ead9',NULL,0,0,NULL,1,0),('5518f7a4-2202-4598-8b27-a23f0864aedb','mario.gzz.gal@gmail.com','MARIO.GZZ.GAL@GMAIL.COM','mario.gzz.gal@gmail.com','MARIO.GZZ.GAL@GMAIL.COM',1,'AQAAAAEAACcQAAAAEIqK4MVDTb3gAz9CD6raroHQjjJHpYojg/UB4glgMjjv3zLT/sKKzrMC/Hc3F03rYw==','7ZAEQJSKINVPNY74QE3XAUUXEJJJUG6F','206dcbf9-2cd5-4f91-845f-262bd0f5bb65',NULL,0,0,NULL,1,0),('5c1a3262-c3d0-45d4-8df5-0f19901729b2','auxcompras.mc@hotmail.com','AUXCOMPRAS.MC@HOTMAIL.COM','auxcompras.mc@hotmail.com','AUXCOMPRAS.MC@HOTMAIL.COM',1,'AQAAAAEAACcQAAAAEDJ9RyfoPuvDZD1JlK3AUHpxajIlkoESkWX73N6JUAPv9SBG5eGb+R5Hri7g4smx3A==','Q2ZYU4AGYM5O3EPIRZFBC55WE26AGPPZ','5a9bc905-0199-47c9-ac63-024154c3e361',NULL,0,0,NULL,1,0),('63f048db-8f22-492e-841d-298543e7a637','ec.galindo@mc-pack.com','EC.GALINDO@MC-PACK.COM','ec.galindo@mc-pack.com','EC.GALINDO@MC-PACK.COM',1,'AQAAAAEAACcQAAAAEEJqe0wgrbvwV7+AJ6fAcsDH4+9wuZp0YS6A6SyTylUUXF3x/lLo9ebLOJnTV8sKLw==','DZGCVLM5DZHTOCO5T226KR5XEYCGUR4Z','384bf82a-6b79-497d-80c8-86ff2370bc96',NULL,0,0,NULL,1,0),('81c4e175-5589-48e8-af61-b0e0218ed513','direccion@mc-pack.com','DIRECCION@MC-PACK.COM','direccion@mc-pack.com','DIRECCION@MC-PACK.COM',1,'AQAAAAEAACcQAAAAEL9zcoJyYHcA10UIuFcEKpKZ6KgTOM5lGxDD1VFqR9na38xHfXNbus7NfzZtxchmzg==','WW2PG44RLPHID6KFPKVB7DTT3FW3BWAI','256a7b08-74a8-4e98-9c08-69b8a6361c92',NULL,0,0,NULL,1,0),('eb8e18aa-90e6-48d1-a7a5-4f5f36552aee','compras@mc-pack.com','COMPRAS@MC-PACK.COM','compras@mc-pack.com','COMPRAS@MC-PACK.COM',1,'AQAAAAEAACcQAAAAEDLH/K7hjEKdY1n+L1ZJber3yJsDG1rgNfJHER0By6RsoXY68SWzKEU7cxNqlO/z4A==','H2VJGV7MMRC4N25HT3YGMGQGP4Z22YOZ','59ce6f66-fd37-49cc-bc3f-6360a3f57987',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(450) NOT NULL,
  `LoginProvider` varchar(128) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `associatedcontactsview`
--

DROP TABLE IF EXISTS `associatedcontactsview`;
/*!50001 DROP VIEW IF EXISTS `associatedcontactsview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
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
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clientcontacts` (
  `ClientId` int(11) NOT NULL,
  `ContactId` int(11) NOT NULL,
  PRIMARY KEY (`ClientId`,`ContactId`),
  KEY `IX_ClientContacts` (`ClientId`),
  KEY `FK_ClientContacts_Contacts` (`ContactId`),
  CONSTRAINT `FK_ClientContacts_Clients` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ClientContacts_Contacts` FOREIGN KEY (`ContactId`) REFERENCES `contacts` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clientcontacts`
--

LOCK TABLES `clientcontacts` WRITE;
/*!40000 ALTER TABLE `clientcontacts` DISABLE KEYS */;
INSERT INTO `clientcontacts` VALUES (3,27),(3,28);
/*!40000 ALTER TABLE `clientcontacts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clients` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MarketName` varchar(100) NOT NULL,
  `LegalName` varchar(100) NOT NULL,
  `FiscalAddress` varchar(200) NOT NULL,
  `City` varchar(100) NOT NULL,
  `Province` varchar(100) NOT NULL,
  `Country` varchar(100) NOT NULL,
  `PostalCode` varchar(20) NOT NULL,
  `PhoneNumber` varchar(20) NOT NULL,
  `PaymentCondition` varchar(200) DEFAULT NULL,
  `Website` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Clients` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES (2,'ALPEZZI CHOCOLATE','ALPEZZI CHOCOLATE S.A. DE C.V.','PROLONGACION LOS ROBLES SUR 351, COL. LOS ROBLES','ZAPOPAN','JALISCO','MEXICO','45134','3330012000','30 DIAS','https://www.alpezzi.com.mx'),(3,'LUCAS - EFFEM MEXICO','EFFEM MEXICO INC Y COMPAÑIA','CARRETERA CHICHIMEQUILLAS KM 4.5','QUERETARO','QUERETARO','MEXICO','76246','4423866784','30 DIAS','www.mars.com');
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contacts`
--

DROP TABLE IF EXISTS `contacts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `contacts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(50) NOT NULL,
  `EmailAddress` varchar(50) DEFAULT NULL,
  `MobilePhone` varchar(20) DEFAULT NULL,
  `Position` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contacts`
--

LOCK TABLES `contacts` WRITE;
/*!40000 ALTER TABLE `contacts` DISABLE KEYS */;
INSERT INTO `contacts` VALUES (16,'ELVA CECILIA GALINDO CASAS','ECGCMC@LIVE.COM','3310640143',NULL),(17,'Mario Gonzalez','mario.gzz@hotmail.com','2222222222','ventas'),(18,'otro contacto de prueba','otro.contacto@mail.com','00112233445',NULL),(19,'otro contacto','otro.contacto@mail.com','001122333',NULL),(20,'otro contacto de prueba','otro.contacto@mail.com','000000000',NULL),(21,'OMAR MARTINEZ','inoxidable.ceal@gmail.com','3312946894','PROPIETARIO '),(22,'OMAR MARTINEZ','inoxidable.ceal@gmail.com','3312946894','PROPIETARIO '),(23,'OMAR','indceal@gamil.com','3311111111','GERENTE'),(24,'OMAR MENDEZ','indusceal@gmail.com','3333333333','GERENTE'),(25,'OMAR R','inoxceal@gmail.com','3333333333','GERENTE'),(26,'OMAR R','omar.r@mail.com','065032038','GERENTE'),(27,'VALERIA TORRES MATUS','valeria.torres.matus@effem.com','4423866784','CAPITAL BUYER'),(28,'ROBERTO RUBALCAVA','roberto.rubalcava@effem.com','8261370216','PROJECT MANAGER - ENGINEERING'),(30,'ALEJANDRA FIGUEROA','afigueroa@maincasa.com','3338111126','VENDEDORA'),(31,'ELENA HUERTA','ventas7gdl@aigsa.com.mx','3313689892','VENDEDORA ');
/*!40000 ALTER TABLE `contacts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `devicecodes`
--

DROP TABLE IF EXISTS `devicecodes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `devicecodes` (
  `UserCode` varchar(200) NOT NULL,
  `DeviceCode` varchar(200) NOT NULL,
  `SubjectId` varchar(200) DEFAULT NULL,
  `SessionId` varchar(100) DEFAULT NULL,
  `ClientId` varchar(200) NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) NOT NULL,
  `Data` longtext NOT NULL,
  PRIMARY KEY (`UserCode`),
  UNIQUE KEY `IX_DeviceCodes_DeviceCode` (`DeviceCode`),
  KEY `IX_DeviceCodes_Expiration` (`Expiration`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `devicecodes`
--

LOCK TABLES `devicecodes` WRITE;
/*!40000 ALTER TABLE `devicecodes` DISABLE KEYS */;
/*!40000 ALTER TABLE `devicecodes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `historyview`
--

DROP TABLE IF EXISTS `historyview`;
/*!50001 DROP VIEW IF EXISTS `historyview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `historyview` AS SELECT 
 1 AS `Id`,
 1 AS `Action`,
 1 AS `TableName`,
 1 AS `UserId`,
 1 AS `UserName`,
 1 AS `UserRoleId`,
 1 AS `UserRoleName`,
 1 AS `PersonShortName`,
 1 AS `PersonFullName`,
 1 AS `TimeOfAction`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `keys`
--

DROP TABLE IF EXISTS `keys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `keys` (
  `Id` varchar(450) NOT NULL,
  `Version` int(11) NOT NULL,
  `Created` datetime(6) NOT NULL,
  `Use` varchar(450) DEFAULT NULL,
  `Algorithm` varchar(100) NOT NULL,
  `IsX509Certificate` tinyint(4) NOT NULL,
  `DataProtected` tinyint(4) NOT NULL,
  `Data` longtext NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Keys_Use` (`Use`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `keys`
--

LOCK TABLES `keys` WRITE;
/*!40000 ALTER TABLE `keys` DISABLE KEYS */;
/*!40000 ALTER TABLE `keys` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `logs`
--

DROP TABLE IF EXISTS `logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `logs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(450) DEFAULT NULL,
  `Action` varchar(50) DEFAULT NULL,
  `TableName` varchar(50) DEFAULT NULL,
  `Succeeded` tinyint(4) NOT NULL,
  `TimeOfAction` datetime DEFAULT NULL,
  `Message` longtext NOT NULL,
  `Exception` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_Logs_AspNetUsers_idx` (`UserId`),
  CONSTRAINT `FK_Logs_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=120 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `logs`
--

LOCK TABLES `logs` WRITE;
/*!40000 ALTER TABLE `logs` DISABLE KEYS */;
INSERT INTO `logs` VALUES (1,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ArticleGroups',1,'2022-06-14 16:19:16','{\"Message\":\"Action: Insert completed successfuly in table ArticleGroups\",\"Value\":{\"Id\":5,\"Name\":\"MAQUINADO\",\"Description\":\"MATERIALES DE MAQUINADO\",\"Code\":\"MA\",\"HasVariablePrice\":false,\"ArticleFamilies\":null}}','N/A'),(2,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Quotes',1,'2022-06-14 16:20:59','{\"Message\":\"Action: Insert completed successfuly in table Quotes\",\"Value\":{\"Id\":2,\"ArticleId\":3,\"ProviderId\":2,\"Price\":150.0,\"SKU\":null,\"DateUpdated\":\"2022-06-14T16:20:58.6599828-05:00\",\"Currency\":\"MXN\",\"Article\":null,\"Provider\":null,\"ArticlesToPurchase\":null}}','N/A'),(3,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ArticlesToPurchase',1,'2022-06-14 16:25:57','{\"Message\":\"Action: Insert completed successfuly in table ArticlesToPurchase\",\"Value\":{\"QuoteId\":2,\"PurchaseOrderId\":3,\"Quantity\":1,\"ReceptionDate\":null,\"DepartureDate\":null,\"SalePrice\":150.0,\"PurchaseOrder\":null,\"Quote\":null}}','N/A'),(31,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PersonInformation',1,'2022-06-28 13:30:02','{\"Message\":\"Action: Insert completed successfuly in table PersonInformation\",\"Value\":{\"Id\":11,\"AspNetUserId\":\"5c1a3262-c3d0-45d4-8df5-0f19901729b2\",\"FirstName\":\"LESLIE\",\"MiddleName\":\"CORAL\",\"FatherSurname\":\"SANTACRUZ\",\"MotherSurname\":\"CASTELLANOS\",\"BirthDate\":\"2000-02-12T00:00:00\",\"Gender\":\"Femenino\"}}','N/A'),(32,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','AspNetUserRoles',1,'2022-06-28 13:30:02','{\"Message\":\"Action: Insert completed successfuly in table AspNetUserRoles\",\"Value\":{\"UserId\":\"5c1a3262-c3d0-45d4-8df5-0f19901729b2\",\"RoleId\":\"921a17ad-d6e3-4082-84fc-0c43b4a86f8d\",\"Role\":null,\"User\":null}}','N/A'),(33,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Contacts',1,'2022-06-28 13:32:21','{\"Message\":\"Action: Insert completed successfuly in table Contacts\",\"Value\":{\"Id\":25,\"FullName\":\"OMAR R\",\"EmailAddress\":\"inoxceal@gmail.com\",\"MobilePhone\":\"3333333333\",\"Position\":\"GERENTE\",\"ClientContacts\":null,\"ProviderContacts\":null}}','N/A'),(34,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Contacts',1,'2022-06-28 14:24:38','{\"Message\":\"Action: Insert completed successfuly in table Contacts\",\"Value\":{\"Id\":26,\"FullName\":\"OMAR R\",\"EmailAddress\":\"omar.r@mail.com\",\"MobilePhone\":\"065032038\",\"Position\":\"GERENTE\",\"ClientContacts\":null,\"ProviderContacts\":null}}','N/A'),(35,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ProviderContacts',1,'2022-06-28 14:24:39','{\"Message\":\"Action: Insert completed successfuly in table ProviderContacts\",\"Value\":{\"ProviderId\":3,\"ContactId\":26,\"Contact\":null,\"Provider\":null}}','N/A'),(36,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-06-28 17:24:24','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":5,\"MarketName\":\"LITOGRAFICA MONTES\",\"LegalName\":\"GUSTAVO ADOLFO MONTES GUERRERO \",\"FiscalAddress\":\"CONTRERAS MEDELLIN 176 COLONIA CENTRO\",\"City\":\"GUADALAJARA \",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44100\",\"PhoneNumber\":\"3336135353\",\"Website\":\"https://litograficamontes.com/impresion/\",\"TypeOfPayment\":\"TRANSFERENCIA \",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":false,\"Observations\":null,\"HasTaxes\":false,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(37,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Contacts',1,'2022-06-29 15:40:21','{\"Message\":\"Action: Insert completed successfuly in table Contacts\",\"Value\":{\"Id\":27,\"FullName\":\"VALERIA TORRES MATUS\",\"EmailAddress\":\"valeria.torres.matus@effem.com\",\"MobilePhone\":\"4423866784\",\"Position\":\"CAPITAL BUYER\",\"ClientContacts\":null,\"ProviderContacts\":null}}','N/A'),(38,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ClientContacts',1,'2022-06-29 15:40:21','{\"Message\":\"Action: Insert completed successfuly in table ClientContacts\",\"Value\":{\"ClientId\":3,\"ContactId\":27,\"Client\":null,\"Contact\":null}}','N/A'),(39,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Contacts',1,'2022-06-29 15:43:35','{\"Message\":\"Action: Insert completed successfuly in table Contacts\",\"Value\":{\"Id\":28,\"FullName\":\"ROBERTO RUBALCAVA\",\"EmailAddress\":\"roberto.rubalcava@effem.com\",\"MobilePhone\":\"8261370216\",\"Position\":\"PROJECT MANAGER - ENGINEERING\",\"ClientContacts\":null,\"ProviderContacts\":null}}','N/A'),(40,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ClientContacts',1,'2022-06-29 15:43:35','{\"Message\":\"Action: Insert completed successfuly in table ClientContacts\",\"Value\":{\"ClientId\":3,\"ContactId\":28,\"Client\":null,\"Contact\":null}}','N/A'),(41,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Update','ProjectProducts',1,'2022-06-29 15:49:03','{\"Message\":\"Action: Update completed successfuly in table ProjectProducts\",\"Value\":{\"ProductId\":1,\"ProjectId\":2,\"SalePrice\":145000.0,\"Quantity\":1,\"Observations\":\"PARA SKWINKLES RELLENOS\",\"Product\":null,\"Project\":null}}','N/A'),(42,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-06-30 11:38:34','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":6,\"MarketName\":\"SIG\",\"LegalName\":\"SOLUCIONES INDUSTRIALES GUADALAJARA\",\"FiscalAddress\":\"C. Pedro García Conde 470-A, 5 de Mayo\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO \",\"PostalCode\":\"44970\",\"PhoneNumber\":\"3336929718\",\"Website\":\"https://siginoxidables.com/\",\"TypeOfPayment\":\"TRANSFERENCIA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":false,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(43,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Contacts',1,'2022-06-30 11:40:56','{\"Message\":\"Action: Insert completed successfuly in table Contacts\",\"Value\":{\"Id\":29,\"FullName\":\"FELIX\",\"EmailAddress\":\"info@siginoxidables.com\",\"MobilePhone\":\"3311544313\",\"Position\":\"VENDEDOR\",\"ClientContacts\":null,\"ProviderContacts\":null}}','N/A'),(44,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ProviderContacts',1,'2022-06-30 11:40:56','{\"Message\":\"Action: Insert completed successfuly in table ProviderContacts\",\"Value\":{\"ProviderId\":6,\"ContactId\":29,\"Contact\":null,\"Provider\":null}}','N/A'),(45,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-06-30 11:52:02','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":7,\"MarketName\":\"CEPILLOS INDUSTRIALESS, HERRERIA Y SOLDADURA \",\"LegalName\":\"JOSÉ LUIS MORALES CIBRIÁN \",\"FiscalAddress\":\"MAZAMITLA NO.3 COLONIA JALISCO\",\"City\":\"TONALA\",\"Province\":\"JALISCO \",\"Country\":\"MEXICO\",\"PostalCode\":\"45403\",\"PhoneNumber\":\"3336656313\",\"Website\":\"N/A\",\"TypeOfPayment\":\"TRANSFERENCIA\",\"PaymentCondition\":\"CONTADO \",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":false,\"Observations\":null,\"HasTaxes\":false,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(46,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Update','Projects',1,'2022-06-30 16:25:02','{\"Message\":\"Action: Update completed successfuly in table Projects\",\"Value\":{\"Id\":2,\"ClientId\":3,\"Type\":\"Proyecto\",\"Description\":\"GUILLOTINA DE CORTE CON TECNOLOGIA ULTRASONICA\",\"Discount\":0.0,\"AdmissionDate\":\"2022-01-21T00:00:00\",\"CommitmentDate\":\"2022-04-15T00:00:00\",\"DeliveryDate\":\"2022-05-31T00:00:00\",\"RealDeliveryDate\":null,\"DeliveryTime\":\"12 A 14 SEM\",\"AgreedCurrency\":\"USD\",\"PaymentCurrency\":\"USD\",\"PaymentConditions\":\"50, 30-10, 20-CE\",\"SalesPerson\":\"MGC\",\"Comision\":0.0,\"HasTaxes\":true,\"Observations\":\"LAB EN NUESTRA PLANTA **Hola mundo**\",\"Code\":\"0C4dTP\",\"ProjectNumber\":\"0591\",\"Client\":null,\"ProjectProducts\":null,\"PurchaseOrders\":null,\"RequisitionArticles\":null}}','N/A'),(47,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-05 17:43:59','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":8,\"MarketName\":\"MAINCASA\",\"LegalName\":\"MAQUINARIA INDUSTRIAL CABRERA S.A DE C.V.\",\"FiscalAddress\":\"CALZADA LAZARO CARDENAS NO.1400 COL. MORELOS\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44910\",\"PhoneNumber\":\"3338111126\",\"Website\":\"www.maincasa.com\",\"TypeOfPayment\":\"TRANSFERENCIA\",\"PaymentCondition\":\"PAGO EN UNA SOLA EXHIBICIÓN \",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":false,\"Observations\":null,\"HasTaxes\":false,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(48,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Contacts',1,'2022-07-05 17:46:09','{\"Message\":\"Action: Insert completed successfuly in table Contacts\",\"Value\":{\"Id\":30,\"FullName\":\"ALEJANDRA FIGUEROA\",\"EmailAddress\":\"afigueroa@maincasa.com\",\"MobilePhone\":\"3338111126\",\"Position\":\"VENDEDORA\",\"ClientContacts\":null,\"ProviderContacts\":null}}','N/A'),(49,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ProviderContacts',1,'2022-07-05 17:46:09','{\"Message\":\"Action: Insert completed successfuly in table ProviderContacts\",\"Value\":{\"ProviderId\":8,\"ContactId\":30,\"Contact\":null,\"Provider\":null}}','N/A'),(50,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Delete','Contacts',1,'2022-07-14 13:56:10','{\"Message\":\"Action: Delete completed successfuly in table Contacts\",\"Value\":{\"Id\":29,\"FullName\":\"FELIX\",\"EmailAddress\":\"info@siginoxidables.com\",\"MobilePhone\":\"3311544313\",\"Position\":\"VENDEDOR\",\"ClientContacts\":[],\"ProviderContacts\":[]}}','N/A'),(51,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-14 14:00:05','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":9,\"MarketName\":\"MOTEC MEX\",\"LegalName\":\"MOTEC MEX S.A DE C.V.\",\"FiscalAddress\":\"AV. SAN RAFAEL 114, COL.SAN RAFAEL \",\"City\":\"GUADALAJARA \",\"Province\":\"JALISCO \",\"Country\":\"MEXICO \",\"PostalCode\":\"67110\",\"PhoneNumber\":\"3315922475\",\"Website\":\"https://motecmex.com.mx/\",\"TypeOfPayment\":\"TRANSFERENCIA\",\"PaymentCondition\":\"CONTADO \",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":false,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(52,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-18 12:15:17','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":10,\"MarketName\":\"DIMINOX\",\"LegalName\":\"DISTRIBUIDORA DE MATERIALES EN ONOXIDABLE DE MEXICO S.A DE C.V. \",\"FiscalAddress\":\"AV. 18 DE MARZO N.747 COL. LA NOGALERA\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44470\",\"PhoneNumber\":\"3312049757\",\"Website\":\"https://diminox.com/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(53,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-18 12:26:54','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":11,\"MarketName\":\"AIGSA\",\"LegalName\":\"ACERO INOXIDABLE DE GUADALAJARA S.A DE C.V.\",\"FiscalAddress\":\"CALLE 22 NO.2500 COL. ZONA INDUSTRIAL \",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO \",\"PostalCode\":\"44940\",\"PhoneNumber\":\"3310579730\",\"Website\":\"https://www.aigsa-aceroinoxidable.net/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(54,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Contacts',1,'2022-07-18 12:28:26','{\"Message\":\"Action: Insert completed successfuly in table Contacts\",\"Value\":{\"Id\":31,\"FullName\":\"ELENA HUERTA\",\"EmailAddress\":\"ventas7gdl@aigsa.com.mx\",\"MobilePhone\":\"3313689892\",\"Position\":\"VENDEDORA \",\"ClientContacts\":null,\"ProviderContacts\":null}}','N/A'),(55,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ProviderContacts',1,'2022-07-18 12:28:26','{\"Message\":\"Action: Insert completed successfuly in table ProviderContacts\",\"Value\":{\"ProviderId\":11,\"ContactId\":31,\"Contact\":null,\"Provider\":null}}','N/A'),(56,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ArticleFamilies',1,'2022-07-18 13:29:26','{\"Message\":\"Action: Insert completed successfuly in table ArticleFamilies\",\"Value\":{\"Id\":4,\"Name\":\"CLEMAS\",\"Description\":\"CLEMAS\",\"Code\":\"CLE\",\"GroupId\":4,\"Group\":null,\"PurchaseArticles\":null}}','N/A'),(61,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PurchaseArticles',1,'2022-07-18 13:38:41','{\"Message\":\"Action: Insert completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":4,\"Name\":\"CLEMAS PARA TIERRA 6MM\",\"Description\":\"CLAMA PARA TIERRA DE 6MM\",\"Unit\":\"PZ\",\"TradeMark\":null,\"Model\":null,\"FamilyId\":4,\"Code\":\"CGT6N\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(62,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-22 13:20:26','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":12,\"MarketName\":\"OXEJALSA\",\"LegalName\":\"OXIGENO Y ELECTRODOS DE JALISCO SA\",\"FiscalAddress\":\"5 DE FEBRERO N.27 COL. LAS CONCHAS\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXCIO\",\"PostalCode\":\"44460\",\"PhoneNumber\":\"3336195438\",\"Website\":\"https://oxejalsa.com.mx/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(63,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-22 13:27:40','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":13,\"MarketName\":\"INFRA\",\"LegalName\":\"INFRA S.A DE C.V.\",\"FiscalAddress\":\"CIRCUNVALACION OBLATOS N.1552 COL.SECTOR LIBERTAD\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44350\",\"PhoneNumber\":\"3336512699\",\"Website\":\"https://grupoinfra.com/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(64,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-22 13:29:30','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":14,\"MarketName\":\"TACSA\",\"LegalName\":\"TORNILLOS Y ACCESORIOS Y CONTROLES S.A DE C.V.\",\"FiscalAddress\":\"AV 8 DE JULIO NO.1690 COL.MORELOS\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44910\",\"PhoneNumber\":\"3338105200\",\"Website\":\"https://www.tornillostacsa.com/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(65,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-22 13:32:52','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":15,\"MarketName\":\"ADOPSSA\",\"LegalName\":\"A.D.O PAPELERIA Y SUMINISTROS\",\"FiscalAddress\":\"CALLE 10 NO.1874 COL. FERROCARRIL\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44440\",\"PhoneNumber\":\"3331689947\",\"Website\":\"N/A\",\"TypeOfPayment\":\"TRANSFERENCIA\",\"PaymentCondition\":\"CREDITO 15 DIAS\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(66,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-22 13:35:06','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":16,\"MarketName\":\"ASM\",\"LegalName\":\"ASM AUTOMATIZACION S.A DE C.V.\",\"FiscalAddress\":\"IGNACIO RAMIREZ NO.764 COL. SANTA TERESITA \",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44600\",\"PhoneNumber\":\"3311992683\",\"Website\":\"https://www.asmautomatizacion.com/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA\",\"PaymentCondition\":\"CREDITO 15 DIAS\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(67,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Delete','ArticleGroups',1,'2022-07-25 15:50:51','{\"Message\":\"Action: Delete completed successfuly in table ArticleGroups\",\"Value\":{\"Id\":5,\"Name\":\"MAQUINADO\",\"Description\":\"MATERIALES DE MAQUINADO\",\"Code\":\"MA\",\"HasVariablePrice\":false,\"ArticleFamilies\":[]}}','N/A'),(68,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-27 08:40:46','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":17,\"MarketName\":\"BIME SOLUCIONES EN SEGURIDAD INDUSTRIAL \",\"LegalName\":\"ELIZABETH BRIONES MARTINEZ\",\"FiscalAddress\":\"Calzada Lázaro Cárdenas 1800, Col. del Fresno\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44900\",\"PhoneNumber\":\"3332715773\",\"Website\":\"https://www.bimeseguridadindustrial.com/\",\"TypeOfPayment\":\"EFECTIVO\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":false,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(69,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-27 08:54:38','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":18,\"MarketName\":\"ELECTRICA ROMO\",\"LegalName\":\"ALAN ESAU ROMO GONZALEZ\",\"FiscalAddress\":\"12 DE OCTUBRE NO.345 COL, LA LOMA\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44410\",\"PhoneNumber\":\"3336574472\",\"Website\":\"https://electrica-romo.ueniweb.com/\",\"TypeOfPayment\":\"TRANSFERENCIA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(70,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-27 09:06:43','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":19,\"MarketName\":\"SIRIO\",\"LegalName\":\"SIRIO RIBBONS\",\"FiscalAddress\":\"Jesús González Ortega 23, Zona Centro\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO \",\"Country\":\"MEXICO\",\"PostalCode\":\"44100\",\"PhoneNumber\":\"3324699190\",\"Website\":\"https://sirioribbons.com/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(87,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-27 10:37:42','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":20,\"MarketName\":\"GALOPEZ TRATAMIENTOS\",\"LegalName\":\"PATRICIA ORQUIDEA CARMONA\",\"FiscalAddress\":\"R MICHEL C. HEROES FERROCARRILEROS 1136 \",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44790\",\"PhoneNumber\":\"3336193735\",\"Website\":\"N/A\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA \",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":false,\"Observations\":\"COLONIA LA AURORA \",\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(88,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-27 10:41:06','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":21,\"MarketName\":\"ABRASIVOS OLMEDO\",\"LegalName\":\"ABRASIVOS OLMEDO S.A. DE C.V.\",\"FiscalAddress\":\"Tabasco1178, Mezquitan Country\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO \",\"Country\":\"MEXICO\",\"PostalCode\":\"44260\",\"PhoneNumber\":\"3338246463\",\"Website\":\"https://www.abrasivosolmedo.com.mx/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":false,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(89,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-27 10:45:07','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":22,\"MarketName\":\"CATANIA UNIFORMES BORDADOS Y PROMOCIONALES\",\"LegalName\":\"RAUL HARO BUGARIN\",\"FiscalAddress\":\"C. Francisco Márquez 1547, San Miguel de Mezquitan\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44260\",\"PhoneNumber\":\"3338533555\",\"Website\":\"https://catania.com.mx/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONICA\",\"PaymentCondition\":\"CONTADO\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":false,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(90,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-27 10:48:47','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":23,\"MarketName\":\"AIG\",\"LegalName\":\"AUTOMATIZACION INDUSTRIAL DE GUADALAJARA\",\"FiscalAddress\":\"PERIFERICO SUR 53-A COL. EL BRICEÑO\",\"City\":\"ZAPOPAN\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"45236\",\"PhoneNumber\":\"3315782252\",\"Website\":\"http://aigdl.com/\",\"TypeOfPayment\":\"TRANSFERENCIA ELECTRONIA\",\"PaymentCondition\":\"CREDITO \",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(91,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','Providers',1,'2022-07-27 10:50:56','{\"Message\":\"Action: Insert completed successfuly in table Providers\",\"Value\":{\"Id\":24,\"MarketName\":\"BI HIGIENE\",\"LegalName\":\"JESUS ALEJANDRO IBARRA ZARATE\",\"FiscalAddress\":\"MANUEL NAVARRETE NO.3646 COL.LA AURORA\",\"City\":\"GUADALAJARA\",\"Province\":\"JALISCO\",\"Country\":\"MEXICO\",\"PostalCode\":\"44790\",\"PhoneNumber\":\"3331051948\",\"Website\":\"N/A\",\"TypeOfPayment\":\"TRANSFERENCIA\",\"PaymentCondition\":\"CREDITO 30 DIAS\",\"CreditLimit\":null,\"Discount\":0.0,\"HomeDelivery\":true,\"Observations\":null,\"HasTaxes\":true,\"ProviderContacts\":null,\"PurchaseOrders\":null,\"Quotes\":null}}','N/A'),(92,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Update','PurchaseArticles',1,'2022-07-27 15:22:45','{\"Message\":\"Action: Update completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":4,\"Name\":\"CLEMAS PARA TIERRA 6MM\",\"Description\":\"CLEMA PARA TIERRA DE 6MM\",\"Unit\":\"PZ\",\"TradeMark\":\"CONNECTWELL\",\"Model\":\"CGT4N\",\"FamilyId\":4,\"Code\":\"CGT6N\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(93,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PurchaseArticles',1,'2022-07-27 15:24:58','{\"Message\":\"Action: Insert completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":5,\"Name\":\"TAPA PARA CLEMA \",\"Description\":\"TAPA PARA CLEMA UN NIVEL\",\"Unit\":\"PZ\",\"TradeMark\":\"CONNECTWELL\",\"Model\":\"EP2.5/AUN\",\"FamilyId\":4,\"Code\":\"EP4UN\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(94,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PurchaseArticles',1,'2022-07-27 15:29:45','{\"Message\":\"Action: Insert completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":6,\"Name\":\"TOPE PARA CLEMA\",\"Description\":\"TOPE PARA CLEMA 4MM\",\"Unit\":\"PZ\",\"TradeMark\":\"CONNECTWELL\",\"Model\":\"CA802\",\"FamilyId\":4,\"Code\":\"CA802\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(95,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PurchaseArticles',1,'2022-07-27 16:09:54','{\"Message\":\"Action: Insert completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":7,\"Name\":\"CLEMA DE UN NIVEL\",\"Description\":\"CLEMA DE UN NIVEL DE 6MM\",\"Unit\":\"PZ\",\"TradeMark\":null,\"Model\":\"CTS6U\",\"FamilyId\":4,\"Code\":\"CTS6U\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(96,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Update','PurchaseArticles',1,'2022-07-27 16:10:30','{\"Message\":\"Action: Update completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":7,\"Name\":\"CLEMA DE UN NIVEL\",\"Description\":\"CLEMA DE UN NIVEL DE 6MM\",\"Unit\":\"PZ\",\"TradeMark\":\"CONNECTWELL\",\"Model\":\"CTS6U\",\"FamilyId\":4,\"Code\":\"CTS6U\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(97,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PurchaseArticles',1,'2022-07-27 17:32:58','{\"Message\":\"Action: Insert completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":8,\"Name\":\"CLEMA DE DOBLE NIVEL DE 4MM\",\"Description\":\"CLEMA DE DOBLE NIVEL DE 4MM\",\"Unit\":\"PZ\",\"TradeMark\":null,\"Model\":\"CDL4UN\",\"FamilyId\":4,\"Code\":\"CDL4UN\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(98,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Update','PurchaseArticles',1,'2022-07-27 17:33:51','{\"Message\":\"Action: Update completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":8,\"Name\":\"CLEMA DE DOBLE NIVEL DE 4MM\",\"Description\":\"CLEMA DE DOBLE NIVEL DE 4MM\",\"Unit\":\"PZ\",\"TradeMark\":\"CONNECTWELL\",\"Model\":\"CDL4UN\",\"FamilyId\":4,\"Code\":\"CDL4UN\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(99,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PurchaseArticles',1,'2022-07-27 17:40:44','{\"Message\":\"Action: Insert completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":9,\"Name\":\"CLEMA PORTA FUSIBLE SIN LED 4MM\",\"Description\":\"CLEMA PORTA FUSIBLE SIN LED 4MM\",\"Unit\":\"PZ\",\"TradeMark\":\"CONNECTWELL\",\"Model\":\"CAFL4U\",\"FamilyId\":4,\"Code\":\"CAFL4U\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(100,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PurchaseArticles',1,'2022-07-27 18:03:38','{\"Message\":\"Action: Insert completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":10,\"Name\":\"TAPA PARA CLEMA UN NIVEL 6MM\",\"Description\":\"TAPA PARA CLEMA UN NIVEL 6MM\",\"Unit\":\"PZ\",\"TradeMark\":\"CONNECTWELL\",\"Model\":\"EP6/10U\",\"FamilyId\":4,\"Code\":\"EP610U\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(101,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PurchaseArticles',1,'2022-07-27 18:05:47','{\"Message\":\"Action: Insert completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":11,\"Name\":\"TAPA PARA CLEMA PORTAFUSIBLE\",\"Description\":\"TAPA PARA CLEMA PORTAFUSIBLES \",\"Unit\":\"PZ\",\"TradeMark\":\"CONNECTWELL\",\"Model\":\"EPCAFL4U\",\"FamilyId\":4,\"Code\":\"EPCAFL4U\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(102,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','PurchaseArticles',1,'2022-07-27 18:07:17','{\"Message\":\"Action: Insert completed successfuly in table PurchaseArticles\",\"Value\":{\"Id\":12,\"Name\":\"PUENTES PARA CLEMAS\",\"Description\":\"PUENTES PARA CLEMAS\",\"Unit\":\"PZ\",\"TradeMark\":\"CONNECTWELL\",\"Model\":\"CA722/10\",\"FamilyId\":4,\"Code\":\"CA72210\",\"Observations\":null,\"Family\":null,\"Quotes\":null,\"RequisitionArticles\":null}}','N/A'),(103,'24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','Insert','ArticleFamilies',1,'2022-07-27 18:14:04','{\"Message\":\"Action: Insert completed successfuly in table ArticleFamilies\",\"Value\":{\"Id\":5,\"Name\":\"BOTON\",\"Description\":\"Dispositivo que tiene como objetivo vigilar y controlar las máquinas de un proceso\",\"Code\":\"BOT\",\"GroupId\":4,\"Group\":null,\"PurchaseArticles\":null}}','N/A');
/*!40000 ALTER TABLE `logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mcproducts`
--

DROP TABLE IF EXISTS `mcproducts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mcproducts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Description` varchar(100) DEFAULT NULL,
  `SugestedPrice` double NOT NULL,
  `Currency` varchar(5) NOT NULL,
  `Type` varchar(50) NOT NULL,
  `Code` varchar(50) NOT NULL,
  `Model` varchar(100) NOT NULL,
  `Observations` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mcproducts`
--

LOCK TABLES `mcproducts` WRITE;
/*!40000 ALTER TABLE `mcproducts` DISABLE KEYS */;
INSERT INTO `mcproducts` VALUES (1,'GUILLOTINA DE CORTE CON TECNOLOGIA ULTRASONICA ',163600,'USD','GUILLOTINA','GUUS001','MCESGU','OPERADA POR SERVOMOTORES');
/*!40000 ALTER TABLE `mcproducts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `persistedgrants`
--

DROP TABLE IF EXISTS `persistedgrants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `persistedgrants` (
  `Key` varchar(200) NOT NULL,
  `Type` varchar(50) NOT NULL,
  `SubjectId` varchar(200) DEFAULT NULL,
  `SessionId` varchar(100) DEFAULT NULL,
  `ClientId` varchar(200) NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) DEFAULT NULL,
  `ConsumedTime` datetime(6) DEFAULT NULL,
  `Data` longtext NOT NULL,
  PRIMARY KEY (`Key`),
  KEY `IX_PersistedGrants_ConsumedTime` (`ConsumedTime`),
  KEY `IX_PersistedGrants_Expiration` (`Expiration`),
  KEY `IX_PersistedGrants_SubjectId_ClientId_Type` (`SubjectId`,`ClientId`,`Type`),
  KEY `IX_PersistedGrants_SubjectId_SessionId_Type` (`SubjectId`,`SessionId`,`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `persistedgrants`
--

LOCK TABLES `persistedgrants` WRITE;
/*!40000 ALTER TABLE `persistedgrants` DISABLE KEYS */;
/*!40000 ALTER TABLE `persistedgrants` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `personinformation`
--

DROP TABLE IF EXISTS `personinformation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `personinformation` (
  `AspNetUserId` varchar(450) DEFAULT NULL,
  `FirstName` varchar(50) NOT NULL,
  `MiddleName` varchar(50) DEFAULT '',
  `FatherSurname` varchar(50) NOT NULL,
  `MotherSurname` varchar(50) DEFAULT NULL,
  `BirthDate` datetime(3) DEFAULT NULL,
  `Gender` varchar(50) DEFAULT NULL,
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `personinformation`
--

LOCK TABLES `personinformation` WRITE;
/*!40000 ALTER TABLE `personinformation` DISABLE KEYS */;
INSERT INTO `personinformation` VALUES ('24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','MARIO','','GONZÁLEZ','CERVANTES','1972-09-13 00:00:00.000','Masculino',2),('5518f7a4-2202-4598-8b27-a23f0864aedb','MARIO','','GONZALEZ','GALINDO','1998-07-15 00:00:00.000','Masculino',3),('eb8e18aa-90e6-48d1-a7a5-4f5f36552aee','JESSICA','','SANTACRUZ',NULL,'1994-11-10 00:00:00.000','Femenino',7),('81c4e175-5589-48e8-af61-b0e0218ed513','ABIGAIL','','QUIÑONEZ','GARCIA','1992-07-20 00:00:00.000','Femenino',8),('29467d9b-19d6-43cd-98ec-48164adf8462','NUSMETH','YARENY','FLORES','CORDOVA','1988-03-02 00:00:00.000','Femenino',9),('63f048db-8f22-492e-841d-298543e7a637','ELVA','CECILIA','GALINDO','CASAS','1972-01-13 00:00:00.000','Femenino',10),('5c1a3262-c3d0-45d4-8df5-0f19901729b2','LESLIE','CORAL','SANTACRUZ','CASTELLANOS','2000-02-12 00:00:00.000','Femenino',11);
/*!40000 ALTER TABLE `personinformation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `projectproducts`
--

DROP TABLE IF EXISTS `projectproducts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `projectproducts` (
  `ProductId` int(11) NOT NULL,
  `ProjectId` int(11) NOT NULL,
  `SalePrice` double NOT NULL,
  `Quantity` int(11) NOT NULL,
  `Observations` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`ProductId`,`ProjectId`),
  KEY `FK_ProjectProducts_Projects_idx` (`ProjectId`),
  CONSTRAINT `FK_ProjectProducts_MCProducts` FOREIGN KEY (`ProductId`) REFERENCES `mcproducts` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_ProjectProducts_Projects` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projectproducts`
--

LOCK TABLES `projectproducts` WRITE;
/*!40000 ALTER TABLE `projectproducts` DISABLE KEYS */;
INSERT INTO `projectproducts` VALUES (1,2,145000,1,'PARA SKWINKLES RELLENOS');
/*!40000 ALTER TABLE `projectproducts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `projectproductsview`
--

DROP TABLE IF EXISTS `projectproductsview`;
/*!50001 DROP VIEW IF EXISTS `projectproductsview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `projectproductsview` AS SELECT 
 1 AS `ProductId`,
 1 AS `ProjectId`,
 1 AS `SalePrice`,
 1 AS `Quantity`,
 1 AS `Observations`,
 1 AS `ProjectNumber`,
 1 AS `ProductType`,
 1 AS `ProductDescription`,
 1 AS `ProductCode`,
 1 AS `ProductModel`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `projects`
--

DROP TABLE IF EXISTS `projects`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `projects` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClientId` int(11) NOT NULL,
  `Type` varchar(50) DEFAULT NULL,
  `Description` varchar(200) NOT NULL,
  `Discount` double NOT NULL,
  `AdmissionDate` datetime(3) DEFAULT NULL,
  `CommitmentDate` datetime(3) DEFAULT NULL,
  `DeliveryDate` datetime(3) DEFAULT NULL,
  `RealDeliveryDate` datetime(3) DEFAULT NULL,
  `DeliveryTime` varchar(100) NOT NULL,
  `AgreedCurrency` varchar(10) NOT NULL,
  `PaymentCurrency` varchar(10) NOT NULL,
  `PaymentConditions` varchar(200) NOT NULL,
  `SalesPerson` varchar(50) NOT NULL,
  `Comision` double NOT NULL,
  `HasTaxes` tinyint(4) NOT NULL,
  `Observations` longtext,
  `Code` varchar(20) NOT NULL,
  `ProjectNumber` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Proyects_Clients` (`ClientId`),
  CONSTRAINT `FK_Proyects_Clients` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projects`
--

LOCK TABLES `projects` WRITE;
/*!40000 ALTER TABLE `projects` DISABLE KEYS */;
INSERT INTO `projects` VALUES (2,3,'Proyecto','GUILLOTINA DE CORTE CON TECNOLOGIA ULTRASONICA',0,'2022-01-21 00:00:00.000','2022-04-15 00:00:00.000','2022-05-31 00:00:00.000',NULL,'12 A 14 SEM','USD','USD','50, 30-10, 20-CE','MGC',0,1,'LAB EN NUESTRA PLANTA **Hola mundo**','0C4dTP','0591'),(19,3,'Refacciones','DESCRIPCION',0,'2022-02-11 00:00:00.000','2022-05-10 00:00:00.000','2022-05-10 00:00:00.000',NULL,'NO APLICA','MXN','MXN','30 DÍAS','MGC',0,1,'OBSERVACIONES','0C4dTR','0592');
/*!40000 ALTER TABLE `projects` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `projectsview`
--

DROP TABLE IF EXISTS `projectsview`;
/*!50001 DROP VIEW IF EXISTS `projectsview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `projectsview` AS SELECT 
 1 AS `Id`,
 1 AS `ClientId`,
 1 AS `Type`,
 1 AS `Description`,
 1 AS `Discount`,
 1 AS `AdmissionDate`,
 1 AS `CommitmentDate`,
 1 AS `DeliveryDate`,
 1 AS `RealDeliveryDate`,
 1 AS `DeliveryTime`,
 1 AS `AgreedCurrency`,
 1 AS `PaymentCurrency`,
 1 AS `PaymentConditions`,
 1 AS `SalesPerson`,
 1 AS `Comision`,
 1 AS `HasTaxes`,
 1 AS `Observations`,
 1 AS `Code`,
 1 AS `ProjectNumber`,
 1 AS `ClientMarketName`,
 1 AS `ClientLegalName`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `providercontacts`
--

DROP TABLE IF EXISTS `providercontacts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `providercontacts` (
  `ProviderId` int(11) NOT NULL,
  `ContactId` int(11) NOT NULL,
  PRIMARY KEY (`ProviderId`,`ContactId`),
  KEY `FK_ProviderContacts_Contacts` (`ContactId`),
  CONSTRAINT `FK_ProviderContacts_Contacts` FOREIGN KEY (`ContactId`) REFERENCES `contacts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ProviderContacts_Providers` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `providercontacts`
--

LOCK TABLES `providercontacts` WRITE;
/*!40000 ALTER TABLE `providercontacts` DISABLE KEYS */;
INSERT INTO `providercontacts` VALUES (2,16),(1,17),(3,26),(8,30),(11,31);
/*!40000 ALTER TABLE `providercontacts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `providers`
--

DROP TABLE IF EXISTS `providers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `providers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MarketName` varchar(100) NOT NULL,
  `LegalName` varchar(100) NOT NULL,
  `FiscalAddress` varchar(200) NOT NULL,
  `City` varchar(100) NOT NULL,
  `Province` varchar(100) NOT NULL,
  `Country` varchar(100) NOT NULL,
  `PostalCode` varchar(20) NOT NULL,
  `PhoneNumber` varchar(20) NOT NULL,
  `Website` varchar(100) NOT NULL,
  `TypeOfPayment` varchar(100) NOT NULL,
  `PaymentCondition` varchar(200) NOT NULL,
  `CreditLimit` varchar(100) DEFAULT NULL,
  `Discount` double NOT NULL,
  `HomeDelivery` tinyint(4) NOT NULL,
  `Observations` varchar(200) DEFAULT NULL,
  `HasTaxes` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `providers`
--

LOCK TABLES `providers` WRITE;
/*!40000 ALTER TABLE `providers` DISABLE KEYS */;
INSERT INTO `providers` VALUES (1,'Proveedor de prueba','Prueba proveedor S.A. de C.V.','Domicilio fiscal','Ciudad','Provincia','pais','01234','0000000','sitio web','Tipo de pago','Condicion de pago',NULL,0,0,NULL,1),(2,'LA PALOMA','LA PALOMA COMPAÑIA DE METALES SA DE CV','CALZ GONZALEZ GALLO 1600','GUADALAJARA','JALISCO','MEXICO','44870','3333333333','www.lapaloma.com','TRANSFERENCIA','CONTADO',NULL,0,1,'EN PIEZAS DE MENOR TAMAÑO Y POR PREMURA PASAMOS A RECOGER',1),(3,'CEAL INOXIDABLE ','INOXIDABLE CEAL ','JUAN PABLO II #1523 COLONIA FEDERALISMO ','GUADALAJARA ','JALISCO ','MEXICO ','44350','3312946894','N/A','TRANSFERENCIA ','CONTADO ',NULL,0,0,'CORTE LASER ',1),(4,'ACEROS SANCHEZ','JOSE DE JESUS SANCHEZ OROZCO ','SAN LORENZO 1736, COLONIA TALPITA','GUADALAJARA','JALISCO ','MEXICO ','44350','3323016735','N/A','TRANSFERENCIA ','CONTADO',NULL,0,1,NULL,1),(5,'LITOGRAFICA MONTES','GUSTAVO ADOLFO MONTES GUERRERO ','CONTRERAS MEDELLIN 176 COLONIA CENTRO','GUADALAJARA ','JALISCO','MEXICO','44100','3336135353','https://litograficamontes.com/impresion/','TRANSFERENCIA ','CONTADO',NULL,0,0,NULL,0),(6,'SIG','SOLUCIONES INDUSTRIALES GUADALAJARA','C. Pedro García Conde 470-A, 5 de Mayo','GUADALAJARA','JALISCO','MEXICO ','44970','3336929718','https://siginoxidables.com/','TRANSFERENCIA','CONTADO',NULL,0,1,NULL,0),(7,'CEPILLOS INDUSTRIALESS, HERRERIA Y SOLDADURA ','JOSÉ LUIS MORALES CIBRIÁN ','MAZAMITLA NO.3 COLONIA JALISCO','TONALA','JALISCO ','MEXICO','45403','3336656313','N/A','TRANSFERENCIA','CONTADO ',NULL,0,0,NULL,0),(8,'MAINCASA','MAQUINARIA INDUSTRIAL CABRERA S.A DE C.V.','CALZADA LAZARO CARDENAS NO.1400 COL. MORELOS','GUADALAJARA','JALISCO','MEXICO','44910','3338111126','www.maincasa.com','TRANSFERENCIA','PAGO EN UNA SOLA EXHIBICIÓN ',NULL,0,0,NULL,0),(9,'MOTEC MEX','MOTEC MEX S.A DE C.V.','AV. SAN RAFAEL 114, COL.SAN RAFAEL ','GUADALAJARA ','JALISCO ','MEXICO ','67110','3315922475','https://motecmex.com.mx/','TRANSFERENCIA','CONTADO ',NULL,0,1,NULL,0),(10,'DIMINOX','DISTRIBUIDORA DE MATERIALES EN ONOXIDABLE DE MEXICO S.A DE C.V. ','AV. 18 DE MARZO N.747 COL. LA NOGALERA','GUADALAJARA','JALISCO','MEXICO','44470','3312049757','https://diminox.com/','TRANSFERENCIA ELECTRONICA','CONTADO',NULL,0,1,NULL,1),(11,'AIGSA','ACERO INOXIDABLE DE GUADALAJARA S.A DE C.V.','CALLE 22 NO.2500 COL. ZONA INDUSTRIAL ','GUADALAJARA','JALISCO','MEXICO ','44940','3310579730','https://www.aigsa-aceroinoxidable.net/','TRANSFERENCIA ELECTRONICA','CONTADO',NULL,0,1,NULL,1),(12,'OXEJALSA','OXIGENO Y ELECTRODOS DE JALISCO SA','5 DE FEBRERO N.27 COL. LAS CONCHAS','GUADALAJARA','JALISCO','MEXCIO','44460','3336195438','https://oxejalsa.com.mx/','TRANSFERENCIA ELECTRONICA','CONTADO',NULL,0,1,NULL,1),(13,'INFRA','INFRA S.A DE C.V.','CIRCUNVALACION OBLATOS N.1552 COL.SECTOR LIBERTAD','GUADALAJARA','JALISCO','MEXICO','44350','3336512699','https://grupoinfra.com/','TRANSFERENCIA ELECTRONICA','CONTADO',NULL,0,1,NULL,1),(14,'TACSA','TORNILLOS Y ACCESORIOS Y CONTROLES S.A DE C.V.','AV 8 DE JULIO NO.1690 COL.MORELOS','GUADALAJARA','JALISCO','MEXICO','44910','3338105200','https://www.tornillostacsa.com/','TRANSFERENCIA ELECTRONICA','CONTADO',NULL,0,1,NULL,1),(15,'ADOPSSA','A.D.O PAPELERIA Y SUMINISTROS','CALLE 10 NO.1874 COL. FERROCARRIL','GUADALAJARA','JALISCO','MEXICO','44440','3331689947','N/A','TRANSFERENCIA','CREDITO 15 DIAS',NULL,0,1,NULL,1),(16,'ASM','ASM AUTOMATIZACION S.A DE C.V.','IGNACIO RAMIREZ NO.764 COL. SANTA TERESITA ','GUADALAJARA','JALISCO','MEXICO','44600','3311992683','https://www.asmautomatizacion.com/','TRANSFERENCIA ELECTRONICA','CREDITO 15 DIAS',NULL,0,1,NULL,1),(17,'BIME SOLUCIONES EN SEGURIDAD INDUSTRIAL ','ELIZABETH BRIONES MARTINEZ','Calzada Lázaro Cárdenas 1800, Col. del Fresno','GUADALAJARA','JALISCO','MEXICO','44900','3332715773','https://www.bimeseguridadindustrial.com/','EFECTIVO','CONTADO',NULL,0,0,NULL,1),(18,'ELECTRICA ROMO','ALAN ESAU ROMO GONZALEZ','12 DE OCTUBRE NO.345 COL, LA LOMA','GUADALAJARA','JALISCO','MEXICO','44410','3336574472','https://electrica-romo.ueniweb.com/','TRANSFERENCIA','CONTADO',NULL,0,1,NULL,1),(19,'SIRIO','SIRIO RIBBONS','Jesús González Ortega 23, Zona Centro','GUADALAJARA','JALISCO ','MEXICO','44100','3324699190','https://sirioribbons.com/','TRANSFERENCIA ELECTRONICA','CONTADO',NULL,0,1,NULL,1),(20,'GALOPEZ TRATAMIENTOS','PATRICIA ORQUIDEA CARMONA','R MICHEL C. HEROES FERROCARRILEROS 1136 ','GUADALAJARA','JALISCO','MEXICO','44790','3336193735','N/A','TRANSFERENCIA ELECTRONICA ','CONTADO',NULL,0,0,'COLONIA LA AURORA ',1),(21,'ABRASIVOS OLMEDO','ABRASIVOS OLMEDO S.A. DE C.V.','Tabasco1178, Mezquitan Country','GUADALAJARA','JALISCO ','MEXICO','44260','3338246463','https://www.abrasivosolmedo.com.mx/','TRANSFERENCIA ELECTRONICA','CONTADO',NULL,0,0,NULL,1),(22,'CATANIA UNIFORMES BORDADOS Y PROMOCIONALES','RAUL HARO BUGARIN','C. Francisco Márquez 1547, San Miguel de Mezquitan','GUADALAJARA','JALISCO','MEXICO','44260','3338533555','https://catania.com.mx/','TRANSFERENCIA ELECTRONICA','CONTADO',NULL,0,0,NULL,1),(23,'AIG','AUTOMATIZACION INDUSTRIAL DE GUADALAJARA','PERIFERICO SUR 53-A COL. EL BRICEÑO','ZAPOPAN','JALISCO','MEXICO','45236','3315782252','http://aigdl.com/','TRANSFERENCIA ELECTRONIA','CREDITO ',NULL,0,1,NULL,1),(24,'BI HIGIENE','JESUS ALEJANDRO IBARRA ZARATE','MANUEL NAVARRETE NO.3646 COL.LA AURORA','GUADALAJARA','JALISCO','MEXICO','44790','3331051948','N/A','TRANSFERENCIA','CREDITO 30 DIAS',NULL,0,1,NULL,1);
/*!40000 ALTER TABLE `providers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchasearticles`
--

DROP TABLE IF EXISTS `purchasearticles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `purchasearticles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(100) NOT NULL,
  `Unit` varchar(20) NOT NULL,
  `TradeMark` varchar(100) DEFAULT NULL,
  `Model` varchar(100) DEFAULT NULL,
  `FamilyId` int(11) NOT NULL,
  `Code` varchar(20) NOT NULL,
  `Observations` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_ShoppingArticles_ArticleFamilies` (`FamilyId`),
  CONSTRAINT `FK_ShoppingArticles_ArticleFamilies` FOREIGN KEY (`FamilyId`) REFERENCES `articlefamilies` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchasearticles`
--

LOCK TABLES `purchasearticles` WRITE;
/*!40000 ALTER TABLE `purchasearticles` DISABLE KEYS */;
INSERT INTO `purchasearticles` VALUES (2,'PANTALLA 7\" WECON','PANTALLA HMI 7\" CODIGO PI3070 2 ENTRADAS','PZA','WECON','PI3070',3,'WE7001',NULL),(3,'LAMINA DE ACERO INOX CALIBRE 12 DE 5 X 10 PIES','LAMINA DE ACERO INOXIDABLE CALIBRE 12 DE 5 X 10 PIES','PZA',NULL,NULL,2,'LA12',NULL),(4,'CLEMAS PARA TIERRA 6MM','CLEMA PARA TIERRA DE 6MM','PZ','CONNECTWELL','CGT4N',4,'CGT6N',NULL),(5,'TAPA PARA CLEMA ','TAPA PARA CLEMA UN NIVEL','PZ','CONNECTWELL','EP2.5/AUN',4,'EP4UN',NULL),(6,'TOPE PARA CLEMA','TOPE PARA CLEMA 4MM','PZ','CONNECTWELL','CA802',4,'CA802',NULL),(7,'CLEMA DE UN NIVEL','CLEMA DE UN NIVEL DE 6MM','PZ','CONNECTWELL','CTS6U',4,'CTS6U',NULL),(8,'CLEMA DE DOBLE NIVEL DE 4MM','CLEMA DE DOBLE NIVEL DE 4MM','PZ','CONNECTWELL','CDL4UN',4,'CDL4UN',NULL),(9,'CLEMA PORTA FUSIBLE SIN LED 4MM','CLEMA PORTA FUSIBLE SIN LED 4MM','PZ','CONNECTWELL','CAFL4U',4,'CAFL4U',NULL),(10,'TAPA PARA CLEMA UN NIVEL 6MM','TAPA PARA CLEMA UN NIVEL 6MM','PZ','CONNECTWELL','EP6/10U',4,'EP610U',NULL),(11,'TAPA PARA CLEMA PORTAFUSIBLE','TAPA PARA CLEMA PORTAFUSIBLES ','PZ','CONNECTWELL','EPCAFL4U',4,'EPCAFL4U',NULL),(12,'PUENTES PARA CLEMAS','PUENTES PARA CLEMAS','PZ','CONNECTWELL','CA722/10',4,'CA72210',NULL);
/*!40000 ALTER TABLE `purchasearticles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchaseorders`
--

DROP TABLE IF EXISTS `purchaseorders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `purchaseorders` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ProjectId` int(11) NOT NULL,
  `IssuedDate` datetime(3) DEFAULT NULL,
  `ProviderId` int(11) NOT NULL,
  `RequisitionId` int(11) DEFAULT NULL,
  `DeliveryDate` datetime(3) DEFAULT NULL,
  `Currency` varchar(10) NOT NULL,
  `Discount` double NOT NULL,
  `Status` varchar(50) NOT NULL,
  `ReceptionDate` datetime(3) DEFAULT NULL,
  `InvoiceNumber` varchar(50) DEFAULT NULL,
  `Observations` longtext,
  `OrderNumber` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_PurchaseOrders_Providers` (`ProviderId`),
  KEY `FK_PurchaseOrders_Projects_idx` (`ProjectId`),
  CONSTRAINT `FK_PurchaseOrders_Projects` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_PurchaseOrders_Providers` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseorders`
--

LOCK TABLES `purchaseorders` WRITE;
/*!40000 ALTER TABLE `purchaseorders` DISABLE KEYS */;
INSERT INTO `purchaseorders` VALUES (2,2,'2022-05-05 17:59:10.734',2,1,'2022-05-09 00:00:00.000','USD',0,'Pendiente',NULL,NULL,NULL,NULL),(3,2,'2022-05-10 17:14:00.250',2,1,'2022-05-13 00:00:00.000','MXN',0,'Pendiente',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `purchaseorders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `purchaseordersview`
--

DROP TABLE IF EXISTS `purchaseordersview`;
/*!50001 DROP VIEW IF EXISTS `purchaseordersview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
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
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quotes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ArticleId` int(11) NOT NULL,
  `ProviderId` int(11) NOT NULL,
  `Price` double NOT NULL,
  `SKU` varchar(100) DEFAULT NULL,
  `DateUpdated` datetime(3) NOT NULL,
  `Currency` varchar(10) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Quotes_Providers` (`ProviderId`),
  KEY `FK_Quotes_PurchaseArticles` (`ArticleId`),
  CONSTRAINT `FK_Quotes_Providers` FOREIGN KEY (`ProviderId`) REFERENCES `providers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Quotes_PurchaseArticles` FOREIGN KEY (`ArticleId`) REFERENCES `purchasearticles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quotes`
--

LOCK TABLES `quotes` WRITE;
/*!40000 ALTER TABLE `quotes` DISABLE KEYS */;
INSERT INTO `quotes` VALUES (1,2,2,300,'H505067','2022-05-04 17:57:07.123','USD'),(2,3,2,150,NULL,'2022-06-14 16:20:58.660','MXN');
/*!40000 ALTER TABLE `quotes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `quotesview`
--

DROP TABLE IF EXISTS `quotesview`;
/*!50001 DROP VIEW IF EXISTS `quotesview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
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
 1 AS `MustQuoteDaily`,
 1 AS `SKU`,
 1 AS `Price`,
 1 AS `Currency`,
 1 AS `DateUpdated`,
 1 AS `ProviderDiscount`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `requisitionarticles`
--

DROP TABLE IF EXISTS `requisitionarticles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `requisitionarticles` (
  `RequisitionId` int(11) NOT NULL,
  `ArticleId` int(11) NOT NULL,
  `ProjectId` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL,
  PRIMARY KEY (`RequisitionId`,`ProjectId`,`ArticleId`),
  KEY `FK_RequisitionArticles_PurchaseArticles` (`ArticleId`),
  KEY `FK_RequisitionArticles_Projects_idx` (`ProjectId`),
  CONSTRAINT `FK_RequisitionArticles_Projects` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_RequisitionArticles_PurchaseArticles` FOREIGN KEY (`ArticleId`) REFERENCES `purchasearticles` (`Id`),
  CONSTRAINT `FK_RequisitionArticles_Requisitions` FOREIGN KEY (`RequisitionId`) REFERENCES `requisitions` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requisitionarticles`
--

LOCK TABLES `requisitionarticles` WRITE;
/*!40000 ALTER TABLE `requisitionarticles` DISABLE KEYS */;
INSERT INTO `requisitionarticles` VALUES (1,2,2,1),(1,3,2,1);
/*!40000 ALTER TABLE `requisitionarticles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `requisitionarticlesview`
--

DROP TABLE IF EXISTS `requisitionarticlesview`;
/*!50001 DROP VIEW IF EXISTS `requisitionarticlesview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
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
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `requisitions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RequisitionNumber` varchar(20) NOT NULL,
  `IssuedDate` datetime(3) DEFAULT NULL,
  `UserId` varchar(450) NOT NULL,
  `RequiredDate` datetime(3) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Requisitions` (`Id`),
  KEY `FK_Requisitions_AspNetUsers` (`UserId`),
  CONSTRAINT `FK_Requisitions_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requisitions`
--

LOCK TABLES `requisitions` WRITE;
/*!40000 ALTER TABLE `requisitions` DISABLE KEYS */;
INSERT INTO `requisitions` VALUES (1,'00001','2022-05-05 00:00:00.000','63f048db-8f22-492e-841d-298543e7a637','2022-05-09 00:00:00.000');
/*!40000 ALTER TABLE `requisitions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `requisitionsview`
--

DROP TABLE IF EXISTS `requisitionsview`;
/*!50001 DROP VIEW IF EXISTS `requisitionsview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `requisitionsview` AS SELECT 
 1 AS `Id`,
 1 AS `RequisitionNumber`,
 1 AS `RequiredDate`,
 1 AS `IssuedDate`,
 1 AS `UserId`,
 1 AS `UserName`,
 1 AS `UserShortName`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `systemrolepermissions`
--

DROP TABLE IF EXISTS `systemrolepermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `systemrolepermissions` (
  `RoleId` varchar(450) NOT NULL,
  `PermissionValue` longtext NOT NULL,
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_SystemRolePermissions` (`RoleId`),
  CONSTRAINT `FK_SystemRolePermissions_AspNetRoles` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `systemrolepermissions`
--

LOCK TABLES `systemrolepermissions` WRITE;
/*!40000 ALTER TABLE `systemrolepermissions` DISABLE KEYS */;
/*!40000 ALTER TABLE `systemrolepermissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `userpersonalinformationview`
--

DROP TABLE IF EXISTS `userpersonalinformationview`;
/*!50001 DROP VIEW IF EXISTS `userpersonalinformationview`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `userpersonalinformationview` AS SELECT 
 1 AS `Id`,
 1 AS `UserName`,
 1 AS `Email`,
 1 AS `Active`,
 1 AS `UserRoleId`,
 1 AS `UserRoleName`,
 1 AS `ShortName`,
 1 AS `FullName`,
 1 AS `BirthDate`,
 1 AS `Gender`*/;
SET character_set_client = @saved_cs_client;

--
-- Final view structure for view `articlestopurchaseview`
--

/*!50001 DROP VIEW IF EXISTS `articlestopurchaseview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `articlestopurchaseview` AS select `poarticle`.`QuoteId` AS `QuoteId`,`quote`.`GroupId` AS `GroupId`,`quote`.`GroupName` AS `GroupName`,`quote`.`FamilyId` AS `FamilyId`,`quote`.`FamilyName` AS `FamilyName`,`quote`.`ArticleId` AS `ArticleId`,`quote`.`ArticleName` AS `ArticleName`,`quote`.`ArticleCode` AS `ArticleCode`,`article`.`TradeMark` AS `TradeMark`,`article`.`Model` AS `Model`,`quote`.`SKU` AS `SKU`,`article`.`Unit` AS `Unit`,`quote`.`DateUpdated` AS `DateUpdated`,`poarticle`.`PurchaseOrderId` AS `PurchaseOrderId`,`poarticle`.`Quantity` AS `Quantity`,`poarticle`.`ReceptionDate` AS `ReceptionDate`,`poarticle`.`DepartureDate` AS `DepartureDate`,`poarticle`.`SalePrice` AS `SalePrice`,`quote`.`Currency` AS `Currency` from ((`articlestopurchase` `poarticle` join `quotesview` `quote` on((`quote`.`Id` = `poarticle`.`QuoteId`))) join `articlesview` `article` on((`article`.`Id` = `quote`.`ArticleId`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `articlesview`
--

/*!50001 DROP VIEW IF EXISTS `articlesview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
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
-- Final view structure for view `historyview`
--

/*!50001 DROP VIEW IF EXISTS `historyview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `historyview` AS select `l`.`Id` AS `Id`,`l`.`Action` AS `Action`,`l`.`TableName` AS `TableName`,`l`.`UserId` AS `UserId`,`userinfo`.`UserName` AS `UserName`,`userinfo`.`UserRoleId` AS `UserRoleId`,`userinfo`.`UserRoleName` AS `UserRoleName`,`userinfo`.`ShortName` AS `PersonShortName`,`userinfo`.`FullName` AS `PersonFullName`,`l`.`TimeOfAction` AS `TimeOfAction` from (`logs` `l` join `userpersonalinformationview` `userinfo` on((`l`.`UserId` = `userinfo`.`Id`))) where (`l`.`Succeeded` = 1) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `projectproductsview`
--

/*!50001 DROP VIEW IF EXISTS `projectproductsview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `projectproductsview` AS select `pproduct`.`ProductId` AS `ProductId`,`pproduct`.`ProjectId` AS `ProjectId`,`pproduct`.`SalePrice` AS `SalePrice`,`pproduct`.`Quantity` AS `Quantity`,`pproduct`.`Observations` AS `Observations`,`project`.`ProjectNumber` AS `ProjectNumber`,`mcproduct`.`Type` AS `ProductType`,`mcproduct`.`Description` AS `ProductDescription`,`mcproduct`.`Code` AS `ProductCode`,`mcproduct`.`Model` AS `ProductModel` from ((`projectproducts` `pproduct` join `projects` `project` on((`project`.`Id` = `pproduct`.`ProjectId`))) join `mcproducts` `mcproduct` on((`mcproduct`.`Id` = `pproduct`.`ProductId`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `projectsview`
--

/*!50001 DROP VIEW IF EXISTS `projectsview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `projectsview` AS select `project`.`Id` AS `Id`,`project`.`ClientId` AS `ClientId`,`project`.`Type` AS `Type`,`project`.`Description` AS `Description`,`project`.`Discount` AS `Discount`,`project`.`AdmissionDate` AS `AdmissionDate`,`project`.`CommitmentDate` AS `CommitmentDate`,`project`.`DeliveryDate` AS `DeliveryDate`,`project`.`RealDeliveryDate` AS `RealDeliveryDate`,`project`.`DeliveryTime` AS `DeliveryTime`,`project`.`AgreedCurrency` AS `AgreedCurrency`,`project`.`PaymentCurrency` AS `PaymentCurrency`,`project`.`PaymentConditions` AS `PaymentConditions`,`project`.`SalesPerson` AS `SalesPerson`,`project`.`Comision` AS `Comision`,`project`.`HasTaxes` AS `HasTaxes`,`project`.`Observations` AS `Observations`,`project`.`Code` AS `Code`,`project`.`ProjectNumber` AS `ProjectNumber`,`client`.`MarketName` AS `ClientMarketName`,`client`.`LegalName` AS `ClientLegalName` from (`projects` `project` join `clients` `client` on((`client`.`Id` = `project`.`ClientId`))) */;
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
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
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
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `quotesview` AS select `quote`.`Id` AS `Id`,`quote`.`ProviderId` AS `ProviderId`,`provider`.`MarketName` AS `ProviderMarketName`,`provider`.`LegalName` AS `ProviderLegalName`,`quote`.`ArticleId` AS `ArticleId`,`article`.`Name` AS `ArticleName`,`article`.`GroupId` AS `GroupId`,`article`.`GroupName` AS `GroupName`,`article`.`FamilyId` AS `FamilyId`,`article`.`FamilyName` AS `FamilyName`,`article`.`Code` AS `ArticleCode`,`article`.`MustQuoteDaily` AS `MustQuoteDaily`,`quote`.`SKU` AS `SKU`,`quote`.`Price` AS `Price`,`quote`.`Currency` AS `Currency`,`quote`.`DateUpdated` AS `DateUpdated`,`provider`.`Discount` AS `ProviderDiscount` from ((`quotes` `quote` join `articlesview` `article` on((`quote`.`ArticleId` = `article`.`Id`))) join `providers` `provider` on((`quote`.`ProviderId` = `provider`.`Id`))) */;
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
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `requisitionarticlesview` AS select `reqarticles`.`RequisitionId` AS `RequisitionId`,`requisition`.`RequisitionNumber` AS `RequisitionNumber`,`reqarticles`.`ArticleId` AS `ArticleId`,`article`.`Name` AS `ArticleName`,`article`.`GroupId` AS `GroupId`,`article`.`GroupName` AS `GroupName`,`article`.`FamilyId` AS `FamilyId`,`article`.`FamilyName` AS `FamilyName`,`article`.`Code` AS `ArticleCode`,`reqarticles`.`ProjectId` AS `ProjectId`,`project`.`ProjectNumber` AS `ProjectNumber`,`reqarticles`.`Quantity` AS `Quantity`,`requisition`.`RequiredDate` AS `RequiredDate` from (((`requisitionarticles` `reqarticles` join `requisitions` `requisition` on((`requisition`.`Id` = `reqarticles`.`RequisitionId`))) join `articlesview` `article` on((`article`.`Id` = `reqarticles`.`ArticleId`))) join `projects` `project` on((`project`.`Id` = `reqarticles`.`ProjectId`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `requisitionsview`
--

/*!50001 DROP VIEW IF EXISTS `requisitionsview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `requisitionsview` AS select `requisition`.`Id` AS `Id`,`requisition`.`RequisitionNumber` AS `RequisitionNumber`,`requisition`.`RequiredDate` AS `RequiredDate`,`requisition`.`IssuedDate` AS `IssuedDate`,`requisition`.`UserId` AS `UserId`,`user`.`UserName` AS `UserName`,`user`.`ShortName` AS `UserShortName` from (`requisitions` `requisition` join `userpersonalinformationview` `user` on((`user`.`Id` = `requisition`.`UserId`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `userpersonalinformationview`
--

/*!50001 DROP VIEW IF EXISTS `userpersonalinformationview`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `userpersonalinformationview` AS select `aspu`.`Id` AS `Id`,`aspu`.`UserName` AS `UserName`,`aspu`.`Email` AS `Email`,`aspu`.`EmailConfirmed` AS `Active`,`aspr`.`Id` AS `UserRoleId`,`aspr`.`Name` AS `UserRoleName`,concat(`pi`.`FirstName`,' ',`pi`.`FatherSurname`) AS `ShortName`,(case when (isnull(`pi`.`MiddleName`) and isnull(`pi`.`MotherSurname`)) then concat(`pi`.`FirstName`,' ',`pi`.`FatherSurname`) when (isnull(`pi`.`MiddleName`) and (`pi`.`MotherSurname` is not null)) then concat(`pi`.`FirstName`,' ',`pi`.`FatherSurname`,' ',`pi`.`MotherSurname`) when ((`pi`.`MiddleName` is not null) and isnull(`pi`.`MotherSurname`)) then concat(`pi`.`FirstName`,' ',`pi`.`MiddleName`,' ',`pi`.`FatherSurname`) else concat(`pi`.`FirstName`,' ',`pi`.`MiddleName`,' ',`pi`.`FatherSurname`,' ',`pi`.`MotherSurname`) end) AS `FullName`,`pi`.`BirthDate` AS `BirthDate`,`pi`.`Gender` AS `Gender` from (((`aspnetusers` `aspu` join `aspnetuserroles` `aspur` on((`aspur`.`UserId` = `aspu`.`Id`))) join `aspnetroles` `aspr` on((`aspur`.`RoleId` = `aspr`.`Id`))) left join `personinformation` `pi` on((`pi`.`AspNetUserId` = `aspu`.`Id`))) where (`pi`.`AspNetUserId` is not null) */;
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

-- Dump completed on 2022-07-28 16:03:24
