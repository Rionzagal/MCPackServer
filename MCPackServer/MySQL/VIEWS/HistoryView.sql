CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `historyview` AS
    SELECT 
        `l`.`Id` AS `Id`,
        `l`.`Action` AS `Action`,
        `l`.`TableName` AS `TableName`,
        `l`.`UserId` AS `UserId`,
        `userinfo`.`UserName` AS `UserName`,
        `userinfo`.`UserRoleId` AS `UserRoleId`,
        `userinfo`.`UserRoleName` AS `UserRoleName`,
        `userinfo`.`ShortName` AS `PersonShortName`,
        `userinfo`.`FullName` AS `PersonFullName`,
        `l`.`TimeOfAction` AS `TimeOfAction`
    FROM
        (`logs` `l`
        JOIN `userpersonalinformationview` `userinfo` ON ((`l`.`UserId` = `userinfo`.`Id`)))
    WHERE
        (`l`.`Succeeded` = 1)