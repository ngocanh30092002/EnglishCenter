﻿using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.Models.DTO
{
    public class CourseDto
    {
        [Required]
        [StringLength(10, MinimumLength = 0)]
        public string CourseId { set; get; }

        [Required]
        [StringLength(50)]
        public string Name { set;get; }
        public string? Description { set; get; }
        public int? NumLesson { set; get; }
        public int? EntryPoint { set; get; }
        public int? StandardPoint { get; set; }
        public int? Priority { get; set; }
        public IFormFile? Image { set; get; }
        public string? ImageUrl { set; get; }
    }
}
