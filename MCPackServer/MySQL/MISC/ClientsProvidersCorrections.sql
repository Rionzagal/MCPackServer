ALTER TABLE `mcpackdb`.`providers` 
ADD COLUMN `RFC` VARCHAR(20) NULL AFTER `Website`;

ALTER TABLE `mcpackdb`.`clients` 
ADD COLUMN `RFC` VARCHAR(20) NULL AFTER `Website`;

ALTER TABLE `mcpackdb`.`contacts`
MODIFY COLUMN `MobilePhone` VARCHAR(40);