-- =============================================
-- Add CurrentSubstanceUseDetails column to DisciplinaryForm table
-- =============================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DisciplinaryForm]') AND type = N'U')
   AND NOT EXISTS (
       SELECT 1 FROM sys.columns
       WHERE object_id = OBJECT_ID(N'[dbo].[DisciplinaryForm]') AND name = 'CurrentSubstanceUseDetails')
BEGIN
    ALTER TABLE [dbo].[DisciplinaryForm]
        ADD [CurrentSubstanceUseDetails] NVARCHAR(MAX) NULL;
END
GO
