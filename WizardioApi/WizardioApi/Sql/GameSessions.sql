CREATE TABLE [dbo].[GameSessions](
	[Id]			[int] IDENTITY(1,1),
	[MaxPlayers]	[int],
	[PlayersCount]	[int],
	[Start]			[date],
	[IsStarted]		[bit],
	[IsEnded]		[bit],
	PRIMARY KEY(Id)
) ON [PRIMARY]
