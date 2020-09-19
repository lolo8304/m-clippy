using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace m_clippy.Models.Migros
{


    public class Product
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("language")]
        public string Language;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("names")]
        public Names Names;

        [JsonProperty("slug")]
        public string Slug;

        [JsonProperty("boss_number")]
        public string BossNumber;

        [JsonProperty("description")]
        public Description Description;

        [JsonProperty("status")]
        public Status Status;

        [JsonProperty("tags")]
        public List<string> Tags;

        [JsonProperty("is_variant")]
        public bool IsVariant;

        [JsonProperty("gtins")]
        public List<string> Gtins;

        [JsonProperty("brand")]
        public Brand Brand;

        [JsonProperty("labels")]
        public List<Label> Labels;

        [JsonProperty("categories")]
        public List<Category> Categories;

        [JsonProperty("additional_categories")]
        [JsonIgnore]
        public List<List<Category>> AdditionalCategories;

        [JsonProperty("nutrition_facts")]
        public NutritionFacts NutritionFacts;

        [JsonProperty("features")]
        public List<Feature> Features;

        [JsonProperty("allergens")]
        public List<Allergen> Allergens;



        [JsonProperty("image")]
        public Image Image;

        [JsonProperty("image_transparent")]
        public ImageTransparent ImageTransparent;

        [JsonProperty("internal_features")]
        public List<InternalFeature> InternalFeatures;

        [JsonProperty("vat")]
        public Vat Vat;

        [JsonProperty("regulated_description")]
        public string RegulatedDescription;

        [JsonProperty("links")]
        public Dictionary<string, Homepage> Links;

        [JsonProperty("regional_availability")]
        public RegionalAvailability RegionalAvailability;

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt;

        [JsonProperty("base_unit")]
        public string BaseUnit;

        [JsonProperty("slugs")]
        public Slugs Slugs;

        [JsonProperty("receipt_text")]
        public string ReceiptText;

        [JsonProperty("main_supplier")]
        public MainSupplier MainSupplier;

        [JsonProperty("price")]
        public Price Price;

        [JsonProperty("retailer")]
        public Retailer Retailer;

        [JsonProperty("ratings")]
        public Ratings Ratings;

        [JsonProperty("data_source")]
        public string DataSource;

        [JsonProperty("origins")]
        public Origins Origins;

        [JsonProperty("ingredients")]
        public string Ingredients;

        [JsonProperty("allergen_text")]
        public string AllergenText;

        [JsonProperty("package")]
        public Package Package;

        [JsonProperty("package_information")]
        public PackageInformation PackageInformation;

        [JsonProperty("related_products")]
        public RelatedProducts RelatedProducts;

        [JsonProperty("reindex_date")]
        public DateTime ReindexDate;

        [JsonProperty("recipe_ingredient_ids")]
        public List<string> RecipeIngredientIds;

        [JsonProperty("views")]
        public List<string> Views;
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Names
    {
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

    public class Status
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("seasonal")]
        public bool Seasonal;

        [JsonProperty("pim_status")]
        public string PimStatus;
    }

    public class Homepage
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("purchasable")]
        public bool Purchasable;
    }


    public class Image
    {
        [JsonProperty("original")]
        public string Original;

        private String _stack;
        [JsonProperty("stack")]
        public string Stack {
            get
            {
                return _stack;
            }
            set
            {
                _stack = null;
                Small = value.Replace("{stack}", "small");
                Medium = value.Replace("{stack}", "medium");
            }
        }


        [JsonProperty("small")]
        public string Small;
        [JsonProperty("medium")]
        public string Medium;

        [JsonProperty("tags")]
        public List<string> Tags;

        [JsonProperty("source")]
        public string Source;

    }

    public class Brand
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("slug")]
        public string Slug;

        [JsonProperty("links")]
        public Dictionary<string,Homepage> Links;

        [JsonProperty("image")]
        public Image Image;
    }

    public class Homepage2
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("purchasable")]
        public bool Purchasable;
    }


    public class Label
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("slug")]
        public string Slug;

        [JsonProperty("image")]
        public Image Image;

        [JsonProperty("links")]
        public Dictionary<string, Homepage> Links;
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

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("parent_code")]
        public string ParentCode;

        [JsonProperty("level")]
        public int Level;

        [JsonProperty("image")]
        public Image Image;
    }

    public class Nutrient
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("pictogram_name")]
        public string PictogramName;

        [JsonProperty("category")]
        public string Category;

        [JsonProperty("quantity")]
        public double Quantity;

        [JsonProperty("rda_percent_operator")]
        public string RdaPercentOperator;

        [JsonProperty("quantity_unit")]
        public string QuantityUnit;

        [JsonProperty("quantity_alternate")]
        public int QuantityAlternate;

        [JsonProperty("quantity_alternate_unit")]
        public string QuantityAlternateUnit;
    }

    public class Standard
    {
        [JsonProperty("nutrients")]
        public List<Nutrient> Nutrients;

        [JsonProperty("base_quantity")]
        public int BaseQuantity;

        [JsonProperty("base_unit")]
        public string BaseUnit;
    }

    public class Nutrient2
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("pictogram_name")]
        public string PictogramName;

        [JsonProperty("category")]
        public string Category;

        [JsonProperty("quantity")]
        public double Quantity;

        [JsonProperty("rda_percent")]
        public int RdaPercent;

        [JsonProperty("rda_percent_operator")]
        public string RdaPercentOperator;

        [JsonProperty("quantity_unit")]
        public string QuantityUnit;

        [JsonProperty("quantity_alternate")]
        public int QuantityAlternate;

        [JsonProperty("quantity_alternate_unit")]
        public string QuantityAlternateUnit;
    }

    public class Portion
    {
        [JsonProperty("nutrients")]
        public List<Nutrient2> Nutrients;

        [JsonProperty("base_description")]
        public string BaseDescription;

        [JsonProperty("base_quantity")]
        public int BaseQuantity;

        [JsonProperty("base_unit")]
        public string BaseUnit;
    }

    public class NutritionFacts
    {
        [JsonProperty("standard")]
        public Standard Standard;

        [JsonProperty("portion")]
        public Portion Portion;
    }

    public class ValueObject
    {
        [JsonProperty("value_code")]
        public string ValueCode;

        [JsonProperty("value")]
        public string Value;

        [JsonProperty("numeric_value")]
        public int? NumericValue;

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
        public List<ValueObject> Values;

        [JsonProperty("category_code")]
        public string CategoryCode;

        [JsonProperty("top_fact")]
        public bool TopFact;
    }

    public class Allergen
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("contamination_code")]
        public string ContaminationCode;

        [JsonProperty("contamination")]
        public string Contamination;
    }

    public class ImageTransparent : Image
    {
        [JsonProperty("source")]
        public string Source;
    }

    public class InternalFeature
    {
        [JsonProperty("label_code")]
        public string LabelCode;

        [JsonProperty("label")]
        public string Label;

        [JsonProperty("values")]
        public List<ValueObject> Values;

        [JsonProperty("category_code")]
        public string CategoryCode;

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

    public class Leshop
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("app_url")]
        public string AppUrl;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("canonical")]
        public string Canonical;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("purchasable")]
        public bool Purchasable;
    }

    public class Gmaa
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class Gmbs
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class Gmge
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class Gmlu
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class Gmnf
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class Gmos
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class Gmti
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class Gmvd
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class Gmvs
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class Gmzh
    {
        [JsonProperty("probability")]
        public int Probability;
    }

    public class RegionalAvailability
    {
        [JsonProperty("gmaa")]
        public Gmaa Gmaa;

        [JsonProperty("gmbs")]
        public Gmbs Gmbs;

        [JsonProperty("gmge")]
        public Gmge Gmge;

        [JsonProperty("gmlu")]
        public Gmlu Gmlu;

        [JsonProperty("gmnf")]
        public Gmnf Gmnf;

        [JsonProperty("gmos")]
        public Gmos Gmos;

        [JsonProperty("gmti")]
        public Gmti Gmti;

        [JsonProperty("gmvd")]
        public Gmvd Gmvd;

        [JsonProperty("gmvs")]
        public Gmvs Gmvs;

        [JsonProperty("gmzh")]
        public Gmzh Gmzh;
    }

    public class Slugs
    {
        [JsonProperty("de")]
        public string De;

        [JsonProperty("fr")]
        public string Fr;

        [JsonProperty("it")]
        public string It;
    }

    public class MainSupplier
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("supplier_product_id")]
        public string SupplierProductId;
    }

    public class Item
    {
        [JsonProperty("price")]
        public double Price;

        [JsonProperty("original_price")]
        public int OriginalPrice;

        [JsonProperty("quantity")]
        public int Quantity;

        [JsonProperty("unit")]
        public string Unit;

        [JsonProperty("varying_quantity")]
        public bool VaryingQuantity;

        [JsonProperty("display_quantity")]
        public string DisplayQuantity;
    }

    public class Logo
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("source")]
        public string Source;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Badge
    {
        [JsonProperty("original")]
        public string Original;

        [JsonProperty("stack")]
        public string Stack;
    }

    public class Discount
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

        [JsonProperty("discount_amount")]
        public string DiscountAmount;

        [JsonProperty("discount_regions")]
        public List<string> DiscountRegions;

        [JsonProperty("reference_product_id")]
        public string ReferenceProductId;

        [JsonProperty("discount_type")]
        public string DiscountType;

        [JsonProperty("discount_type_id")]
        public string DiscountTypeId;

        [JsonProperty("discount_type_label")]
        public string DiscountTypeLabel;

        [JsonProperty("organisation")]
        public string Organisation;

        [JsonProperty("cooperative")]
        public string Cooperative;

        [JsonProperty("image")]
        public Image Image;

        [JsonProperty("image_transparent")]
        public ImageTransparent ImageTransparent;

        [JsonProperty("logo")]
        public Logo Logo;

        [JsonProperty("badge")]
        public Badge Badge;

        [JsonProperty("location_planning_type")]
        public string LocationPlanningType;

        [JsonProperty("reduction_type_id")]
        public string ReductionTypeId;

        [JsonProperty("special_advertisement")]
        public bool SpecialAdvertisement;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("discount_hint")]
        public string DiscountHint;

        [JsonProperty("high_performer")]
        public bool HighPerformer;

        [JsonProperty("collective_discount")]
        public bool CollectiveDiscount;

        [JsonProperty("instead_of")]
        public string InsteadOf;

        [JsonProperty("price")]
        public double Price;

        [JsonProperty("original_price")]
        public int OriginalPrice;

        [JsonProperty("disclaimer")]
        public string Disclaimer;

        [JsonProperty("source")]
        public string Source;

        [JsonProperty("boss_number")]
        public string BossNumber;

        [JsonProperty("discount_role_id")]
        public string DiscountRoleId;

        [JsonProperty("discount_role_label")]
        public string DiscountRoleLabel;

        [JsonProperty("last_imported")]
        public DateTime LastImported;
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
        public Discount Discount;

        [JsonProperty("discount_hint")]
        public string DiscountHint;
    }

    public class Retailer
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

    public class Package
    {
        [JsonProperty("content")]
        public int Content;

        [JsonProperty("content_unit_code")]
        public string ContentUnitCode;

        [JsonProperty("content_unit")]
        public string ContentUnit;

        [JsonProperty("net_weight")]
        public double NetWeight;

        [JsonProperty("net_weight_unit_code")]
        public string NetWeightUnitCode;

        [JsonProperty("net_weight_unit")]
        public string NetWeightUnit;

        [JsonProperty("precision")]
        public string Precision;

        [JsonProperty("brutto_weight")]
        public double BruttoWeight;

        [JsonProperty("brutto_weight_unit_code")]
        public string BruttoWeightUnitCode;

        [JsonProperty("brutto_volume")]
        public double BruttoVolume;

        [JsonProperty("brutto_volume_unit_code")]
        public string BruttoVolumeUnitCode;

        [JsonProperty("height")]
        public double Height;

        [JsonProperty("length")]
        public double Length;

        [JsonProperty("width")]
        public int Width;

        [JsonProperty("unit_dimension")]
        public string UnitDimension;

        [JsonProperty("price_comparison_content")]
        public int PriceComparisonContent;

        [JsonProperty("size")]
        public string Size;
    }

    public class Tu
    {
        [JsonProperty("brutto_weight")]
        public double BruttoWeight;

        [JsonProperty("brutto_weight_unit_code")]
        public string BruttoWeightUnitCode;

        [JsonProperty("brutto_volume")]
        public double BruttoVolume;

        [JsonProperty("brutto_volume_unit_code")]
        public string BruttoVolumeUnitCode;

        [JsonProperty("height")]
        public double Height;

        [JsonProperty("length")]
        public double Length;

        [JsonProperty("width")]
        public double Width;

        [JsonProperty("dimension_unit_code")]
        public string DimensionUnitCode;

        [JsonProperty("number_of_base_units")]
        public int NumberOfBaseUnits;
    }

    public class Cu
    {
        [JsonProperty("brutto_weight")]
        public double BruttoWeight;

        [JsonProperty("brutto_weight_unit_code")]
        public string BruttoWeightUnitCode;

        [JsonProperty("brutto_volume")]
        public double BruttoVolume;

        [JsonProperty("brutto_volume_unit_code")]
        public string BruttoVolumeUnitCode;

        [JsonProperty("height")]
        public double Height;

        [JsonProperty("length")]
        public double Length;

        [JsonProperty("width")]
        public int Width;

        [JsonProperty("dimension_unit_code")]
        public string DimensionUnitCode;

        [JsonProperty("number_of_base_units")]
        public int NumberOfBaseUnits;
    }

    public class Lu
    {
        [JsonProperty("brutto_weight")]
        public double BruttoWeight;

        [JsonProperty("brutto_weight_unit_code")]
        public string BruttoWeightUnitCode;

        [JsonProperty("brutto_volume")]
        public int BruttoVolume;

        [JsonProperty("brutto_volume_unit_code")]
        public string BruttoVolumeUnitCode;

        [JsonProperty("height")]
        public double Height;

        [JsonProperty("length")]
        public int Length;

        [JsonProperty("width")]
        public int Width;

        [JsonProperty("dimension_unit_code")]
        public string DimensionUnitCode;

        [JsonProperty("number_of_base_units")]
        public int NumberOfBaseUnits;
    }

    public class PackageInformation
    {
        [JsonProperty("tu")]
        public Tu Tu;

        [JsonProperty("cu")]
        public Cu Cu;

        [JsonProperty("lu")]
        public Lu Lu;
    }

    public class PurchaseRecommendations
    {
        [JsonProperty("product_ids")]
        public List<string> ProductIds;
    }

    public class RelatedProducts
    {
        [JsonProperty("purchase_recommendations")]
        public PurchaseRecommendations PurchaseRecommendations;
    }


}
