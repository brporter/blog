CREATE TABLE [dbo].[posts] (
    [postid]   INT             IDENTITY (1, 1) NOT NULL,
    [title]    NVARCHAR (1024) NOT NULL,
    [body]     NVARCHAR (MAX)  NOT NULL,
    [created]  DATETIME        NOT NULL,
    [modified] DATETIME        NOT NULL,
    [enabled]  BIT             CONSTRAINT [DF_posts_enabled] DEFAULT ((1)) NOT NULL,
    [userid]   INT             NOT NULL,
    CONSTRAINT [PK_posts] PRIMARY KEY CLUSTERED ([postid] ASC),
    CONSTRAINT [FK_posts_users] FOREIGN KEY ([userid]) REFERENCES [dbo].[users] ([userid])
);


GO
CREATE NONCLUSTERED INDEX [IX_posts_userid_created]
    ON [dbo].[posts]([userid] ASC, [created] DESC);

