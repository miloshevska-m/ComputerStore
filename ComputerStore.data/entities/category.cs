﻿using System.ComponentModel.DataAnnotations;

namespace ComputerStore.data.entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
