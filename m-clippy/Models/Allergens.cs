using System.Collections.Generic;

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
