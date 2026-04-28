using System.Globalization;

namespace FoodCalculation.Models
{
    /// <summary>
    /// Represents a single ingredient with a name and an amount string (e.g. "100 g").
    /// The amount is stored as a combined string of value and unit so that
    /// arbitrary units like "slices" are supported alongside metric ones.
    /// </summary>
    public class Ingredient
    {
        public string Name { get; set; }

        /// <summary>
        /// Amount string in the format "value unit", e.g. "100 g" or "2 slices".
        /// </summary>
        public string Amount { get; set; }

        public Ingredient(string name, string amount)
        {
            Name = name;
            Amount = amount;
        }

        /// <summary>
        /// Extracts only the numeric part from the amount string.
        /// For example, "100 g" returns "100".
        /// </summary>
        private string SplitAmountValue()
        {
            string[] parts = Amount.Split(' ');
            return parts[0];
        }

        /// <summary>
        /// Calculates the total amount for a given number of people.
        /// Multiplies the per-person value by the number of people.
        /// </summary>
        /// <param name="people">Number of people to calculate for.</param>
        /// <returns>The total amount as an integer.</returns>
        public int GetAmount(int people)
        {
            int perPerson = int.Parse(SplitAmountValue());
            return perPerson * people;
        }
    }
}
