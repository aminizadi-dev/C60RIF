-- =============================================
-- Grant DisciplinaryForm permissions to Administrators and Forum Moderators
-- Run after app startup/migration has seeded PermissionRecord rows, or use the INSERT block below.
-- =============================================

-- Seed permission records if missing (existing installs may not have them yet)
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

-- Administrators (role id 1) and Forum Moderators (role id 2)
DECLARE @RoleIds TABLE (RoleId INT);
INSERT INTO @RoleIds (RoleId) VALUES (1), (2);

INSERT INTO [PermissionRecord_Role_Mapping] ([CustomerRole_Id], [PermissionRecord_Id])
SELECT r.RoleId, pr.[Id]
FROM @RoleIds r
CROSS JOIN [PermissionRecord] pr
WHERE pr.[SystemName] IN (
    N'DisciplinaryForms.DisciplinaryFormsView',
    N'DisciplinaryForms.DisciplinaryFormsCreateEditDelete'
)
  AND NOT EXISTS (
      SELECT 1
      FROM [PermissionRecord_Role_Mapping] m
      WHERE m.[CustomerRole_Id] = r.RoleId AND m.[PermissionRecord_Id] = pr.[Id]
  );
GO
