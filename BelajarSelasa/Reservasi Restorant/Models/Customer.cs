using System.Security.AccessControl;

namespace RestaurantReservationMentahan.Models{
    public class Customer:ICustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
