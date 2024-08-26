# Library Management System

## Overview
The **Library Management System** is a RESTful Web API developed using .NET Core. This project simulates a real-world library system where users can manage a collection of books, authors, and members, as well as handle borrowing and returning of books.

This project is designed to showcase my skills in C# and .NET Core, following best practices such as TDD, Dependency Injection, and SOLID principles. The goal is to build a maintainable, scalable, and testable application.

## Project Structure
The solution is organized into the following projects:
- **LibraryManagementSystem.Api**: The Web API project that handles HTTP requests and responses.
- **LibraryManagementSystem.Core**: A class library containing business logic, domain models, and service interfaces.
- **LibraryManagementSystem.Infrastructure**: A class library for data access using Entity Framework Core. This layer contains the implementation of repositories and database context.
- **LibraryManagementSystem.Tests**: A test project using xUnit for unit tests. This project follows Test-Driven Development (TDD) practices.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or any other IDE that supports .NET Core
- [Git](https://git-scm.com/)

### Installation
1. Clone the repository:
   ```bash```
   git clone https://github.com/yourusername/LibraryManagementSystem.git

2. Navigate to the project directory:
   ```bash```
cd LibraryManagementSystem

3. Restore the project dependencies:
   ```bash```
dotnet restore

4. Build the solution:
   ```bash```
dotnet build

Running the Application

5. Navigate to the LibraryManagementSystem.Api project directory:
   ```bash```
cd src/LibraryManagementSystem.Api

6. Run the application:
   ```bash```
dotnet run

The API will be available at https://localhost:5001 (or another port configured in the launchSettings.json).

7. Running Tests
Navigate to the LibraryManagementSystem.Tests project directory:
   ```bash```
cd tests/LibraryManagementSystem.Tests

8. Run the tests:
   ```bash```
dotnet test

Project Features
Book Management: Add, update, delete, and search books.
Author Management: Manage author details and associated books.
Member Management: Register members and manage their information.
Borrowing and Returning: Handle borrowing and returning of books with tracking of due dates.

Roadmap
Implement additional features such as advanced search, pagination, and filtering.
Integrate authentication and authorization for secured endpoints.
Enhance logging and exception handling.

Contributing
Contributions are welcome! Please create a feature branch from develop and submit a pull request with a detailed explanation of your changes.

License
This project is licensed under the MIT License - see the LICENSE file for details.

