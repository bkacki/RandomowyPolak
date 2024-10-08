using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomPersonalDataGenerator
{
    public class RandomPeselGenerator
    {
        private static readonly Random random = new Random();
        private List<DateTime> _randomBirthDayList = new List<DateTime>();

        public RandomPeselGenerator()
        {
            BirthDateGenerator birthDateGenerator = new BirthDateGenerator();
            _randomBirthDayList.Add(birthDateGenerator.GenerateRandomBirthDate());
        }

       public RandomPeselGenerator(int howMany)
        {
            BirthDateGenerator birthDateGenerator = new BirthDateGenerator();
            _randomBirthDayList = birthDateGenerator.GenerateRandomBirthDates(howMany);
        }

        public RandomPeselGenerator(List<DateTime> dateTimes)
        {
            _randomBirthDayList = new List<DateTime>(dateTimes);
        }

        public RandomPeselGenerator(DateTime dateTime)
        {
            _randomBirthDayList.Add(dateTime);
        }

        public string GenerateRandomPesel(Gender gender)
        {
            if (_randomBirthDayList.Count == 0)
                throw new InvalidOperationException("Birth day list is empty.");

            DateTime dateTime = _randomBirthDayList.First();
            string PESEL = string.Empty;
            string day = dateTime.ToString("dd");
            string month = dateTime.ToString("MM");
            string year = dateTime.ToString("yy");
            if (dateTime.Year >= 2000)
                month = (int.Parse(month) + 20).ToString("D2");
            //6 first digits of PESEL number
            PESEL = $"{year}{month}{day}";
            //3 random digits of PESEL number
            PESEL += random.Next(100, 1000).ToString();
            //gender related digit
            PESEL += (gender == Gender.Female) ? (2 * random.Next(5)).ToString() : (2 * random.Next(5) + 1).ToString();

            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int controlSum = 0;
            for (int i = 0; i < 10; i++)
                controlSum += weights[i] * int.Parse(PESEL[i].ToString());

            controlSum = (10 - (controlSum % 10)) % 10;

            //last digit = control sum
            PESEL += controlSum.ToString();

            _randomBirthDayList.Remove(dateTime);
            return PESEL;
        }

        public List<string> GenerateRandomPeselList(Gender gender, int howMany)
        {
            List<string> randomPESEL = new List<string>();
            for (var i = 0; i < howMany; i++)
                randomPESEL.Add(GenerateRandomPesel(gender));

            return randomPESEL;
        }
    }
}
