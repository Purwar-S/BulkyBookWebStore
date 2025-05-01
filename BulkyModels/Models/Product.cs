using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string? Title { get; set; }
        
        [Required]
        public string? Author{ get; set; }
        
        [Required, NotNull]
        public string Description { get; set; }
        
        [Required]
        public string? ISBN { get; set; }
        
        [Required,Display(Name="List Price")]
        public double ListPrice { get; set; }
        
        [Required, Display(Name = "Price for 1-50")]
        public double Price { get; set; }
        
        [Required, Display(Name = "Price for 50+")]
        public double Price50 { get; set; }

        [Required, Display(Name = "Price for 100+")]
        public double Price100 { get; set; }
        [Display(Name ="Category Id")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Cat_Id { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}
