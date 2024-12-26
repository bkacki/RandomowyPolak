# Randomowy Polak

Randomowy Polak is a Blazor WebAssembly application that generates random personal data for Polish individuals. This project uses various services to generate random names, birth dates, phone numbers, PESEL numbers, and addresses.

## Features

- Generate random personal data including:
  - Name
  - Phone number
  - Birth day
  - PESEL
  - Address
- Select which data fields to include in the generated results
- Display generated data in a styled table

## Technologies Used

- Blazor WebAssembly
- .NET 8
- Bootstrap 5.3
- RandomPersonalDataGenerator library (https://github.com/bkacki/RandomPersonalDataGenerator)

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

### Installation

1. Clone the repository:
`git clone https://github.com/yourusername/RandomowyPolak.git`


2. Navigate to the project directory:
`cd RandomowyPolak`


3. Open the solution in Visual Studio 2022:
`start RandomowyPolak.sln`


4. Restore the dependencies:
`dotnet restore`


### Running the Application

1. Build and run the application:
`dotnet run`


2. Open your browser and navigate to `https://localhost:5001` to see the application in action.

## Usage

1. Enter the number of people to generate.
2. Select the data fields you want to include in the generated results.
3. Click the "Generate" button to generate random personal data.
4. The generated data will be displayed in a table.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License.



