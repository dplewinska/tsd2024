using Microsoft.AspNetCore.Mvc;
using Yummy.Models;
using System.Text.Encodings.Web;

namespace Yummy.Controllers
{
    public class RecipesController : Controller
    {
        public static List<Recipe> _recipes = new List<Recipe>
        {
            new Recipe
            {
                ID = 1,
                Name = "Blueberry Pancakes",
                Time = "30 minutes",
                Difficulty = "Easy",
                NumberOfLikes = 0,
                Ingredients = "Flour, milk, eggs, baking powder, salt, sugar, blueberries",
                Process = "In a bowl, mix flour, baking powder, salt, sugar. In another bowl, whisk milk and eggs. Combine wet and dry ingredients, fold in blueberries. Cook pancakes on a hot griddle until golden.",
                TipsAndTricks = "Keep the batter slightly lumpy for fluffy pancakes."
            },
            new Recipe
            {
                ID = 2,
                Name = "Avocado Toast with Poached Egg",
                Time = "15 minutes",
                Difficulty = "Medium",
                NumberOfLikes = 0,
                Ingredients = "Bread, avocado, lemon juice, salt, pepper, eggs, vinegar",
                Process = "Toast the bread. Mash avocado with lemon juice, salt, and pepper. Spread on toast. Poach eggs in simmering water with vinegar for 3 minutes. Place egg on toast.",
                TipsAndTricks = "Use sourdough bread for extra flavor."
            }
        };



        public IActionResult Index()
        {
            return View(_recipes);
        }

        public IActionResult Details(int id)
        {
            var recipe = _recipes.FirstOrDefault(r => r.ID == id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                recipe.ID = _recipes.Max(r => r.ID) + 1; // Simple ID assignment for our in-memory list
                _recipes.Add(recipe);
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        public IActionResult Edit(int id)
        {
            var recipe = _recipes.FirstOrDefault(r => r.ID == id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Recipe recipe)
        {
            if (id != recipe.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var recipeIndex = _recipes.FindIndex(r => r.ID == id);
                if (recipeIndex != -1)
                {
                    _recipes[recipeIndex] = recipe; // Update the recipe
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(recipe);
        }

        public IActionResult Delete(int id)
        {
            var recipe = _recipes.FirstOrDefault(r => r.ID == id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var recipe = _recipes.FirstOrDefault(r => r.ID == id);
            if (recipe != null)
            {
                _recipes.Remove(recipe);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}