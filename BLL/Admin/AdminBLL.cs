using DAL;
using DAL.Admin;
using DAL.User;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.Admin
{
    public class AdminBLL
    {
        private static AdminBLL _instance;
        public static AdminBLL Instance => _instance ?? (_instance = new AdminBLL());

        private readonly AdminDAL _adminDAL;
        //private readonly UserDAL _userDAL;

        private AdminBLL()
        {
            _adminDAL = new AdminDAL();
            // _userDAL = new UserDAL();
        }

        #region User Management
        public List<UserDTO> GetAllUsers()
        {
            return _adminDAL.GetAllUsers();
        }

        public UserDTO GetUserDetails(string userId)
        {
            return _adminDAL.GetUserDetailsById(userId);
        }

        public bool AddNewUser(UserDTO user, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (_adminDAL.CheckUserIdExists(user.Company_UserId))
            {
                errorMessage = "Mã nhân viên (hoặc Tên đăng nhập) này đã tồn tại.";
                return false;
            }
            if (!string.IsNullOrEmpty(user.Email) && _adminDAL.CheckEmailExists(user.Email))
            {
                errorMessage = "Email này đã được sử dụng cho tài khoản khác.";
                return false;
            }
            if (!string.IsNullOrEmpty(user.Phone) && _adminDAL.CheckPhoneExists(user.Phone))
            {
                errorMessage = "Số điện thoại này đã được sử dụng cho tài khoản khác.";
                return false;
            }
            user.Company_Username = user.Company_UserId;
            user.PasswordHash = "D17F25ECFBCC7857F7BEBEA469308BE0B2580943E96D13A3AD98A13675C4BFC2";

            try
            {
                return _adminDAL.AddNewUser(user);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    if (ex.Message.ToLower().Contains("primary key") || ex.Message.ToLower().Contains("company_userid"))
                        errorMessage = "Mã nhân viên đã tồn tại.";
                    else if (ex.Message.ToLower().Contains("email"))
                        errorMessage = "Email đã tồn tại.";
                    else if (ex.Message.ToLower().Contains("phone"))
                        errorMessage = "Số điện thoại đã tồn tại.";
                    else
                        errorMessage = "Dữ liệu nhập vào bị trùng lặp với một nhân viên khác.";
                }
                else
                {
                    errorMessage = "Lỗi cơ sở dữ liệu: " + ex.Message;
                }
                return false;
            }
        }

        public bool UpdateUser(UserDTO user, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(user.Email) && _adminDAL.CheckEmailExists(user.Email, user.Company_UserId))
            {
                errorMessage = "Email này đã được sử dụng cho tài khoản khác.";
                return false;
            }
            if (!string.IsNullOrEmpty(user.Phone) && _adminDAL.CheckPhoneExists(user.Phone, user.Company_UserId))
            {
                errorMessage = "Số điện thoại này đã được sử dụng cho tài khoản khác.";
                return false;
            }
            try
            {
                return _adminDAL.UpdateUser(user);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    errorMessage = "Dữ liệu cập nhật bị trùng lặp (Email hoặc SĐT) với một nhân viên khác.";
                }
                else
                {
                    errorMessage = "Lỗi cơ sở dữ liệu khi cập nhật: " + ex.Message;
                }
                return false;
            }
        }

        public bool DeleteUser(string userId, out string errorMessage)
        {
            errorMessage = string.Empty;
            var userToDelete = this.GetUserDetails(userId); 
            if (userToDelete == null)
            {
                errorMessage = "Không tìm thấy người dùng.";
                return false;
            }

            if (userToDelete.RoleId == "R001") // Role Admin
            {
                var allActiveUsers = _adminDAL.GetAllUsers();
                if (allActiveUsers.Count(u => u.RoleId == "R001" && u.IsActive) <= 1)
                {
                    errorMessage = "Không thể xóa Admin cuối cùng của hệ thống.";
                    return false;
                }
            }

            try
            {
                return _adminDAL.DeleteUser(userId);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    errorMessage = "Không thể xóa nhân viên này vì có dữ liệu liên quan (hợp đồng, kết quả,...).";
                }
                else
                {
                    errorMessage = "Lỗi cơ sở dữ liệu: " + ex.Message;
                }
                return false;
            }
        }

        public DataTable GetAllRoles()
        {
            return _adminDAL.GetAllRoles();
        }

        public DataTable GetAllDepartments()
        {
            return _adminDAL.GetAllDepartments();
        }

        public string GetNextCustomerId()
        {
            return _adminDAL.GetNextCustomerId();
        }
        #endregion

        #region Customer Management
        public List<CustomerDTO> GetAllCustomers()
        {
            return _adminDAL.GetAllCustomers();
        }

        public CustomerDTO GetCustomerDetails(string customerId)
        {
            return _adminDAL.GetCustomerDetailsById(customerId);
        }

        public bool AddNewCustomer(CustomerDTO customer, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (_adminDAL.CheckCustomerIdExists(customer.CustomerId))
            {
                errorMessage = "Mã khách hàng này đã tồn tại.";
                return false;
            }
            return _adminDAL.AddNewCustomer(customer);
        }

        public bool UpdateCustomer(CustomerDTO customer, out string errorMessage)
        {
            errorMessage = string.Empty;
            return _adminDAL.UpdateCustomer(customer);
        }

        public bool DeleteCustomer(string customerId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                return _adminDAL.DeleteCustomer(customerId);
            }
            catch (Exception ex)
            {
                errorMessage = "Không thể xóa khách hàng vì đang có dữ liệu hợp đồng liên quan. Lỗi: " + ex.Message;
                return false;
            }
        }
        #endregion

        #region Order Management
        public List<OrderDTO> GetAllOrders()
        {
            return _adminDAL.GetAllOrders();
        }

        public OrderDTO GetOrderDetails(string contractId)
        {
            var order = _adminDAL.GetOrderDetails(contractId);
            if (order != null)
            {
                switch (order.Status)
                {
                    case "InProgress":
                        order.Status = "Đang thực hiện";
                        break;
                    case "Done":
                        order.Status = "Đã hoàn thành";
                        break;
                    case "Cancelled":
                        order.Status = "Đã hủy";
                        break;
                    case "Planned":
                        order.Status = "Đã lên kế hoạch";
                        break;
                    default:
                        order.Status = "Không xác định";
                        break;
                }
            }
            return order;
        }

        private string MapStatusToDb(string uiStatus)
        {
            if (string.IsNullOrWhiteSpace(uiStatus)) return "InProgress";
            uiStatus = uiStatus.Trim();
            switch (uiStatus)
            {
                case "Đang thực hiện": return "InProgress";
                case "Đã hoàn thành": return "Done";
                case "Đã hủy": return "Cancelled";
                case "Đã lên kế hoạch": return "Planned";
            }
            // Cho phép nhập trực tiếp InProgress/Done/... cũng hợp lệ
            if (new[] { "InProgress", "Done", "Cancelled", "Planned" }.Contains(uiStatus)) return uiStatus;
            return "InProgress";
        }

        private string MapStatusToUi(string dbStatus)
        {
            switch (dbStatus)
            {
                case "InProgress": return "Đang thực hiện";
                case "Done": return "Đã hoàn thành";
                case "Cancelled": return "Đã hủy";
                case "Planned": return "Đã lên kế hoạch";
            }
            return "Không xác định";
        }

        public bool AddNewOrder(OrderDTO order, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (_adminDAL.CheckContractIdExists(order.OrderId))
            {
                errorMessage = "Mã hợp đồng đã tồn tại.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(order.CustomerId))
            {
                errorMessage = "Mã khách hàng không được trống.";
                return false;
            }
            order.Status = MapStatusToDb(order.Status);
            try
            {
                return _adminDAL.AddNewOrder(order);
            }
            catch (SqlException ex)
            {
                errorMessage = "Lỗi cơ sở dữ liệu khi thêm hợp đồng: " + ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi thêm hợp đồng: " + ex.Message;
                return false;
            }
        }

        public bool UpdateOrder(OrderDTO order, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(order.OrderId))
            {
                errorMessage = "Thiếu mã hợp đồng.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(order.CustomerId))
            {
                errorMessage = "Mã khách hàng không được trống.";
                return false;
            }
            order.Status = MapStatusToDb(order.Status);
            try
            {
                return _adminDAL.UpdateOrder(order);
            }
            catch (SqlException ex)
            {
                errorMessage = "Lỗi cơ sở dữ liệu khi cập nhật hợp đồng: " + ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi cập nhật hợp đồng: " + ex.Message;
                return false;
            }
        }

        public bool DeleteOrder(string contractId, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(contractId))
            {
                errorMessage = "Thiếu mã hợp đồng.";
                return false;
            }
            try
            {
                return _adminDAL.DeleteOrder(contractId);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    errorMessage = "Không thể xóa vì có dữ liệu liên quan (nền mẫu, lô đo, thông báo...).";
                }
                else
                {
                    errorMessage = "Lỗi cơ sở dữ liệu khi xóa hợp đồng: " + ex.Message;
                }
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi xóa hợp đồng: " + ex.Message;
                return false;
            }
        }

        public string GetNextContractId()
        {
            return _adminDAL.GetNextContractId();
        }
        #endregion
    }
}