using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomPersonalDataGenerator
{
    public class RandomNameGenerator
    {
        private static Random random = new Random();
        private  List<string> _maleName = new List<string>();
        private  List<string> _maleSurname = new List<string>();
        private  List<string> _femaleName = new List<string>();
        private  List<string> _femaleSurname = new List<string>();

        private readonly HttpClient _httpClient;

        public RandomNameGenerator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task InitializeAsync()
        {
            _maleName = await LoadNamesFromFileAsync("Data/MaleNames.csv");
            _maleSurname = await LoadNamesFromFileAsync("Data/MaleSurnames.csv");
            _femaleName = await LoadNamesFromFileAsync("Data/FemaleNames.csv");
            _femaleSurname = await LoadNamesFromFileAsync("Data/FemaleSurnames.csv");
        }

        private async Task<List<string>> LoadNamesFromFileAsync(string fileName)
        {
            try
            {
                // Pobieranie pliku jako tekst z folderu wwwroot
                var response = await _httpClient.GetStringAsync(fileName);
                var lines = response.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //Console.WriteLine($"Loaded {lines.Count} names from {fileName}");
                return lines;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching {fileName}: {ex.Message}");
                return new List<string>();
            }
        }

        public string GenerateRandomName(Gender gender)
        {
            return GenerateRandomNames(gender, 1).First();
        }

        public List<string> GenerateRandomNames(Gender gender, int howMany)
        {
            List<string> names = new List<string>();

            for (var i = 0; i < howMany; i++)
            {
                string name, surname;

                if (gender == Gender.Male)
                {
                    name = _maleName[random.Next(_maleName.Count)];
                    surname = _maleSurname[random.Next(_maleSurname.Count)];
                }
                else if (gender == Gender.Female)
                {
                    name = _femaleName[random.Next(_femaleName.Count)];
                    surname = _femaleSurname[random.Next(_femaleSurname.Count)];
                }
                else
                {
                    throw new ArgumentException("Invalid gender value.");
                }

                names.Add($"{CapitalizeFirstLetter(name)} {CapitalizeFirstLetter(surname)}");
            }

            return names;
        }

        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }
    }
}
