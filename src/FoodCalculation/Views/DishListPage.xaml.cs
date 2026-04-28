using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using FoodCalculation.Models;
using FoodCalculation.Services;

namespace FoodCalculation.Views
{
    /// <summary>
    /// Page that displays all dishes as clickable buttons.
    /// Supports adding new dishes, refreshing, and navigating to individual dish details.
    /// </summary>
    public partial class DishListPage : Page
    {
        private readonly DishManager _dishManager;
        private double _fontSize = 1;

        public DishListPage(DishManager dishManager)
        {
            InitializeComponent();
            _dishManager = dishManager;
            _dishManager.RefreshDishes(this);
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateFontSizes();
        }

        /// <summary>
        /// Scales all font sizes proportionally to the page width.
        /// </summary>
        private void UpdateFontSizes()
        {
            _fontSize = this.ActualWidth * 0.05;
            PageTitle.FontSize = _fontSize;
            BackButton.FontSize = _fontSize * 0.5;
            RefreshButton.FontSize = _fontSize * 0.5;
            AddButton.FontSize = _fontSize * 0.4;

            foreach (var child in DishStackPanel.Children.OfType<Button>())
            {
                child.FontSize = _fontSize * 0.5;
            }
        }

        /// <summary>
        /// Called externally (e.g. after adding a dish) to refresh the list from the manager.
        /// </summary>
        public void RefreshFromManager()
        {
            _dishManager.RefreshDishes(this);
        }

        /// <summary>
        /// Back button: clears the frame content to return to MainWindow.
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

        /// <summary>
        /// Filter button handler - placeholder for future filter implementation.
        /// </summary>
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Filter logic not yet implemented
        }

        /// <summary>
        /// Handles clicking on an individual dish button.
        /// Navigates to the IngredientPage for the selected dish.
        /// </summary>
        private void DishClicked(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            Window parentWindow = Window.GetWindow(this);
            var mainFrame = parentWindow?.FindName("MainFrame") as Frame;

            string dishName = clickedButton.Content.ToString()!;
            Dish? selectedDish = _dishManager.Dishes.FirstOrDefault(d => d.Name == dishName);

            if (selectedDish != null && mainFrame != null)
            {
                var ingredientPage = new IngredientPage(selectedDish, _dishManager);
                ingredientPage.PageTitle.Content = selectedDish.Name;
                mainFrame.Content = null;
                mainFrame.Navigate(ingredientPage);
                ingredientPage.Display(selectedDish);
            }
        }

        /// <summary>
        /// Navigates to the AddDishPage.
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Window.GetWindow(this);
            if (mainWindow != null)
            {
                var mainFrame = mainWindow.FindName("MainFrame") as Frame;
                if (mainFrame != null)
                {
                    mainFrame.Navigate(new AddDishPage(_dishManager));
                }
            }
        }

        /// <summary>
        /// Refreshes the dish list and re-applies font sizes.
        /// </summary>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _dishManager.RefreshDishes(this);
            UpdateFontSizes();
        }

        /// <summary>
        /// Dynamically creates a styled button for a dish and adds it to the list.
        /// Includes hover animations matching the app's visual style.
        /// </summary>
        /// <param name="name">The dish name to display on the button.</param>
        public void AddDish(string name)
        {
            Button btn = new Button
            {
                Content = name,
                FontSize = 25,
                Foreground = Brushes.White,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
            };

            // Build a custom rounded template for the button
            var template = new ControlTemplate(typeof(Button));
            var borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(25));
            borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
            borderFactory.SetValue(Border.PaddingProperty, new Thickness(10));

            var contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            borderFactory.AppendChild(contentPresenterFactory);
            template.VisualTree = borderFactory;

            // Mouse enter animation: slightly increase font size
            var enterTrigger = new EventTrigger { RoutedEvent = UIElement.MouseEnterEvent };
            var enterStoryboard = new Storyboard();
            var enterAnim = new DoubleAnimation
            {
                By = 2,
                Duration = TimeSpan.FromSeconds(0.2),
                FillBehavior = FillBehavior.HoldEnd
            };
            Storyboard.SetTarget(enterAnim, btn);
            Storyboard.SetTargetProperty(enterAnim, new PropertyPath(Button.FontSizeProperty));
            enterStoryboard.Children.Add(enterAnim);
            enterTrigger.Actions.Add(new BeginStoryboard { Storyboard = enterStoryboard });

            // Mouse leave animation: restore font size
            var leaveTrigger = new EventTrigger { RoutedEvent = UIElement.MouseLeaveEvent };
            var leaveStoryboard = new Storyboard();
            var leaveAnim = new DoubleAnimation
            {
                By = -2,
                Duration = TimeSpan.FromSeconds(0.2),
                FillBehavior = FillBehavior.Stop
            };
            Storyboard.SetTarget(leaveAnim, btn);
            Storyboard.SetTargetProperty(leaveAnim, new PropertyPath(Button.FontSizeProperty));
            leaveStoryboard.Children.Add(leaveAnim);
            leaveTrigger.Actions.Add(new BeginStoryboard { Storyboard = leaveStoryboard });

            btn.Triggers.Add(leaveTrigger);
            btn.Triggers.Add(enterTrigger);
            btn.Template = template;
            btn.Click += DishClicked;

            DishStackPanel.Children.Add(btn);
        }

        /// <summary>
        /// Removes all dish buttons from the list.
        /// </summary>
        public void ClearDishes()
        {
            DishStackPanel.Children.Clear();
        }
    }
}
