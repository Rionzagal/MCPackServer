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
  CONSTRAINT `FK_Logs_AspNetUsers` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
