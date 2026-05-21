using Entities;
using System.Collections.Generic;

namespace PlantifyAdmin.Services
{
    public interface IPlantationService
    {
        List<Plantation> GetAll();
        Plantation GetById(string id);
        void Add(Plantation plantation);
        void Update(Plantation plantation);
        void Delete(string id);
    }
}
