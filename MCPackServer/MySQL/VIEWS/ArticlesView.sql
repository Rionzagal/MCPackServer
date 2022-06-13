CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `articlesview` AS
    SELECT 
        `article`.`Id` AS `Id`,
        `article`.`Name` AS `Name`,
        `article`.`Description` AS `Description`,
        `article`.`Unit` AS `Unit`,
        `article`.`TradeMark` AS `TradeMark`,
        `article`.`Model` AS `Model`,
        `article`.`FamilyId` AS `FamilyId`,
        `family`.`Name` AS `FamilyName`,
        `family`.`GroupId` AS `GroupId`,
        `group`.`Name` AS `GroupName`,
        CONCAT(`group`.`Code`,
                '-',
                `family`.`Code`,
                '-',
                `article`.`Code`) AS `Code`,
        `group`.`HasVariablePrice` AS `MustQuoteDaily`,
        `article`.`Observations` AS `Observations`
    FROM
        ((`purchasearticles` `article`
        JOIN `articlefamilies` `family` ON ((`article`.`FamilyId` = `family`.`Id`)))
        JOIN `articlegroups` `group` ON ((`family`.`GroupId` = `group`.`Id`)))