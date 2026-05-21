using Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PlantationDAL
    {
        private static readonly IMongoCollection<Plantation> _plantations= MongoHelper.GetPlantationCollection;


        // Get all plantations
        public static List<Plantation> GetAll()
        {
            return _plantations.Find(p => true).ToList();
        }

        // Get plantation by Id
        public static Plantation GetById(string id)
        {
            return _plantations.Find(p => p.Id == id).FirstOrDefault();
        }

        // Add plantation
        public static void Add(Plantation plantation)
        {
            _plantations.InsertOne(plantation);
        }

        // Update plantation
        public static void Update(Plantation plantation)
        {
            _plantations.ReplaceOne(p => p.Id == plantation.Id, plantation);
        }

        // Delete plantation
        public static void Delete(string id)
        {
            _plantations.DeleteOne(p => p.Id == id);
        }
    }
}
