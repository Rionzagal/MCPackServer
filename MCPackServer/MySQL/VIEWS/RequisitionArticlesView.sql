CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `mcpackdb`.`requisitionarticlesview` AS
    SELECT 
        `reqarticles`.`RequisitionId` AS `RequisitionId`,
        `requisition`.`RequisitionNumber` AS `RequisitionNumber`,
        `reqarticles`.`ArticleId` AS `ArticleId`,
        `mcpackdb`.`article`.`Name` AS `ArticleName`,
        `mcpackdb`.`article`.`GroupId` AS `GroupId`,
        `mcpackdb`.`article`.`GroupName` AS `GroupName`,
        `mcpackdb`.`article`.`FamilyId` AS `FamilyId`,
        `mcpackdb`.`article`.`FamilyName` AS `FamilyName`,
        `mcpackdb`.`article`.`Code` AS `ArticleCode`,
        `reqarticles`.`ProjectId` AS `ProjectId`,
        `project`.`ProjectNumber` AS `ProjectNumber`,
        `reqarticles`.`Quantity` AS `Quantity`,
        `requisition`.`RequiredDate` AS `RequiredDate`
    FROM
        (((`mcpackdb`.`requisitionarticles` `reqarticles`
        JOIN `mcpackdb`.`requisitions` `requisition` ON ((`requisition`.`Id` = `reqarticles`.`RequisitionId`)))
        JOIN `mcpackdb`.`articlesview` `article` ON ((`mcpackdb`.`article`.`Id` = `reqarticles`.`ArticleId`)))
        JOIN `mcpackdb`.`projects` `project` ON ((`project`.`Id` = `reqarticles`.`ProjectId`)))