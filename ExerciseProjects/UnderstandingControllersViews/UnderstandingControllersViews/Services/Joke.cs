using System.Text.Json;

namespace UnderstandingControllersViews.Services
{
    public class Joke
    {
        public string type { get; set; }

        public Value value { get; set; }

        public async Task<string> GetJoke()
        {
            string apiResponse = "";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://api.icndb.com/jokes/random"))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            var joke = JsonSerializer.Deserialize<Joke>(apiResponse);
            return joke.value.joke;
        }
    }

    public class Value
    {
        public int id { get; set; }

        public string joke { get; set; }

        public object[] categories { get; set; }
    }
}
