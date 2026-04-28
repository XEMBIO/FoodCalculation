using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FoodCalculation.Models;
using FoodCalculation.Services;

namespace FoodCalculation.Views
{
    /// <summary>
    /// Displays ingredients for a selected dish and allows the user to calculate
    /// scaled amounts based on the number of people. The calculated ingredients
    /// can then be confirmed and added to the global shopping list.
    /// </summary>
    public partial class IngredientPage : Page
    {
        private readonly Dish _currentDish;
        private readonly DishManager _dishManager;
        private readonly List<Ingredient> _scaledIngredients = new List<Ingredient>();
        private double _fontSize = 20;

        public IngredientPage(Dish currentDish, DishManager dishManager)
        {
            InitializeComponent();
            _currentDish = currentDish;
            _dishManager = dishManager;
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _fontSize = this.ActualWidth * 0.05;
            PageTitle.FontSize = _fontSize;
            BackButton.FontSize = _fontSize * 0.5;
            AmountPeopleTextBox.FontSize = _fontSize * 0.4;

            foreach (var item in IngredientsPanel.Children.OfType<TextBlock>())
            {
                item.FontSize = _fontSize * 0.35;
            }
            foreach (var item in IngredientAmountPanel.Children.OfType<TextBlock>())
            {
                item.FontSize = _fontSize * 0.35;
            }
        }

        /// <summary>
        /// Navigates back to the dish list page.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService?.CanGoBack == true)
            {
                this.NavigationService.GoBack();
                return;
            }

            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var mainFrame = parentWindow.FindName("MainFrame") as Frame;
                if (mainFrame != null)
                {
                    var dishListPage = mainFrame.Content as DishListPage;
                    if (dishListPage != null)
                    {
                        mainFrame.Navigate(dishListPage);
                        return;
                    }
                    mainFrame.Navigate(new DishListPage(_dishManager));
                }
            }
        }

        /// <summary>
        /// Displays the base ingredients (per person) in the left panel.
        /// </summary>
        /// <param name="dish">The dish whose ingredients to display.</param>
        public void Display(Dish dish)
        {
            IngredientsPanel.Children.Clear();
            foreach (var ingredient in dish.Ingredients)
            {
                string text = $"{ingredient.Name}: {ingredient.Amount}";
                IngredientsPanel.Children.Add(new TextBlock
                {
                    Text = text,
                    FontSize = _fontSize * 0.35,
                    Foreground = Brushes.White,
                    Margin = new Thickness(5)
                });
            }
        }

        /// <summary>
        /// Calculates scaled ingredient amounts based on the number of people entered.
        /// Handles unit conversion: g -> kg and ml -> l when amounts reach 1000+,
        /// and the reverse when amounts drop below 1.
        /// </summary>
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            IngredientAmountPanel.Children.Clear();

            double amountPeople = 1;
            if (!string.IsNullOrEmpty(AmountPeopleTextBox.Text))
            {
                amountPeople = int.Parse(AmountPeopleTextBox.Text);
            }

            foreach (var ingredient in _currentDish.Ingredients)
            {
                string[] splitAmount = ingredient.Amount.Split(' ');
                double baseAmount = double.Parse(splitAmount[0]);
                double scaledAmount = baseAmount * amountPeople;

                // Apply unit conversion if needed
                double formattedAmount = FormatAmount(scaledAmount, splitAmount[1]);
                string unit = splitAmount[1];
                if (formattedAmount != scaledAmount)
                {
                    unit = ConvertUnit(unit);
                }

                _scaledIngredients.Add(new Ingredient(ingredient.Name, formattedAmount + " " + unit));

                IngredientAmountPanel.Children.Add(new TextBlock
                {
                    Text = formattedAmount + " " + unit,
                    FontSize = _fontSize * 0.35,
                    Foreground = Brushes.White,
                    Margin = new Thickness(5)
                });
            }
        }

        /// <summary>
        /// Converts a unit to its counterpart: g <-> kg, ml <-> l.
        /// </summary>
        private string ConvertUnit(string unit)
        {
            return unit switch
            {
                "g" => "kg",
                "kg" => "g",
                "ml" => "l",
                "l" => "ml",
                _ => unit
            };
        }

        /// <summary>
        /// Formats an amount by converting between base and large units.
        /// g/ml: if >= 1000, divide by 1000 (convert up).
        /// kg/l: if less than 1, multiply by 1000 (convert down).
        /// </summary>
        private double FormatAmount(double amount, string unit)
        {
            if (unit == "g" || unit == "ml")
            {
                return amount >= 1000 ? amount / 1000 : amount;
            }
            else if (unit == "kg" || unit == "l")
            {
                return amount < 1 ? amount * 1000 : amount;
            }
            return amount;
        }

        /// <summary>
        /// Only allows numeric input in the people count text box.
        /// </summary>
        private void NumberTextBox_PreviewInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        /// <summary>
        /// Adds the calculated (scaled) ingredients to the global shopping list
        /// and navigates back.
        /// </summary>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            _dishManager.AddedIngredients.AddRange(_scaledIngredients);
            BackButton_Click(sender, e);
        }
    }
}
