CREATE TABLE [dbo].[AppRole] (
    [roleId]   INT          IDENTITY (1, 1) NOT NULL,
    [roleName] VARCHAR (50) NULL,
    CONSTRAINT [PK_AppRole] PRIMARY KEY CLUSTERED ([roleId] ASC)
);

