CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `mcpackdb`.`projectproductsview` AS
    SELECT 
        `pproduct`.`ProductId` AS `ProductId`,
        `pproduct`.`ProjectId` AS `ProjectId`,
        `pproduct`.`SalePrice` AS `SalePrice`,
        `pproduct`.`Quantity` AS `Quantity`,
        `pproduct`.`Observations` AS `Observations`,
        `project`.`ProjectNumber` AS `ProjectNumber`,
        `mcproduct`.`Type` AS `ProductType`,
        `mcproduct`.`Description` AS `ProductDescription`,
        `mcproduct`.`Code` AS `ProductCode`,
        `mcproduct`.`Model` AS `ProductModel`
    FROM
        ((`mcpackdb`.`projectproducts` `pproduct`
        JOIN `mcpackdb`.`projects` `project` ON ((`project`.`Id` = `pproduct`.`ProjectId`)))
        JOIN `mcpackdb`.`mcproducts` `mcproduct` ON ((`mcproduct`.`Id` = `pproduct`.`ProductId`)))