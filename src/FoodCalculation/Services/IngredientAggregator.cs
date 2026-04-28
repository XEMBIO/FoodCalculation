using FoodCalculation.Models;

namespace FoodCalculation.Services
{
    /// <summary>
    /// Aggregates duplicate ingredients by combining their amounts.
    /// Handles unit conversions between g/kg and ml/l when totals exceed 1000.
    /// This replaces the old "RepeatManager" with a clearer name.
    /// </summary>
    public class IngredientAggregator
    {
        public List<string> IngredientNames { get; private set; } = new List<string>();
        public List<string> IngredientAmounts { get; private set; } = new List<string>();
        public List<Ingredient> Ingredients { get; set; }

        public IngredientAggregator(List<Ingredient> ingredients)
        {
            Ingredients = ingredients;
        }

        /// <summary>
        /// Processes all ingredients: merges duplicates by summing their amounts,
        /// and converts units upward (g -> kg, ml -> l) when totals reach 1000+.
        /// </summary>
        public void Refresh()
        {
            IngredientAmounts.Clear();
            IngredientNames.Clear();

            foreach (var ingredient in Ingredients)
            {
                if (IngredientNames.Contains(ingredient.Name))
                {
                    // Ingredient already exists - merge amounts
                    int index = IngredientNames.IndexOf(ingredient.Name);
                    string[] existingParts = IngredientAmounts[index].Split(' ');
                    string[] newParts = ingredient.Amount.Split(' ');

                    // Normalize both to base units (g, ml) before adding
                    double existingValue = ConvertToBaseUnit(existingParts[0], existingParts[1]);
                    double newValue = ConvertToBaseUnit(newParts[0], newParts[1]);

                    // Ensure both units are in base form for calculation
                    existingParts[1] = NormalizeToBaseUnit(existingParts[1]);
                    newParts[1] = NormalizeToBaseUnit(newParts[1]);

                    double totalAmount = existingValue + newValue;

                    // Convert up to larger unit if total >= 1000
                    if (totalAmount >= 1000)
                    {
                        totalAmount = totalAmount / 1000;
                        existingParts[1] = GetLargerUnit(existingParts[1]);
                    }

                    IngredientAmounts[index] = $"{totalAmount} {existingParts[1]}";
                }
                else
                {
                    // New ingredient - add directly
                    IngredientNames.Add(ingredient.Name);
                    IngredientAmounts.Add(ingredient.Amount);
                }
            }
        }

        /// <summary>
        /// Returns the next larger unit: g -> kg, ml -> l.
        /// </summary>
        private string GetLargerUnit(string unit)
        {
            return unit.ToLower() switch
            {
                "g" => "kg",
                "ml" => "l",
                _ => unit
            };
        }

        /// <summary>
        /// Converts a value to its base unit (grams or milliliters).
        /// kg and l are multiplied by 1000 to convert down.
        /// </summary>
        private double ConvertToBaseUnit(string amount, string unit)
        {
            double value = Convert.ToDouble(amount);

            return unit.ToLower() switch
            {
                "kg" or "l" => value * 1000,
                _ => value
            };
        }

        /// <summary>
        /// Normalizes a unit to its base form: kg -> g, l -> ml.
        /// Other units are returned unchanged.
        /// </summary>
        private string NormalizeToBaseUnit(string unit)
        {
            return unit.ToLower() switch
            {
                "kg" => "g",
                "l" => "ml",
                _ => unit
            };
        }
    }
}
