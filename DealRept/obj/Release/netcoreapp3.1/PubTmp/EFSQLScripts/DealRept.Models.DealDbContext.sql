CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `Banks` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(200) NOT NULL,
        `Code` char(6) NOT NULL,
        PRIMARY KEY (`ID`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `Cities` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(300) NOT NULL,
        PRIMARY KEY (`ID`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `ContractStatuses` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(20) NOT NULL,
        PRIMARY KEY (`ID`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `Specializations` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(255) NOT NULL,
        PRIMARY KEY (`ID`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `Branches` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(200) NOT NULL,
        `StreetBuilding` varchar(100) NOT NULL,
        `PostalIndex` char(5) NOT NULL,
        `HeadFirstName` varchar(50) NOT NULL,
        `HeadLastName` varchar(50) NOT NULL,
        `HeadMiddleName` varchar(50) NOT NULL,
        `HeadTelephone` varchar(16) NOT NULL,
        `HeadEmail` varchar(100) NOT NULL,
        `CityID` int NOT NULL,
        PRIMARY KEY (`ID`),
        CONSTRAINT `FK_Branches_Cities_CityID` FOREIGN KEY (`CityID`) REFERENCES `Cities` (`ID`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `Suppliers` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(200) NOT NULL,
        `StreetBuilding` varchar(100) NOT NULL,
        `PostalIndex` char(5) NOT NULL,
        `LegalCode` varchar(20) NOT NULL,
        `BankAccount` varchar(29) NOT NULL,
        `CompanyTelephone` varchar(16) NOT NULL,
        `CompanyEmail` varchar(100) NOT NULL,
        `ContactFirstName` varchar(50) NULL,
        `ContactLastName` varchar(50) NULL,
        `ContactMiddleName` varchar(50) NULL,
        `ContactTelephone` varchar(16) NULL,
        `ContactEmail` varchar(100) NULL,
        `DirectorFirstName` varchar(50) NOT NULL,
        `DirectorLastName` varchar(50) NOT NULL,
        `DirectorMiddleName` varchar(50) NOT NULL,
        `NegativeRemarks` text NULL,
        `CityID` int NOT NULL,
        `BankID` int NOT NULL,
        `SpecializationID` int NOT NULL,
        PRIMARY KEY (`ID`),
        CONSTRAINT `FK_Suppliers_Banks_BankID` FOREIGN KEY (`BankID`) REFERENCES `Banks` (`ID`),
        CONSTRAINT `FK_Suppliers_Cities_CityID` FOREIGN KEY (`CityID`) REFERENCES `Cities` (`ID`),
        CONSTRAINT `FK_Suppliers_Specializations_SpecializationID` FOREIGN KEY (`SpecializationID`) REFERENCES `Specializations` (`ID`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `CurrentContracts` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `ContractNumber` varchar(20) NOT NULL,
        `ConclusionDate` Date NOT NULL,
        `TerminationDate` Date NOT NULL,
        `Subject` Text NOT NULL,
        `ContractAmount` Decimal(10,2) NOT NULL,
        `IsProlonged` tinyint(1) NOT NULL DEFAULT FALSE,
        `IsGoods` tinyint(1) NOT NULL,
        `Remarks` text NULL,
        `SupplierID` int NOT NULL,
        `BranchID` int NOT NULL,
        `ContractStatusID` int NOT NULL,
        PRIMARY KEY (`ID`),
        CONSTRAINT `FK_CurrentContracts_Branches_BranchID` FOREIGN KEY (`BranchID`) REFERENCES `Branches` (`ID`),
        CONSTRAINT `FK_CurrentContracts_ContactStatuses_ContractStatusID` FOREIGN KEY (`ContractStatusID`) REFERENCES `ContractStatuses` (`ID`),
        CONSTRAINT `FK_CurrentContracts_Suppliers_SupplierID` FOREIGN KEY (`SupplierID`) REFERENCES `Suppliers` (`ID`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `PastContracts` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `ContractNumber` varchar(20) NOT NULL,
        `ConclusionDate` Date NOT NULL,
        `TerminationDate` Date NOT NULL,
        `TransitionDate` Date NOT NULL,
        `Subject` Text NOT NULL,
        `ContractAmount` Decimal(10,2) NOT NULL,
        `IsProlonged` tinyint(1) NOT NULL,
        `IsGoods` tinyint(1) NOT NULL,
        `Remarks` text NULL,
        `SupplierID` int NOT NULL,
        `BranchID` int NOT NULL,
        `ContractStatusID` int NOT NULL,
        PRIMARY KEY (`ID`),
        CONSTRAINT `FK_PastContracts_Branches_BranchID` FOREIGN KEY (`BranchID`) REFERENCES `Branches` (`ID`),
        CONSTRAINT `FK_PastContracts_ContactStatuses_ContractStatusID` FOREIGN KEY (`ContractStatusID`) REFERENCES `ContractStatuses` (`ID`),
        CONSTRAINT `FK_PastContracts_Suppliers_SupplierID` FOREIGN KEY (`SupplierID`) REFERENCES `Suppliers` (`ID`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `CurrentDocuments` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(200) NOT NULL,
        `PathToDocument` varchar(255) NOT NULL,
        `DateOfUploading` Date NOT NULL,
        `CurrentContractID` int NOT NULL,
        PRIMARY KEY (`ID`),
        CONSTRAINT `FK_CurrentDocuments_CurrentContracts_CurrentContractID` FOREIGN KEY (`CurrentContractID`) REFERENCES `CurrentContracts` (`ID`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE TABLE `PastDocuments` (
        `ID` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(200) NOT NULL,
        `PathToDocument` varchar(255) NOT NULL,
        `DateOfUploading` Date NOT NULL,
        `PastContractID` int NOT NULL,
        PRIMARY KEY (`ID`),
        CONSTRAINT `FK_PastDocuments_PastContracts_PastContractID` FOREIGN KEY (`PastContractID`) REFERENCES `PastContracts` (`ID`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_Branches_CityID` ON `Branches` (`CityID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_CurrentContracts_BranchID` ON `CurrentContracts` (`BranchID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_CurrentContracts_ContractStatusID` ON `CurrentContracts` (`ContractStatusID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_CurrentContracts_SupplierID` ON `CurrentContracts` (`SupplierID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_CurrentDocuments_CurrentContractID` ON `CurrentDocuments` (`CurrentContractID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_PastContracts_BranchID` ON `PastContracts` (`BranchID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_PastContracts_ContractStatusID` ON `PastContracts` (`ContractStatusID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_PastContracts_SupplierID` ON `PastContracts` (`SupplierID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_PastDocuments_PastContractID` ON `PastDocuments` (`PastContractID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_Suppliers_BankID` ON `Suppliers` (`BankID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_Suppliers_CityID` ON `Suppliers` (`CityID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    CREATE INDEX `IX_Suppliers_SpecializationID` ON `Suppliers` (`SpecializationID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230107190044_Initial')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230107190044_Initial', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230110142548_BankAccountToChar')
BEGIN
    ALTER TABLE `Suppliers` MODIFY `BankAccount` char(29) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230110142548_BankAccountToChar')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230110142548_BankAccountToChar', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230110151513_FixTypoContractStatus')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230110151513_FixTypoContractStatus', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230110182918_AddniqeUIndex')
BEGIN
    ALTER TABLE `Suppliers` ADD CONSTRAINT `AK_Suppliers_LegalCode` UNIQUE (`LegalCode`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230110182918_AddniqeUIndex')
BEGIN
    ALTER TABLE `PastContracts` ADD CONSTRAINT `AK_PastContracts_ContractNumber_ConclusionDate` UNIQUE (`ContractNumber`, `ConclusionDate`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230110182918_AddniqeUIndex')
BEGIN
    ALTER TABLE `CurrentContracts` ADD CONSTRAINT `AK_CurrentContracts_ContractNumber_ConclusionDate` UNIQUE (`ContractNumber`, `ConclusionDate`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230110182918_AddniqeUIndex')
BEGIN
    ALTER TABLE `Branches` ADD CONSTRAINT `AK_Branches_Name` UNIQUE (`Name`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230110182918_AddniqeUIndex')
BEGIN
    ALTER TABLE `Banks` ADD CONSTRAINT `AK_Banks_Code` UNIQUE (`Code`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230110182918_AddniqeUIndex')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230110182918_AddniqeUIndex', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230112190855_DisplayFormatFix')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230112190855_DisplayFormatFix', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230128162156_addTransitionDateToCurrentContacts')
BEGIN
    ALTER TABLE `CurrentContracts` ADD `TransitionDate` Date NOT NULL DEFAULT '0001-01-01 00:00:00.000000';
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230128162156_addTransitionDateToCurrentContacts')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230128162156_addTransitionDateToCurrentContacts', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230128165113_nullableTransitioDate')
BEGIN
    ALTER TABLE `CurrentContracts` MODIFY `TransitionDate` Date NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230128165113_nullableTransitioDate')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230128165113_nullableTransitioDate', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230129080534_ImplementDocumentsInheretence')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230129080534_ImplementDocumentsInheretence', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230129093603_AddAllDocumentsToContract')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230129093603_AddAllDocumentsToContract', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230129181640_DeleteTransitionDateFromCurrentCon')
BEGIN
    ALTER TABLE `CurrentContracts` DROP COLUMN `TransitionDate`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230129181640_DeleteTransitionDateFromCurrentCon')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230129181640_DeleteTransitionDateFromCurrentCon', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230204170539_AdBranchCode')
BEGIN
    ALTER TABLE `Branches` DROP CONSTRAINT `AK_Branches_Name`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230204170539_AdBranchCode')
BEGIN
    ALTER TABLE `Branches` ADD `Code` varchar(5) NOT NULL DEFAULT '';
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230204170539_AdBranchCode')
BEGIN
    ALTER TABLE `Branches` ADD CONSTRAINT `AK_Branches_Code` UNIQUE (`Code`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230204170539_AdBranchCode')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230204170539_AdBranchCode', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230205160604_AddConstrainForDate')
BEGIN
    ALTER TABLE `CurrentContracts` ADD CONSTRAINT `CHK_TermDate` CHECK (TerminationDate > ConclusionDate);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230205160604_AddConstrainForDate')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230205160604_AddConstrainForDate', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230209180717_AddUniqueDocumentName')
BEGIN
    ALTER TABLE `CurrentDocuments` ADD CONSTRAINT `AK_CurrentDocuments_Name` UNIQUE (`Name`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230209180717_AddUniqueDocumentName')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230209180717_AddUniqueDocumentName', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230210163742_AddUniqueKeyDocumument')
BEGIN
    ALTER TABLE `CurrentDocuments` DROP CONSTRAINT `AK_CurrentDocuments_Name`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230210163742_AddUniqueKeyDocumument')
BEGIN
    ALTER TABLE `CurrentDocuments` ADD CONSTRAINT `AK_CurrentDocuments_Name_CurrentContractID` UNIQUE (`Name`, `CurrentContractID`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230210163742_AddUniqueKeyDocumument')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230210163742_AddUniqueKeyDocumument', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230216173645_AddPropertyCourtDispute')
BEGIN
    ALTER TABLE `PastContracts` ADD `IsCourtDispute` tinyint(1) NOT NULL DEFAULT FALSE;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230216173645_AddPropertyCourtDispute')
BEGIN
    ALTER TABLE `CurrentContracts` ADD `IsCourtDispute` tinyint(1) NOT NULL DEFAULT FALSE;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230216173645_AddPropertyCourtDispute')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230216173645_AddPropertyCourtDispute', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230220080921_SupplierDataLengths')
BEGIN
    ALTER TABLE `Suppliers` MODIFY `LegalCode` varchar(10) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230220080921_SupplierDataLengths')
BEGIN
    ALTER TABLE `Suppliers` MODIFY `ContactTelephone` varchar(13) NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230220080921_SupplierDataLengths')
BEGIN
    ALTER TABLE `Suppliers` MODIFY `CompanyTelephone` varchar(13) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230220080921_SupplierDataLengths')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230220080921_SupplierDataLengths', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230224095253_AddCollumnsToBranches')
BEGIN
    ALTER TABLE `Branches` MODIFY `HeadTelephone` varchar(13) NOT NULL;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230224095253_AddCollumnsToBranches')
BEGIN
    ALTER TABLE `Branches` ADD `BranchEmail` varchar(100) NOT NULL DEFAULT '';
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230224095253_AddCollumnsToBranches')
BEGIN
    ALTER TABLE `Branches` ADD `BranchTelephone` varchar(13) NOT NULL DEFAULT '';
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230224095253_AddCollumnsToBranches')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230224095253_AddCollumnsToBranches', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230309175102_CheckForDiffs')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230309175102_CheckForDiffs', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230419113921_check')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230419113921_check', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230509093656_checkForChange')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230509093656_checkForChange', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230524090240_leggalToPostalAddressInBranches')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230524090240_leggalToPostalAddressInBranches', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230604175413_AddedCheckConstrainTransDate')
BEGIN
    ALTER TABLE `PastContracts` ADD CONSTRAINT `CHK_TransDate` CHECK (TransitionDate > ConclusionDate);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230604175413_AddedCheckConstrainTransDate')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230604175413_AddedCheckConstrainTransDate', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230608170017_changedPastContactTransDateConstaraint')
BEGIN
    ALTER TABLE `PastContracts` DROP CONSTRAINT `CHK_TransDate`;
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230608170017_changedPastContactTransDateConstaraint')
BEGIN
    ALTER TABLE `PastContracts` ADD CONSTRAINT `CHK_TransDate` CHECK (TransitionDate >= ConclusionDate);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230608170017_changedPastContactTransDateConstaraint')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230608170017_changedPastContactTransDateConstaraint', '5.0.17');
END;

COMMIT;

