﻿using DocumentCrudService.Cqrs.Results;

namespace DocumentCrudService.Cqrs.Dto
{
    public class DocumentNameDto : IResult
    {
        public string Name { get; set; }
        public int Version { get; set; }
        public Guid CreatorId { get; set; }
        public Guid Id { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
