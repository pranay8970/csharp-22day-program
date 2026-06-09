SELECT COUNT(*) AS Patients
FROM Patient;
SELECT COUNT(*) AS Encounters
FROM Encounter;
SELECT COUNT(*) AS Diagnoses
FROM Diagnosis;
SELECT COUNT(*) AS Claims
FROM Claim;

INSERT INTO Patient
(
Mrn,
FullName,
DateOfBirth,
Gender,
City,
IsActive
)
VALUES
(
'MRN999999',
'Rahul Verma',
'1985-06-15',
'M',
'Hyderabad',
1
);

select * from Patient where Mrn = 'MRN999999';

SELECT *
FROM Patient
WHERE Mrn = 'MRN999999';





INSERT INTO Encounter
(
PatientId,
ProviderId,
DepartmentId,
AdmitDate,
DischargeDate,
EncounterType
)
SELECT
1007,
1,
1,
DATEADD(DAY,-v.number,GETDATE()),
GETDATE(),
'Outpatient'
FROM master..spt_values v
WHERE v.type = 'P'
AND v.number < 500;

INSERT INTO Diagnosis
(
EncounterId,
IcdCode,
Description,
DiagnosedOn
)
SELECT
EncounterId,
'I10',
'Hypertension',
GETDATE()
FROM Encounter
WHERE PatientId = 1007;

INSERT INTO Claim
(
EncounterId,
InsuranceId,
BilledAmount,
ReimbursedAmt,
Status
)
SELECT
EncounterId,
1,
15000,
12000,
'Paid'
FROM Encounter
WHERE PatientId = 1007;

SELECT *
FROM Patient
WHERE MRN = 'MRN999999';

SELECT *
FROM Patient
WHERE PatientId = 1003;

INSERT INTO Patient (MRN, FullName, DateOfBirth, Gender, City)
VALUES ('MRN999999', 'Test Patient', '1995-01-01', 'M', 'Hyderabad');

SELECT
    Status,
    COUNT(*)                          AS ClaimCount,
    SUM(BilledAmount)                 AS TotalBilled,
    SUM(ReimbursedAmt)                AS TotalReimbursed,
    SUM(BilledAmount - ReimbursedAmt) AS Gap
FROM Claim
GROUP BY Status
ORDER BY TotalBilled DESC;
 
-- Single Revenue-at-Risk figure (everything not yet Paid)
SELECT SUM(BilledAmount) AS RevenueAtRisk
FROM Claim
WHERE Status <> 'Paid';

INSERT INTO Patient (MRN, FullName, DateOfBirth, Gender, City, IsActive)
VALUES ('MRN888888', 'Meera Iyer', '1979-02-11', 'F', 'Hyderabad', 1);

-- 2) Note the PatientId (assume @pid below; replace if different)
DECLARE @pid INT = (SELECT PatientId FROM Patient WHERE MRN = 'MRN888888');
 
-- 3) Create 100 encounters for this patient
INSERT INTO Encounter (PatientId, ProviderId, DepartmentId, AdmitDate, DischargeDate, EncounterType)
SELECT @pid, 1, 1, DATEADD(DAY, -v.number, GETDATE()), GETDATE(), 'Inpatient'
FROM master..spt_values v
WHERE v.type = 'P' AND v.number < 100;
 
-- 4) Give EACH encounter 3 diagnoses (300 total)
INSERT INTO Diagnosis (EncounterId, IcdCode, Description, DiagnosedOn)
SELECT e.EncounterId, x.Icd, x.Descr, GETDATE()
FROM Encounter e
CROSS JOIN (VALUES ('I10','Hypertension'),('E11','Type 2 Diabetes'),('J45','Asthma')) AS x(Icd, Descr)
WHERE e.PatientId = @pid;
 
-- 5) Give EACH encounter 3 claims (300 total)
INSERT INTO Claim (EncounterId, InsuranceId, BilledAmount, ReimbursedAmt, Status)
SELECT e.EncounterId, 1, y.Billed, y.Reimb, y.St
FROM Encounter e
CROSS JOIN (VALUES (15000,12000,'Paid'),(8000,0,'Denied'),(20000,0,'Submitted')) AS y(Billed, Reimb, St)
WHERE e.PatientId = @pid;

--Verify your fan-out before coding. These counts are the TRUE row counts; remember them.
DECLARE @pid INT = (SELECT PatientId FROM Patient WHERE MRN = 'MRN888888');
SELECT COUNT(*) AS Encounters FROM Encounter WHERE PatientId = @pid;          -- expect 100
SELECT COUNT(*) AS Diagnoses  FROM Diagnosis d JOIN Encounter e ON d.EncounterId=e.EncounterId
  WHERE e.PatientId = @pid;                                                    -- expect 300
SELECT COUNT(*) AS Claims     FROM Claim c JOIN Encounter e ON c.EncounterId=e.EncounterId
  WHERE e.PatientId = @pid;








