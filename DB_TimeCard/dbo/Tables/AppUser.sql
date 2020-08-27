CREATE TABLE [dbo].[AppUser] (
    [userId]              INT            IDENTITY (1, 1) NOT NULL,
    [userName]            VARCHAR (20)   NOT NULL,
    [userFullName]        VARCHAR (50)   NOT NULL,
    [passwordHash]        BINARY (64)    NOT NULL,
    [lastLogin]           DATETIME       NULL,
    [loginAttempts]       INT            NOT NULL,
    [active]              BIT            NOT NULL,
    [reset]               BIT            CONSTRAINT [DF__AppUser__reset__540C7B00] DEFAULT ((0)) NOT NULL,
    [refreshTokenExpired] DATETIME       NULL,
    [refreshToken]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AppUser] PRIMARY KEY CLUSTERED ([userId] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IAppUser_UserName]
    ON [dbo].[AppUser]([userName] ASC);

