<<<<<<< HEAD
# IdentityManager - Advanced Authentication & OAuth System
>>>>>>> cc4f6fda4026bdb1d178dcfed7d5685e0c4ce907

Welcome to the TaskManager API project! This repository showcases a comprehensive and secure Authentication and Authorization system built using **ASP.NET Core 8 Web API**, following Clean Architecture principles.

The project demonstrates sophisticated user management capabilities including **JWT-based Authentication**, **Role-Based Access Control (RBAC)**, and **External OAuth Integration (Google & GitHub)**, making it a robust template for modern .NET applications.

## 🚀 Key Features

### 🔐 Authentication & Security
*   **JWT Bearer Authentication:** Secure, stateless API access using JSON Web Tokens (JWT).
*   **Identity Framework:** Built on top of Microsoft Identity for proven security standards.
*   **Password Policies:** Enforced strong password rules (uppercase, lowercase, digits, special chars).
*   **Secure Password Hashing:** Utilizing standard cryptographic practices.

### 🌐 OAuth Integration
Seamlessly sign in users with popular providers:
*   **Google Authentication:** Native integration handling the OAuth 2.0 flow.
*   **GitHub Authentication:** GitHub OAuth flow implementation for developer-focused apps.
*   *Automatic account creation upon first external login.*

### 📧 User Management & Verification
*   **Email Verification:** Registration process includes OTP (One-Time Password) verification sent via email.
*   **Password Reset Flow:** Secure "Forgot Password" functionality using OTP verification.
*   **Profile Management:** User-initiated password changes.

### 🛡️ Role-Based Access Control (RBAC)
*   **Dynamic Role Assignment:** APIs to assign and remove roles from users.
*   **Role-Protected Endpoints:** Secure endpoints accessible only by specific roles (e.g., Admin).

## 🏗️ Architecture

The solution follows **Clean Architecture (Onion Architecture)** to ensure separation of concerns and maintainability:

*   **Domain:** Contains the core entities (`User`, `Role`) and business logic.
*   **Application:** Holds interfaces, DTOs, and Service implementations (`AuthService`, `JwtTokenService`).
*   **Infrastructure:** Manages data access (`DbContext`, `Repositories`) and external services (`EmailService`).
*   **Web (API):** The entry point containing Controllers (`AuthController`, `OAuthController`) and Configuration (`Program.cs`).

## 🛠️ Technology Stack

*   **Framework:** .NET 8 SDK
*   **Database:** SQL Server (Entity Framework Core)
*   **Authentication:** ASP.NET Core Identity & JWT Bearer
*   **External Auth:** Google & GitHub OAuth Providers
*   **Email Service:** SMTP (System.Net.Mail)
*   **Documentation:** Swagger/OpenAPI

## ⚙️ Configuration & Setup

To run this project locally, follow these steps:

### 1. Prerequisites
*   .NET 8 SDK installed.
*   SQL Server (LocalDB or full instance).

### 2. Configure `appsettings.json`
Update the configuration file in the `Web` project with your local settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=TaskManagerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "YOUR_SUPER_SECRET_KEY_MUST_BE_LONG_ENOUGH",
    "Issuer": "TaskManagerApi",
    "Audience": "TaskManagerClient",
    "DurationInMinutes": 60
  },
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    },
    "GitHub": {
      "ClientId": "YOUR_GITHUB_CLIENT_ID",
      "ClientSecret": "YOUR_GITHUB_CLIENT_SECRET"
    }
  },
  "EmailSettings": {
    "Email": "your-email@example.com",
    "Password": "your-email-password",
    "Host": "smtp.example.com",
    "Port": 587
  }
}
```

### 3. Database Migration
Open a terminal in the solution directory and run:

```bash
dotnet ef database update --project Infrastructure --startup-project Web
```

### 4. Run the Application
```bash
dotnet run --project Web
```
The API will be available at `https://localhost:7001` (or your configured port), and Swagger UI at `/swagger`.

## 🔌 API Endpoints Overview

### Authentication (`/api/Auth`)
| Method | Endpoint | Description |
|:---|:---|:---|
| `POST` | `/register` | Register a new user account. |
| `POST` | `/login` | Authenticate user and receive JWT. |
| `POST` | `/confirm-email` | Verify email address using provided OTP. |
| `POST` | `/forgot-password` | Request a password reset OTP. |
| `POST` | `/reset-password` | Reset password using valid OTP. |
| `POST` | `/change-password` | Change password for logged-in user. |

### OAuth (`/api/OAuth`)
| Method | Endpoint | Description |
|:---|:---|:---|
| `GET` | `/google-login` | Initiate Google OAuth flow. |
| `GET` | `/google-response` | Callback for Google OAuth. |
| `GET` | `/github-login` | Initiate GitHub OAuth flow. |
| `GET` | `/github-response` | Callback for GitHub OAuth. |

### Role Management
| Method | Endpoint | Description |
|:---|:---|:---|
| `POST` | `/assign-role` | Assign a role to a user. |
| `POST` | `/remove-role` | Remove a role from a user. |
| `GET` | `/user-roles/{id}` | Get all roles for a specific user. |

## 🌟 Showcased Skills
*   **Security Implementation:** Secure implementation of JWT, Password Hashing, and OTP-based verification.
*   **Third-Party Integration:** Setting up and handling OAuth flows with major providers.
*   **Architectural Design:** Structuring a complex authentication system within a clean architecture.
*   **Data Validation:** Ensuring robust input handling and error reporting.

---
<<<<<<< HEAD

*Created by [Ahmed Mohamed Ibrahim]*
>>>>>>> cc4f6fda4026bdb1d178dcfed7d5685e0c4ce907
