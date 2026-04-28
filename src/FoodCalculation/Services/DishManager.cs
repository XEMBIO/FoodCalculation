using FoodCalculation.Models;
using FoodCalculation.Views;

namespace FoodCalculation.Services
{
    /// <summary>
    /// Central manager that holds all dishes and the accumulated ingredient list.
    /// Provides methods to add dishes, refresh the dish list UI, and apply filters.
    /// </summary>
    public class DishManager
    {
        public List<Dish> Dishes { get; set; } = new List<Dish>();
        public List<Dish> FilteredDishes { get; set; } = new List<Dish>();

        /// <summary>
        /// Ingredients that the user has confirmed from the IngredientPage.
        /// These are accumulated across multiple dish selections and displayed
        /// on the MyIngredients page.
        /// </summary>
        public List<Ingredient> AddedIngredients { get; set; } = new List<Ingredient>();

        /// <summary>
        /// Initializes the manager with a set of default sample dishes.
        /// </summary>
        public DishManager()
        {
            // Default sample dishes so the app is not empty on first launch
            Dishes.Add(new Dish("Pasta", new List<Ingredient>
            {
                new Ingredient("Pasta", "100 g"),
                new Ingredient("Tomato Sauce", "50 ml"),
                new Ingredient("Cheese", "20 g")
            }, false, false));

            Dishes.Add(new Dish("Salad", new List<Ingredient>
            {
                new Ingredient("Lettuce", "50 g"),
                new Ingredient("Tomato", "30 g"),
                new Ingredient("Cucumber", "30 g"),
                new Ingredient("Dressing", "20 ml")
            }, false, false));

            Dishes.Add(new Dish("Sandwich", new List<Ingredient>
            {
                new Ingredient("Bread", "2 slices"),
                new Ingredient("Ham", "50 g"),
                new Ingredient("Cheese", "20 g"),
                new Ingredient("Lettuce", "10 g"),
                new Ingredient("Tomato", "10 g")
            }, false, false));
        }

        /// <summary>
        /// Refreshes the dish list on the given DishListPage by clearing
        /// and re-adding all dish buttons.
        /// </summary>
        /// <param name="dishListPage">The page whose UI should be updated.</param>
        public void RefreshDishes(DishListPage dishListPage)
        {
            dishListPage.ClearDishes();

            foreach (var dish in Dishes)
            {
                dishListPage.AddDish(dish.Name);
            }
        }

        /// <summary>
        /// Placeholder for future filter functionality.
        /// Intended to filter dishes by ingredients and dietary preferences.
        /// </summary>
        public void ApplyFilter(List<Ingredient> filteredIngredients, bool vegan, bool vegetarian)
        {
            FilteredDishes.Clear();
            foreach (var dish in Dishes)
            {
                // Filter logic not yet implemented
            }
        }

        /// <summary>
        /// Adds a new dish to the collection.
        /// </summary>
        public void AddDish(List<Ingredient> ingredients, string name, bool isVegan, bool isVegetarian)
        {
            Dishes.Add(new Dish(name, ingredients, isVegan, isVegetarian));
        }
    }
}
