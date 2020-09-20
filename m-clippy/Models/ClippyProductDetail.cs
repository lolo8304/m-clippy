using System;
using System.Collections.Generic;
using m_clippy.Models.Migros;

namespace m_clippy.Models
{
    public class ClippyProductDetail
    {
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public string Original { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }
        public string Quantity { get; set; }

        public string ArticleID { get; set; }
        

        public bool LocationAlert { get; set; }
        public bool HabitsAlert { get; set; }
        public bool AllergyAlert { get; set; }

        public ClippyProductDetail()
        {
        }
    }

}
