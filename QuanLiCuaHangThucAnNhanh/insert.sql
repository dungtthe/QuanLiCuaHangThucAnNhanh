USE QuanLiCuaHangThucAnNhanh;
GO

-- Chèn dữ liệu vào bảng KhachHang
INSERT INTO KhachHang (HoTen, NgaySinh, SoDienThoai, Email, DiaChi, DiemTichLuy)
VALUES 
(N'Nguyen Van A', '1985-05-15', '0912345678', 'nguyenvana@gmail.com', N'123 Nguyen Trai, Ha Noi', 100),
(N'Tran Thi B', '1990-08-20', '0923456789', 'tranthib@gmail.com', N'456 Le Loi, Ho Chi Minh', 150);

-- Chèn dữ liệu vào bảng NguoiDung
INSERT INTO NguoiDung (HoTen, NgaySinh, SoDienThoai, Email, DiaChi, TenTaiKhoan, MatKhau, Loai) 
VALUES 
(N'Le Thi C', '1988-11-25', '0934567890', 'lethic@gmail.com', N'789 Tran Hung Dao, Da Nang', 'lethic', '5f4dcc3b5aa765d61d8327deb882cf99', 0), 
(N'Pham Van D', '1992-03-10', '0945678901', 'phamvand@gmail.com', N'123 Phan Dinh Phung, Hue', 'phamvand', '5f4dcc3b5aa765d61d8327deb882cf99', 0); 

-- Chèn dữ liệu vào bảng DanhMucSanPham
INSERT INTO DanhMucSanPham (TenDanhMuc) 
VALUES 
(N'Bánh mì'), 
(N'Nước ngọt');

-- Chèn dữ liệu vào bảng SanPham
INSERT INTO SanPham (TenSP, SoLuongTon, GiaNhap, DanhMucSanPhamID)
VALUES 
(N'Bánh mì pate', 50, 10000, 1), 
(N'Bánh mì trứng', 40, 12000, 1),
(N'Coca Cola', 100, 8000, 2),
(N'Pepsi', 80, 8000, 2);

-- Chèn dữ liệu vào bảng HoaDonNhap
INSERT INTO HoaDonNhap (TongTienNhap, NguoiDungID) 
VALUES 
(500000, 1),
(200000, 2);

-- Chèn dữ liệu vào bảng ChiTietHoaDonNhap
INSERT INTO ChiTietHoaDonNhap (HoaDonNhapID, SanPhamID, SoLuong, DonGia)
VALUES 
(1, 1, 50, 10000), 
(1, 2, 40, 12000), 
(2, 3, 100, 8000), 
(2, 4, 80, 8000);

-- Chèn dữ liệu vào bảng HoaDonBan
INSERT INTO HoaDonBan (TongTienBan, NguoiDungID, KhachHangID) 
VALUES 
(120000, 1, 2),
(80000, 2, 3);

-- Chèn dữ liệu vào bảng ChiTietHoaDonBan
INSERT INTO ChiTietHoaDonBan (HoaDonBanID, SanPhamID, SoLuong, DonGia)
VALUES 
(1, 1, 10, 12000), 
(1, 2, 5, 14000), 
(2, 3, 10, 10000), 
(2, 4, 5, 10000);

GO
