USE master;
GO

IF DB_ID('QLQTMT') IS NOT NULL
BEGIN
    ALTER DATABASE QLQTMT SET SINGLE_USER WITH ROLLBACK IMMEDIATE; 
    DROP DATABASE QLQTMT;
END
GO

CREATE DATABASE QLQTMT;
GO

USE QLQTMT;
GO

CREATE TABLE [Role](
  RoleId varchar(10) PRIMARY KEY,
  RoleName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Department(
  DepartmentId varchar(10) PRIMARY KEY,
  DepartmentName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Company_User(
  Company_UserId varchar(10) PRIMARY KEY,
  Company_Username  NVARCHAR(50)  NOT NULL UNIQUE,
  PasswordHash  NVARCHAR(200) NOT NULL,
  FullName  NVARCHAR(100) NOT NULL,
  Email  NVARCHAR(100) NULL UNIQUE,
  Phone   NVARCHAR(20)  NULL UNIQUE,
  DateOfBirth DATE NULL,
  Gender NVARCHAR(10) NULL CHECK (Gender IN ('Nam', N'Nữ', 'Khác')),
  Address NVARCHAR(300) NULL,
  AvatarPath NVARCHAR(500) NULL,
  IsActive BIT NOT NULL DEFAULT(1),
  RoleId varchar(10) NOT NULL FOREIGN KEY REFERENCES [Role](RoleId),
  DepartmentId     varchar(10) NOT NULL FOREIGN KEY REFERENCES Department(DepartmentId),
  EmailVerifiedAt  DATETIME  NULL,
  PhoneVerifiedAt  DATETIME  NULL,
  LastLoginAt      DATETIME  NULL,
  TwoFactorSecret NVARCHAR(200) NULL,
  IsTwoFactorEnabled BIT NOT NULL DEFAULT(0)
);

CREATE TABLE Customer(
  CustomerId varchar(10) PRIMARY KEY,
  CustomerName NVARCHAR(200) NOT NULL,
  RepName NVARCHAR(100) NULL,
  Address NVARCHAR(300) NULL,
  RepEmail NVARCHAR(100) NULL,
  RepPhone NVARCHAR(20)  NULL
);

CREATE TABLE Contract(
  ContractId varchar(10) PRIMARY KEY,
  CustomerId varchar(10) NOT NULL FOREIGN KEY REFERENCES Customer(CustomerId),
  SignDate DATE NOT NULL,
  PlanDueDate  DATE NOT NULL,
  Amount DECIMAL(18,2) NULL,
  Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Planned','InProgress', 'Done', 'Cancelled')), 
  SalesCompany_UserId varchar(10) NULL FOREIGN KEY REFERENCES Company_User(Company_UserId)
);

CREATE TABLE ContractItem(
  ItemId varchar(10) PRIMARY KEY,
  ContractId varchar(10) NOT NULL FOREIGN KEY REFERENCES Contract(ContractId) ON DELETE CASCADE,
  SampleCode NVARCHAR(30)  NOT NULL,
  SampleName NVARCHAR(200) NULL,
  SampleLocation NVARCHAR(300) NULL,
  DepartmentId varchar(10) NULL FOREIGN KEY REFERENCES Department(DepartmentId),
  SampleType NVARCHAR(50) NULL CHECK (SampleType IN (N'Khí thải', N'Nước thải', N'Không khí', N'Tiếng ồn'))
);

CREATE TABLE Unit(
  UnitId varchar(10) PRIMARY KEY,
  Symbol NVARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE SampleParameter(
  ParameterId varchar(10) PRIMARY KEY,
  ParameterName   NVARCHAR(100) NOT NULL,
  UnitId varchar(10) NOT NULL FOREIGN KEY REFERENCES Unit(UnitId),
  DepartmentId varchar(10) NULL FOREIGN KEY REFERENCES Department(DepartmentId),
  MinValue DECIMAL(18,4) NULL,
  MaxValue DECIMAL(18,4) NULL,
  StandardCode NVARCHAR(30)  NULL
);

CREATE TABLE Assignment(
  ItemId varchar(10) NOT NULL FOREIGN KEY REFERENCES ContractItem(ItemId) ON DELETE CASCADE,
  ParameterId   varchar(10) NOT NULL FOREIGN KEY REFERENCES SampleParameter(ParameterId) ON DELETE CASCADE,
  DepartmentId varchar(10) NOT NULL FOREIGN KEY REFERENCES Department(DepartmentId),
  PRIMARY KEY (ItemId, ParameterId)
);

-- giới hạn/ghi chú theo từng nền mẫu - thông số cho Phòng Kế hoạch
CREATE TABLE ItemParameterSetting(
  ItemId varchar(10) NOT NULL FOREIGN KEY REFERENCES ContractItem(ItemId) ON DELETE CASCADE,
  ParameterId varchar(10) NOT NULL FOREIGN KEY REFERENCES SampleParameter(ParameterId) ON DELETE CASCADE,
  MinValue DECIMAL(18,4) NULL,
  MaxValue DECIMAL(18,4) NULL,
  Note NVARCHAR(500) NULL,
  SampleType NVARCHAR(50) NULL CHECK (SampleType IN (N'Khí thải', N'Nước thải', N'Không khí', N'Tiếng ồn')),
  UpdatedBy varchar(10) NULL FOREIGN KEY REFERENCES Company_User(Company_UserId),
  UpdatedAt DATETIME NOT NULL DEFAULT SYSDATETIME(),
  PRIMARY KEY (ItemId, ParameterId)
);

CREATE TABLE MeasurementBatch(
  BatchId varchar(10) PRIMARY KEY,
  ItemId varchar(10) NOT NULL FOREIGN KEY REFERENCES ContractItem(ItemId) ON DELETE CASCADE,
  StartDate DATETIME NOT NULL,
  EndDate DATETIME NULL,
  Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Planned','InProgress','Completed')),
  LeaderCompany_UserId  varchar(10) NULL FOREIGN KEY REFERENCES Company_User(Company_UserId)
);

CREATE TABLE Result(
  ResultId varchar(10) PRIMARY KEY,
  BatchId  varchar(10) NOT NULL FOREIGN KEY REFERENCES MeasurementBatch(BatchId) ON DELETE CASCADE,
  ParameterId varchar(10) NOT NULL FOREIGN KEY REFERENCES SampleParameter(ParameterId),
  Value DECIMAL(18,4),
  ImportAt DATETIME  NOT NULL DEFAULT SYSDATETIME(),
  Company_UserId varchar(10) NOT NULL FOREIGN KEY REFERENCES Company_User(Company_UserId),
  OutofStandard BIT NOT NULL DEFAULT(0),
  Note NVARCHAR(500) NULL
);

CREATE TABLE ResultReview(
  ReviewId varchar(10) PRIMARY KEY,
  BatchId  varchar(10) NOT NULL UNIQUE FOREIGN KEY REFERENCES MeasurementBatch(BatchId) ON DELETE CASCADE,
  ReviewedAt DATETIME NULL,
  ReviewerId  varchar(10) NULL FOREIGN KEY REFERENCES Company_User(Company_UserId),
  Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Pending','Approved','Rejected')),
  Comment NVARCHAR(500) NULL
);

CREATE TABLE Notification(
  NotificationId  varchar(10) PRIMARY KEY,
  Type NVARCHAR(30) NOT NULL,
  Message NVARCHAR(400) NOT NULL,
  CreatedAt DATETIME NOT NULL DEFAULT SYSDATETIME(),
  DaysLate INT NULL,
  ContractId varchar(10) NULL FOREIGN KEY REFERENCES Contract(ContractId) ,
  BatchId varchar(10) NULL FOREIGN KEY REFERENCES MeasurementBatch(BatchId) ,
  TargetCompany_UserId varchar(10) NULL FOREIGN KEY REFERENCES Company_User(Company_UserId),
  TargetDeptId varchar(10) NULL FOREIGN KEY REFERENCES Department(DepartmentId)
);

CREATE TABLE AuditLog(
  LogId varchar(10) PRIMARY KEY,
  Entity NVARCHAR(50) NOT NULL,
  EntityId varchar(10) NOT NULL,
  Action NVARCHAR(30) NOT NULL,
  ActorId  varchar(10) NOT NULL FOREIGN KEY REFERENCES Company_User(Company_UserId),
  CreatedAt    DATETIME2(0) NOT NULL DEFAULT SYSDATETIME()
);

CREATE TABLE VerificationToken(
  TokenId BIGINT IDENTITY(1,1) PRIMARY KEY,
  Company_UserId  varchar(10) NOT NULL FOREIGN KEY REFERENCES Company_User(Company_UserId) ON DELETE CASCADE,
  Type         NVARCHAR(30) NOT NULL,
  TokenValue   NVARCHAR(200) NOT NULL,
  Destination  NVARCHAR(100) NULL,
  ExpiresAt    DATETIME NOT NULL,
  ConsumedAt   DATETIME NULL,
  CreatedAt    DATETIME NOT NULL DEFAULT SYSDATETIME()
);


GO


-- Roles
INSERT INTO [Role] (RoleId, RoleName) VALUES 
('R001', 'Admin'), 
('R002', 'HeadOfDepartment'),
('R003', 'Employee');

-- Departments
INSERT INTO Department (DepartmentId, DepartmentName) VALUES 
('D001', N'Quản lý'), 
('D002', N'Phòng Kinh Doanh'),
('D003', N'Phòng Kế Hoạch'),
('D004', N'Phòng Hiện Trường'),
('D005', N'Phòng Thí Nghiệm'),
('D006', N'Phòng Kết Quả');

-- Users
DECLARE @HashedDefaultPassword NVARCHAR(200) = 'D17F25ECFBCC7857F7BEBEA469308BE0B2580943E96D13A3AD98A13675C4BFC2';
INSERT INTO Company_User (Company_UserId, Company_Username, PasswordHash, FullName, Email, Phone, DateOfBirth, Gender, Address, AvatarPath, RoleId, DepartmentId, IsActive, EmailVerifiedAt) VALUES
-- Admin
('Admin', 'admin', @HashedDefaultPassword, 'Admin KwAhn ne', 'dkimanh244@gmail.com', '0345390000', '2005-07-25', N'Nữ', N'Orionix, Quận 1, TP.HCM', NULL, 'R001', 'D001', 1, GETDATE()),

-- Trưởng Phòng Kinh Doanh
('TPKD', 'TP_KD', @HashedDefaultPassword, N'Trưởng Phòng Kinh Doanh', 'tvinhquang312@gmail.com', '0774451724', '1985-03-15', 'Nam', N'Quận 1, TP.HCM', NULL, 'R002', 'D002', 1, GETDATE())

-- Thêm khách hàng mẫu với email
INSERT INTO Customer (CustomerId, CustomerName, RepName, Address, RepEmail, RepPhone) VALUES
('KH001', N'Cty Môi trường xanh', N'Trần Văn An', N'123 Nguyễn Văn Linh, Quận 7, TP.HCM', 'tranvanan@moitruongxanh.com', '0909123456'),
('KH002', N'Cty ABC', N'Nguyễn Văn B', N'456 Lê Văn Việt, Quận 9, TP.HCM', 'nguyenvanb@ctyabc.com', '0909123457'),
('KH003', N'Cty XYZ', N'Lê Thị C', N'789 Nguyễn Thị Minh Khai, Quận 1, TP.HCM', 'lethic@ctyxyz.com', '0909123458'),
('KH004', N'Cty Công nghệ Việt', N'Phạm Minh D', N'321 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM', 'phamminhd@congngheviet.com', '0909123459'),
('KH005', N'Cty Xây dựng Hồng', N'Hoàng Thị E', N'654 Cách Mạng Tháng 8, Quận 10, TP.HCM', 'hoangthie@xaydunghong.com', '0909123460'),
('KH006', N'Cty Sản xuất Đông Á', N'Võ Văn F', N'987 Nguyễn Huệ, Quận 1, TP.HCM', 'vovanf@dongasx.com', '0909123461'),
('KH007', N'Cty Dịch vụ Tây Nam', N'Đặng Thị G', N'147 Lê Lợi, Quận 1, TP.HCM', 'dangthig@taynamdv.com', '0909123462'),
('KH008', N'Cty Thương mại Bắc', N'Bùi Văn H', N'258 Trần Hưng Đạo, Quận 5, TP.HCM', 'buivanh@thuongmaibac.com', '0909123463'),
('KH009', N'Cty Logistics Nam', N'Ngô Thị I', N'369 Võ Văn Tần, Quận 3, TP.HCM', 'ngothii@logisticsnam.com', '0909123464'),
('KH010', N'Cty Đầu tư Trung', N'Lý Văn K', N'741 Nguyễn Đình Chiểu, Quận 3, TP.HCM', 'lyvank@daututrung.com', '0909123465'),
('KH011', N'Cty Phát triển Đông', N'Trịnh Thị L', N'852 Đinh Tiên Hoàng, Quận 1, TP.HCM', 'trinhthil@phattriendong.com', '0909123466'),
('KH012', N'Cty Nghiên cứu Tây', N'Phan Văn M', N'963 Nguyễn Thị Minh Khai, Quận 3, TP.HCM', 'phanvanm@nghiencuutay.com', '0909123467'),
('KH013', N'Cty Sản xuất Nam', N'Vũ Thị N', N'159 Lý Tự Trọng, Quận 1, TP.HCM', 'vuthin@sanxuatnam.com', '0909123468');

-- Thêm hợp đồng với các trạng thái khác nhau để test
INSERT INTO Contract (ContractId, CustomerId, SignDate, PlanDueDate, Amount, Status, SalesCompany_UserId) VALUES
('HD001', 'KH001', DATEADD(day, -25, GETDATE()), DATEADD(day, -7, GETDATE()), 15000000, 'InProgress', 'TPKD'),  -- Quá hạn 7 ngày
('HD002', 'KH002', DATEADD(day, -20, GETDATE()), DATEADD(day, -3, GETDATE()), 25000000, 'InProgress', 'TPKD'),  -- Quá hạn 3 ngày
('HD003', 'KH003', DATEADD(day, -15, GETDATE()), DATEADD(day, 5, GETDATE()), 18000000, 'InProgress', 'TPKD'),   -- Chưa quá hạn
('HD004', 'KH004', DATEADD(day, -30, GETDATE()), DATEADD(day, -10, GETDATE()), 32000000, 'Done', 'TPKD'),      -- Hoàn thành
('HD005', 'KH005', DATEADD(day, -12, GETDATE()), DATEADD(day, 8, GETDATE()), 22000000, 'InProgress', 'TPKD'),  -- Đang thực hiện
('HD006', 'KH006', DATEADD(day, -18, GETDATE()), DATEADD(day, -2, GETDATE()), 28000000, 'InProgress', 'TPKD'),  -- Quá hạn 2 ngày
('HD007', 'KH007', DATEADD(day, -8, GETDATE()), DATEADD(day, 12, GETDATE()), 19500000, 'InProgress', 'TPKD'),  -- Chưa quá hạn
('HD008', 'KH008', DATEADD(day, -35, GETDATE()), DATEADD(day, -15, GETDATE()), 41000000, 'Done', 'TPKD'),     -- Hoàn thành
('HD009', 'KH009', DATEADD(day, -22, GETDATE()), DATEADD(day, -5, GETDATE()), 26000000, 'InProgress', 'TPKD'), -- Quá hạn 5 ngày
('HD010', 'KH010', DATEADD(day, -5, GETDATE()), DATEADD(day, 15, GETDATE()), 17500000, 'InProgress', 'TPKD'),   -- Chưa quá hạn
('HD011', 'KH011', DATEADD(day, -28, GETDATE()), DATEADD(day, -8, GETDATE()), 33500000, 'InProgress', 'TPKD'),  -- Quá hạn 8 ngày
('HD012', 'KH012', DATEADD(day, -10, GETDATE()), DATEADD(day, 10, GETDATE()), 21000000, 'InProgress', 'TPKD'),  -- Chưa quá hạn
('HD013', 'KH013', DATEADD(day, -40, GETDATE()), DATEADD(day, -20, GETDATE()), 45000000, 'Cancelled', 'TPKD'); -- Hủy bỏ


-- Thêm thông báo hợp đồng quá hạn mẫu
INSERT INTO Notification (NotificationId, Type, Message, CreatedAt, DaysLate, ContractId, TargetCompany_UserId, TargetDeptId) VALUES
('TB001', 'OVERDUE_CONTRACT', 'Hợp đồng HD001 đã quá hạn 7 ngày', GETDATE(), 7, 'HD001', 'TPKD', 'D002'),
('TB002', 'OVERDUE_CONTRACT', 'Hợp đồng HD002 đã quá hạn 3 ngày', GETDATE(), 3, 'HD002', 'TPKD', 'D002'),
('TB003', 'OVERDUE_CONTRACT', 'Hợp đồng HD006 đã quá hạn 2 ngày', GETDATE(), 2, 'HD006', 'TPKD', 'D002'),
('TB004', 'OVERDUE_CONTRACT', 'Hợp đồng HD009 đã quá hạn 5 ngày', GETDATE(), 5, 'HD009', 'TPKD', 'D002'),
('TB005', 'OVERDUE_CONTRACT', 'Hợp đồng HD011 đã quá hạn 8 ngày', GETDATE(), 8, 'HD011', 'TPKD', 'D002');


-- Đơn vị
INSERT INTO Unit(UnitId, Symbol) VALUES
('U001', N'°C'),
('U002', N'dBA'),
('U003', N'µg/m³'),
('U004', N'mg/Nm³'),
('U005', N'mg/m³');

-- Bổ sung đơn vị phục vụ dữ liệu nền mẫu mở rộng
IF NOT EXISTS (SELECT 1 FROM Unit WHERE UnitId='U006') INSERT INTO Unit(UnitId, Symbol) VALUES ('U006', N'mg/L');
IF NOT EXISTS (SELECT 1 FROM Unit WHERE UnitId='U007') INSERT INTO Unit(UnitId, Symbol) VALUES ('U007', N'pH');
IF NOT EXISTS (SELECT 1 FROM Unit WHERE UnitId='U008') INSERT INTO Unit(UnitId, Symbol) VALUES ('U008', N'MPN/100ml');

-- Danh mục thông số
INSERT INTO SampleParameter(ParameterId, ParameterName, UnitId, DepartmentId, MinValue, MaxValue, StandardCode) VALUES
('TS001', N'Nhiệt độ', 'U001', NULL, 10, 40, N'TCVN-Temp'),
('TS002', N'Tiếng ồn',  'U002', NULL, 50, 100, N'TCVN-Noise'),
('TS003', N'SO₂',       'U003', NULL, 0, 350, N'TCVN-Air'),
-- Thông số khí thải
('TS004', N'CO', 'U004', NULL, 0, 30, N'TCVN-KhiThai'),
('TS005', N'NOx', 'U004', NULL, 0, 200, N'TCVN-KhiThai'),
('TS006', N'SO2', 'U004', NULL, 0, 500, N'TCVN-KhiThai'),
('TS007', N'Bụi tổng', 'U005', NULL, 0, 200, N'TCVN-KhiThai'),
('TS008', N'Pb và hợp chất, tính theo Pb', 'U005', NULL, 0, 0.005, N'TCVN-KhiThai'),
('TS009', N'Cd và hợp chất, tính theo Cd', 'U005', NULL, 0, 0.002, N'TCVN-KhiThai'),
('TS010', N'As và hợp chất, tính theo As', 'U005', NULL, 0, 0.003, N'TCVN-KhiThai');

-- Bổ sung VOCs cho khí thải
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='TS011')
INSERT INTO SampleParameter(ParameterId, ParameterName, UnitId, DepartmentId, MinValue, MaxValue, StandardCode)
VALUES ('TS011', N'VOCs', 'U005', NULL, 0, 100, N'TCVN-KhiThai');

-- Bổ sung thông số Nước thải
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT005') INSERT INTO SampleParameter VALUES ('NT005', N'pH', 'U007', NULL, 6.00, 9.00, N'TCVN-Water');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT006') INSERT INTO SampleParameter VALUES ('NT006', N'BOD5', 'U006', NULL, 0, 50, N'TCVN-Water');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT007') INSERT INTO SampleParameter VALUES ('NT007', N'COD', 'U006', NULL, 0, 100, N'TCVN-Water');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT008') INSERT INTO SampleParameter VALUES ('NT008', N'TSS', 'U006', NULL, 0, 100, N'TCVN-Water');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT009') INSERT INTO SampleParameter VALUES ('NT009', N'NH3-N', 'U006', NULL, 0, 10, N'TCVN-Water');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT010') INSERT INTO SampleParameter VALUES ('NT010', N'Tổng N', 'U006', NULL, 0, 40, N'TCVN-Water');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT011') INSERT INTO SampleParameter VALUES ('NT011', N'Tổng P', 'U006', NULL, 0, 10, N'TCVN-Water');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT012') INSERT INTO SampleParameter VALUES ('NT012', N'Dầu mỡ', 'U006', NULL, 0, 20, N'TCVN-Water');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT013') INSERT INTO SampleParameter VALUES ('NT013', N'Kim loại nặng', 'U006', NULL, 0, 0.5, N'TCVN-Water');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='NT015') INSERT INTO SampleParameter VALUES ('NT015', N'Coliform', 'U008', NULL, 0, 3000, N'TCVN-Water');

