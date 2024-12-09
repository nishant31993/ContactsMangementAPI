# Contacts Management Application

## Setup Instructions

1. Clone the repository.
2. Open the solution in Visual Studio or your preferred IDE.
3. Run the application using `dotnet run` or via the IDE.

## How to Run

The application will be running locally at `https://localhost:5037`.

## Design Decisions

- The backend is built using .NET 8 Web API.
- The application uses a JSON file as the data storage (mock database).
- The `ContactService` class handles CRUD operations and validation logic.
- Error handling is done within the controller methods using try-catch blocks and added middleware for global error handling.

## Scaling Considerations

- For larger datasets, a database would replace the JSON file.
- Indexing and pagination would be used for efficient querying.
