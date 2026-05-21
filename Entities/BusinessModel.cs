using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BusinessModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "The Shop Name field is required.")]
        public string ShopName { get; set; } = string.Empty;  // Make it non-nullable

        [Required(ErrorMessage = "The Location field is required.")]
        public string Location { get; set; } = string.Empty;  // Non-nullable

        [Required(ErrorMessage = "The Image field is required.")]
        public string Image { get; set; } = string.Empty;     // Non-nullable

        [Required(ErrorMessage = "The Phone field is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; } = string.Empty;    // Already non-nullable

        [BsonIgnore]
        public bool IsEditing { get; set; } = false;

    }
}
