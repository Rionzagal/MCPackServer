CREATE TABLE `personinformation` (
  `AspNetUserId` varchar(450) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `FirstName` varchar(50) NOT NULL,
  `MiddleName` varchar(50) DEFAULT '',
  `FatherSurname` varchar(50) NOT NULL,
  `MotherSurname` varchar(50) NOT NULL,
  `BirthDate` datetime(3) DEFAULT NULL,
  `Gender` varchar(50) DEFAULT NULL,
  `Id` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
