# 🌏 Vi Vu Việt Nam - Web Du Lịch ASP.NET Core

Website du lịch hoàn chỉnh được xây dựng bằng **ASP.NET Core 8 MVC** với phong cách thiết kế **thân thiện, gần gũi**.

## 📋 Thông tin dự án

- **Framework**: ASP.NET Core 8 MVC
- **ORM**: Entity Framework Core 8
- **Database**: SQL Server / LocalDB
- **Frontend**: Bootstrap 5 + Font Awesome 6
- **Authentication**: Cookie-based

## 🚀 Cách chạy dự án

### Yêu cầu
- Visual Studio 2022 (Community trở lên)
- .NET 8 SDK
- SQL Server hoặc SQL Server LocalDB (đã cài sẵn với VS)

### Bước 1: Mở Solution
Mở file `TravelWeb.sln` bằng Visual Studio 2022.

### Bước 2: Tạo Database

**Cách 1 - Dùng SQL script (khuyên dùng):**
1. Mở SQL Server Management Studio (SSMS) hoặc Azure Data Studio
2. Kết nối đến `(localdb)\mssqllocaldb`
3. Chạy file `Database/CreateDatabase.sql`

**Cách 2 - Dùng Entity Framework Migrations:**
Mở Package Manager Console trong Visual Studio và chạy:
```
Add-Migration InitialCreate
Update-Database
```

### Bước 3: Chạy ứng dụng
- Nhấn `F5` hoặc nút **Run** trong Visual Studio
- Hoặc chạy lệnh: `dotnet run` trong terminal

Web sẽ chạy tại: `https://localhost:5001`

## 👤 Tài khoản demo

| Tài khoản | Email | Mật khẩu |
|---|---|---|
| Admin | admin@vivuvietnam.vn | 123456 |
| Khách hàng | demo@gmail.com | 123456 |

## 📁 Cấu trúc thư mục

```
TravelWeb/
├── TravelWeb.sln                    # Solution file
├── TravelWeb/
│   ├── Controllers/                  # MVC Controllers
│   │   ├── HomeController.cs
│   │   ├── TourController.cs
│   │   ├── BookingController.cs
│   │   └── AccountController.cs
│   ├── Models/                       # Entity Models
│   │   ├── User.cs
│   │   ├── Tour.cs
│   │   ├── Destination.cs
│   │   ├── Category.cs
│   │   ├── Booking.cs
│   │   ├── Review.cs
│   │   ├── Blog.cs
│   │   └── Contact.cs
│   ├── Data/
│   │   └── TravelDbContext.cs       # EF DbContext
│   ├── Views/                        # Razor Views
│   │   ├── Shared/_Layout.cshtml
│   │   ├── Home/
│   │   ├── Tour/
│   │   ├── Booking/
│   │   └── Account/
│   ├── wwwroot/                      # Static files
│   │   ├── css/site.css
│   │   ├── js/site.js
│   │   └── images/
│   ├── Program.cs                    # Entry point
│   ├── appsettings.json             # Configuration
│   └── TravelWeb.csproj
└── Database/
    └── CreateDatabase.sql           # SQL script
```

## ✨ Tính năng chính

### Cho khách hàng
- 🏠 **Trang chủ**: Hero banner, tìm kiếm nhanh, tour nổi bật, điểm đến, danh mục, blog
- 🗺️ **Danh sách tour**: Lọc theo giá, điểm đến, danh mục, có phân trang
- 📋 **Chi tiết tour**: Thông tin đầy đủ, đánh giá, tour liên quan
- 🎫 **Đặt tour**: Form đặt tour với tính giá tự động, nhiều phương thức thanh toán
- ✅ **Xác nhận**: Trang xác nhận đẹp với mã booking
- 🧳 **Lịch sử**: Xem danh sách tour đã đặt
- 🔐 **Tài khoản**: Đăng ký, đăng nhập, đăng xuất (mã hóa SHA256)
- 📞 **Liên hệ**: Form gửi tin nhắn

### Tính năng kỹ thuật
- ✅ Entity Framework Core với Code-First
- ✅ Seed data tự động
- ✅ Cookie Authentication
- ✅ Session management
- ✅ Anti-forgery token (chống CSRF)
- ✅ Responsive design (mobile-friendly)
- ✅ Validation (DataAnnotations)
- ✅ TempData flash messages

## 🎨 Phong cách thiết kế

**Thân thiện, gần gũi** với:
- Màu sắc ấm áp: cam (#ff7e29), vàng (#ffc94d), hồng (#ff8fa3)
- Font Quicksand bo tròn, dễ đọc
- Bo góc mềm mại (border-radius lớn)
- Emoji và icon nhiều
- Hiệu ứng hover mượt mà
- Layout thoáng, dễ nhìn

## 🛠️ Mở rộng

Có thể bổ sung:
- Admin Dashboard (CRUD tour, user, booking)
- Hệ thống thanh toán thật (VNPay, MoMo)
- Gửi email xác nhận
- Tìm kiếm full-text
- Upload ảnh
- API REST/GraphQL
- SignalR cho chat real-time

## 📞 Hỗ trợ

Nếu có vấn đề khi chạy:
1. Đảm bảo đã cài .NET 8 SDK
2. Kiểm tra connection string trong `appsettings.json`
3. Chạy `dotnet restore` để tải packages
4. Chạy `dotnet build` để build project

---
© 2026 Vi Vu Việt Nam - Made with ❤️ in Vietnam
