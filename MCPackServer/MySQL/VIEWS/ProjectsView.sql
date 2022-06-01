CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `mcpackdb`.`projectsview` AS
    SELECT 
        `project`.`Id` AS `Id`,
        `project`.`ClientId` AS `ClientId`,
        `project`.`Type` AS `Type`,
        `project`.`Description` AS `Description`,
        `project`.`Discount` AS `Discount`,
        `project`.`AdmissionDate` AS `AdmissionDate`,
        `project`.`CommitmentDate` AS `CommitmentDate`,
        `project`.`DeliveryDate` AS `DeliveryDate`,
        `project`.`RealDeliveryDate` AS `RealDeliveryDate`,
        `project`.`DeliveryTime` AS `DeliveryTime`,
        `project`.`AgreedCurrency` AS `AgreedCurrency`,
        `project`.`PaymentCurrency` AS `PaymentCurrency`,
        `project`.`PaymentConditions` AS `PaymentConditions`,
        `project`.`SalesPerson` AS `SalesPerson`,
        `project`.`Comision` AS `Comision`,
        `project`.`HasTaxes` AS `HasTaxes`,
        `project`.`Observations` AS `Observations`,
        `project`.`Code` AS `Code`,
        `project`.`ProjectNumber` AS `ProjectNumber`,
        `client`.`MarketName` AS `ClientMarketName`,
        `client`.`LegalName` AS `ClientLegalName`
    FROM
        (`mcpackdb`.`projects` `project`
        JOIN `mcpackdb`.`clients` `client` ON ((`client`.`Id` = `project`.`ClientId`)))