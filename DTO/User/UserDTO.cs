using System;

namespace DTO
{
    public class UserDTO
    {
        public string Company_UserId { get; set; }
        public string Company_Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string AvatarPath { get; set; }
        public bool IsActive { get; set; }
        public string RoleId { get; set; }
        public string DepartmentId { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public DateTime? PhoneVerifiedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string TwoFactorSecret { get; set; }
        public bool IsTwoFactorEnabled { get; set; }

        public string RoleName { get; set; }
        public string DepartmentName { get; set; }
    }

    public class CustomerDTO
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string RepName { get; set; }
        public string Address { get; set; }
        public string RepEmail { get; set; }
        public string RepPhone { get; set; }

        public DateTime? LastSignDate { get; set; }
        public string Frequency { get; set; }
        public string Industry { get; set; }
        public string TaxCode { get; set; }
        public string InteractionHistory { get; set; }
    }

    public class VerificationTokenDTO
    {
        public long TokenId { get; set; }
        public string Company_UserId { get; set; }
        public string Type { get; set; }
        public string TokenValue { get; set; }
        public string Destination { get; set; } // Email or Phone number
        public DateTime ExpiresAt { get; set; }
        public DateTime? ConsumedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}