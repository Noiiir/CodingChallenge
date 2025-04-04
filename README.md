# Task Management System

A full-stack task management application built with Angular frontend and ASP.NET Core Web API backend.

## Setup Instructions

### Prerequisites
- [Node.js](https://nodejs.org/) (v18.x or later)
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- [SQL Server](https://www.microsoft.com/sql-server) (Express or Developer edition)
- [Angular CLI](https://angular.dev/tools/cli/setup-local) (v17.x or later)

### Backend Setup (ASP.NET Core Web API)

1. **Navigate to the API project directory**
   ```bash
   cd TaskManagementApi
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the database connection**
   
   Open `appsettings.json` and update the connection string to match your SQL Server instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```
   
   For SQL Server Express:
   ```
   "Server=localhost\\SQLEXPRESS;Database=TaskManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the API**
   ```bash
   dotnet run
   ```
   
   The API will be available at https://localhost:7001 (or similar port)
   Swagger documentation will be available at https://localhost:7001/swagger

### Frontend Setup (Angular)

1. **Navigate to the Angular project directory**
   ```bash
   cd CodingTest
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Run the application**
   ```bash
   ng serve
   ```
   
   The application will be accessible at http://localhost:4200

## API Documentation

### Task Endpoints

| Method | Endpoint                   | Description                               | Request Body | Response |
|--------|----------------------------|-------------------------------------------|--------------|----------|
| GET    | /api/tasks                 | Get paginated list of tasks               | N/A          | `{ "items": Task[], "totalCount": number, "pageNumber": number, "pageSize": number, "totalPages": number }` |
| GET    | /api/tasks/{id}            | Get task by ID                            | N/A          | Task object |
| POST   | /api/tasks                 | Create a new task                         | CreateTaskDto | Task object |
| PUT    | /api/tasks/{id}            | Update an existing task                   | UpdateTaskDto | No content |
| DELETE | /api/tasks/{id}            | Delete a task                             | N/A          | No content |
| GET    | /api/tasks/due-next-week   | Get tasks due within next 7 days          | N/A          | Task[] |
| GET    | /api/tasks/by-priority     | Get count of tasks grouped by priority    | N/A          | `{ [priority: string]: number }` |
| GET    | /api/tasks/overdue         | Get tasks with past due dates             | N/A          | Task[] |
| PUT    | /api/tasks/update-status   | Update status of tasks matching criteria  | Query params: `currentStatus`, `newStatus` | No content |

### Architecture Overview

```
ProjectRoot/
├── CodingTest/                        # Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/            # Shared UI components
│   │   │   ├── features/
│   │   │   │   └── tasks/             # Task-related components
│   │   │   │       ├── pages/         # Task pages (list, detail, form)
│   │   │   │       └── components/    # Task-specific components
│   │   │   ├── models/                # TypeScript interfaces
│   │   │   ├── services/              # API services
│   │   │   └── app.routes.ts          # Routing configuration
│   │   └── ...
└── TaskManagementApi/                 # ASP.NET Core Backend
    ├── Controllers/                   # API controllers
    ├── Data/                          # DbContext and database access
    ├── DTOs/                          # Data Transfer Objects
    ├── Models/                        # Domain models
    ├── Repositories/                  # Repository pattern implementations
    └── Program.cs                     # Application entry point and configuration
```

### Backend Architecture

- **ASP.NET Core Web API**: Provides RESTful endpoints for task management
- **Entity Framework Core**: ORM for database access and migrations
- **Repository Pattern**: Abstracts data access logic from controllers
- **DTO Pattern**: Separates API contracts from internal domain models
- **Dependency Injection**: For loose coupling and testability

### Frontend Architecture

- **Angular Component Architecture**: Modular UI components
- **Angular Material**: UI component library
- **Reactive Forms**: For form handling and validation
- **RxJS**: For reactive state management and API calls
- **Lazy Loading**: Modules loaded on demand for better performance
- **Responsive Design**: Mobile-friendly UI using Angular Material


