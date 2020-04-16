using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class MealPlanViewModel
    {

        public MealPlanViewModel()
        {
            Meals = new List<Meal>();
            AvailableRecipes = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public int Day { get; set; }
        public string RecipeId { get; set; }
        public string PlanName { get; set; }
        public IList<Meal> Meals { get; set; }
        public IList<SelectListItem> AvailableRecipes { get; set; }

        public void RecipeDropdownList(IList<Recipe> allRecipes)
        {
            foreach (Recipe item in allRecipes)
            {
                SelectListItem entry = new SelectListItem { Text = item.Name, Value = item.Id.ToString() };
                AvailableRecipes.Add(entry);
            }
        }
        
        public string MealType(int i)
        {
            string result = "";
            switch (i)
            {
                case 0:
                    result = "Breakfast";
                    break;
                case 1:
                    result = "Lunch";
                    break;
                case 2:
                    result = "Dinner";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
    }
}
