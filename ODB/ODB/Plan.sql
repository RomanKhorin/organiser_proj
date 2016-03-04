CREATE TABLE [dbo].[Plan] ( 
[User_login] VARCHAR (50) NOT NULL, 
[Description] VARCHAR (140) NOT NULL, 
[IsCompleted] VARCHAR (1) NOT NULL, 
PRIMARY KEY CLUSTERED ([Description] ASC), 
CONSTRAINT [User_login] FOREIGN KEY ([User_login]) REFERENCES [dbo].[User] ([Login]) 
);
