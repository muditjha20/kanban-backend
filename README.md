# 🧠 Kanban Board - Backend API

This is the **backend API** for the full-stack Kanban Board web app. It is built using **ASP.NET Core (C#)** and connects to a **PostgreSQL** database hosted on Render.

The API supports secure CRUD operations for tasks, columns, and users, protected using **Firebase Authentication**.

> 🔗 **Live Backend URL**: [kanban-backend](https://kanban-backend-2vbh.onrender.com)
> 
> 💻 **Frontend Repo**: [kanban-frontend](https://github.com/muditjha20/kanban-frontend)

---

## 🚀 Features

- ✅ RESTful API using ASP.NET Core Web API
- 🔐 Firebase Authentication (verifies Firebase tokens server-side)
- 📦 PostgreSQL database via Entity Framework Core
- 🧱 Models for Task, Column, and User
- 🌐 CORS enabled for frontend communication
- 📂 Migration-based schema updates using EF Core
- ☁️ Hosted on Render with environment-secured configuration

---

## 🛠️ Tech Stack

| Component    | Tech                                       |
|--------------|--------------------------------------------|
| Language     | C#                                         |
| Framework    | ASP.NET Core Web API                       |
| ORM          | Entity Framework Core + Npgsql (PostgreSQL provider) |
| Auth         | Firebase Admin SDK (token verification)    |
| DB           | PostgreSQL (hosted on Render)              |
| Hosting      | Render.com                                 |

---

## 📦 Endpoints Overview

| Method | Endpoint              | Description                     | Auth Required |
|--------|-----------------------|----------------------------------|---------------|
| GET    | `/api/tasks`          | Get all tasks                   | ✅ Yes        |
| POST   | `/api/tasks`          | Create a new task               | ✅ Yes        |
| PUT    | `/api/tasks/{id}`     | Update an existing task         | ✅ Yes        |
| DELETE | `/api/tasks/{id}`     | Delete a task                   | ✅ Yes        |
| GET    | `/api/tasks/{id}`     | Get a task by ID                | ✅ Yes        |
| ...    | `/api/columns`        | Column CRUD (auto-seeded)       | ✅ Yes        |

All endpoints require a valid Firebase token passed as a `Bearer` token in the `Authorization` header.

---

## 🔐 Firebase Auth Integration

- On frontend login (email/password or Google), a Firebase **ID token** is generated.
- This token is included in requests as:
  ```
  Authorization: Bearer <id_token>
  ```
- On the backend, the token is verified using the Firebase Admin SDK with a downloaded `firebase-service-account.json` file.

Make sure the service account file is available in the root directory during deployment.

---

## 🔧 Environment Setup

### Prerequisites

- .NET 8 SDK
- PostgreSQL database
- Firebase project (with service account JSON)

### Configuration

Ensure your `appsettings.json` or environment variables include:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...;Database=...;Username=...;Password=...;..."
  }
}
```

### Migrations (Local Only)

If needed locally, apply migrations like so:

```bash
dotnet ef migrations add AddUserIdToTasks
dotnet ef database update
```

Ensure your working environment has the correct DB set up before applying migrations.

---

## 🌍 Deployment Notes (Render)

- Add the `firebase-service-account.json` manually to the root folder via CLI or Render Dashboard.
- Set `DefaultConnection` as an environment variable on Render.
- Enable HTTPS redirection and CORS for frontend URL.
- Deploy via GitHub integration or manual push.

---

## 👨‍💻 Author

**Mudit Mayank Jha**  
B.Sc. Computer Science @ The University of the West Indies  
[GitHub](https://github.com/muditjha20) · [LinkedIn](https://linkedin.com/in/muditjha)

---

## 📝 License

MIT License — feel free to fork, enhance, and share.
