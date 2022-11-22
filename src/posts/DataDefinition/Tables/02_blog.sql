IF OBJECT_ID('dbo.blogs') IS NULL BEGIN
CREATE TABLE [dbo].[blogs] (
    [blogid]      INT             IDENTITY (1, 1) NOT NULL,
    [slug]        NVARCHAR (1024) NOT NULL,
    [title]       NVARCHAR (1024) NOT NULL,
    [description] NVARCHAR (MAX)  NOT NULL,
    [created]     DATETIME        NOT NULL,
    [modified]    DATETIME        NOT NULL,
    [enabled]     BIT             CONSTRAINT [DF_blog_enabled] DEFAULT ((1)) NOT NULL,
    [userid]      INT             NOT NULL,
    CONSTRAINT [FK_blogs_users] FOREIGN KEY ([userid]) REFERENCES [dbo].[users] ([userid]),
    CONSTRAINT [UQ_blogs_slug] UNIQUE ([slug])
    );

CREATE NONCLUSTERED INDEX [IX_blogs_userid_created]
    ON [dbo].[blogs]([userid] ASC, [created] DESC);

CREATE NONCLUSTERED INDEX [IX_blogs_slug]
    ON [dbo].[blogs]([slug]);
END