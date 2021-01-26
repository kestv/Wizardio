using System;

namespace WizardioApi.Models
{
    public class GameSessions
    {
        public int Id { get; set; }
        public int MaxPlayers { get; set; }
        public int PlayersCount { get; set; }
        public DateTime Start { get; set; }
        public bool IsStarted { get; set; }
        public bool IsEnded { get; set; }
    }
}
