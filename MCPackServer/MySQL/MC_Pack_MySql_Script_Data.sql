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
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `articlefamilies`
--

LOCK TABLES `articlefamilies` WRITE;
/*!40000 ALTER TABLE `articlefamilies` DISABLE KEYS */;
INSERT INTO `articlefamilies` VALUES (2,'INOXIDABLE','ANGULO, CUADRADO, HEXAGONO, LAMINA, PLACA, PTR, REDONDO, SOLERA, TUBING, TUBO','IN',3),(3,'PANTALLAS','PANTALLAS','PA',4);
/*!40000 ALTER TABLE `articlefamilies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `articlegroups`
--

LOCK TABLES `articlegroups` WRITE;
/*!40000 ALTER TABLE `articlegroups` DISABLE KEYS */;
INSERT INTO `articlegroups` VALUES (3,'ACEROS Y METALES','TODO TIPO DE ACEROS Y METALES','ME',1),(4,'ELECTRICO','MATERIAL ELECTRICO Y ELECTRONICO','EL',0);
/*!40000 ALTER TABLE `articlegroups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `articlestopurchase`
--

LOCK TABLES `articlestopurchase` WRITE;
/*!40000 ALTER TABLE `articlestopurchase` DISABLE KEYS */;
INSERT INTO `articlestopurchase` VALUES (1,2,0,NULL,NULL,300);
/*!40000 ALTER TABLE `articlestopurchase` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
INSERT INTO `aspnetroleclaims` VALUES (249,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Users'),(250,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Users.View'),(251,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Users.Create'),(252,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Users.Edit'),(253,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Users.Delete'),(254,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Roles'),(255,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Roles.View'),(256,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Roles.Create'),(257,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Roles.Edit'),(258,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Roles.Delete'),(259,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Clients'),(260,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Clients.View'),(261,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Clients.Create'),(262,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Clients.Edit'),(263,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Clients.Delete'),(264,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Contacts'),(265,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Contacts.View'),(266,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Contacts.Create'),(267,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Contacts.Edit'),(268,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Contacts.Delete'),(269,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Providers'),(270,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Providers.View'),(271,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Providers.Create'),(272,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Providers.Edit'),(273,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Providers.Delete'),(274,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Products'),(275,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Products.View'),(276,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Products.Create'),(277,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Products.Edit'),(278,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Products.Delete'),(279,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Articles'),(280,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Articles.View'),(281,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Articles.Create'),(282,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Articles.Edit'),(283,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Articles.Delete'),(284,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleFamilies.View'),(285,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleFamilies.Create'),(286,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleFamilies.Edit'),(287,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleFamilies.Delete'),(288,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleGroups.View'),(289,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleGroups.Create'),(290,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleGroups.Edit'),(291,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ArticleGroups.Delete'),(292,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Quotes.View'),(293,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Quotes.Create'),(294,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Quotes.Edit'),(295,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Quotes.Delete'),(296,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Projects'),(297,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Projects.View'),(298,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Projects.Create'),(299,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Projects.Edit'),(300,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Projects.Delete'),(301,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.PurchaseOrders'),(302,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.PurchaseOrders.View'),(303,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.PurchaseOrders.Create'),(304,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.PurchaseOrders.Edit'),(305,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.PurchaseOrders.Delete'),(306,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Menu.Requisitions'),(307,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Requisitions.View'),(308,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Requisitions.Create'),(309,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Requisitions.Edit'),(310,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Requisitions.Delete'),(311,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Users'),(312,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Users.View'),(313,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Users.Create'),(314,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Users.Edit'),(315,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Users.Delete'),(316,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Roles'),(317,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Roles.View'),(318,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Roles.Create'),(319,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Roles.Edit'),(320,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Roles.Delete'),(321,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Clients'),(322,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Clients.View'),(323,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Clients.Create'),(324,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Clients.Edit'),(325,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Clients.Delete'),(326,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Contacts'),(327,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Contacts.View'),(328,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Contacts.Create'),(329,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Contacts.Edit'),(330,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Contacts.Delete'),(331,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Providers'),(332,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Providers.View'),(333,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Providers.Create'),(334,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Providers.Edit'),(335,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Providers.Delete'),(336,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Products'),(337,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Products.View'),(338,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Products.Create'),(339,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Products.Edit'),(340,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Products.Delete'),(341,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Articles'),(342,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Articles.View'),(343,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Articles.Create'),(344,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Articles.Edit'),(345,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Articles.Delete'),(346,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleFamilies.View'),(347,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleFamilies.Create'),(348,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleFamilies.Edit'),(349,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleFamilies.Delete'),(350,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleGroups.View'),(351,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleGroups.Create'),(352,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleGroups.Edit'),(353,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ArticleGroups.Delete'),(354,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Quotes.View'),(355,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Quotes.Create'),(356,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Quotes.Edit'),(357,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Quotes.Delete'),(358,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Projects'),(359,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Projects.View'),(360,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Projects.Create'),(361,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Projects.Edit'),(362,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Projects.Delete'),(363,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.PurchaseOrders'),(364,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.PurchaseOrders.View'),(365,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.PurchaseOrders.Create'),(366,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.PurchaseOrders.Edit'),(367,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.PurchaseOrders.Delete'),(368,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Menu.Requisitions'),(369,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Requisitions.View'),(370,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Requisitions.Create'),(371,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Requisitions.Edit'),(372,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Requisitions.Delete'),(373,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Reports.View'),(374,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.Reports.Create'),(375,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.ProjectSpecial.ClientChange'),(376,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.RoleClaims.View'),(377,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.RoleClaims.Create'),(378,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.RoleClaims.Edit'),(379,'6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Permission','Permissions.RoleClaims.Delete'),(380,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Reports.View'),(381,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.Reports.Create'),(382,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.ProjectSpecial.ClientChange'),(383,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.RoleClaims.View'),(384,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.RoleClaims.Create'),(385,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.RoleClaims.Edit'),(386,'AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Permission','Permissions.RoleClaims.Delete'),(387,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Menu.Providers'),(388,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Menu.Articles'),(389,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Menu.PurchaseOrders'),(390,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Menu.Requisitions'),(391,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Contacts.Create'),(392,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Contacts.View'),(393,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Contacts.Edit'),(394,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Contacts.Delete'),(395,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Providers.Create'),(396,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Providers.View'),(397,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Providers.Edit'),(399,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Articles.Create'),(400,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Articles.View'),(401,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Articles.Edit'),(403,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleFamilies.Create'),(404,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleFamilies.View'),(405,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleFamilies.Edit'),(407,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleGroups.Create'),(408,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleGroups.View'),(409,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.ArticleGroups.Edit'),(411,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Quotes.Create'),(412,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Quotes.View'),(413,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Quotes.Edit'),(415,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.PurchaseOrders.Create'),(416,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.PurchaseOrders.View'),(417,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.PurchaseOrders.Edit'),(419,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Requisitions.Create'),(420,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Requisitions.View'),(421,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Requisitions.Edit'),(423,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Reports.View'),(424,'3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Permission','Permissions.Reports.Create'),(425,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Users'),(426,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Roles'),(427,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Clients'),(428,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Providers'),(429,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Products'),(430,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Articles'),(431,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Projects'),(432,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.PurchaseOrders'),(433,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Menu.Requisitions'),(434,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Users.View'),(435,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Roles.View'),(436,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Clients.View'),(437,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Articles.View'),(438,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Products.View'),(439,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Providers.View'),(440,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Contacts.View'),(441,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.ArticleFamilies.View'),(442,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.ArticleGroups.View'),(443,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Quotes.View'),(444,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Projects.View'),(445,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.PurchaseOrders.View'),(446,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Requisitions.View'),(447,'9a29c706-d755-421f-b15f-7a19b2e8fc20','Permission','Permissions.Reports.View'),(448,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Users'),(449,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Roles'),(450,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Clients'),(451,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Providers'),(452,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Products'),(453,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Articles'),(454,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Projects'),(455,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.PurchaseOrders'),(456,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Menu.Requisitions'),(457,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Clients.Create'),(458,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Clients.View'),(459,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Clients.Edit'),(460,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Clients.Delete'),(461,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Contacts.Create'),(462,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Contacts.View'),(463,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Contacts.Edit'),(464,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Contacts.Delete'),(465,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Products.Create'),(466,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Products.View'),(467,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Products.Edit'),(468,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Products.Delete'),(469,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Projects.Create'),(470,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Projects.View'),(471,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Projects.Edit'),(472,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Projects.Delete'),(473,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Reports.View'),(474,'1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Permission','Permissions.Reports.Create'),(478,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.Providers'),(480,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.Articles'),(481,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.Projects'),(482,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.PurchaseOrders'),(483,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Menu.Requisitions'),(484,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Contacts.Create'),(485,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Contacts.View'),(486,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Contacts.Edit'),(487,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Contacts.Delete'),(488,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Providers.Create'),(489,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Providers.View'),(490,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Providers.Edit'),(492,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Articles.Create'),(493,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Articles.View'),(494,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Articles.Edit'),(496,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleFamilies.Create'),(497,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleFamilies.View'),(498,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleFamilies.Edit'),(500,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleGroups.Create'),(501,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleGroups.View'),(502,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.ArticleGroups.Edit'),(504,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Quotes.Create'),(505,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Quotes.View'),(506,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Quotes.Edit'),(507,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Quotes.Delete'),(508,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.PurchaseOrders.Create'),(509,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.PurchaseOrders.View'),(510,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.PurchaseOrders.Edit'),(513,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Requisitions.View'),(516,'921a17ad-d6e3-4082-84fc-0c43b4a86f8d','Permission','Permissions.Reports.View'),(520,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Clients'),(521,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Providers'),(522,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Products'),(523,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Articles'),(524,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Projects'),(525,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.PurchaseOrders'),(526,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Menu.Requisitions'),(527,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Clients.Create'),(528,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Clients.View'),(529,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Clients.Edit'),(531,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Contacts.Create'),(532,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Contacts.View'),(533,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Contacts.Edit'),(534,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Contacts.Delete'),(535,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Products.Create'),(536,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Products.View'),(537,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Products.Edit'),(540,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Providers.View'),(544,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Quotes.View'),(548,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Projects.View'),(549,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Projects.Edit'),(552,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.PurchaseOrders.View'),(555,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Requisitions.Create'),(556,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Requisitions.View'),(557,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Requisitions.Edit'),(559,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Reports.View'),(560,'35a25ec1-8631-4a65-98d6-7a6ecce83d32','Permission','Permissions.Reports.Create');
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` VALUES ('1db0ca6c-4053-4d29-b2d7-604c37b27b5f','Proyectos','PROYECTOS','663d8a53-3003-405c-a0e9-604ae8b4052a'),('35a25ec1-8631-4a65-98d6-7a6ecce83d32','Admvo','ADMVO','69100be9-7347-49c5-8e35-bccf1a8c0ff2'),('3b9abf42-31f2-4d3d-8bb7-f3764be31b31','Compras','COMPRAS','d866d593-0220-4934-8742-a8b45d84885b'),('6E4134C5-FE58-478F-A6EE-DE4A1A87CC16','Developer','DEVELOPER','de4ab0c6-a4b9-11ec-9d6a-40a8f0c78b90'),('921a17ad-d6e3-4082-84fc-0c43b4a86f8d','AuxCompras','AUXCOMPRAS','668a7bfa-d8e5-479c-87aa-c8569619d696'),('9a29c706-d755-421f-b15f-7a19b2e8fc20','Consultas','CONSULTAS','bb8feac1-28f1-4e93-b595-023f6ae37d11'),('AABE1774-BA38-4EE6-89E6-0405E1F1A6A6','Admin','ADMIN','de578703-a4b9-11ec-9d6a-40a8f0c78b90');
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` VALUES ('29467d9b-19d6-43cd-98ec-48164adf8462','1db0ca6c-4053-4d29-b2d7-604c37b27b5f'),('29467d9b-19d6-43cd-98ec-48164adf8462','35a25ec1-8631-4a65-98d6-7a6ecce83d32'),('eb8e18aa-90e6-48d1-a7a5-4f5f36552aee','3b9abf42-31f2-4d3d-8bb7-f3764be31b31'),('5518f7a4-2202-4598-8b27-a23f0864aedb','6E4134C5-FE58-478F-A6EE-DE4A1A87CC16'),('8933d248-1c5c-4ded-90c4-bd2c24abc54f','921a17ad-d6e3-4082-84fc-0c43b4a86f8d'),('81c4e175-5589-48e8-af61-b0e0218ed513','9a29c706-d755-421f-b15f-7a19b2e8fc20'),('24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','AABE1774-BA38-4EE6-89E6-0405E1F1A6A6'),('63f048db-8f22-492e-841d-298543e7a637','AABE1774-BA38-4EE6-89E6-0405E1F1A6A6');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','mario.gonzalez@mc-pack.com','MARIO.GONZALEZ@MC-PACK.COM','mario.gonzalez@mc-pack.com','MARIO.GONZALEZ@MC-PACK.COM',1,'AQAAAAEAACcQAAAAELHwgiT84OdkakvazK02mYEn2oBLgRJZZld8fkl5mYBY8yTrYiJaZviDVdNUAB1AUQ==','KG2ZNVHIR6JAO6UJ5NDT4S47SNEQ3WHQ','75d099e0-c736-404a-9e18-95c37c697125',NULL,0,0,NULL,1,0),('29467d9b-19d6-43cd-98ec-48164adf8462','administracion@mc-pack.com','ADMINISTRACION@MC-PACK.COM','administracion@mc-pack.com','ADMINISTRACION@MC-PACK.COM',1,'AQAAAAEAACcQAAAAEFil6Tim7PVE1i15A1vTx0Bpt6zI1fBOq3jIagfhSuS87eOXWum2RMsqpm+Vt73hCw==','UP5HINYJ2QF7YW557B6GRTPGMDBSSONN','9a99173f-836b-4838-b720-3cf23183ead9',NULL,0,0,NULL,1,0),('5518f7a4-2202-4598-8b27-a23f0864aedb','mario.gzz.gal@gmail.com','MARIO.GZZ.GAL@GMAIL.COM','mario.gzz.gal@gmail.com','MARIO.GZZ.GAL@GMAIL.COM',1,'AQAAAAEAACcQAAAAEIqK4MVDTb3gAz9CD6raroHQjjJHpYojg/UB4glgMjjv3zLT/sKKzrMC/Hc3F03rYw==','7ZAEQJSKINVPNY74QE3XAUUXEJJJUG6F','206dcbf9-2cd5-4f91-845f-262bd0f5bb65',NULL,0,0,NULL,1,0),('63f048db-8f22-492e-841d-298543e7a637','ec.galindo@mc-pack.com','EC.GALINDO@MC-PACK.COM','ec.galindo@mc-pack.com','EC.GALINDO@MC-PACK.COM',1,'AQAAAAEAACcQAAAAEEJqe0wgrbvwV7+AJ6fAcsDH4+9wuZp0YS6A6SyTylUUXF3x/lLo9ebLOJnTV8sKLw==','DZGCVLM5DZHTOCO5T226KR5XEYCGUR4Z','384bf82a-6b79-497d-80c8-86ff2370bc96',NULL,0,0,NULL,1,0),('81c4e175-5589-48e8-af61-b0e0218ed513','direccion@mc-pack.com','DIRECCION@MC-PACK.COM','direccion@mc-pack.com','DIRECCION@MC-PACK.COM',1,'AQAAAAEAACcQAAAAEL9zcoJyYHcA10UIuFcEKpKZ6KgTOM5lGxDD1VFqR9na38xHfXNbus7NfzZtxchmzg==','WW2PG44RLPHID6KFPKVB7DTT3FW3BWAI','256a7b08-74a8-4e98-9c08-69b8a6361c92',NULL,0,0,NULL,1,0),('8933d248-1c5c-4ded-90c4-bd2c24abc54f','auxcompras.mc@hotmail.com','AUXCOMPRAS.MC@HOTMAIL.COM','auxcompras.mc@hotmail.com','AUXCOMPRAS.MC@HOTMAIL.COM',1,'AQAAAAEAACcQAAAAEJppaksUCgpHv9MuKxFhyfWaK7AOsQUuvLt7tDnXCSJVT9o4mjKn4S0LoWqhrDc/EQ==','YTIO656XHMQOXOJOOO3WNEUDHDNXM276','efcb863c-fc08-41d2-a74a-e1100880bdc6',NULL,0,0,NULL,1,0),('eb8e18aa-90e6-48d1-a7a5-4f5f36552aee','compras@mc-pack.com','COMPRAS@MC-PACK.COM','compras@mc-pack.com','COMPRAS@MC-PACK.COM',1,'AQAAAAEAACcQAAAAEDLH/K7hjEKdY1n+L1ZJber3yJsDG1rgNfJHER0By6RsoXY68SWzKEU7cxNqlO/z4A==','H2VJGV7MMRC4N25HT3YGMGQGP4Z22YOZ','59ce6f66-fd37-49cc-bc3f-6360a3f57987',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `clientcontacts`
--

LOCK TABLES `clientcontacts` WRITE;
/*!40000 ALTER TABLE `clientcontacts` DISABLE KEYS */;
/*!40000 ALTER TABLE `clientcontacts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES (2,'ALPEZZI CHOCOLATE','ALPEZZI CHOCOLATE S.A. DE C.V.','PROLONGACION LOS ROBLES SUR 351, COL. LOS ROBLES','ZAPOPAN','JALISCO','MEXICO','45134','3330012000','30 DIAS','https://www.alpezzi.com.mx'),(3,'LUCAS - EFFEM MEXICO','EFFEM MEXICO INC Y COMPAÑIA','CARRETERA CHICHIMEQUILLAS KM 4.5','QUERETARO','QUERETARO','MEXICO','76246','4423866784','30 DIAS','www.mars.com');
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `contacts`
--

LOCK TABLES `contacts` WRITE;
/*!40000 ALTER TABLE `contacts` DISABLE KEYS */;
INSERT INTO `contacts` VALUES (16,'ELVA CECILIA GALINDO CASAS','ECGCMC@LIVE.COM','3310640143',NULL),(17,'Mario Gonzalez','mario.gzz@hotmail.com','2222222222','ventas'),(18,'otro contacto de prueba','otro.contacto@mail.com','00112233445',NULL),(19,'otro contacto','otro.contacto@mail.com','001122333',NULL),(20,'otro contacto de prueba','otro.contacto@mail.com','000000000',NULL);
/*!40000 ALTER TABLE `contacts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `devicecodes`
--

LOCK TABLES `devicecodes` WRITE;
/*!40000 ALTER TABLE `devicecodes` DISABLE KEYS */;
/*!40000 ALTER TABLE `devicecodes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `keys`
--

LOCK TABLES `keys` WRITE;
/*!40000 ALTER TABLE `keys` DISABLE KEYS */;
/*!40000 ALTER TABLE `keys` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `logs`
--

LOCK TABLES `logs` WRITE;
/*!40000 ALTER TABLE `logs` DISABLE KEYS */;
/*!40000 ALTER TABLE `logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `mcproducts`
--

LOCK TABLES `mcproducts` WRITE;
/*!40000 ALTER TABLE `mcproducts` DISABLE KEYS */;
INSERT INTO `mcproducts` VALUES (1,'GUILLOTINA DE CORTE CON TECNOLOGIA ULTRASONICA ',163600,'USD','GUILLOTINA','GUUS001','MCESGU','OPERADA POR SERVOMOTORES');
/*!40000 ALTER TABLE `mcproducts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `persistedgrants`
--

LOCK TABLES `persistedgrants` WRITE;
/*!40000 ALTER TABLE `persistedgrants` DISABLE KEYS */;
/*!40000 ALTER TABLE `persistedgrants` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `projectproducts`
--

LOCK TABLES `projectproducts` WRITE;
/*!40000 ALTER TABLE `projectproducts` DISABLE KEYS */;
INSERT INTO `projectproducts` VALUES (1,2,145000,1,NULL);
/*!40000 ALTER TABLE `projectproducts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `projects`
--

LOCK TABLES `projects` WRITE;
/*!40000 ALTER TABLE `projects` DISABLE KEYS */;
INSERT INTO `projects` VALUES (2,3,'Proyecto','GUILLOTINA DE CORTE CON TECNOLOGIA ULTRASONICA',0,'2022-01-21 00:00:00.000','2022-04-15 00:00:00.000','2022-05-31 00:00:00.000',NULL,'12 A 14 SEM','USD','USD','50, 30-10, 20-CE','MGC',0,1,'LAB EN NUESTRA PLANTA','0C4dTP','0591'),(19,3,'Refacciones','DESCRIPCION',0,'2022-02-11 00:00:00.000','2022-05-10 00:00:00.000','2022-05-10 00:00:00.000',NULL,'NO APLICA','MXN','MXN','30 DÍAS','MGC',0,1,'OBSERVACIONES','0C4dTR','0592');
/*!40000 ALTER TABLE `projects` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `providercontacts`
--

LOCK TABLES `providercontacts` WRITE;
/*!40000 ALTER TABLE `providercontacts` DISABLE KEYS */;
INSERT INTO `providercontacts` VALUES (2,16),(1,17);
/*!40000 ALTER TABLE `providercontacts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `providers`
--

LOCK TABLES `providers` WRITE;
/*!40000 ALTER TABLE `providers` DISABLE KEYS */;
INSERT INTO `providers` VALUES (1,'Proveedor de prueba','Prueba proveedor S.A. de C.V.','Domicilio fiscal','Ciudad','Provincia','pais','01234','0000000','sitio web','Tipo de pago','Condicion de pago',NULL,0,0,NULL,1),(2,'LA PALOMA','LA PALOMA COMPAÑIA DE METALES SA DE CV','CALZ GONZALEZ GALLO 1600','GUADALAJARA','JALISCO','MEXICO','44870','3333333333','www.lapaloma.com','TRANSFERENCIA','CONTADO',NULL,0,1,'EN PIEZAS DE MENOR TAMAÑO Y POR PREMURA PASAMOS A RECOGER',1);
/*!40000 ALTER TABLE `providers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `purchasearticles`
--

LOCK TABLES `purchasearticles` WRITE;
/*!40000 ALTER TABLE `purchasearticles` DISABLE KEYS */;
INSERT INTO `purchasearticles` VALUES (2,'PANTALLA 7\" WECON','PANTALLA HMI 7\" CODIGO PI3070 2 ENTRADAS','PZA','WECON','PI3070',3,'WE7001',NULL),(3,'LAMINA DE ACERO INOX CALIBRE 12 DE 5 X 10 PIES','LAMINA DE ACERO INOXIDABLE CALIBRE 12 DE 5 X 10 PIES','PZA',NULL,NULL,2,'LA12',NULL);
/*!40000 ALTER TABLE `purchasearticles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `purchaseorders`
--

LOCK TABLES `purchaseorders` WRITE;
/*!40000 ALTER TABLE `purchaseorders` DISABLE KEYS */;
INSERT INTO `purchaseorders` VALUES (2,2,'2022-05-05 17:59:10.734',2,1,'2022-05-09 00:00:00.000','USD',0,'Pendiente',NULL,NULL,NULL,NULL),(3,2,'2022-05-10 17:14:00.250',2,1,'2022-05-13 00:00:00.000','MXN',0,'Pendiente',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `purchaseorders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `quotes`
--

LOCK TABLES `quotes` WRITE;
/*!40000 ALTER TABLE `quotes` DISABLE KEYS */;
INSERT INTO `quotes` VALUES (1,2,2,300,'H505067','2022-05-04 17:57:07.123','USD');
/*!40000 ALTER TABLE `quotes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `requisitionarticles`
--

LOCK TABLES `requisitionarticles` WRITE;
/*!40000 ALTER TABLE `requisitionarticles` DISABLE KEYS */;
INSERT INTO `requisitionarticles` VALUES (1,2,2,1),(1,3,2,1);
/*!40000 ALTER TABLE `requisitionarticles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `requisitions`
--

LOCK TABLES `requisitions` WRITE;
/*!40000 ALTER TABLE `requisitions` DISABLE KEYS */;
INSERT INTO `requisitions` VALUES (1,'00001','2022-05-05 00:00:00.000','63f048db-8f22-492e-841d-298543e7a637','2022-05-09 00:00:00.000');
/*!40000 ALTER TABLE `requisitions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `systemrolepermissions`
--

LOCK TABLES `systemrolepermissions` WRITE;
/*!40000 ALTER TABLE `systemrolepermissions` DISABLE KEYS */;
/*!40000 ALTER TABLE `systemrolepermissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `userinformation`
--

LOCK TABLES `userinformation` WRITE;
/*!40000 ALTER TABLE `userinformation` DISABLE KEYS */;
INSERT INTO `userinformation` VALUES ('24ba5ec9-bbc8-447c-ae64-1c516a8e11d3','MARIO','','GONZÁLEZ','CERVANTES','1972-09-13 00:00:00.000','Masculino'),('29467d9b-19d6-43cd-98ec-48164adf8462','NUSMETH','YARENY','FLORES','CORDOVA','1988-03-02 00:00:00.000','Femenino'),('5518f7a4-2202-4598-8b27-a23f0864aedb','MARIO',' ','GONZALEZ','GALINDO','1998-07-15 00:00:00.000','Masculino'),('63f048db-8f22-492e-841d-298543e7a637','ELVA','CECILIA','GALINDO','CASAS','1972-01-13 00:00:00.000','Femenino'),('81c4e175-5589-48e8-af61-b0e0218ed513','Abigail','','Quiñonez','García','1992-07-20 00:00:00.000','Femenino'),('8933d248-1c5c-4ded-90c4-bd2c24abc54f','JULIETA','','MARTINEZ','RIZO','1999-02-14 00:00:00.000','Femenino'),('eb8e18aa-90e6-48d1-a7a5-4f5f36552aee','JESSICA','','SANTACRUZ','','1994-11-10 00:00:00.000','Femenino');
/*!40000 ALTER TABLE `userinformation` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-06-14 16:09:18
