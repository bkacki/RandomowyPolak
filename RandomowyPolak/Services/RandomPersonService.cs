using RandomPersonalDataGenerator;

namespace RandomowyPolak.Services
{
    public interface IRandomPersonService
    {
        public Task<RandomPerson> GenerateRandomPerson();
    }

    public class RandomPersonService : IRandomPersonService
    {
        private BirthDateGenerator _birthDateGenerator;
        private PhoneNumberGenerator _phoneNumberGenerator;
        private RandomNameGenerator _randomNameGenerator;
        private RandomPeselGenerator _randomPeselGenerator;
        private RandomAddress _randomAddress;

        public RandomPersonService(HttpClient httpClient)
        {
            _birthDateGenerator = new BirthDateGenerator();
            _phoneNumberGenerator = new PhoneNumberGenerator(httpClient);
            _randomNameGenerator = new RandomNameGenerator(httpClient);           
            _randomPeselGenerator = new RandomPeselGenerator();
            _randomAddress = new RandomAddress(httpClient);
        }

        public async Task<RandomPerson> GenerateRandomPerson()
        {
            await _randomNameGenerator.InitializeAsync();
            await _phoneNumberGenerator.InitializeAsync();
            return await RandomPerson.CreateAsync(_birthDateGenerator, _randomAddress, _randomNameGenerator, _phoneNumberGenerator);
        }
    }
}
