﻿namespace CompanyManagementServiceApi.ViewModels
{
    public class UserResponce
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
    }
}