# ReviewSlotManager

Backend API quản lý slot review cho nhóm sinh viên và giảng viên, xây dựng theo mô hình 3-layer:
- ReviewSlotManager: Web API (Presentation)
- ServiceLayer: Business logic
- RepositoryLayer: Data access (EF Core + SQL Server)

## 1. Yêu cầu môi trường

- .NET SDK 9.0+
- Docker Desktop (khuyến nghị để chạy kèm SQL Server)
- (Tùy chọn) EF CLI

Cài EF CLI nếu máy chưa có:

```powershell
dotnet tool install --global dotnet-ef
```

## 2. Cấu trúc thư mục chính

- ReviewSlotManager.sln
- ReviewSlotManager/
- ServiceLayer/
- RepositoryLayer/
- docker-compose.yml

## 3. Chạy bằng Docker (khuyến nghị cho team)

Stack gồm:
- api: ASP.NET Core API
- sqlserver: SQL Server 2022

Chạy build + up:

```powershell
docker compose up -d --build
```

Kiểm tra trạng thái:

```powershell
docker compose ps
```

Xem log API:

```powershell
docker compose logs -f api
```

Swagger:
- http://localhost:8080/swagger

Lưu ý:
- API tự chạy migration khi start.
- API có seed dữ liệu mẫu lần đầu nếu DB rỗng.

Dừng dịch vụ:

```powershell
docker compose down
```

Xóa luôn dữ liệu DB (volume):

```powershell
docker compose down -v
```

## 4. Chạy local không Docker

### 4.1 Chuẩn bị SQL Server

Cập nhật connection string trong file:
- ReviewSlotManager/appsettings.json

Key đang dùng:
- ConnectionStrings:DefaultConnectionString

### 4.2 Tạo/Update database

```powershell
dotnet ef database update --project .\RepositoryLayer\RepositoryLayer.csproj --startup-project .\ReviewSlotManager\ReviewSlotManager.csproj
```

### 4.3 Chạy API

```powershell
dotnet run --project .\ReviewSlotManager\ReviewSlotManager.csproj
```

Swagger URL local: xem theo output khi chạy (thường là https://localhost:<port>/swagger).

## 5. Workflow migration cho team

Khi thay đổi entity hoặc mapping trong DbContext:

1. Tạo migration mới:

```powershell
dotnet ef migrations add <TenMigration> --project .\RepositoryLayer\RepositoryLayer.csproj --startup-project .\ReviewSlotManager\ReviewSlotManager.csproj --output-dir Migrations
```

2. Apply migration vào DB:

```powershell
dotnet ef database update --project .\RepositoryLayer\RepositoryLayer.csproj --startup-project .\ReviewSlotManager\ReviewSlotManager.csproj
```

3. Commit cả:
- File migration mới trong RepositoryLayer/Migrations
- Code thay đổi liên quan entity/repository/service

## 6. API chính

Base route: /api

- GET /api/Slots
- GET /api/Slots/round/{roundId}
- GET /api/ReviewRounds
- GET /api/ReviewRounds/open
- GET /api/GroupSlotRegistrations
- POST /api/GroupSlotRegistrations
- PUT /api/GroupSlotRegistrations/{registrationId}/cancel
- GET /api/ReviewerSlotRegistrations
- POST /api/ReviewerSlotRegistrations
- PUT /api/ReviewerSlotRegistrations/{registrationId}/cancel

## 7. Seed dữ liệu mẫu

Khi DB chưa có dữ liệu users, hệ thống tự seed:
- 5 users (Moderator, GVHD, GV Review, 2 Students)
- 1 semester
- 1 review round mở đăng ký
- 2 groups, 2 group members
- 1 reviewer slot config
- 2 slots

Logic seed nằm tại:
- RepositoryLayer/Data/DbSeeder.cs

## 8. Troubleshooting nhanh

1. API crash khi startup do DB chưa sẵn sàng
- Đã có retry migration trong Program.cs.
- Kiểm tra lại sqlserver container có healthy chưa.

2. Lỗi login SQL Server
- Kiểm tra User ID/Password trong connection string.
- Nếu dùng Docker compose mặc định, password là: YourStrong@Passw0rd

3. Chạy lệnh EF bị lỗi startup project
- Đảm bảo luôn truyền cả --project và --startup-project đúng như hướng dẫn ở trên.

## 9. Gợi ý workflow làm việc nhóm

- Mỗi tính năng tạo branch riêng.
- Nếu đổi schema DB, luôn thêm migration trong cùng PR.
- Trước khi push: chạy docker compose up hoặc dotnet build để kiểm tra compile.
- Review PR tập trung vào business rule trong ServiceLayer và transaction logic trong RepositoryLayer.
