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

## 2. Quick Start (Docker - 30 giây)

```powershell
# Clone/pull code
cd path-to-repo

# Khởi động
docker compose up -d --build

# Chờ ~30s, rồi truy cập
http://localhost:8080/swagger/index.html
```

**Xong!** Có thể test API ngay trên Swagger UI.

## 3. Cấu trúc thư mục chính

- ReviewSlotManager.sln
- ReviewSlotManager/
- ServiceLayer/
- RepositoryLayer/
- docker-compose.yml

## 4. Chạy bằng Docker (khuyến nghị cho team)

### 4.1 Stack dịch vụ

- **api**: ASP.NET Core API (.NET 9)
- **sqlserver**: SQL Server 2022 Express

### 4.2 Setup & Start

**Bước 1: Build images và khởi động containers**

```powershell
docker compose up -d --build
```

Lần đầu sẽ mất 1-2 phút để:
- Pull/build images
- Khởi động SQL Server
- API tự chạy migration
- Seed dữ liệu mẫu (nếu DB rỗng)

**Bước 2: Kiểm tra trạng thái**

```powershell
docker compose ps
```

Kỳ vọng:
- `api`: UP (healthy sau 10-15 giây)
- `sqlserver`: UP (healthy sau 20-30 giây)

**Bước 3: Xem log API**

```powershell
docker compose logs -f api
```

Chờ đến khi thấy dòng:
```
Now listening on: http://0.0.0.0:8000
```

### 4.3 Truy cập API

Swagger UI (Interactive API docs):
- **URL**: http://localhost:8080/swagger/index.html
- **Direct links**:
  - http://localhost:8080/api/Slots
  - http://localhost:8080/api/ReviewRounds
  - http://localhost:8080/api/GroupSlotRegistrations

Health check:
```powershell
curl http://localhost:8080/api/ReviewRounds
```

### 4.4 Quản lý containers

**Xem chi tiết logs**:
```powershell
docker compose logs -f
```

**Khởi động lại API (không reset DB)**:
```powershell
docker compose restart api
```

**Dừng dịch vụ (giữ lại dữ liệu)**:
```powershell
docker compose down
```

**Dừng & xóa luôn dữ liệu DB (volume)**:
```powershell
docker compose down -v
```

**Xóa luôn images**:
```powershell
docker compose down -v --rmi all
```

### 4.5 Troubleshooting Docker

| Vấn đề | Nguyên nhân | Giải pháp |
|--------|-----------|----------|
| API crash sau khi start | DB chưa sẵn sàng | Chờ 30s, API tự retry. Xem `docker compose logs api` |
| Swagger không truy cập được | API chưa healthy | Kiểm tra `docker compose ps`, chờ UP |
| Lỗi port 8080 đã dùng | Port conflict | Đóng ứng dụng khác hoặc sửa port trong docker-compose.yml |
| DB không seed dữ liệu | Migration chưa xong | Xem `docker compose logs sqlserver` |
| Muốn reset DB | Cần xóa volume | Chạy `docker compose down -v` |

## 5. Chạy local không Docker

### 5.1 Chuẩn bị SQL Server

Cập nhật connection string trong file:
- ReviewSlotManager/appsettings.json

Key đang dùng:
- ConnectionStrings:DefaultConnectionString

### 5.2 Tạo/Update database

```powershell
dotnet ef database update --project .\RepositoryLayer\RepositoryLayer.csproj --startup-project .\ReviewSlotManager\ReviewSlotManager.csproj
```

### 5.3 Chạy API

```powershell
dotnet run --project .\ReviewSlotManager\ReviewSlotManager.csproj
```

Swagger URL local: xem theo output khi chạy (thường là https://localhost:<port>/swagger).

## 6. Workflow migration cho team

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

## 7. API chính

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

## 8. Seed dữ liệu mẫu

Khi DB chưa có dữ liệu users, hệ thống tự seed:
- 5 users (Moderator, GVHD, GV Review, 2 Students)
- 1 semester
- 1 review round mở đăng ký
- 2 groups, 2 group members
- 1 reviewer slot config
- 2 slots

Logic seed nằm tại:
- RepositoryLayer/Data/DbSeeder.cs

## 9. Troubleshooting nhanh

### Local development (không Docker)

1. **API crash khi startup do DB chưa sẵn sàng**
   - Đã có retry migration trong Program.cs.
   - Kiểm tra lại SQL Server instance có chạy và accessible không.

2. **Lỗi login SQL Server**
   - Kiểm tra User ID/Password trong connection string (appsettings.json).
   - Windows Auth: `Server=localhost;Database=ReviewSlotDb;Trusted_Connection=true;`
   - SQL Auth: `Server=localhost;Database=ReviewSlotDb;User Id=sa;Password=...;`

3. **Chạy lệnh EF bị lỗi startup project**
   - Đảm bảo luôn truyền cả --project và --startup-project đúng như hướng dẫn ở trên.

### Docker issues

4. **API không healthy sau 30s**
   ```powershell
   docker compose logs api | findstr "ERROR\|Exception"
   ```
   - Thường là DB chưa sẵn sàng, chờ thêm
   - Hoặc connection string sai (check docker-compose.yml)

5. **SQL Server container khởi động chậm (lần đầu)**
   - Bình thường mất 20-40s
   - Nếu quá 60s, kiểm tra:
     ```powershell
     docker compose logs sqlserver | tail -20
     ```

6. **"Bind for 0.0.0.0:8080 failed"**
   - Port 8080 đã bị dùng. Giải pháp:
     ```powershell
     # Tìm process dùng port 8080
     netstat -ano | findstr :8080
     # Hoặc sửa port trong docker-compose.yml
     ports:
       - "8081:8000"  # External:Internal
     ```

7. **Database seed fails hoặc data không đúng**
   - Xóa volume và rebuild:
     ```powershell
     docker compose down -v
     docker compose up -d --build
     ```

## 10. Gợi ý workflow làm việc nhóm

- Mỗi tính năng tạo branch riêng.
- Nếu đổi schema DB, luôn thêm migration trong cùng PR.
- Trước khi push: chạy docker compose up hoặc dotnet build để kiểm tra compile.
- Review PR tập trung vào business rule trong ServiceLayer và transaction logic trong RepositoryLayer.
