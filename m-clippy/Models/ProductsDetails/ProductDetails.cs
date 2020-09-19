using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace m_clippy.Models.ProductDetails
{
    // generated with https://json2csharp.com/
    public class ProductDetails
    {
        [JsonProperty("total_hits")]
        public int TotalHits;

        [JsonProperty("facets")]
        public Facets Facets;

        [JsonProperty("products")]
        public List<ProductDetail> Products;
    }

    public class PurchasableOnline
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term> Terms;
    }


    public class Retailer
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term> Terms;
    }

    public class Discount
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<object> Terms;
    }

    public class Term
    {
        [JsonProperty("term")]
        public string ItsTerm;

        [JsonProperty("count")]
        public int Count;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("active")]
        public bool Active;

        [JsonProperty("slug")]
        public string Slug;
    }

    public class Label
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term> Terms;
    }

    public class Brand
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term> Terms;
    }

    public class Facets
    {
        [JsonProperty("purchasable_online")]
        public PurchasableOnline PurchasableOnline;

        [JsonProperty("retailer")]
        public Retailer Retailer;

        [JsonProperty("discount")]
        public Discount Discount;

        [JsonProperty("label")]
        public Label Label;

        [JsonProperty("brand")]
        public Brand Brand;
    }

    public class Image
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Brand2
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("slug")]
        public string Slug;

        [JsonProperty("image")]
        public Image Image;
    }

    public class Image2
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Label2
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("slug")]
        public string Slug;

        [JsonProperty("image")]
        public Image2 Image;
    }

    public class Image3
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Category
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("slug")]
        public string Slug;

        [JsonProperty("visible")]
        public bool Visible;

        [JsonProperty("parent_code")]
        public string ParentCode;

        [JsonProperty("image")]
        public Image3 Image;

        [JsonProperty("level")]
        public int Level;

        [JsonProperty("title")]
        public string Title;
    }

    public class Value
    {
        [JsonProperty("value_code")]
        public string ValueCode;

        [JsonProperty("value")]
        public string ItsValue;

        [JsonProperty("numeric_value")]
        public double? NumericValue;

        [JsonProperty("boolean_value")]
        public bool? BooleanValue;
    }

    public class Feature
    {
        [JsonProperty("label_code")]
        public string LabelCode;

        [JsonProperty("label")]
        public string Label;

        [JsonProperty("values")]
        public List<Value> Values;

        [JsonProperty("top_fact")]
        public bool TopFact;
    }

    public class Image4
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class ImageTransparent
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Value2
    {
        [JsonProperty("value_code")]
        public string ValueCode;

        [JsonProperty("value")]
        public string Value;

        [JsonProperty("numeric_value")]
        public double? NumericValue;
    }

    public class InternalFeature
    {
        [JsonProperty("label_code")]
        public string LabelCode;

        [JsonProperty("label")]
        public string Label;

        [JsonProperty("values")]
        public List<Value2> Values;

        [JsonProperty("top_fact")]
        public bool TopFact;
    }

    public class Vat
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("percentage")]
        public double Percentage;
    }

    public class MigrosCh
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("canonical")]
        public string Canonical;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("purchasable")]
        public bool Purchasable;
    }

    public class Migipedia
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("canonical")]
        public string Canonical;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("purchasable")]
        public bool Purchasable;
    }

    public class Links
    {
        [JsonProperty("migros_ch")]
        public MigrosCh MigrosCh;

        [JsonProperty("migipedia")]
        public Migipedia Migipedia;
    }

    public class Item
    {
        [JsonProperty("price")]
        public double Price;

        [JsonProperty("quantity")]
        public int Quantity;

        [JsonProperty("unit")]
        public string Unit;

        [JsonProperty("varying_quantity")]
        public bool VaryingQuantity;

        [JsonProperty("display_quantity")]
        public string DisplayQuantity;
    }

    public class Base
    {
        [JsonProperty("price")]
        public double Price;

        [JsonProperty("quantity")]
        public int Quantity;

        [JsonProperty("unit")]
        public string Unit;
    }

    public class Price
    {
        [JsonProperty("valid_from")]
        public DateTime ValidFrom;

        [JsonProperty("valid_to")]
        public DateTime ValidTo;

        [JsonProperty("currency")]
        public string Currency;

        [JsonProperty("source")]
        public string Source;

        [JsonProperty("item")]
        public Item Item;

        [JsonProperty("base")]
        public Base Base;
    }

    public class Retailer2
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;
    }

    public class Ratings
    {
        [JsonProperty("count_all")]
        public int CountAll;

        [JsonProperty("average_all")]
        public double AverageAll;
    }

    public class Origins
    {
        [JsonProperty("producing_country")]
        public string ProducingCountry;
    }

    public class Description
    {
        [JsonProperty("text")]
        public string Text;

        [JsonProperty("source")]
        public string Source;
    }

    public class ProductDetail
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("language")]
        public string Language;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("slug")]
        public string Slug;

        [JsonProperty("boss_number")]
        public string BossNumber;

        [JsonProperty("tags")]
        public List<string> Tags;

        [JsonProperty("is_variant")]
        public bool IsVariant;

        [JsonProperty("gtins")]
        public List<string> Gtins;

        [JsonProperty("brand")]
        public Brand2 Brand;

        [JsonProperty("labels")]
        public List<Label2> Labels;

        [JsonProperty("categories")]
        public List<Category> Categories;

        [JsonProperty("additional_categories")]
        public List<List<Category>> AdditionalCategories;

        [JsonProperty("features")]
        public List<Feature> Features;

        [JsonProperty("image")]
        public Image4 Image;

        [JsonProperty("origins")]
        public Origins Origins;

        [JsonProperty("image_transparent")]
        public ImageTransparent ImageTransparent;

        [JsonProperty("internal_features")]
        public List<InternalFeature> InternalFeatures;

        [JsonProperty("vat")]
        public Vat Vat;

        [JsonProperty("links")]
        public Links Links;

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt;

        [JsonProperty("receipt_text")]
        public string ReceiptText;

        [JsonProperty("price")]
        public Price Price;

        [JsonProperty("retailer")]
        public Retailer2 Retailer;

        [JsonProperty("ratings")]
        public Ratings Ratings;

        [JsonProperty("reindex_date")]
        public DateTime ReindexDate;

        [JsonProperty("description")]
        public Description Description;

        [JsonProperty("allergen_text")]
        public string AllergenText;
    }

}
