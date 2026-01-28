-- =============================================
-- Script to replace "مشتری" with "کاربر" and "مشتریان" with "کاربران" 
-- in LocaleStringResource table for Persian language
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
-- STEP 4: Apply the replacements
-- =============================================
-- IMPORTANT: 
-- 1. First replace "مشتریان" (customers - plural) to avoid partial replacement
-- 2. Then replace "مشتری" (customer - singular)
-- This ensures "مشتریان" is replaced correctly before "مشتری" is processed

-- Replace "مشتریان" with "کاربران" (customers -> users)
UPDATE [LocaleStringResource]
SET [ResourceValue] = REPLACE([ResourceValue], N'مشتریان', N'کاربران')
WHERE [LanguageId] = @LanguageId
  AND [ResourceValue] LIKE N'%مشتریان%';

PRINT 'Replaced "مشتریان" with "کاربران": ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' row(s) updated.';

-- Replace "مشتری" with "کاربر" (customer -> user)
UPDATE [LocaleStringResource]
SET [ResourceValue] = REPLACE([ResourceValue], N'مشتری', N'کاربر')
WHERE [LanguageId] = @LanguageId
  AND [ResourceValue] LIKE N'%مشتری%';

PRINT 'Replaced "مشتری" with "کاربر": ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' row(s) updated.';

-- =============================================
-- STEP 5: Verify the changes
-- =============================================
-- Uncomment to verify changes:
/*
SELECT 
    Id,
    ResourceName,
    ResourceValue
FROM [LocaleStringResource]
WHERE LanguageId = @LanguageId
  AND (
      ResourceValue LIKE N'%کاربران%' 
      OR ResourceValue LIKE N'%کاربر%'
  )
ORDER BY ResourceName;
*/

GO

PRINT '=============================================';
PRINT 'Replacement completed successfully!';
PRINT '=============================================';
PRINT 'Please verify the changes and clear the cache:';
PRINT '1. Go to Admin Panel > Configuration > Maintenance > Clear Cache';
PRINT '2. Or delete cache files from App_Data folder';
PRINT '=============================================';

