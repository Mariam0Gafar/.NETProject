## 📚 University Management System (.NET + Angular)

- **Backend**: ASP.NET Core Web API
- **Frontend**: Angular
- **Database**: SQL Server

---

## 📁 Project Structure

```

.NetProject/
├── Backend/        → ASP.NET Core Web API
└── Frontend/       → Angular App

````

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/.NetProject.git
cd .NetProject
````

---

## ⚙️ Backend Setup (ASP.NET Core)

### a. Navigate to Backend Folder

```bash
cd Backend
```
---

### b. Configure the Database

Open the `appsettings.json` file and edit the connection string section:

```json
"ConnectionStrings": {
  "DbConnection": "server=YOUR_SERVER_NAME;database=WebApp3;user id=YOUR_ID;password=YOUR_PASSWORD;trusted_connection=true;trust server certificate=true"
}
```

> Replace the values accordingly:
>
> * `YOUR_SERVER_NAME`: e.g., `(localdb)\\MSSQLLocalDB`, `localhost`, or your SQL Server name
> * `YOUR_ID`: your SQL Server login username (for SQL Authentication)
> * `YOUR_PASSWORD`: your SQL Server password

---

### c. Run the Backend Server

```bash
dotnet run
```

The API will start at:

```
https://localhost:7096
```

> Swagger will open automatically in your browser (`https://localhost:7096/swagger`)

The application will:

* Automatically apply any pending migrations
* Create the database (if missing)
* Seed roles
* Create a default admin user

---

## 💻 Frontend Setup (Angular)

### a. Navigate to Frontend Folder

```bash
cd ../Frontend
```

### b. Install Angular Dependencies

```bash
npm install
```

### c. Run the Angular App

```bash
ng serve
```

Open your browser:

```
http://localhost:4200
```

---

## 🔐 Default Admin Account

After first run, the following user is seeded automatically:

| Email                                     | Password    |
| ----------------------------------------- | ----------- |
| [admin@admin.com](mailto:admin@admin.com) | Test123321. |

You can use this to log in as an administrator.

---

## 🛠 Tech Stack

* ✅ ASP.NET Core Web API (.NET 8)
* ✅ Angular 17+
* ✅ SQL Server
* ✅ Entity Framework Core
* ✅ JWT Authentication
* ✅ Role-Based Authorization
* ✅ Swagger for API testing

---

## ⚠️ Troubleshooting

* 🔌 Make sure SQL Server or LocalDB is running
* 🔑 Check your connection string is correct
* 🧼 Run `dotnet clean` then `dotnet ef database update` if migration errors occur
* 🌐 Ensure CORS is configured properly if API calls are blocked

---

## 📄 License

This project is open source under the [MIT License](LICENSE).

---
