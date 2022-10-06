﻿using System.ComponentModel.DataAnnotations;

namespace StructureServiceApi.ViewModels.UpdateRequest
{
    public class UpdatePositionRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,20}[a-zA-Z]$")]
        public string Name { get; set; }
    }
}
