using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace StartechML.Models
{
    public class PublicationRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("category_id")]
        public string CategoryId { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("available_quantity")]
        public int AvailableQuantity { get; set; }

        [JsonPropertyName("condition")]
        public string Condition { get; set; } = "new";

        [JsonPropertyName("listing_type_id")]
        public string ListingTypeId { get; set; } = "gold_special";

        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; } = "ARS";


        [JsonPropertyName("attributes")]
        public List<ItemAttribute> Attributes { get; set; } = new();


        [JsonPropertyName("pictures")]
        public List<ItemPicture> Pictures { get; set; } = new();
    }

    public class ItemAttribute
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("value_name")]
        public string ValueName { get; set; } = string.Empty;
    }

    public class ItemPicture
    {
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
    }
}
