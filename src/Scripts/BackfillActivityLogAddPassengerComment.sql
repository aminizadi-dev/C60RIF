-- =============================================================================
-- Rewrite historical "AddNewPassenger" activity log comments to the new format
--
--   old: رهجو جدید با شناسه 27912 افزوده شد
--   new: رهجو جدید با شماره رهایی {RecoveryNo} و شماره کارت {CardNo} افزوده شد
--
-- The recovery form id is read from [ActivityLog].[EntityId] (it is stored there
-- by InsertActivityAsync), so the comment text is never parsed.
--   RecoveryNo -> [RecoveryForm].[RecoveryNo]
--   CardNo     -> [Person].[CardNo] via [RecoveryForm].[PersonId]
--
-- Notes:
--   * Idempotent: rows already in the new format are skipped, so it can be re-run.
--   * Step 2 keeps the old comments in a backup table (rollback in step 5).
--   * Run steps 1..4 in order; step 1 alone is a safe dry run.
-- =============================================================================
SET NOCOUNT ON;
GO

-- -----------------------------------------------------------------------------
-- STEP 1: dry run - what will change and what cannot be resolved
-- -----------------------------------------------------------------------------
DECLARE @CardNoFallback NVARCHAR(50) = N'نامشخص'; -- used when the person has no card number

-- 1a. summary
SELECT
    [Total]        = COUNT(*),
    [AlreadyNew]   = SUM(CASE WHEN al.[Comment] LIKE N'%شماره رهایی%' THEN 1 ELSE 0 END),
    [WillUpdate]   = SUM(CASE WHEN al.[Comment] NOT LIKE N'%شماره رهایی%' AND rf.[Id] IS NOT NULL THEN 1 ELSE 0 END),
    [Unresolvable] = SUM(CASE WHEN al.[Comment] NOT LIKE N'%شماره رهایی%' AND rf.[Id] IS NULL THEN 1 ELSE 0 END)
FROM [dbo].[ActivityLog] al
INNER JOIN [dbo].[ActivityLogType] alt ON alt.[Id] = al.[ActivityLogTypeId]
LEFT JOIN [dbo].[RecoveryForm] rf ON rf.[Id] = al.[EntityId]
WHERE alt.[SystemKeyword] = 'AddNewPassenger';

-- 1b. preview of the new text (first 50 rows)
SELECT TOP (50)
    al.[Id],
    al.[EntityId],
    [OldComment] = al.[Comment],
    [NewComment] = N'رهجو جدید با شماره رهایی ' + rf.[RecoveryNo] +
                   N' و شماره کارت ' + ISNULL(NULLIF(LTRIM(RTRIM(p.[CardNo])), N''), @CardNoFallback) +
                   N' افزوده شد'
FROM [dbo].[ActivityLog] al
INNER JOIN [dbo].[ActivityLogType] alt ON alt.[Id] = al.[ActivityLogTypeId]
INNER JOIN [dbo].[RecoveryForm] rf ON rf.[Id] = al.[EntityId]
LEFT JOIN [dbo].[Person] p ON p.[Id] = rf.[PersonId]
WHERE alt.[SystemKeyword] = 'AddNewPassenger'
  AND al.[Comment] NOT LIKE N'%شماره رهایی%'
ORDER BY al.[Id] DESC;

-- 1c. rows that cannot be resolved (no EntityId, or the recovery form was deleted).
--     These keep their original comment; see the optional step 6.
SELECT
    al.[Id],
    al.[EntityId],
    al.[Comment],
    al.[CreatedOnUtc],
    [Reason] = CASE WHEN al.[EntityId] IS NULL THEN N'EntityId is null'
                    ELSE N'RecoveryForm row no longer exists' END
FROM [dbo].[ActivityLog] al
INNER JOIN [dbo].[ActivityLogType] alt ON alt.[Id] = al.[ActivityLogTypeId]
LEFT JOIN [dbo].[RecoveryForm] rf ON rf.[Id] = al.[EntityId]
WHERE alt.[SystemKeyword] = 'AddNewPassenger'
  AND al.[Comment] NOT LIKE N'%شماره رهایی%'
  AND rf.[Id] IS NULL
ORDER BY al.[Id] DESC;
GO

-- -----------------------------------------------------------------------------
-- STEP 2: back up the comments that are about to change
-- -----------------------------------------------------------------------------
IF OBJECT_ID(N'[dbo].[ActivityLogCommentBackup_AddNewPassenger]', N'U') IS NULL
    CREATE TABLE [dbo].[ActivityLogCommentBackup_AddNewPassenger](
        [ActivityLogId]  [int] NOT NULL,
        [OldComment]     [nvarchar](4000) NULL,
        [BackedUpOnUtc]  [datetime2](7) NOT NULL
            CONSTRAINT [DF_ActivityLogCommentBackup_AddNewPassenger_BackedUpOnUtc] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_ActivityLogCommentBackup_AddNewPassenger] PRIMARY KEY CLUSTERED ([ActivityLogId] ASC)
    );

