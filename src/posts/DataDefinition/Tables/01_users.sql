IF OBJECT_ID('dbo.users') IS NULL
CREATE TABLE [dbo].[users] (
    [userid]       INT             IDENTITY (1, 1) NOT NULL,
    [emailaddress] NVARCHAR (2048) NOT NULL,
    [handle]       NVARCHAR (2048) NOT NULL,
    CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED ([userid] ASC)
);

