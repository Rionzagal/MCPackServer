CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `mcpackdb`.`articlestopurchaseview` AS
    SELECT 
        `poarticle`.`QuoteId` AS `QuoteId`,
        `mcpackdb`.`quote`.`GroupId` AS `GroupId`,
        `mcpackdb`.`quote`.`GroupName` AS `GroupName`,
        `mcpackdb`.`quote`.`FamilyId` AS `FamilyId`,
        `mcpackdb`.`quote`.`FamilyName` AS `FamilyName`,
        `mcpackdb`.`quote`.`ArticleId` AS `ArticleId`,
        `mcpackdb`.`quote`.`ArticleName` AS `ArticleName`,
        `mcpackdb`.`quote`.`ArticleCode` AS `ArticleCode`,
        `mcpackdb`.`article`.`TradeMark` AS `TradeMark`,
        `mcpackdb`.`article`.`Model` AS `Model`,
        `mcpackdb`.`quote`.`SKU` AS `SKU`,
        `mcpackdb`.`article`.`Unit` AS `Unit`,
        `mcpackdb`.`quote`.`DateUpdated` AS `DateUpdated`,
        `poarticle`.`PurchaseOrderId` AS `PurchaseOrderId`,
        `poarticle`.`Quantity` AS `Quantity`,
        `poarticle`.`EntryDate` AS `EntryDate`,
        `poarticle`.`DepartureDate` AS `DepartureDate`,
        `poarticle`.`SalePrice` AS `SalePrice`,
        `mcpackdb`.`quote`.`Currency` AS `Currency`
    FROM
        ((`mcpackdb`.`articlestopurchase` `poarticle`
        JOIN `mcpackdb`.`quotesview` `quote` ON ((`mcpackdb`.`quote`.`Id` = `poarticle`.`QuoteId`)))
        JOIN `mcpackdb`.`articlesview` `article` ON ((`mcpackdb`.`article`.`Id` = `mcpackdb`.`quote`.`ArticleId`)))