using System.Collections.Generic;

namespace m_clippy.Models
{
    public class Mocks
    {
        private Dictionary<string, string> userIds;
        private string defaultUserId = "b6adb9a1-9f93-49b9-8793-d6f91d44e4a3";
        private string defaultClientId = "102532";

        public Mocks()
        {
            userIds = new Dictionary<string, string>();
            userIds.Add("30fa3852-3f8f-47ce-ad4a-72cbb6356d7f", "102530");
            userIds.Add("4f124c26-50ff-4775-96cb-d95360631e00", "102531");
            userIds.Add(defaultUserId, defaultClientId); // default
        }

        private string getClientId(string userId) {
            if (userIds.TryGetValue(userId, out var clientId)) {
                return clientId;
            } else {
                if (userId.Length == 6 && int.TryParse(userId, out var clientAsUserId) && clientAsUserId / 1000 == 102) {
                    return userId;
                }
                return defaultClientId;
            }
        }


        private User NewUser(string userId) {
            var clientId = getClientId(userId);
            var clientIdPoints = int.Parse(clientId);
            var defaultPoints = int.Parse(defaultClientId);
            var points = 9167;
            return new User
            {
                Id = userId,
                FirstName = "Franziska",
                LastName = "Muster",
                Cumulus = $"2099 354 {defaultPoints / 1000} {defaultPoints % 1000}",
                //Points = $"{points + 10 * (defaultPoints - clientIdPoints)}",
                Points = $"{clientIdPoints}",
                // ClientId = "102530",
                // ClientId = "102531",
                //ClientId = "102532",
                ClientId = clientId,
                Configured = true,
                Alerts = new Alerts()
            };
        }


        public User GetMockUserById(string userId)
        {
            var user = NewUser(userId);
            user.Alerts.Habits = true;
            user.Alerts.Location = true;
            user.Alerts.Allergies = true;

            user.Habits = new Habits
            {
                Bio = true,
                Casher = false,
                Halal = false,
                Vegan = false,
                Vegetarian = false
            };


            user.Locations = new Location
            {
                Exclusion1 = "China",
                Exclusion2 = null,

                National = 1,
                Regional = 2,
                Outside = 3
            };

            user.Allergies = new Allergies()

            {
                Matching = new List<string>()
                {
                    "MAPI_ALLERGENES_laktosefrei"
                }
            };


            return user;
        }

    }
}
