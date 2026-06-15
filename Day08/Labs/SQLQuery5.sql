-- =====================================================
-- Smart Hospital EHR Database Setup Script
-- Creates EHRMVC Database + Healthcare Schema + Tables
-- =====================================================

-- 1. CREATE DATABASE IF IT DOES NOT EXIST
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'EHRMVC')
BEGIN
    CREATE DATABASE EHRMVC;
    PRINT 'Database EHRMVC created successfully.';
END
ELSE
BEGIN
    PRINT 'Database EHRMVC already exists.';
END
GO

-- Switch to the database
USE EHRMVC;
GO

-- =====================================================
-- 2. ENSURE HEALTHCARE SCHEMA
-- =====================================================
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Healthcare')
BEGIN
    EXEC ('CREATE SCHEMA Healthcare');
    PRINT 'Schema Healthcare created.';
END
GO

-- =====================================================
-- 3. DOCTORS TABLE
-- =====================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.tables 
    WHERE name = 'Doctors' AND schema_id = SCHEMA_ID('Healthcare')
)
BEGIN
    CREATE TABLE Healthcare.Doctors
    (
        DoctorId INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        Specialty NVARCHAR(100) NOT NULL,
        LicenseNumber NVARCHAR(20) NOT NULL UNIQUE,
        Email NVARCHAR(100) NOT NULL,
        Phone NVARCHAR(20) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
        ModifiedDate DATETIME2 DEFAULT GETUTCDATE()
    );
    PRINT 'Table Healthcare.Doctors created.';
END
GO

-- =====================================================
-- 4. PATIENTS TABLE
-- =====================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.tables 
    WHERE name = 'Patients' AND schema_id = SCHEMA_ID('Healthcare')
)
BEGIN
    CREATE TABLE Healthcare.Patients
    (
        PatientId INT IDENTITY(1,1) PRIMARY KEY,
        PatientGuid UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        DateOfBirth DATE NOT NULL,
        Gender NVARCHAR(10) NOT NULL,
        SSN NVARCHAR(11) NULL,
        Email NVARCHAR(100) NOT NULL,
        Phone NVARCHAR(20) NOT NULL,
        Address NVARCHAR(200) NOT NULL,
        City NVARCHAR(50) NOT NULL,
        State NVARCHAR(2) NOT NULL,
        ZipCode NVARCHAR(10) NOT NULL,
        EmergencyContactName NVARCHAR(100) NOT NULL,
        EmergencyContactPhone NVARCHAR(20) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
        ModifiedDate DATETIME2 DEFAULT GETUTCDATE()
    );
    PRINT 'Table Healthcare.Patients created.';
END
GO

-- =====================================================
-- 5. APPOINTMENTS TABLE
-- =====================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.tables 
    WHERE name = 'Appointments' AND schema_id = SCHEMA_ID('Healthcare')
)
BEGIN
    CREATE TABLE Healthcare.Appointments
    (
        AppointmentId INT IDENTITY(1,1) PRIMARY KEY,
        PatientId INT NOT NULL,
        DoctorId INT NOT NULL,
        AppointmentDate DATETIME2 NOT NULL,
        DurationMinutes INT NOT NULL DEFAULT 30,
        ReasonForVisit NVARCHAR(500) NOT NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT 'Scheduled',
        Notes NVARCHAR(MAX) NULL,
        CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
        ModifiedDate DATETIME2 DEFAULT GETUTCDATE(),

        CONSTRAINT FK_Appointments_Patient 
            FOREIGN KEY (PatientId) REFERENCES Healthcare.Patients(PatientId),
        CONSTRAINT FK_Appointments_Doctor 
            FOREIGN KEY (DoctorId) REFERENCES Healthcare.Doctors(DoctorId)
    );
    PRINT 'Table Healthcare.Appointments created.';
END
GO

-- =====================================================
-- 6. AUDIT LOG TABLE (HIPAA Compliance)
-- =====================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.tables 
    WHERE name = 'AuditLog' AND schema_id = SCHEMA_ID('Healthcare')
)
BEGIN
    CREATE TABLE Healthcare.AuditLog
    (
        AuditId INT IDENTITY(1,1) PRIMARY KEY,
        UserId NVARCHAR(100) NOT NULL,
        Action NVARCHAR(50) NOT NULL,
        TableName NVARCHAR(50) NOT NULL,
        RecordId INT NOT NULL,
        PatientId INT NULL,
        IPAddress NVARCHAR(50) NULL,
        UserAgent NVARCHAR(500) NULL,
        AccessDate DATETIME2 DEFAULT GETUTCDATE(),
        Details NVARCHAR(MAX) NULL
    );
    PRINT 'Table Healthcare.AuditLog created.';
