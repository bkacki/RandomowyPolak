using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomPersonalDataGenerator
{
    public class BirthDateGenerator
    {
        private static readonly Random random = new Random();
        private DateTime _eighteenYears = DateTime.Now.AddYears(-18);
        private DateTime _lowerLimit = new DateTime(1950, 1, 1);
        private readonly TimeSpan _numberOfDays;

        public BirthDateGenerator()
        {
            _numberOfDays = _eighteenYears - _lowerLimit;
        }

        public BirthDateGenerator(DateTime lowerLimit)
        {
            _lowerLimit = lowerLimit;
            _numberOfDays = _eighteenYears - _lowerLimit;
        }

        public DateTime GenerateRandomBirthDate()
        {
            int randomDays = random.Next(0, (int)_numberOfDays.TotalDays);
            return _lowerLimit.AddDays(randomDays);
        }

        public List<DateTime> GenerateRandomBirthDates(int howMany)
        {
            List<DateTime> birthDateList = new List<DateTime>();

            for(var i=0; i<howMany; i++)
            {
                int randomDays = random.Next(0, (int)_numberOfDays.TotalDays);
                DateTime date = _lowerLimit.AddDays(randomDays);
                birthDateList.Add(date);
            }

            return birthDateList;
        }
    }
}
