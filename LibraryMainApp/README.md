# Library Management System
ICT272 - Web Design and Development | T1 2026

## Group Members

| Student Name | Student ID | Role |
|---|---|---|
| Masahiro Togasaki | 20027985 | Database Logic / ERD / Full-stack support|
| Hussain | 12300164 | Backend Justification, Security and Testing |
| Danish Bashir | 20028695 | Frontend / UI |
| Manpreet Singh | 20029163 | Introduction / User Manual / Conclusion |

---

## Tech Stack

| Technology | Version |
|---|---|
| ASP.NET Core MVC | .NET 10.0 |
| Entity Framework Core | 10.0.7 |
| SQL Server Express | Latest |
| Bootstrap | 5 |
| C# | Latest |

---

## Prerequisites

Make sure you have the following installed before running the project:

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (with ASP.NET and web development workload)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

---

## Environment Setup

### 1. Clone or Extract the Project
Extract the zip file and open the solution file:
```
LibraryMainApp.sln
```

### 2. Configure the Database Connection
Open `appsettings.json` and update the connection string if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=LibraryMainDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

> If your SQL Server instance has a different name, replace `localhost\\SQLEXPRESS` with your server name.
> You can find your server name in **SQL Server Object Explorer** inside Visual Studio.

### 3. Apply Database Migrations
Open **Package Manager Console** in Visual Studio:
```
Tools → NuGet Package Manager → Package Manager Console
```

Run the following commands in order:
```
Add-Migration InitialSetup
Update-Database
```

> If migrations already exist, just run:
> ```
> Update-Database
> ```

### 4. Run the Application
Press **F5** in Visual Studio or click the green **Run** button.

The app will automatically:
- Connect to the database
- Seed initial data (Staff, Member, and Books)

---

## Default Login Credentials

### Librarian / Admin
| Field | Value |
|---|---|
| Email | `admin@library.com` |
| Password | `demo` |

### Member
| Field | Value |
|---|---|
| Email | `demo@library.com` |
| Password | `demo` |

> You can also register a new member account via the **Register** page.

---

## Project Structure

```
LibraryMainApp/
├── Controllers/
│   ├── AccountController.cs        # Login, Register, Logout
│   ├── LibrarianController.cs      # Librarian Dashboard
│   ├── MemberController.cs         # Member Dashboard
│   ├── LibraryOperationsController.cs  # Browse Books, Loans, Borrow, Reserve
│   ├── ReportsController.cs        # Library Reports
│   └── HomeController.cs           # Home Page
│
├── Models/
│   ├── Book.cs
│   ├── Member.cs
│   ├── Staff.cs
│   ├── Loan.cs
│   ├── Reservation.cs
│   ├── Fine.cs
│   ├── Feedback.cs
│   └── LibraryProfile.cs
│
├── Views/
│   ├── Account/
│   │   ├── Login.cshtml
│   │   └── Register.cshtml
│   ├── Librarian/
│   │   └── Dashboard.cshtml
│   ├── Member/
│   │   └── Dashboard.cshtml
│   ├── Home/
│   │   └── Index.cshtml
│   ├── LibraryOperations/
│   │   ├── Index.cshtml            # Browse Books
│   │   └── Loans.cshtml
│   ├── Reports/
│   │   └── Index.cshtml
│   └── Shared/
│       └── _Layout.cshtml          # Master layout
│
├── Data/
│   └── ApplicationDbContext.cs     # EF Core DB Context
│
├── wwwroot/
│   ├── css/
│   │   ├── site.css
│   │   └── custom.css              # Danish Front_End design
│   └── js/
│       └── site.js
│
├── Migrations/                     # EF Core Migrations
├── appsettings.json                # DB connection string
└── Program.cs                      # App startup + seed data
```

---

## Key Features

- **Browse Books** — View all available books in a card grid layout
- **Borrow / Reserve** — Borrow available books or reserve borrowed ones
- **Loans Management** — View and return borrowed books
- **Reports** — Overdue books, most borrowed books, most active members
- **Login / Register** — Session-based authentication for Librarians and Members
- **Librarian Dashboard** — Manage books, loans, reports, and library profile
- **Member Dashboard** — View current loans and browse books

---

## Troubleshooting

| Problem | Solution |
|---|---|
| Database connection error | Check SQL Server is running and server name in `appsettings.json` is correct |
| Pending model changes error | Run `Add-Migration <name>` then `Update-Database` in Package Manager Console |
| Seed data not showing | Make sure `Update-Database` was run before starting the app |
| Login not working | Check `dbo.Staffs` and `dbo.Members` tables have data in SQL Server Object Explorer |
