namespace WizardioApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? ClientId { get; set; }
        public int? SessionId { get; set; }
    }
}
