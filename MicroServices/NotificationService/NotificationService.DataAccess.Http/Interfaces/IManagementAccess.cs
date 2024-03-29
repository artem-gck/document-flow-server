﻿using NotificationService.DataAccess.Http.Entity;

namespace NotificationService.DataAccess.Http.Interfaces
{
    public interface IManagementAccess
    {
        public Task<UserResponce> GetUserInfoAsync(Guid id);
    }
}