INSERT INTO [dbo].[ActivityLogCommentBackup_AddNewPassenger] ([ActivityLogId], [OldComment])
SELECT al.[Id], al.[Comment]
FROM [dbo].[ActivityLog] al
INNER JOIN [dbo].[ActivityLogType] alt ON alt.[Id] = al.[ActivityLogTypeId]
INNER JOIN [dbo].[RecoveryForm] rf ON rf.[Id] = al.[EntityId]
WHERE alt.[SystemKeyword] = 'AddNewPassenger'
  AND al.[Comment] NOT LIKE N'%شماره رهایی%'
  AND NOT EXISTS (SELECT 1 FROM [dbo].[ActivityLogCommentBackup_AddNewPassenger] b
                  WHERE b.[ActivityLogId] = al.[Id]);

PRINT N'Backed up rows: ' + CAST(@@ROWCOUNT AS NVARCHAR(20));
GO

-- -----------------------------------------------------------------------------
-- STEP 3: update the comments
-- -----------------------------------------------------------------------------
DECLARE @CardNoFallback NVARCHAR(50) = N'نامشخص';

BEGIN TRY
    BEGIN TRANSACTION;

    UPDATE al
    SET al.[Comment] = N'رهجو جدید با شماره رهایی ' + rf.[RecoveryNo] +
                       N' و شماره کارت ' + ISNULL(NULLIF(LTRIM(RTRIM(p.[CardNo])), N''), @CardNoFallback) +
                       N' افزوده شد'
    FROM [dbo].[ActivityLog] al
    INNER JOIN [dbo].[ActivityLogType] alt ON alt.[Id] = al.[ActivityLogTypeId]
    INNER JOIN [dbo].[RecoveryForm] rf ON rf.[Id] = al.[EntityId]
    LEFT JOIN [dbo].[Person] p ON p.[Id] = rf.[PersonId]
    WHERE alt.[SystemKeyword] = 'AddNewPassenger'
      AND al.[Comment] NOT LIKE N'%شماره رهایی%';

    PRINT N'Updated rows: ' + CAST(@@ROWCOUNT AS NVARCHAR(20));

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH
GO

-- -----------------------------------------------------------------------------
-- STEP 4: verify
-- -----------------------------------------------------------------------------
SELECT TOP (50) al.[Id], al.[EntityId], al.[Comment], al.[CreatedOnUtc]
FROM [dbo].[ActivityLog] al
INNER JOIN [dbo].[ActivityLogType] alt ON alt.[Id] = al.[ActivityLogTypeId]
WHERE alt.[SystemKeyword] = 'AddNewPassenger'
ORDER BY al.[Id] DESC;
GO

-- -----------------------------------------------------------------------------
-- STEP 5 (optional): rollback - restore the original comments
-- -----------------------------------------------------------------------------
--UPDATE al
--SET al.[Comment] = b.[OldComment]
--FROM [dbo].[ActivityLog] al
--INNER JOIN [dbo].[ActivityLogCommentBackup_AddNewPassenger] b ON b.[ActivityLogId] = al.[Id];
--GO

-- -----------------------------------------------------------------------------
-- STEP 6 (optional): rows whose EntityId is null - take the id from the comment text
-- Only run this if step 1c reported rows with "EntityId is null". It assumes the id
-- is the only digit sequence in the comment. Review the SELECT before the UPDATE.
-- -----------------------------------------------------------------------------
--DECLARE @CardNoFallback NVARCHAR(50) = N'نامشخص';
--
--WITH parsed AS (
--    SELECT
--        al.[Id]         AS [ActivityLogId],
--        al.[Comment]    AS [OldComment],
--        TRY_CAST(SUBSTRING(al.[Comment],
--            PATINDEX(N'%[0-9]%', al.[Comment]),
--            PATINDEX(N'%[^0-9]%', SUBSTRING(al.[Comment], PATINDEX(N'%[0-9]%', al.[Comment]), 4000) + N'X') - 1
--        ) AS INT) AS [ParsedRecoveryFormId]
--    FROM [dbo].[ActivityLog] al
--    INNER JOIN [dbo].[ActivityLogType] alt ON alt.[Id] = al.[ActivityLogTypeId]
--    WHERE alt.[SystemKeyword] = 'AddNewPassenger'
--      AND al.[EntityId] IS NULL
--      AND al.[Comment] NOT LIKE N'%شماره رهایی%'
--      AND PATINDEX(N'%[0-9]%', al.[Comment]) > 0
--)
--SELECT
--    x.[ActivityLogId], x.[ParsedRecoveryFormId], x.[OldComment],
--    [NewComment] = N'رهجو جدید با شماره رهایی ' + rf.[RecoveryNo] +
--                   N' و شماره کارت ' + ISNULL(NULLIF(LTRIM(RTRIM(p.[CardNo])), N''), @CardNoFallback) +
--                   N' افزوده شد'
--FROM parsed x
--INNER JOIN [dbo].[RecoveryForm] rf ON rf.[Id] = x.[ParsedRecoveryFormId]
--LEFT JOIN [dbo].[Person] p ON p.[Id] = rf.[PersonId];
--GO
