-- =============================================
-- Script to create City table in SQL Server
-- =============================================

-- Check if table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[City]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[City](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](500) NOT NULL,
        [Published] [bit] NOT NULL,
        [DisplayOrder] [int] NOT NULL,
        CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([Id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY];

    -- Add index on Name for search functionality
    CREATE NONCLUSTERED INDEX [IX_City_Name] 
    ON [dbo].[City] ([Name] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    -- Add index on DisplayOrder for sorting
    CREATE NONCLUSTERED INDEX [IX_City_DisplayOrder] 
    ON [dbo].[City] ([DisplayOrder] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];

    PRINT 'City table created successfully.';
END
ELSE
BEGIN
    PRINT 'City table already exists.';
END
GO

