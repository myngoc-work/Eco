using System;
using System.Collections.Generic;
using DAL.User;
using DTO;

namespace BLL.User
{
    public class NotificationBLL
    {
        private NotificationDAL _notificationDAL;

        public NotificationBLL()
        {
            _notificationDAL = new NotificationDAL();
        }

        public List<NotificationDTO> GetOverdueContractNotifications()
        {
            return _notificationDAL.GetOverdueContractNotifications();
        }

        public List<NotificationDTO> SearchOverdueContractNotifications(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return GetOverdueContractNotifications();
            }

            return _notificationDAL.SearchOverdueContractNotifications(searchText.Trim());
        }
    }
}
