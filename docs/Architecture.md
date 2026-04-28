# Architecture Overview

## Application Design

FoodCalculation follows a straightforward WPF page-navigation pattern.
The `MainWindow` hosts a `Frame` control that loads different `Page` instances for each view.
A single `DishManager` instance is created in `MainWindow` and passed to all pages, serving as
the shared data store for the application session.

## Layers

### Models (`Models/`)

Simple data classes with no dependencies on WPF:

- **Dish** - Holds a name, list of `Ingredient` objects, and boolean dietary flags (`IsVegan`, `IsVegetarian`).
- **Ingredient** - Holds a name and an amount string in "value unit" format (e.g. "100 g", "2 slices"). Includes a helper method `GetAmount(int people)` to calculate scaled totals.

### Services (`Services/`)

Business logic that operates on the models:

- **DishManager** - Central data store. Holds the list of all dishes, a filtered subset (placeholder), and the accumulated `AddedIngredients` list (the user's shopping list). Initializes with three sample dishes (Pasta, Salad, Sandwich). Provides `RefreshDishes()` to update the UI, `AddDish()` to create new dishes, and `ApplyFilter()` as a placeholder for future filtering.

- **IngredientAggregator** (formerly `RepeatManager`) - Takes a list of ingredients and merges duplicates by name, summing their amounts. Handles metric unit conversions:
  - Normalizes kg -> g and l -> ml before adding
  - Converts back to kg/l when the total reaches 1000+

### Views (`Views/`)

WPF pages with code-behind. Each page receives the `DishManager` via constructor injection.

- **MainWindow** - Entry point with two navigation buttons: "Gerichte" (Dishes) and "Zutaten" (Ingredients). Contains the shared `Frame` named `MainFrame`.
- **DishListPage** - Displays all dishes as clickable buttons. Supports refresh, add, filter (placeholder), and back navigation.
- **AddDishPage** - Form to create a new dish. Input fields: dish name, ingredient (format: "Name Amount Unit"), vegan/vegetarian checkboxes. Ingredients are added one at a time and displayed in a scrollable list.
- **IngredientPage** - Shows a selected dish's ingredients per person (left panel). The user enters a number of people, and the right panel shows the scaled amounts with automatic unit conversion.
- **MyIngredientsPage** - Displays the aggregated ingredient list from all confirmed dishes, with duplicates merged via `IngredientAggregator`.

## Navigation Flow

```
MainWindow
  |-- [Gerichte] --> DishListPage
  |                   |-- [dish click] --> IngredientPage --> [Bestaetigen] --> back to DishListPage
  |                   |-- [Hinzufuegen] --> AddDishPage --> [Bestaetigen] --> back to DishListPage
  |                   +-- [Zurueck] --> MainWindow
  +-- [Zutaten] --> MyIngredientsPage
                    +-- [Zurueck] --> MainWindow
```

## Responsive Design

All pages implement a `SizeChanged` handler that scales font sizes proportionally to the window width (typically 5% of `ActualWidth` for titles, with smaller factors for buttons and content). This ensures the UI remains readable at any window size.

## Styling

- Consistent blue gradient background (`#006ba9` to `#001a4a`)
- Semi-transparent dark containers (`#a1000000`) with rounded corners
- White text throughout
- Hover animations on all buttons (margin shrink + font size increase over 0.2s)
