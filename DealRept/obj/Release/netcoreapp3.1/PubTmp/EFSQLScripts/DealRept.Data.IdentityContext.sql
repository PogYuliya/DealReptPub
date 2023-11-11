CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE TABLE `AspNetRoles` (
        `Id` varchar(255) NOT NULL,
        `Name` varchar(256) NULL,
        `NormalizedName` varchar(256) NULL,
        `ConcurrencyStamp` text NULL,
        PRIMARY KEY (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE TABLE `AspNetUsers` (
        `Id` varchar(255) NOT NULL,
        `FirstName` varchar(50) NOT NULL,
        `MiddleName` varchar(50) NOT NULL,
        `LastName` varchar(50) NOT NULL,
        `EmployeeNumber` int NOT NULL,
        `IsApproved` tinyint(1) NOT NULL,
        `UserName` varchar(256) NULL,
        `NormalizedUserName` varchar(256) NULL,
        `Email` varchar(256) NULL,
        `NormalizedEmail` varchar(256) NULL,
        `EmailConfirmed` tinyint(1) NOT NULL,
        `PasswordHash` text NULL,
        `SecurityStamp` text NULL,
        `ConcurrencyStamp` text NULL,
        `PhoneNumber` text NULL,
        `PhoneNumberConfirmed` tinyint(1) NOT NULL,
        `TwoFactorEnabled` tinyint(1) NOT NULL,
        `LockoutEnd` timestamp NULL,
        `LockoutEnabled` tinyint(1) NOT NULL,
        `AccessFailedCount` int NOT NULL,
        PRIMARY KEY (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE TABLE `AspNetRoleClaims` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `RoleId` varchar(255) NOT NULL,
        `ClaimType` text NULL,
        `ClaimValue` text NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE TABLE `AspNetUserClaims` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserId` varchar(255) NOT NULL,
        `ClaimType` text NULL,
        `ClaimValue` text NULL,
        PRIMARY KEY (`Id`),
        CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE TABLE `AspNetUserLogins` (
        `LoginProvider` varchar(255) NOT NULL,
        `ProviderKey` varchar(255) NOT NULL,
        `ProviderDisplayName` text NULL,
        `UserId` varchar(255) NOT NULL,
        PRIMARY KEY (`LoginProvider`, `ProviderKey`),
        CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE TABLE `AspNetUserRoles` (
        `UserId` varchar(255) NOT NULL,
        `RoleId` varchar(255) NOT NULL,
        PRIMARY KEY (`UserId`, `RoleId`),
        CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE TABLE `AspNetUserTokens` (
        `UserId` varchar(255) NOT NULL,
        `LoginProvider` varchar(255) NOT NULL,
        `Name` varchar(255) NOT NULL,
        `Value` text NULL,
        PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
        CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('b6dccac9-640f-4290-9583-74aa3639b743', '3cc44424-ae65-43e6-8ffb-43f34f5a90cb', 'ContractStaff', 'CONTRACTSTAFF');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('b3df0079-8fb5-47d3-98af-6868d5b4902c', '651f5778-97a9-4d45-acdd-92968d804b0e', 'Administrator', 'ADMINISTRATOR');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('cb7f9839-85f8-474f-999b-44ed5ddd51b3', '0bc62864-6caa-4388-8929-494d9835173e', 'BranchStaff', 'BRANCHSTAFF');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('a1339fc2-cd79-4b1f-af0e-787fc60eb333', '51e7f070-f967-4ae1-92aa-6d05834d61df', 'JustRegistered', 'JUSTREGISTERED');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('66612b9c-b66b-455b-9d2b-d0f37a7af7e7', '1a9ca471-a60a-4f0a-b1a5-72210b332e3f', 'Suspended', 'SUSPENDED');
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230414095144_Initial1')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230414095144_Initial1', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230416101158_addedRegistrationDate')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = '66612b9c-b66b-455b-9d2b-d0f37a7af7e7';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230416101158_addedRegistrationDate')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = 'a1339fc2-cd79-4b1f-af0e-787fc60eb333';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230416101158_addedRegistrationDate')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = 'b3df0079-8fb5-47d3-98af-6868d5b4902c';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230416101158_addedRegistrationDate')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = 'b6dccac9-640f-4290-9583-74aa3639b743';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230416101158_addedRegistrationDate')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = 'cb7f9839-85f8-474f-999b-44ed5ddd51b3';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230416101158_addedRegistrationDate')
BEGIN
    ALTER TABLE `AspNetUsers` ADD `RegistrationDate` datetime NOT NULL DEFAULT '0001-01-01 00:00:00.000000';
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230416101158_addedRegistrationDate')
BEGIN
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('762919a1-1647-496f-b6e2-a9cf3d40925f', '0fd80c9c-ee21-422b-a0b3-7f19b312734e', 'ContractStaff', 'CONTRACTSTAFF');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('207a6677-5968-4e92-90d0-2787f7a0093b', '9a394f22-e4b6-4127-a445-f400dd065088', 'Administrator', 'ADMINISTRATOR');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('dd1d721e-dd0f-412b-828b-4efc47287a28', '836ad27b-bb32-4e67-bb6c-036ca193c29e', 'BranchStaff', 'BRANCHSTAFF');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('969f1b27-f295-4953-b97a-f7860d643312', '93881dd4-ed8d-49eb-82bc-7ad0e7991dc0', 'JustRegistered', 'JUSTREGISTERED');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('d22ee507-087f-47c2-868c-bf7b92c4d616', 'eb14c1d4-bfe7-44de-a7de-e6cbb52af806', 'Suspended', 'SUSPENDED');
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230416101158_addedRegistrationDate')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230416101158_addedRegistrationDate', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230417095736_employeeNumberUnique')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = '207a6677-5968-4e92-90d0-2787f7a0093b';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230417095736_employeeNumberUnique')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = '762919a1-1647-496f-b6e2-a9cf3d40925f';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230417095736_employeeNumberUnique')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = '969f1b27-f295-4953-b97a-f7860d643312';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230417095736_employeeNumberUnique')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = 'd22ee507-087f-47c2-868c-bf7b92c4d616';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230417095736_employeeNumberUnique')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = 'dd1d721e-dd0f-412b-828b-4efc47287a28';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230417095736_employeeNumberUnique')
BEGIN
    ALTER TABLE `AspNetUsers` ADD CONSTRAINT `AK_AspNetUsers_EmployeeNumber` UNIQUE (`EmployeeNumber`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230417095736_employeeNumberUnique')
BEGIN
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('29c1a6fd-63ac-4c8e-afc2-245d23bddd51', 'f01e7d75-1c9a-400d-adb7-c9d7dc4bc9a9', 'ContractStaff', 'CONTRACTSTAFF');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('c9a18f57-3633-4b48-b8fc-b17d4097f53c', '11b460ce-d022-453a-8e99-a6d4e8daa7f0', 'Administrator', 'ADMINISTRATOR');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('07f79753-8549-4fb7-a527-1cf878a52d22', 'fbeb6eb8-ddd2-4694-8d8e-642838ea13bd', 'BranchStaff', 'BRANCHSTAFF');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('f309bcd5-30d9-4697-b89c-f320ec7786f6', '022ca703-b1e8-4e16-9fd7-5c43685c7125', 'JustRegistered', 'JUSTREGISTERED');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('4f39fbc6-35f6-4d57-b480-0dd1996dfc49', 'fc271436-121d-45ab-945c-75bebc1e5bf4', 'Suspended', 'SUSPENDED');
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230417095736_employeeNumberUnique')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230417095736_employeeNumberUnique', '5.0.17');
END;

COMMIT;

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230606074903_checkChanges')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = '07f79753-8549-4fb7-a527-1cf878a52d22';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230606074903_checkChanges')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = '29c1a6fd-63ac-4c8e-afc2-245d23bddd51';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230606074903_checkChanges')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = '4f39fbc6-35f6-4d57-b480-0dd1996dfc49';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230606074903_checkChanges')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = 'c9a18f57-3633-4b48-b8fc-b17d4097f53c';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230606074903_checkChanges')
BEGIN
    DELETE FROM `AspNetRoles`
    WHERE `Id` = 'f309bcd5-30d9-4697-b89c-f320ec7786f6';
    SELECT ROW_COUNT();
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230606074903_checkChanges')
BEGIN
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('7875c06c-c194-4dd1-9369-8ac75bc8b43c', '022c2b18-965d-451e-ac96-be886803c49c', 'ContractStaff', 'CONTRACTSTAFF');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('f3554651-4f0a-4615-8537-9802f7447c55', 'f3c82cf3-b530-48d0-bdac-73b773859a8d', 'Administrator', 'ADMINISTRATOR');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('344c4438-435c-484d-a046-78c26f5420ad', 'ebe17614-7ec9-42cf-bf86-f7f97d050076', 'BranchStaff', 'BRANCHSTAFF');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('83e30c84-1b12-4ebb-bdc4-b96ae4549c9f', '33620093-c553-4d4a-848a-fd5f64726439', 'JustRegistered', 'JUSTREGISTERED');
    INSERT INTO `AspNetRoles` (`Id`, `ConcurrencyStamp`, `Name`, `NormalizedName`)
    VALUES ('f58a1fca-f621-48a7-baa5-128d740d2737', 'b1369fbf-369f-4289-a94e-96b619a80b19', 'Suspended', 'SUSPENDED');
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230606074903_checkChanges')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230606074903_checkChanges', '5.0.17');
END;

COMMIT;

