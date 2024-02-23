using System.Data;
using System.Net;

namespace ModelBindingValidation.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public DateTime DOB { get; set; }

        public Address HomeAddress { get; set; } = new Address();

        public Role Role { get; set; }
    }

    public class Address
    {
        public string HouseNumber { get; set; } = String.Empty;

        public string Street { get; set; } = String.Empty;

        public string City { get; set; } = String.Empty;

        public string PostalCode { get; set; } = String.Empty;

        public string Country { get; set; } = String.Empty;
    }

    public enum Role
    {
        Admin,
        Designer,
        Manager
    }
}
