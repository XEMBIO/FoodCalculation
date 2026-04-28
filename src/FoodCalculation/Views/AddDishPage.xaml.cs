using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FoodCalculation.Models;
using FoodCalculation.Services;

namespace FoodCalculation.Views
{
    /// <summary>
    /// Page for creating a new dish with a name, dietary flags, and a list of ingredients.
    /// Ingredient input format: "Name Amount Unit" (e.g. "Pasta 100 g").
    /// </summary>
    public partial class AddDishPage : Page
    {
        private readonly DishManager _dishManager;
        private readonly List<Ingredient> _ingredients = new List<Ingredient>();
        private double _fontSize = 0;

        public AddDishPage(DishManager dishManager)
        {
            InitializeComponent();
            _dishManager = dishManager;
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _fontSize = this.ActualWidth * 0.05;
            PageTitle.FontSize = _fontSize;
            BackButton.FontSize = _fontSize * 0.5;
            AddIngredientButton.FontSize = _fontSize * 0.4;
            DishNameTextBox.FontSize = _fontSize * 0.5;
            ConfirmButton.FontSize = _fontSize * 0.5;

            foreach (var textBlock in IngredientListPanel.Children.OfType<TextBlock>())
            {
                textBlock.FontSize = _fontSize * 0.4;
            }
        }

        /// <summary>
        /// Navigates back to the DishListPage.
        /// If navigation history exists, goes back; otherwise creates a new DishListPage.
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
                        dishListPage.RefreshFromManager();
                        return;
                    }
                    mainFrame.Navigate(new DishListPage(_dishManager));
                }
            }
        }

        /// <summary>
        /// Saves the new dish to the DishManager and navigates back.
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = DishNameTextBox?.Text?.Trim() ?? string.Empty;
            bool isVegan = VeganCheckBox.IsChecked == true;
            bool isVegetarian = VegetarianCheckBox.IsChecked == true;

            if (_ingredients.Count > 0)
            {
                _dishManager.AddDish(_ingredients, name, isVegan, isVegetarian);
            }

            // Refresh the dish list if it exists in the navigation stack
            Window parentWindow = Window.GetWindow(this);
            var mainFrame = parentWindow?.FindName("MainFrame") as Frame;
            var dishListPage = mainFrame?.Content as DishListPage;
            dishListPage?.RefreshFromManager();

            BackButton_Click(sender, e);
        }

        /// <summary>
        /// Parses the ingredient input text and adds the ingredient.
        /// Expected format: "Name Amount Unit" (exactly 3 parts).
        /// </summary>
        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            string[]? parts = IngredientNameTextBox?.Text?.Trim().Split(' ');

            if (parts != null && parts.Length == 3)
            {
                string name = parts[0];
                string amount = parts[1] + " " + parts[2];
                _ingredients.Add(new Ingredient(name, amount));
                IngredientNameTextBox!.Text = "";
            }

            RefreshIngredientList();
        }

        /// <summary>
        /// Re-renders the ingredient list in the UI panel.
        /// </summary>
        private void RefreshIngredientList()
        {
            if (_ingredients.Count > 0)
            {
                IngredientListPanel.Children.Clear();
                foreach (var ingredient in _ingredients)
                {
                    string text = $"{ingredient.Name}: {ingredient.Amount} / Person";
                    IngredientListPanel.Children.Add(new TextBlock
                    {
                        Text = text,
                        Foreground = Brushes.White,
                        FontSize = _fontSize * 0.4
                    });
                }
            }
        }

        /// <summary>
        /// Ensures vegan and vegetarian are mutually exclusive.
        /// </summary>
        private void VegetarianCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (VegetarianCheckBox.IsChecked == true)
            {
                VeganCheckBox.IsChecked = false;
            }
        }

        private void VeganCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (VeganCheckBox.IsChecked == true)
            {
                VegetarianCheckBox.IsChecked = false;
            }
        }
    }
}
