IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260620152846_FirstMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260620152846_FirstMigration', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260620160454_AddingCostomersTable'
)
BEGIN
    CREATE TABLE [CustomersTable] (
        [Id] INTEGER NOT NULL,
        [CustomerName] TEXT NOT NULL,
        [CustomerDateOfBirth] TEXT NOT NULL,
        [CustomerNationalId] REAL NOT NULL,
        [IsMale] INTEGER NOT NULL,
        [Grade] INTEGER NOT NULL,
        [Notes] TEXT NULL,
        [CustomerEmail] TEXT NOT NULL,
        [CustomerPhoenNumber] TEXT NOT NULL,
        CONSTRAINT [PK_CustomersTable] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260620160454_AddingCostomersTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260620160454_AddingCostomersTable', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624151052_AddingCustomeraddress'
)
BEGIN
    ALTER TABLE [CustomersTable] ADD [Adress] TEXT NOT NULL DEFAULT '';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624151052_AddingCustomeraddress'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624151052_AddingCustomeraddress', N'10.0.9');
END;

COMMIT;
GO

