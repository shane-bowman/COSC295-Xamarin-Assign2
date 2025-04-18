using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Assignment2
{
    public class MatchesPage : ContentPage
    {
        private Opponent selectedOpponent;
        private ObservableCollection<MatchDisplay> matches;
        private Picker gamePicker;
        private DatePicker datePicker;
        private Entry commentsEntry;
        private Switch winSwitch;
        private Button saveButton;
        // Added fields for editing
        private MatchDisplay selectedMatch; // Tracks the match being edited
        private Button cancelButton;       // Button to cancel editing

        public MatchesPage(Opponent opponent)
        {
            selectedOpponent = opponent;
            Title = "Opponents";

            // Add ToolbarItems for "Games" and "Settings"
            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Games",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => Navigation.PushAsync(new GamesPage()))
            });
            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Settings",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => Navigation.PushAsync(new SettingsPage()))
            });

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            // ListView for matches
            matches = new ObservableCollection<MatchDisplay>();
            var matchesListView = new ListView
            {
                ItemsSource = matches,
                ItemTemplate = new DataTemplate(() =>
                {
                    // Grid for each match entry
                    var grid = new Grid
                    {
                        RowDefinitions =
                        {
                            new RowDefinition { Height = GridLength.Auto }, // Opponent name
                            new RowDefinition { Height = GridLength.Auto }, // Date and comment
                            new RowDefinition { Height = GridLength.Auto }  // Game and win
                        },
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }, // Left column (wider)
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }  // Right column (narrower)
                        },
                        RowSpacing = 5,
                        Padding = new Thickness(10),
                        ColumnSpacing = 10
                    };

                    // Opponent name (spans both columns)
                    var nameLabel = new Label
                    {
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        LineBreakMode = LineBreakMode.WordWrap
                    };
                    nameLabel.SetBinding(Label.TextProperty, "OpponentName");
                    grid.Children.Add(nameLabel, 0, 0);
                    Grid.SetColumnSpan(nameLabel, 2);

                    // Date
                    var dateLabel = new Label
                    {
                        FontSize = 14,
                        LineBreakMode = LineBreakMode.WordWrap
                    };
                    dateLabel.SetBinding(Label.TextProperty, "Date");
                    grid.Children.Add(dateLabel, 0, 1);

                    // Comment
                    var commentLabel = new Label
                    {
                        FontSize = 14,
                        HorizontalTextAlignment = TextAlignment.End,
                        LineBreakMode = LineBreakMode.WordWrap
                    };
                    commentLabel.SetBinding(Label.TextProperty, "Comments");
                    grid.Children.Add(commentLabel, 1, 1);

                    // Game
                    var gameLabel = new Label
                    {
                        FontSize = 14,
                        LineBreakMode = LineBreakMode.WordWrap
                    };
                    gameLabel.SetBinding(Label.TextProperty, "GameName");
                    grid.Children.Add(gameLabel, 0, 2);

                    // Win status
                    var winToggle = new Switch
                    {
                        HorizontalOptions = LayoutOptions.End,
                        IsEnabled = false // Read-only
                    };
                    winToggle.SetBinding(Switch.IsToggledProperty, "Match.Win");
                    grid.Children.Add(winToggle, 1, 2);

                    // Create the ViewCell
                    var viewCell = new ViewCell { View = grid };

                    // Add delete context action
                    var deleteAction = new MenuItem
                    {
                        Text = "Delete",
                        IsDestructive = true
                    };
                    deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                    deleteAction.Clicked += async (sender, e) =>
                    {
                        var menuItem = (MenuItem)sender;
                        var matchDisplay = (MatchDisplay)menuItem.CommandParameter;

                        // Confirm deletion with the user
                        bool confirm = await DisplayAlert("Delete Match", "Are you sure you want to delete this match?", "Yes", "No");
                        if (confirm)
                        {
                            try
                            {
                                // Delete from database and remove from collection
                                App.Database.DeleteMatch(matchDisplay.Match);
                                matches.Remove(matchDisplay);
                            }
                            catch (Exception ex)
                            {
                                // Handle any errors during deletion
                                await DisplayAlert("Error", $"Failed to delete match: {ex.Message}", "OK");
                            }
                        }
                    };
                    viewCell.ContextActions.Add(deleteAction);

                    return viewCell;
                }),
                RowHeight = 100,
                SeparatorVisibility = SeparatorVisibility.Default
            };

            // Handle tapping a match to edit
            matchesListView.ItemTapped += (sender, e) =>
            {
                selectedMatch = e.Item as MatchDisplay;
                if (selectedMatch != null)
                {
                    // Load match details into the form
                    datePicker.Date = selectedMatch.Match.Date;
                    commentsEntry.Text = selectedMatch.Match.Comments;
                    gamePicker.SelectedItem = selectedMatch.GameName;
                    winSwitch.IsToggled = selectedMatch.Match.Win;
                    saveButton.Text = "UPDATE";
                    cancelButton.IsVisible = true;
                    matchesListView.SelectedItem = null; // Deselect the item
                }
            };

            // Form for adding a match
            datePicker = new DatePicker
            {
                Date = DateTime.Today,
                Format = "D",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            commentsEntry = new Entry
            {
                Placeholder = "Enter comment",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            gamePicker = new Picker
            {
                Title = "Select Game",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            winSwitch = new Switch
            {
                IsToggled = false,
                HorizontalOptions = LayoutOptions.Start
            };
            saveButton = new Button
            {
                Text = "ADD",
                HorizontalOptions = LayoutOptions.Center
            };
            saveButton.Clicked += OnSaveMatchClicked;

            // Added cancel button for editing
            cancelButton = new Button
            {
                Text = "CANCEL",
                IsVisible = false, // Hidden by default
                HorizontalOptions = LayoutOptions.Center
            };
            cancelButton.Clicked += (sender, e) =>
            {
                selectedMatch = null;
                saveButton.Text = "ADD";
                cancelButton.IsVisible = false;
                ResetForm();
            };

            // Form layout
            var formLayout = new StackLayout
            {
                Spacing = 15,
                Padding = new Thickness(10),
                Children =
                {
                    new Label { Text = "Add Match", FontSize = 20, FontAttributes = FontAttributes.Bold },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        Children =
                        {
                            new Label { Text = "Date:", VerticalOptions = LayoutOptions.Center },
                            datePicker
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        Children =
                        {
                            new Label { Text = "Comment:", VerticalOptions = LayoutOptions.Center },
                            commentsEntry
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        Children =
                        {
                            new Label { Text = "Game:", VerticalOptions = LayoutOptions.Center },
                            gamePicker
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        Children =
                        {
                            new Label { Text = "Win?", VerticalOptions = LayoutOptions.Center },
                            winSwitch
                        }
                    },
                    new StackLayout // Added to group buttons horizontally
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        HorizontalOptions = LayoutOptions.Center,
                        Children = { saveButton, cancelButton }
                    }
                }
            };

            // Main layout with Grid
            var mainGrid = new Grid
            {
                Padding = 10,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Auto }
                }
            };
            mainGrid.Children.Add(matchesListView, 0, 0);
            mainGrid.Children.Add(formLayout, 0, 1);

            Content = mainGrid;
        }

        private async void LoadData()
        {
            try
            {
                matches.Clear();
                var loadedMatches = App.Database.GetMatchesForOpponent(selectedOpponent.Id);
                var games = App.Database.GetGames();
                var gameDict = games.ToDictionary(g => g.Id, g => g.Name);

                foreach (var match in loadedMatches)
                {
                    matches.Add(new MatchDisplay
                    {
                        Match = match,
                        OpponentName = $"{selectedOpponent.FName} {selectedOpponent.LName}",
                        GameName = gameDict.ContainsKey(match.GameId) ? gameDict[match.GameId] : "Unknown"
                    });
                }

                gamePicker.ItemsSource = games.Select(g => g.Name).ToList();
                string lastGame = Preferences.Get("LastGame", "Chess");
                gamePicker.SelectedItem = lastGame;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }

        private async void OnSaveMatchClicked(object sender, EventArgs e)
        {
            try
            {
                if (gamePicker.SelectedIndex == -1)
                {
                    await DisplayAlert("Error", "Please select a game.", "OK");
                    return;
                }

                var games = App.Database.GetGames();
                var selectedGame = games[gamePicker.SelectedIndex];

                if (selectedMatch == null) // Add mode
                {
                    var newMatch = new Match
                    {
                        OpponentId = selectedOpponent.Id,
                        GameId = selectedGame.Id,
                        Date = datePicker.Date,
                        Comments = commentsEntry.Text,
                        Win = winSwitch.IsToggled
                    };
                    App.Database.SaveMatch(newMatch);
                }
                else // Edit mode
                {
                    selectedMatch.Match.Date = datePicker.Date;
                    selectedMatch.Match.Comments = commentsEntry.Text;
                    selectedMatch.Match.GameId = selectedGame.Id;
                    selectedMatch.Match.Win = winSwitch.IsToggled;
                    App.Database.SaveMatch(selectedMatch.Match);
                }

                Preferences.Set("LastGame", selectedGame.Name);
                LoadData();
                ResetForm(); // Reset form after saving
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save match: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Resets the form fields to default values and sets the mode to add.
        /// </summary>
        private void ResetForm()
        {
            selectedMatch = null;
            saveButton.Text = "ADD";
            cancelButton.IsVisible = false;
            datePicker.Date = DateTime.Today;
            commentsEntry.Text = "";
            winSwitch.IsToggled = false;
            string lastGame = Preferences.Get("LastGame", "Chess");
            gamePicker.SelectedItem = lastGame;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
            ResetForm(); // Reset form when page appears
        }

        public class MatchDisplay
        {
            public Match Match { get; set; }
            public string OpponentName { get; set; }
            public string GameName { get; set; }
            public string Date => $"{Match.Date:dddd, MMMM d, yyyy}";
            public string Comments => Match.Comments ?? "";
        }
    }
}