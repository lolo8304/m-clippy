using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace m_clippy.Models.Cart
{
    // generated with https://json2csharp.com/

    public class CartItem
    {
        [JsonProperty("artikelID")]
        public object ArtikelID;

        [JsonProperty("menge")]
        public int Menge;
    }

    public class Cart
    {
        [JsonProperty("MyArray")]
        public List<CartItem> cartItems;
    }



}
