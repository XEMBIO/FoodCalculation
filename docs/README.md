# FoodCalculation

A WPF desktop application for scaling recipe ingredient quantities when cooking for groups.
Instead of manually calculating proportions, this tool automates the math to expand recipes for any number of people.

## Technology Stack

- **Framework:** .NET 8.0 (Windows)
- **UI:** WPF (Windows Presentation Foundation)
- **Language:** C#

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Windows 10/11

### Build and Run

```bash
dotnet build FoodCalculation.sln
dotnet run --project src/FoodCalculation/FoodCalculation.csproj
```

Or open `FoodCalculation.sln` in Visual Studio and press F5.

## Project Structure

```
FoodCalculation/
|-- FoodCalculation.sln          # Visual Studio solution file
|-- .gitignore
|-- docs/                        # Documentation
|   |-- README.md                # This file
|   |-- Architecture.md          # Technical architecture overview
|   +-- UserGuide.md             # End-user guide
+-- src/
    +-- FoodCalculation/
        |-- FoodCalculation.csproj
        |-- App.xaml / App.xaml.cs
        |-- Models/              # Data models
        |   |-- Dish.cs          # Recipe with name, ingredients, dietary flags
        |   +-- Ingredient.cs    # Single ingredient with name and amount
        |-- Services/            # Business logic
        |   |-- DishManager.cs   # Manages dish collection and persistence
        |   +-- IngredientAggregator.cs  # Merges duplicate ingredients
        +-- Views/               # UI pages
            |-- MainWindow.xaml  # Main navigation hub
            |-- DishListPage.xaml        # Lists all dishes
            |-- AddDishPage.xaml         # Form to create a new dish
            |-- IngredientPage.xaml      # Dish detail with scaling
            +-- MyIngredientsPage.xaml   # Aggregated shopping list
```

## Features

- Browse a list of dishes (recipes)
- Add new dishes with custom ingredients and dietary flags (vegan/vegetarian)
- Select a dish and calculate ingredient amounts for any number of people
- Automatic unit conversion (g -> kg, ml -> l) when amounts exceed 1000
- Aggregated "My Ingredients" view that merges duplicates across multiple dishes
- Responsive UI that scales with window size
- Smooth hover animations on all buttons

## License

This project is for personal / educational use.
