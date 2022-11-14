CREATE PROCEDURE [dbo].GetPostsByHandle
    @Handle NVARCHAR(2048),
    @PageNumber INT,
    @RowsPerPage INT
AS
    SELECT 
        posts.postid, 
        posts.title, 
        posts.body, 
        posts.created, 
        posts.modified, 
        posts.enabled, 
        posts.userid 
    FROM 
        dbo.posts 
        INNER JOIN dbo.users ON dbo.users.handle = @Handle 
    ORDER BY created DESC 
OFFSET( (@PageNumber - 1) * @RowsPerPage ) ROWS FETCH NEXT @RowsPerPage ROWS ONLY

RETURN 0
