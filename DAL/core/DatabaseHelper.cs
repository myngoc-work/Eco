using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            // Đồng bộ connection string với DataProvider để tránh truy cập sai DB giữa các module
            var preferred = ConfigurationManager.ConnectionStrings["db_QLQTMT_Connection"];
            var fallback = ConfigurationManager.ConnectionStrings["DefaultConnection"];

            if (preferred != null)
            {
                connectionString = preferred.ConnectionString;
            }
            else if (fallback != null)
            {
                connectionString = fallback.ConnectionString;
            }
            else
            {
                throw new ConfigurationErrorsException("Không tìm thấy connection string db_QLQTMT_Connection hoặc DefaultConnection.");
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

    }
}
