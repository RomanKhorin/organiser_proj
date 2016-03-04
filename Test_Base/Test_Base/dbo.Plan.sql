CREATE TABLE [dbo].[Plan]
(
	[description] VARCHAR(MAX) NOT NULL PRIMARY KEY, 
    [is_completed] BIT NOT NULL, 
    CONSTRAINT [FK_Plan_User] FOREIGN KEY ([description]) REFERENCES [User]([login])
)
