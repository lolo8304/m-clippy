using System;
using System.Collections.Generic;

namespace m_clippy.Models
{
    public class Mocks
    {
        public Mocks()
        {
        }


        public User User1()
        {
            var user = new User
            {
                Id = "b6adb9a1-9f93-49b9-8793-d6f91d44e4a3",
                FirstName = "Franziska",
                LastName = "Muster",
                Cumulus = "2099 354 963 435",
                Points = "9167",
                ClientId = "102531",

                Configured = false,

                Alerts = new Alerts()
            };
            user.Alerts.Habits = true;
            user.Alerts.Location = true;
            user.Alerts.Allergies = true;

            user.Habits = new Habits
            {
                Bio = true,
                Casher = true,
                Halal = true,
                Vegan = true,
                Vegetarian = true
            };


            user.Location = new Location
            {
                Exclusion1 = "China",
                Exclusion2 = null,

                National = 2,
                Regional = 1,
                Outside = 3
            };

            user.Allergies = new Allergies()

            {
                Matching = new List<string>()
                {
                    "MAPI_ALLERGENES_laktosefrei","MAPI_ALLERGENES_ohneNuesse","MAPI_ALLERGENES_ohneSoja"
                }
            };


            return user;
        }

    }
}
