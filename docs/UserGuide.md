# User Guide

## Starting the Application

Launch the application by running the executable or pressing F5 in Visual Studio.
You will see the main screen with two buttons:

- **Gerichte** (Dishes) - Opens the dish list
- **Zutaten** (Ingredients) - Opens your aggregated shopping list

## Browsing Dishes

1. Click **Gerichte** on the main screen.
2. You will see a list of available dishes (Pasta, Salad, Sandwich are pre-loaded).
3. Click any dish name to open its ingredient details.
4. Use the **Refresh** button to reload the list.
5. Click **Zurueck** (Back) to return to the main screen.

## Adding a New Dish

1. From the dish list, click **Hinzufuegen** (Add).
2. Enter the dish name in the top text field.
3. For each ingredient, type in the format: `Name Amount Unit` (e.g. `Butter 20 g`) and click **add**.
   - The ingredient will appear in the list below.
4. Optionally check **Vegan** or **Vegetarisch** (these are mutually exclusive).
5. Click **Bestaetigen** (Confirm) to save the dish.

## Calculating Ingredient Amounts

1. Click on a dish from the dish list.
2. The left panel shows the ingredients per person.
3. Enter the number of people in the input field (top left).
4. Click the calculate button next to the input field.
5. The right panel shows the scaled amounts.
   - Amounts >= 1000g are automatically converted to kg.
   - Amounts >= 1000ml are automatically converted to l.
6. Click the green **Bestaetigen** (Confirm) button to add these ingredients to your shopping list.
7. Click **Zurueck** (Back) to return to the dish list.

## Viewing Your Shopping List

1. From the main screen, click **Zutaten** (Ingredients).
2. This page shows all ingredients you have confirmed from various dishes.
3. Duplicate ingredients are automatically merged:
   - Same ingredient from different dishes gets its amounts summed.
   - Units are converted as needed (e.g. 500g + 600g = 1.1 kg).
4. If no ingredients have been added yet, a placeholder message is shown.

## Tips

- All amounts are specified **per person**. The calculator multiplies by the number of people you enter.
- The ingredient input format must be exactly three words: `Name Amount Unit`.
- The application remembers your ingredients for the current session. Closing the app resets everything.
