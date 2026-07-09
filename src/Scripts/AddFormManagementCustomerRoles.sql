-- =============================================
-- Adds two dedicated admin roles so that only users who are assigned to
-- them can access the corresponding form in the admin panel:
--   - مدیریت فرم رهایی      (SystemName: ReleaseFormManagers)      -> Passengers.* permissions (used by RecoveryFormController)
--   - مدیریت فرم انضباطی   (SystemName: DisciplinaryFormManagers) -> DisciplinaryForms.* permissions (used by DisciplinaryFormController)
--
-- No source code changes are required: access is already restricted per
-- permission via [CheckPermission] on the controllers and via PermissionNames
-- on the admin menu items (see AdminMenu.cs). Assign the resulting role(s)
-- to a user from Admin > Customers > (edit user) > Customer roles.
--
-- Safe to re-run: every insert/mapping is guarded with an existence check
-- and the whole script runs inside a transaction that rolls back on error.
-- =============================================

BEGIN TRANSACTION;

BEGIN TRY
    -- =============================================
    -- STEP 1: Create the CustomerRole rows (if they don't already exist)
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM [CustomerRole] WHERE [SystemName] = N'ReleaseFormManagers')
    BEGIN
        INSERT INTO [CustomerRole]
            ([Name], [FreeShipping], [TaxExempt], [Active], [IsSystemRole], [SystemName], [EnablePasswordLifetime], [OverrideTaxDisplayType], [DefaultTaxDisplayTypeId], [PurchasedWithProductId])
        VALUES
            (N'مدیریت فرم رهایی', 0, 0, 1, 0, N'ReleaseFormManagers', 0, 0, 0, 0);

        PRINT 'Created customer role: مدیریت فرم رهایی (ReleaseFormManagers)';
    END
    ELSE
        PRINT 'Customer role ReleaseFormManagers already exists, skipping insert.';

    IF NOT EXISTS (SELECT 1 FROM [CustomerRole] WHERE [SystemName] = N'DisciplinaryFormManagers')
    BEGIN
        INSERT INTO [CustomerRole]
            ([Name], [FreeShipping], [TaxExempt], [Active], [IsSystemRole], [SystemName], [EnablePasswordLifetime], [OverrideTaxDisplayType], [DefaultTaxDisplayTypeId], [PurchasedWithProductId])
        VALUES
            (N'مدیریت فرم انضباطی', 0, 0, 1, 0, N'DisciplinaryFormManagers', 0, 0, 0, 0);

        PRINT 'Created customer role: مدیریت فرم انضباطی (DisciplinaryFormManagers)';
    END
    ELSE
        PRINT 'Customer role DisciplinaryFormManagers already exists, skipping insert.';

    -- =============================================
    -- STEP 2: Make sure the permission records we need already exist
    -- (they are normally seeded by the application on startup; this is
    -- just a safety net for environments where that hasn't run yet)
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM [PermissionRecord] WHERE [SystemName] = N'Security.AccessAdminPanel')
    BEGIN
        INSERT INTO [PermissionRecord] ([Name], [SystemName], [Category])
        VALUES (N'Access admin area', N'Security.AccessAdminPanel', N'Security');
    END

    IF NOT EXISTS (SELECT 1 FROM [PermissionRecord] WHERE [SystemName] = N'Passengers.PassengersView')
    BEGIN
        INSERT INTO [PermissionRecord] ([Name], [SystemName], [Category])
        VALUES (N'Admin area. Passengers. View', N'Passengers.PassengersView', N'Passengers');
    END

    IF NOT EXISTS (SELECT 1 FROM [PermissionRecord] WHERE [SystemName] = N'Passengers.PassengersCreateEditDelete')
    BEGIN
        INSERT INTO [PermissionRecord] ([Name], [SystemName], [Category])
        VALUES (N'Admin area. Passengers. Create, edit, delete', N'Passengers.PassengersCreateEditDelete', N'Passengers');
    END

    IF NOT EXISTS (SELECT 1 FROM [PermissionRecord] WHERE [SystemName] = N'DisciplinaryForms.DisciplinaryFormsView')
    BEGIN
        INSERT INTO [PermissionRecord] ([Name], [SystemName], [Category])
        VALUES (N'Admin area. Disciplinary forms. View', N'DisciplinaryForms.DisciplinaryFormsView', N'DisciplinaryForms');
    END

    IF NOT EXISTS (SELECT 1 FROM [PermissionRecord] WHERE [SystemName] = N'DisciplinaryForms.DisciplinaryFormsCreateEditDelete')
    BEGIN
        INSERT INTO [PermissionRecord] ([Name], [SystemName], [Category])
        VALUES (N'Admin area. Disciplinary forms. Create, edit, delete', N'DisciplinaryForms.DisciplinaryFormsCreateEditDelete', N'DisciplinaryForms');
    END

    -- =============================================
    -- STEP 3: Map permissions to the new roles
    -- =============================================

    -- مدیریت فرم رهایی -> can log into admin + view/create/edit/delete Passengers (Recovery form)
    INSERT INTO [PermissionRecord_Role_Mapping] ([CustomerRole_Id], [PermissionRecord_Id])
    SELECT r.[Id], pr.[Id]
    FROM [CustomerRole] r
    CROSS JOIN [PermissionRecord] pr
    WHERE r.[SystemName] = N'ReleaseFormManagers'
      AND pr.[SystemName] IN (
          N'Security.AccessAdminPanel',
          N'Passengers.PassengersView',
          N'Passengers.PassengersCreateEditDelete'
      )
      AND NOT EXISTS (
          SELECT 1
          FROM [PermissionRecord_Role_Mapping] m
          WHERE m.[CustomerRole_Id] = r.[Id] AND m.[PermissionRecord_Id] = pr.[Id]
      );

    PRINT 'Mapped Passengers.* + AccessAdminPanel permissions to ReleaseFormManagers.';

    -- مدیریت فرم انضباطی -> can log into admin + view/create/edit/delete Disciplinary forms
    INSERT INTO [PermissionRecord_Role_Mapping] ([CustomerRole_Id], [PermissionRecord_Id])
    SELECT r.[Id], pr.[Id]
    FROM [CustomerRole] r
    CROSS JOIN [PermissionRecord] pr
    WHERE r.[SystemName] = N'DisciplinaryFormManagers'
      AND pr.[SystemName] IN (
          N'Security.AccessAdminPanel',
          N'DisciplinaryForms.DisciplinaryFormsView',
          N'DisciplinaryForms.DisciplinaryFormsCreateEditDelete'
      )
      AND NOT EXISTS (
          SELECT 1
          FROM [PermissionRecord_Role_Mapping] m
          WHERE m.[CustomerRole_Id] = r.[Id] AND m.[PermissionRecord_Id] = pr.[Id]
      );

    PRINT 'Mapped DisciplinaryForms.* + AccessAdminPanel permissions to DisciplinaryFormManagers.';

    COMMIT TRANSACTION;

    PRINT '=============================================';
    PRINT 'Transaction committed successfully!';
    PRINT 'Next step: assign these roles to users from';
    PRINT 'Admin > Customers > (edit user) > Customer roles.';
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
