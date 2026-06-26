<div align="center">

# 📝 Online Exam Platform

**A full-featured online examination REST API built with ASP.NET Core 9**

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF_Core-9.0-512BD4?style=flat-square)](https://docs.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![JWT](https://img.shields.io/badge/Auth-JWT-000000?style=flat-square&logo=jsonwebtokens)](https://jwt.io/)
[![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)](LICENSE)

</div>

---

## 📌 Overview

Online Exam Platform is a RESTful backend API that enables professors to create and manage exams, assign them to students, and automatically evaluate multiple-choice answers. Students can register, take exams within their allowed time window, and receive instant results.

---

## ✨ Features

- 🔐 **JWT Authentication** with role-based access control (Prof / Student)
- 🔄 **Refresh Token** support with revocation
- 📋 **Exam Management** — create, edit, and assign exams to specific students
- ❓ **Question Management** — supports Multiple Choice, Descriptive, and Fill-in-the-Blank question types
- ✅ **Auto Grading** — automatic correctness check for multiple-choice answers
- 🕒 **Time Window Enforcement** — students can only access exams within the scheduled time
- 🧾 **Answer History** — full answer sheet per student per exam
- 🛡️ **FluentValidation** — request validation on all endpoints
- 🧱 **Clean Architecture** — Repository Pattern, Unit of Work, Service Layer

---

## 🏗️ Architecture

```
OnlineExam/
├── Controllers/          # API endpoints
├── Service/              # Business logic
├── ServiceInterfaces/    # Service contracts
├── Repository/           # Data access layer
├── RepositoryInterfaces/ # Repository contracts
├── Entity/               # Domain models
├── DTOs/                 # Data Transfer Objects
├── DbContext/            # EF Core DbContext
├── Validator/            # FluentValidation validators
├── Enum/                 # QuestionType enum
├── Common/               # Shared utilities (Result pattern)
├── UnitOfWork/           # Transaction coordination
├── Seader/               # Role seeding
└── Migrations/           # EF Core migrations
```

---

## 🗃️ Domain Model

| Entity | Description |
|--------|-------------|
| `User` | Student or Professor (ASP.NET Identity) |
| `Exam` | Exam with start time, duration, and creator |
| `Question` | Question with type, options, and correct answer |
| `Answer` | Student's submitted answer with grading result |
| `RefreshToken` | JWT refresh token with expiration and revocation |

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 9 |
| ORM | Entity Framework Core 9 |
| Database | SQL Server |
| Auth | ASP.NET Core Identity + JWT Bearer |
| Validation | FluentValidation 11 |
| API Docs | Swagger / Swashbuckle |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) (or SQL Server Express)

### 1. Clone the repository

```bash
git clone https://github.com/Mohammadhasan79/Online_Exam.git
cd Online_Exam
```

### 2. Configure `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OnlineExamDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Secret": "your-super-secret-key-min-32-characters",
    "Issuer": "OnlineExam",
    "Audience": "OnlineExamUsers",
    "ExpireMinutes": 60
  }
}
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run the project

```bash
dotnet run
```

Swagger UI will be available at `https://localhost:{port}/swagger`

---

## 🔑 Authentication Flow

```
POST /api/auth/register   →  Register as Prof or Student
POST /api/auth/login      →  Get access token + refresh token
POST /api/auth/refresh    →  Get new access token using refresh token
```

All protected endpoints require:
```
Authorization: Bearer {your_access_token}
```

---

## 📡 API Endpoints

### Auth
| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/register` | Public | Register a new user |
| POST | `/api/auth/login` | Public | Login and get tokens |
| POST | `/api/auth/refresh` | Public | Refresh access token |

### Exams
| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| POST | `/api/exam` | Prof | Create a new exam |
| GET | `/api/exam/{id}` | Prof / Student | Get exam with questions |
| GET | `/api/exam/my-exams` | Student | Get assigned exams |

### Questions
| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| POST | `/api/question` | Prof | Add question to exam |
| PUT | `/api/question/{id}` | Prof | Edit a question |
| DELETE | `/api/question/{id}` | Prof | Delete a question |

### Answers
| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| POST | `/api/answer` | Student | Submit answer list |
| GET | `/api/answer/{examId}` | Prof | View student answers |
| GET | `/api/answer/students/{examId}` | Prof | List students who completed exam |

---

## 🧩 Question Types

```csharp
public enum QuestionType
{
    Descriptive  = 1,   // Free-text answer (not auto-graded)
    MultiChoice  = 2,   // Auto-graded against CorrectAnswer
    FillInBlank  = 3,   // Text completion
}
```

---

## ✅ Validation

All requests are validated automatically via FluentValidation. Invalid requests return `400 Bad Request` with a list of error messages — no extra code needed in controllers.

---

## 🔐 Role-Based Access

| Role | Permissions |
|------|-------------|
| `Prof` | Create/edit exams and questions, view all student answers |
| `Student` | View assigned exams (within time window), submit answers |

---

## 📄 License

This project is licensed under the MIT License.

---

<div align="center">

Made with ❤️ by [Mohammadhasan79](https://github.com/Mohammadhasan79)

</div>
