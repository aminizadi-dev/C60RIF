-- =============================================
-- ROLLBACK Script: Convert "کاربر" back to "مشتری" and "کاربران" back to "مشتریان"
-- Use this script if you need to undo the replacement
-- =============================================

-- =============================================
-- STEP 1: Find your Persian Language ID
-- =============================================
-- Uncomment and run this query first to find your Persian language ID:
-- SELECT Id, Name, LanguageCulture FROM [Language] WHERE LanguageCulture LIKE '%fa%' OR Name LIKE '%Persian%' OR Name LIKE '%Farsi%';

-- =============================================
-- STEP 2: Set the Language ID variable
-- =============================================
-- Replace the number below with your Persian language ID from Step 1
DECLARE @LanguageId INT = 2; -- TODO: Replace 2 with your actual Persian language ID

-- =============================================
-- STEP 3: Preview rollback changes (OPTIONAL)
-- =============================================
-- Uncomment the following query to preview rollback changes:
/*
SELECT 
    Id,
    ResourceName,
    ResourceValue AS [Current Value],
    REPLACE(
        REPLACE(ResourceValue, N'کاربران', N'مشتریان'),
        N'کاربر', N'مشتری'
    ) AS [Rollback Value]
FROM [LocaleStringResource]
WHERE LanguageId = @LanguageId
  AND (
      ResourceValue LIKE N'%کاربران%' 
      OR ResourceValue LIKE N'%کاربر%'
  )
ORDER BY ResourceName;
*/

-- =============================================
-- STEP 4: Apply rollback (WITH TRANSACTION)
-- =============================================

BEGIN TRANSACTION;

BEGIN TRY
    -- Replace "کاربران" back to "مشتریان" (users -> customers)
    -- IMPORTANT: Do this FIRST to avoid partial replacement
    UPDATE [LocaleStringResource]
    SET [ResourceValue] = REPLACE([ResourceValue], N'کاربران', N'مشتریان')
    WHERE [LanguageId] = @LanguageId
      AND [ResourceValue] LIKE N'%کاربران%';

    DECLARE @RowsUpdated1 INT = @@ROWCOUNT;
    PRINT 'Replaced "کاربران" back to "مشتریان": ' + CAST(@RowsUpdated1 AS NVARCHAR(10)) + ' row(s) updated.';

    -- Replace "کاربر" back to "مشتری" (user -> customer)
    UPDATE [LocaleStringResource]
    SET [ResourceValue] = REPLACE([ResourceValue], N'کاربر', N'مشتری')
    WHERE [LanguageId] = @LanguageId
      AND [ResourceValue] LIKE N'%کاربر%';

    DECLARE @RowsUpdated2 INT = @@ROWCOUNT;
    PRINT 'Replaced "کاربر" back to "مشتری": ' + CAST(@RowsUpdated2 AS NVARCHAR(10)) + ' row(s) updated.';

    -- If everything is OK, commit the transaction
    COMMIT TRANSACTION;
    
    PRINT '=============================================';
    PRINT 'Rollback completed successfully!';
    PRINT 'Total rows updated: ' + CAST(@RowsUpdated1 + @RowsUpdated2 AS NVARCHAR(10));
    PRINT '=============================================';
    PRINT 'Please clear the cache to see the changes:';
    PRINT '1. Go to Admin Panel > Configuration > Maintenance > Clear Cache';
    PRINT '2. Or delete cache files from App_Data folder';
    PRINT '=============================================';

END TRY
BEGIN CATCH
    -- If there's an error, rollback the transaction
    ROLLBACK TRANSACTION;
    
    PRINT '=============================================';
    PRINT 'ERROR: Rollback transaction failed!';
    PRINT 'Error Message: ' + ERROR_MESSAGE();
    PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS NVARCHAR(10));
    PRINT 'Error Line: ' + CAST(ERROR_LINE() AS NVARCHAR(10));
    PRINT '=============================================';
    
    THROW;
END CATCH;

GO

