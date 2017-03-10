namespace ColossalCave.Webhook.Models
{
    public class BasicAuthorizationModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsEnabled { get; set; }
    }
}
