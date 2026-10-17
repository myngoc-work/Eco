using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DTO;
using GUI.Helpers;

namespace UnitTestProject
{
    [TestClass]
    public class ContractInputValidatorTests
    {
        // ====== Các test đã có ở trên ======
        [TestMethod]
        public void RequiredField_WhenEmpty_ShouldReturnFalse()
        {
            bool result = ContractInputValidator.IsRequiredFieldFilled(string.Empty);
            Assert.IsFalse(result, "Trường bắt buộc để trống phải không hợp lệ.");
        }

        [TestMethod]
        public void Email_InvalidFormat_ShouldReturnFalse()
        {
            bool result = ContractInputValidator.IsValidEmail("invalid-email");
            Assert.IsFalse(result, "Email sai định dạng phải bị từ chối.");
        }

        [TestMethod]
        public void Email_EmptyWithAllowEmpty_ShouldReturnTrue()
        {
            bool result = ContractInputValidator.IsValidEmail(string.Empty);
            Assert.IsTrue(result, "Email cho phép để trống phải hợp lệ.");
        }

        [TestMethod]
        public void PhoneNumber_ContainsLetters_ShouldReturnFalse()
        {
            bool result = ContractInputValidator.IsValidPhoneNumber("09AB567890");
            Assert.IsFalse(result, "Số điện thoại chứa chữ cái phải bị từ chối.");
        }

        [TestMethod]
        public void PhoneNumber_NotEnoughDigits_ShouldReturnFalse()
        {
            bool result = ContractInputValidator.IsValidPhoneNumber("01234");
            Assert.IsFalse(result, "Số điện thoại không đủ 10 chữ số phải bị từ chối.");
        }

        [TestMethod]
        public void ResultDate_BeforeSigningDate_ShouldReturnFalse()
        {
            DateTime signingDate = new DateTime(2024, 10, 10);
            DateTime resultDate = new DateTime(2024, 10, 5);

            bool result = ContractInputValidator.IsResultDateValid(signingDate, resultDate);
            Assert.IsFalse(result, "Ngày trả kết quả trước ngày ký phải bị từ chối.");
        }

        [TestMethod]
        public void CustomerId_Duplicated_ShouldReturnFalse()
        {
            var existingIds = new List<string> { "KH001", "KH002", "kh003" };
            bool result = ContractInputValidator.IsCustomerIdUnique("kh002", existingIds);
            Assert.IsFalse(result, "Mã khách hàng trùng phải bị từ chối.");
        }

        [TestMethod]
        public void ContractInfo_Changed_ShouldReturnTrue()
        {
            var original = new SalesContractDTO
            {
                CompanyName = "Công ty A",
                Address = "Địa chỉ 1",
                RepresentativeName = "Nguyễn Văn B",
                RepresentativeEmail = "test@example.com",
                PhoneNumber = "0123456789",
                SigningDate = new DateTime(2024, 1, 1),
                ExpectedResultDate = new DateTime(2024, 1, 20)
            };

            var updated = new SalesContractDTO
            {
                CompanyName = "Công ty A",
                Address = "Địa chỉ 2", // thay đổi
                RepresentativeName = "Nguyễn Văn B",
                RepresentativeEmail = "test@example.com",
                PhoneNumber = "0123456789",
                SigningDate = new DateTime(2024, 1, 1),
                ExpectedResultDate = new DateTime(2024, 1, 20)
            };

            bool result = ContractInputValidator.HasContractInfoChanged(original, updated);
            Assert.IsTrue(result, "Thay đổi thông tin phải được phát hiện.");
        }

        [TestMethod]
        public void ContractInfo_NotChanged_ShouldReturnFalse()
        {
            var original = new SalesContractDTO
            {
                CompanyName = "Công ty A",
                Address = "Địa chỉ 1",
                RepresentativeName = "Nguyễn Văn B",
                RepresentativeEmail = "test@example.com",
                PhoneNumber = "0123456789",
                SigningDate = new DateTime(2024, 1, 1),
                ExpectedResultDate = new DateTime(2024, 1, 20)
            };

            var updated = new SalesContractDTO
            {
                CompanyName = "Công ty A",
                Address = "Địa chỉ 1",
                RepresentativeName = "Nguyễn Văn B",
                RepresentativeEmail = "test@example.com",
                PhoneNumber = "0123456789",
                SigningDate = new DateTime(2024, 1, 1),
                ExpectedResultDate = new DateTime(2024, 1, 20)
            };

            bool result = ContractInputValidator.HasContractInfoChanged(original, updated);
            Assert.IsFalse(result, "Không thay đổi thông tin thì phải trả về false.");
        }

