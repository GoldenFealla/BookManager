using System.Text.Json.Serialization;

namespace BookManager.Dtos
{
   public record class BookDto
   {
      [JsonPropertyName("isbn")]
      public required string ISBN { get; set; }

      [JsonPropertyName("title")]
      public required string Title { get; set; }

      [JsonPropertyName("author")]
      public required string Author { get; set; }

      [JsonPropertyName("year_of_publication")]
      public required int YearOfPublication { get; set; }

      [JsonPropertyName("publisher")]
      public required string Publisher { get; set; }

      [JsonPropertyName("image_url_s")]
      public required string ImageURLS { get; set; }

      [JsonPropertyName("image_url_m")]
      public required string ImageURLM { get; set; }

      [JsonPropertyName("image_url_l")]
      public required string ImageURLL { get; set; }
   };
}
