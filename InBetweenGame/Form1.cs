using System;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.ComponentModel;

namespace InBetweenGame
{
    public partial class Form1 : Form
    {
        #region Constants
        private const int SpeechRate = 4;
        private const int SpeechVolume = 100;
        private const int MinimumBet = 10;
        private const int DefaultStartingChips = 100;
        private const int TurnDelayMilliseconds = 3000;
        private const int ThinkingDelayMilliseconds = 1500;
        private const int SkipTurnDelayMilliseconds = 3000;
        private const string SaveGameFileName = "savegame.dat";
        #endregion

        #region Fields

        private int _currentPlayer = 0;
        private int _playerCount = 0;
        private bool _isGameInProgress = false;
        private bool _gameLoopRunning = false;
        private Card _leftCard;
        private Card _centerCard;
        private Card _rightCard;
        private int _currentBetAmount = 0;

        public readonly Random _rng;
        private readonly SpeechSynthesizer _cardTalk;
        private readonly string _resourceDirectory;
        private readonly string _appDirectory;
        private readonly string _speechFile;
        private readonly string _FullLog;
        private readonly string _LastGameLog;
        private readonly string _saveGameFile;
        private Deck _gameDeck;
        private TaskCompletionSource<int> _humanBetTcs;
        private readonly Dictionary<string, string> _imagePathCache;

        // Felt texture and visual effects
        private TextureBrush _feltBrush;
        private Panel _leftCardPanel;
        private Panel _centerCardPanel;
        private Panel _rightCardPanel;

        // Enhanced bet controls
        private Panel _betControlPanel;
        private Button _betMinButton;
        private Button _bet10Button;
        private Button _bet25Button;
        private Button _bet50Button;
        private Button _bet75Button;
        private Button _betMaxButton;
        private Button _placeBetButton;
        private Label _oddsLabel;
        private Label _spreadLabel;
        private Label _suggestedBetLabel;
        private ProgressBar _oddsProgressBar;
        private Label _currentBetLabel;

        // Static readonly dictionaries for faster lookups
        private static readonly Dictionary<char, int> CardValues = new Dictionary<char, int>
        {
            { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 },
            { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'T', 10 },
            { 'J', 11 }, { 'Q', 12 }, { 'K', 13 }, { 'A', 14 }
        };

        private static readonly Dictionary<char, string> RankNames = new Dictionary<char, string>
        {
            { '2', "Two" }, { '3', "Three" }, { '4', "Four" }, { '5', "Five" },
            { '6', "Six" }, { '7', "Seven" }, { '8', "Eight" }, { '9', "Nine" },
            { 'T', "Ten" }, { 'J', "Jack" }, { 'Q', "Queen" }, { 'K', "King" }, { 'A', "Ace" }
        };

        private static readonly Dictionary<char, string> SuitNames = new Dictionary<char, string>
        {
            { 'H', "Hearts" }, { 'D', "Diamonds" }, { 'C', "Clubs" }, { 'S', "Spades" }
        };
        #endregion

        #region Structs
        public struct PlayerInfo
        {
            public int Chips;
            public int Losses;
            public string Name;
            public string Type;
            public int Wins;
            public double WinPercentage;
            public int WinStreak;
        }
        #endregion

        #region Properties
        public PlayerInfo[] Players { get; set; } = new PlayerInfo[6];
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();

