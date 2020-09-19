using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace m_clippy.Models.Allergens
{

    public class ProductList
    {
        [JsonProperty("total_hits")]
        public int TotalHits;

        [JsonProperty("facets")]
        public Facets Facets;

        [JsonProperty("products")]
        public List<Product2> Products;
    }

    public class PurchasableOnline
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term2> Terms;
    }

    public class Term2
    {
        [JsonProperty("term")]
        public string Term;

        [JsonProperty("count")]
        public int Count;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("active")]
        public bool Active;

        [JsonProperty("slug")]
        public string Slug;
    }

    public class Retailer
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term2> Terms;
    }

    public class Discount
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term2> Terms;
    }

  

    public class Label
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term2> Terms;
    }


    public class Brand
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term2> Terms;
    }

    public class Term6
    {
        [JsonProperty("term")]
        public string Term;

        [JsonProperty("count")]
        public int Count;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("active")]
        public bool Active;
    }

    public class MAPIALLERGENES
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("terms")]
        public List<Term6> Terms;
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

        [JsonProperty("MAPI_ALLERGENES")]
        public MAPIALLERGENES MAPIALLERGENES;
    }

    public class Image
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
        public Image Image;
    }

    public class Value2
    {
        [JsonProperty("value_code")]
        public string ValueCode;

        [JsonProperty("value")]
        public string Value;

        [JsonProperty("numeric_value")]
        public double NumericValue;
    }

    public class Feature
    {
        [JsonProperty("label_code")]
        public string LabelCode;

        [JsonProperty("label")]
        public string Label;

        [JsonProperty("values")]
        public List<Value2> Values;

        [JsonProperty("top_fact")]
        public bool TopFact;

        [JsonProperty("unit_code")]
        public string UnitCode;

        [JsonProperty("unit")]
        public string Unit;

        [JsonProperty("unit_symbol")]
        public string UnitSymbol;
    }

    public class MICFarbenMoebelAlle
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("value")]
        public string Value;

        [JsonProperty("hex_color")]
        public string HexColor;
    }

    public class MICBreiteMICTiefeMICHoeheAlle
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("value")]
        public string Value;
    }

    public class Attributes
    {
        [JsonProperty("MIC_farben|Moebel_Alle")]
        public MICFarbenMoebelAlle MICFarbenMoebelAlle;

        [JsonProperty("MIC_breite/MIC_tiefe/MIC_hoehe|Alle")]
        public MICBreiteMICTiefeMICHoeheAlle MICBreiteMICTiefeMICHoeheAlle;
    }

    public class Image2
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Image3
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("code")]
        public string Code;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Value3
    {
        [JsonProperty("value_code")]
        public string ValueCode;

        [JsonProperty("value")]
        public string Value;

        [JsonProperty("image")]
        public Image3 Image;

        [JsonProperty("numeric_value")]
        public int? NumericValue;

        [JsonProperty("boolean_value")]
        public bool? BooleanValue;
    }

    public class InternalFeature
    {
        [JsonProperty("label_code")]
        public string LabelCode;

        [JsonProperty("label")]
        public string Label;

        [JsonProperty("values")]
        public List<Value3> Values;

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

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("purchasable")]
        public bool Purchasable;

        [JsonProperty("canonical")]
        public string Canonical;
    }

    public class Migipedia
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("purchasable")]
        public bool Purchasable;

        [JsonProperty("canonical")]
        public string Canonical;
    }

    public class Doitgarden
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

        [JsonProperty("doitgarden")]
        public Doitgarden Doitgarden;
    }

    public class Item
    {
        [JsonProperty("price")]
        public double Price;

        [JsonProperty("original_price")]
        public double OriginalPrice;

        [JsonProperty("quantity")]
        public int Quantity;

        [JsonProperty("unit")]
        public string Unit;

        [JsonProperty("varying_quantity")]
        public bool VaryingQuantity;

        [JsonProperty("display_quantity")]
        public string DisplayQuantity;
    }

    public class Discount2
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("region")]
        public string Region;

        [JsonProperty("start_date")]
        public DateTime StartDate;

        [JsonProperty("end_date")]
        public DateTime EndDate;

        [JsonProperty("publication_date")]
        public DateTime PublicationDate;

        [JsonProperty("discount_regions")]
        public List<string> DiscountRegions;

        [JsonProperty("discount_type")]
        public string DiscountType;

        [JsonProperty("discount_type_id")]
        public string DiscountTypeId;

        [JsonProperty("discount_type_label")]
        public string DiscountTypeLabel;

        [JsonProperty("special_advertisement")]
        public bool SpecialAdvertisement;

        [JsonProperty("discount_hint")]
        public string DiscountHint;

        [JsonProperty("high_performer")]
        public bool HighPerformer;

        [JsonProperty("collective_discount")]
        public bool CollectiveDiscount;

        [JsonProperty("price")]
        public double Price;

        [JsonProperty("original_price")]
        public double OriginalPrice;

        [JsonProperty("source")]
        public string Source;

        [JsonProperty("discount_role_id")]
        public string DiscountRoleId;

        [JsonProperty("discount_role_label")]
        public string DiscountRoleLabel;

        [JsonProperty("last_imported")]
        public DateTime LastImported;

        [JsonProperty("discount_amount")]
        public string DiscountAmount;

        [JsonProperty("reduction_type_id")]
        public string ReductionTypeId;

        [JsonProperty("advertisement_type_id")]
        public string AdvertisementTypeId;
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

        [JsonProperty("discount")]
        public Discount2 Discount;

        [JsonProperty("discount_hint")]
        public string DiscountHint;
    }

    public class Retailer2
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;
    }

    public class Description
    {
        [JsonProperty("text")]
        public string Text;

        [JsonProperty("source")]
        public string Source;
    }

    public class Image4
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
        public Image4 Image;
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

        [JsonProperty("level")]
        public int Level;
    }

    public class ImageTransparent
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Ratings
    {
        [JsonProperty("count_all")]
        public int CountAll;

        [JsonProperty("average_all")]
        public int AverageAll;
    }

    public class AdditionalImage
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Product2
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("language")]
        public string Language;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("slug")]
        public string Slug;

        [JsonProperty("product_type")]
        public string ProductType;

        [JsonProperty("boss_number")]
        public string BossNumber;

        [JsonProperty("tags")]
        public List<string> Tags;

        [JsonProperty("is_variant")]
        public bool IsVariant;

        [JsonProperty("base_product_id")]
        public string BaseProductId;

        [JsonProperty("gtins")]
        public List<string> Gtins;

        [JsonProperty("labels")]
        public List<Label2> Labels;

        [JsonProperty("features")]
        public List<Feature> Features;

        [JsonProperty("attributes")]
        public Attributes Attributes;

        [JsonProperty("image")]
        public Image2 Image;

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

        [JsonProperty("warranty_months")]
        public int WarrantyMonths;

        [JsonProperty("reindex_date")]
        public DateTime ReindexDate;

        [JsonProperty("description")]
        public Description Description;

        [JsonProperty("brand")]
        public Brand2 Brand;

        [JsonProperty("categories")]
        public List<Category> Categories;

        [JsonProperty("image_transparent")]
        public ImageTransparent ImageTransparent;

        [JsonProperty("ratings")]
        public Ratings Ratings;

        [JsonProperty("additional_images")]
        public List<AdditionalImage> AdditionalImages;

        [JsonProperty("additional_categories")]
        public List<List<Category>> AdditionalCategories;
    }




}
