using System;
namespace m_clippy.Models
{
    public class User
    {
        public Habits Habits { get; set; }
        public Location Location { get; set; }
        public Allergies Allergies { get; set; }
        public Alerts Alerts { get; set; }

        public User()
        {
        }
    }
}
