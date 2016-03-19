CREATE TABLE [dbo].[Plan_entity] ( 
[Id] INT IDENTITY (1, 1) NOT NULL, 
[User_login] VARCHAR (50) NOT NULL, 
[Description] VARCHAR (140) NOT NULL, 
[IsCompleted] VARCHAR (1) NOT NULL, 
[DeadLine] DATE NOT NULL, 
[Date] DATETIME NOT NULL, 
CONSTRAINT [PK_Plan] PRIMARY KEY CLUSTERED ([Date] ASC, [Id] ASC), 
CONSTRAINT [Login] FOREIGN KEY ([User_login]) REFERENCES [dbo].[User] ([Login]) 
);