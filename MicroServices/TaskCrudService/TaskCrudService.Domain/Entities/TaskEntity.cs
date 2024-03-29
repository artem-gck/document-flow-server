﻿namespace TaskCrudService.Domain.Entities
{
    public class TaskEntity : BaseEntity
    {
        public TypeEntity? Type { get; set; }
        public Guid? TypeId { get; set; }
        public string Header { get; set; }
        public string? Status { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<PerformerEntity>? Performers { get; set; }
        public List<DocumentEntity>? Documents { get; set; }
    }
}
