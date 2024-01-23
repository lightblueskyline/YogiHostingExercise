namespace Configuration.Models
{
    public class OpenWeatherMapApi
    {
        public string Name { get; set; } = String.Empty;

        public string Url { get; set; } = String.Empty;

        public bool IsSecured { get; set; }
    }
}
