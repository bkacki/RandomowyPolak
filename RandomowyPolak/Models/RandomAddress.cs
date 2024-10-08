using Newtonsoft.Json.Linq;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Net.Http;
using System.Security;

namespace RandomPersonalDataGenerator
{
    public class RandomAddress
    {
        private static readonly Random random = new Random();
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        //cities with population bigger than 100k in Poland
        private static readonly List<(string name, double lat, double lng)> knownLocations = new List<(string, double, double)>
        {
            ("Warszawa", 52.2297, 21.0122),
            ("Kraków", 50.0647, 19.9450),
            ("Łódź", 51.7592, 19.4586),
            ("Wrocław", 51.1079, 17.0385),
            ("Poznań", 52.4064, 16.9252),
            ("Gdańsk", 54.3520, 18.6466),
            ("Gdynia", 54.5180, 18.5300),
            ("Szczecin", 53.4285, 14.5528),
            ("Lublin", 51.2500, 22.3331),
            ("Katowice", 50.2649, 19.0238),
            ("Białystok", 53.1325, 23.1688),
            ("Kielce", 50.8661, 20.6288),
            ("Toruń", 53.0138, 18.5984),
            ("Bydgoszcz", 53.1235, 18.0077),
            ("Zabrze", 50.3380, 18.7859),
            ("Częstochowa", 50.8116, 19.1204),
            ("Radom", 51.4025, 21.1470),
            ("Rzeszów", 50.0415, 21.9998),
            ("Gliwice", 50.4086, 18.6774),
            ("Olsztyn", 53.7770, 20.4820),
            ("Tychy", 50.1391, 18.9928),
            ("Rybnik", 50.0933, 18.5415),
            ("Tarnów", 49.9911, 20.9875),
            ("Elbląg", 54.1815, 19.3930),
            ("Nowy Sącz", 49.6162, 20.7164),
            ("Słupsk", 54.4612, 17.0333),
            ("Jastrzębie-Zdrój", 49.9564, 18.6001),
            ("Dąbrowa Górnicza", 50.3114, 19.1445),
            ("Włocławek", 52.6452, 19.0675),
            ("Kalisz", 51.7520, 18.0910),
            ("Koszalin", 54.1942, 16.1934),
            ("Zielona Góra", 51.9354, 15.5069),
            ("Legnica", 51.2072, 16.1556),
            ("Mysłowice", 50.2541, 19.1756),
            ("Gorzów Wielkopolski", 52.7386, 15.2282),
            ("Ruda Śląska", 50.3533, 18.8928),
            ("Płock", 52.5498, 19.7041),
            ("Ostrów Wielkopolski", 51.6340, 17.8286),
            ("Chorzów", 50.2996, 18.9622),
            ("Kongresowice", 52.3951, 15.5481),
            ("Świętochłowice", 50.2978, 18.9039)
        };

        public RandomAddress(HttpClient httpClient)
        {
            _client = httpClient;
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            _apiKey = _configuration["ApiSettings:ApiKey"];
            //Console.WriteLine($"API Key: {_apiKey}");
        }

        public async Task<string> RandomAddressAsync()
        {
            bool addressFound = false;
            int maxTries = 20;
            int attempts = 0;

            while (!addressFound && attempts < maxTries)
            {
                attempts++;

                // Wybór losowej lokalizacji z listy
                var location = knownLocations[random.Next(knownLocations.Count)];
                double lat = location.lat + (random.NextDouble() * 0.01 - 0.005); // Małe losowe przesunięcie
                double lng = location.lng + (random.NextDouble() * 0.01 - 0.005); // Małe losowe przesunięcie

                // Tworzenie URL zapytania do API Google
                string url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat.ToString(CultureInfo.InvariantCulture)},{lng.ToString(CultureInfo.InvariantCulture)}&key={_apiKey}";

                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Odpowiedź z API: {responseBody}");
                JObject json = JObject.Parse(responseBody);

                // Pobranie adresu z wyniku zapytania
                var results = json["results"];
                if (results != null && results.Any())
                {
                    var firstResult = results[0];
                    if (IsValidAddress(firstResult, out string address))
                    {
                        addressFound = true;
                        return address;
                    }
                }
            }

            return "Nie udało się znaleźć losowego adresu w mieście w Polsce po kilku próbach.";
        }

        private bool IsValidAddress(JToken firstResult, out string address)
        {
            address = firstResult?["formatted_address"]?.ToString();
            bool isPoland = false, isCity = false, isStreet = false;

            var addressComponents = firstResult?["address_components"];
            if (addressComponents != null)
            {
                foreach (var component in addressComponents)
                {
                    var types = component["types"]?.ToObject<List<string>>();
                    if (types != null)
                    {
                        if (types.Contains("country") && (component["long_name"]?.ToString().Contains("Poland") == true || component["long_name"]?.ToString().Contains("Polska") == true))
                            isPoland = true;
                        if (types.Contains("locality"))
                            isCity = true;
                        if (types.Contains("route"))
                            isStreet = true;
                    }
                }
            }

            //Console.WriteLine($"Sprawdzany adres: {address}, Polska: {isPoland}, Miasto: {isCity}, Ulica: {isStreet}");

            return !string.IsNullOrEmpty(address) && isPoland && isCity && isStreet;
        }
    }
}