CREATE TABLE [dbo].[Payment] (
    [payId]        INT            IDENTITY (1, 1) NOT NULL,
    [contractorId] INT            NOT NULL,
    [jobId]        INT            NOT NULL,
    [payDate]      DATETIME       NOT NULL,
    [checkNo]      INT            NOT NULL,
    [hours]        NUMERIC (6, 2) NOT NULL,
    [workDay]      NUMERIC (6, 2) DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED ([payId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Payment]
    ON [dbo].[Payment]([contractorId] ASC, [jobId] ASC);

