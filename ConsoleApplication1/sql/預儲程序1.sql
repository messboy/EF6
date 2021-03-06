SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.GetCourse
@p nvarchar(50)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
SELECT Course.CourseID, Course.Title, Course.Credits, Department.Name AS DepartmentName 
FROM Course INNER JOIN Department ON Course.DepartmentID = Department.DepartmentID
WHERE Course.Title like @p
END
GO