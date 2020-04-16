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
    public class MealPlanController : Controller
    {
        private readonly IRecipeDAL recipeDAO;
        private readonly IAuthProvider authProvider;
        private readonly IMealPlanDAL mealDAO;
        private readonly IUserDAL userDAO;


        public MealPlanController(IMealPlanDAL mealDAO, IRecipeDAL recipeDAO, IAuthProvider authProvider, IUserDAL userDAO)
        {
            this.mealDAO = mealDAO;
            this.recipeDAO = recipeDAO;
            this.authProvider = authProvider;
            this.userDAO = userDAO;
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult Index()
        {
            IDictionary<int, string> plans = mealDAO.GetAllMealPlans(authProvider.GetCurrentUser().Id);

            return View(plans);
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult Detail(int planId, int day = 0)
        {
            if (!userDAO.AuthenticatePlan(authProvider.GetCurrentUser().Id, planId))
            {
                return RedirectToAction("WhoArtThou", "Home");
            }
            MealPlanViewModel model = GetSession();
            model.Id = planId;
            model.Day = day;
            model.PlanName = mealDAO.GetAllMealPlans(authProvider.GetCurrentUser().Id)[planId];
            model.Meals = mealDAO.GetMealPlan(planId);
            model.AvailableRecipes.Clear();
            model.RecipeDropdownList(recipeDAO.GetAllRecipes(authProvider.GetCurrentUser().Id));
            SaveSession(model);
            return View(model);
        }

        [AuthorizationFilter]
        [HttpGet]
        public IActionResult CreatePlan()
        {
            return View();
        }

        [AuthorizationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePlan(MealPlanViewModel model)
        {
            if (model.PlanName != null && model.PlanName.Length <= 50)
            {
                int plan = mealDAO.CreateMealPlan(authProvider.GetCurrentUser().Id, model.PlanName);
                return RedirectToAction("Detail", new { planId = plan });
            }
            else
            {
                TempData["P"] = "Please Enter a Plan Name";
                return View(model);
            }

        }

        [AuthorizationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRecipeToPlan(MealPlanViewModel model, int orderId)
        {
            int plan = GetSession().Id;
            int today = GetSession().Day;

            if (!string.IsNullOrEmpty(model.RecipeId))
            {
                mealDAO.AddToPlan(plan, model.RecipeId, orderId);
            }
            else
            {   
                TempData["R" + Convert.ToString((orderId - 1) % 3)] = "You have not selected a recipe";
            }
            return RedirectToAction("Detail", new { planId = plan, day = today });
        }

        [AuthorizationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRecipeFromPlan(int orderId, int recipeId)
        {
            int plan = GetSession().Id;
            int today = GetSession().Day;
            mealDAO.DeleteFromPlan(plan, recipeId, orderId);
            return RedirectToAction("Detail", new { planId = plan, day = today });
        }

        public IActionResult GroceryList(int planId)
        {
            if (!userDAO.AuthenticatePlan(authProvider.GetCurrentUser().Id, planId))
            {
                return RedirectToAction("WhoArtThou", "Home");
            }
            string planName = mealDAO.GetAllMealPlans(authProvider.GetCurrentUser().Id)[planId];
            IList<string> groceryList = mealDAO.GetGroceries(planId);

            Groceries groceries = new Groceries()
            {
                GroceryList = groceryList,
                PlanName = planName
            };

            return View(groceries);
        }

        private void SaveSession(MealPlanViewModel temp)
        {
            HttpContext.Session.Set("MealPlan", temp);
        }

        private MealPlanViewModel GetSession()
        {
            MealPlanViewModel temp = null;
            if (HttpContext.Session.Get<MealPlanViewModel>("MealPlan") == null)
            {
                temp = new MealPlanViewModel();
                SaveSession(temp);
            }
            else
            {
                temp = HttpContext.Session.Get<MealPlanViewModel>("MealPlan");
            }
            return temp;
        }

    }
}