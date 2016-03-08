CREATE TABLE [dbo].[Plan] ( 
[User_login] VARCHAR (50) NOT NULL, 
[Description] VARCHAR (140) NOT NULL, 
[IsCompleted] VARCHAR (1) NOT NULL, 
[DeadLine] DATETIME NOT NULL, 
CONSTRAINT [PK_Plan] PRIMARY KEY CLUSTERED ([Description] ASC, [DeadLine] ASC), 
CONSTRAINT [User_login] FOREIGN KEY ([User_login]) REFERENCES [dbo].[User] ([Login]) 
);
