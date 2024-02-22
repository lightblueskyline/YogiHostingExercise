namespace APIControllers.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public string StartLocation { get; set; } = String.Empty;

        public string EndLocation { get; set; } = String.Empty;

    }
}
