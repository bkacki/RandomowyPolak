using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomPersonalDataGenerator
{
    public class RandomPerson
    {
        public string? Name { get; set; }
        public Gender? Gender { get; set; }
        private DateTime _dateTime { get; set; }
        public string? BirthDay { get; set; }
        public string? PESEL { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        private static Random random = new Random();

        // Prywatny konstruktor (nadal pozostaje synchronizacyjny, bez asynchronicznych operacji)
        private RandomPerson() { }

        // Statyczna asynchroniczna metoda fabryczna
        public static async Task<RandomPerson> CreateAsync(
            BirthDateGenerator birthDateGenerator,
            RandomAddress randomAddress,
            RandomNameGenerator randomNameGenerator,
            PhoneNumberGenerator phoneNumberGenerator)
        {
            var person = new RandomPerson();

            // Asynchroniczne przypisania
            person.Gender = (Gender)(random.Next(2));
            person.Name = randomNameGenerator.GenerateRandomName((Gender)person.Gender);
            person._dateTime = birthDateGenerator.GenerateRandomBirthDate();
            person.BirthDay = person._dateTime.ToString("dd-MM-yyyy");

            // Generowanie PESEL i innych danych (operacje synchroniczne)
            RandomPeselGenerator randomPesel = new RandomPeselGenerator(person._dateTime);
            person.PESEL = randomPesel.GenerateRandomPesel((Gender)person.Gender);
            person.PhoneNumber = phoneNumberGenerator.GenerateRandomPhoneNumber();

            // Asynchroniczne pobranie adresu
            person.Address = await randomAddress.RandomAddressAsync();

            return person;
        }

        public override string ToString()
        {
            return $"{Name,-25}{PhoneNumber,-20}{BirthDay,-15}{PESEL,-20}{Address,-20}";
        }
    }
}
