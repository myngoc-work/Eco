using BLL.Utils;
using DTO;
using System;
using System.Linq;

namespace GUI.Helpers
{
    public static class PermissionHelper
    {
        // Kiểm tra xem user hiện tại có phải Admin không
        public static bool IsAdmin()
        {
            var user = UserSession.Instance.CurrentUser;
            if (user == null) return false;
            // Kiểm tra cả RoleName và DepartmentName
            return user.RoleName == "Admin" || user.DepartmentName == "Quản lý";
        }

        // Kiểm tra xem user có thuộc phòng ban cụ thể không
        public static bool IsDepartment(string departmentName)
        {
            var user = UserSession.Instance.CurrentUser;
            if (user == null) return false;
            // So sánh không phân biệt hoa thường và loại bỏ khoảng trắng thừa
            return user.DepartmentName != null && 
                   user.DepartmentName.Trim().Equals(departmentName.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        // Kiểm tra xem user có thuộc một trong các phòng ban không
        public static bool IsAnyDepartment(params string[] departmentNames)
        {
            var user = UserSession.Instance.CurrentUser;
            if (user == null || user.DepartmentName == null) return false;
            string userDept = user.DepartmentName.Trim();
            return departmentNames.Any(dept => dept.Trim().Equals(userDept, StringComparison.OrdinalIgnoreCase));
        }

        // Kiểm tra quyền truy cập tab "Thông tin chung" trong quản lý đơn hàng
        public static bool CanAccessGeneralInfoTab()
        {
            return IsAdmin() || IsDepartment("Phòng Kinh Doanh");
        }

        // Kiểm tra quyền truy cập tab "Nền mẫu và thông số" trong quản lý đơn hàng
        public static bool CanAccessParametersTab()
        {
            return IsAdmin() || IsAnyDepartment("Phòng Kế Hoạch", "Phòng Hiện Trường", "Phòng Thí Nghiệm", "Phòng Kết Quả");
        }

        // Kiểm tra quyền truy cập tab "Kết quả" trong quản lý đơn hàng
        public static bool CanAccessResultsTab()
        {
            return IsAdmin();
        }

        // Kiểm tra quyền chỉnh sửa "Phân công" trong tab Nền mẫu và thông số
        public static bool CanEditAssignment()
        {
            return IsAdmin() || IsDepartment("Phòng Kế Hoạch");
        }

        // Kiểm tra quyền chỉnh sửa "Giới hạn" trong tab Nền mẫu và thông số
        public static bool CanEditLimitRange()
        {
            return IsAdmin() || IsDepartment("Phòng Kế Hoạch");
        }

        // Kiểm tra quyền chỉnh sửa kết quả đo đạt (phòng hiện trường/thí nghiệm)
        public static bool CanEditResults()
        {
            return IsAdmin() || IsAnyDepartment("Phòng Hiện Trường", "Phòng Thí Nghiệm");
        }

        // Kiểm tra quyền chỉnh sửa thông tin ở ô của phòng hiện trường/thí nghiệm
        public static bool CanEditFieldLabInfo()
        {
            return IsAdmin();
        }

        // Kiểm tra quyền xem (read-only) tab Nền mẫu và thông số
        public static bool CanViewParametersTab()
        {
            return IsAdmin() || IsAnyDepartment("Phòng Kế Hoạch", "Phòng Hiện Trường", "Phòng Thí Nghiệm", "Phòng Kết Quả");
        }

        // Kiểm tra quyền thêm, sửa, xóa trong tab chức năng quản lý (đơn hàng, khách hàng)
        public static bool CanManageData()
        {
            return IsAdmin() || IsDepartment("Phòng Kinh Doanh");
        }

        // Kiểm tra quyền thêm, sửa, xóa trong quản lý nhân viên (chỉ Admin)
        public static bool CanManageEmployees()
        {
            return IsAdmin();
        }

        // Lấy tên phòng ban hiện tại
        public static string GetCurrentDepartment()
        {
            var user = UserSession.Instance.CurrentUser;
            return user?.DepartmentName ?? "";
        }
    }
}

