-- Caleb Marks
-- Create book inventory database before scaffolding

IF DB_ID('CCAD23BookInventory') IS NULL
BEGIN
    CREATE DATABASE CCAD23BookInventory;
END
GO

USE CCAD23BookInventory;
GO

-- ISBN serves as primary key
IF OBJECT_ID('dbo.Book') IS NULL
BEGIN
    CREATE TABLE dbo.Book
    (
        Isbn nvarchar(13) NOT NULL PRIMARY KEY,
        Name nvarchar(200) NOT NULL,
        AuthorName nvarchar(200) NOT NULL,
        Description nvarchar(max) NULL,
        Quantity int NOT NULL
    );

    INSERT INTO dbo.Book (Isbn, Name, AuthorName, Description, Quantity)
    VALUES
        ('9780135957059', 'The Pragmatic Programmer', 'David Thomas', 'Practical habits for software developers.', 3),
        ('9780132350884', 'Clean Code', 'Robert C. Martin', 'Guidance on writing readable code.', 2),
        ('9780440412670', 'Where the Red Fern Grows', 'Wilson Rawls', 'A boy and two hounds in the Oklahoma Ozarks.', 5);
END
GO
