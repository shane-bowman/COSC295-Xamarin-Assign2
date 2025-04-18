using System;
using Xamarin.Forms;

namespace Assignment2
{
    /// <summary>
    /// A page for adding a new opponent to the database.
    /// </summary>
    public class AddNewOpponentPage : ContentPage
    {
        private Entry firstNameEntry;
        private Entry lastNameEntry;
        private Entry addressEntry;
        private Entry phoneEntry;
        private Entry emailEntry;

        /// <summary>
        /// Initializes a new instance of the AddNewOpponentPage.
        /// </summary>
        public AddNewOpponentPage()
        {
            Title = "Opponents"; 

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
        /// Sets up the UI components of the page using a TableView.
        /// </summary>
        private void InitializeUI()
        {
            // Initialize form fields
            firstNameEntry = new Entry { Placeholder = "Enter first name" };
            lastNameEntry = new Entry { Placeholder = "Enter last name" };
            addressEntry = new Entry { Placeholder = "Enter address" };
            phoneEntry = new Entry { Placeholder = "Enter phone", Keyboard = Keyboard.Telephone };
            emailEntry = new Entry { Placeholder = "Enter email", Keyboard = Keyboard.Email };

            // Create a TableView for the form as required by the assignment
            var tableView = new TableView
            {
                Root = new TableRoot
                {
                    new TableSection("Add New Opponent")
                    {
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = { new Label { Text = "First Name:", VerticalOptions = LayoutOptions.Center }, firstNameEntry }
                            }
                        },
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = { new Label { Text = "Last Name:", VerticalOptions = LayoutOptions.Center }, lastNameEntry }
                            }
                        },
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = { new Label { Text = "Address:", VerticalOptions = LayoutOptions.Center }, addressEntry }
                            }
                        },
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = { new Label { Text = "Phone:", VerticalOptions = LayoutOptions.Center }, phoneEntry }
                            }
                        },
                        new ViewCell
                        {
                            View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = { new Label { Text = "Email:", VerticalOptions = LayoutOptions.Center }, emailEntry }
                            }
                        }
                    }
                }
            };

            // Save button
            Button saveButton = new Button { Text = "Save" };
            saveButton.Clicked += OnSaveClicked;

            // Layout
            Content = new StackLayout
            {
                Padding = 20,
                Children = { tableView, saveButton }
            };
        }

        /// <summary>
        /// Handles the save button click to add a new opponent to the database.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(firstNameEntry.Text) || string.IsNullOrWhiteSpace(lastNameEntry.Text))
            {
                await DisplayAlert("Error", "First Name and Last Name are required.", "OK");
                return;
            }

            // Create new opponent
            Opponent newOpponent = new Opponent
            {
                FName = firstNameEntry.Text,
                LName = lastNameEntry.Text,
                Address = addressEntry.Text,
                Phone = phoneEntry.Text,
                Email = emailEntry.Text
            };

            // Save to database with error handling
            try
            {
                App.Database.SaveOpponent(newOpponent);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save opponent: {ex.Message}", "OK");
            }
        }
    }
}