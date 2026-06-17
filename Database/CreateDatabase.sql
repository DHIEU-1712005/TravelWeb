-- ==============================================
-- DATABASE: TravelWebDB
-- Mô tả: Database cho web du lịch Vi Vu Việt Nam
-- ==============================================

CREATE DATABASE TravelWebDB;
GO

USE TravelWebDB;
GO

-- 1. ROLES
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255)
);

-- 2. USERS
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    DateOfBirth DATE,
    Gender NVARCHAR(10),
    AvatarUrl NVARCHAR(255),
    RoleId INT DEFAULT 2,
    Status NVARCHAR(20) DEFAULT 'active',
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 3. CATEGORIES
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Slug NVARCHAR(100) UNIQUE,
    Icon NVARCHAR(255),
    Description NVARCHAR(MAX)
);

-- 4. DESTINATIONS
CREATE TABLE Destinations (
    DestinationId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(150) NOT NULL,
    Slug NVARCHAR(150) UNIQUE,
    Country NVARCHAR(100) DEFAULT N'Việt Nam',
    City NVARCHAR(100),
    Region NVARCHAR(100),
    Description NVARCHAR(MAX),
    ThumbnailUrl NVARCHAR(255),
    ViewCount INT DEFAULT 0,
    IsFeatured BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 5. TOURS
CREATE TABLE Tours (
    TourId INT PRIMARY KEY IDENTITY(1,1),
    TourCode NVARCHAR(50),
    Name NVARCHAR(200) NOT NULL,
    Slug NVARCHAR(200),
    DestinationId INT FOREIGN KEY REFERENCES Destinations(DestinationId),
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    Description NVARCHAR(MAX),
    Detail NVARCHAR(MAX),
    DurationDays INT NOT NULL,
    DurationNights INT,
    DepartureLocation NVARCHAR(150),
    PriceAdult DECIMAL(12,2) NOT NULL,
    PriceChild DECIMAL(12,2),
    DiscountPercent DECIMAL(5,2) DEFAULT 0,
    MaxParticipants INT DEFAULT 30,
    TransportType NVARCHAR(50),
    Includes NVARCHAR(MAX),
    Excludes NVARCHAR(MAX),
    Notes NVARCHAR(MAX),
    ThumbnailUrl NVARCHAR(255),
    Status NVARCHAR(20) DEFAULT 'active',
    RatingAvg DECIMAL(3,2) DEFAULT 0,
    ViewCount INT DEFAULT 0,
    BookingCount INT DEFAULT 0,
    IsFeatured BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 6. BOOKINGS
CREATE TABLE Bookings (
    BookingId INT PRIMARY KEY IDENTITY(1,1),
    BookingCode NVARCHAR(20),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    TourId INT FOREIGN KEY REFERENCES Tours(TourId),
    NumAdults INT NOT NULL DEFAULT 1,
    NumChildren INT DEFAULT 0,
    TotalAmount DECIMAL(12,2),
    FinalAmount DECIMAL(12,2),
    ContactName NVARCHAR(100) NOT NULL,
    ContactPhone NVARCHAR(20) NOT NULL,
    ContactEmail NVARCHAR(100) NOT NULL,
    SpecialRequests NVARCHAR(MAX),
    DepartureDate DATETIME,
    BookingStatus NVARCHAR(20) DEFAULT 'pending',
    PaymentStatus NVARCHAR(20) DEFAULT 'unpaid',
    PaymentMethod NVARCHAR(50),
    BookedAt DATETIME DEFAULT GETDATE()
);

-- 7. REVIEWS
CREATE TABLE Reviews (
    ReviewId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    TourId INT FOREIGN KEY REFERENCES Tours(TourId),
    Title NVARCHAR(200),
    Content NVARCHAR(MAX),
    Rating TINYINT CHECK (Rating BETWEEN 1 AND 5),
    Status NVARCHAR(20) DEFAULT 'approved',
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 8. BLOGS
CREATE TABLE Blogs (
    BlogId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Slug NVARCHAR(255) UNIQUE,
    Category NVARCHAR(100),
    Summary NVARCHAR(MAX),
    Content NVARCHAR(MAX),
    ThumbnailUrl NVARCHAR(255),
    ViewCount INT DEFAULT 0,
    Status NVARCHAR(20) DEFAULT 'published',
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 9. CONTACTS
CREATE TABLE Contacts (
    ContactId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Subject NVARCHAR(200),
    Message NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'new',
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- ==============================================
-- DỮ LIỆU MẪU
-- ==============================================

-- Roles
INSERT INTO Roles (RoleName, Description) VALUES
(N'Admin', N'Quản trị viên'),
(N'Customer', N'Khách hàng'),
(N'Staff', N'Nhân viên');

-- Users (mật khẩu mặc định: 123456 - SHA256)
INSERT INTO Users (Username, Email, PasswordHash, FullName, Phone, RoleId) VALUES
(N'admin', N'admin@vivuvietnam.vn', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Quản trị viên', N'0901234567', 1),
(N'demo', N'demo@gmail.com', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Khách hàng Demo', N'0987654321', 2);

-- Categories
INSERT INTO Categories (Name, Slug, Icon) VALUES
(N'Biển đảo', N'bien-dao', N'🏖️'),
(N'Núi rừng', N'nui-rung', N'⛰️'),
(N'Văn hóa', N'van-hoa', N'🏛️'),
(N'Ẩm thực', N'am-thuc', N'🍜'),
(N'Mạo hiểm', N'mao-hiem', N'🎢');

-- Destinations
INSERT INTO Destinations (Name, Country, City, Region, IsFeatured, Description) VALUES
(N'Hạ Long', N'Việt Nam', N'Quảng Ninh', N'Miền Bắc', 1, N'Vịnh Hạ Long - kỳ quan thiên nhiên thế giới'),
(N'Đà Nẵng', N'Việt Nam', N'Đà Nẵng', N'Miền Trung', 1, N'Thành phố đáng sống nhất Việt Nam'),
(N'Phú Quốc', N'Việt Nam', N'Kiên Giang', N'Miền Nam', 1, N'Đảo ngọc Phú Quốc thiên đường nghỉ dưỡng'),
(N'Sapa', N'Việt Nam', N'Lào Cai', N'Miền Bắc', 1, N'Sapa - thị trấn trong mây'),
(N'Hội An', N'Việt Nam', N'Quảng Nam', N'Miền Trung', 1, N'Phố cổ Hội An di sản văn hóa thế giới'),
(N'Đà Lạt', N'Việt Nam', N'Lâm Đồng', N'Miền Trung', 1, N'Thành phố ngàn hoa Đà Lạt');

-- Tours
INSERT INTO Tours (TourCode, Name, DestinationId, CategoryId, DurationDays, DurationNights, PriceAdult, PriceChild, DiscountPercent, DepartureLocation, TransportType, IsFeatured, Description) VALUES
(N'HN-HL-3N2D', N'Hà Nội - Hạ Long 3 ngày 2 đêm', 1, 1, 3, 2, 3500000, 2500000, 10, N'Hà Nội', N'Xe khách', 1, N'Khám phá vẻ đẹp kỳ vĩ của Vịnh Hạ Long'),
(N'HCM-DN-4N3D', N'Đà Nẵng - Hội An - Bà Nà 4N3D', 2, 3, 4, 3, 5500000, 4000000, 15, N'TP.HCM', N'Máy bay', 1, N'Khám phá Đà Nẵng - Hội An - Bà Nà Hills'),
(N'HCM-PQ-3N2D', N'Phú Quốc - Đảo ngọc 3N2D', 3, 1, 3, 2, 4800000, 3500000, 20, N'TP.HCM', N'Máy bay', 1, N'Nghỉ dưỡng tại đảo ngọc Phú Quốc'),
(N'HN-SP-2N3D', N'Sapa - Fansipan 2N3D', 4, 2, 3, 2, 2800000, 2000000, 5, N'Hà Nội', N'Xe khách', 1, N'Chinh phục đỉnh Fansipan'),
(N'DN-HA-2N1D', N'Hội An - Phố cổ 2N1D', 5, 3, 2, 1, 1800000, 1200000, 0, N'Đà Nẵng', N'Xe khách', 0, N'Khám phá phố cổ Hội An'),
(N'HCM-DL-3N2D', N'Đà Lạt - Thành phố ngàn hoa 3N2D', 6, 4, 3, 2, 3200000, 2300000, 10, N'TP.HCM', N'Xe khách', 1, N'Tận hưởng khí hậu Đà Lạt');

-- Blogs
INSERT INTO Blogs (Title, Slug, Category, Summary, Content) VALUES
(N'10 điểm đến không thể bỏ qua tại Việt Nam 2026', N'10-diem-den-viet-nam-2026', N'Cẩm nang', N'Khám phá top 10 điểm đến hấp dẫn nhất Việt Nam', N'Việt Nam có rất nhiều điểm đến tuyệt vời...'),
(N'Kinh nghiệm du lịch Phú Quốc tự túc', N'kinh-nghiem-du-lich-phu-quoc', N'Kinh nghiệm', N'Tổng hợp kinh nghiệm du lịch Phú Quốc từ A-Z', N'Phú Quốc đảo ngọc...'),
(N'Ẩm thực Hội An - những món ăn đặc sản', N'am-thuc-hoi-an', N'Ẩm thực', N'Khám phá ẩm thực phố cổ Hội An', N'Cao lầu, mì Quảng, bánh mì Phượng...');

-- Sample Reviews
INSERT INTO Reviews (UserId, TourId, Title, Content, Rating) VALUES
(2, 1, N'Tour rất tuyệt vời', N'Cảnh đẹp, dịch vụ tốt, hướng dẫn viên nhiệt tình. Rất đáng để trải nghiệm!', 5),
(2, 3, N'Phú Quốc tuyệt vời', N'Bãi biển đẹp, hải sản tươi ngon, resort sang trọng. Sẽ quay lại!', 5);

PRINT N'✅ Database TravelWebDB đã được tạo thành công!';
PRINT N'📧 Tài khoản demo: demo@gmail.com / 123456';
PRINT N'🔐 Tài khoản admin: admin@vivuvietnam.vn / 123456';
GO
