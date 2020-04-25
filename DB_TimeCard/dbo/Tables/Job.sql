CREATE TABLE [dbo].[Job] (
    [jobId]     INT          IDENTITY (1, 1) NOT NULL,
    [clientId]  INT          NOT NULL,
    [projectId] INT          NOT NULL,
    [billType]  INT          NOT NULL,
    [active]    BIT          NOT NULL,
    [descr]     VARCHAR (50) NULL,
    CONSTRAINT [PK__Table__164AA1A8F50834A7] PRIMARY KEY CLUSTERED ([jobId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Job]
    ON [dbo].[Job]([clientId] ASC, [projectId] ASC, [billType] ASC);

