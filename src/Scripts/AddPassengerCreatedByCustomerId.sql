-- =============================================
-- Add CreatedByCustomerId column to Passenger table
-- =============================================

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[Passenger]') AND name = 'CreatedByCustomerId')
BEGIN
    ALTER TABLE [dbo].[Passenger]
    ADD [CreatedByCustomerId] INT NULL;

    IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type = N'U')
    BEGIN
        ALTER TABLE [dbo].[Passenger] WITH CHECK ADD CONSTRAINT [FK_Passenger_Customer_CreatedBy]
        FOREIGN KEY([CreatedByCustomerId]) REFERENCES [dbo].[Customer] ([Id]);

        ALTER TABLE [dbo].[Passenger] CHECK CONSTRAINT [FK_Passenger_Customer_CreatedBy];
    END
END
GO
