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
