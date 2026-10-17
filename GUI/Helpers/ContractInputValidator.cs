using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace GUI.Helpers
{
    /// <summary>
    /// Tập hợp các hàm kiểm tra đầu vào dùng chung cho màn hình quản lý hợp đồng.
    /// Được tách riêng để dễ dàng viết unit test.
    /// </summary>
    public static class ContractInputValidator
    {
        /// <summary>
        /// Kiểm tra trường bắt buộc đã được nhập hay chưa.
        /// </summary>
        public static bool IsRequiredFieldFilled(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Kiểm tra định dạng email. Cho phép bỏ trống nếu allowEmpty = true.
        /// </summary>
        public static bool IsValidEmail(string email, bool allowEmpty = true)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return allowEmpty;
            }

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra số điện thoại hợp lệ: gồm 10 chữ số, bắt đầu bằng 0.
        /// </summary>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            string cleanPhone = phoneNumber
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty);

            return cleanPhone.Length == 10 &&
                   cleanPhone.StartsWith("0") &&
                   cleanPhone.All(char.IsDigit);
        }

        /// <summary>
        /// Kiểm tra ngày trả kết quả phải >= ngày ký hợp đồng.
        /// </summary>
        public static bool IsResultDateValid(DateTime signingDate, DateTime resultDate)
        {
            return resultDate.Date >= signingDate.Date;
        }

        /// <summary>
        /// Kiểm tra khách hàng có bị trùng mã hay không.
        /// </summary>
        public static bool IsCustomerIdUnique(string customerId, IEnumerable<string> existingCustomerIds)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return false;
            if (existingCustomerIds == null) return true;
            return !existingCustomerIds.Contains(customerId, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Kiểm tra xem dữ liệu hợp đồng có thay đổi so với ban đầu hay không.
        /// </summary>
        public static bool HasContractInfoChanged(SalesContractDTO original, SalesContractDTO updated)
        {
            if (original == null || updated == null) return false;

            return !string.Equals(original.CompanyName, updated.CompanyName, StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(original.Address, updated.Address, StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(original.RepresentativeName, updated.RepresentativeName, StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(original.RepresentativeEmail, updated.RepresentativeEmail, StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(original.PhoneNumber, updated.PhoneNumber, StringComparison.OrdinalIgnoreCase) ||
                   original.SigningDate.Date != updated.SigningDate.Date ||
                   original.ExpectedResultDate.Date != updated.ExpectedResultDate.Date;
        }
    }
}

