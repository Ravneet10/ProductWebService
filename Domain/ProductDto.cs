using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

    }
}
