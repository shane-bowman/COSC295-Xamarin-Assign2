using System;
using Xamarin.Forms;

namespace Assignment2
{
    /// <summary>
    /// A page that allows the user to reset the app to its default state.
    /// </summary>
    public class SettingsPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the SettingsPage.
        /// </summary>
        public SettingsPage()
        {
            Title = "Settings";

            // Add toolbar items for navigation
            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Games",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(async () => await Navigation.PushAsync(new GamesPage()))
            });
            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Settings",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(async () => await Navigation.PushAsync(new SettingsPage()))
            });

            InitializeUI();
        }

        /// <summary>
        /// Sets up the UI components of the page.
        /// </summary>
        private void InitializeUI()
        {
            // Label describing the button's purpose
            var descriptionLabel = new Label
            {
                Text = "Reset the app to default data. This will delete all opponents, matches, and games, and repopulate the games with default values (Chess, Checkers, Dominoes).",
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(20, 20, 20, 10)
            };

            // Reset App button
            var resetButton = new Button
            {
                Text = "Reset App",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(20, 10, 20, 20)
            };
            resetButton.Clicked += OnResetClicked;

            // Layout
            Content = new StackLayout
            {
                Children = { descriptionLabel, resetButton },
                Padding = new Thickness(10),
                VerticalOptions = LayoutOptions.Center
            };
        }

        /// <summary>
        /// Handles the reset button click to reset the app's database.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private async void OnResetClicked(object sender, EventArgs e)
        {
            // Confirm the reset action
            bool confirm = await DisplayAlert(
                "Reset App",
                "Are you sure you want to reset the app? This will delete all opponents, matches, and games, and repopulate the games with default values.",
                "Yes", "No");
            if (!confirm)
            {
                return;
            }

            // Reset the database
            try
            {
                App.Database.ResetDatabase();
                await DisplayAlert("Success", "The app has been reset to default data.", "OK");
                // Navigate back to the OpponentsPage to reflect the reset state
                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to reset the app: {ex.Message}", "OK");
            }
        }
    }
}