using System;

namespace Assets.Scripts.Api.Models
{
    public class GameSession
    {
        public int Id;
        public int MaxPlayers;
        public int PlayersCount;
        public DateTime Start;
        public bool IsStarted;
        public bool IsEnded;
    }
}
