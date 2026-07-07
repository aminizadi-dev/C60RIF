-- =============================================
-- Add AgencyId relation to DisciplinaryForm table
-- =============================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DisciplinaryForm]') AND type = N'U')
   AND NOT EXISTS (
       SELECT 1 FROM sys.columns
       WHERE object_id = OBJECT_ID(N'[dbo].[DisciplinaryForm]') AND name = 'AgencyId')
BEGIN
    ALTER TABLE [dbo].[DisciplinaryForm]
        ADD [AgencyId] INT NULL;

    IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agency]') AND type = N'U')
    BEGIN
        ALTER TABLE [dbo].[DisciplinaryForm] WITH CHECK ADD CONSTRAINT [FK_DisciplinaryForm_Agency]
        FOREIGN KEY([AgencyId]) REFERENCES [dbo].[Agency] ([Id]);

        ALTER TABLE [dbo].[DisciplinaryForm] CHECK CONSTRAINT [FK_DisciplinaryForm_Agency];
    END
END

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DisciplinaryForm]') AND name = 'AgencyId')
   AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DisciplinaryForm]') AND name = 'IX_DisciplinaryForm_AgencyId')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DisciplinaryForm_AgencyId]
    ON [dbo].[DisciplinaryForm] ([AgencyId] ASC);
END
GO
