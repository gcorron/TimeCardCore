CREATE TABLE [dbo].[Work] (
    [workId]       INT            IDENTITY (1, 1) NOT NULL,
    [contractorId] INT            NOT NULL,
    [jobId]        INT            NOT NULL,
    [workDay]      NUMERIC (6, 2) NOT NULL,
    [descr]        VARCHAR (250)  NOT NULL,
    [hours]        NUMERIC (4, 2) NOT NULL,
    [workType]     INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Work] PRIMARY KEY CLUSTERED ([workId] ASC)
);

