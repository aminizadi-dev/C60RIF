-- =============================================
-- Backfill Passenger.CreatedByCustomerId from ActivityLog (run after InsertPassengerActivityLogTypes.sql)
-- =============================================

UPDATE p
SET p.[CreatedByCustomerId] = x.[CustomerId]
FROM [dbo].[Passenger] p
INNER JOIN (
    SELECT al.[EntityId], MIN(al.[CustomerId]) AS [CustomerId]
    FROM [dbo].[ActivityLog] al
    INNER JOIN [dbo].[ActivityLogType] alt ON al.[ActivityLogTypeId] = alt.[Id]
    WHERE alt.[SystemKeyword] = 'AddNewPassenger'
      AND al.[EntityName] = 'Passenger'
      AND al.[EntityId] IS NOT NULL
    GROUP BY al.[EntityId]
) x ON p.[Id] = x.[EntityId]
WHERE p.[CreatedByCustomerId] IS NULL;
GO
