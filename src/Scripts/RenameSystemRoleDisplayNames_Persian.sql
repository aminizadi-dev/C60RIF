-- =============================================
-- Renames the DISPLAY name of built-in system customer roles to Persian
-- (e.g. "Administrators" is shown as "ادمین" everywhere in the admin UI:
-- role list, ACL screen, and the "Customer roles" picker on the user edit page).
--
-- IMPORTANT: this script only updates the [Name] column, never [SystemName].
-- All application logic (NopCustomerDefaults.AdministratorsRoleName,
-- IsAdminAsync, permission seeding, etc.) looks roles up by [SystemName],
-- so changing [Name] is purely cosmetic and cannot break anything.
-- ([SystemName] itself is also protected from edits for system roles by
-- CustomerRoleController - see "CantEditSystem" validation.)
--
-- Feel free to edit the literal Persian text below before running.
-- Safe to re-run: it just re-applies the same UPDATE.
-- =============================================

-- =============================================
-- STEP 1 (OPTIONAL): Preview current role names before changing them
-- =============================================
/*
SELECT [Id], [Name], [SystemName], [IsSystemRole]
FROM [CustomerRole]
WHERE [SystemName] IN (N'Administrators', N'ForumModerators', N'Registered', N'Guests', N'Vendors')
ORDER BY [Id];
*/

BEGIN TRANSACTION;

BEGIN TRY
    UPDATE [CustomerRole] SET [Name] = N'ادمین' WHERE [SystemName] = N'Administrators';
    PRINT 'Administrators -> ادمین: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' row(s) updated.';

    -- In this fork, ForumModerators has been repurposed to bundle
    -- Passengers + DisciplinaryForms + Reports permissions (see
    -- DefaultPermissionConfigManager.cs), so it is displayed as "ناظر فرم‌ها".
    -- Change the literal below if you'd prefer different wording.
    UPDATE [CustomerRole] SET [Name] = N'ناظر فرم‌ها' WHERE [SystemName] = N'ForumModerators';
    PRINT 'ForumModerators -> ناظر فرم‌ها: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' row(s) updated.';

    UPDATE [CustomerRole] SET [Name] = N'کاربر عضو' WHERE [SystemName] = N'Registered';
    PRINT 'Registered -> کاربر عضو: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' row(s) updated.';

    UPDATE [CustomerRole] SET [Name] = N'مهمان' WHERE [SystemName] = N'Guests';
    PRINT 'Guests -> مهمان: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' row(s) updated.';

    UPDATE [CustomerRole] SET [Name] = N'فروشنده' WHERE [SystemName] = N'Vendors';
    PRINT 'Vendors -> فروشنده: ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' row(s) updated.';

    COMMIT TRANSACTION;

    PRINT '=============================================';
    PRINT 'Transaction committed successfully!';
    PRINT 'Please clear the cache afterwards:';
    PRINT '  Admin Panel > Configuration > Maintenance > Clear Cache';
    PRINT '=============================================';
END TRY
BEGIN CATCH
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
