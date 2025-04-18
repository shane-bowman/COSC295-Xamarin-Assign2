using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Assignment2
{
    /// <summary>
    /// The main page of the app, displaying a list of opponents and allowing navigation to their matches.
    /// </summary>
    public class OpponentsPage : ContentPage
    {
        private ObservableCollection<Opponent> opponents;

        /// <summary>
        /// Initializes a new instance of the OpponentsPage.
        /// </summary>
        public OpponentsPage()
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

            // Load opponents from the database
            opponents = new ObservableCollection<Opponent>();

            // Set up the ListView
            ListView listView = new ListView
            {
                ItemsSource = opponents,
                ItemTemplate = new DataTemplate(typeof(OpponentCell)),
                RowHeight = OpponentCell.RowHeight
            };

            // Handle tapping an opponent to view matches
            listView.ItemTapped += async (sender, e) =>
            {
                listView.SelectedItem = null; // Deselect item
                if (e.Item is Opponent selectedOpponent)
                {
                    await Navigation.PushAsync(new MatchesPage(selectedOpponent));
                }
            };

            // Layout
            StackLayout layout = new StackLayout { Padding = 10 };
            layout.Children.Add(listView);

            // Add New Opponent button
            Button btnNew = new Button { Text = "Add New Opponent" };
            btnNew.Clicked += async (sender, e) => await Navigation.PushAsync(new AddNewOpponentPage());
            layout.Children.Add(btnNew);

            Content = layout;
        }

        /// <summary>
        /// Refreshes the list of opponents when the page appears.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                // Refresh the list when returning to this page
                opponents.Clear();
                foreach (var opponent in App.Database.GetOpponents())
                {
                    opponents.Add(opponent);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load opponents: {ex.Message}", "OK");
            }
        }
    }

    /// <summary>
    /// A custom ViewCell for displaying opponent information in the ListView.
    /// </summary>
    public class OpponentCell : ViewCell
    {
        public const int RowHeight = 40; // Reduced height since we only have one row now

        /// <summary>
        /// Initializes a new instance of the OpponentCell.
        /// </summary>
        public OpponentCell()
        {
            // Grid to layout the name and phone number on the same row
            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star }, // Left column for name
                    new ColumnDefinition { Width = GridLength.Auto }  // Right column for phone
                },
                Padding = new Thickness(5),
                ColumnSpacing = 10
            };

            // StackLayout for first and last name on the same line
            var nameLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 5
            };

            // First name (bold)
            Label lblFirstName = new Label { TextColor = Color.Black };
            lblFirstName.SetBinding(Label.TextProperty, "FName");

            // Last name (gray)
            Label lblLastName = new Label { TextColor = Color.Black };
            lblLastName.SetBinding(Label.TextProperty, "LName");

            nameLayout.Children.Add(lblFirstName);
            nameLayout.Children.Add(lblLastName);

            // Phone number (right side)
            Label lblPhone = new Label { TextColor = Color.Black, HorizontalOptions = LayoutOptions.End };
            lblPhone.SetBinding(Label.TextProperty, "Phone");

            // Add to grid
            grid.Children.Add(nameLayout, 0, 0);
            grid.Children.Add(lblPhone, 1, 0);

            View = grid;

            // Context action for delete (simulates tap-holding)
            MenuItem mi = new MenuItem { Text = "Delete", IsDestructive = true };
            mi.Clicked += async (sender, e) =>
            {
                var opponent = (Opponent)this.BindingContext;
                ListView parent = (ListView)this.Parent;
                ObservableCollection<Opponent> list = (ObservableCollection<Opponent>)parent.ItemsSource;

                bool confirm = await Application.Current.MainPage.DisplayAlert(
                    "Delete Opponent",
                    $"Delete {opponent.FName} {opponent.LName} and all their matches?",
                    "Yes", "No");
                if (confirm)
                {
                    try
                    {
                        App.Database.DeleteOpponent(opponent);
                        list.Remove(opponent);
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete opponent: {ex.Message}", "OK");
                    }
                }
            };
            ContextActions.Add(mi);
        }
    }
}