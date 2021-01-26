CREATE TABLE [dbo].[Users](
	[Id]		[int]			NULL,
	[Username]	[nchar](255)	NULL,
	[Password]	[nchar](255)	NULL,
	[ClientId]	[int]			DEFAULT(0),
	[SessionId] [int]			DEFAULT(0),
	PRIMARY KEY(Id),
	CONSTRAINT FK_Users_SessionId FOREIGN KEY (SessionId) 
	REFERENCES Persons(SessionId)
) ON [PRIMARY]
GO