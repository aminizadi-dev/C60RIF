-- =============================================
-- SAFE VERSION: Script to replace "مشتری" with "کاربر" and "مشتریان" with "کاربران" 
-- in LocaleStringResource table for Persian language
-- This version uses TRANSACTION for safety
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
-- STEP 3: Preview changes (OPTIONAL - Run this first to see what will be changed)
-- =============================================
-- Uncomment the following query to preview changes before applying:
/*
SELECT 
    Id,
    ResourceName,
    ResourceValue AS [Current Value],
    REPLACE(
        REPLACE(ResourceValue, N'مشتریان', N'کاربران'),
        N'مشتری', N'کاربر'
    ) AS [New Value]
FROM [LocaleStringResource]
WHERE LanguageId = @LanguageId
  AND (
      ResourceValue LIKE N'%مشتریان%' 
      OR ResourceValue LIKE N'%مشتری%'
  )
ORDER BY ResourceName;
*/

-- =============================================
-- STEP 4: Apply the replacements (WITH TRANSACTION)
-- =============================================
-- This version uses TRANSACTION so you can ROLLBACK if something goes wrong

BEGIN TRANSACTION;

BEGIN TRY
    -- Replace "مشتریان" with "کاربران" (customers -> users)
    -- IMPORTANT: Do this FIRST to avoid partial replacement
    UPDATE [LocaleStringResource]
    SET [ResourceValue] = REPLACE([ResourceValue], N'مشتریان', N'کاربران')
    WHERE [LanguageId] = @LanguageId
      AND [ResourceValue] LIKE N'%مشتریان%';

    DECLARE @RowsUpdated1 INT = @@ROWCOUNT;
    PRINT 'Replaced "مشتریان" with "کاربران": ' + CAST(@RowsUpdated1 AS NVARCHAR(10)) + ' row(s) updated.';

    -- Replace "مشتری" with "کاربر" (customer -> user)
    UPDATE [LocaleStringResource]
    SET [ResourceValue] = REPLACE([ResourceValue], N'مشتری', N'کاربر')
    WHERE [LanguageId] = @LanguageId
      AND [ResourceValue] LIKE N'%مشتری%';

    DECLARE @RowsUpdated2 INT = @@ROWCOUNT;
    PRINT 'Replaced "مشتری" with "کاربر": ' + CAST(@RowsUpdated2 AS NVARCHAR(10)) + ' row(s) updated.';

    -- If everything is OK, commit the transaction
    COMMIT TRANSACTION;
    
    PRINT '=============================================';
    PRINT 'Transaction committed successfully!';
    PRINT 'Total rows updated: ' + CAST(@RowsUpdated1 + @RowsUpdated2 AS NVARCHAR(10));
    PRINT '=============================================';
    PRINT 'Please verify the changes and clear the cache:';
    PRINT '1. Go to Admin Panel > Configuration > Maintenance > Clear Cache';
    PRINT '2. Or delete cache files from App_Data folder';
    PRINT '=============================================';

END TRY
BEGIN CATCH
    -- If there's an error, rollback the transaction
    ROLLBACK TRANSACTION;
    
    PRINT '=============================================';
    PRINT 'ERROR: Transaction rolled back!';
    PRINT 'Error Message: ' + ERROR_MESSAGE();
    PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS NVARCHAR(10));
    PRINT 'Error Line: ' + CAST(ERROR_LINE() AS NVARCHAR(10));
    PRINT '=============================================';
    
    THROW;
END CATCH;

GO

