-- =============================================
-- Script to create AntiX table in SQL Server
-- =============================================

-- Check if table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AntiX]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AntiX](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](500) NOT NULL,
        [Published] [bit] NOT NULL,
        [DisplayOrder] [int] NOT NULL,
        CONSTRAINT [PK_AntiX] PRIMARY KEY CLUSTERED ([Id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY];

    -- Add index on Name for search functionality
    CREATE NONCLUSTERED INDEX [IX_AntiX_Name] 
    ON [dbo].[AntiX] ([Name] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on DisplayOrder for sorting
    CREATE NONCLUSTERED INDEX [IX_AntiX_DisplayOrder] 
    ON [dbo].[AntiX] ([DisplayOrder] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    PRINT 'AntiX table created successfully.';
END
ELSE
BEGIN
    PRINT 'AntiX table already exists.';
END
GO

