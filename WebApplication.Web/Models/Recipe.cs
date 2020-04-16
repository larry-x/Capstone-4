using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Ingredients = new List<string[]>();
        }

        public int UserId { get; set; } = 1;
        public List<string[]> Ingredients { get; set; }

        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Recipe name can't be more than 100 characters.")]
        public string Name { get; set; }
        [StringLength(100, ErrorMessage = "Recipe name can't be more than 100 characters.")]
        public string Description { get; set; }
        [StringLength(100, ErrorMessage = "Recipe name can't be more than 100 characters.")]
        public string Category { get; set; }
        [StringLength(2500, ErrorMessage = "Instructions for the recipe must be less than 2500 characters.")]
        public string Instructions { get; set; }

    }
}