            // Use more reliable design-time check
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            // Runtime initialization only
            try
            {
                _rng = new Random();
                _cardTalk = new SpeechSynthesizer
                {
                    Rate = SpeechRate,
                    Volume = SpeechVolume
                };
                _resourceDirectory = Path.Combine(AppContext.BaseDirectory, "Resources");
                _appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InBetween");

                _speechFile = Path.Combine(_appDirectory, "speech.ini");
                _saveGameFile = Path.Combine(_appDirectory, SaveGameFileName);
                _FullLog = Path.Combine(_appDirectory, "Full Log.csv");
                _LastGameLog = Path.Combine(_appDirectory, "last game.txt");

                LoadSpeechSettings();

                // Initialize felt texture and visual enhancements
                InitializeFeltTexture();
                InitializeCardPanels();

                // Initialize enhanced bet controls
                InitializeEnhancedBetControls();

                // Initialize image path cache for all 52 cards
                _imagePathCache = new Dictionary<string, string>(52);
                InitializeImagePathCache();

                _gameDeck = new Deck(_rng);
                _gameDeck.Shuffle();

                InitializePlayerDataGridView();
                UpdatePlayerDisplay();
                ResetCards();

                LoadGame();

                this.FormClosing += Form1_FormClosing;
                this.Paint += Form1_Paint;
                this.KeyPreview = true;
                this.KeyDown += Form1_KeyDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing form: {ex.Message}", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Creates a felt texture for the background
        /// </summary>
        private void InitializeFeltTexture()
        {
            // Create a felt-like texture programmatically
            Bitmap feltTexture = new Bitmap(100, 100);
            using (Graphics g = Graphics.FromImage(feltTexture))
            {
                // Base color - dark green felt
                g.Clear(Color.FromArgb(0, 100, 0));

                // Add texture noise
                Random rand = new Random(42); // Fixed seed for consistent texture
                for (int i = 0; i < 10000; i++)
                {
                    int x = rand.Next(feltTexture.Width);
                    int y = rand.Next(feltTexture.Height);
                    int brightness = rand.Next(-15, 15);
                    Color baseColor = Color.FromArgb(0, 100, 0);
                    Color pixelColor = Color.FromArgb(
                        Math.Max(0, Math.Min(255, baseColor.R + brightness)),
                        Math.Max(0, Math.Min(255, baseColor.G + brightness)),
                        Math.Max(0, Math.Min(255, baseColor.B + brightness))
                    );
                    feltTexture.SetPixel(x, y, pixelColor);
                }
            }

            _feltBrush = new TextureBrush(feltTexture, WrapMode.Tile);
        }

        /// <summary>
        /// Creates styled panels around card picture boxes for casino look
        /// </summary>
        private void InitializeCardPanels()
        {
            // Left card panel
            _leftCardPanel = CreateCardPanel(LeftCardPB);
            _leftCardPanel.Paint += CardPanel_Paint;

            // Center card panel
            _centerCardPanel = CreateCardPanel(CenterCardPB);
            _centerCardPanel.Paint += CardPanel_Paint;

            // Right card panel
            _rightCardPanel = CreateCardPanel(RightCardPB);
            _rightCardPanel.Paint += CardPanel_Paint;

            // Add panels to form (behind picture boxes)
            this.Controls.Add(_leftCardPanel);
            this.Controls.Add(_centerCardPanel);
            this.Controls.Add(_rightCardPanel);
            _leftCardPanel.SendToBack();
            _centerCardPanel.SendToBack();
            _rightCardPanel.SendToBack();

            // Bring picture boxes to front
            LeftCardPB.BringToFront();
            CenterCardPB.BringToFront();
            RightCardPB.BringToFront();
        }

        /// <summary>
        /// Initializes enhanced betting controls with casino styling - vertical layout
        /// </summary>
        private void InitializeEnhancedBetControls()
        {
            Font PanelFont = new Font("Tahoma", 18F, FontStyle.Bold);

            // Main bet control panel - vertical orientation on the right side
            _betControlPanel = new Panel
            {
                AccessibleName = "Bet Control Panel",
                Size = new Size(280, 660),
                Location = new Point(900, 50),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            _betControlPanel.Paint += BetControlPanel_Paint;

            int yPos = 15;
            int leftMargin = 15;
            int controlWidth = 250;

            // Current bet display - at the top
            _currentBetLabel = new Label
            {
                Text = "$0",
                Font = new Font("Tahoma", 28F, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(controlWidth, 50),
                Location = new Point(leftMargin, yPos),
                TextAlign = ContentAlignment.MiddleCenter
            };
            yPos += 60;

            // Spread indicator label
            _spreadLabel = new Label
            {
                Text = "SPREAD: --",
                Font = PanelFont,
                ForeColor = Color.Gold,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(controlWidth, 30),
                Location = new Point(leftMargin, yPos),
                TextAlign = ContentAlignment.MiddleCenter
            };
            yPos += 40;

            // Odds percentage label
            _oddsLabel = new Label
            {
                Text = "WIN ODDS: --%",
                Font = PanelFont,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(controlWidth, 30),
                Location = new Point(leftMargin, yPos),
                TextAlign = ContentAlignment.MiddleCenter
            };
            yPos += 40;

            // Odds progress bar
            _oddsProgressBar = new ProgressBar
            {
                Size = new Size(controlWidth, 25),
                Location = new Point(leftMargin, yPos),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Style = ProgressBarStyle.Continuous
            };
            yPos += 35;

            // Suggested bet label
            _suggestedBetLabel = new Label
            {
                Text = "Suggested: $--",
                Font = PanelFont,
                ForeColor = Color.LightGreen,
                BackColor = Color.Transparent,
                AutoSize = false,
                Size = new Size(controlWidth, 30),
                Location = new Point(leftMargin, yPos),
                TextAlign = ContentAlignment.MiddleCenter
            };
            yPos += 50;

            // Quick bet buttons - vertical stack
            int buttonWidth = controlWidth;
            int buttonHeight = 45;
            int buttonSpacing = 10;

            _betMinButton = CreateQuickBetButton("&MINIMUM BET", leftMargin, yPos, buttonWidth, buttonHeight, 0, PanelFont);
            yPos += buttonHeight + buttonSpacing;

            _bet10Button = CreateQuickBetButton("10% OF CHIPS", leftMargin, yPos, buttonWidth, buttonHeight, 1, PanelFont);
            yPos += buttonHeight + buttonSpacing;

            _bet25Button = CreateQuickBetButton("25% OF CHIPS", leftMargin, yPos, buttonWidth, buttonHeight, 2, PanelFont);
            yPos += buttonHeight + buttonSpacing;

            _bet50Button = CreateQuickBetButton("50% OF CHIPS", leftMargin, yPos, buttonWidth, buttonHeight, 3, PanelFont);
            yPos += buttonHeight + buttonSpacing;

            _bet75Button = CreateQuickBetButton("75% OF CHIPS", leftMargin, yPos, buttonWidth, buttonHeight, 4, PanelFont);
            yPos += buttonHeight + buttonSpacing;

            _betMaxButton = CreateQuickBetButton("MA&XIMUM BET", leftMargin, yPos, buttonWidth, buttonHeight, 5, PanelFont);
            yPos += buttonHeight + buttonSpacing + 10;

            // Place Bet button
            _placeBetButton = new Button
            {
                Text = "PLACE &BET",
                Size = new Size(buttonWidth, 60),
                Location = new Point(leftMargin, yPos),
                Font = new Font("Tahoma", 20F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 128, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _placeBetButton.FlatAppearance.BorderColor = Color.Gold;
            _placeBetButton.FlatAppearance.BorderSize = 3;
            _placeBetButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(34, 139, 34);
            _placeBetButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 100, 0);
            _placeBetButton.Click += PlaceBetButton_Click;

            // Add all controls to panel
            _betControlPanel.Controls.Add(_currentBetLabel);
            _betControlPanel.Controls.Add(_spreadLabel);
            _betControlPanel.Controls.Add(_oddsLabel);
            _betControlPanel.Controls.Add(_oddsProgressBar);
            _betControlPanel.Controls.Add(_suggestedBetLabel);
            _betControlPanel.Controls.Add(_betMinButton);
            _betControlPanel.Controls.Add(_bet10Button);
            _betControlPanel.Controls.Add(_bet25Button);
            _betControlPanel.Controls.Add(_bet50Button);
            _betControlPanel.Controls.Add(_bet75Button);
            _betControlPanel.Controls.Add(_betMaxButton);
            _betControlPanel.Controls.Add(_placeBetButton);

            // Add panel to form
            this.Controls.Add(_betControlPanel);
            _betControlPanel.BringToFront();

            // Initially hide the panel
            _betControlPanel.Visible = false;
        }

        /// <summary>
        /// Creates a styled quick bet button
        /// </summary>
        private Button CreateQuickBetButton(string text, int x, int y, int width, int height, int betType, Font font)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                Location = new Point(x, y),
                Font = font,
                BackColor = Color.FromArgb(139, 0, 0), // Dark red
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderColor = Color.Gold;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(178, 34, 34);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(220, 20, 60);

            btn.Click += (s, e) => SetQuickBet(betType);

            return btn;
        }

        /// <summary>
        /// Paints the bet control panel background
        /// </summary>
        private void BetControlPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = new Rectangle(5, 5, panel.Width - 10, panel.Height - 10);

            // Draw background with gradient
            using (LinearGradientBrush bgBrush = new LinearGradientBrush(
                bounds,
                Color.FromArgb(200, 20, 40, 20),
                Color.FromArgb(200, 0, 60, 0),
                LinearGradientMode.Vertical))
            {
                using (GraphicsPath bgPath = CreateRoundedRectangle(bounds, 15))
                {
                    g.FillPath(bgBrush, bgPath);
                }
            }

            // Draw gold border
            using (GraphicsPath borderPath = CreateRoundedRectangle(bounds, 15))
            {
                using (Pen borderPen = new Pen(Color.FromArgb(218, 165, 32), 3))
                {
                    g.DrawPath(borderPen, borderPath);
                }
            }
        }

        /// <summary>
        /// Creates a styled panel for card placement area
        /// </summary>
        private Panel CreateCardPanel(PictureBox cardPictureBox)
        {
            Panel panel = new Panel
            {
                Location = new Point(cardPictureBox.Left - 15, cardPictureBox.Top - 15),
                Size = new Size(cardPictureBox.Width + 30, cardPictureBox.Height + 30),
                BackColor = Color.Transparent,
                Anchor = cardPictureBox.Anchor
            };

            return panel;
        }

        /// <summary>
        /// Paints the card panel with border and shadow effects
        /// </summary>
        private void CardPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = new Rectangle(8, 8, panel.Width - 16, panel.Height - 16);

            // Draw drop shadow
            using (GraphicsPath shadowPath = CreateRoundedRectangle(new Rectangle(bounds.X + 6, bounds.Y + 6, bounds.Width, bounds.Height), 10))
            {
                using (PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath))
                {
                    shadowBrush.CenterColor = Color.FromArgb(100, 0, 0, 0);
                    shadowBrush.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
                    g.FillPath(shadowBrush, shadowPath);
                }
            }

            // Draw border outline (gold/brass casino style)
            using (GraphicsPath borderPath = CreateRoundedRectangle(bounds, 10))
            {
                // Outer gold border
                using (Pen outerPen = new Pen(Color.FromArgb(218, 165, 32), 4))
                {
                    g.DrawPath(outerPen, borderPath);
                }

                // Inner darker border for depth
                Rectangle innerBounds = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
                using (GraphicsPath innerPath = CreateRoundedRectangle(innerBounds, 8))
                using (Pen innerPen = new Pen(Color.FromArgb(139, 105, 20), 2))
                {
                    g.DrawPath(innerPen, innerPath);
                }
            }
        }

