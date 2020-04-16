using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IRecipeDAL recipeDAO;
        private readonly IAuthProvider authProvider;
        private readonly IUserDAL userDAO;

        public RecipeController(IRecipeDAL recipeDAO, IAuthProvider authProvider, IUserDAL userDAO)
        {
            this.recipeDAO = recipeDAO;
            this.authProvider = authProvider;
            this.userDAO = userDAO;
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult AddRecipe()
        {
            IList<string> ingredients = recipeDAO.GetIngredients();
            NewRecipeViewModel model = new NewRecipeViewModel();
            model.AddToDropdownList(ingredients);
            model.NewRecipe.UserId = authProvider.GetCurrentUser().Id;
            SaveSession(model);
            return View(model);
        }

        [AuthorizationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRecipe(NewRecipeViewModel model)
        {
            model.SelectedMeasurement = (model.SelectedMeasurement == null) ? "whole" :model.SelectedMeasurement;
            model.SelectedQuantity = Math.Round(model.SelectedQuantity, 2);

            if (model.SelectedIngredient == null)
            {
                TempData["I"] = "Please select at least one ingredient.";
                model = GetSession();
            } else if(model.SelectedQuantity <= 0 || model.SelectedQuantity >= 10000) {
               TempData["Q"] = "Please enter a valid quantity.";
                model = GetSession();
            } else if(model.SelectedMeasurement.Length > 50)
            {
                TempData["M"] = "Please enter a unit of measurement less than 50 characters.";
                model = GetSession();
            }
            else
            {
                string[] ingredientEntry = new string[3] { model.SelectedQuantity.ToString(), model.SelectedMeasurement, model.SelectedIngredient };
                model = GetSession();
                model.NewRecipe.Ingredients.Add(ingredientEntry);
                SaveSession(model);
            }

            return View(model);
        }

        [AuthorizationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRecipeFinal(NewRecipeViewModel model)
        {
            model.NewRecipe.Description = (model.NewRecipe.Description == null) ? " " : model.NewRecipe.Description;

            if (model.NewRecipe.Instructions == null || model.NewRecipe.Instructions.Length > 2500)
            {
                TempData["T"] = "Please provide preparation instructions (under 2500 characters).";
                model = GetSession();
            } else if(model.NewRecipe.Category == null || model.NewRecipe.Category.Length > 50)
            {
                TempData["C"] = "Please choose a category.";
                model = GetSession();
            } else if (model.NewRecipe.Description.Length > 2500)
            {
                TempData["D"] = "Your description is too long. Chill out.";
                model = GetSession();
            } else if(model.NewRecipe.Name == null || model.NewRecipe.Name.Length > 100)
            {
                TempData["N"] = "Please enter a name for this recipe.";
                model = GetSession();
            } else if(GetSession().NewRecipe.Ingredients.Count == 0)
            {
                TempData["E"] = "This recipe has no ingredients!!";
                model = GetSession();
            }
            else
            {
                List<string[]> temp = GetSession().NewRecipe.Ingredients;
                int user_id = GetSession().NewRecipe.UserId;
                model.NewRecipe.Ingredients = temp;
                model.NewRecipe.UserId = user_id;
                recipeDAO.AddNewRecipe(model.NewRecipe);
                return RedirectToAction("RecipeLibrary");
            }

            return View("AddRecipe", model);
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult NewIngredient()
        {
            return View();
        }

        [AuthorizationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewIngredient(NewRecipeViewModel model)
        {
            if (model.NewIngredient == null || model.NewIngredient.Length > 50)
            {
                TempData["Error"] = "Please enter a new ingredient (no more than 50 characters).";
                return View();
            }

            else
            {
                string newIngredient = model.NewIngredient;
                model = GetSession();
                model.AvailableIngredients.Clear();
                recipeDAO.AddNewIngredient(newIngredient);
                model.Selection = newIngredient;
                model.AddToDropdownList(recipeDAO.GetIngredients());
                model.Selection = null;
                SaveSession(model);

                return View("AddRecipe", model);
            }
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult RecipeLibrary()
        {
            IList<Recipe> recipes = recipeDAO.GetAllRecipes(authProvider.GetCurrentUser().Id);
            return View(recipes);
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult RecipeDetail(int id)
        {
            if (!userDAO.AuthenticateRecipe(authProvider.GetCurrentUser().Id, id))
            {
                return RedirectToAction("WhoArtThou", "Home");
            }
            Recipe recipe = recipeDAO.GetRecipe(id);
            return View(recipe);
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult ModifyRecipe(int id)
        {
            if( !userDAO.AuthenticateRecipe(authProvider.GetCurrentUser().Id, id))
            {
                return RedirectToAction("WhoArtThou", "Home");
            }
            IList<string> ingredients = recipeDAO.GetIngredients();            
            NewRecipeViewModel model = new NewRecipeViewModel();
            model.AddToDropdownList(ingredients);
            Recipe recipe = recipeDAO.GetRecipe(id);
            model.NewRecipe = recipe;
            SaveSession(model);
            return View(model);
        }

        [AuthorizationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModifyRecipe(NewRecipeViewModel model)
        {
            model.NewRecipe.Description = (model.NewRecipe.Description == null) ? " " : model.NewRecipe.Description;

            if (model.NewRecipe.Instructions == null || model.NewRecipe.Instructions.Length > 2500)
            {
                TempData["T"] = "Please provide preparation instructions (under 2500 characters).";
                model = GetSession();
            }
            else if (model.NewRecipe.Category == null || model.NewRecipe.Category.Length > 50)
            {
                TempData["C"] = "Please choose a category.";
                model = GetSession();
            }
            else if (model.NewRecipe.Description.Length > 2500)
            {
                TempData["D"] = "Your description is too long. Chill out.";
                model = GetSession();
            }
            else if (model.NewRecipe.Name == null || model.NewRecipe.Name.Length > 100)
            {
                TempData["N"] = "Please enter a name for this recipe.";
                model = GetSession();
            }
            else if (GetSession().NewRecipe.Ingredients.Count == 0)
            {
                TempData["E"] = "This recipe has no ingredients!!";
                model = GetSession();
            }
            else
            {
                List<string[]> temp = GetSession().NewRecipe.Ingredients;
                int temp_id = GetSession().NewRecipe.Id;
                int user_id = GetSession().NewRecipe.UserId;
                model.NewRecipe.Ingredients = temp;
                model.NewRecipe.Id = temp_id;
                model.NewRecipe.UserId = user_id;
                recipeDAO.UpdateRecipe(model.NewRecipe);
                return RedirectToAction("RecipeDetail", new { id = temp_id } );
            }
            
            return View(model);
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult DeleteIngredient(string name)
        {
            NewRecipeViewModel model = GetSession();
            string[] deleteThis = model.NewRecipe.Ingredients.Find(arr => arr[2].Contains(name));
            model.NewRecipe.Ingredients.Remove(deleteThis);
            SaveSession(model);
            return View("ModifyRecipe", model);
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult AddIngredient()
        {
            NewRecipeViewModel model = GetSession();
            return View(model);
        }

        [AuthorizationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddIngredient(NewRecipeViewModel model)
        {
            model.SelectedMeasurement = (model.SelectedMeasurement == null) ? "whole" : model.SelectedMeasurement;
            model.SelectedQuantity = Math.Round(model.SelectedQuantity, 2);

            if (model.SelectedIngredient == null)
            {
                TempData["I"] = "Please select at least one ingredient.";
                model = GetSession();
            }
            else if (model.SelectedQuantity <= 0 || model.SelectedQuantity >= 10000)
            {
                TempData["Q"] = "Please enter a valid quantity.";
                model = GetSession();
            }
            else if (model.SelectedMeasurement.Length > 50)
            {
                TempData["M"] = "Please enter a unit of measurement less than 50 characters.";
                model = GetSession();
            }
            else
            {
                string[] ingredientEntry = new string[3] { model.SelectedQuantity.ToString(), model.SelectedMeasurement, model.SelectedIngredient };
                model = GetSession();
                model.NewRecipe.Ingredients.Add(ingredientEntry);
                SaveSession(model);
                return View("ModifyRecipe", model);
            }

            return View(model);
        }

        [AuthorizationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewIngredient_M(NewRecipeViewModel model)
        {
            if (model.NewIngredient == null || model.NewIngredient.Length > 50)
            {
                model = GetSession();
                TempData["Error"] = "Please enter a new ingredient.";
            }
            else
            {
                string newIngredient = model.NewIngredient;
                model = GetSession();
                model.AvailableIngredients.Clear();
                recipeDAO.AddNewIngredient(newIngredient);
                model.Selection = newIngredient;
                model.AddToDropdownList(recipeDAO.GetIngredients());
                model.Selection = null;
                SaveSession(model);
            }
            return View("AddIngredient", model);
        }

        private void SaveSession(NewRecipeViewModel temp)
        {
            HttpContext.Session.Set("NewRecipe", temp);
        }

        private NewRecipeViewModel GetSession()
        {
            NewRecipeViewModel temp = null;
            if (HttpContext.Session.Get<NewRecipeViewModel>("NewRecipe") == null)
            {
                temp = new NewRecipeViewModel();
                SaveSession(temp);
            }
            else
            {
                temp = HttpContext.Session.Get<NewRecipeViewModel>("NewRecipe");
            }
            return temp;
        }



    }
}