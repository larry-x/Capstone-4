using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IRecipeDAL
    {
        IList<string> GetIngredients();

        void AddNewIngredient(string ingredient);

        void AddNewRecipe(Recipe recipe);

        IList<Recipe> GetAllRecipes(int userId);

        Recipe GetRecipe(int id);

        int GetIngredientId(string name);

        void UpdateRecipe(Recipe recipe);

    }
}
