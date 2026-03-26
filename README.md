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

```
ReviewSlotManager.sln
docker-compose.yml
ReviewSlotManager/          # Web API (Controllers, Middlewares)
│   Controllers/
│   Middlewares/
ServiceLayer/               # Business logic
│   DTOs/
│   Mappings/
│   Services/
│   │   Interfaces/         # Service interfaces (tách riêng)
│   Settings/
│   Exceptions/
RepositoryLayer/            # Data access (EF Core)
│   Data/
│   Entities/
│   Enums/
│   Migrations/
│   Repositories/
│   │   Interfaces/         # Repository interfaces (tách riêng)
```

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

Base route: `/api`

Tất cả API theo chuẩn **Shopify REST** (GET list có phân trang, GET by id, GET count, POST create → 201, PUT update, DELETE → 204).

### Auth

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| POST | /api/Auth/login | Đăng nhập, trả JWT token |

### Semesters

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/Semesters | Danh sách semester (phân trang) |
| GET | /api/Semesters/{id} | Chi tiết semester |
| GET | /api/Semesters/count | Đếm tổng semester |
| GET | /api/Semesters/active | Semester đang active |
| POST | /api/Semesters | Tạo semester |
| PUT | /api/Semesters/{id} | Cập nhật semester |
| DELETE | /api/Semesters/{id} | Xóa semester |

### Users

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/Users | Danh sách users (phân trang) |
| GET | /api/Users/{id} | Chi tiết user |
| GET | /api/Users/count | Đếm tổng users |
| POST | /api/Users | Tạo user (password tự hash) |
| PUT | /api/Users/{id} | Cập nhật user |
| DELETE | /api/Users/{id} | Xóa user |

### Groups

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/Groups | Danh sách groups (phân trang) |
| GET | /api/Groups/{id} | Chi tiết group |
| GET | /api/Groups/count | Đếm tổng groups |
| GET | /api/Groups/semester/{semesterId} | Groups theo semester |
| POST | /api/Groups | Tạo group |
| PUT | /api/Groups/{id} | Cập nhật group |
| DELETE | /api/Groups/{id} | Xóa group |

### GroupMembers

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/GroupMembers | Danh sách members (phân trang) |
| GET | /api/GroupMembers/{id} | Chi tiết member |
| GET | /api/GroupMembers/count | Đếm tổng members |
| GET | /api/GroupMembers/group/{groupId} | Members theo group |
| POST | /api/GroupMembers | Thêm member vào group |
| DELETE | /api/GroupMembers/{id} | Xóa member |

### Slots

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/Slots | Danh sách slots (phân trang) |
| GET | /api/Slots/{id} | Chi tiết slot |
| GET | /api/Slots/round/{roundId} | Slots theo review round |
| POST | /api/Slots | Tạo slot |
| PUT | /api/Slots/{id} | Cập nhật slot |
| DELETE | /api/Slots/{id} | Xóa slot |

### ReviewRounds

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/ReviewRounds | Danh sách review rounds (phân trang) |
| GET | /api/ReviewRounds/{id} | Chi tiết review round |
| GET | /api/ReviewRounds/open | Các round đang mở đăng ký |
| POST | /api/ReviewRounds | Tạo review round |
| PUT | /api/ReviewRounds/{id} | Cập nhật review round |
| DELETE | /api/ReviewRounds/{id} | Xóa review round |

### ReviewerSlotConfigs

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/ReviewerSlotConfigs | Danh sách configs (phân trang) |
| GET | /api/ReviewerSlotConfigs/{id} | Chi tiết config |
| GET | /api/ReviewerSlotConfigs/round/{roundId} | Configs theo round |
| POST | /api/ReviewerSlotConfigs | Tạo config |
| PUT | /api/ReviewerSlotConfigs/{id} | Cập nhật config |
| DELETE | /api/ReviewerSlotConfigs/{id} | Xóa config |

