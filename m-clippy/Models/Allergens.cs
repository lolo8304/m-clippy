using System;
using System.Collections.Generic;
using m_clippy.Models.Migros;

namespace m_clippy.Models
{
    public class AllergenList
    {
        public List<AllergenEntry> list { get; set; }

        public AllergenList()
        {
            list = new List<AllergenEntry>();
        }
    }

}
