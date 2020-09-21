using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace m_clippy.Models.Purchase
{
    // generated with https://json2csharp.com/

    public class Purchase
    {
        [JsonProperty("einkaufID")]
        public string EinkaufID;

        [JsonProperty("kundeID")]
        public int KundeID;

        [JsonProperty("profitKstID")]
        public int ProfitKstID;

        [JsonProperty("profitKstNameDe")]
        public string ProfitKstNameDe;

        [JsonProperty("genossenschaftsCode")]
        public string GenossenschaftsCode;

        [JsonProperty("time")]
        public DateTime Time;
    }

    
    public class Purchases
    {
        [JsonProperty("Purchase")]
        public List<Purchase> purchases;
    }


}