        /// <summary>
        /// Creates a rounded rectangle path for smooth corners
        /// </summary>
        private GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Paints the felt texture background
        /// </summary>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (_feltBrush != null)
            {
                e.Graphics.FillRectangle(_feltBrush, this.ClientRectangle);
            }
        }

        private void InitializePlayerDataGridView()
        {
            Font DataGridFont = new Font("Tahoma", 18F, FontStyle.Bold);
            PlayersDGV.ColumnCount = 6;

            // Column 0: Player Name
            PlayersDGV.Columns[0].Name = "PLAYER";
            PlayersDGV.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PlayersDGV.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            PlayersDGV.Columns[0].DefaultCellStyle.Font = DataGridFont;

            // Column 1: Type
            PlayersDGV.Columns[1].Name = "TYPE";
            PlayersDGV.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PlayersDGV.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            PlayersDGV.Columns[1].DefaultCellStyle.Font = DataGridFont;

            // Column 2: Chips
            PlayersDGV.Columns[2].Name = "CHIPS";
            PlayersDGV.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PlayersDGV.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PlayersDGV.Columns[2].DefaultCellStyle.Font = DataGridFont;

            // Column 3: Wins
            PlayersDGV.Columns[3].Name = "WINS";
            PlayersDGV.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PlayersDGV.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PlayersDGV.Columns[3].DefaultCellStyle.Font = DataGridFont;

            // Column 4: Losses
            PlayersDGV.Columns[4].Name = "LOSSES";
            PlayersDGV.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PlayersDGV.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PlayersDGV.Columns[4].DefaultCellStyle.Font = DataGridFont;

            // Column 5: Win %
            PlayersDGV.Columns[5].Name = "WIN %";
            PlayersDGV.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            PlayersDGV.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PlayersDGV.Columns[5].DefaultCellStyle.Font = DataGridFont;

            // Style the DataGridView
            PlayersDGV.EnableHeadersVisualStyles = false;
            PlayersDGV.Font = DataGridFont;
            PlayersDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGreen;
            PlayersDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            PlayersDGV.ColumnHeadersDefaultCellStyle.Font = DataGridFont;
            PlayersDGV.DefaultCellStyle.Font = DataGridFont;
            PlayersDGV.DefaultCellStyle.BackColor = Color.White;
            PlayersDGV.DefaultCellStyle.ForeColor = Color.Black;
            PlayersDGV.DefaultCellStyle.SelectionBackColor = Color.Gold;
            PlayersDGV.DefaultCellStyle.SelectionForeColor = Color.Black;
            PlayersDGV.RowTemplate.Height = 30;

            PlayersDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            PlayersDGV.GridColor = Color.DarkGreen;
            PlayersDGV.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        }

        /// <summary>
        /// Pre-populate the image path cache for all 52 cards
        /// </summary>
        private void InitializeImagePathCache()
        {
            char[] suits = { 'H', 'D', 'C', 'S' };
            char[] ranks = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

            foreach (char suit in suits)
            {
                foreach (char rank in ranks)
                {
                    string key = $"{rank}{suit}";
                    _imagePathCache[key] = Path.Combine(_resourceDirectory, $"{rank}-{suit}.png");
                }
            }
        }
        #endregion

