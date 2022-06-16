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
        `userview`.`ShortName` AS `ShortName`,
        `userview`.`FullName` AS `FullName`,
        `userview`.`UserName` AS `UserName`,
        `l`.`TimeOfAction` AS `TimeOfAction`
    FROM
        (`logs` `l`
        JOIN `userinformationview` `userview` ON ((`userview`.`Id` = `l`.`UserId`)))