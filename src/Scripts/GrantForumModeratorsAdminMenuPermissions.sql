-- =============================================
-- Grant Forum Moderators (CustomerRole Id = 2) permissions for admin dashboard and performance report menu
-- =============================================

DECLARE @RoleId INT = 2;

IF NOT EXISTS (SELECT 1 FROM [PermissionRecord] WHERE [SystemName] = N'Reports.PassengerPerformanceView')
BEGIN
    INSERT INTO [PermissionRecord] ([Name], [SystemName], [Category])
    VALUES (N'Admin area. Reports. Passenger performance', N'Reports.PassengerPerformanceView', N'Reports');
END

INSERT INTO [PermissionRecord_Role_Mapping] ([CustomerRole_Id], [PermissionRecord_Id])
SELECT @RoleId, pr.[Id]
FROM [PermissionRecord] pr
WHERE pr.[SystemName] IN (
    N'Security.AccessAdminPanel',
    N'Reports.PassengerPerformanceView',
    N'Passengers.PassengersView'
)
  AND NOT EXISTS (
      SELECT 1
      FROM [PermissionRecord_Role_Mapping] m
      WHERE m.[CustomerRole_Id] = @RoleId AND m.[PermissionRecord_Id] = pr.[Id]
  );
GO
