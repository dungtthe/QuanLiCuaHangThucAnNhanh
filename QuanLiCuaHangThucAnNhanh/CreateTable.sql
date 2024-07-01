USE MASTER;

GO
DROP DATABASE QuanLiCuaHangThucAnNhanh;
GO

GO
CREATE DATABASE QuanLiCuaHangThucAnNhanh;
GO

GO
USE QuanLiCuaHangThucAnNhanh;
GO


CREATE TABLE THAMSO (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    HeSoBan FLOAT DEFAULT 1.2
);
GO

GO
CREATE TABLE KhachHang (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(500),
    NgaySinh SMALLDATETIME,
    SoDienThoai VARCHAR(50) UNIQUE,
    Email VARCHAR(500) UNIQUE,
    DiaChi NVARCHAR(1000),
    DiemTichLuy INT DEFAULT 0,
	IsDeleted BIT DEFAULT 0
);



CREATE TABLE NguoiDung (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(500),
    NgaySinh SMALLDATETIME,
    SoDienThoai VARCHAR(50) UNIQUE,
    Email VARCHAR(500) UNIQUE,
    DiaChi NVARCHAR(1000),
    TenTaiKhoan VARCHAR(255) NOT NULL UNIQUE,
	MatKhau VARCHAR(255) NOT NULL,
    Loai INT DEFAULT 0, --0 là nhân viên, 1 là quản lí
	Image VARBINARY(MAX),
	IsDeleted BIT DEFAULT 0
);



CREATE TABLE DanhMucSanPham (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(MAX),
	IsDeleted BIT DEFAULT 0
);

CREATE TABLE SanPham (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    TenSP NVARCHAR(500),
    SoLuongTon INT NOT NULL,
    GiaNhap MONEY NOT NULL,
    DanhMucSanPhamID INT NOT NULL,
	Image VARBINARY(MAX),
	IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (DanhMucSanPhamID) REFERENCES DanhMucSanPham(ID)
);



CREATE TABLE HoaDonNhap (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    NgayTao SMALLDATETIME DEFAULT GETDATE(),
    TongTienNhap MONEY NOT NULL DEFAULT 0,
    NguoiDungID INT NOT NULL, 
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (NguoiDungID) REFERENCES NguoiDung(ID)
);
GO


CREATE TABLE ChiTietHoaDonNhap (
    HoaDonNhapID INT NOT NULL,
    SanPhamID INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia MONEY NOT NULL,
	IsDeleted BIT DEFAULT 0
    PRIMARY KEY (HoaDonNhapID, SanPhamID),
    FOREIGN KEY (HoaDonNhapID) REFERENCES HoaDonNhap(ID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(ID)
);

CREATE TABLE HoaDonBan (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    NgayTao SMALLDATETIME DEFAULT GETDATE(),
    TongTienBan MONEY NOT NULL DEFAULT 0,
    NguoiDungID INT NOT NULL, 
    KhachHangID INT NOT NULL, 
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (NguoiDungID) REFERENCES NguoiDung(ID),
    FOREIGN KEY (KhachHangID) REFERENCES KhachHang(ID)
);
GO


CREATE TABLE ChiTietHoaDonBan (
    HoaDonBanID INT NOT NULL,
    SanPhamID INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGia MONEY NOT NULL,
    IsDeleted BIT DEFAULT 0,
    PRIMARY KEY (HoaDonBanID, SanPhamID),
    FOREIGN KEY (HoaDonBanID) REFERENCES HoaDonBan(ID),
    FOREIGN KEY (SanPhamID) REFERENCES SanPham(ID)
);






--insert dữ liệu
INSERT INTO THAMSO DEFAULT VALUES;

-- mật khẩu là admin
INSERT INTO NguoiDung (HoTen, NgaySinh, SoDienThoai, Email, DiaChi, TenTaiKhoan, MatKhau, Loai) 
VALUES (N'DungTTIT', '1990-01-01', '0987654321', 'dung@gmail.com', N'Thái Bình', 'admin', '21232f297a57a5a743894a0e4a801fc3', 1);


INSERT INTO DanhMucSanPham (TenDanhMuc) 
VALUES 
(N'Thức ăn'),
(N'Đồ uống');


GO

GO