-- Bổ sung thông số Không khí xung quanh (dùng SampleType 'Khác')
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='KK001') INSERT INTO SampleParameter VALUES ('KK001', N'Bụi PM10', 'U003', NULL, 0, 150, N'TCVN-AirAmb');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='KK002') INSERT INTO SampleParameter VALUES ('KK002', N'Bụi PM2.5', 'U003', NULL, 0, 75, N'TCVN-AirAmb');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='KK003') INSERT INTO SampleParameter VALUES ('KK003', N'SO2', 'U003', NULL, 0, 350, N'TCVN-AirAmb');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='KK004') INSERT INTO SampleParameter VALUES ('KK004', N'NO2', 'U003', NULL, 0, 200, N'TCVN-AirAmb');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='KK005') INSERT INTO SampleParameter VALUES ('KK005', N'CO', 'U003', NULL, 0, 30000, N'TCVN-AirAmb');

-- Bổ sung thông số Tiếng ồn chi tiết
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='TN001') INSERT INTO SampleParameter VALUES ('TN001', N'Tiếng ồn ban ngày', 'U002', NULL, 0, 70, N'TCVN-Noise');
IF NOT EXISTS (SELECT 1 FROM SampleParameter WHERE ParameterId='TN002') INSERT INTO SampleParameter VALUES ('TN002', N'Tiếng ồn ban đêm', 'U002', NULL, 0, 55, N'TCVN-Noise');

-- Nền mẫu thuộc các hợp đồng
INSERT INTO ContractItem(ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType) VALUES
('OK001', 'HD001', N'KK1', N'Không khí xung quanh', N'123 Nguyễn Văn Linh, Quận 7, TP.HCM', 'D003', N'Khí thải'),
('OK002', 'HD002', N'KK2', N'Không khí xung quanh', N'456 Lê Văn Việt, Quận 9, TP.HCM', 'D003', N'Khí thải'),
('OK003', 'HD003', N'KK3', N'Không khí xung quanh', N'789 Nguyễn Thị Minh Khai, Quận 1, TP.HCM', 'D003', N'Khí thải'),
('OK004', 'HD004', N'KK4', N'Không khí xung quanh', N'321 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM', 'D003', N'Khí thải'),
('NT005', 'HD005', N'KK5', N'Không khí xung quanh', N'654 Cách Mạng Tháng 8, Quận 10, TP.HCM', 'D003', N'Nước thải'),
('TN006', 'HD006', N'KK6', N'Không khí xung quanh', N'987 Nguyễn Huệ, Quận 1, TP.HCM', 'D003', N'Tiếng ồn'),
('OK007', 'HD007', N'KK7', N'Không khí xung quanh', N'147 Lê Lợi, Quận 1, TP.HCM', 'D003', N'Khí thải'),
('NT008', 'HD008', N'KK8', N'Không khí xung quanh', N'258 Trần Hưng Đạo, Quận 5, TP.HCM', 'D003', N'Nước thải'),
('TN009', 'HD009', N'KK9', N'Không khí xung quanh', N'369 Võ Văn Tần, Quận 3, TP.HCM', 'D003', N'Tiếng ồn'),
('OK010', 'HD010', N'KK10', N'Không khí xung quanh', N'741 Nguyễn Đình Chiểu, Quận 3, TP.HCM', 'D003', N'Khí thải'),
('KK011', 'HD011', N'KK11', N'Không khí xung quanh', N'852 Đinh Tiên Hoàng, Quận 1, TP.HCM', 'D003', N'Không khí'),
('NT012', 'HD012', N'KK12', N'Không khí xung quanh', N'963 Nguyễn Thị Minh Khai, Quận 3, TP.HCM', 'D003', N'Nước thải'),
('KK013', 'HD013', N'KK13', N'Không khí xung quanh', N'159 Lý Tự Trọng, Quận 1, TP.HCM', 'D003', N'Không khí');

-- Bổ sung nền mẫu rõ ràng cho HĐ HD001 (nhiều vị trí/ống khói)
INSERT INTO ContractItem(ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType) VALUES
('OK001A', 'HD001', N'VT1', N'Ống khói sau HTXL khí thải phòng phun hàn công nghệ Plasma', N'Khu xưởng A - Ống khói số 1', 'D003', N'Khí thải'),
('OK001B', 'HD001', N'VT2', N'Ống khói sau HTXL khí thải phòng phun hàn công nghệ HVOF',   N'Khu xưởng A - Ống khói số 2', 'D003', N'Khí thải'),
('OK001C', 'HD001', N'VT3', N'Ống khói sau HTXL khí thải khu mạ điện',                         N'Xưởng xử lý bề mặt - Ống khói mạ',   'D003', N'Khí thải');

-- Phòng Kế hoạch phân công HT (D004) / PT (D005) cho tất cả hợp đồng
INSERT INTO Assignment(ItemId, ParameterId, DepartmentId) VALUES
-- HD001 - OK001 (Khí thải)
('OK001', 'TS001', 'D004'), -- HT
('OK001', 'TS002', 'D004'), -- HT
('OK001', 'TS003', 'D005'), -- PT
('OK001', 'TS004', 'D005'), -- CO - PT
('OK001', 'TS005', 'D005'), -- NOx - PT
('OK001', 'TS006', 'D005'), -- SO2 - PT
('OK001', 'TS007', 'D004'), -- Bụi tổng - HT
-- HD001 - OK001A (Khí thải - Ống khói Plasma)
('OK001A', 'TS001', 'D004'),
('OK001A', 'TS002', 'D004'),
('OK001A', 'TS003', 'D005'),
('OK001A', 'TS004', 'D005'), -- CO
('OK001A', 'TS005', 'D005'), -- NOx
('OK001A', 'TS006', 'D005'), -- SO2
('OK001A', 'TS007', 'D004'), -- Bụi tổng
('OK001A', 'TS008', 'D005'), -- Pb
('OK001A', 'TS009', 'D005'), -- Cd
('OK001A', 'TS010', 'D005'), -- As
-- HD001 - OK001B (Khí thải - Ống khói HVOF)
('OK001B', 'TS001', 'D004'),
('OK001B', 'TS004', 'D005'), -- CO
('OK001B', 'TS005', 'D005'), -- NOx
('OK001B', 'TS006', 'D005'), -- SO2
('OK001B', 'TS007', 'D004'), -- Bụi tổng
('OK001B', 'TS008', 'D005'), -- Pb
-- HD001 - OK001C (Khí thải - Ống khói mạ điện)
('OK001C', 'TS001', 'D004'),
('OK001C', 'TS002', 'D004'),
('OK001C', 'TS003', 'D005'),
('OK001C', 'TS004', 'D005'), -- CO
('OK001C', 'TS005', 'D005'), -- NOx
('OK001C', 'TS006', 'D005'), -- SO2
('OK001C', 'TS007', 'D004'), -- Bụi tổng
('OK001C', 'TS010', 'D005'), -- As
-- HD002
('OK002', 'TS001', 'D004'), -- HT
('OK002', 'TS002', 'D004'), -- HT
('OK002', 'TS003', 'D005'), -- PT
-- HD003
('OK003', 'TS001', 'D004'), -- HT
('OK003', 'TS002', 'D004'), -- HT
('OK003', 'TS003', 'D005'), -- PT
-- HD004 (Khí thải) - thêm thông số khí thải
('OK004', 'TS001', 'D004'), -- HT
('OK004', 'TS002', 'D004'), -- HT
('OK004', 'TS003', 'D005'), -- PT
('OK004', 'TS004', 'D005'), -- CO - PT
('OK004', 'TS005', 'D005'), -- NOx - PT
('OK004', 'TS006', 'D005'), -- SO2 - PT
('OK004', 'TS007', 'D004'), -- Bụi tổng - HT
-- HD005 (Nước) - không có thông số khí thải
('NT005', 'TS001', 'D004'), -- HT
('NT005', 'TS002', 'D004'), -- HT
('NT005', 'TS003', 'D005'), -- PT
-- HD006
('TN006', 'TS001', 'D004'), -- HT
('TN006', 'TS002', 'D004'), -- HT
('TN006', 'TS003', 'D005'), -- PT
-- HD007 (Khí thải) - thêm thông số khí thải
('OK007', 'TS001', 'D004'), -- HT
('OK007', 'TS002', 'D004'), -- HT
('OK007', 'TS003', 'D005'), -- PT
('OK007', 'TS004', 'D005'), -- CO - PT
('OK007', 'TS005', 'D005'), -- NOx - PT
('OK007', 'TS007', 'D004'), -- Bụi tổng - HT
-- HD008 (Nước) - không có thông số khí thải
('NT008', 'TS001', 'D004'), -- HT
('NT008', 'TS002', 'D004'), -- HT
('NT008', 'TS003', 'D005'), -- PT
-- HD009 (Tiếng ồn) - không có thông số khí thải
('TN009', 'TS001', 'D004'), -- HT
('TN009', 'TS002', 'D004'), -- HT
('TN009', 'TS003', 'D005'), -- PT
-- HD010 (Khí thải) - thêm thông số khí thải
('OK010', 'TS001', 'D004'), -- HT
('OK010', 'TS002', 'D004'), -- HT
('OK010', 'TS003', 'D005'), -- PT
('OK010', 'TS004', 'D005'), -- CO - PT
('OK010', 'TS005', 'D005'), -- NOx - PT
('OK010', 'TS006', 'D005'), -- SO2 - PT
('OK010', 'TS007', 'D004'), -- Bụi tổng - HT
-- HD011
('KK011', 'TS001', 'D004'), -- HT
('KK011', 'TS002', 'D004'), -- HT
('KK011', 'TS003', 'D005'), -- PT
-- HD012
('NT012', 'TS001', 'D004'), -- HT
('NT012', 'TS002', 'D004'), -- HT
('NT012', 'TS003', 'D005'), -- PT
-- HD013
('KK013', 'TS001', 'D004'), -- HT
('KK013', 'TS002', 'D004'), -- HT
('KK013', 'TS003', 'D005'); -- PT

