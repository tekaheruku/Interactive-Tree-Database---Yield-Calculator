-- =====================================================================
-- Interactive Tree Database & Yield Calculator
-- 001_Schema.sql - creates all tables, keys, constraints and indexes.
--
-- The program runs this script automatically the first time it starts
-- (see DatabaseInitializer.cs). It can also be opened and run by hand in
-- Visual Studio (SQL Server Object Explorer) or SQL Server Management Studio.
-- Every CREATE is guarded with IF OBJECT_ID(...) IS NULL, so running the
-- script twice does no harm.
--
-- Design notes
--   * 3rd normal form: category names live only in Categories; species
--     point to them with a foreign key.
--   * CHECK constraints repeat the validation rules of SpeciesValidator.cs,
--     so invalid values are rejected even if the program is bypassed.
--   * All times are stored in UTC (SYSUTCDATETIME) and shown in local time.
-- =====================================================================

IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchemaVersion
    (
        Version   INT       NOT NULL CONSTRAINT PK_SchemaVersion PRIMARY KEY,
        AppliedAt DATETIME2 NOT NULL CONSTRAINT DF_SchemaVersion_AppliedAt DEFAULT SYSUTCDATETIME()
    );
END
GO

-- Lookup table for the filter chips (Fruit-Bearing / Non-Fruit-Bearing).
IF OBJECT_ID(N'dbo.Categories', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categories
    (
        CategoryId  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categories PRIMARY KEY,
        Name        NVARCHAR(50)      NOT NULL CONSTRAINT UQ_Categories_Name UNIQUE,
        Description NVARCHAR(200)     NULL
    );
END
GO

-- One row per tree species: the data the calculator works with.
IF OBJECT_ID(N'dbo.TreeSpecies', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TreeSpecies
    (
        SpeciesId             INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TreeSpecies PRIMARY KEY,
        CommonName            NVARCHAR(100)     NOT NULL,
        ScientificName        NVARCHAR(150)     NULL,
        CategoryId            INT               NOT NULL,
        ProductType           NVARCHAR(60)      NOT NULL,
        YieldUnit             NVARCHAR(20)      NOT NULL,
        YieldPerTreePerYear   DECIMAL(12,2)     NOT NULL,
        PricePerUnit          DECIMAL(12,2)     NOT NULL,
        HarvestType           NVARCHAR(10)      NOT NULL CONSTRAINT DF_TreeSpecies_HarvestType DEFAULT N'Annual',
        YearsToFirstHarvest   INT               NOT NULL,
        YearsToFullProduction INT               NOT NULL,
        HarvestSeason         NVARCHAR(100)     NULL,
        Notes                 NVARCHAR(1000)    NULL,
        CreatedAt             DATETIME2         NOT NULL CONSTRAINT DF_TreeSpecies_CreatedAt DEFAULT SYSUTCDATETIME(),
        UpdatedAt             DATETIME2         NOT NULL CONSTRAINT DF_TreeSpecies_UpdatedAt DEFAULT SYSUTCDATETIME(),

        CONSTRAINT UQ_TreeSpecies_CommonName UNIQUE (CommonName),
        CONSTRAINT FK_TreeSpecies_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories (CategoryId),
        CONSTRAINT CK_TreeSpecies_CommonName CHECK (LEN(CommonName) > 0),
        CONSTRAINT CK_TreeSpecies_Yield CHECK (YieldPerTreePerYear > 0 AND YieldPerTreePerYear <= 100000),
        CONSTRAINT CK_TreeSpecies_Price CHECK (PricePerUnit >= 0 AND PricePerUnit <= 1000000),
        CONSTRAINT CK_TreeSpecies_HarvestType CHECK (HarvestType IN (N'Annual', N'Rotation')),
        CONSTRAINT CK_TreeSpecies_FirstHarvest CHECK (YearsToFirstHarvest BETWEEN 0 AND 60),
        CONSTRAINT CK_TreeSpecies_FullProduction CHECK (YearsToFullProduction BETWEEN 0 AND 60
                                                        AND YearsToFullProduction >= YearsToFirstHarvest),
        CONSTRAINT CK_TreeSpecies_RotationAge CHECK (HarvestType <> N'Rotation' OR YearsToFirstHarvest >= 1)
    );

    -- Speeds up the category filter and the JOIN to Categories.
    CREATE INDEX IX_TreeSpecies_CategoryId ON dbo.TreeSpecies (CategoryId);
END
GO

-- Administrator accounts. Only a salted PBKDF2 hash of the password is stored.
IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        UserId             INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
        Username           NVARCHAR(50)      NOT NULL CONSTRAINT UQ_Users_Username UNIQUE,
        PasswordHash       VARBINARY(64)     NOT NULL,
        PasswordSalt       VARBINARY(32)     NOT NULL,
        Iterations         INT               NOT NULL,
        MustChangePassword BIT               NOT NULL CONSTRAINT DF_Users_MustChangePassword DEFAULT 1,
        FailedLoginCount   INT               NOT NULL CONSTRAINT DF_Users_FailedLoginCount DEFAULT 0,
        LockoutUntil       DATETIME2         NULL,
        LastLoginAt        DATETIME2         NULL,
        CreatedAt          DATETIME2         NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME(),

        CONSTRAINT CK_Users_Iterations CHECK (Iterations > 0),
        CONSTRAINT CK_Users_FailedLoginCount CHECK (FailedLoginCount >= 0)
    );
END
GO

-- Saved calculations. Deleting a species deletes its scenarios (ON DELETE CASCADE).
IF OBJECT_ID(N'dbo.Scenarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Scenarios
    (
        ScenarioId      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Scenarios PRIMARY KEY,
        Name            NVARCHAR(100)     NOT NULL,
        SpeciesId       INT               NOT NULL,
        TreeCount       INT               NOT NULL,
        ProjectionYears INT               NOT NULL CONSTRAINT DF_Scenarios_ProjectionYears DEFAULT 10,
        Notes           NVARCHAR(500)     NULL,
        CreatedAt       DATETIME2         NOT NULL CONSTRAINT DF_Scenarios_CreatedAt DEFAULT SYSUTCDATETIME(),

        CONSTRAINT FK_Scenarios_TreeSpecies FOREIGN KEY (SpeciesId)
            REFERENCES dbo.TreeSpecies (SpeciesId) ON DELETE CASCADE,
        CONSTRAINT CK_Scenarios_Name CHECK (LEN(Name) > 0),
        CONSTRAINT CK_Scenarios_TreeCount CHECK (TreeCount BETWEEN 1 AND 100000),
        CONSTRAINT CK_Scenarios_ProjectionYears CHECK (ProjectionYears BETWEEN 1 AND 30)
    );

    CREATE INDEX IX_Scenarios_SpeciesId ON dbo.Scenarios (SpeciesId);
END
GO

-- History of administrator actions. Details stores the record name as text,
-- so the history still makes sense after a record is deleted.
IF OBJECT_ID(N'dbo.AuditLog', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AuditLog
    (
        AuditId    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLog PRIMARY KEY,
        UserId     INT               NULL,
        Action     NVARCHAR(30)      NOT NULL,
        EntityName NVARCHAR(50)      NULL,
        EntityId   INT               NULL,
        Details    NVARCHAR(500)     NULL,
        OccurredAt DATETIME2         NOT NULL CONSTRAINT DF_AuditLog_OccurredAt DEFAULT SYSUTCDATETIME(),

        CONSTRAINT FK_AuditLog_Users FOREIGN KEY (UserId) REFERENCES dbo.Users (UserId)
    );

    CREATE INDEX IX_AuditLog_OccurredAt ON dbo.AuditLog (OccurredAt DESC);
END
GO
