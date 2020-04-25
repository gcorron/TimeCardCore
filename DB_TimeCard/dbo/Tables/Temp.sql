CREATE TABLE [dbo].[Temp] (
    [Cycle]       FLOAT (53)     NULL,
    [Day]         FLOAT (53)     NULL,
    [Date]        DATETIME       NULL,
    [Client]      NVARCHAR (255) NULL,
    [Project]     NVARCHAR (255) NULL,
    [Hold]        FLOAT (53)     NULL,
    [Time]        FLOAT (53)     NULL,
    [Description] NVARCHAR (255) NULL,
    [Paid]        NVARCHAR (255) NULL,
    [F10]         NVARCHAR (255) NULL,
    [F11]         NVARCHAR (255) NULL,
    [billType]    VARCHAR (50)   NULL,
    [workType]    VARCHAR (50)   NULL,
    [clientid]    INT            NULL,
    [projectid]   INT            NULL,
    [jobid]       INT            NULL,
    [billtypeid]  INT            NULL,
    [worktypeid]  INT            NULL
);

