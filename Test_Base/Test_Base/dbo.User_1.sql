CREATE TABLE [dbo].[User]
(
	[login] VARCHAR(MAX) NOT NULL PRIMARY KEY, 
    [name] VARCHAR(50) NOT NULL, 
    [surname] VARCHAR(50) NOT NULL, 
    [password] VARCHAR(50) NOT NULL, 
    [date_of_birth] DATE NOT NULL
)
