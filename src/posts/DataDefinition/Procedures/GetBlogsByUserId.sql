CREATE PROCEDURE [dbo].GetBlogsByUserId
(
    @UserId INT
)
AS
SELECT
    blogid,
    slug,
    title,
    description,
    created,
    modified,
    enabled,
    userid
FROM
    dbo.blogs
WHERE
    userid = @UserId
ORDER BY created DESC

RETURN 0
