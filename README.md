# SWork Project

Welcome to the **SWork** project, a robust application built using .NET Core to manage job applications, employers, students, and related functionalities. This README provides an overview of the project structure, design patterns, and guidelines for contributors.

## Project Overview

SWork is designed as a scalable and maintainable system following the **Clean Architecture** and **Onion Architecture** principles. The solution is organized into multiple layers, each with a specific responsibility, ensuring separation of concerns and facilitating easy maintenance and testing.

## Solution Structure

The `SWork` solution consists of 7 projects, organized into distinct layers:

### 1. **SWork.API**
- **Role**: Presentation Layer, handling HTTP requests and responses via RESTful APIs.
- **Contents**:
  - `Controllers`: Defines API endpoints (e.g., `UsersController`, `JobsController`).
  - `appsettings.json`: Configuration file for database connections, security keys, etc.
  - `Program.cs`: Application startup file, configuring HTTP pipeline, dependency injection, and hosting (e.g., Kestrel).
  - `SWork.API.http`: Optional file for API request simulation (e.g., using Postman or VS Code REST Client).
- **Dependencies**: Depends on `SWork.Service` for business logic.
- **Example**: The `/api/users` endpoint in `UsersController` calls `IUserService` to retrieve user data.

### 2. **SWork.Common**
- **Role**: Common Utilities Layer, providing cross-cutting concerns and helper functions.
- **Contents**:
  - `Helper`: Contains utility methods (e.g., string formatting, password hashing, file handling).
  - `Middleware`: Custom middleware for logging, exception handling, authentication (e.g., JWT middleware).
- **Dependencies**: Independent, supports all other layers.
- **Example**: A JWT authentication middleware checks tokens before allowing access to protected endpoints.

### 3. **SWork.Data**
- **Role**: Data Access Layer, managing interactions with the database.
- **Contents**:
  - `Entities`: Contains model classes representing database tables (e.g., `User`, `Job`, `Application`).
  - `DTO`: Data Transfer Objects for mapping data between layers, reducing direct entity dependency.
  - `Migrations`: Manages database schema changes using Entity Framework Core.
  - `Models/SWorkDbContext.cs`: EF Core context, defining `DbSet` for entities and relationship configurations.
- **Dependencies**: Depends on `SWork.Common` (if using helpers), independent of business logic.
- **Example**: `SWorkDbContext` defines `DbSet<User>` and configures the one-to-many relationship between `User` and `Notification`.

### 4. **SWork.Repository**
- **Role**: Repository Layer, implementing data access logic based on contracts from `SWork.Repository.Contract`.
- **Contents**:
  - `Repository`: Implements repository classes (e.g., `UserRepository`, `JobRepository`) with methods like `GetAll()`, `Add()`, `Update()`.
  - `DependencyInjection.cs`: Configures dependency injection to register repositories in the IoC container.
- **Dependencies**: Depends on `SWork.Data` (for context and entities) and `SWork.Repository.Contract` (for interfaces).
- **Example**: `UserRepository` uses `SWorkDbContext` to query data from the `Users` table.

### 5. **SWork.Repository.Contract**
- **Role**: Repository Contract Layer, defining abstractions for data access following the Dependency Inversion Principle.
- **Contents**:
  - `Interfaces`: Contains interfaces like `IRepository<T>`, `IUserRepository`, `IJobRepository`, defining data query methods.
- **Dependencies**: Independent, provides abstractions only.
- **Example**: `IUserRepository` defines `Task<User> GetByIdAsync(int id)`.

### 6. **SWork.Service**
- **Role**: Business Logic Layer, containing core business rules and processes.
- **Contents**:
  - `Services`: Implements service classes (e.g., `UserService`, `JobService`) with business logic.
- **Dependencies**: Depends on `SWork.ServiceContract` (interfaces) and `SWork.Repository` (repositories).
- **Example**: `UserService` calls `IUserRepository` to fetch user data and applies access control logic before returning results.

### 7. **SWork.ServiceContract**
- **Role**: Service Contract Layer, defining abstractions for business logic.
- **Contents**:
  - `Interfaces`: Contains interfaces like `IUserService`, `IJobService`, defining business methods (e.g., `GetUserById`, `CreateJob`).
- **Dependencies**: Independent, provides abstractions only.
- **Example**: `IUserService` defines `Task<UserDto> GetUserByIdAsync(int id)`.

## Data Flow and Interaction

### Data Flow
1. **Client**: Sends an HTTP request to `SWork.API` (e.g., GET `/api/users/1`).
2. **SWork.API**: Invokes a service from `SWork.Service` (e.g., `IUserService.GetUserByIdAsync(1)`).
3. **SWork.Service**: Uses a repository from `SWork.Repository` (e.g., `IUserRepository.GetByIdAsync(1)`).
4. **SWork.Repository**: Interacts with `SWork.Data` via `SWorkDbContext` to query the database.
5. **Response**: Data flows back through the layers, with DTOs used for mapping.

### Dependency Injection (DI)
- Dependencies (services, repositories) are injected via constructors, configured in `DependencyInjection.cs` and registered in `Program.cs` of `SWork.API`.
- Example: `IUserService` and `IUserRepository` are injected into `UsersController`.

### Middleware
- Middleware from `SWork.Common` (e.g., logging, authentication) is added to the HTTP pipeline in `Program.cs` to preprocess requests before reaching controllers.

## Design Patterns and Principles
- **Clean Architecture**: Organizes the application into concentric layers, with domain logic at the center and outer layers depending on inner layers.
- **Repository Pattern**: Separates data access logic from business logic, providing a clean abstraction.
- **Dependency Inversion Principle (DIP)**: High-level modules depend on abstractions (interfaces) rather than implementations.
- **Dependency Injection (DI)**: Manages dependencies using an IoC container, enhancing flexibility and testability.

## Benefits
- **Separation of Concerns**: Each layer has a distinct responsibility, making maintenance and scaling easier.
- **Testability**: Interfaces enable mocking for unit tests (e.g., mocking `IUserRepository` to test `UserService`).
- **Interchangeability**: Allows swapping implementations (e.g., replacing EF Core with Dapper) without affecting business logic.
- **Scalability**: New features can be added by extending services and repositories without breaking existing structure.

## Improvement Suggestions
- **Add Domain Layer**: If `SWork.Data.Entities` only contains database models, consider a separate `SWork.Domain` layer for business rules.
- **Optimize DI**: Ensure all dependencies are correctly registered in `DependencyInjection.cs` and avoid circular dependencies.
- **Documentation**: Add XML comments to interfaces in `SWork.Repository.Contract` and `SWork.ServiceContract` for better development support.
- **Logging**: Implement a logging middleware in `SWork.Common` to monitor performance and errors.

## Getting Started
1. Clone the repository: `git clone <repository-url>`.
2. Install dependencies: `dotnet restore`.
3. Configure `appsettings.json` with your database connection string.
4. Apply migrations: `dotnet ef database update`.
5. Run the application: `dotnet run --project SWork.API`.

## Contributing
- Fork the repository.
- Create a new branch: `git checkout -b feature/<feature-name>`.
- Commit changes: `git commit -m "Add <description>"`.
- Push to the branch: `git push origin feature/<feature-name>`.
- Open a pull request.

## License

