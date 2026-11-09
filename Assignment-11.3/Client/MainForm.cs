using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Caleb_Marks_Assignment_11._3_Client
{
    public partial class MainForm : Form
    {
        HttpClient gamesClient = new HttpClient();

        // Zero means no game selected
        int selectedGameId = 0;

        // Build form controls
        public MainForm()
        {
            InitializeComponent();
        }

        // Set API address & show games
        private async void MainForm_Load(object sender, EventArgs e)
        {
            gamesClient.BaseAddress = new Uri("http://localhost:5222/api/games");
            btnUpdate.Enabled = false;
            await LoadGames();
        }

        // Reload games using button
        private async void btnLoad_Click(object sender, EventArgs e)
        {
            await LoadGames();
        }

        // Request games, read JSON, & fill grid
        private async Task LoadGames()
        {
            try
            {
                HttpResponseMessage response = await gamesClient.GetAsync(gamesClient.BaseAddress);
                if (response.IsSuccessStatusCode)
                {
                    List<Game>? games = await response.Content.ReadFromJsonAsync<List<Game>>();
                    gamesGrid.DataSource = games;
                    gamesGrid.ClearSelection();
                }
                else
                {
                    ShowStatusError("load games", response);
                }
            }
            catch (HttpRequestException error)
            {
                ShowApiError(error);
            }
        }

        // Validate form values & post new game
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            Game? game = ReadGame();
            if (game == null)
            {
                return;
            }

            try
            {
                using (StringContent content = CreateJsonContent(game))
                {
                    HttpResponseMessage response = await gamesClient.PostAsync(gamesClient.BaseAddress, content);
                    if (response.IsSuccessStatusCode)
                    {
                        ResetForm();
                        await LoadGames();
                    }
                    else
                    {
                        ShowStatusError("add game", response);
                    }
                }
            }
            catch (HttpRequestException error)
            {
                ShowApiError(error);
            }
        }

        // Fill form using selected row & enable Update
        private void btnSelect_Click(object sender, EventArgs e)
        {
            Game? game = GetSelectedGame();
            if (game == null)
            {
                return;
            }

            selectedGameId = game.Id;
            txtSeason.Text = game.Season.ToString();
            txtOpponent.Text = game.Opponent;
            txtOuScore.Text = game.OuScore.ToString();
            txtOpponentScore.Text = game.OpponentScore.ToString();
            txtLocation.Text = game.Location;
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
        }

        // Validate form values & put changes using game ID
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            Game? game = ReadGame();
            if (game == null)
            {
                return;
            }

            game.Id = selectedGameId;

            try
            {
                using (StringContent content = CreateJsonContent(game))
                {
                    HttpResponseMessage response = await gamesClient.PutAsync(gamesClient.BaseAddress + "/" + game.Id, content);
                    if (response.IsSuccessStatusCode)
                    {
                        ResetForm();
                        await LoadGames();
                    }
                    else
                    {
                        ShowStatusError("update game", response);
                    }
                }
            }
            catch (HttpRequestException error)
            {
                ShowApiError(error);
            }
        }

        // Confirm & delete selected game using its ID
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            Game? game = GetSelectedGame();
            if (game == null)
            {
                return;
            }

            DialogResult answer = MessageBox.Show("Delete " + game.Season + " " + game.Opponent + "?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            try
            {
                HttpResponseMessage response = await gamesClient.DeleteAsync(gamesClient.BaseAddress + "/" + game.Id);
                if (response.IsSuccessStatusCode)
                {
                    ResetForm();
                    await LoadGames();
                }
                else
                {
                    ShowStatusError("delete game", response);
                }
            }
            catch (HttpRequestException error)
            {
                ShowApiError(error);
            }
        }

        // Clear form; nothing saved
        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // Return selected game; missing selection shows message
        private Game? GetSelectedGame()
        {
            if (gamesGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a game in the grid first.");
                return null;
            }

            return gamesGrid.SelectedRows[0].DataBoundItem as Game;
        }

        // Build game using user values; invalid input shows message & returns null
        private Game? ReadGame()
        {
            int season;
            int ouScore;
            int opponentScore;
            string opponent = txtOpponent.Text.Trim();
            string location = txtLocation.Text.Trim();

            if (!Int32.TryParse(txtSeason.Text, out season) || season < 1895 || season > 2100)
            {
                MessageBox.Show("Enter a season from 1895 to 2100.");
                return null;
            }

            // Require opponent & location; limit length
            if (opponent.Length == 0 || opponent.Length > 100 || location.Length == 0 || location.Length > 100)
            {
                MessageBox.Show("Enter an opponent and location, each 100 characters or fewer.");
                return null;
            }

            if (!Int32.TryParse(txtOuScore.Text, out ouScore) || ouScore < 0 || ouScore > 200 ||
                !Int32.TryParse(txtOpponentScore.Text, out opponentScore) || opponentScore < 0 || opponentScore > 200)
            {
                MessageBox.Show("Enter both scores from 0 to 200.");
                return null;
            }

            Game game = new Game();
            game.Season = season;
            game.Opponent = opponent;
            game.OuScore = ouScore;
            game.OpponentScore = opponentScore;
            game.Location = location;

            return game;
        }

        // Build JSON request body using game
        private StringContent CreateJsonContent(Game game)
        {
            return new StringContent(JsonSerializer.Serialize(game), Encoding.UTF8, "application/json");
        }

        // Empty form & return Add mode
        private void ResetForm()
        {
            selectedGameId = 0;
            txtSeason.Clear();
            txtOpponent.Clear();
            txtOuScore.Clear();
            txtOpponentScore.Clear();
            txtLocation.Clear();
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            gamesGrid.ClearSelection();
        }

        // Show failed request's status code
        private void ShowStatusError(string action, HttpResponseMessage response)
        {
            MessageBox.Show("Could not " + action + ". Status code: " + (int)response.StatusCode);
        }

        // Explain unreachable API
        private void ShowApiError(HttpRequestException error)
        {
            MessageBox.Show("The API could not be reached. Start the API project first.\n\n" + error.Message);
        }
    }
}