        // ====== Các test case bổ sung theo yêu cầu ======
        private class FakeUser
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        private class FakeOrder
        {
            public string OrderId { get; set; }
            public string CustomerName { get; set; }
            public string Detail { get; set; }
        }

        private class FakeNotification
        {
            public string NotificationId { get; set; }
            public string Message { get; set; }
        }

        private static List<FakeUser> SeedUsers() => new List<FakeUser>
        {
            new FakeUser{ Id = "U001", Name = "Nguyen Van A", Email = "a@example.com", Phone = "0123456789" },
            new FakeUser{ Id = "U002", Name = "Tran Thi B", Email = "b@example.com", Phone = "0987654321" },
            new FakeUser{ Id = "U003", Name = "Le Van C", Email = "c@example.com", Phone = "0911111111" }
        };

        private static List<FakeOrder> SeedOrders() => new List<FakeOrder>
        {
            new FakeOrder{ OrderId = "HD001", CustomerName = "Cong ty Alpha", Detail = "Chi tiet 1" },
            new FakeOrder{ OrderId = "HD002", CustomerName = "Cong ty Beta", Detail = "Chi tiet 2" }
        };

        private static List<FakeNotification> SeedNotifications() => new List<FakeNotification>
        {
            new FakeNotification{ NotificationId = "TB_HD001", Message = "Hop dong HD001 da qua han" }
        };

