using System;
using System.Collections.Generic;


namespace m_clippy.Models
{
    public class ClippyProductsDetails
    {
        public List<ClippyProductDetail> list { get; set; }

        // 0 to 100% target
        public int GreenScore { get; set; }

        public int HabitsCounter { get; set; }
        public int LocationCounter { get; set; }
        public int AllergyCounter { get; set; }

        public ClippyProductsDetails()
        {
            list = new List<ClippyProductDetail>();
        }
    }

}
