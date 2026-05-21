using DAL;
using Entities;
using System.Collections.Generic;

namespace PlantifyAdmin.Services
{
    public class PlantationService : IPlantationService
    {
        public List<Plantation> GetAll()
        {
            return PlantationDAL.GetAll();
        }

        public Plantation GetById(string id)
        {
            return PlantationDAL.GetById(id);
        }

        public void Add(Plantation plantation)
        {
            PlantationDAL.Add(plantation);
        }

        public void Update(Plantation plantation)
        {
            PlantationDAL.Update(plantation);
        }

        public void Delete(string id)
        {
            PlantationDAL.Delete(id);
        }
    }
}
