namespace ADO.Models
{
    public class Inventory
    {
        public int ID { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