        #region Game Management
        /// <summary>
        /// Starts a new game with configured players
        /// </summary>
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(_saveGameFile))
            {
                var result = MessageBox.Show(
                    "A saved game is in progress. Would you like to continue playing or start a new game?\n\nYes = Continue saved game\nNo = Delete save and start new game\nCancel = Don't start new game",
                    "Saved Game Found",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    LoadGame();
                    return;
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    try
                    {
                        File.Delete(_saveGameFile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting saved game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            NewGame newGameDialog = new NewGame(this);

            if (newGameDialog.ShowDialog() == DialogResult.OK)
            {
                var configuredPlayers = newGameDialog.GetConfiguredPlayers();
                Players = new PlayerInfo[6];

                for (int i = 0; i < configuredPlayers.Count && i < Players.Length; i++)
                {
                    Players[i] = configuredPlayers[i];
                }

                _playerCount = configuredPlayers.Count;
                _currentPlayer = 0;
                _isGameInProgress = true;

                // Reset deck
                _gameDeck = new Deck(_rng);
                _gameDeck.Shuffle();

                UpdatePlayerDisplay();
                UpdateStatus($"New Game started with {_playerCount} players, Starting chips are ${Players[0].Chips} ");

                // Start the game
                _ = StartGameLoopAsync();
            }
        }

        private CancellationTokenSource _gameLoopCts;

        /// <summary>
        /// Main game loop that processes each player's turn
        /// </summary>
        private async Task StartGameLoopAsync()
        {
            if (_gameLoopRunning)
            {
                return;
            }

            _gameLoopCts?.Cancel();
            _gameLoopCts = new CancellationTokenSource();
            var cancellationToken = _gameLoopCts.Token;

            _gameLoopRunning = true;

            try
            {
                while (_isGameInProgress && !cancellationToken.IsCancellationRequested)
                {
                    // Validate current player index
                    if (_currentPlayer < 0 || _currentPlayer >= _playerCount)
                    {
                        _currentPlayer = 0;
                    }

                    // Check for game end conditions
                    if (CheckGameEndConditions())
                    {
                        SaveFullLog();
                        SaveLastGameLog();

                        // Safe file deletion
                        try
                        {
                            if (File.Exists(_saveGameFile))
                            {
                                File.Delete(_saveGameFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting save file: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        _isGameInProgress = false;
                        break;
                    }

                    // Check if deck needs reshuffling
                    if (_gameDeck.CardsLeft < 3)
                    {
                        UpdateStatus("Reshuffling deck...");
                        _gameDeck = new Deck(_rng);
                        _gameDeck.Shuffle();
                        await Task.Delay(ThinkingDelayMilliseconds, cancellationToken);
                    }

                    // Process current player's turn
                    await ProcessPlayerTurnAsync();

                    // Move to next player
                    MoveToNextPlayer();

                    // Reset cards for next turn
                    ResetCards();
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancelling
            }
            finally
            {
                _gameLoopRunning = false;
            }
        }

        /// <summary>
        /// Processes a single player's turn
        /// </summary>
        private async Task ProcessPlayerTurnAsync()
        {
            // Validate current player index
            if (_currentPlayer < 0 || _currentPlayer >= _playerCount)
            {
                return;
            }

            // Common turn setup
            string turnMessage = $"{Players[_currentPlayer].Name}'s turn.  ";
            if (cardsToolStripMenuItem.Checked)
            {
                turnMessage += "Your cards are: ";
            }
            UpdateStatus(turnMessage);

            // Only place new cards if there aren't already cards on the table
            if (_leftCard == null || _rightCard == null)
            {
                PlaceCards();
            }

            if (_leftCard == null || _rightCard == null)
            {
                UpdateStatus("Error: Unable to place cards. Skipping turn.");
                await Task.Delay(SkipTurnDelayMilliseconds);
                return;
            }

            var turnResult = CheckTurnSkipConditions(_leftCard, _rightCard);
            if (turnResult.ShouldSkip)
            {
                UpdateStatus($"{turnResult.Reason} Turn skipped.");
                UpdatePlayerDisplay();
                await Task.Delay(SkipTurnDelayMilliseconds);
                return;
            }

            UpdatePlayerDisplay();

            // Player-specific logic
            if (Players[_currentPlayer].Type == "Human")
            {
                await HumanPlayerTurnAsync();
            }
            else if (Players[_currentPlayer].Type == "Computer")
            {
                await ComputerPlayerTurnAsync();
            }
        }

        /// <summary>
        /// Checks if the game should end
        /// </summary>
        private bool CheckGameEndConditions()
        {
            int playersWithChips = Players.Take(_playerCount).Count(p => p.Chips > 0);

            if (playersWithChips == 0)
            {
                UpdateStatus("Game Over! All players are out of chips.");
                MessageBox.Show("Game Over! All players are out of chips.", "In Between", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            if (playersWithChips == 1)
            {
                var winner = Players.Take(_playerCount).First(p => p.Chips > 0);
                UpdateStatus($"Game Over! {winner.Name} wins with ${winner.Chips}!");
                MessageBox.Show($"{winner.Name} wins with ${winner.Chips}!", "Winner!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }

            return false;
        }
        #endregion

        #region Player Turn Logic
        /// <summary>
        /// Handles a human player's turn
        /// </summary>
        private async Task HumanPlayerTurnAsync()
        {
            // Validate current player index
            if (_currentPlayer < 0 || _currentPlayer >= _playerCount)
            {
                return;
            }

            // Initialize bet amount
            int maxBet = Math.Max(0, Players[_currentPlayer].Chips);
            if (maxBet < MinimumBet)
            {
                UpdateStatus($"{Players[_currentPlayer].Name} has insufficient chips to bet. Turn skipped.");
                await Task.Delay(SkipTurnDelayMilliseconds);
                return;
            }

            _currentBetAmount = MinimumBet;
            _placeBetButton.Enabled = true;

            // Show enhanced bet controls
            UpdateBetControlDisplay();
            _betControlPanel.Visible = true;
            _betControlPanel.Focus();

            // Create a new TaskCompletionSource and wait for the player to place bet
            _humanBetTcs = new TaskCompletionSource<int>();
            int betAmount = await _humanBetTcs.Task;

            // Hide bet controls
            _betControlPanel.Visible = false;

            // Process the bet if it's valid
            if (betAmount > 0)
            {
                await ProcessBetAsync(betAmount);
            }
            else
            {
                UpdateStatus($"You pass. Moving to next player.");
                await Task.Delay(TurnDelayMilliseconds);
            }
        }

        /// <summary>
        /// Handles a computer player's turn
        /// </summary>
        private async Task ComputerPlayerTurnAsync()
        {
            // Validate current player index
            if (_currentPlayer < 0 || _currentPlayer >= _playerCount)
            {
                return;
            }

            int leftValue = GetCardValue(_leftCard.Rank);
            int rightValue = GetCardValue(_rightCard.Rank);
            int betAmount = CalculateAIBet(leftValue, rightValue);

            await Task.Delay(ThinkingDelayMilliseconds);

            if (betAmount <= 0 || betAmount > Players[_currentPlayer].Chips)
            {
                betAmount = Math.Min(MinimumBet, Players[_currentPlayer].Chips);
            }

            await ProcessBetAsync(betAmount);
        }

        /// <summary>
        /// Checks if the turn should be skipped due to matching or consecutive cards
        /// </summary>
        private (bool ShouldSkip, string Reason) CheckTurnSkipConditions(Card leftCard, Card rightCard)
        {
            if (leftCard == null || rightCard == null)
            {
                return (true, "Invalid cards!");
            }

            int leftValue = GetCardValue(leftCard.Rank);
            int rightValue = GetCardValue(rightCard.Rank);

            if (leftValue == rightValue)
            {
                return (true, "Matching cards!");
            }

            if (Math.Abs(rightValue - leftValue) == 1)
            {
                return (true, "Consecutive cards!");
            }

            return (false, string.Empty);
        }
        #endregion

        #region Bet Processing
        /// <summary>
        /// Processes a player's bet and determines win/loss
        /// </summary>
        private async Task ProcessBetAsync(int betAmount)
        {
            // Validate current player and bet amount
            if (_currentPlayer < 0 || _currentPlayer >= _playerCount || betAmount <= 0)
            {
                return;
            }

            await Task.Delay(ThinkingDelayMilliseconds);

            // Draw the center card
            _centerCard = _gameDeck.GetNextCard();

            if (_centerCard == null)
            {
                UpdateStatus("Error: Unable to draw center card.");
                return;
            }

            PlaceCardImage(CenterCardPB, _centerCard.Rank, _centerCard.Suit);

            // Determine win/loss
            bool isWinner = IsCardBetween(_centerCard.Rank, _leftCard.Rank, _rightCard.Rank);

            UpdatePlayerStats(betAmount, isWinner);

            string result = isWinner ? "WINS" : "LOSES";
            UpdateStatus($"{Players[_currentPlayer].Name} {result} ${betAmount}, and now has ${Players[_currentPlayer].Chips}.");

            // Highlight the result
            HighlightBetResult(isWinner);

            UpdatePlayerDisplay();

            if (Players[_currentPlayer].Chips <= 0)
            {
                UpdateStatus($"{Players[_currentPlayer].Name} is out of chips!");
            }

            await Task.Delay(TurnDelayMilliseconds);
        }

        /// <summary>
        /// Updates player statistics after a bet
        /// </summary>
        private void UpdatePlayerStats(int betAmount, bool isWinner)
        {
            // Validate current player index
            if (_currentPlayer < 0 || _currentPlayer >= _playerCount)
            {
                return;
            }

            if (isWinner)
            {
                Players[_currentPlayer].Chips += betAmount;
                Players[_currentPlayer].Wins++;
                Players[_currentPlayer].WinStreak++;
            }
            else
            {
                Players[_currentPlayer].Chips -= betAmount;
                Players[_currentPlayer].Losses++;
                Players[_currentPlayer].WinStreak = 0;
            }

            int totalGames = Players[_currentPlayer].Wins + Players[_currentPlayer].Losses;
            if (totalGames > 0)
            {
                Players[_currentPlayer].WinPercentage = (double)Players[_currentPlayer].Wins / totalGames * 100;
            }
        }
        #endregion

        #region Enhanced Bet Controls
        /// <summary>
        /// Updates the bet control display with current odds and suggestions
        /// </summary>
        private void UpdateBetControlDisplay()
        {
            if (_leftCard == null || _rightCard == null || _currentPlayer < 0 || _currentPlayer >= _playerCount)
            {
                return;
            }

            int leftValue = GetCardValue(_leftCard.Rank);
            int rightValue = GetCardValue(_rightCard.Rank);
            int spread = rightValue - leftValue - 1;

            // Update spread label
            _spreadLabel.Text = $"SPREAD: {spread}";
            _spreadLabel.ForeColor = GetSpreadColor(spread);

            // Calculate win odds
            int totalPossibleCards = 52 - 2; // Excluding left and right cards
            int winningCards = spread;
            double winPercentage = (double)winningCards / 13 * 100; // 13 possible ranks

            _oddsLabel.Text = $"WIN ODDS: {winPercentage:F1}%";
            _oddsProgressBar.Value = Math.Min(100, (int)winPercentage);

            // Set progress bar color based on odds
            if (winPercentage >= 60)
                _oddsLabel.ForeColor = Color.LightGreen;
            else if (winPercentage >= 40)
                _oddsLabel.ForeColor = Color.Yellow;
            else
                _oddsLabel.ForeColor = Color.LightCoral;

            // Calculate suggested bet
            int suggestedBet = CalculateSuggestedBet(spread, Players[_currentPlayer].Chips);
            _suggestedBetLabel.Text = $"Suggested: ${suggestedBet}";

            if (statusBarToolStripMenuItem.Checked)
            {
                //speak spread, odds, and suggested bet
                _cardTalk.SpeakAsync(_spreadLabel.Text);
                _cardTalk.SpeakAsync(_oddsLabel.Text);
                _cardTalk.SpeakAsync(_suggestedBetLabel.Text);
            }

            // Update current bet display
            UpdateCurrentBetDisplay();
        }

        /// <summary>
        /// Gets the color for spread display based on favorability
        /// </summary>
        private Color GetSpreadColor(int spread)
        {
            if (spread >= 9) return Color.LightGreen;
            if (spread >= 6) return Color.Yellow;
            if (spread >= 4) return Color.Orange;
            return Color.LightCoral;
        }

        /// <summary>
        /// Calculates a suggested bet amount based on odds and chips
        /// </summary>
        private int CalculateSuggestedBet(int spread, int chips)
        {
            double multiplier;

            if (spread >= 9)
                multiplier = 0.5;
            else if (spread >= 6)
                multiplier = 0.3;
            else if (spread >= 4)
                multiplier = 0.2;
            else if (spread >= 2)
                multiplier = 0.1;
            else
                return MinimumBet;

            int suggested = (int)(chips * multiplier);
            suggested = Math.Max(MinimumBet, (suggested / MinimumBet) * MinimumBet);
            return Math.Min(suggested, chips);
        }

        /// <summary>
        /// Updates the current bet display label
        /// </summary>
        private void UpdateCurrentBetDisplay()
        {
            _currentBetLabel.Text = $"${_currentBetAmount}";
        }

        /// <summary>
        /// Sets quick bet amount
        /// </summary>
        private void SetQuickBet(int betType)
        {
            if (_currentPlayer < 0 || _currentPlayer >= _playerCount || !_placeBetButton.Enabled)
                return;

            int chips = Players[_currentPlayer].Chips;
            int betValue;

            switch (betType)
            {
                case 0: // MIN
                    betValue = MinimumBet;
                    break;
                case 1: // 10%
                    betValue = Math.Max(MinimumBet, chips / 10);
                    betValue = (betValue / MinimumBet) * MinimumBet;
                    break;
                case 2: // 25%
                    betValue = Math.Max(MinimumBet, chips / 4);
                    betValue = (betValue / MinimumBet) * MinimumBet;
                    break;
                case 3: // 50%
                    betValue = Math.Max(MinimumBet, chips / 2);
                    betValue = (betValue / MinimumBet) * MinimumBet;
                    break;
                case 4: // 75%
                    betValue = Math.Max(MinimumBet, (chips * 3) / 4);
                    betValue = (betValue / MinimumBet) * MinimumBet;
                    break;
                case 5: // MAX
                    betValue = chips;
                    break;
                default:
                    betValue = MinimumBet;
                    break;
            }

            _currentBetAmount = Math.Min(betValue, chips);
            UpdateCurrentBetDisplay();
        }

        /// <summary>
        /// Handles keyboard shortcuts for quick betting
        /// </summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_placeBetButton.Enabled || _currentPlayer < 0 || _currentPlayer >= _playerCount)
                return;

            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    SetQuickBet(0); // MIN
                    e.Handled = true;
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    SetQuickBet(1); // 10%
                    e.Handled = true;
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    SetQuickBet(2); // 25%
                    e.Handled = true;
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    SetQuickBet(3); // 50%
                    e.Handled = true;
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    SetQuickBet(4); // 75%
                    e.Handled = true;
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    SetQuickBet(5); // MAX
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    if (_placeBetButton.Enabled)
                    {
                        _placeBetButton.PerformClick();
                        e.Handled = true;
                    }
                    break;
            }
        }
        #endregion

        #region Card Management
        /// <summary>
        /// Places the left and right cards on the board
        /// </summary>
        private void PlaceCards()
        {
            var card1 = _gameDeck.GetNextCard();
            var card2 = _gameDeck.GetNextCard();

            // Validate cards were drawn successfully
            if (card1 == null || card2 == null)
            {
                _leftCard = null;
                _rightCard = null;
                return;
            }

            // Put the lower ranked card on the left and the higher ranked card on the right
            if (GetCardValue(card1.Rank) < GetCardValue(card2.Rank))
            {
                _leftCard = card1;
                _rightCard = card2;
            }
            else
            {
                _leftCard = card2;
                _rightCard = card1;
            }

            PlaceCardImage(LeftCardPB, _leftCard.Rank, _leftCard.Suit);
            PlaceCardImage(RightCardPB, _rightCard.Rank, _rightCard.Suit);
        }

        /// <summary>
        /// Displays a card image in a picture box
        /// </summary>
        private void PlaceCardImage(PictureBox pictureBox, char rank, char suit)
        {
            if (pictureBox == null)
            {
                throw new ArgumentNullException(nameof(pictureBox));
            }

            try
            {
                string imagePath = GetImagePath(rank, suit);
                if (File.Exists(imagePath))
                {
                    pictureBox.ImageLocation = imagePath;

                    string rankName = GetRankName(rank);
                    string suitName = GetSuitName(suit);

                    // Only set valid accessibility description
                    if (rankName != "Unknown" && suitName != "Unknown")
                    {
                        pictureBox.AccessibleDescription = $"{rankName} of {suitName}";
                    }

                    if (cardsToolStripMenuItem.Checked == true && rankName != "Unknown")
                    {
                        _cardTalk.SpeakAsync(rankName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading card image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Resets all cards to show card backs
        /// </summary>
        private void ResetCards()
        {
            // Clear card references
            _leftCard = null;
            _centerCard = null;
            _rightCard = null;

            try
            {
                string cardBackImage = Path.Combine(_resourceDirectory, "CardBack.png");
                if (File.Exists(cardBackImage))
                {
                    LeftCardPB.ImageLocation = cardBackImage;
                    CenterCardPB.ImageLocation = cardBackImage;
                    RightCardPB.ImageLocation = cardBackImage;
                }
                else
                {
                    MessageBox.Show($"Card back image not found: {cardBackImage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting cards: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Save/Load Game
        /// <summary>
        /// Saves the current game state to disk
        /// </summary>
        private void SaveGame()
        {
            try
            {
                Directory.CreateDirectory(_appDirectory);

                using (StreamWriter writer = new StreamWriter(_saveGameFile))
                {
                    writer.WriteLine($"PlayerCount={_playerCount}");
                    writer.WriteLine($"CurrentPlayer={_currentPlayer}");
                    writer.WriteLine($"IsGameInProgress={_isGameInProgress}");

                    for (int i = 0; i < _playerCount; i++)
                    {
                        writer.WriteLine($"Player{i}.Name={Players[i].Name}");
                        writer.WriteLine($"Player{i}.Type={Players[i].Type}");
                        writer.WriteLine($"Player{i}.Chips={Players[i].Chips}");
                        writer.WriteLine($"Player{i}.Wins={Players[i].Wins}");
                        writer.WriteLine($"Player{i}.Losses={Players[i].Losses}");
                        writer.WriteLine($"Player{i}.WinStreak={Players[i].WinStreak}");
                        writer.WriteLine($"Player{i}.WinPercentage={Players[i].WinPercentage}");
                    }

                    // Save current bet amount
                    writer.WriteLine($"CurrentBetAmount={_currentBetAmount}");

                    // Save card state BEFORE deck state
                    bool hasLeftCard = _leftCard != null;
                    bool hasCenterCard = _centerCard != null;
                    bool hasRightCard = _rightCard != null;

                    writer.WriteLine($"HasLeftCard={hasLeftCard}");
                    writer.WriteLine($"LeftCard.Rank={(_leftCard != null ? _leftCard.Rank.ToString() : "")}");
                    writer.WriteLine($"LeftCard.Suit={(_leftCard != null ? _leftCard.Suit.ToString() : "")}");

                    writer.WriteLine($"HasCenterCard={hasCenterCard}");
                    writer.WriteLine($"CenterCard.Rank={(_centerCard != null ? _centerCard.Rank.ToString() : "")}");
                    writer.WriteLine($"CenterCard.Suit={(_centerCard != null ? _centerCard.Suit.ToString() : "")}");

                    writer.WriteLine($"HasRightCard={hasRightCard}");
                    writer.WriteLine($"RightCard.Rank={(_rightCard != null ? _rightCard.Rank.ToString() : "")}");
                    writer.WriteLine($"RightCard.Suit={(_rightCard != null ? _rightCard.Suit.ToString() : "")}");

                    // Save deck state AFTER cards (deck should already have these cards removed)
                    writer.WriteLine($"DeckState={_gameDeck.GetDeckState()}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads a saved game state from disk
        /// </summary>
        private void LoadGame()
        {
            try
            {
                if (!File.Exists(_saveGameFile))
                {
                    return;
                }

                var lines = File.ReadAllLines(_saveGameFile);
                var data = new Dictionary<string, string>();

                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { '=' }, 2);
                    if (parts.Length == 2)
                    {
                        data[parts[0].Trim()] = parts[1].Trim();
                    }
                }

                if (data.TryGetValue("PlayerCount", out string playerCountStr) && int.TryParse(playerCountStr, out int playerCount))
                {
                    _playerCount = playerCount;
                }
                else
                {
                    return;
                }

                if (data.TryGetValue("CurrentPlayer", out string currentPlayerStr) && int.TryParse(currentPlayerStr, out int currentPlayer))
                {
                    _currentPlayer = currentPlayer;
                }

                if (data.TryGetValue("IsGameInProgress", out string isGameInProgressStr) && bool.TryParse(isGameInProgressStr, out bool isGameInProgress))
                {
                    _isGameInProgress = isGameInProgress;
                }

                // Load current bet amount
                if (data.TryGetValue("CurrentBetAmount", out string currentBetAmountStr) && int.TryParse(currentBetAmountStr, out int currentBetAmount))
                {
                    _currentBetAmount = currentBetAmount;
                }

                Players = new PlayerInfo[6];
                for (int i = 0; i < _playerCount; i++)
                {
                    Players[i] = new PlayerInfo
                    {
                        Name = data.TryGetValue($"Player{i}.Name", out string name) ? name : "",
                        Type = data.TryGetValue($"Player{i}.Type", out string type) ? type : "Computer",
                        Chips = data.TryGetValue($"Player{i}.Chips", out string chips) && int.TryParse(chips, out int chipsValue) ? chipsValue : 100,
                        Wins = data.TryGetValue($"Player{i}.Wins", out string wins) && int.TryParse(wins, out int winsValue) ? winsValue : 0,
                        Losses = data.TryGetValue($"Player{i}.Losses", out string losses) && int.TryParse(losses, out int lossesValue) ? lossesValue : 0,
                        WinStreak = data.TryGetValue($"Player{i}.WinStreak", out string winStreak) && int.TryParse(winStreak, out int winStreakValue) ? winStreakValue : 0,
                        WinPercentage = data.TryGetValue($"Player{i}.WinPercentage", out string winPct) && double.TryParse(winPct, out double winPctValue) ? winPctValue : 0
                    };
                }

                // Load deck state first
                if (data.TryGetValue("DeckState", out string deckState))
                {
                    _gameDeck = new Deck(_rng);
                    _gameDeck.SetDeckState(deckState);
                }

                // Reset cards first
                _leftCard = null;
                _centerCard = null;
                _rightCard = null;

                // Load drawn cards - check HasCard flags to determine if cards should be displayed
                bool hasLeftCard = data.TryGetValue("HasLeftCard", out string hasLeftCardStr) && bool.Parse(hasLeftCardStr);
                bool hasCenterCard = data.TryGetValue("HasCenterCard", out string hasCenterCardStr) && bool.Parse(hasCenterCardStr);
                bool hasRightCard = data.TryGetValue("HasRightCard", out string hasRightCardStr) && bool.Parse(hasRightCardStr);

                if (hasLeftCard &&
                    data.TryGetValue("LeftCard.Rank", out string leftRank) && !string.IsNullOrEmpty(leftRank) &&
                    data.TryGetValue("LeftCard.Suit", out string leftSuit) && !string.IsNullOrEmpty(leftSuit))
                {
                    _leftCard = new Card(leftRank[0], leftSuit[0]);
                    PlaceCardImage(LeftCardPB, _leftCard.Rank, _leftCard.Suit);
                }
                else
                {
                    // Show card back
                    string cardBackImage = Path.Combine(_resourceDirectory, "CardBack.png");
                    if (File.Exists(cardBackImage))
                    {
                        LeftCardPB.ImageLocation = cardBackImage;
                    }
                }

                if (hasCenterCard &&
                    data.TryGetValue("CenterCard.Rank", out string centerRank) && !string.IsNullOrEmpty(centerRank) &&
                    data.TryGetValue("CenterCard.Suit", out string centerSuit) && !string.IsNullOrEmpty(centerSuit))
                {
                    _centerCard = new Card(centerRank[0], centerSuit[0]);
                    PlaceCardImage(CenterCardPB, _centerCard.Rank, _centerCard.Suit);
                }
                else
                {
                    // Show card back
                    string cardBackImage = Path.Combine(_resourceDirectory, "CardBack.png");
                    if (File.Exists(cardBackImage))
                    {
                        CenterCardPB.ImageLocation = cardBackImage;
                    }
                }

                if (hasRightCard &&
                    data.TryGetValue("RightCard.Rank", out string rightRank) && !string.IsNullOrEmpty(rightRank) &&
                    data.TryGetValue("RightCard.Suit", out string rightSuit) && !string.IsNullOrEmpty(rightSuit))
                {
                    _rightCard = new Card(rightRank[0], rightSuit[0]);
                    PlaceCardImage(RightCardPB, _rightCard.Rank, _rightCard.Suit);
                }
                else
                {
                    // Show card back
                    string cardBackImage = Path.Combine(_resourceDirectory, "CardBack.png");
                    if (File.Exists(cardBackImage))
                    {
                        RightCardPB.ImageLocation = cardBackImage;
                    }
                }

                UpdatePlayerDisplay();
                UpdateStatus($"Game loaded! {_playerCount} players. {Players[_currentPlayer].Name}'s turn.");

                if (_isGameInProgress)
                {
                    _ = StartGameLoopAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading saved game: {ex.Message}\n\nStarting fresh.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                try
                {
                    if (File.Exists(_saveGameFile))
                    {
                        File.Delete(_saveGameFile);
                    }
                }
                catch { }
            }
        }
        #endregion

        #region AI Logic
        /// <summary>
        /// Calculates the bet amount for a computer player
        /// </summary>
        private int CalculateAIBet(int leftValue, int rightValue)
        {
            // Validate current player index
            if (_currentPlayer < 0 || _currentPlayer >= _playerCount)
            {
                return MinimumBet;
            }

            int spread = rightValue - leftValue - 1;
            int currentChips = Players[_currentPlayer].Chips;
            int betAmount = 0;

            // AI betting strategy based on spread
            if (spread >= VeryFavorableSpread) // Very favorable odds
            {
                betAmount = (int)(currentChips * (VeryFavorableMultiplierMin + _rng.NextDouble() * VeryFavorableMultiplierMax));
            }
            else if (spread >= GoodSpread) // Good odds
            {
                betAmount = (int)(currentChips * (0.25 + _rng.NextDouble() * 0.15));
            }
            else if (spread >= DecentSpread) // Decent odds
            {
                betAmount = (int)(currentChips * (0.15 + _rng.NextDouble() * 0.1));
            }
            else if (spread >= 2) // Poor odds
            {
                betAmount = _rng.NextDouble() < 0.6 ? MinimumBet : 0;
            }
            else // Very poor odds
            {
                betAmount = _rng.NextDouble() < 0.2 ? MinimumBet : 0;
            }

            // Win streak bonus
            if (Players[_currentPlayer].WinStreak >= 3 && spread >= 4)
            {
                betAmount = (int)(betAmount * 1.3);
            }

            // Desperation factor (low on chips)
            if (currentChips <= 50 && spread >= 5)
            {
                betAmount = currentChips;
            }

            // Ensure bet is within valid range
            betAmount = Math.Max(0, Math.Min(betAmount, currentChips));

            // Round to nearest $10
            if (betAmount > 0 && betAmount >= MinimumBet)
            {
                betAmount = Math.Max(MinimumBet, (betAmount / MinimumBet) * MinimumBet);
            }

            return betAmount;
        }
        #endregion

        #region UI Updates
        /// <summary>
        /// Updates the player display grid
        /// </summary>
        public void UpdatePlayerDisplay()
        {
            try
            {
                PlayersDGV.Rows.Clear();

                for (int i = 0; i < _playerCount; i++)
                {
                    if (!string.IsNullOrEmpty(Players[i].Name))
                    {
                        string winPercentage = Players[i].WinPercentage > 0 ? $"{Players[i].WinPercentage:F1}%" : "0.0%";

                        PlayersDGV.Rows.Add(
                            Players[i].Name,
                            Players[i].Type,
                            $"${Players[i].Chips}",
                            Players[i].Wins,
                            Players[i].Losses,
                            winPercentage
                        );
                    }
                }

                // Safe selection and scrolling with bounds checking
                if (_currentPlayer >= 0 && _currentPlayer < _playerCount && PlayersDGV.Rows.Count > _currentPlayer)
                {
                    PlayersDGV.ClearSelection();
                    PlayersDGV.Rows[_currentPlayer].Selected = true;

                    // Safe scrolling
                    try
                    {
                        if (PlayersDGV.Rows.Count > 0 && _currentPlayer < PlayersDGV.Rows.Count)
                        {
                            PlayersDGV.FirstDisplayedScrollingRowIndex = _currentPlayer;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // Scrolling failed, but not critical
                    }
                }

                // Color-code chip status
                for (int i = 0; i < PlayersDGV.Rows.Count; i++)
                {
                    var row = PlayersDGV.Rows[i];
                    int chips = Players[i].Chips;

                    // Color-code based on chip count
                    if (chips == 0)
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                    else if (chips < 50)
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                    else if (chips > 200)
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    else
                        row.DefaultCellStyle.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                System.Diagnostics.Debug.WriteLine($"Error updating player display: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the status message
        /// </summary>
        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), message);
                return;
            }

            StatusTB.Text = message;
            if (statusBarToolStripMenuItem.Checked)
            {
                try
                {
                    _cardTalk.SpeakAsync(message);
                }
                catch (Exception)
                {
                    // Speech failed, but don't crash
                }
            }
        }

        /// <summary>
        /// Moves to the next player with chips remaining
        /// </summary>
        private void MoveToNextPlayer()
        {
            int playersChecked = 0;

            do
            {
                _currentPlayer++;

                if (_currentPlayer >= _playerCount)
                {
                    _currentPlayer = 0;
                }

                playersChecked++;

                if (playersChecked >= _playerCount)
                {
                    // All players checked, none have chips
                    return;
                }
            }
            while (_currentPlayer < _playerCount && Players[_currentPlayer].Chips <= 0);

            UpdatePlayerDisplay();
        }
        #endregion

        #region Card Utilities
        /// <summary>
        /// Checks if a card rank falls between two other ranks
        /// </summary>
        private bool IsCardBetween(char centerRank, char leftRank, char rightRank)
        {
            int centerValue = GetCardValue(centerRank);
            int leftValue = GetCardValue(leftRank);
            int rightValue = GetCardValue(rightRank);

            return centerValue > leftValue && centerValue < rightValue;
        }

        /// <summary>
        /// Gets the numeric value of a card rank
        /// </summary>
        private int GetCardValue(char rank)
        {
            return CardValues.TryGetValue(rank, out int value) ? value : 0;
        }

        /// <summary>
        /// Gets the file path for a card image
        /// </summary>
        private string GetImagePath(char rank, char suit)
        {
            string key = $"{rank}{suit}";
            return _imagePathCache.TryGetValue(key, out string path) ? path : Path.Combine(_resourceDirectory, $"{rank}-{suit}.png");
        }
        #endregion

        #region Speech
        /// <summary>
        /// Speaks the name of a card
        /// </summary>
        private void SpeakCard(char rank)
        {
            string rankName = GetRankName(rank);
            string cardName = $"{rankName}";

            try
            {
                _cardTalk.SpeakAsync(cardName);
            }
            catch (Exception)
            {
                // Speech failed, but don't crash
            }
        }

        /// <summary>
        /// Gets the spoken name of a rank
        /// </summary>
        private string GetRankName(char rank)
        {
            return RankNames.TryGetValue(rank, out string name) ? name : "Unknown";
        }

        /// <summary>
        /// Gets the spoken name of a suit
        /// </summary>
        private string GetSuitName(char suit)
        {
            return SuitNames.TryGetValue(suit, out string name) ? name : "Unknown";
        }

        private void SafeSpeak(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            try
            {
                if (_cardTalk.State == SynthesizerState.Ready || _cardTalk.State == SynthesizerState.Speaking)
                {
                    _cardTalk.SpeakAsync(message);
                }
            }
            catch (ObjectDisposedException)
            {
                // SpeechSynthesizer disposed
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Speech error: {ex.Message}");
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the Place Bet button click
        /// </summary>
        private void PlaceBetButton_Click(object sender, EventArgs e)
        {
            int betAmount = _currentBetAmount;

            // Disable controls
            _placeBetButton.Enabled = false;

            // Complete the TaskCompletionSource to resume the game loop
            if (_humanBetTcs != null && !_humanBetTcs.Task.IsCompleted)
            {
                _humanBetTcs.SetResult(betAmount);
            }
        }

        /// <summary>
        /// Sets the bet to the maximum available chips
        /// </summary>
        private void placeMaximumBetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentPlayer >= 0 && _currentPlayer < _playerCount && Players[_currentPlayer].Chips > 0)
            {
                SetQuickBet(4); // MAX bet
            }
        }

        private void chipsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var player in Players.Take(_playerCount))
                {
                    if (player.Chips > 0 && !string.IsNullOrEmpty(player.Name))
                    {
                        string message = $"{player.Name} has {player.Chips} chips.";
                        _cardTalk.SpeakAsync(message);
                    }
                }

                var richestPlayer = Players.Take(_playerCount).OrderByDescending(p => p.Chips).FirstOrDefault();
                if (!string.IsNullOrEmpty(richestPlayer.Name))
                {
                    string message = $"{richestPlayer.Name} is the chip leader.";
                    _cardTalk.SpeakAsync(message);
                }
            }
            catch (Exception)
            {
                // Speech failed, but don't crash
            }
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusBarToolStripMenuItem.Checked = !statusBarToolStripMenuItem.Checked;
        }

        private void cardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cardsToolStripMenuItem.Checked = !cardsToolStripMenuItem.Checked;
        }

        private void SaveSpeechSettings()
        {
            try
            {
                Directory.CreateDirectory(_appDirectory);
                File.WriteAllText(_speechFile, $"StatusBar={statusBarToolStripMenuItem.Checked}\nCards={cardsToolStripMenuItem.Checked}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving speech settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSpeechSettings()
        {
            try
            {
                if (File.Exists(_speechFile))
                {
                    var lines = File.ReadAllLines(_speechFile);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("StatusBar="))
                        {
                            statusBarToolStripMenuItem.Checked = line.Split('=')[1].Trim() == "True";
                        }
                        else if (line.StartsWith("Cards="))
                        {
                            cardsToolStripMenuItem.Checked = line.Split('=')[1].Trim() == "True";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading speech settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSpeechSettings();
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSpeechSettings();
            if (_isGameInProgress)
            {
                SaveGame();
            }

            // Dispose of SpeechSynthesizer
            _cardTalk?.Dispose();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "In Between Card Game\n\n" +
                "Version 1.0\n\n" +
                "A casino-style card betting game where you bet on whether the third card will fall between the first two cards.\n\n" +
                "© 2026",
                "About In Between",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        #endregion

        #region Logging
        private void SaveFullLog()
        {
            try
            {
                Directory.CreateDirectory(_appDirectory);

                bool fileExists = File.Exists(_FullLog);

                using (var writer = new StreamWriter(_FullLog, true))
                {
                    // Write header only if file is new
                    if (!fileExists)
                    {
                        for (int i = 0; i < PlayersDGV.Columns.Count; i++)
                        {
                            writer.Write(PlayersDGV.Columns[i].HeaderText);
                            if (i < PlayersDGV.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();
                    }

                    // Write game timestamp
                    writer.WriteLine($"Game Ended: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                    // Write rows
                    for (int i = 0; i < PlayersDGV.Rows.Count; i++)
                    {
                        for (int j = 0; j < PlayersDGV.Columns.Count; j++)
                        {
                            writer.Write(PlayersDGV.Rows[i].Cells[j].Value);
                            if (j < PlayersDGV.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();
                    }

                    // Add separator
                    writer.WriteLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving full log: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveLastGameLog()
        {
            try
            {
                Directory.CreateDirectory(_appDirectory);

                using (var writer = new StreamWriter(_LastGameLog))
                {
                    writer.WriteLine($"Game Ended: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine();

                    // Determine column width based on longest player name
                    int nameColumnWidth = 15;
                    for (int i = 0; i < _playerCount; i++)
                    {
                        if (!string.IsNullOrEmpty(Players[i].Name))
                        {
                            nameColumnWidth = Math.Max(nameColumnWidth, Players[i].Name.Length + 2);
                        }
                    }

                    // Write header row with player names
                    writer.Write("Stat".PadRight(15));
                    for (int i = 0; i < _playerCount; i++)
                    {
                        if (!string.IsNullOrEmpty(Players[i].Name))
                        {
                            writer.Write(Players[i].Name.PadRight(nameColumnWidth));
                        }
                    }
                    writer.WriteLine();

                    // Write separator line
                    writer.Write(new string('-', 15));
                    for (int i = 0; i < _playerCount; i++)
                    {
                        if (!string.IsNullOrEmpty(Players[i].Name))
                        {
                            writer.Write(new string('-', nameColumnWidth));
                        }
                    }
                    writer.WriteLine();

                    // Write Type row
                    writer.Write("Type".PadRight(15));
                    for (int i = 0; i < _playerCount; i++)
                    {
                        if (!string.IsNullOrEmpty(Players[i].Name))
                        {
                            writer.Write(Players[i].Type.PadRight(nameColumnWidth));
                        }
                    }
                    writer.WriteLine();

                    // Write Chips row
                    writer.Write("Chips".PadRight(15));
                    for (int i = 0; i < _playerCount; i++)
                    {
                        if (!string.IsNullOrEmpty(Players[i].Name))
                        {
                            writer.Write(($"${Players[i].Chips}").PadRight(nameColumnWidth));
                        }
                    }
                    writer.WriteLine();

                    // Write Wins row
                    writer.Write("Wins".PadRight(15));
                    for (int i = 0; i < _playerCount; i++)
                    {
                        if (!string.IsNullOrEmpty(Players[i].Name))
                        {
                            writer.Write(Players[i].Wins.ToString().PadRight(nameColumnWidth));
                        }
                    }
                    writer.WriteLine();

                    // Write Losses row
                    writer.Write("Losses".PadRight(15));
                    for (int i = 0; i < _playerCount; i++)
                    {
                        if (!string.IsNullOrEmpty(Players[i].Name))
                        {
                            writer.Write(Players[i].Losses.ToString().PadRight(nameColumnWidth));
                        }
                    }
                    writer.WriteLine();

                    // Write Win % row
                    writer.Write("Win %".PadRight(15));
                    for (int i = 0; i < _playerCount; i++)
                    {
                        if (!string.IsNullOrEmpty(Players[i].Name))
                        {
                            string winPercentage = Players[i].WinPercentage > 0 ? $"{Players[i].WinPercentage:F1}%" : "0.0%";
                            writer.Write(winPercentage.PadRight(nameColumnWidth));
                        }
                    }
                    writer.WriteLine();

                    // Write Win Streak row
                    writer.Write("Win Streak".PadRight(15));
                    for (int i = 0; i < _playerCount; i++)
                    {
                        if (!string.IsNullOrEmpty(Players[i].Name))
                        {
                            writer.Write(Players[i].WinStreak.ToString().PadRight(nameColumnWidth));
                        }
                    }
                    writer.WriteLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving last game log: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region AI Betting Constants
        private const int VeryFavorableSpread = 9;
        private const int GoodSpread = 6;
        private const int DecentSpread = 4;
        private const int PoorSpread = 2;
        private const double VeryFavorableMultiplierMin = 0.4;
        private const double VeryFavorableMultiplierMax = 0.2;
        private const double GoodMultiplierMin = 0.15;
        private const double GoodMultiplierMax = 0.3;
        private const double DecentMultiplierMin = 0.1;
        private const double DecentMultiplierMax = 0.2;
        private const double PoorMultiplierMin = 0.05;
        private const double PoorMultiplierMax = 0.1;
        #endregion

        #region Utility Methods
        private void LogDebug(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
#endif
        }

        private void HighlightBetResult(bool isWinner)
        {
            Color originalColor = StatusTB.BackColor;
            StatusTB.BackColor = isWinner ? Color.LightGreen : Color.LightCoral;

            System.Windows.Forms.Timer flashTimer = new System.Windows.Forms.Timer { Interval = 1500 };
            flashTimer.Tick += (s, e) =>
            {
                StatusTB.BackColor = originalColor;
                flashTimer.Stop();
                flashTimer.Dispose();
            };
            flashTimer.Start();
        }
        #endregion
    }
}