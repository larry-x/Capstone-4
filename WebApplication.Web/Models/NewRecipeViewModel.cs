using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class NewRecipeViewModel
    {
        public NewRecipeViewModel()
        {
            AvailableIngredients = new List<SelectListItem>();
            NewRecipe = new Recipe();
        }

        public IList<SelectListItem> AvailableIngredients { get; set; }
        public string Selection { get; set; }

        public void AddToDropdownList(IList<string> ingredientList)
        {
            foreach (string item in ingredientList)
            {
                SelectListItem entry = new SelectListItem { Text = item, Value = item };
                if (item == Selection)
                    entry.Selected = true;
                AvailableIngredients.Add(entry);
            }
        }

      
        [StringLength(50, ErrorMessage = "Ingredient name can't be more than 50 characters")]
        public string NewIngredient { get; set; }

        public Recipe NewRecipe { get; set; }

        public string SelectedIngredient { get; set; }
      
        [Range(0, 9999, ErrorMessage = "Quantity must be a numerical value")]
        public double SelectedQuantity { get; set; }

        
        [StringLength(50, ErrorMessage = "Form of measurement must be less than 50 characters.")]
        public string SelectedMeasurement { get; set; }
        
    }
}
