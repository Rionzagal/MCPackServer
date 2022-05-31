CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `mcpackdb`.`purchaseordersview` AS
    SELECT 
        `po`.`Id` AS `Id`,
        `po`.`IssuedDate` AS `IssuedDate`,
        `po`.`DeliveryDate` AS `DeliveryDate`,
        `po`.`Currency` AS `Currency`,
        `po`.`Discount` AS `Discount`,
        `po`.`Status` AS `Status`,
        `po`.`ReceptionDate` AS `ReceptionDate`,
        `po`.`InvoiceNumber` AS `InvoiceNumber`,
        `po`.`Observations` AS `Observations`,
        `po`.`OrderNumber` AS `OrderNumber`,
        `po`.`ProviderId` AS `ProviderId`,
        `provider`.`LegalName` AS `ProviderLegalName`,
        `provider`.`HasTaxes` AS `HasTaxes`,
        `po`.`ProjectId` AS `ProjectId`,
        `project`.`ProjectNumber` AS `ProjectNumber`,
        `project`.`ClientId` AS `ClientId`,
        `client`.`MarketName` AS `ClientMarketName`,
        `po`.`RequisitionId` AS `RequisitionId`,
        IF((NULL <> `po`.`RequisitionId`),
            `requisition`.`RequisitionNumber`,
            'N/A') AS `RequisitionNumber`
    FROM
        ((((`mcpackdb`.`purchaseorders` `po`
        JOIN `mcpackdb`.`providers` `provider` ON ((`po`.`ProviderId` = `provider`.`Id`)))
        JOIN `mcpackdb`.`projects` `project` ON ((`po`.`ProjectId` = `project`.`Id`)))
        JOIN `mcpackdb`.`clients` `client` ON ((`project`.`ClientId` = `client`.`Id`)))
        LEFT JOIN `mcpackdb`.`requisitions` `requisition` ON ((`po`.`RequisitionId` = `requisition`.`Id`)))