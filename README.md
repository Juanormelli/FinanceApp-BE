# FinanceApp-BE

## Introduction

FinanceApp-BE is a backend service for managing and tracking financial investments, created for learning and development purposes.

## Table of Contents

- [Introduction](#introduction)
- [Installation](#installation)
- [Usage](#usage)
- [Features](#features)
- [Dependencies](#dependencies)
- [Configuration](#configuration)
- [Modules](#modules)
- [Project Structure](#project-structure)
- [Troubleshooting](#troubleshooting)
- [Contributors](#contributors)
- [License](#license)

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/Juanormelli/FinanceApp-BE.git
    ```
2. Navigate to the project directory:
    ```sh
    cd FinanceApp-BE
    ```
3. Install dependencies:
    ```sh
    dotnet restore
    ``## Usage

1. Update `appsettings.json`.
2. Run the application:
    ```sh
    dotnet run
    ```

## Features

- Manage financial investments
- CRUD operations for investment data
- Generate performance reports
- User authentication and authorization

## Dependencies

- .NET Core SDK
- Entity Framework Core
- SQL Server

## Configuration

Update the `appsettings.json` file with your configuration settings:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YourConnectionStringHere"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Modules

### User Module

- **Controller**: Manages user endpoints (register, login, profile).
- **Model**: Defines user properties (Id, Username, PasswordHash, Email, CreatedAt).
- **Repository**: Handles data access (Register, Authenticate, GetById).
- **UseCase**: Contains business logic (ExecuteRegister, ExecuteAuthenticate, ExecuteGetProfile).

## Project Structure

- **Controllers**: Contains API controllers for handling HTTP requests. Example: `UserController`.
- **Models**: Defines the data structures used in the application. Example: `User`.
- **Repositories**: Contains classes for data access logic. Example: `UserRepository`.
- **UseCase**: Contains business logic for different operations. Example: `UserUseCase`.
- **Migrations**: Stores database migration files.
- **Properties**: Contains configuration files such as `launchSettings.json`.

## Troubleshooting

- Ensure the .NET Core SDK is installed.
- Verify your database connection string in `appsettings.json`.
- Check the application logs for error details.

## Contributors

- [Juanormelli](https://github.com/Juanormelli)

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/Juanormelli/FinanceApp-BE/blob/master/LICENSE.txt) file for details.
