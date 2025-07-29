# Invox Survey API

**Invox Survey** is a full-featured backend system designed for creating and managing face-to-face survey campaigns. It supports real-time tracking, role-based access, and structured reporting. The goal is to make data collection easier, faster, and scalable — perfect for political parties, campaign teams, corporate field research, or social studies.

This project is built during free time and stays fully open source. It reflects practical backend design focused on simplicity, clarity, and maintainability.

---

## Tech Stack

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- Docker
- C# (strongly typed enums, layered architecture)
- JWT Authentication *(coming soon)*

---

## Features (so far)

- Modular seeding system for:
  - Places (survey locations)
  - Question types (open, single, multiple)
  - Survey status flow (Draft → Published → Finished → Archived)
  - Role setup (Admin, Interviewer)
  - Secure user creation with password hashing
- Enum-driven catalogs for clean database design
- Environment-based configuration with safe fallbacks
- Containerized backend ready for deployment

---

## Seeder Passwords

Initial users are seeded when the system starts. You can define custom passwords through environment variables:

- `USER_ADMIN_PWD` → Admin password
- `USER_INTERVIEWER_PWS` → Interviewer password

If not set, the system uses default fallback values:
- `admin123!`
- `interview123!`

These defaults should be replaced in production.

---

## Running Locally

Use the `env.example` file as a starting point. Copy it to `.env` and update the values to match your local or production configuration.


```bash
docker-compose up --build
