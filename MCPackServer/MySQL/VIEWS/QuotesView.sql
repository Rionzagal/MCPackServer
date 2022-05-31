CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `mcpackdb`.`quotesview` AS
    SELECT 
        `quote`.`Id` AS `Id`,
        `quote`.`ProviderId` AS `ProviderId`,
        `provider`.`MarketName` AS `ProviderMarketName`,
        `provider`.`LegalName` AS `ProviderLegalName`,
        `quote`.`ArticleId` AS `ArticleId`,
        `mcpackdb`.`article`.`Name` AS `ArticleName`,
        `mcpackdb`.`article`.`GroupId` AS `GroupId`,
        `mcpackdb`.`article`.`GroupName` AS `GroupName`,
        `mcpackdb`.`article`.`FamilyId` AS `FamilyId`,
        `mcpackdb`.`article`.`FamilyName` AS `FamilyName`,
        `mcpackdb`.`article`.`Code` AS `ArticleCode`,
        `quote`.`SKU` AS `SKU`,
        `quote`.`Price` AS `Price`,
        `quote`.`Currency` AS `Currency`,
        `quote`.`DateUpdated` AS `DateUpdated`
    FROM
        ((`mcpackdb`.`quotes` `quote`
        JOIN `mcpackdb`.`articlesview` `article` ON ((`quote`.`ArticleId` = `mcpackdb`.`article`.`Id`)))
        JOIN `mcpackdb`.`providers` `provider` ON ((`quote`.`ProviderId` = `provider`.`Id`)))