INSERT INTO ItemParameterSetting(ItemId, ParameterId, MinValue, MaxValue, Note, UpdatedBy)
VALUES
-- HD001
('OK001', 'TS001', 8, 38, N'Giới hạn nhiệt độ', 'Admin'),
('OK001', 'TS002', 40, 90, N'Giới hạn tiếng ồn', 'Admin'),
('OK001', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
('OK001A', 'TS001', 8, 38, N'Giới hạn nhiệt độ', 'Admin'),
('OK001A', 'TS002', 40, 90, N'Giới hạn tiếng ồn', 'Admin'),
('OK001A', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
('OK001B', 'TS001', 8, 38, N'Giới hạn nhiệt độ', 'Admin'),
('OK001B', 'TS002', 40, 90, N'Giới hạn tiếng ồn', 'Admin'),
('OK001B', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
('OK001C', 'TS001', 8, 38, N'Giới hạn nhiệt độ', 'Admin'),
('OK001C', 'TS002', 40, 90, N'Giới hạn tiếng ồn', 'Admin'),
('OK001C', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
-- HD002
('OK002', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'),
('OK002', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'),
('OK002', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
-- HD003
('OK003', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'),
('OK003', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'),
('OK003', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
-- HD004
('OK004', 'TS001', 12, 35, N'Giới hạn nhiệt độ', 'Admin'),
('OK004', 'TS002', 45, 95, N'Giới hạn tiếng ồn', 'Admin'),
('OK004', 'TS003', 0, 300, N'Giới hạn SO2', 'Admin'),
-- HD005
('NT005', 'TS001', 15, 42, N'Giới hạn nhiệt độ', 'Admin'),
('NT005', 'TS002', 55, 105, N'Giới hạn tiếng ồn', 'Admin'),
('NT005', 'TS003', 0, 400, N'Giới hạn SO2', 'Admin'),
-- HD006
('TN006', 'TS001', 8, 35, N'Giới hạn nhiệt độ', 'Admin'),
('TN006', 'TS002', 40, 90, N'Giới hạn tiếng ồn', 'Admin'),
('TN006', 'TS003', 0, 320, N'Giới hạn SO2', 'Admin'),
-- HD007
('OK007', 'TS001', 10, 38, N'Giới hạn nhiệt độ', 'Admin'),
('OK007', 'TS002', 50, 95, N'Giới hạn tiếng ồn', 'Admin'),
('OK007', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
-- HD008
('NT008', 'TS001', 12, 40, N'Giới hạn nhiệt độ', 'Admin'),
('NT008', 'TS002', 45, 100, N'Giới hạn tiếng ồn', 'Admin'),
('NT008', 'TS003', 0, 380, N'Giới hạn SO2', 'Admin'),
-- HD009
('TN009', 'TS001', 9, 36, N'Giới hạn nhiệt độ', 'Admin'),
('TN009', 'TS002', 48, 92, N'Giới hạn tiếng ồn', 'Admin'),
('TN009', 'TS003', 0, 330, N'Giới hạn SO2', 'Admin'),
-- HD010
('OK010', 'TS001', 11, 39, N'Giới hạn nhiệt độ', 'Admin'),
('OK010', 'TS002', 52, 98, N'Giới hạn tiếng ồn', 'Admin'),
('OK010', 'TS003', 0, 360, N'Giới hạn SO2', 'Admin'),
-- HD011
('KK011', 'TS001', 7, 37, N'Giới hạn nhiệt độ', 'Admin'),
('KK011', 'TS002', 42, 88, N'Giới hạn tiếng ồn', 'Admin'),
('KK011', 'TS003', 0, 340, N'Giới hạn SO2', 'Admin'),
-- HD012
('NT012', 'TS001', 13, 41, N'Giới hạn nhiệt độ', 'Admin'),
('NT012', 'TS002', 53, 102, N'Giới hạn tiếng ồn', 'Admin'),
('NT012', 'TS003', 0, 370, N'Giới hạn SO2', 'Admin'),
-- HD013
('KK013', 'TS001', 6, 34, N'Giới hạn nhiệt độ', 'Admin'),
('KK013', 'TS002', 38, 85, N'Giới hạn tiếng ồn', 'Admin'),
('KK013', 'TS003', 0, 310, N'Giới hạn SO2', 'Admin');

-- Lô đo và kết quả mẫu cho tất cả hợp đồng
INSERT INTO MeasurementBatch(BatchId, ItemId, StartDate, EndDate, Status, LeaderCompany_UserId)
VALUES 
('B001', 'OK001', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B001A','OK001A', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B001B','OK001B', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B001C','OK001C', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B002', 'OK002', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B003', 'OK003', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B004', 'OK004', DATEADD(day,-2,GETDATE()), GETDATE(), 'Completed', 'TPKD'),
('B005', 'NT005', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B006', 'TN006', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B007', 'OK007', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B008', 'NT008', DATEADD(day,-3,GETDATE()), GETDATE(), 'Completed', 'TPKD'),
('B009', 'TN009', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B010', 'OK010', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B011', 'KK011', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B012', 'NT012', DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'),
('B013', 'KK013', DATEADD(day,-1,GETDATE()), NULL, 'Planned', 'TPKD');

INSERT INTO Result(ResultId, BatchId, ParameterId, Value, Company_UserId, OutofStandard, Note) VALUES
-- HD001 - có kết quả Không đạt
('KQ001', 'B001', 'TS001', 9.0, 'TPKD', 0, N'Đạt'),
('KQ002', 'B001', 'TS002', 120.0, 'TPKD', 1, N'Không đạt'),
('KQ003', 'B001', 'TS003', 200.0, 'TPKD', 0, N'Đạt'),
-- HD002 - kết quả bình thường
('KQ004', 'B002', 'TS001', 25.0, 'TPKD', 0, N'Đạt'),
('KQ005', 'B002', 'TS002', 65.0, 'TPKD', 0, N'Đạt'),
('KQ006', 'B002', 'TS003', 150.0, 'TPKD', 0, N'Đạt'),
-- HD003 - chưa có kết quả
('KQ007', 'B003', 'TS001', NULL, 'TPKD', 0, NULL),
('KQ008', 'B003', 'TS002', NULL, 'TPKD', 0, NULL),
('KQ009', 'B003', 'TS003', NULL, 'TPKD', 0, NULL),
-- HD004 - hoàn thành
('KQ010', 'B004', 'TS001', 28.5, 'TPKD', 0, N'Đạt'),
('KQ011', 'B004', 'TS002', 75.2, 'TPKD', 0, N'Đạt'),
('KQ012', 'B004', 'TS003', 180.8, 'TPKD', 0, N'Đạt'),
-- HD005 - đang thực hiện
('KQ013', 'B005', 'TS001', 32.1, 'TPKD', 0, N'Đạt'),
('KQ014', 'B005', 'TS002', 85.7, 'TPKD', 0, N'Đạt'),
('KQ015', 'B005', 'TS003', 220.3, 'TPKD', 0, N'Đạt'),
-- HD006 - quá hạn
('KQ016', 'B006', 'TS001', 7.2, 'TPKD', 1, N'Không đạt'),
('KQ017', 'B006', 'TS002', 95.4, 'TPKD', 1, N'Không đạt'),
('KQ018', 'B006', 'TS003', 350.6, 'TPKD', 1, N'Không đạt'),
-- HD007 - chưa có kết quả
('KQ019', 'B007', 'TS001', NULL, 'TPKD', 0, NULL),
('KQ020', 'B007', 'TS002', NULL, 'TPKD', 0, NULL),
('KQ021', 'B007', 'TS003', NULL, 'TPKD', 0, NULL),
-- HD008 - hoàn thành
('KQ022', 'B008', 'TS001', 22.8, 'TPKD', 0, N'Đạt'),
('KQ023', 'B008', 'TS002', 68.9, 'TPKD', 0, N'Đạt'),
('KQ024', 'B008', 'TS003', 195.2, 'TPKD', 0, N'Đạt'),
-- HD009 - quá hạn
('KQ025', 'B009', 'TS001', 5.8, 'TPKD', 1, N'Không đạt'),
('KQ026', 'B009', 'TS002', 98.7, 'TPKD', 1, N'Không đạt'),
('KQ027', 'B009', 'TS003', 340.1, 'TPKD', 1, N'Không đạt'),
-- HD010 - chưa có kết quả
('KQ028', 'B010', 'TS001', NULL, 'TPKD', 0, NULL),
('KQ029', 'B010', 'TS002', NULL, 'TPKD', 0, NULL),
('KQ030', 'B010', 'TS003', NULL, 'TPKD', 0, NULL),
-- HD011 - quá hạn
('KQ031', 'B011', 'TS001', 6.5, 'TPKD', 1, N'Không đạt'),
('KQ032', 'B011', 'TS002', 92.3, 'TPKD', 1, N'Không đạt'),
('KQ033', 'B011', 'TS003', 360.8, 'TPKD', 1, N'Không đạt'),
-- HD012 - chưa có kết quả
('KQ034', 'B012', 'TS001', NULL, 'TPKD', 0, NULL),
('KQ035', 'B012', 'TS002', NULL, 'TPKD', 0, NULL),
('KQ036', 'B012', 'TS003', NULL, 'TPKD', 0, NULL),
-- HD013 - chưa có kết quả (hủy bỏ)
('KQ037', 'B013', 'TS001', NULL, 'TPKD', 0, NULL),
('KQ038', 'B013', 'TS002', NULL, 'TPKD', 0, NULL),
('KQ039', 'B013', 'TS003', NULL, 'TPKD', 0, NULL);


-- Thêm 3 nhân viên mẫu
INSERT INTO Company_User (Company_UserId, Company_Username, PasswordHash, FullName, Email, Phone, DateOfBirth, Gender, Address, RoleId, DepartmentId, IsActive, EmailVerifiedAt) VALUES
-- Nhân viên Phòng Hiện Trường
('NV001', 'NV001', @HashedDefaultPassword, N'Nguyễn Văn An', 'an.nv@ecogreen.com', '0912345678', '1995-10-20', 'Nam', N'100 Võ Văn Tần, Quận 3, TP.HCM', 'R003', 'D004', 1, GETDATE()),

-- Nhân viên Phòng Thí Nghiệm
('NV002', 'NV002', @HashedDefaultPassword, N'Lê Thị Bình', 'binh.lt@ecogreen.com', '0987654321', '1998-05-12', N'Nữ', N'250 Nguyễn Thị Minh Khai, Quận 1, TP.HCM', 'R003', 'D005', 1, GETDATE()),

-- Trưởng Phòng Kế Hoạch
('TPKH', 'TP_KH', @HashedDefaultPassword, N'Phạm Văn Cường', 'cuong.pv@ecogreen.com', '0905112233', '1988-08-30', 'Nam', N'50 Pasteur, Quận 1, TP.HCM', 'R002', 'D003', 1, GETDATE()),

-- Bổ sung 3 nhân viên mẫu
('NV003', 'NV003', @HashedDefaultPassword, N'Trần Quốc Dũng', 'dung.tq@ecogreen.com', '0903003003', '1994-02-18', 'Nam', N'12A Nguyễn Bỉnh Khiêm, Quận 1, TP.HCM', 'R003', 'D003', 1, GETDATE()),
('NV004', 'NV004', @HashedDefaultPassword, N'Phạm Thị Hạnh', 'hanh.pt@ecogreen.com', '0911001100', '1996-07-09', N'Nữ', N'88 Nguyễn Trãi, Quận 5, TP.HCM', 'R003', 'D004', 1, GETDATE()),
('NV005', 'NV005', @HashedDefaultPassword, N'Đỗ Minh Khôi', 'khoi.dm@ecogreen.com', '0922002200', '1993-11-25', 'Nam', N'45 Xô Viết Nghệ Tĩnh, Bình Thạnh, TP.HCM', 'R003', 'D005', 1, GETDATE());
GO

-- Loại bỏ các nền mẫu chung chung 'Không khí xung quanh' để chỉ giữ các vị trí cụ thể
DELETE FROM ContractItem WHERE SampleName = N'Không khí xung quanh';

-- Bổ sung ContractItem cho các nền mẫu trong file CSV (nếu chưa tồn tại)
-- Nhóm khí thải bổ sung cho HD002..HD007
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='OK002B') INSERT INTO ContractItem(ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType) VALUES ('OK002B','HD002',N'OK002B',N'Ống khói lò đốt chất thải rắn công nghiệp',N'Vị trí lò đốt','D003',N'Khí thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='OK003C') INSERT INTO ContractItem VALUES ('OK003C','HD003',N'OK003C',N'Hệ thống xử lý khí thải phân xưởng sơn',N'Phân xưởng sơn','D003',N'Khí thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='OK004D') INSERT INTO ContractItem VALUES ('OK004D','HD004',N'OK004D',N'Khí thải lò hơi công suất lớn',N'Khu lò hơi','D003',N'Khí thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='OK005E') INSERT INTO ContractItem VALUES ('OK005E','HD005',N'OK005E',N'Khí thải máy phát điện dự phòng',N'Nhà máy','D003',N'Khí thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='OK006F') INSERT INTO ContractItem VALUES ('OK006F','HD006',N'OK006F',N'Khí thải nhà máy xi mạng',N'Nhà máy','D003',N'Khí thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='OK007G') INSERT INTO ContractItem VALUES ('OK007G','HD007',N'OK007G',N'Khí thải xưởng gia công cơ khí',N'Xưởng cơ khí','D003',N'Khí thải');

-- Nhóm nước thải: phân cho HD005/HD008/HD012
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='NT001A') INSERT INTO ContractItem VALUES ('NT001A','HD005',N'NT001A',N'Nước thải sinh hoạt sau xử lý',N'Khu xử lý','D003',N'Nước thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='NT002B') INSERT INTO ContractItem VALUES ('NT002B','HD008',N'NT002B',N'Nước thải công nghiệp sau HTXL',N'Khu công nghiệp','D003',N'Nước thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='NT003C') INSERT INTO ContractItem VALUES ('NT003C','HD012',N'NT003C',N'Nước thải bếp ăn tập thể',N'Bếp ăn','D003',N'Nước thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='NT004D') INSERT INTO ContractItem VALUES ('NT004D','HD005',N'NT004D',N'Nước thải xưởng nhuộm vải',N'Xưởng nhuộm','D003',N'Nước thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='NT005E') INSERT INTO ContractItem VALUES ('NT005E','HD005',N'NT005E',N'Nước thải bệnh viện sau xử lý',N'Bệnh viện','D003',N'Nước thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='NT006F') INSERT INTO ContractItem VALUES ('NT006F','HD008',N'NT006F',N'Nước thải khách sạn',N'Khách sạn','D003',N'Nước thải');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='NT007G') INSERT INTO ContractItem VALUES ('NT007G','HD012',N'NT007G',N'Nước thải nhà máy chế biến thủy sản',N'Nhà máy','D003',N'Nước thải');

-- Không khí xung quanh: gán vào HD003 (tùy chọn), SampleType đặt 'Khác' để không vi phạm CHECK
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='KK001') INSERT INTO ContractItem VALUES ('KK001','HD003',N'KK001',N'Không khí xung quanh nhà máy - Điểm 1',N'Điểm 1','D003',N'Không khí');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='KK002') INSERT INTO ContractItem VALUES ('KK002','HD003',N'KK002',N'Không khí xung quanh nhà máy - Điểm 2',N'Điểm 2','D003',N'Không khí');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='KK003') INSERT INTO ContractItem VALUES ('KK003','HD003',N'KK003',N'Không khí khu dân cư lân cận',N'Khu dân cư','D003',N'Không khí');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='KK004') INSERT INTO ContractItem VALUES ('KK004','HD003',N'KK004',N'Không khí trường học gần nhà máy',N'Trường học','D003',N'Không khí');
IF NOT EXISTS (SELECT 1 FROM ContractItem WHERE ItemId='KK005') INSERT INTO ContractItem VALUES ('KK005','HD003',N'KK005',N'Không khí khu công nghiệp',N'Khu công nghiệp','D003',N'Không khí');

-- Phân công & giới hạn (Assignment + ItemParameterSetting) và tạo Batch + Result cho dữ liệu CSV
-- Helper: map phân công
-- Khí thải: OK001A đã có. Bổ sung OK002B..OK007G
INSERT INTO Assignment(ItemId, ParameterId, DepartmentId)
SELECT v.ItemId, v.ParameterId, CASE WHEN v.Assign='PT' THEN 'D005' ELSE 'D004' END
FROM (VALUES
('OK002B','TS004','PT'),('OK002B','TS005','PT'),('OK002B','TS006','PT'),('OK002B','TS007','D004'),('OK002B','TS008','PT'),
('OK003C','TS004','PT'),('OK003C','TS005','PT'),('OK003C','TS006','PT'),('OK003C','TS007','D004'),('OK003C','TS011','PT'),
('OK004D','TS004','PT'),('OK004D','TS005','PT'),('OK004D','TS006','PT'),('OK004D','TS007','D004'),
('OK005E','TS004','PT'),('OK005E','TS005','PT'),('OK005E','TS006','PT'),
('OK006F','TS004','PT'),('OK006F','TS007','D004'),('OK006F','TS006','PT'),
('OK007G','TS004','PT'),('OK007G','TS005','PT'),('OK007G','TS007','D004')
) AS v(ItemId,ParameterId,Assign)
WHERE NOT EXISTS (SELECT 1 FROM Assignment a WHERE a.ItemId=v.ItemId AND a.ParameterId=v.ParameterId);

-- Nước thải
INSERT INTO Assignment(ItemId, ParameterId, DepartmentId)
SELECT v.ItemId, v.ParameterId, CASE WHEN v.Assign='PT' THEN 'D005' ELSE 'D004' END
FROM (VALUES
('NT001A','NT005','D004'),('NT001A','NT006','PT'),('NT001A','NT007','PT'),('NT001A','NT008','PT'),('NT001A','NT009','PT'),('NT001A','NT010','PT'),('NT001A','NT011','PT'),
('NT002B','NT005','D004'),('NT002B','NT006','PT'),('NT002B','NT007','PT'),('NT002B','NT008','PT'),('NT002B','NT012','PT'),('NT002B','NT013','PT'),
('NT003C','NT005','D004'),('NT003C','NT006','PT'),('NT003C','NT007','PT'),('NT003C','NT012','PT'),
('NT004D','NT005','D004'),('NT004D','NT006','PT'),('NT004D','NT007','PT'),('NT004D','NT014','PT'),
('NT005E','NT005','D004'),('NT005E','NT006','PT'),('NT005E','NT007','PT'),('NT005E','NT015','PT'),
('NT006F','NT005','D004'),('NT006F','NT006','PT'),('NT006F','NT007','PT'),('NT006F','NT012','PT'),
('NT007G','NT005','D004'),('NT007G','NT006','PT'),('NT007G','NT007','PT'),('NT007G','NT008','PT')
) AS v(ItemId,ParameterId,Assign)
WHERE NOT EXISTS (SELECT 1 FROM Assignment a WHERE a.ItemId=v.ItemId AND a.ParameterId=v.ParameterId);

-- Không khí xung quanh
INSERT INTO Assignment(ItemId, ParameterId, DepartmentId)
SELECT v.ItemId, v.ParameterId, 'D004'
FROM (VALUES
('KK001','KK001'),('KK001','KK002'),('KK001','KK003'),('KK001','KK004'),('KK001','KK005'),
('KK002','KK001'),('KK002','KK002'),('KK002','KK003'),('KK002','KK004'),('KK002','KK005'),
('KK003','KK001'),('KK003','KK002'),('KK003','KK003'),('KK003','KK004'),('KK003','KK005'),
('KK004','KK001'),('KK004','KK002'),('KK004','KK003'),('KK004','KK004'),('KK004','KK005'),
('KK005','KK001'),('KK005','KK002'),('KK005','KK003'),('KK005','KK004'),('KK005','KK005')
) AS v(ItemId,ParameterId)
WHERE NOT EXISTS (SELECT 1 FROM Assignment a WHERE a.ItemId=v.ItemId AND a.ParameterId=v.ParameterId);

-- Giới hạn cho các mục theo CSV (ItemParameterSetting)
-- Ví dụ một số mục tiêu biểu; các giới hạn khác đã nằm trong SampleParameter mặc định
MERGE ItemParameterSetting AS tgt USING (SELECT 'OK002B' AS ItemId, 'TS008' AS ParameterId, NULL AS MinV, 0.01 AS MaxV) s
ON (tgt.ItemId=s.ItemId AND tgt.ParameterId=s.ParameterId)
WHEN MATCHED THEN UPDATE SET MaxValue=s.MaxV, UpdatedBy='Admin', UpdatedAt=SYSDATETIME()
WHEN NOT MATCHED THEN INSERT(ItemId,ParameterId,MaxValue,UpdatedBy) VALUES (s.ItemId,s.ParameterId,s.MaxV,'Admin');

-- Tạo lô đo cho các nền mẫu mới
INSERT INTO MeasurementBatch(BatchId, ItemId, StartDate, EndDate, Status, LeaderCompany_UserId)
SELECT v.BatchId, v.ItemId, DATEADD(day,-1,GETDATE()), NULL, 'InProgress', 'TPKD'
FROM (VALUES('BIT002B','OK002B'),('BIT003C','OK003C'),('BIT004D','OK004D'),('BIT005E','OK005E'),('BIT006F','OK006F'),('BIT007G','OK007G'),
('BNT001A','NT001A'),('BNT002B','NT002B'),('BNT003C','NT003C'),('BNT004D','NT004D'),('BNT005E','NT005E'),('BNT006F','NT006F'),('BNT007G','NT007G'),
('BKK001','KK001'),('BKK002','KK002'),('BKK003','KK003'),('BKK004','KK004'),('BKK005','KK005')) AS v(BatchId,ItemId)
WHERE NOT EXISTS (SELECT 1 FROM MeasurementBatch b WHERE b.BatchId=v.BatchId);

-- Kết quả theo CSV (rút gọn, minh họa các dòng tiêu biểu)
INSERT INTO Result(ResultId, BatchId, ParameterId, Value, Company_UserId, OutofStandard, Note)
VALUES
('KQ100','BIT002B','TS004',28.7,'TPKD',0,N'Đạt'),
('KQ101','BIT002B','TS005',185.4,'TPKD',0,N'Đạt'),
('KQ102','BIT002B','TS006',450.8,'TPKD',0,N'Đạt'),
('KQ103','BIT002B','TS007',175.6,'TPKD',0,N'Đạt'),
('KQ104','BIT002B','TS008',0.012,'TPKD',1,N'Không đạt'),
('KQ105','BIT003C','TS011',55.2,'TPKD',0,N'Đạt'),
('KQ106','BNT001A','NT005',7.2,'TPKD',0,N'Đạt'),
('KQ107','BNT001A','NT006',28.5,'TPKD',0,N'Đạt'),
('KQ108','BNT001A','NT007',65.3,'TPKD',0,N'Đạt'),
('KQ109','BNT001A','NT008',42.8,'TPKD',0,N'Đạt'),
('KQ110','BNT001A','NT009',8.5,'TPKD',0,N'Đạt'),
('KQ111','BNT001A','NT010',18.2,'TPKD',0,N'Đạt'),
('KQ112','BNT001A','NT011',5.6,'TPKD',0,N'Đạt'),
('KQ113','BNT002B','NT007',108.7,'TPKD',1,N'Không đạt'),
('KQ114','BKK001','KK001',85.5,'TPKD',0,N'Đạt'),
('KQ115','BKK001','KK002',52.3,'TPKD',0,N'Đạt'),
('KQ116','BKK001','KK003',125.8,'TPKD',0,N'Đạt'),
('KQ117','BKK001','KK004',95.2,'TPKD',0,N'Đạt'),
('KQ118','BKK001','KK005',8500,'TPKD',0,N'Đạt'),
('KQ119','BKK002','KK001',165.7,'TPKD',1,N'Không đạt'),
('KQ120','BNN002','TN001',72.8,'TPKD',1,N'Không đạt');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH014') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH014', N'Cty Môi trường xanh', N'123 Nguyễn Văn Linh, Quận 7, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH015') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH015', N'Cty ABC', N'456 Lê Văn Việt, Quận 9, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH016') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH016', N'Cty XYZ', N'789 Nguyễn Thị Minh Khai, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH017') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH017', N'Cty Công nghệ Việt', N'321 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH018') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH018', N'Cty Xây dựng Hồng', N'654 Cách Mạng Tháng 8, Quận 10, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH019') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH019', N'Cty Sản xuất Đông Á', N'987 Nguyễn Huệ, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH020') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH020', N'Cty Dịch vụ Tây Nam', N'147 Lê Lợi, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH021') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH021', N'Cty Thương mại Bắc', N'258 Trần Hưng Đạo, Quận 5, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH022') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH022', N'Cty Logistics Nam', N'369 Võ Văn Tần, Quận 3, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH023') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH023', N'Cty Đầu tư Trung', N'741 Nguyễn Đình Chiểu, Quận 3, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH024') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH024', N'Cty Phát triển Đông', N'852 Đinh Tiên Hoàng, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH025') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH025', N'Cty Nghiên cứu Tây', N'963 Nguyễn Thị Minh Khai, Quận 3, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH026') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH026', N'Cty Sản xuất Nam', N'159 Lý Tự Trọng, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH027') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH027', N'Công ty CP Thép Hòa Phát', N'KCN Phố Nối A, Hưng Yên');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH028') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH028', N'Công ty TNHH Samsung Electronics Việt Nam', N'KCN Yên Phong, Bắc Ninh');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH029') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH029', N'Công ty TNHH Intel Products Việt Nam', N'Lô I2, Khu Công nghệ cao, Quận 9, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH030') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH030', N'Công ty TNHH Piaggio Việt Nam', N'Lô M, KCN Bình Xuyên, Vĩnh Phúc');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH031') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH031', N'Công ty CP Ô tô Trường Hải (Thaco)', N'KCN Tam Hiệp, Núi Thành, Quảng Nam');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH032') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH032', N'Công ty TNHH Nhà máy Bia Heineken Việt Nam', N'Tầng 18, Tòa nhà Vietcombank, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH033') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH033', N'Công ty CP Sữa Việt Nam (Vinamilk)', N'Số 10 Tân Trào, Phường Tân Phú, Quận 7, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH034') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH034', N'Công ty CP Tôn Đông Á', N'Số 5, đường số 5, KCN Sóng Thần 1, Dĩ An, Bình Dương');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH035') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH035', N'Công ty TNHH Giấy Sài Gòn', N'172-174 Nguyễn Cư Trinh, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH036') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH036', N'Công ty CP Nhựa Bình Minh', N'240 Hậu Giang, Phường 9, Quận 6, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH037') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH037', N'Công ty CP Dệt may - Đầu tư - Thương mại Thành Công', N'36 Tây Thạnh, P. Tây Thạnh, Q. Tân Phú, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH038') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH038', N'Công ty TNHH PouYuen Việt Nam', N'Phường Tân Tạo, Quận Bình Tân, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH039') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH039', N'Tổng Công ty CP May Việt Tiến', N'07 Lê Minh Xuân, Phường 7, Q. Tân Bình, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH040') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH040', N'Công ty CP May Sông Hồng', N'105 Nguyễn Đức Thuận, TP. Nam Định, Nam Định');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH041') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH041', N'Công ty TNHH Chang Shin Việt Nam', N'KCN Thạnh Phú, Vĩnh Cửu, Đồng Nai');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH042') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH042', N'Công ty CP Thực phẩm Kỹ nghệ Masan (Masan Food)', N'Lầu 12, Tòa nhà MPlaza, 39 Lê Duẩn, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH043') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH043', N'Công ty TNHH Nước giải khát Suntory PepsiCo Việt Nam', N'Tầng 5, Khách sạn Sheraton, 88 Đồng Khởi, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH044') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH044', N'Công ty TNHH Nestlé Việt Nam', N'Lầu 5, Tòa nhà Empress, 138-142 Hai Bà Trưng, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH045') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH045', N'Công ty CP Acecook Việt Nam', N'Lô II-3, Đường số 11, KCN Tân Bình, Q. Tân Phú, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH046') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH046', N'Công ty TNHH Thực Phẩm Orion Vina', N'Lô E-13, KCN Mỹ Phước 2, Bến Cát, Bình Dương');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH047') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH047', N'Tập đoàn Công nghiệp – Viễn thông Quân đội (Viettel)', N'Lô D26, KĐT Mới Cầu Giấy, Yên Hòa, Cầu Giấy, Hà Nội');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH048') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH048', N'Công ty CP FPT', N'Tòa nhà FPT, số 10 Phạm Văn Bạch, Dịch Vọng, Cầu Giấy, Hà Nội');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH049') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH049', N'Công ty CP VNG', N'Lô Z06, Đường 13, KCN Tân Thuận, Quận 7, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH050') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH050', N'Công ty CP MISA', N'Tòa nhà MISA, Lô 5, CVPM Quang Trung, Quận 12, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH051') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH051', N'Công ty TNHH Shopee Việt Nam', N'Tầng 28, Tòa nhà Saigon Centre 2, 67 Lê Lợi, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH052') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH052', N'Công ty CP Dược Hậu Giang', N'288 Nguyễn Văn Cừ, An Hòa, Ninh Kiều, Cần Thơ');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH053') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH053', N'Công ty CP Hóa chất Cơ bản Miền Nam', N'22 Lý Thái Tổ, Phường 2, Quận 3, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH054') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH054', N'Công ty TNHH Sơn AkzoNobel Việt Nam', N'Lô G, KCN Amata, Biên Hòa, Đồng Nai');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH055') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH055', N'Công ty CP Traphaco', N'75 Yên Ninh, Ba Đình, Hà Nội');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH056') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH056', N'Công ty CP Xây dựng Coteccons', N'236/6 Điện Biên Phủ, Phường 17, Q. Bình Thạnh, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH057') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH057', N'Công ty CP Tập đoàn Xây dựng Hòa Bình', N'235 Võ Thị Sáu, Phường 7, Quận 3, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH058') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH058', N'Tổng Công ty Xi măng Việt Nam (Vicem)', N'228 Lê Duẩn, Đống Đa, Hà Nội');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH059') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH059', N'Công ty CP Xi măng Hà Tiên 1', N'360 Bến Chương Dương, Cầu Kho, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH060') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH060', N'Tập đoàn Điện lực Việt Nam (EVN)', N'11 Cửa Bắc, Trúc Bạch, Ba Đình, Hà Nội');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH061') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH061', N'Công ty CP Nước - Môi trường Bình Dương (Biwase)', N'11 Ngô Văn Trị, Phú Lợi, Thủ Dầu Một, Bình Dương');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH062') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH062', N'Công ty TNHH MTV Môi trường Đô thị TP.HCM (CITENCO)', N'42-44 Võ Thị Sáu, Tân Định, Quận 1, TP.HCM');
IF NOT EXISTS (SELECT 1 FROM Customer WHERE CustomerId='KH063') INSERT INTO Customer(CustomerId, CustomerName, Address) VALUES ('KH063', N'Công ty CP Đầu tư & Phát triển Năng lượng Mặt Trời (Solarergy)', N'21A Nguyễn Văn Trỗi, Phường 12, Q. Phú Nhuận, TP.HCM');

