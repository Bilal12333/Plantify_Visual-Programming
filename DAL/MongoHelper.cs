using Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class MongoHelper
    {
        private static readonly IMongoDatabase _database;
        static MongoHelper()
        {
            var connectionString = "mongodb+srv://Plantifyadmin:plantify123@plantify.jofqjsn.mongodb.net/dbPlantsData?retryWrites=true&w=majority&appName=Plantify";
            var dbName = "dbPlantsData";
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        public static IMongoCollection<CartModel> GetCartCollection =>
            _database.GetCollection<CartModel>("Cart");
       
        public static IMongoCollection<PlantModel> GetProductCollection =>
           _database.GetCollection<PlantModel>("Plants");
       
        public static IMongoCollection<BusinessModel> GetBusinessCollection =>
      _database.GetCollection<BusinessModel>("Business");

        

        public static IMongoCollection<Plantation> GetPlantationCollection =>
    _database.GetCollection<Plantation>("Events");

        public static IMongoCollection<OrderModel> GetOrderCollection =>
    _database.GetCollection<OrderModel>("Orders");
        public static IMongoCollection<DriveParticipant> GetParticipantCollection =>
    _database.GetCollection<DriveParticipant>("Participants");






    }
}
