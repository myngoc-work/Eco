
using DTO; 

namespace BLL.Utils
{
    public sealed class UserSession
    {
        private static readonly UserSession instance = new UserSession();

        private UserSession() { }

        public static UserSession Instance
        {
            get { return instance; }
        }

        public UserDTO CurrentUser { get; private set; }

        public void Login(UserDTO user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        public bool IsAdmin()
        {
            return CurrentUser != null && CurrentUser.RoleName == "Admin";
        }

        public bool IsHeadOfDepartment()
        {
            return CurrentUser != null && CurrentUser.RoleName == "HeadOfDepartment";
        }

        //public bool HasRole(string roleName)
        //{
        //    return CurrentUser != null && CurrentUser.RoleName == roleName;
        //}
    }
}