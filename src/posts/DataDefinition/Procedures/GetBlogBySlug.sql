CREATE PROCEDURE [dbo].GetBlogBySlug
(
    @Slug NVARCHAR(1024)
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
    slug = @Slug
ORDER BY created DESC

RETURN 0
