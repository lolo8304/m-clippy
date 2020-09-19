using System;
using System.Collections.Generic;


namespace m_clippy.Models
{
    public class ClippyProductsDetails
    {
        public List<ClippyProductDetail> list { get; set; }

        // 0 to 100% target
        public int Score { get; set; }

        public int HabitsCounter { get; set; }
        public int LocationCounter { get; set; }
        public int AllergyCounter { get; set; }


        public int CountriesCounter { get; set; }
        public string PlanesKm { get; set; }
        public string CarKm { get; set; }

        public int NotVeganCounter { get; set; }
        public int VeganCounter { get; set; }

        public int NotVegetarianCounter { get; set; }
        public int VegetarianCounter { get; set; }

        public int NotBioCounter { get; set; }
        public int BioCounter { get; set; }

        public int NoAllergensCounter { get; set; }
        public int AllergensCounter { get; set; }

        public double NationalSum { get; set; }
        public double RegionalSum { get; set; }
        public double OutsideSum { get; set; }

        public Dictionary<string, int> allergens { get; set; }

        public ClippyProductsDetails()
        {
            list = new List<ClippyProductDetail>();
            allergens = new Dictionary<string, int>();
        }
    }

}
