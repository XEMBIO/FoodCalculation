namespace FoodCalculation.Models
{
    /// <summary>
    /// Represents a dish (recipe) consisting of a name, a list of ingredients,
    /// and dietary flags for vegan / vegetarian classification.
    /// </summary>
    public class Dish
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public bool IsVegan { get; set; }
        public bool IsVegetarian { get; set; }

        /// <param name="name">Display name of the dish.</param>
        /// <param name="ingredients">List of ingredients with per-person amounts.</param>
        /// <param name="isVegan">Whether the dish is vegan.</param>
        /// <param name="isVegetarian">Whether the dish is vegetarian.</param>
        public Dish(string name, List<Ingredient> ingredients, bool isVegan, bool isVegetarian)
        {
            Name = name;
            Ingredients = ingredients;
            IsVegan = isVegan;
            IsVegetarian = isVegetarian;
        }
    }
}
