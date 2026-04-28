using System.Windows;
using FoodCalculation.Services;

namespace FoodCalculation.Views
{
    /// <summary>
    /// Main window of the application. Provides navigation to the dish list
    /// and the aggregated ingredients overview via a shared Frame.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Single DishManager instance shared across all pages
        public DishManager DishManager { get; } = new DishManager();

        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;
        }

        /// <summary>
        /// Dynamically scales font sizes based on window width for responsive layout.
        /// </summary>
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateFontSizes();
        }

        private void UpdateFontSizes()
        {
            double fontSize = this.ActualWidth * 0.06;
            ZutatenButton.FontSize = fontSize;
            GerichteButton.FontSize = fontSize;
            PageTitle.FontSize = fontSize;
        }

        /// <summary>
        /// Navigates to the dish list page.
        /// </summary>
        private void GerichteButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DishListPage(DishManager));
        }

        /// <summary>
        /// Navigates to the aggregated ingredients overview page.
        /// </summary>
        private void ZutatenButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MyIngredientsPage(DishManager));
        }
    }
}