END
GO

-- =====================================================
-- 7. INSERT SAMPLE DATA (Only if tables are empty)
-- =====================================================

-- Sample Doctors
IF NOT EXISTS (SELECT 1 FROM Healthcare.Doctors)
BEGIN
    INSERT INTO Healthcare.Doctors 
    (FirstName, LastName, Specialty, LicenseNumber, Email, Phone)
    VALUES
        ('Sarah', 'Johnson', 'Cardiology', 'LIC-CAR-1001', 'sarah.johnson@ehr.com', '555-1001'),
        ('Michael', 'Chen', 'Pediatrics', 'LIC-PED-1002', 'michael.chen@ehr.com', '555-1002'),
        ('Emily', 'Rodriguez', 'Internal Medicine', 'LIC-INT-1003', 'emily.rodriguez@ehr.com', '555-1003'),
        ('David', 'Kim', 'Orthopedics', 'LIC-ORT-1004', 'david.kim@ehr.com', '555-1004');
    PRINT 'Sample Doctors inserted.';
END
GO

-- Sample Patients
IF NOT EXISTS (SELECT 1 FROM Healthcare.Patients)
BEGIN
    INSERT INTO Healthcare.Patients
    (
        FirstName, LastName, DateOfBirth, Gender,
        Email, Phone, Address, City, State, ZipCode,
        EmergencyContactName, EmergencyContactPhone
    )
    VALUES
        ('John', 'Smith', '1985-03-15', 'Male',
         'john.smith@email.com', '555-2001',
         '123 Main St', 'Springfield', 'IL', '62701',
         'Jane Smith', '555-2002'),

        ('Maria', 'Garcia', '1990-07-22', 'Female',
         'maria.garcia@email.com', '555-2003',
         '456 Oak Ave', 'Springfield', 'IL', '62702',
         'Carlos Garcia', '555-2004');
    PRINT 'Sample Patients inserted.';
END
GO

-- Sample Appointment
IF NOT EXISTS (SELECT 1 FROM Healthcare.Appointments)
BEGIN
    INSERT INTO Healthcare.Appointments
    (
        PatientId, DoctorId, AppointmentDate,
        ReasonForVisit, Status
    )
    VALUES
        (1, 1, DATEADD(DAY, 1, GETDATE()),
         'Routine cardiac checkup', 'Scheduled');
    PRINT 'Sample Appointment inserted.';
END
GO

-- Sample Audit Log
IF NOT EXISTS (SELECT 1 FROM Healthcare.AuditLog)
BEGIN
    INSERT INTO Healthcare.AuditLog
    (
        UserId, Action, TableName, RecordId, Details
    )
    VALUES
        ('SystemSeed', 'INSERT', 'InitialSetup', 0,
         'Initial healthcare demo data seeded');
    PRINT 'Sample Audit Log inserted.';
END
GO

PRINT '=====================================';
PRINT 'EHRMVC Database Setup Completed!';
PRINT '=====================================';


--5.1 Ensure Database & Schema

USE EHRMVC;
GO

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'Healthcare')
    EXEC('CREATE SCHEMA Healthcare');
GO

--5.2 PatientTransactions Table (Operational Data)

CREATE TABLE Healthcare.PatientTransactions
(
    TransactionId INT IDENTITY PRIMARY KEY,
    PatientName NVARCHAR(100) NOT NULL,
    TransactionType NVARCHAR(20) NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    TransactionDate DATETIME2 NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);
GO

--5.3 TransactionLedger Table (Immutable History)

CREATE TABLE Healthcare.TransactionLedger
(
    LedgerId INT IDENTITY PRIMARY KEY,
    TransactionId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,     -- Snapshot of original amount
    PreviousHash NVARCHAR(256) NOT NULL,
    CurrentHash NVARCHAR(256) NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Ledger_Transaction
        FOREIGN KEY (TransactionId)
        REFERENCES Healthcare.PatientTransactions(TransactionId)
);
GO


UPDATE Healthcare.PatientTransactions
SET Amount = 999999
WHERE TransactionId = 1;

SELECT * FROM Healthcare.PatientTransactions;
SELECT * FROM Healthcare.TransactionLedger;


