using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FoodCalculation.Services;

namespace FoodCalculation.Views
{
    /// <summary>
    /// Displays all ingredients the user has confirmed across multiple dishes,
    /// aggregated so that duplicate ingredients are merged with their amounts summed.
    /// </summary>
    public partial class MyIngredientsPage : Page
    {
        private readonly DishManager _dishManager;
        private readonly IngredientAggregator _aggregator;
        private double _fontSize = 1;

        public MyIngredientsPage(DishManager dishManager)
        {
            InitializeComponent();
            _dishManager = dishManager;
            _aggregator = new IngredientAggregator(dishManager.AddedIngredients);
            this.SizeChanged += OnSizeChanged;
            DisplayIngredients();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateFontSizes();
        }

        private void UpdateFontSizes()
        {
            _fontSize = this.ActualWidth * 0.05;
            PageTitle.FontSize = _fontSize;
            BackButton.FontSize = _fontSize * 0.5;

            foreach (var child in IngredientsStackPanel.Children.OfType<TextBlock>())
            {
                child.FontSize = _fontSize * 0.4;
            }
        }

        /// <summary>
        /// Runs the aggregator to merge duplicate ingredients, then displays the result.
        /// Shows a placeholder message if no ingredients have been added yet.
        /// </summary>
        private void DisplayIngredients()
        {
            _aggregator.Ingredients = _dishManager.AddedIngredients;
            IngredientsStackPanel.Children.Clear();
            _aggregator.Refresh();

            if (_aggregator.IngredientNames.Count != 0)
            {
                for (int i = 0; i < _aggregator.IngredientNames.Count; i++)
                {
                    string name = _aggregator.IngredientNames[i];
                    string amount = _aggregator.IngredientAmounts[i];
                    IngredientsStackPanel.Children.Add(new TextBlock
                    {
                        Text = $"{name}: {amount}",
                        FontSize = 16,
                        Margin = new Thickness(5),
                        Foreground = Brushes.White
                    });
                }
            }
            else
            {
                IngredientsStackPanel.Children.Add(new TextBlock
                {
                    Text = "No ingredients added.",
                    FontSize = 16,
                    Margin = new Thickness(5),
                    Foreground = Brushes.White
                });
            }
        }

        /// <summary>
        /// Clears the frame to return to the main window.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var mainFrame = parentWindow.FindName("MainFrame") as Frame;
                if (mainFrame != null)
                {
                    mainFrame.Content = null;
                }
            }
        }
    }
}
