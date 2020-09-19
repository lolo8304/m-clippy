using System;
using System.Collections.Generic;

namespace m_clippy.Models
{
    public class Location
    {
        public int Regional { get; set; }
        public int National { get; set; }
        public int Outside { get; set; }
        public String Exclusion1 { get; set; }
        public String Exclusion2 { get; set; }

        public Location()
        {
        }
    }
}
