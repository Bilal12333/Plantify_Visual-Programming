using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Plantation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public string? OrganizedBy { get; set; }
        public int TotalSaplings { get; set; }
        public string? SaplingType { get; set; }
        public List<string> Participants { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double Latitude { get; set; } = 33.6844; // default Rawalpindi
        public double Longitude { get; set; } = 73.0479;
    }
}

