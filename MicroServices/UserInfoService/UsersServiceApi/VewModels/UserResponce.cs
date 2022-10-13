﻿using System.ComponentModel.DataAnnotations;

namespace UsersServiceApi.VewModels
{
    public class UserResponce
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? PositionId { get; set; }
    }
}
