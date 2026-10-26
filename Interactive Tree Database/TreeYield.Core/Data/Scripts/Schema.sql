-- =====================================================================
-- Interactive Tree Database & Yield Calculator
-- Schema.sql - the Microsoft Access (Jet 4.0) tables, keys and rules.
--
-- The program runs this script when it has to create a new TreeYieldDb.mdb
-- (see DatabaseInitializer.vb). Each statement ends with a semicolon.
-- Lines starting with -- are comments and are removed before running,
-- because Access SQL does not support comments.
--
-- Design notes
--   * 3rd normal form: category names live only in Categories; species
--     point to them with a foreign key (a Relationship in Access).
--   * CHECK constraints repeat the rules of SpeciesValidator.vb. Access
--     enforces them as "validation rules", even when someone types data
--     straight into a table in Microsoft Access.
--   * All times are stored in UTC and shown in local time by the program.
--   * Column names avoid Access reserved words (Name, Action, Version).
-- =====================================================================

CREATE TABLE SchemaVersion (
    VersionNumber INTEGER NOT NULL CONSTRAINT PK_SchemaVersion PRIMARY KEY,
    AppliedAt DATETIME NOT NULL
);

-- Lookup table for the filter chips (Fruit-Bearing / Non-Fruit-Bearing).
CREATE TABLE Categories (
    CategoryId COUNTER NOT NULL CONSTRAINT PK_Categories PRIMARY KEY,
    CategoryName TEXT(50) NOT NULL CONSTRAINT UQ_Categories_Name UNIQUE,
    Description TEXT(200),
    CONSTRAINT CK_Categories_Name CHECK (CategoryName <> '')
);

-- One row per tree species: the data the calculator works with.
CREATE TABLE TreeSpecies (
    SpeciesId COUNTER NOT NULL CONSTRAINT PK_TreeSpecies PRIMARY KEY,
    CommonName TEXT(100) NOT NULL CONSTRAINT UQ_TreeSpecies_CommonName UNIQUE,
    ScientificName TEXT(150),
    CategoryId INTEGER NOT NULL,
    ProductType TEXT(60) NOT NULL,
    YieldUnit TEXT(20) NOT NULL,
    YieldPerTreePerYear DECIMAL(12, 2) NOT NULL,
    PricePerUnit DECIMAL(12, 2) NOT NULL,
    HarvestType TEXT(10) NOT NULL,
    YearsToFirstHarvest INTEGER NOT NULL,
    YearsToFullProduction INTEGER NOT NULL,
    HarvestSeason TEXT(100),
    Notes MEMO,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    CONSTRAINT FK_TreeSpecies_Categories FOREIGN KEY (CategoryId) REFERENCES Categories (CategoryId),
    CONSTRAINT CK_TreeSpecies_CommonName CHECK (CommonName <> ''),
    CONSTRAINT CK_TreeSpecies_Yield CHECK (YieldPerTreePerYear > 0 AND YieldPerTreePerYear <= 100000),
    CONSTRAINT CK_TreeSpecies_Price CHECK (PricePerUnit >= 0 AND PricePerUnit <= 1000000),
    CONSTRAINT CK_TreeSpecies_HarvestType CHECK (HarvestType IN ('Annual', 'Rotation')),
    CONSTRAINT CK_TreeSpecies_FirstHarvest CHECK (YearsToFirstHarvest BETWEEN 0 AND 60),
    CONSTRAINT CK_TreeSpecies_FullProduction CHECK (YearsToFullProduction BETWEEN 0 AND 60),
    CONSTRAINT CK_TreeSpecies_YearOrder CHECK (YearsToFullProduction >= YearsToFirstHarvest),
    CONSTRAINT CK_TreeSpecies_RotationAge CHECK (HarvestType <> 'Rotation' OR YearsToFirstHarvest >= 1)
);

-- Administrator accounts. Only a salted PBKDF2 hash of the password is stored (as Base64 text).
CREATE TABLE Users (
    UserId COUNTER NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    Username TEXT(50) NOT NULL CONSTRAINT UQ_Users_Username UNIQUE,
    PasswordHash TEXT(100) NOT NULL,
    PasswordSalt TEXT(50) NOT NULL,
    Iterations INTEGER NOT NULL,
    MustChangePassword BIT NOT NULL,
    FailedLoginCount INTEGER NOT NULL,
    LockoutUntil DATETIME,
    LastLoginAt DATETIME,
    CreatedAt DATETIME NOT NULL,
    CONSTRAINT CK_Users_Username CHECK (Username <> ''),
    CONSTRAINT CK_Users_Iterations CHECK (Iterations > 0),
    CONSTRAINT CK_Users_FailedLoginCount CHECK (FailedLoginCount >= 0)
);

-- Saved calculations. Deleting a species deletes its scenarios (ON DELETE CASCADE).
CREATE TABLE Scenarios (
    ScenarioId COUNTER NOT NULL CONSTRAINT PK_Scenarios PRIMARY KEY,
    ScenarioName TEXT(100) NOT NULL,
    SpeciesId INTEGER NOT NULL,
    TreeCount INTEGER NOT NULL,
    ProjectionYears INTEGER NOT NULL,
    Notes MEMO,
    CreatedAt DATETIME NOT NULL,
    CONSTRAINT FK_Scenarios_TreeSpecies FOREIGN KEY (SpeciesId) REFERENCES TreeSpecies (SpeciesId) ON DELETE CASCADE,
    CONSTRAINT CK_Scenarios_Name CHECK (ScenarioName <> ''),
    CONSTRAINT CK_Scenarios_TreeCount CHECK (TreeCount BETWEEN 1 AND 100000),
    CONSTRAINT CK_Scenarios_ProjectionYears CHECK (ProjectionYears BETWEEN 1 AND 30)
);

-- History of administrator actions. Details stores the record name as text,
-- so the history still makes sense after a record is deleted.
CREATE TABLE AuditLog (
    AuditId COUNTER NOT NULL CONSTRAINT PK_AuditLog PRIMARY KEY,
    UserId INTEGER,
    ActionCode TEXT(30) NOT NULL,
    EntityName TEXT(50),
    EntityId INTEGER,
    Details MEMO,
    OccurredAt DATETIME NOT NULL,
    CONSTRAINT FK_AuditLog_Users FOREIGN KEY (UserId) REFERENCES Users (UserId)
);

-- Speeds up "newest first" in the audit log screen.
CREATE INDEX IX_AuditLog_OccurredAt ON AuditLog (OccurredAt);
