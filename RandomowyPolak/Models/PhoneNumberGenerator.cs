using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RandomPersonalDataGenerator
{
    public class PhoneNumberGenerator
    {
        private static Random random = new Random();
        private readonly List<int> _phoneNumberPrefixes = new List<int>();
        private readonly HttpClient _httpClient;

        public PhoneNumberGenerator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("Data/PhoneNumberPrefixes.csv");
                var lines = response.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    if (int.TryParse(line.Trim(), out int prefix))
                    {
                        _phoneNumberPrefixes.Add(prefix);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading prefixes: " + ex.Message);
            }
        }

        public string GenerateRandomPhoneNumber()
        {
            return GenerateRandomPhoneNumber(1).First();
        }

        public List<string> GenerateRandomPhoneNumber(int howMany)
        {
            List<string> phoneNumbers = new List<string>();

            for (var i = 0; i < howMany; i++)
            {
                string prefix = _phoneNumberPrefixes[random.Next(_phoneNumberPrefixes.Count)].ToString();
                int numberOfDigits = 9-prefix.Length;
                int min = (int)Math.Pow(10, numberOfDigits - 1);
                int max = (int)Math.Pow(10, numberOfDigits) - 1;
                string restOfNumber = random.Next(min, max + 1).ToString();
                string phoneNumber = prefix + restOfNumber;

                phoneNumbers.Add($"+48 {phoneNumber.Substring(0,3)} {phoneNumber.Substring(3,3)} {phoneNumber.Substring(6,3)}");
            }

            return phoneNumbers;
        }
    }
}
