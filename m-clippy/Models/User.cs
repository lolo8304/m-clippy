namespace m_clippy.Models
{
    public class User
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string Cumulus { get; set; }
        public string Points { get; set; }
        public string Id { get; set; }
        public string ClientId { get; set; }

        public bool Configured { get; set; }

        public Habits Habits { get; set; }
        public Location Locations { get; set; }
        public Allergies Allergies { get; set; }
        public Alerts Alerts { get; set; }

        public User()
        {
        }
    }
}
