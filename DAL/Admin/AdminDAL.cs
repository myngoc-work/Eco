using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Admin
{
    public class AdminDAL
    {
        #region User Management

        public UserDTO GetUserDetailsById(string userId)
        {
            UserDTO user = null;
            string query = "SELECT * FROM Company_User WHERE Company_UserId = @Company_UserId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Company_UserId", userId)
            };
            DataTable data = DataProvider.ExecuteQuery(query, parameters);
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                user = new UserDTO
                {
                    Company_UserId = row["Company_UserId"].ToString(),
                    Company_Username = row["Company_Username"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Email = row["Email"].ToString(),
                    Phone = row["Phone"] != DBNull.Value ? row["Phone"].ToString() : null,
                    DateOfBirth = row["DateOfBirth"] != DBNull.Value ? (DateTime?)row["DateOfBirth"] : null,
                    Gender = row["Gender"] != DBNull.Value ? row["Gender"].ToString() : null,
                    Address = row["Address"] != DBNull.Value ? row["Address"].ToString() : null,
                    IsActive = (bool)row["IsActive"],
                    RoleId = row["RoleId"].ToString(),
                    DepartmentId = row["DepartmentId"].ToString()
                };
            }
            return user;
        }

        public List<UserDTO> GetAllUsers()
        {
            var userList = new List<UserDTO>();
            string query = @"SELECT u.Company_UserId, u.FullName, u.IsActive, r.RoleName, d.DepartmentName, r.RoleId
                             FROM Company_User u
                             LEFT JOIN [Role] r ON u.RoleId = r.RoleId
                             LEFT JOIN Department d ON u.DepartmentId = d.DepartmentId
                             WHERE u.IsActive = 1
                             ORDER BY u.FullName";
            DataTable data = DataProvider.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                userList.Add(new UserDTO
                {
                    Company_UserId = row["Company_UserId"].ToString(),
                    FullName = row["FullName"].ToString(),
                    IsActive = (bool)row["IsActive"],
                    RoleId = row["RoleId"].ToString(),
                    RoleName = row["RoleName"] != DBNull.Value ? row["RoleName"].ToString() : "N/A",
                    DepartmentName = row["DepartmentName"] != DBNull.Value ? row["DepartmentName"].ToString() : "N/A"
                });
            }
            return userList;
        }

        public bool AddNewUser(UserDTO user)
        {
            string query = @"INSERT INTO Company_User 
                               (Company_UserId, Company_Username, PasswordHash, FullName, Email, Phone, DateOfBirth, Gender, Address, IsActive, RoleId, DepartmentId, IsTwoFactorEnabled) 
                           VALUES 
                               (@Company_UserId, @Company_Username, @PasswordHash, @FullName, @Email, @Phone, @DateOfBirth, @Gender, @Address, @IsActive, @RoleId, @DepartmentId, @IsTwoFactorEnabled)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Company_UserId", user.Company_UserId),
                new SqlParameter("@Company_Username", user.Company_Username),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Phone", (object)user.Phone ?? DBNull.Value),
                new SqlParameter("@DateOfBirth", (object)user.DateOfBirth ?? DBNull.Value),
                new SqlParameter("@Gender", (object)user.Gender ?? DBNull.Value),
                new SqlParameter("@Address", (object)user.Address ?? DBNull.Value),
                new SqlParameter("@IsActive", user.IsActive),
                new SqlParameter("@RoleId", user.RoleId),
                new SqlParameter("@DepartmentId", user.DepartmentId),
                new SqlParameter("@IsTwoFactorEnabled", false)
            };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateUser(UserDTO user)
        {
            string query = @"UPDATE Company_User 
                           SET FullName = @FullName, Email = @Email, Phone = @Phone, DateOfBirth = @DateOfBirth, 
                               Gender = @Gender, Address = @Address, RoleId = @RoleId, DepartmentId = @DepartmentId
                           WHERE Company_UserId = @Company_UserId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Phone", (object)user.Phone ?? DBNull.Value),
                new SqlParameter("@DateOfBirth", (object)user.DateOfBirth ?? DBNull.Value),
                new SqlParameter("@Gender", (object)user.Gender ?? DBNull.Value),
                new SqlParameter("@Address", (object)user.Address ?? DBNull.Value),
                new SqlParameter("@RoleId", user.RoleId),
                new SqlParameter("@DepartmentId", user.DepartmentId),
                new SqlParameter("@Company_UserId", user.Company_UserId)
            };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteUser(string userId)
        {
            string query = "DELETE FROM Company_User WHERE Company_UserId = @Company_UserId";
            var parameters = new SqlParameter[] { new SqlParameter("@Company_UserId", userId) };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CheckUserIdExists(string userId)
        {
            string query = "SELECT COUNT(1) FROM Company_User WHERE Company_UserId = @Company_UserId OR Company_Username = @Company_UserId";
            var parameters = new SqlParameter[] { new SqlParameter("@Company_UserId", userId) };
            DataTable result = DataProvider.ExecuteQuery(query, parameters);
            return (int)result.Rows[0][0] > 0;
        }

        public bool CheckEmailExists(string email, string currentUserId = null)
        {
            string query = "SELECT COUNT(1) FROM Company_User WHERE Email = @Email";
            var parameters = new List<SqlParameter> { new SqlParameter("@Email", email) };
            if (!string.IsNullOrEmpty(currentUserId))
            {
                query += " AND Company_UserId <> @Company_UserId";
                parameters.Add(new SqlParameter("@Company_UserId", currentUserId));
            }
            DataTable result = DataProvider.ExecuteQuery(query, parameters.ToArray());
            return (int)result.Rows[0][0] > 0;
        }

        public bool CheckPhoneExists(string phone, string currentUserId = null)
        {
            string query = "SELECT COUNT(1) FROM Company_User WHERE Phone = @Phone";
            var parameters = new List<SqlParameter> { new SqlParameter("@Phone", phone) };
            if (!string.IsNullOrEmpty(currentUserId))
            {
                query += " AND Company_UserId <> @Company_UserId";
                parameters.Add(new SqlParameter("@Company_UserId", currentUserId));
            }
            DataTable result = DataProvider.ExecuteQuery(query, parameters.ToArray());
            return (int)result.Rows[0][0] > 0;
        }

        public DataTable GetAllRoles()
        {
            string query = "SELECT RoleId, RoleName FROM Role ORDER BY RoleName";
            return DataProvider.ExecuteQuery(query);
        }

        public DataTable GetAllDepartments()
        {
            string query = "SELECT DepartmentId, DepartmentName FROM Department ORDER BY DepartmentName";
            return DataProvider.ExecuteQuery(query);
        }
        #endregion

        #region Customer Management
        public List<CustomerDTO> GetAllCustomers()
        {
            var customerList = new List<CustomerDTO>();
            string query = "SELECT CustomerId, CustomerName, RepName FROM Customer ORDER BY CustomerName";
            DataTable data = DataProvider.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                customerList.Add(new CustomerDTO
                {
                    CustomerId = row["CustomerId"].ToString(),
                    CustomerName = row["CustomerName"].ToString(),
                    RepName = row["RepName"].ToString()
                });
            }
            return customerList;
        }

        public CustomerDTO GetCustomerDetailsById(string customerId)
        {
            CustomerDTO customer = null;
            string query = "SELECT * FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerId", customerId)
            };
            DataTable data = DataProvider.ExecuteQuery(query, parameters);
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                customer = new CustomerDTO
                {
                    CustomerId = row["CustomerId"].ToString(),
                    CustomerName = row["CustomerName"].ToString(),
                    RepName = row["RepName"].ToString(),
                    Address = row["Address"].ToString(),
                    RepEmail = row["RepEmail"].ToString(),
                    RepPhone = row["RepPhone"].ToString()
                };
            }
            return customer;
        }

        public bool AddNewCustomer(CustomerDTO customer)
        {
            string query = @"INSERT INTO Customer (CustomerId, CustomerName, RepName, Address, RepEmail, RepPhone) 
                               VALUES (@CustomerId, @CustomerName, @RepName, @Address, @RepEmail, @RepPhone)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerId", customer.CustomerId),
                new SqlParameter("@CustomerName", customer.CustomerName),
                new SqlParameter("@RepName", (object)customer.RepName ?? DBNull.Value),
                new SqlParameter("@Address", (object)customer.Address ?? DBNull.Value),
                new SqlParameter("@RepEmail", (object)customer.RepEmail ?? DBNull.Value),
                new SqlParameter("@RepPhone", (object)customer.RepPhone ?? DBNull.Value)
            };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateCustomer(CustomerDTO customer)
        {
            string query = @"UPDATE Customer 
                               SET CustomerName = @CustomerName, RepName = @RepName, Address = @Address, 
                                   RepEmail = @RepEmail, RepPhone = @RepPhone
                               WHERE CustomerId = @CustomerId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerName", customer.CustomerName),
                new SqlParameter("@RepName", (object)customer.RepName ?? DBNull.Value),
                new SqlParameter("@Address", (object)customer.Address ?? DBNull.Value),
                new SqlParameter("@RepEmail", (object)customer.RepEmail ?? DBNull.Value),
                new SqlParameter("@RepPhone", (object)customer.RepPhone ?? DBNull.Value),
                new SqlParameter("@CustomerId", customer.CustomerId)
            };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteCustomer(string customerId)
        {
            string query = "DELETE FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerId", customerId)
            };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CheckCustomerIdExists(string customerId)
        {
            string query = "SELECT COUNT(1) FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new SqlParameter[] { new SqlParameter("@CustomerId", customerId) };
            DataTable result = DataProvider.ExecuteQuery(query, parameters);
            return (int)result.Rows[0][0] > 0;
        }

        public string GetNextCustomerId()
        {
            string query = @"SELECT ISNULL(MAX(CAST(SUBSTRING(CustomerId, 3, 10) AS INT)), 0) FROM Customer WHERE CustomerId LIKE 'KH%'";
            DataTable dt = DataProvider.ExecuteQuery(query);
            int next = 1;
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0][0].ToString(), out next);
                next += 1;
            }
            return $"KH{next:000}";
        }
        #endregion

        #region Order Management
        public List<OrderDTO> GetAllOrders()
        {
            var orders = new List<OrderDTO>();
            string query = @"
                SELECT 
                    c.ContractId,
                    c.CustomerId,
                    cu.CustomerName,
                    cu.RepName
                FROM Contract c
                LEFT JOIN Customer cu ON cu.CustomerId = c.CustomerId
                ORDER BY c.SignDate DESC;";
            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                var order = new OrderDTO
                {
                    OrderId = row["ContractId"].ToString(),
                    CustomerId = row["CustomerId"].ToString(),
                    CustomerName = row["CustomerName"]?.ToString(),
                    CustomerContactName = row["RepName"]?.ToString()
                };
                orders.Add(order);
            }
            return orders;
        }

        public OrderDTO GetOrderDetails(string contractId)
        {
            OrderDTO order = null;
            string query = @"
                SELECT 
                    c.ContractId, 
                    c.CustomerId, 
                    c.PlanDueDate, 
                    c.Status, 
                    c.SalesCompany_UserId AS PersonInChargeId,
                    u.FullName AS PersonInChargeName,
                    cu.CustomerName,
                    cu.RepName,
                    (SELECT MIN(mb.StartDate) 
                     FROM ContractItem ci 
                     JOIN MeasurementBatch mb ON ci.ItemId = mb.ItemId 
                     WHERE ci.ContractId = c.ContractId) AS SampleStartDate
                FROM 
                    Contract c
                LEFT JOIN 
                    Company_User u ON c.SalesCompany_UserId = u.Company_UserId
                LEFT JOIN
                    Customer cu ON cu.CustomerId = c.CustomerId
                WHERE 
                    c.ContractId = @ContractId;";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ContractId", contractId)
            };
            DataTable data = DataProvider.ExecuteQuery(query, parameters);

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                order = new OrderDTO
                {
                    OrderId = row["ContractId"].ToString(),
                    CustomerId = row["CustomerId"].ToString(),
                    ExpectedReturnDate = row["PlanDueDate"] != DBNull.Value ? Convert.ToDateTime(row["PlanDueDate"]) : DateTime.MinValue,
                    Status = row["Status"].ToString(),
                    // Lưu mã nhân viên để ghi ngược vào SalesCompany_UserId, tránh lỗi cắt chuỗi
                    PersonInCharge = row["PersonInChargeId"] != DBNull.Value ? row["PersonInChargeId"].ToString() : null,
                    CustomerName = row["CustomerName"]?.ToString(),
                    CustomerContactName = row["RepName"]?.ToString()
                };

                if (row["SampleStartDate"] != DBNull.Value)
                {
                    order.SampleStartDate = Convert.ToDateTime(row["SampleStartDate"]);
                }
                else
                {
                    order.SampleStartDate = DateTime.MinValue;
                }
            }
            return order;
        }

        public string GetNextContractId()
        {
            string query = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ContractId, 3, 10) AS INT)), 0) FROM Contract WHERE ContractId LIKE 'HD%'";
            DataTable dt = DataProvider.ExecuteQuery(query);
            int next = 1;
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0][0].ToString(), out next);
                next += 1;
            }
            return $"HD{next:000}";
        }

        public bool CheckContractIdExists(string contractId)
        {
            string query = "SELECT COUNT(1) FROM Contract WHERE ContractId = @ContractId";
            var parameters = new SqlParameter[] { new SqlParameter("@ContractId", contractId) };
            DataTable result = DataProvider.ExecuteQuery(query, parameters);
            return (int)result.Rows[0][0] > 0;
        }

        public bool AddNewOrder(OrderDTO order)
        {
            string query = @"INSERT INTO Contract (ContractId, CustomerId, SignDate, PlanDueDate, Amount, Status, SalesCompany_UserId)
                             VALUES (@ContractId, @CustomerId, @SignDate, @PlanDueDate, @Amount, @Status, @SalesCompany_UserId)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ContractId", order.OrderId),
                new SqlParameter("@CustomerId", order.CustomerId),
                new SqlParameter("@SignDate", DateTime.Now),
                new SqlParameter("@PlanDueDate", (object)(order.ExpectedReturnDate == DateTime.MinValue ? (DateTime?)null : order.ExpectedReturnDate) ?? DBNull.Value),
                new SqlParameter("@Amount", DBNull.Value),
                new SqlParameter("@Status", order.Status),
                new SqlParameter("@SalesCompany_UserId", (object)order.PersonInCharge ?? DBNull.Value)
            };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateOrder(OrderDTO order)
        {
            string query = @"UPDATE Contract
                             SET CustomerId = @CustomerId,
                                 Status = @Status,
                                 SalesCompany_UserId = @SalesCompany_UserId
                             WHERE ContractId = @ContractId";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerId", order.CustomerId),
                new SqlParameter("@Status", order.Status),
                new SqlParameter("@SalesCompany_UserId", (object)order.PersonInCharge ?? DBNull.Value),
                new SqlParameter("@ContractId", order.OrderId)
            };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteOrder(string contractId)
        {
            string query = "DELETE FROM Contract WHERE ContractId = @ContractId";
            var parameters = new SqlParameter[] { new SqlParameter("@ContractId", contractId) };
            return DataProvider.ExecuteNonQuery(query, parameters) > 0;
        }
        #endregion
    }
}