### GroupSlotRegistrations

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/GroupSlotRegistrations | Danh sách đăng ký (phân trang) |
| GET | /api/GroupSlotRegistrations/{id} | Chi tiết đăng ký |
| GET | /api/GroupSlotRegistrations/count | Đếm tổng đăng ký |
| GET | /api/GroupSlotRegistrations/slot/{slotId} | Đăng ký theo slot |
| GET | /api/GroupSlotRegistrations/group/{groupId} | Đăng ký theo group |
| POST | /api/GroupSlotRegistrations | Nhóm đăng ký slot |
| PUT | /api/GroupSlotRegistrations/{registrationId}/cancel | Hủy đăng ký |

### ReviewerSlotRegistrations

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/ReviewerSlotRegistrations | Danh sách đăng ký (phân trang) |
| GET | /api/ReviewerSlotRegistrations/{id} | Chi tiết đăng ký |
| GET | /api/ReviewerSlotRegistrations/count | Đếm tổng đăng ký |
| GET | /api/ReviewerSlotRegistrations/slot/{slotId} | Đăng ký theo slot |
| GET | /api/ReviewerSlotRegistrations/reviewer/{reviewerId} | Đăng ký theo reviewer |
| POST | /api/ReviewerSlotRegistrations | Reviewer đăng ký slot |
| PUT | /api/ReviewerSlotRegistrations/{registrationId}/cancel | Hủy đăng ký |

### Notifications

| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | /api/Notifications | Danh sách thông báo (phân trang) |
| GET | /api/Notifications/{id} | Chi tiết thông báo |
| GET | /api/Notifications/count | Đếm tổng thông báo |
| GET | /api/Notifications/user/{userId} | Thông báo theo user |
| POST | /api/Notifications | Tạo thông báo |
| PUT | /api/Notifications/{id}/read | Đánh dấu đã đọc |
| PUT | /api/Notifications/user/{userId}/read-all | Đánh dấu tất cả đã đọc |
| DELETE | /api/Notifications/{id} | Xóa thông báo |

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

## 11. Changelog

### Branch: `api/login-CRUD-basic`

**Thêm mới CRUD APIs cho tất cả entities theo chuẩn Shopify REST:**

- **Semesters** — full CRUD + GET active semester
- **Users** — full CRUD (password tự hash bằng BCrypt)
- **Groups** — full CRUD + GET theo semester
- **GroupMembers** — full CRUD + GET theo group
- **ReviewerSlotConfigs** — full CRUD + GET theo round
- **Notifications** — full CRUD + đánh dấu đã đọc (1 hoặc tất cả)

**Mở rộng APIs đã có:**

- **Slots** — thêm POST create, PUT update, DELETE
- **ReviewRounds** — thêm POST create, PUT update, DELETE
- **GroupSlotRegistrations** — thêm GET by id, GET count, GET by slot, GET by group
- **ReviewerSlotRegistrations** — thêm GET by id, GET count, GET by slot, GET by reviewer

**DTOs mới:**

- CreateSemesterDto, UpdateSemesterDto, SemesterDto
- CreateUserDto, UpdateUserDto, UserDto
- CreateGroupDto, UpdateGroupDto, GroupDto
- CreateGroupMemberDto, GroupMemberDto
- CreateReviewerSlotConfigDto, UpdateReviewerSlotConfigDto, ReviewerSlotConfigDto
- CreateNotificationDto, NotificationDto
- CreateSlotDto, UpdateSlotDto
- CreateReviewRoundDto, UpdateReviewRoundDto

**Tái cấu trúc code:**

- Tách interface Service vào thư mục riêng: `ServiceLayer/Services/Interfaces/`
- Tách interface Repository vào thư mục riêng: `RepositoryLayer/Repositories/Interfaces/`
- Mở rộng `BaseService` hỗ trợ Create, Update, Delete (trước đó chỉ có Read)
- Mở rộng `BaseController` thêm `GetById` endpoint

**Tổng cộng: ~70 API endpoints (từ 10 lên 70+)**
