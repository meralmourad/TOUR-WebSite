# TOUR-WebSite Backend

This project is the backend for the TOUR-WebSite application. It is built using .NET Core and includes APIs for managing users, trips, bookings, and more.

## Prerequisites

Before running the project, ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 10.0 or later)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio](https://visualstudio.microsoft.com/) or any other IDE that supports .NET development

## Steps to Run the Project

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd TOUR-WebSite/Backend
   ```

2. **Set Up the Database**
   - Update the connection string in `appsettings.json` to point to your SQL Server instance.
   - Apply migrations to create the database schema:
     ```bash
     dotnet ef database update
     ```

3. **Run the Application**
   - Start the application using the .NET CLI:
     ```bash
     dotnet run
     ```
   - Alternatively, open the project in Visual Studio and press `F5` to start debugging.

4. **Access the API**
   - The API will be available at `https://localhost:<port>` or `http://localhost:<port>`.
   - Use tools like [Postman](https://www.postman.com/) or a browser to test the endpoints.

## Access API Documentation

After running the application, you can access the Swagger UI for API documentation at:

- `http://localhost:<port>/swagger`

Use this interface to explore and test the available API endpoints.

## Project Structure

- **Controllers**: Contains API controllers for handling HTTP requests.
- **Data**: Includes the database context and unit of work implementation.
- **Migrations**: Contains EF Core migrations for database schema changes.
- **Models**: Defines the data models used in the application.
- **Repositories**: Implements data access logic.

## Common Commands

- **Run the Application**:
  ```bash
  dotnet run
  ```
- **Apply Migrations**:
  ```bash
  dotnet ef database update
  ```
- **Add a New Migration**:
  ```bash
  dotnet ef migrations add <MigrationName>
  ```

## Troubleshooting

- Ensure the SQL Server instance is running and accessible.
- Verify the connection string in `appsettings.json`.
- Check for missing dependencies by running:
  ```bash
  dotnet restore
  ```

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.