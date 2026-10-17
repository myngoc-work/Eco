# Hệ Thống Quản Lý Quy Trình Môi Trường (QLQTMT)

Hệ thống quản lý toàn diện cho các hợp đồng đo lường và phân tích môi trường, hỗ trợ quản lý từ khâu ký hợp đồng đến xuất báo cáo kết quả.

## Mục Lục

- [Tổng Quan](#tổng-quan)
- [Tính Năng Chính](#tính-năng-chính)
- [Yêu Cầu Hệ Thống](#yêu-cầu-hệ-thống)
- [Cài Đặt](#cài-đặt)
- [Cấu Hình](#cấu-hình)
- [Hướng Dẫn Sử Dụng](#hướng-dẫn-sử-dụng)
- [Cấu Trúc Dự Án](#cấu-trúc-dự-án)
- [Công Nghệ Sử Dụng](#công-nghệ-sử-dụng)
- [Đóng Góp](#đóng-góp)
- [Giấy Phép](#giấy-phép)

## Tổng Quan

Hệ thống QLQTMT được thiết kế để quản lý toàn bộ quy trình làm việc của một công ty môi trường, bao gồm:

- **Quản lý hợp đồng**: Tạo, theo dõi và quản lý hợp đồng với khách hàng
- **Quản lý đơn hàng**: Theo dõi các đơn hàng đo lường môi trường
- **Quản lý mẫu**: Quản lý các nền mẫu (Khí thải, Nước thải, Không khí, Tiếng ồn)
- **Quản lý kết quả**: Nhập và quản lý kết quả đo lường từ các phòng ban
- **Xuất báo cáo**: Tự động tạo PDF báo cáo và gửi email cho khách hàng
- **Dự đoán AI**: Sử dụng Machine Learning để dự đoán khả năng tái ký hợp đồng

## Tính Năng Chính

### Xác Thực và Bảo Mật

- Đăng nhập với username/password
- Xác thực 2 yếu tố (2FA) bằng TOTP
- Quản lý mật khẩu và reset mật khẩu
- Phân quyền theo vai trò (Admin, User)

### Quản Lý Hợp Đồng

- Tạo và quản lý hợp đồng bán hàng
- Theo dõi trạng thái hợp đồng (Đang thực hiện, Hoàn thành, Quá hạn)
- Quản lý thông tin khách hàng
- Tự động tính ngày dự kiến trả kết quả (20 ngày sau ngày ký)

### Quản Lý Đơn Hàng và Mẫu

- Tạo và quản lý các nền mẫu (KT, NT, KK, TN)
- Phân công thông số cho các phòng ban (Kế hoạch, Hiện trường, Phòng thí nghiệm)
- Tạo lô đo và quản lý lịch đo
- Theo dõi tiến độ hoàn thành

### Quản Lý Kết Quả

- Nhập kết quả đo từ phòng kế hoạch và phòng thí nghiệm
- Tự động kiểm tra kết quả vượt tiêu chuẩn
- Quản lý ghi chú cho từng thông số
- Cập nhật thời gian hoàn thành tự động

### Xuất Báo Cáo

- Tự động tạo PDF báo cáo cho từng nền mẫu
- Bao gồm thông tin chi tiết: Batch ID, ngày lấy mẫu, kết quả đo, QCVN
- Chữ ký điện tử của các trưởng phòng
- Gửi email tự động với file PDF đính kèm

### AI Dự Đoán

- Dự đoán khả năng tái ký hợp đồng dựa trên lịch sử
- Sử dụng Machine Learning (ML.NET)
- Hiển thị phần trăm khả năng tái ký cho từng khách hàng

### Thông Báo

- Gửi email tự động cho hợp đồng quá hạn
- Thông báo khi có kết quả mới
- Dashboard hiển thị thống kê real-time

### Thống Kê và Báo Cáo

- Dashboard tổng quan với các card thống kê
- Biểu đồ doanh thu và số lượng đơn hàng
- Lọc theo khoảng thời gian
- Export dữ liệu

## Yêu Cầu Hệ Thống

### Phần Mềm

- **.NET Framework 4.8** hoặc cao hơn
- **SQL Server 2019** hoặc cao hơn
- **Visual Studio 2019/2022** (để phát triển)

### Hệ Điều Hành

- Windows 10/11 hoặc Windows Server 2016+

### Phần Cứng Tối Thiểu

- RAM: 4GB (khuyến nghị 8GB)
- Ổ cứng: 2GB dung lượng trống
- CPU: Dual-core 2.0GHz trở lên

## Cài Đặt

### 1. Clone Repository

```bash
git clone https://github.com/KimNgan0906/enviroment.git
cd enviroment
```

### 2. Tạo Database

1. Mở SQL Server Management Studio
2. Chạy file `database_QLQTMT.sql` để tạo database và các bảng
3. Kiểm tra database `QLQTMT` đã được tạo thành công

### 3. Cấu Hình Connection String

Mở file `GUI/App.config` và cập nhật connection string:

```xml
<connectionStrings>
  <add name="db_QLQTMT_Connection" 
       connectionString="Server=YOUR_SERVER_NAME;Database=QLQTMT;Trusted_Connection=True;TrustServerCertificate=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 4. Cấu Hình Email (Tùy chọn)

Cập nhật thông tin SMTP trong `GUI/App.config`:

```xml
<appSettings>
  <add key="SmtpHost" value="smtp.gmail.com" />
  <add key="SmtpPort" value="587" />
  <add key="EnableSsl" value="true" />
  <add key="FromEmail" value="your-email@gmail.com" />
  <add key="SmtpUsername" value="your-email@gmail.com" />
  <add key="SmtpPassword" value="your-app-password" />
</appSettings>
```

### 5. Build và Chạy

1. Mở file `PR_QuanLyHopDong.sln` trong Visual Studio
2. Restore NuGet packages (click chuột phải vào Solution → Restore NuGet Packages)
3. Build solution (Ctrl+Shift+B)
4. Chạy project `GUI` (F5)

## Cấu Hình

### Cấu Hình Chữ Ký PDF

Để tùy chỉnh tên người ký trong PDF, thêm vào `GUI/App.config`:

```xml
<appSettings>
  <add key="PlanningHeadNameOverride" value="Tên Trưởng Phòng Kế Hoạch" />
  <add key="LabHeadNameOverride" value="Tên Trưởng Phòng Thí Nghiệm" />
  <add key="SalesHeadNameOverride" value="Tên Trưởng Phòng Kinh Doanh" />
</appSettings>
```

Nếu để trống, hệ thống sẽ tự động lấy từ database.

## Hướng Dẫn Sử Dụng

### Đăng Nhập

1. Mở ứng dụng
2. Nhập Username và Password
3. Nếu đã bật 2FA, nhập mã TOTP từ ứng dụng xác thực
4. Click "Đăng nhập"

### Quản Lý Hợp Đồng

1. Vào tab **"Quản lý hợp đồng"**
2. Click **"Thêm hợp đồng"** để tạo mới
3. Điền thông tin: Khách hàng, Ngày ký, Ngày dự kiến trả
4. Click **"Lưu"**

### Thêm Nền Mẫu

1. Chọn hợp đồng cần thêm nền mẫu
2. Click **"Thêm nền mẫu"**
3. Nhập tên nền mẫu
4. Chọn loại mẫu (Khí thải, Nước thải, Không khí, Tiếng ồn)
5. Mã nền mẫu sẽ tự động được tạo (KT01, NT01, KK01, TN01...)

### Thêm Thông Số

1. Chọn nền mẫu
2. Click **"Thêm thông số"**
3. Chọn thông số từ danh sách
4. Phân công cho phòng ban (HT hoặc PT)
5. Click **"Lưu"**

### Tạo Lô Đo

1. Chọn nền mẫu
2. Click **"Tạo lô đo"**
3. Chọn ngày bắt đầu
4. Click **"Tạo"**

### Nhập Kết Quả

1. Vào tab **"Thông số"**
2. Chọn nền mẫu và lô đo
3. Nhập kết quả vào cột **"Kết quả"**
4. Thêm ghi chú nếu cần
5. Click **"Lưu"**

### Xuất Báo Cáo PDF

1. Chọn hợp đồng
2. Click **"Gửi email"** hoặc **"In báo cáo"**
3. Hệ thống sẽ tự động tạo PDF cho từng nền mẫu
4. Nếu chọn gửi email, PDF sẽ được gửi đến email khách hàng

### Xem Thống Kê

1. Vào tab **"Trang chủ"** để xem dashboard tổng quan
2. Vào tab **"Thống kê"** để xem biểu đồ và lọc theo thời gian
3. Click **"Thống kê"** để áp dụng bộ lọc

## Cấu Trúc Dự Án

```
enviroment/
├── GUI/                    # Giao diện người dùng (WinForms)
│   ├── Authentication/     # Form đăng nhập, 2FA
│   ├── Main/              # Form chính và các UserControl
│   │   ├── Admin/         # Quản lý Admin
│   │   └── User/          # Quản lý User
│   └── Resources/         # Hình ảnh, icon
├── BLL/                    # Business Logic Layer
│   ├── Admin/             # Logic nghiệp vụ Admin
│   └── User/              # Logic nghiệp vụ User
├── DAL/                    # Data Access Layer
│   ├── Admin/             # Truy cập dữ liệu Admin
│   ├── User/              # Truy cập dữ liệu User
│   └── core/              # DatabaseHelper
├── DTO/                    # Data Transfer Objects
│   └── User/              # Các DTO cho User
├── MLModel/                # Model AI dự đoán
├── TrainingApp/            # Ứng dụng training model
├── AIPrediction/           # Dự án AI prediction
└── database_QLQTMT.sql     # Script tạo database
```

## Công Nghệ Sử Dụng

### Backend

- **.NET Framework 4.8** - Framework chính
- **C#** - Ngôn ngữ lập trình
- **SQL Server** - Database
- **ML.NET** - Machine Learning

### Frontend

- **Windows Forms** - Giao diện desktop
- **Guna.UI2** - UI Framework
- **iTextSharp** - Tạo PDF

### Libraries & Packages

- **Microsoft.Data.SqlClient** - Kết nối SQL Server
- **Otp.NET** - Tạo mã TOTP cho 2FA
- **QRCoder** - Tạo QR code
- **NAudio** - Xử lý âm thanh
- **Vosk** - Nhận dạng giọng nói
- **Newtonsoft.Json** - Xử lý JSON

## Phát Triển

### Thêm Tính Năng Mới

1. Tạo DTO trong `DTO/User/` nếu cần
2. Thêm method trong `DAL/User/` để truy cập database
3. Thêm logic nghiệp vụ trong `BLL/User/`
4. Tạo UI trong `GUI/Main/User/`

### Training Model AI

1. Chạy `TrainingApp` để train model mới
2. Model sẽ được lưu vào `MLModel/MLModel1.zip`
3. Model sẽ tự động được sử dụng trong ứng dụng

## Xử Lý Lỗi

### Lỗi Kết Nối Database

- Kiểm tra SQL Server đã chạy chưa
- Kiểm tra connection string trong `App.config`
- Kiểm tra quyền truy cập database

### Lỗi Gửi Email

- Kiểm tra cấu hình SMTP trong `App.config`
- Với Gmail, cần sử dụng App Password
- Kiểm tra firewall có chặn port 587 không

### Lỗi Timeout

- Đã tăng timeout lên 60 giây
- Nếu vẫn lỗi, kiểm tra database có bị lock không
- Kiểm tra network connection

## Ghi Chú

- Database connection string cần được cấu hình đúng trước khi chạy
- Email SMTP cần được cấu hình để sử dụng tính năng gửi email
- Model AI cần được train trước khi sử dụng tính năng dự đoán

---

**Phiên bản**: 1.0.0
**Cập nhật lần cuối**: 2025
