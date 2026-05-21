using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrderModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; } = "";

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";

        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string PostalCode { get; set; } = "";

        public string PaymentMethod { get; set; } = "";

        public decimal SubTotal { get; set; }
        public decimal Gst { get; set; }
        public decimal FinalTotal { get; set; }

        public List<PlantModel> Items { get; set; } = new();

        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