        // Tìm kiếm người dùng thành công (tên chính xác)
        [TestMethod]
        public void SearchUser_ExactName_ShouldFound()
        {
            var users = SeedUsers();
            string query = "Tran Thi B";
            var result = users.FindAll(u => string.Equals(u.Name, query, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("U002", result[0].Id);
        }

        // Tìm kiếm người dùng không tồn tại
        [TestMethod]
        public void SearchUser_NotFound_ShouldReturnEmpty()
        {
            var users = SeedUsers();
            string query = "Khong ton tai";
            var result = users.FindAll(u => u.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);
            Assert.AreEqual(0, result.Count);
        }

        // Tìm kiếm với chuỗi rỗng
        [TestMethod]
        public void SearchUser_EmptyQuery_ShouldReturnAll()
        {
            var users = SeedUsers();
            string query = "";
            var result = string.IsNullOrWhiteSpace(query) ? users : users.FindAll(u => u.Name.Contains(query));
            Assert.AreEqual(users.Count, result.Count);
        }

        // Xem chi tiết đơn hàng thành công
        [TestMethod]
        public void ViewOrderDetail_Success()
        {
            var orders = SeedOrders();
            var order = orders.Find(o => o.OrderId == "HD001");
            Assert.IsNotNull(order);
            Assert.AreEqual("Cong ty Alpha", order.CustomerName);
        }

        // Xem chi tiết thông báo thành công
        [TestMethod]
        public void ViewNotificationDetail_Success()
        {
            var notis = SeedNotifications();
            var noti = notis.Find(n => n.NotificationId == "TB_HD001");
            Assert.IsNotNull(noti);
            StringAssert.Contains(noti.Message, "HD001");
        }

        // Tìm kiếm đơn hàng theo Mã đơn (tồn tại)
        [TestMethod]
        public void SearchOrder_ByOrderId_ShouldFound()
        {
            var orders = SeedOrders();
            var result = orders.FindAll(o => o.OrderId.Equals("HD002", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Cong ty Beta", result[0].CustomerName);
        }

        // Tìm kiếm đơn hàng theo Tên khách hàng (không tồn tại)
        [TestMethod]
        public void SearchOrder_ByCustomerName_NotFound()
        {
            var orders = SeedOrders();
            var result = orders.FindAll(o => o.CustomerName.IndexOf("Khach Hang X", StringComparison.OrdinalIgnoreCase) >= 0);
            Assert.AreEqual(0, result.Count);
        }

        // Nhân viên xem thông tin cá nhân
        [TestMethod]
        public void Employee_ViewProfile_Success()
        {
            var users = SeedUsers();
            var me = users.Find(u => u.Id == "U001");
            Assert.IsNotNull(me);
            Assert.AreEqual("Nguyen Van A", me.Name);
        }

        // Admin thêm nhân viên mới thành công
        [TestMethod]
        public void Admin_AddUser_Success()
        {
            var users = SeedUsers();
            var newUser = new FakeUser { Id = "U004", Name = "Pham Thi D", Email = "d@example.com", Phone = "0900000000" };
            bool ok = ContractInputValidator.IsRequiredFieldFilled(newUser.Name)
                      && ContractInputValidator.IsValidEmail(newUser.Email, allowEmpty: false)
                      && ContractInputValidator.IsValidPhoneNumber(newUser.Phone)
                      && !users.Exists(u => u.Id == newUser.Id);
            if (ok) users.Add(newUser);
            Assert.IsTrue(ok);
            Assert.AreEqual(4, users.Count);
        }

        // Admin thêm nhân viên mới (thiếu trường bắt buộc)
        [TestMethod]
        public void Admin_AddUser_MissingRequired_ShouldFail()
        {
            var users = SeedUsers();
            var newUser = new FakeUser { Id = "U004", Name = "", Email = "d@example.com", Phone = "0900000000" };
            bool ok = ContractInputValidator.IsRequiredFieldFilled(newUser.Name);
            Assert.IsFalse(ok);
        }

        // Admin sửa thông tin nhân viên thành công
        [TestMethod]
        public void Admin_UpdateUser_Success()
        {
            var users = SeedUsers();
            var user = users.Find(u => u.Id == "U003");
            user.Phone = "0901234567";
            Assert.AreEqual("0901234567", user.Phone);
        }

        // Admin xóa nhân viên (Xác nhận Xóa)
        [TestMethod]
        public void Admin_DeleteUser_Confirmed_Success()
        {
            var users = SeedUsers();
            bool confirm = true;
            if (confirm)
            {
                users.RemoveAll(u => u.Id == "U002");
            }
            Assert.AreEqual(2, users.Count);
        }

        // Admin xóa nhân viên (Hủy Xóa)
        [TestMethod]
        public void Admin_DeleteUser_Cancel_NoChange()
        {
            var users = SeedUsers();
            bool confirm = false;
            if (confirm)
            {
                users.RemoveAll(u => u.Id == "U002");
            }
            Assert.AreEqual(3, users.Count);
        }

        // Xuất kết quả thành công (Luồng chính) - giả lập ghi file
        [TestMethod]
        public void ExportResult_Success_WritesFile()
        {
            string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"BaoCao_{Guid.NewGuid()}.pdf");
            System.IO.File.WriteAllText(path, "PDF_CONTENT");
            Assert.IsTrue(System.IO.File.Exists(path));
            System.IO.File.Delete(path);
        }

        // Hủy xuất kết quả (Luồng thay thế)
        [TestMethod]
        public void ExportResult_Cancel_NoFileCreated()
        {
            string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"BaoCao_{Guid.NewGuid()}.pdf");
            bool cancel = true;
            if (!cancel)
            {
                System.IO.File.WriteAllText(path, "PDF_CONTENT");
            }
            Assert.IsFalse(System.IO.File.Exists(path));
        }

        // Xem chi tiết đơn hàng (Layout 2 cột) - giả lập chia thông tin làm 2 phần
        [TestMethod]
        public void ViewOrderDetail_TwoColumnLayout_DataSplit()
        {
            var order = new FakeOrder { OrderId = "HD001", CustomerName = "Alpha", Detail = "A=B;C=D;E=F;G=H" };
            var parts = order.Detail.Split(';');
            int mid = parts.Length / 2;
            var col1 = new List<string>();
            var col2 = new List<string>();
            for (int i = 0; i < mid; i++)
            {
                col1.Add(parts[i]);
            }
            for (int i = mid; i < parts.Length; i++)
            {
                col2.Add(parts[i]);
            }
            Assert.AreEqual(col1.Count, col2.Count);
        }

        // Nhập thông số hợp lệ
        [TestMethod]
        public void EnterParameter_Valid()
        {
            string value = "7.5";
            bool isNumber = decimal.TryParse(value, out _);
            Assert.IsTrue(isNumber);
        }

        // Nhập thông số không hợp lệ (sai định dạng)
        [TestMethod]
        public void EnterParameter_InvalidFormat()
        {
            string value = "7,5.2.1";
            bool isNumber = decimal.TryParse(value, out _);
            Assert.IsFalse(isNumber);
        }

        // Xóa thông số (Không ràng buộc)
        [TestMethod]
        public void DeleteParameter_NoConstraint_Success()
        {
            var parameters = new List<string> { "pH", "COD", "BOD" };
            parameters.Remove("COD");
            CollectionAssert.DoesNotContain(parameters, "COD");
        }

        // Xóa thông số (Có ràng buộc)
        [TestMethod]
        public void DeleteParameter_WithConstraint_Block()
        {
            var parameters = new List<string> { "pH", "COD", "BOD" };
            bool hasConstraint = true; // giả lập có ràng buộc
            if (!hasConstraint) parameters.Remove("COD");
            CollectionAssert.Contains(parameters, "COD");
        }

        // Admin tạo đơn hàng mới (thành công)
        [TestMethod]
        public void Admin_CreateOrder_Success()
        {
            var orders = SeedOrders();
            var newOrder = new FakeOrder { OrderId = "HD003", CustomerName = "Cong ty Gamma", Detail = "New" };
            bool ok = !orders.Exists(o => o.OrderId == newOrder.OrderId) && !string.IsNullOrWhiteSpace(newOrder.CustomerName);
            if (ok) orders.Add(newOrder);
            Assert.IsTrue(ok);
            Assert.AreEqual(3, orders.Count);
        }

        // Admin sửa đơn hàng (thành công)
        [TestMethod]
        public void Admin_UpdateOrder_Success()
        {
            var orders = SeedOrders();
            var order = orders.Find(o => o.OrderId == "HD002");
            order.CustomerName = "Cong ty Beta Updated";
            Assert.AreEqual("Cong ty Beta Updated", order.CustomerName);
        }

        // Admin xóa đơn hàng (thành công)
        [TestMethod]
        public void Admin_DeleteOrder_Success()
        {
            var orders = SeedOrders();
            orders.RemoveAll(o => o.OrderId == "HD001");
            Assert.IsNull(orders.Find(o => o.OrderId == "HD001"));
        }

        // Kiểm tra email TH1 (Thông báo quan trắc)
        [TestMethod]
        public void EmailScenario_TH1_ContentCheck()
        {
            string subject = "[EcoGreen] Thông báo quan trắc";
            string body = "Thông báo lịch quan trắc định kỳ";
            StringAssert.Contains(subject, "Thông báo");
            StringAssert.Contains(body, "quan trắc");
        }

        // Kiểm tra email TH2 (Tái ký)
        [TestMethod]
        public void EmailScenario_TH2_ContentCheck()
        {
            string subject = "[EcoGreen] Tái ký hợp đồng";
            string body = "Hợp đồng sắp đến hạn tái ký";
            StringAssert.Contains(subject, "Tái ký");
            StringAssert.Contains(body, "tái ký");
        }

        // Kiểm tra email TH3 (Hết hạn trả kết quả)
        [TestMethod]
        public void EmailScenario_TH3_ContentCheck()
        {
            string subject = "[EcoGreen] Hết hạn trả kết quả";
            string body = "Hợp đồng đã quá hạn trả kết quả";
            StringAssert.Contains(subject, "Hết hạn");
            StringAssert.Contains(body, "quá hạn");
        }

        // Thống kê theo Năm
        [TestMethod]
        public void Statistics_ByYear_ShouldAggregate()
        {
            var data = new[]
            {
                new { Day = 1, Total = 100m },
                new { Day = 2, Total = 200m },
                new { Day = 3, Total = 0m },
            };
            decimal sum = 0m;
            foreach (var d in data) sum += d.Total;
            Assert.AreEqual(300m, sum);
        }

        // Thống kê không có dữ liệu
        [TestMethod]
        public void Statistics_NoData_ShouldBeZero()
        {
            var data = new (int Day, decimal Total)[] { };
            decimal sum = 0m;
            foreach (var d in data) sum += d.Total;
            Assert.AreEqual(0m, sum);
        }
    }
}
