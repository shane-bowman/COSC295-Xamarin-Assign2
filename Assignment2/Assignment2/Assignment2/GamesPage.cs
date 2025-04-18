using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Assignment2
{
    public class GamesPage : ContentPage
    {
        private ObservableCollection<GameDisplay> games;
        private ListView gamesListView;

        public GamesPage()
        {
            Title = "Prev Title";

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
            LoadData();
        }

        private void InitializeUI()
        {
            games = new ObservableCollection<GameDisplay>();
            gamesListView = new ListView
            {
                ItemsSource = games,
                ItemTemplate = new DataTemplate(() =>
                {
                    // Define UI elements for each game
                    var nameLabel = new Label
                    {
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold
                    };
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    var descriptionLabel = new Label
                    {
                        FontSize = 14,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                    descriptionLabel.SetBinding(Label.TextProperty, "Description");

                    var ratingLabel = new Label
                    {
                        FontSize = 14,
                        HorizontalOptions = LayoutOptions.End
                    };
                    ratingLabel.SetBinding(Label.TextProperty, "Rating", stringFormat: "{0:F1}");

                    // Horizontal StackLayout to place description and rating on the same row
                    var descriptionRatingLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        Children = { descriptionLabel, ratingLabel }
                    };

                    var matchCountLabel = new Label
                    {
                        FontSize = 14
                    };
                    matchCountLabel.SetBinding(Label.TextProperty, "MatchCount", stringFormat: "Matches: {0}");

                    // Main layout for the ListView item
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(10),
                            Spacing = 5,
                            Children =
                            {
                                nameLabel,
                                descriptionRatingLayout,
                                matchCountLabel
                            }
                        }
                    };
                }),
                RowHeight = 100
            };

            // Set the content directly to the ListView, removing the "Games" header
            Content = new StackLayout
            {
                Children = { gamesListView },
                Padding = new Thickness(10)
            };
        }

        private void LoadData()
        {
            try
            {
                games.Clear();
                var allGames = App.Database.GetGames();
                foreach (var game in allGames)
                {
                    var matchCount = App.Database.GetMatchCountForGame(game.Id);
                    games.Add(new GameDisplay
                    {
                        Name = game.Name,
                        Description = game.Description,
                        Rating = game.Rating,
                        MatchCount = matchCount
                    });
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load games: {ex.Message}", "OK");
            }
        }
    }

    public class GameDisplay
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public int MatchCount { get; set; }
    }
}