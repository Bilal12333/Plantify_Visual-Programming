using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PlantModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]

        public string? ProID { get; set; }
        public string? Id { get; set; }
        public string PlantName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; } = 1;

        public string BusinessId { get; set; } = string.Empty;
        public string SubCategory { get; set; } = string.Empty;





    }
}
