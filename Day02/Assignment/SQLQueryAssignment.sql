SELECT
    p.FullName AS ProviderName,
    d.Name AS DepartmentName,
    COUNT(e.EncounterId) AS TotalEncountersHandled,
    RANK() OVER (ORDER BY COUNT(e.EncounterId) DESC) AS ProviderRank
FROM Provider p
LEFT JOIN Encounter e 
    ON p.ProviderId = e.ProviderId
LEFT JOIN Department d 
    ON p.DepartmentId = d.DepartmentId
GROUP BY 
    p.ProviderId,
    p.FullName,
    d.Name
ORDER BY 
    TotalEncountersHandled DESC;


CREATE OR ALTER VIEW vw_Billing_Claims
AS
SELECT
    c.ClaimId,
    c.Status,
    c.BilledAmount,
    c.ReimbursedAmt,
    (c.BilledAmount - ISNULL(c.ReimbursedAmt, 0)) AS OutstandingAmount,
    
    e.EncounterId,
    e.EncounterType,
    e.AdmitDate,
    e.DischargeDate,
    
    p.PatientId,
    p.FullName AS PatientName,
    
    pr.ProviderId,
    pr.FullName AS ProviderName,
    
    d.DepartmentId,
    d.Name AS DepartmentName

FROM Claim c
JOIN Encounter e ON c.EncounterId = e.EncounterId
JOIN Patient p ON e.PatientId = p.PatientId
LEFT JOIN Provider pr ON e.ProviderId = pr.ProviderId
JOIN Department d ON e.DepartmentId = d.DepartmentId;


CREATE OR ALTER PROCEDURE sp_ExecutiveDashboard
AS
BEGIN
    SET NOCOUNT ON;

   
    SELECT 
        COUNT(*) AS TotalActivePatients
    FROM Patient
    WHERE IsActive = 1;


    
    SELECT TOP 5
        d.Name AS DepartmentName,
        COUNT(*) AS TotalEncounters
    FROM Encounter e
    JOIN Department d ON e.DepartmentId = d.DepartmentId
    GROUP BY d.Name
    ORDER BY TotalEncounters DESC;


   
    SELECT
        COUNT(*) AS DeniedClaims,
        SUM(BilledAmount) AS TotalDeniedAmount
    FROM Claim
    WHERE Status = 'Denied';


    
    SELECT
        p.PatientId,
        p.FullName,
        COUNT(e.EncounterId) AS TotalAdmissions
    FROM Encounter e
    JOIN Patient p ON e.PatientId = p.PatientId
    WHERE e.EncounterType = 'Inpatient'
    GROUP BY p.PatientId, p.FullName
    HAVING COUNT(e.EncounterId) >= 3
    ORDER BY TotalAdmissions DESC;

END;

EXEC sp_RevenueLeakageAnalysis;

EXEC sp_ExecutiveDashboard;

SELECT * FROM vw_Billing_Claims;

CREATE OR ALTER PROCEDURE sp_30DayReadmissions
AS
BEGIN
    SELECT 
        e1.PatientId,
        p.FullName,
        e1.DischargeDate,
        e2.AdmitDate AS ReadmissionDate,
        DATEDIFF(DAY, e1.DischargeDate, e2.AdmitDate) AS DaysBetween
    FROM Encounter e1
    JOIN Encounter e2 
        ON e1.PatientId = e2.PatientId
       AND e2.AdmitDate > e1.DischargeDate
       AND DATEDIFF(DAY, e1.DischargeDate, e2.AdmitDate) <= 30
    JOIN Patient p ON e1.PatientId = p.PatientId
    WHERE e1.DischargeDate IS NOT NULL;
END;


CREATE OR ALTER PROCEDURE sp_HighRiskPatients
AS
BEGIN
    SELECT
        p.PatientId,
        p.FullName,
        COUNT(e.EncounterId) AS TotalAdmissions
    FROM Encounter e
    JOIN Patient p ON e.PatientId = p.PatientId
    WHERE e.EncounterType = 'Inpatient'
    GROUP BY p.PatientId, p.FullName
    HAVING COUNT(e.EncounterId) >= 3
    ORDER BY TotalAdmissions DESC;
END;


CREATE OR ALTER PROCEDURE sp_ProviderWorkload
AS
BEGIN
    SELECT
        pr.ProviderId,
        pr.FullName,
        pr.Specialty,
        COUNT(e.EncounterId) AS TotalEncounters
    FROM Provider pr
    LEFT JOIN Encounter e ON pr.ProviderId = e.ProviderId
    GROUP BY pr.ProviderId, pr.FullName, pr.Specialty
    ORDER BY TotalEncounters DESC;
END;

CREATE OR ALTER PROCEDURE sp_RevenueAnalysis
AS
BEGIN
    SELECT
        Status,
        COUNT(*) AS TotalClaims,
        SUM(BilledAmount) AS TotalBilled,
        SUM(ISNULL(ReimbursedAmt, 0)) AS TotalPaid,
        SUM(BilledAmount - ISNULL(ReimbursedAmt, 0)) AS Outstanding
    FROM Claim
    GROUP BY Status
    ORDER BY Outstanding DESC;
END;


ALTER TABLE Insurance
ADD
    ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START HIDDEN,
    ValidTo   DATETIME2 GENERATED ALWAYS AS ROW END HIDDEN,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo);

ALTER TABLE Insurance
SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.InsuranceHistory));


DECLARE @PatientId INT = 1;
DECLARE @SixMonthsAgo DATETIME2 = DATEADD(MONTH, -6, SYSUTCDATETIME());

SELECT
    InsuranceId,
    Payer,
    PolicyNumber,
    ValidFrom,
    ValidTo
FROM Insurance
FOR SYSTEM_TIME ALL
WHERE PatientId = @PatientId
AND ValidFrom <= @SixMonthsAgo
AND ValidTo >= @SixMonthsAgo
ORDER BY ValidFrom;

