CREATE TABLE [dbo].[WorkOpen] (
    [ContractorId] INT NOT NULL,
    [workCycle]    INT NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [WorkOpenI1]
    ON [dbo].[WorkOpen]([ContractorId] ASC, [workCycle] ASC);

