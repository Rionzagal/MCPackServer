CREATE 
    ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `userpersonalinformationview` AS
    SELECT 
        `aspu`.`Id` AS `Id`,
        `aspu`.`UserName` AS `UserName`,
        `aspu`.`Email` AS `Email`,
        `aspu`.`EmailConfirmed` AS `Active`,
        `aspr`.`Id` AS `UserRoleId`,
        `aspr`.`Name` AS `UserRoleName`,
        CONCAT(`pi`.`FirstName`,
                ' ',
                `pi`.`FatherSurname`) AS `ShortName`,
        (CASE
            WHEN
                ((`pi`.`MiddleName` IS NULL)
                    AND (`pi`.`MotherSurname` IS NULL))
            THEN
                CONCAT(`pi`.`FirstName`,
                        ' ',
                        `pi`.`FatherSurname`)
            WHEN
                ((`pi`.`MiddleName` IS NULL)
                    AND (`pi`.`MotherSurname` IS NOT NULL))
            THEN
                CONCAT(`pi`.`FirstName`,
                        ' ',
                        `pi`.`FatherSurname`,
                        ' ',
                        `pi`.`MotherSurname`)
            WHEN
                ((`pi`.`MiddleName` IS NOT NULL)
                    AND (`pi`.`MotherSurname` IS NULL))
            THEN
                CONCAT(`pi`.`FirstName`,
                        ' ',
                        `pi`.`MiddleName`,
                        ' ',
                        `pi`.`FatherSurname`)
            ELSE CONCAT(`pi`.`FirstName`,
                    ' ',
                    `pi`.`MiddleName`,
                    ' ',
                    `pi`.`FatherSurname`,
                    ' ',
                    `pi`.`MotherSurname`)
        END) AS `FullName`,
        `pi`.`BirthDate` AS `BirthDate`,
        `pi`.`Gender` AS `Gender`
    FROM
        (((`aspnetusers` `aspu`
        JOIN `aspnetuserroles` `aspur` ON ((`aspur`.`UserId` = `aspu`.`Id`)))
        JOIN `aspnetroles` `aspr` ON ((`aspur`.`RoleId` = `aspr`.`Id`)))
        LEFT JOIN `personinformation` `pi` ON ((`pi`.`AspNetUserId` = `aspu`.`Id`)))
    WHERE
        (`pi`.`AspNetUserId` IS NOT NULL)