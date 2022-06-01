CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `mcpackdb`.`requisitionsview` AS
    SELECT 
        `requisition`.`Id` AS `Id`,
        `requisition`.`RequisitionNumber` AS `RequisitionNumber`,
        `requisition`.`RequiredDate` AS `RequiredDate`,
        `requisition`.`IssuedDate` AS `IssuedDate`,
        `requisition`.`UserId` AS `UserId`,
        `mcpackdb`.`user`.`UserName` AS `UserName`,
        `mcpackdb`.`user`.`ShortName` AS `UserShortName`
    FROM
        (`mcpackdb`.`requisitions` `requisition`
        JOIN `mcpackdb`.`userinformationview` `user` ON ((`mcpackdb`.`user`.`Id` = `requisition`.`UserId`)))