-- =====================================================================
-- THEM 30 HOP DONG MOI DE TEST TINH NANG AI DU DOAN
-- =====================================================================
USE QLQTMT;
GO

-- Thêm 30 hợp đồng mới với các trạng thái khác nhau (Done, InProgress, Cancelled)
-- Một số hợp đồng Done để test AI prediction
INSERT INTO Contract (ContractId, CustomerId, SignDate, PlanDueDate, Amount, Status, SalesCompany_UserId) VALUES
-- Hợp đồng đã hoàn thành (Done) - để test AI prediction
('HD014', 'KH014', DATEADD(day, -120, GETDATE()), DATEADD(day, -90, GETDATE()), 18000000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
('HD015', 'KH015', DATEADD(day, -110, GETDATE()), DATEADD(day, -85, GETDATE()), 22000000, 'Done', 'TPKD'),  -- Hoàn thành sớm 5 ngày
('HD016', 'KH016', DATEADD(day, -100, GETDATE()), DATEADD(day, -75, GETDATE()), 19500000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
('HD017', 'KH017', DATEADD(day, -95, GETDATE()), DATEADD(day, -70, GETDATE()), 28000000, 'Done', 'TPKD'),   -- Hoàn thành đúng hạn
('HD018', 'KH018', DATEADD(day, -90, GETDATE()), DATEADD(day, -65, GETDATE()), 25000000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
('HD019', 'KH019', DATEADD(day, -85, GETDATE()), DATEADD(day, -60, GETDATE()), 32000000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
('HD020', 'KH020', DATEADD(day, -80, GETDATE()), DATEADD(day, -55, GETDATE()), 21000000, 'Done', 'TPKD'),   -- Hoàn thành đúng hạn
('HD021', 'KH021', DATEADD(day, -75, GETDATE()), DATEADD(day, -50, GETDATE()), 29000000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
('HD022', 'KH022', DATEADD(day, -70, GETDATE()), DATEADD(day, -45, GETDATE()), 24000000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
('HD023', 'KH023', DATEADD(day, -65, GETDATE()), DATEADD(day, -40, GETDATE()), 35000000, 'Done', 'TPKD'), -- Hoàn thành đúng hạn
('HD024', 'KH024', DATEADD(day, -60, GETDATE()), DATEADD(day, -35, GETDATE()), 27000000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
('HD025', 'KH025', DATEADD(day, -55, GETDATE()), DATEADD(day, -30, GETDATE()), 31000000, 'Done', 'TPKD'),   -- Hoàn thành đúng hạn
('HD026', 'KH026', DATEADD(day, -50, GETDATE()), DATEADD(day, -25, GETDATE()), 23000000, 'Done', 'TPKD'),   -- Hoàn thành đúng hạn
('HD027', 'KH027', DATEADD(day, -45, GETDATE()), DATEADD(day, -20, GETDATE()), 26000000, 'Done', 'TPKD'),   -- Hoàn thành đúng hạn
('HD028', 'KH028', DATEADD(day, -40, GETDATE()), DATEADD(day, -15, GETDATE()), 33000000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
('HD029', 'KH029', DATEADD(day, -35, GETDATE()), DATEADD(day, -10, GETDATE()), 20000000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
('HD030', 'KH030', DATEADD(day, -30, GETDATE()), DATEADD(day, -5, GETDATE()), 28500000, 'Done', 'TPKD'),  -- Hoàn thành đúng hạn
-- Hợp đồng đang thực hiện (InProgress)
('HD031', 'KH031', DATEADD(day, -25, GETDATE()), DATEADD(day, 5, GETDATE()), 19000000, 'InProgress', 'TPKD'),
('HD032', 'KH032', DATEADD(day, -20, GETDATE()), DATEADD(day, 10, GETDATE()), 24000000, 'InProgress', 'TPKD'),
('HD033', 'KH033', DATEADD(day, -15, GETDATE()), DATEADD(day, 15, GETDATE()), 27500000, 'InProgress', 'TPKD'),
('HD034', 'KH034', DATEADD(day, -10, GETDATE()), DATEADD(day, 20, GETDATE()), 22500000, 'InProgress', 'TPKD'),
('HD035', 'KH035', DATEADD(day, -5, GETDATE()), DATEADD(day, 25, GETDATE()), 31000000, 'InProgress', 'TPKD'),
('HD036', 'KH036', DATEADD(day, -3, GETDATE()), DATEADD(day, 27, GETDATE()), 26500000, 'InProgress', 'TPKD'),
('HD037', 'KH037', DATEADD(day, -2, GETDATE()), DATEADD(day, 28, GETDATE()), 29000000, 'InProgress', 'TPKD'),
('HD038', 'KH038', DATEADD(day, -1, GETDATE()), DATEADD(day, 29, GETDATE()), 25500000, 'InProgress', 'TPKD'),
-- Hợp đồng đã hủy (Cancelled)
('HD039', 'KH039', DATEADD(day, -50, GETDATE()), DATEADD(day, -20, GETDATE()), 18000000, 'Cancelled', 'TPKD'),
('HD040', 'KH040', DATEADD(day, -45, GETDATE()), DATEADD(day, -15, GETDATE()), 22000000, 'Cancelled', 'TPKD'),
('HD041', 'KH041', DATEADD(day, -40, GETDATE()), DATEADD(day, -10, GETDATE()), 19500000, 'Cancelled', 'TPKD'),
('HD042', 'KH042', DATEADD(day, -35, GETDATE()), DATEADD(day, -5, GETDATE()), 28000000, 'Cancelled', 'TPKD'),
('HD043', 'KH043', DATEADD(day, -30, GETDATE()), DATEADD(day, 0, GETDATE()), 25000000, 'Cancelled', 'TPKD');
GO

-- Thêm ContractItem cho 30 hợp đồng mới
INSERT INTO ContractItem(ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType) VALUES
-- HD014-HD030 (Done)
('OK014', 'HD014', N'OK014', N'Khí thải ống khói', N'Khu công nghiệp A', 'D003', N'Khí thải'),
('NT015', 'HD015', N'NT015', N'Nước thải sau xử lý', N'Khu xử lý nước', 'D003', N'Nước thải'),
('KK016', 'HD016', N'KK016', N'Không khí xung quanh', N'Khu dân cư', 'D003', N'Không khí'),
('TN017', 'HD017', N'TN017', N'Tiếng ồn công nghiệp', N'Khu sản xuất', 'D003', N'Tiếng ồn'),
('OK018', 'HD018', N'OK018', N'Khí thải lò hơi', N'Khu lò hơi', 'D003', N'Khí thải'),
('NT019', 'HD019', N'NT019', N'Nước thải sinh hoạt', N'Khu xử lý', 'D003', N'Nước thải'),
('KK020', 'HD020', N'KK020', N'Không khí xung quanh', N'Khu công nghiệp B', 'D003', N'Không khí'),
('OK021', 'HD021', N'OK021', N'Khí thải ống khói', N'Ống khói số 1', 'D003', N'Khí thải'),
('NT022', 'HD022', N'NT022', N'Nước thải công nghiệp', N'Khu xử lý nước', 'D003', N'Nước thải'),
('TN023', 'HD023', N'TN023', N'Tiếng ồn máy móc', N'Xưởng sản xuất', 'D003', N'Tiếng ồn'),
('OK024', 'HD024', N'OK024', N'Khí thải ống khói', N'Ống khói số 2', 'D003', N'Khí thải'),
('NT025', 'HD025', N'NT025', N'Nước thải sau xử lý', N'Khu xử lý', 'D003', N'Nước thải'),
('KK026', 'HD026', N'KK026', N'Không khí xung quanh', N'Khu dân cư', 'D003', N'Không khí'),
('OK027', 'HD027', N'OK027', N'Khí thải lò đốt', N'Khu lò đốt', 'D003', N'Khí thải'),
('NT028', 'HD028', N'NT028', N'Nước thải sinh hoạt', N'Khu xử lý nước', 'D003', N'Nước thải'),
('TN029', 'HD029', N'TN029', N'Tiếng ồn công nghiệp', N'Khu sản xuất', 'D003', N'Tiếng ồn'),
('OK030', 'HD030', N'OK030', N'Khí thải ống khói', N'Ống khói số 3', 'D003', N'Khí thải'),
-- HD031-HD038 (InProgress)
('OK031', 'HD031', N'OK031', N'Khí thải ống khói', N'Khu công nghiệp C', 'D003', N'Khí thải'),
('NT032', 'HD032', N'NT032', N'Nước thải sau xử lý', N'Khu xử lý', 'D003', N'Nước thải'),
('KK033', 'HD033', N'KK033', N'Không khí xung quanh', N'Khu dân cư', 'D003', N'Không khí'),
('TN034', 'HD034', N'TN034', N'Tiếng ồn máy móc', N'Xưởng sản xuất', 'D003', N'Tiếng ồn'),
('OK035', 'HD035', N'OK035', N'Khí thải lò hơi', N'Khu lò hơi', 'D003', N'Khí thải'),
('NT036', 'HD036', N'NT036', N'Nước thải công nghiệp', N'Khu xử lý nước', 'D003', N'Nước thải'),
('KK037', 'HD037', N'KK037', N'Không khí xung quanh', N'Khu công nghiệp D', 'D003', N'Không khí'),
('OK038', 'HD038', N'OK038', N'Khí thải ống khói', N'Ống khói số 4', 'D003', N'Khí thải'),
-- HD039-HD043 (Cancelled)
('OK039', 'HD039', N'OK039', N'Khí thải ống khói', N'Khu công nghiệp E', 'D003', N'Khí thải'),
('NT040', 'HD040', N'NT040', N'Nước thải sau xử lý', N'Khu xử lý', 'D003', N'Nước thải'),
('KK041', 'HD041', N'KK041', N'Không khí xung quanh', N'Khu dân cư', 'D003', N'Không khí'),
('TN042', 'HD042', N'TN042', N'Tiếng ồn công nghiệp', N'Khu sản xuất', 'D003', N'Tiếng ồn'),
('OK043', 'HD043', N'OK043', N'Khí thải lò hơi', N'Khu lò hơi', 'D003', N'Khí thải');
GO

-- Thêm Assignment cho các ContractItem (phân công thông số)
INSERT INTO Assignment(ItemId, ParameterId, DepartmentId) VALUES
-- HD014-HD030 (Done) - mỗi item có 3-5 thông số
('OK014', 'TS001', 'D004'), ('OK014', 'TS002', 'D004'), ('OK014', 'TS003', 'D005'), ('OK014', 'TS004', 'D005'), ('OK014', 'TS007', 'D004'),
('NT015', 'NT005', 'D004'), ('NT015', 'NT006', 'D005'), ('NT015', 'NT007', 'D005'), ('NT015', 'NT008', 'D004'),
('KK016', 'KK001', 'D004'), ('KK016', 'KK002', 'D005'), ('KK016', 'KK003', 'D005'),
('TN017', 'TN001', 'D004'), ('TN017', 'TN002', 'D004'),
('OK018', 'TS001', 'D004'), ('OK018', 'TS003', 'D005'), ('OK018', 'TS004', 'D005'), ('OK018', 'TS007', 'D004'),
('NT019', 'NT005', 'D004'), ('NT019', 'NT006', 'D005'), ('NT019', 'NT007', 'D005'),
('KK020', 'KK001', 'D004'), ('KK020', 'KK002', 'D005'), ('KK020', 'KK003', 'D005'),
('OK021', 'TS001', 'D004'), ('OK021', 'TS002', 'D004'), ('OK021', 'TS003', 'D005'), ('OK021', 'TS004', 'D005'),
('NT022', 'NT005', 'D004'), ('NT022', 'NT006', 'D005'), ('NT022', 'NT007', 'D005'), ('NT022', 'NT008', 'D004'),
('TN023', 'TN001', 'D004'), ('TN023', 'TN002', 'D004'),
('OK024', 'TS001', 'D004'), ('OK024', 'TS003', 'D005'), ('OK024', 'TS007', 'D004'),
('NT025', 'NT005', 'D004'), ('NT025', 'NT006', 'D005'), ('NT025', 'NT007', 'D005'),
('KK026', 'KK001', 'D004'), ('KK026', 'KK002', 'D005'),
('OK027', 'TS001', 'D004'), ('OK027', 'TS002', 'D004'), ('OK027', 'TS003', 'D005'), ('OK027', 'TS004', 'D005'),
('NT028', 'NT005', 'D004'), ('NT028', 'NT006', 'D005'), ('NT028', 'NT007', 'D005'),
('TN029', 'TN001', 'D004'), ('TN029', 'TN002', 'D004'),
('OK030', 'TS001', 'D004'), ('OK030', 'TS003', 'D005'), ('OK030', 'TS004', 'D005'), ('OK030', 'TS007', 'D004'),
-- HD031-HD038 (InProgress)
('OK031', 'TS001', 'D004'), ('OK031', 'TS002', 'D004'), ('OK031', 'TS003', 'D005'),
('NT032', 'NT005', 'D004'), ('NT032', 'NT006', 'D005'), ('NT032', 'NT007', 'D005'),
('KK033', 'KK001', 'D004'), ('KK033', 'KK002', 'D005'),
('TN034', 'TN001', 'D004'), ('TN034', 'TN002', 'D004'),
('OK035', 'TS001', 'D004'), ('OK035', 'TS003', 'D005'), ('OK035', 'TS007', 'D004'),
('NT036', 'NT005', 'D004'), ('NT036', 'NT006', 'D005'),
('KK037', 'KK001', 'D004'), ('KK037', 'KK002', 'D005'), ('KK037', 'KK003', 'D005'),
('OK038', 'TS001', 'D004'), ('OK038', 'TS002', 'D004'), ('OK038', 'TS003', 'D005'),
-- HD039-HD043 (Cancelled)
('OK039', 'TS001', 'D004'), ('OK039', 'TS002', 'D004'), ('OK039', 'TS003', 'D005'),
('NT040', 'NT005', 'D004'), ('NT040', 'NT006', 'D005'),
('KK041', 'KK001', 'D004'), ('KK041', 'KK002', 'D005'),
('TN042', 'TN001', 'D004'), ('TN042', 'TN002', 'D004'),
('OK043', 'TS001', 'D004'), ('OK043', 'TS003', 'D005');
GO

-- Thêm ItemParameterSetting cho các ContractItem
INSERT INTO ItemParameterSetting(ItemId, ParameterId, MinValue, MaxValue, Note, UpdatedBy) VALUES
-- HD014-HD030 (Done)
('OK014', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK014', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'), ('OK014', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
('NT015', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT015', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'), ('NT015', 'NT007', 0, 100, N'Giới hạn COD', 'Admin'),
('KK016', 'KK001', 0, 150, N'Giới hạn PM10', 'Admin'), ('KK016', 'KK002', 0, 75, N'Giới hạn PM2.5', 'Admin'),
('TN017', 'TN001', 0, 70, N'Giới hạn tiếng ồn ban ngày', 'Admin'), ('TN017', 'TN002', 0, 55, N'Giới hạn tiếng ồn ban đêm', 'Admin'),
('OK018', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK018', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
('NT019', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT019', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('KK020', 'KK001', 0, 150, N'Giới hạn PM10', 'Admin'), ('KK020', 'KK002', 0, 75, N'Giới hạn PM2.5', 'Admin'),
('OK021', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK021', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'),
('NT022', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT022', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('TN023', 'TN001', 0, 70, N'Giới hạn tiếng ồn ban ngày', 'Admin'), ('TN023', 'TN002', 0, 55, N'Giới hạn tiếng ồn ban đêm', 'Admin'),
('OK024', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK024', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
('NT025', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT025', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('KK026', 'KK001', 0, 150, N'Giới hạn PM10', 'Admin'), ('KK026', 'KK002', 0, 75, N'Giới hạn PM2.5', 'Admin'),
('OK027', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK027', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'),
('NT028', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT028', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('TN029', 'TN001', 0, 70, N'Giới hạn tiếng ồn ban ngày', 'Admin'), ('TN029', 'TN002', 0, 55, N'Giới hạn tiếng ồn ban đêm', 'Admin'),
('OK030', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK030', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
-- HD031-HD038 (InProgress)
('OK031', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK031', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'),
('NT032', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT032', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('KK033', 'KK001', 0, 150, N'Giới hạn PM10', 'Admin'), ('KK033', 'KK002', 0, 75, N'Giới hạn PM2.5', 'Admin'),
('TN034', 'TN001', 0, 70, N'Giới hạn tiếng ồn ban ngày', 'Admin'), ('TN034', 'TN002', 0, 55, N'Giới hạn tiếng ồn ban đêm', 'Admin'),
('OK035', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK035', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
('NT036', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT036', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('KK037', 'KK001', 0, 150, N'Giới hạn PM10', 'Admin'), ('KK037', 'KK002', 0, 75, N'Giới hạn PM2.5', 'Admin'),
('OK038', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK038', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'),
-- HD039-HD043 (Cancelled)
('OK039', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK039', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'),
('NT040', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT040', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('KK041', 'KK001', 0, 150, N'Giới hạn PM10', 'Admin'), ('KK041', 'KK002', 0, 75, N'Giới hạn PM2.5', 'Admin'),
('TN042', 'TN001', 0, 70, N'Giới hạn tiếng ồn ban ngày', 'Admin'), ('TN042', 'TN002', 0, 55, N'Giới hạn tiếng ồn ban đêm', 'Admin'),
('OK043', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK043', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin');
GO

-- Thêm MeasurementBatch cho các hợp đồng Done (HD014-HD030) - đã hoàn thành
INSERT INTO MeasurementBatch(BatchId, ItemId, StartDate, EndDate, Status, LeaderCompany_UserId) VALUES
('B014', 'OK014', DATEADD(day, -120, GETDATE()), DATEADD(day, -90, GETDATE()), 'Completed', 'TPKD'),
('B015', 'NT015', DATEADD(day, -110, GETDATE()), DATEADD(day, -85, GETDATE()), 'Completed', 'TPKD'),
('B016', 'KK016', DATEADD(day, -100, GETDATE()), DATEADD(day, -75, GETDATE()), 'Completed', 'TPKD'),
('B017', 'TN017', DATEADD(day, -95, GETDATE()), DATEADD(day, -70, GETDATE()), 'Completed', 'TPKD'),
('B018', 'OK018', DATEADD(day, -90, GETDATE()), DATEADD(day, -65, GETDATE()), 'Completed', 'TPKD'),
('B019', 'NT019', DATEADD(day, -85, GETDATE()), DATEADD(day, -60, GETDATE()), 'Completed', 'TPKD'),
('B020', 'KK020', DATEADD(day, -80, GETDATE()), DATEADD(day, -55, GETDATE()), 'Completed', 'TPKD'),
('B021', 'OK021', DATEADD(day, -75, GETDATE()), DATEADD(day, -50, GETDATE()), 'Completed', 'TPKD'),
('B022', 'NT022', DATEADD(day, -70, GETDATE()), DATEADD(day, -45, GETDATE()), 'Completed', 'TPKD'),
('B023', 'TN023', DATEADD(day, -65, GETDATE()), DATEADD(day, -40, GETDATE()), 'Completed', 'TPKD'),
('B024', 'OK024', DATEADD(day, -60, GETDATE()), DATEADD(day, -35, GETDATE()), 'Completed', 'TPKD'),
('B025', 'NT025', DATEADD(day, -55, GETDATE()), DATEADD(day, -30, GETDATE()), 'Completed', 'TPKD'),
('B026', 'KK026', DATEADD(day, -50, GETDATE()), DATEADD(day, -25, GETDATE()), 'Completed', 'TPKD'),
('B027', 'OK027', DATEADD(day, -45, GETDATE()), DATEADD(day, -20, GETDATE()), 'Completed', 'TPKD'),
('B028', 'NT028', DATEADD(day, -40, GETDATE()), DATEADD(day, -15, GETDATE()), 'Completed', 'TPKD'),
('B029', 'TN029', DATEADD(day, -35, GETDATE()), DATEADD(day, -10, GETDATE()), 'Completed', 'TPKD'),
('B030', 'OK030', DATEADD(day, -30, GETDATE()), DATEADD(day, -5, GETDATE()), 'Completed', 'TPKD'),
-- Thêm MeasurementBatch cho các hợp đồng InProgress (HD031-HD038)
('B031', 'OK031', DATEADD(day, -25, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B032', 'NT032', DATEADD(day, -20, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B033', 'KK033', DATEADD(day, -15, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B034', 'TN034', DATEADD(day, -10, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B035', 'OK035', DATEADD(day, -5, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B036', 'NT036', DATEADD(day, -3, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B037', 'KK037', DATEADD(day, -2, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B038', 'OK038', DATEADD(day, -1, GETDATE()), NULL, 'InProgress', 'TPKD'),
-- Thêm MeasurementBatch cho các hợp đồng Cancelled (HD039-HD043) - đã hủy
('B039', 'OK039', DATEADD(day, -50, GETDATE()), DATEADD(day, -20, GETDATE()), 'Completed', 'TPKD'),
('B040', 'NT040', DATEADD(day, -45, GETDATE()), DATEADD(day, -15, GETDATE()), 'Completed', 'TPKD'),
('B041', 'KK041', DATEADD(day, -40, GETDATE()), DATEADD(day, -10, GETDATE()), 'Completed', 'TPKD'),
('B042', 'TN042', DATEADD(day, -35, GETDATE()), DATEADD(day, -5, GETDATE()), 'Completed', 'TPKD'),
('B043', 'OK043', DATEADD(day, -30, GETDATE()), DATEADD(day, 0, GETDATE()), 'Completed', 'TPKD');
GO

-- Thêm Result cho các MeasurementBatch đã hoàn thành (HD014-HD030 và HD039-HD043)
-- Một số kết quả có OutofStandard = 1 để test AI prediction
INSERT INTO Result(ResultId, BatchId, ParameterId, Value, Company_UserId, OutofStandard, Note) VALUES
-- HD014 - có 1 kết quả không đạt
('KQ014', 'B014', 'TS001', 35.0, 'TPKD', 0, N'Đạt'),
('KQ015', 'B014', 'TS002', 85.0, 'TPKD', 0, N'Đạt'),
('KQ016', 'B014', 'TS003', 320.0, 'TPKD', 0, N'Đạt'),
('KQ017', 'B014', 'TS004', 25.0, 'TPKD', 0, N'Đạt'),
('KQ018', 'B014', 'TS007', 180.0, 'TPKD', 1, N'Không đạt - vượt giới hạn'),
-- HD015 - tất cả đạt
('KQ019', 'B015', 'NT005', 7.2, 'TPKD', 0, N'Đạt'),
('KQ020', 'B015', 'NT006', 35.0, 'TPKD', 0, N'Đạt'),
('KQ021', 'B015', 'NT007', 80.0, 'TPKD', 0, N'Đạt'),
('KQ022', 'B015', 'NT008', 75.0, 'TPKD', 0, N'Đạt'),
-- HD016 - tất cả đạt
('KQ023', 'B016', 'KK001', 120.0, 'TPKD', 0, N'Đạt'),
('KQ024', 'B016', 'KK002', 60.0, 'TPKD', 0, N'Đạt'),
('KQ025', 'B016', 'KK003', 280.0, 'TPKD', 0, N'Đạt'),
-- HD017 - có 1 kết quả không đạt
('KQ026', 'B017', 'TN001', 65.0, 'TPKD', 0, N'Đạt'),
('KQ027', 'B017', 'TN002', 50.0, 'TPKD', 0, N'Đạt'),
-- HD018 - tất cả đạt
('KQ028', 'B018', 'TS001', 32.0, 'TPKD', 0, N'Đạt'),
('KQ029', 'B018', 'TS003', 300.0, 'TPKD', 0, N'Đạt'),
('KQ030', 'B018', 'TS004', 20.0, 'TPKD', 0, N'Đạt'),
('KQ031', 'B018', 'TS007', 150.0, 'TPKD', 0, N'Đạt'),
-- HD019 - tất cả đạt
('KQ032', 'B019', 'NT005', 7.5, 'TPKD', 0, N'Đạt'),
('KQ033', 'B019', 'NT006', 40.0, 'TPKD', 0, N'Đạt'),
('KQ034', 'B019', 'NT007', 85.0, 'TPKD', 0, N'Đạt'),
-- HD020 - tất cả đạt
('KQ035', 'B020', 'KK001', 110.0, 'TPKD', 0, N'Đạt'),
('KQ036', 'B020', 'KK002', 55.0, 'TPKD', 0, N'Đạt'),
('KQ037', 'B020', 'KK003', 250.0, 'TPKD', 0, N'Đạt'),
-- HD021 - có 2 kết quả không đạt
('KQ038', 'B021', 'TS001', 38.0, 'TPKD', 0, N'Đạt'),
('KQ039', 'B021', 'TS002', 95.0, 'TPKD', 0, N'Đạt'),
('KQ040', 'B021', 'TS003', 380.0, 'TPKD', 1, N'Không đạt - vượt giới hạn'),
('KQ041', 'B021', 'TS004', 28.0, 'TPKD', 0, N'Đạt'),
-- HD022 - tất cả đạt
('KQ042', 'B022', 'NT005', 7.0, 'TPKD', 0, N'Đạt'),
('KQ043', 'B022', 'NT006', 30.0, 'TPKD', 0, N'Đạt'),
('KQ044', 'B022', 'NT007', 70.0, 'TPKD', 0, N'Đạt'),
('KQ045', 'B022', 'NT008', 60.0, 'TPKD', 0, N'Đạt'),
-- HD023 - tất cả đạt
('KQ046', 'B023', 'TN001', 60.0, 'TPKD', 0, N'Đạt'),
('KQ047', 'B023', 'TN002', 45.0, 'TPKD', 0, N'Đạt'),
-- HD024 - tất cả đạt
('KQ048', 'B024', 'TS001', 30.0, 'TPKD', 0, N'Đạt'),
('KQ049', 'B024', 'TS003', 280.0, 'TPKD', 0, N'Đạt'),
('KQ050', 'B024', 'TS007', 140.0, 'TPKD', 0, N'Đạt'),
-- HD025 - tất cả đạt
('KQ051', 'B025', 'NT005', 7.8, 'TPKD', 0, N'Đạt'),
('KQ052', 'B025', 'NT006', 45.0, 'TPKD', 0, N'Đạt'),
('KQ053', 'B025', 'NT007', 90.0, 'TPKD', 0, N'Đạt'),
-- HD026 - tất cả đạt
('KQ054', 'B026', 'KK001', 130.0, 'TPKD', 0, N'Đạt'),
('KQ055', 'B026', 'KK002', 65.0, 'TPKD', 0, N'Đạt'),
-- HD027 - có 1 kết quả không đạt
('KQ056', 'B027', 'TS001', 36.0, 'TPKD', 0, N'Đạt'),
('KQ057', 'B027', 'TS002', 90.0, 'TPKD', 0, N'Đạt'),
('KQ058', 'B027', 'TS003', 310.0, 'TPKD', 0, N'Đạt'),
('KQ059', 'B027', 'TS004', 22.0, 'TPKD', 0, N'Đạt'),
-- HD028 - tất cả đạt
('KQ060', 'B028', 'NT005', 7.3, 'TPKD', 0, N'Đạt'),
('KQ061', 'B028', 'NT006', 38.0, 'TPKD', 0, N'Đạt'),
('KQ062', 'B028', 'NT007', 75.0, 'TPKD', 0, N'Đạt'),
-- HD029 - tất cả đạt
('KQ063', 'B029', 'TN001', 58.0, 'TPKD', 0, N'Đạt'),
('KQ064', 'B029', 'TN002', 42.0, 'TPKD', 0, N'Đạt'),
-- HD030 - tất cả đạt
('KQ065', 'B030', 'TS001', 28.0, 'TPKD', 0, N'Đạt'),
('KQ066', 'B030', 'TS003', 290.0, 'TPKD', 0, N'Đạt'),
('KQ067', 'B030', 'TS004', 18.0, 'TPKD', 0, N'Đạt'),
('KQ068', 'B030', 'TS007', 130.0, 'TPKD', 0, N'Đạt'),
-- HD039-HD043 (Cancelled) - một số kết quả
('KQ069', 'B039', 'TS001', 35.0, 'TPKD', 0, N'Đạt'),
('KQ070', 'B039', 'TS002', 80.0, 'TPKD', 0, N'Đạt'),
('KQ071', 'B039', 'TS003', 300.0, 'TPKD', 0, N'Đạt'),
('KQ072', 'B040', 'NT005', 7.0, 'TPKD', 0, N'Đạt'),
('KQ073', 'B040', 'NT006', 40.0, 'TPKD', 0, N'Đạt'),
('KQ074', 'B041', 'KK001', 125.0, 'TPKD', 0, N'Đạt'),
('KQ075', 'B041', 'KK002', 62.0, 'TPKD', 0, N'Đạt'),
('KQ076', 'B042', 'TN001', 62.0, 'TPKD', 0, N'Đạt'),
('KQ077', 'B042', 'TN002', 48.0, 'TPKD', 0, N'Đạt'),
('KQ078', 'B043', 'TS001', 32.0, 'TPKD', 0, N'Đạt'),
('KQ079', 'B043', 'TS003', 280.0, 'TPKD', 0, N'Đạt');
GO

PRINT 'Da them 30 hop dong moi (HD014-HD043) voi day du du lieu lien quan de test AI prediction.';
GO

USE QLQTMT;
GO

-- Cap nhat EndDate cho cac hop dong cu the can sua
UPDATE mb SET mb.EndDate = GETDATE()
FROM MeasurementBatch mb JOIN ContractItem ci ON mb.ItemId = ci.ItemId
WHERE ci.ContractId IN ('HD004', 'HD008'); -- Chi sua EndDate cho HD004 va HD008

-- Cap nhat cac hop dong da Huy
UPDATE mb SET mb.EndDate = c.PlanDueDate
FROM MeasurementBatch mb JOIN ContractItem ci ON mb.ItemId = ci.ItemId JOIN Contract c ON ci.ContractId = c.ContractId
WHERE c.Status = 'Cancelled' AND mb.EndDate IS NULL;
GO

PRINT 'Da cap nhat EndDate de tinh DaysOverdue cho du lieu that chinh xac.';
GO

-- =====================================================================
-- TAO/CAP NHAT VIEW NGUON VOI LOGIC CHONG TRUNG LAP
-- =====================================================================
IF OBJECT_ID('V_Contract_History_For_AI', 'V') IS NOT NULL
    DROP VIEW V_Contract_History_For_AI;
GO

CREATE VIEW V_Contract_History_For_AI AS
WITH ContractFeatures AS (
    SELECT
        c.ContractId, c.CustomerId, c.SignDate, ISNULL(MAX(mb.EndDate), c.PlanDueDate) AS EffectiveCompletionDate,
        c.PlanDueDate, LEAD(c.SignDate, 1) OVER (PARTITION BY c.CustomerId ORDER BY c.SignDate) as NextContractSignDate
    FROM Contract c
    LEFT JOIN ContractItem ci ON c.ContractId = ci.ContractId
    LEFT JOIN MeasurementBatch mb ON ci.ItemId = mb.ItemId
    WHERE c.Status IN ('Done', 'Cancelled')
    GROUP BY c.ContractId, c.CustomerId, c.SignDate, c.PlanDueDate
)
SELECT 
    f.ContractId, f.CustomerId,
    CAST(DATEDIFF(day, f.SignDate, f.EffectiveCompletionDate) AS REAL) AS CompletionTimeInDays,
    CAST(CASE WHEN DATEDIFF(day, f.PlanDueDate, f.EffectiveCompletionDate) > 0 THEN DATEDIFF(day, f.PlanDueDate, f.EffectiveCompletionDate) ELSE 0 END AS REAL) AS DaysOverdue,
    CAST(ISNULL((SELECT COUNT(*) FROM Result r JOIN MeasurementBatch mb ON r.BatchId = mb.BatchId JOIN ContractItem ci ON mb.ItemId = ci.ItemId WHERE ci.ContractId = f.ContractId AND r.OutofStandard = 1), 0) AS REAL) AS NonStandardResultsCount,
    CAST(ISNULL((SELECT COUNT(*) FROM Assignment a JOIN ContractItem ci ON a.ItemId = ci.ItemId WHERE ci.ContractId = f.ContractId), 0) AS REAL) AS TotalParametersCount,
    CAST(CASE WHEN f.NextContractSignDate IS NOT NULL AND DATEDIFF(day, f.EffectiveCompletionDate, f.NextContractSignDate) <= 90 THEN 1 ELSE 0 END AS BIT) AS Renewed
FROM ContractFeatures f;
GO

PRINT 'VIEW V_Contract_History_For_AI da san sang.';
GO

-- =====================================================================
-- CHUAN BI BANG RIENG CHO AI VA NAP DU LIEU THAT
-- =====================================================================
IF OBJECT_ID('AI_TrainingData', 'U') IS NULL
BEGIN
    CREATE TABLE AI_TrainingData (
        ContractId VARCHAR(20) PRIMARY KEY, CustomerId VARCHAR(20), CompletionTimeInDays REAL,
        DaysOverdue REAL, NonStandardResultsCount REAL, TotalParametersCount REAL, Renewed BIT
    );
END;
GO

TRUNCATE TABLE AI_TrainingData;
GO

INSERT INTO AI_TrainingData (
    ContractId, CustomerId, CompletionTimeInDays, DaysOverdue, 
    NonStandardResultsCount, TotalParametersCount, Renewed
)
SELECT 
    ContractId, CustomerId, CompletionTimeInDays, DaysOverdue, 
    NonStandardResultsCount, TotalParametersCount, Renewed
FROM V_Contract_History_For_AI;
GO

PRINT 'Bang AI_TrainingData da nap du lieu that thanh cong.';
GO

-- =====================================================================
-- TAO DU LIEU AO VA CHEN TRUC TIEP VAO BANG AI
-- =====================================================================
SET NOCOUNT ON;
DECLARE @i INT = 20;
WHILE @i <= 120
BEGIN
    DECLARE @ContractId_AI VARCHAR(20) = 'HD_AI_' + FORMAT(@i, '000');
    DECLARE @CustomerId_AI VARCHAR(20) = 'KH_AI_' + FORMAT(ABS(CHECKSUM(NEWID())) % 50, '00');
    DECLARE @SignDate_AI DATE = DATEADD(day, - (ABS(CHECKSUM(NEWID())) % 365 + 60), GETDATE());
    DECLARE @PlanDueDate_AI DATE = DATEADD(day, 30, @SignDate_AI);
    DECLARE @DelayDays_AI INT = CASE WHEN RAND() < 0.3 THEN ABS(CHECKSUM(NEWID())) % 15 + 1 ELSE 0 END;
    DECLARE @HasBadResult_AI BIT = CASE WHEN RAND() < 0.25 THEN 1 ELSE 0 END;
    DECLARE @WillRenew_AI BIT = 1;
    
    IF @DelayDays_AI > 10 OR @HasBadResult_AI = 1 BEGIN SET @WillRenew_AI = 0; END
    IF @DelayDays_AI < 5 AND @HasBadResult_AI = 0 AND RAND() < 0.2 BEGIN SET @WillRenew_AI = 0; END
    
    DECLARE @ActualCompletionDate_AI DATE = DATEADD(day, @DelayDays_AI, @PlanDueDate_AI);

    INSERT INTO AI_TrainingData (
        ContractId, CustomerId, CompletionTimeInDays, DaysOverdue, 
        NonStandardResultsCount, TotalParametersCount, Renewed
    )
    VALUES (
        @ContractId_AI, @CustomerId_AI,
        CAST(DATEDIFF(day, @SignDate_AI, @ActualCompletionDate_AI) AS REAL),
        CAST(CASE WHEN DATEDIFF(day, @PlanDueDate_AI, @ActualCompletionDate_AI) > 0 THEN DATEDIFF(day, @PlanDueDate_AI, @ActualCompletionDate_AI) ELSE 0 END AS REAL),
        CAST(CASE WHEN @HasBadResult_AI = 1 THEN 1 ELSE 0 END AS REAL),
        CAST(ABS(CHECKSUM(NEWID())) % 15 + 1 AS REAL),
        @WillRenew_AI
    );

    SET @i = @i + 1;
END;
GO

PRINT ' 101 dong du lieu da duoc chen vao AI_TrainingData.';
GO

-- =====================================================================
-- THEM 10 HOP DONG DA QUA HAN DE TEST LOGIC TU DONG HOAN THANH
-- =====================================================================
USE QLQTMT;
GO

-- Thêm 10 hợp đồng đã quá hạn (PlanDueDate < GETDATE() và Status = 'InProgress' hoặc 'Planned')
INSERT INTO Contract (ContractId, CustomerId, SignDate, PlanDueDate, Amount, Status, SalesCompany_UserId) VALUES
-- Hợp đồng quá hạn 1-10 ngày
('HD044', 'KH044', DATEADD(day, -40, GETDATE()), DATEADD(day, -10, GETDATE()), 18000000, 'InProgress', 'TPKD'),  -- Quá hạn 10 ngày
('HD045', 'KH045', DATEADD(day, -35, GETDATE()), DATEADD(day, -8, GETDATE()), 22000000, 'InProgress', 'TPKD'),  -- Quá hạn 8 ngày
('HD046', 'KH046', DATEADD(day, -30, GETDATE()), DATEADD(day, -5, GETDATE()), 19500000, 'InProgress', 'TPKD'),  -- Quá hạn 5 ngày
('HD047', 'KH047', DATEADD(day, -28, GETDATE()), DATEADD(day, -3, GETDATE()), 28000000, 'InProgress', 'TPKD'),  -- Quá hạn 3 ngày
('HD048', 'KH048', DATEADD(day, -25, GETDATE()), DATEADD(day, -1, GETDATE()), 25000000, 'InProgress', 'TPKD'),  -- Quá hạn 1 ngày
-- Hợp đồng quá hạn 11-20 ngày
('HD049', 'KH049', DATEADD(day, -50, GETDATE()), DATEADD(day, -20, GETDATE()), 32000000, 'InProgress', 'TPKD'),  -- Quá hạn 20 ngày
('HD050', 'KH050', DATEADD(day, -45, GETDATE()), DATEADD(day, -15, GETDATE()), 21000000, 'InProgress', 'TPKD'),  -- Quá hạn 15 ngày
('HD051', 'KH051', DATEADD(day, -42, GETDATE()), DATEADD(day, -12, GETDATE()), 29000000, 'InProgress', 'TPKD'),  -- Quá hạn 12 ngày
-- Hợp đồng với Status = 'Planned' (cũng cần tự động hoàn thành)
('HD052', 'KH052', DATEADD(day, -38, GETDATE()), DATEADD(day, -7, GETDATE()), 24000000, 'Planned', 'TPKD'),  -- Quá hạn 7 ngày
('HD053', 'KH053', DATEADD(day, -33, GETDATE()), DATEADD(day, -2, GETDATE()), 35000000, 'Planned', 'TPKD');  -- Quá hạn 2 ngày
GO

-- Thêm ContractItem cho 10 hợp đồng quá hạn
INSERT INTO ContractItem(ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType) VALUES
('OK044', 'HD044', N'OK044', N'Khí thải ống khói', N'Khu công nghiệp F', 'D003', N'Khí thải'),
('NT045', 'HD045', N'NT045', N'Nước thải sau xử lý', N'Khu xử lý nước', 'D003', N'Nước thải'),
('KK046', 'HD046', N'KK046', N'Không khí xung quanh', N'Khu dân cư', 'D003', N'Không khí'),
('TN047', 'HD047', N'TN047', N'Tiếng ồn công nghiệp', N'Khu sản xuất', 'D003', N'Tiếng ồn'),
('OK048', 'HD048', N'OK048', N'Khí thải lò hơi', N'Khu lò hơi', 'D003', N'Khí thải'),
('NT049', 'HD049', N'NT049', N'Nước thải sinh hoạt', N'Khu xử lý', 'D003', N'Nước thải'),
('KK050', 'HD050', N'KK050', N'Không khí xung quanh', N'Khu công nghiệp G', 'D003', N'Không khí'),
('OK051', 'HD051', N'OK051', N'Khí thải ống khói', N'Ống khói số 5', 'D003', N'Khí thải'),
('NT052', 'HD052', N'NT052', N'Nước thải công nghiệp', N'Khu xử lý nước', 'D003', N'Nước thải'),
('TN053', 'HD053', N'TN053', N'Tiếng ồn máy móc', N'Xưởng sản xuất', 'D003', N'Tiếng ồn');
GO

-- Thêm Assignment cho các ContractItem
INSERT INTO Assignment(ItemId, ParameterId, DepartmentId) VALUES
-- HD044-HD048
('OK044', 'TS001', 'D004'), ('OK044', 'TS002', 'D004'), ('OK044', 'TS003', 'D005'), ('OK044', 'TS004', 'D005'),
('NT045', 'NT005', 'D004'), ('NT045', 'NT006', 'D005'), ('NT045', 'NT007', 'D005'), ('NT045', 'NT008', 'D004'),
('KK046', 'KK001', 'D004'), ('KK046', 'KK002', 'D005'), ('KK046', 'KK003', 'D005'),
('TN047', 'TN001', 'D004'), ('TN047', 'TN002', 'D004'),
('OK048', 'TS001', 'D004'), ('OK048', 'TS003', 'D005'), ('OK048', 'TS007', 'D004'),
-- HD049-HD053
('NT049', 'NT005', 'D004'), ('NT049', 'NT006', 'D005'), ('NT049', 'NT007', 'D005'),
('KK050', 'KK001', 'D004'), ('KK050', 'KK002', 'D005'),
('OK051', 'TS001', 'D004'), ('OK051', 'TS002', 'D004'), ('OK051', 'TS003', 'D005'),
('NT052', 'NT005', 'D004'), ('NT052', 'NT006', 'D005'), ('NT052', 'NT007', 'D005'),
('TN053', 'TN001', 'D004'), ('TN053', 'TN002', 'D004');
GO

-- Thêm ItemParameterSetting cho các ContractItem
INSERT INTO ItemParameterSetting(ItemId, ParameterId, MinValue, MaxValue, Note, UpdatedBy) VALUES
('OK044', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK044', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'),
('NT045', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT045', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('KK046', 'KK001', 0, 150, N'Giới hạn PM10', 'Admin'), ('KK046', 'KK002', 0, 75, N'Giới hạn PM2.5', 'Admin'),
('TN047', 'TN001', 0, 70, N'Giới hạn tiếng ồn ban ngày', 'Admin'), ('TN047', 'TN002', 0, 55, N'Giới hạn tiếng ồn ban đêm', 'Admin'),
('OK048', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK048', 'TS003', 0, 350, N'Giới hạn SO2', 'Admin'),
('NT049', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT049', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('KK050', 'KK001', 0, 150, N'Giới hạn PM10', 'Admin'), ('KK050', 'KK002', 0, 75, N'Giới hạn PM2.5', 'Admin'),
('OK051', 'TS001', 10, 40, N'Giới hạn nhiệt độ', 'Admin'), ('OK051', 'TS002', 50, 100, N'Giới hạn tiếng ồn', 'Admin'),
('NT052', 'NT005', 6.0, 9.0, N'Giới hạn pH', 'Admin'), ('NT052', 'NT006', 0, 50, N'Giới hạn BOD5', 'Admin'),
('TN053', 'TN001', 0, 70, N'Giới hạn tiếng ồn ban ngày', 'Admin'), ('TN053', 'TN002', 0, 55, N'Giới hạn tiếng ồn ban đêm', 'Admin');
GO

-- Thêm MeasurementBatch cho các hợp đồng quá hạn (đang thực hiện)
INSERT INTO MeasurementBatch(BatchId, ItemId, StartDate, EndDate, Status, LeaderCompany_UserId) VALUES
('B044', 'OK044', DATEADD(day, -40, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B045', 'NT045', DATEADD(day, -35, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B046', 'KK046', DATEADD(day, -30, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B047', 'TN047', DATEADD(day, -28, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B048', 'OK048', DATEADD(day, -25, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B049', 'NT049', DATEADD(day, -50, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B050', 'KK050', DATEADD(day, -45, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B051', 'OK051', DATEADD(day, -42, GETDATE()), NULL, 'InProgress', 'TPKD'),
('B052', 'NT052', DATEADD(day, -38, GETDATE()), NULL, 'Planned', 'TPKD'),
('B053', 'TN053', DATEADD(day, -33, GETDATE()), NULL, 'Planned', 'TPKD');
GO

-- =====================================================================
-- CAP NHAT MA NEN MAU THEO LOAI MAU
-- Quy tắc đặt tên:
-- Khí thải → KT01, KT02, KT03... (Khí Thải)
-- Nước thải → NT01, NT02, NT03... (Nước Thải)
-- Không khí → KK01, KK02, KK03... (Không Khí)
-- Tiếng ồn → TN01, TN02, TN03... (Tiếng Ồn)
-- Lưu ý: Không được đặt trùng giữa các nền mẫu
-- =====================================================================
USE QLQTMT;
GO

BEGIN TRANSACTION;

-- Tạo bảng tạm để lưu mapping ItemId cũ → mới
CREATE TABLE #ItemIdMapping (
    OldItemId VARCHAR(10),
    NewItemId VARCHAR(10),
    SampleType NVARCHAR(50),
    RowNum INT
);

-- Điền mapping dựa trên SampleType với số thứ tự liên tục cho mỗi loại
INSERT INTO #ItemIdMapping (OldItemId, NewItemId, SampleType, RowNum)
SELECT 
    ItemId AS OldItemId,
    CASE 
        WHEN SampleType = N'Khí thải' THEN 'KT' + RIGHT('0' + CAST(ROW_NUMBER() OVER (PARTITION BY SampleType ORDER BY ContractId, ItemId) AS VARCHAR(2)), 2)
        WHEN SampleType = N'Nước thải' THEN 'NT' + RIGHT('0' + CAST(ROW_NUMBER() OVER (PARTITION BY SampleType ORDER BY ContractId, ItemId) AS VARCHAR(2)), 2)
        WHEN SampleType = N'Không khí' THEN 'KK' + RIGHT('0' + CAST(ROW_NUMBER() OVER (PARTITION BY SampleType ORDER BY ContractId, ItemId) AS VARCHAR(2)), 2)
        WHEN SampleType = N'Tiếng ồn' THEN 'TN' + RIGHT('0' + CAST(ROW_NUMBER() OVER (PARTITION BY SampleType ORDER BY ContractId, ItemId) AS VARCHAR(2)), 2)
        ELSE ItemId
    END AS NewItemId,
    SampleType,
    ROW_NUMBER() OVER (PARTITION BY SampleType ORDER BY ContractId, ItemId) AS RowNum
FROM ContractItem
WHERE SampleType IS NOT NULL;

-- Kiểm tra xem có trùng lặp không
IF EXISTS (
    SELECT NewItemId, COUNT(*) 
    FROM #ItemIdMapping 
    GROUP BY NewItemId 
    HAVING COUNT(*) > 1
)
BEGIN
    PRINT 'CANH BAO: Co ma nen mau bi trung lap!';
    SELECT NewItemId, COUNT(*) AS SoLuong
    FROM #ItemIdMapping 
    GROUP BY NewItemId 
    HAVING COUNT(*) > 1;
    ROLLBACK TRANSACTION;
    RETURN;
END

-- Cập nhật ContractItem (cần tạm thời disable foreign key constraints)
-- Bước 1: Tạo bảng tạm với ItemId mới
SELECT 
    m.NewItemId AS ItemId,
    ci.ContractId,
    ci.SampleCode,
    ci.SampleName,
    ci.SampleLocation,
    ci.DepartmentId,
    ci.SampleType
INTO #ContractItemNew
FROM ContractItem ci
INNER JOIN #ItemIdMapping m ON ci.ItemId = m.OldItemId;

-- Bước 2: Cập nhật MeasurementBatch trước (để giữ BatchId cho Result)
UPDATE mb
SET mb.ItemId = m.NewItemId
FROM MeasurementBatch mb
INNER JOIN #ItemIdMapping m ON mb.ItemId = m.OldItemId;

-- Bước 3: Xóa dữ liệu cũ từ các bảng con (không cần xóa Result vì BatchId không đổi)
DELETE FROM ItemParameterSetting WHERE ItemId IN (SELECT OldItemId FROM #ItemIdMapping);
DELETE FROM Assignment WHERE ItemId IN (SELECT OldItemId FROM #ItemIdMapping);

-- Bước 3: Xóa và insert lại ContractItem
DELETE FROM ContractItem WHERE ItemId IN (SELECT OldItemId FROM #ItemIdMapping);
INSERT INTO ContractItem (ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType)
SELECT ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType
FROM #ContractItemNew;

-- Bước 2: Lưu dữ liệu Assignment và ItemParameterSetting vào bảng tạm trước khi xóa
SELECT a.ItemId, a.ParameterId, a.DepartmentId
INTO #AssignmentTemp
FROM Assignment a
WHERE a.ItemId IN (SELECT OldItemId FROM #ItemIdMapping);

SELECT ips.ItemId, ips.ParameterId, ips.MinValue, ips.MaxValue, ips.Note, ips.SampleType, ips.UpdatedBy, ips.UpdatedAt
INTO #ItemParameterSettingTemp
FROM ItemParameterSetting ips
WHERE ips.ItemId IN (SELECT OldItemId FROM #ItemIdMapping);

-- Bước 3: Cập nhật MeasurementBatch trước (để giữ BatchId cho Result)
UPDATE mb
SET mb.ItemId = m.NewItemId
FROM MeasurementBatch mb
INNER JOIN #ItemIdMapping m ON mb.ItemId = m.OldItemId;

-- Bước 4: Xóa dữ liệu cũ từ các bảng con (không cần xóa Result vì BatchId không đổi)
DELETE FROM ItemParameterSetting WHERE ItemId IN (SELECT OldItemId FROM #ItemIdMapping);
DELETE FROM Assignment WHERE ItemId IN (SELECT OldItemId FROM #ItemIdMapping);

-- Bước 5: Xóa và insert lại ContractItem
DELETE FROM ContractItem WHERE ItemId IN (SELECT OldItemId FROM #ItemIdMapping);
INSERT INTO ContractItem (ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType)
SELECT ItemId, ContractId, SampleCode, SampleName, SampleLocation, DepartmentId, SampleType
FROM #ContractItemNew;

-- Bước 6: Cập nhật Assignment với ItemId mới từ bảng tạm
INSERT INTO Assignment (ItemId, ParameterId, DepartmentId)
SELECT m.NewItemId, a.ParameterId, a.DepartmentId
FROM #AssignmentTemp a
INNER JOIN #ItemIdMapping m ON a.ItemId = m.OldItemId;

-- Bước 7: Cập nhật ItemParameterSetting với ItemId mới từ bảng tạm
INSERT INTO ItemParameterSetting (ItemId, ParameterId, MinValue, MaxValue, Note, SampleType, UpdatedBy, UpdatedAt)
SELECT m.NewItemId, ips.ParameterId, ips.MinValue, ips.MaxValue, ips.Note, ips.SampleType, ips.UpdatedBy, ips.UpdatedAt
FROM #ItemParameterSettingTemp ips
INNER JOIN #ItemIdMapping m ON ips.ItemId = m.OldItemId;

-- Lưu ý: MeasurementBatch đã được cập nhật ở bước 2, Result không cần cập nhật vì BatchId không đổi

-- Xóa các bảng tạm
DROP TABLE #ItemIdMapping;
DROP TABLE #ContractItemNew;
DROP TABLE #AssignmentTemp;
DROP TABLE #ItemParameterSettingTemp;

COMMIT TRANSACTION;
GO

PRINT 'Da cap nhat tat ca ma nen mau theo quy tac: KT01-KTxx (Khi thai), NT01-NTxx (Nuoc thai), KK01-KKxx (Khong khi), TN01-TNxx (Tieng on).';
GO

-- =====================================================================
--KIEM TRA KET QUA CUOI CUNG
-- =====================================================================
SELECT * FROM AI_TrainingData;
GO

SELECT 
    vai.ContractId,
    vai.CustomerId,
    cust.CustomerName,
    vai.CompletionTimeInDays,
    vai.DaysOverdue,
    vai.NonStandardResultsCount,
    vai.TotalParametersCount 
FROM 
    V_Contract_History_For_AI AS vai
JOIN 
    Customer AS cust ON vai.CustomerId = cust.CustomerId
JOIN
    Contract AS c ON vai.ContractId = c.ContractId
WHERE 
    c.Status = 'Done'
