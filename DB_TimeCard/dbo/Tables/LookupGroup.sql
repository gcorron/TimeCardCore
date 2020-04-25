CREATE TABLE [dbo].[LookupGroup] (
    [GroupId] INT          IDENTITY (1, 1) NOT NULL,
    [Descr]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_LookupGroup] PRIMARY KEY CLUSTERED ([GroupId] ASC)
);

