# Uniqlo API Project

## Overview
API สำหรับจัดการข้อมูล UqImportView, Auth, และฟีเจอร์อื่น ๆ ด้วย .NET Core, Entity Framework Core, MySQL, JWT Authentication

## Features
- CRUD สำหรับ UqImportView
- ระบบ Auth (Login, Register, JWT Token)
- User Profile
- Database Migration ด้วย EF Core
- รองรับ Swagger UI

## Getting Started
### 1. Clone Project
```sh
git clone <repo-url>
cd api-uniqlo4
```

### 2. Restore Dependencies
```sh
dotnet restore WepApi.csproj
```

### 3. Build Project
```sh
dotnet build WepApi.csproj
```

### 4. Database Setup
- ตั้งค่า connection string ใน `appsettings.json`
- สร้าง/อัปเดตฐานข้อมูล
```sh
dotnet ef database update
dotnet ef database update --context MySQLDbContext
dotnet ef database update --context WepApiIdentityDbContext
```

### 5. Run Project
```sh
dotnet watch run
```

### 6. Swagger UI
- เปิดเบราว์เซอร์ที่ `http://localhost:<port>/swagger`

## Folder Structure
- `Controllers/` : API Controllers
- `Models/` : Entity Models
- `ModelsDto/` : DTO สำหรับรับ/ส่งข้อมูล
- `Data/` : DbContext
- `Areas/Identity/Data/` : Identity Models & DbContext
- `services/` : Service Layer
- `Migrations/` : EF Core Migration Files

## Common Commands
- สร้าง migration ใหม่
```sh
dotnet ef migrations add <MigrationName>
```
- อัปเดตฐานข้อมูล
```sh
dotnet ef database update
```

## Notes
- ควร include โฟลเดอร์ `Migrations/` ใน git สำหรับโปรเจกต์ใช้งานจริง
- ตั้งค่า JWT ใน `appsettings.json` ให้ตรงกับ production

## Contact
- ผู้พัฒนา: <your name>
- Email: <your email>
