using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using WebApplication.Web.Models;
using WebApplication.Web.DAL;
using WebApplication.Web.Controllers;
using System.Data.SqlClient;
using System.Linq;

namespace WebApplication.Tests.UnitTests
{
    [TestClass]
    public class RecipeSqlDALTest : ParentDALTest
    {
        [TestMethod]
        public void AddNewIngredient_Should_IncreaseCountBy1()
        {

            RecipeSqlDAL recipe = new RecipeSqlDAL(ConnectionString);
            IList<string> ingredients = recipe.GetIngredients();
            int startCount = ingredients.Count();

            recipe.AddNewIngredient("X_ingredient");
            IList<string> endIngredients = recipe.GetIngredients();

            int endCount = endIngredients.Count();

            Assert.AreEqual(startCount + 1, endCount);
        }


        [TestMethod]
        public void GetIngredientsTest()
        {
            RecipeSqlDAL dao = new RecipeSqlDAL(ConnectionString);
            IList<string> ingredients = dao.GetIngredients();
            Assert.AreEqual(GetRowCount("Ingredients"), ingredients.Count);
        }

        [TestMethod]
        public void AddNewRecipe_Should_IncreaseCountBy1()
        {
            RecipeSqlDAL dao = new RecipeSqlDAL(ConnectionString);
            Recipe dummy = new Recipe()
            {
                UserId = 1,
                Category = "blank",
                Description = "blank",
                Name = "testing",
                Instructions = "1 2 3"
            };

            int startCount = GetRowCount("Recipes");
            dao.AddNewRecipe(dummy);
            int endCount = GetRowCount("Recipes");

            Assert.AreEqual(startCount + 1, endCount);
        }

        [TestMethod]
        public void GetRecipeIngredientsTest()
        {
            RecipeSqlDAL dao = new RecipeSqlDAL(ConnectionString);
            IList<string[]> testObj = dao.GetRecipeIngredients(recipe_id);
            Assert.AreEqual(2, testObj.Count);
        }

        [DataTestMethod]
        [DataRow("Yummy meal!")]
        public void GetRecipe_Should_Return_Proper_Description(string expected)
        {
            RecipeSqlDAL dao = new RecipeSqlDAL(ConnectionString);
            string result = dao.GetRecipe(recipe_id).Description;
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("Custard", 1)]
        [DataRow("Flan", 2)]
        public void GetIngredientIdTest(string name, int id)
        {
            RecipeSqlDAL dao = new RecipeSqlDAL(ConnectionString);
            int result = dao.GetIngredientId(name);
            if (id == 1)
                Assert.AreEqual(ingredient_id1, result);
            else
                Assert.AreEqual(ingredient_id2, result);
        }

        [DataTestMethod]
        [DataRow(1)]
        public void GetAllRecipesTest(int id)
        {
            RecipeSqlDAL dao = new RecipeSqlDAL(ConnectionString);
            int expected = GetRowCount("Recipes");
            IList<Recipe> result = dao.GetAllRecipes(id);
            Assert.AreEqual(expected, result.Count);
        }

        [TestMethod]
        public void UpdateRecipe_Name_Should_Match()
        {
            RecipeSqlDAL dao = new RecipeSqlDAL(ConnectionString);
            Recipe dummy = new Recipe()
            {
                Id = recipe_id,
                UserId = 1,
                Category = "desert",
                Description = "Yummy meal!",
                Name = "dummy",
                Instructions = "1 2 3"
            };
            dao.UpdateRecipe(dummy);
            Recipe result = dao.GetRecipe(recipe_id);
            Assert.AreEqual("dummy", result.Name);
        }

    }
}

