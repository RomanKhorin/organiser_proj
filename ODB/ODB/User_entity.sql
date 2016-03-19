CREATE TABLE [dbo].[User_entity] ( 
[Login] VARCHAR (50) NOT NULL, 
[Password] VARCHAR (10) NOT NULL, 
[Name] VARCHAR(50) NOT NULL, 
[Surname] VARCHAR(50) NOT NULL, 
[Date_of_birth] DATETIME NOT NULL, 
PRIMARY KEY CLUSTERED ([Login] ASC) 
);
