CREATE TABLE [dbo].[Lookup] (
    [id]      INT          IDENTITY (1, 1) NOT NULL,
    [groupId] INT          NOT NULL,
    [descr]   VARCHAR (50) NOT NULL,
    [val]     VARCHAR (20) CONSTRAINT [DF_Lookup_val] DEFAULT ('') NOT NULL,
    [active]  BIT          CONSTRAINT [DF_Lookup_active] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Lookup] PRIMARY KEY CLUSTERED ([id] ASC)
);



