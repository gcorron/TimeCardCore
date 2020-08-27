CREATE TABLE [dbo].[JobStart] (
    [contractorId] INT            NOT NULL,
    [jobId]        INT            NOT NULL,
    [startDay]     NUMERIC (6, 2) NOT NULL,
    [closed]       BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_JobStart] PRIMARY KEY CLUSTERED ([contractorId] ASC, [jobId] ASC)
);



