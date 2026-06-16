# In Between Card Game

A Windows Forms implementation of the classic casino card game "In Between" (also known as Acey-Deucey), featuring AI opponents, speech synthesis, and a casino-style interface.

## 🎮 Game Overview

In Between is a betting card game where players wager that a third card drawn will fall between two initial cards. The game features:

- **Classic gameplay** with proper betting rules
- **AI opponents** with strategic betting algorithms
- **Casino-style interface** with felt texture and card animations
- **Speech synthesis** for card announcements and game events
- **Save/Load functionality** to continue games later
- **Statistical tracking** for wins, losses, and win streaks

## 🎯 How to Play

1. **Starting the Game**: Begin with 100 chips (configurable)
2. **Two cards are dealt**: One on the left, one on the right
3. **Place your bet**: Wager chips that the next card will fall between the two cards
4. **Win or Lose**: 
   - **Win**: The center card's value is between the left and right cards
   - **Lose**: The center card matches or falls outside the range
   - **Auto-skip**: Consecutive or matching cards automatically skip the turn

### Betting Rules

- **Minimum bet**: $10
- **Maximum bet**: Your total chip count
- **Quick bet options**: Min, 10, 25, 50, 75, or Max
- **Odds calculator**: Shows your winning probability based on the spread

## 🖥️ System Requirements

- **OS**: Windows 7 or later
- **Framework**: .NET Framework 4.8.1
- **IDE**: Visual Studio 2022 or later (for development)

## 🚀 Getting Started

### Running the Game

1. Clone the repository:
   ```bash
   git clone https://github.com/HiTechCharles/InBetweenGame.git
   ```

2. Open `InBetweenGame.sln` in Visual Studio

3. Build and run the solution (F5)

### First Time Setup

- The game will create a folder in your Documents directory: `Documents\InBetween`
- This folder stores:
  - Save games (`savegame.dat`)
  - Game logs (`Full Log.csv`, `last game.txt`)
  - Speech settings (`speech.ini`)

## 🎲 Features

### Gameplay Features
- **Multiple AI opponents**: Play against up to 5 computer players
- **Smart AI betting**: Computer players use strategic algorithms based on card spreads
- **Turn management**: Automatic turn rotation with visual indicators
- **Deck management**: Proper shuffling and card tracking

### Visual Features
- **Casino-style felt background**: Textured green felt for authentic casino feel
- **Card animations**: Smooth card displays and transitions
- **Bet control panel**: Modern UI with odds calculator and quick bet buttons
- **Player statistics grid**: Real-time tracking of chips, wins, losses, and streaks

### Audio Features
- **Text-to-speech**: Cards and game events are announced
- **Configurable speech**: Adjust rate and volume or disable entirely
- **Speech settings persistence**: Preferences saved between sessions

### Data Features
- **Save/Load games**: Resume your game anytime
- **Game logging**: Detailed CSV logs of all hands played
- **Statistical tracking**: Win/loss ratios and streak tracking
- **Player profiles**: Different player types (Human/Computer)

## 🎨 Game Interface

The interface includes:
- **Three card positions**: Left, Center (revealed after bet), and Right
- **Player data grid**: Shows all players' chips, wins, losses, and statistics
- **Bet control panel**: 
  - Current bet display
  - Spread indicator
  - Win odds percentage
  - Quick bet buttons
  - Place bet confirmation
- **Status bar**: Game messages and turn information

## 🔧 Development

### Project Structure

```
InBetweenGame/
├── Form1.cs              # Main game form and logic
├── Form1.Designer.cs     # UI designer code
├── Card.cs               # Card and Deck classes
├── NewGame.cs            # New game setup dialog
├── Program.cs            # Application entry point
├── Resources/            # Card images (52 cards + backs)
│   ├── A-H.png          # Ace of Hearts
│   ├── K-S.png          # King of Spades
│   └── ...              # All 52 playing cards
└── Properties/           # Assembly info and settings
```

### Key Classes

- **Form1**: Main game form, handles gameplay loop, UI, and player management
- **Card**: Represents a playing card with rank and suit
- **Deck**: Manages a 52-card deck with shuffling and dealing
- **PlayerInfo**: Struct containing player data (chips, wins, losses, stats)

### Technologies Used

- **C# / .NET Framework 4.8.1**
- **Windows Forms**: UI framework
- **System.Speech.Synthesis**: Text-to-speech functionality
- **GDI+**: Graphics rendering for felt texture and visual effects

## 📊 Game Statistics

The game tracks:
- Total chips for each player
- Number of wins and losses
- Win percentage
- Current win streak
- Historical game logs in CSV format

## 🎤 Speech Synthesis

The game uses Windows Speech Synthesis to announce:
- Cards being dealt
- Player turns
- Bet results
- Game events

Speech can be configured via `Documents\InBetween\speech.ini`:
```ini
Rate=4
Volume=100
Enabled=True
```

## 💾 Save Game Format

Games are saved to `Documents\InBetween\savegame.dat` containing:
- Player count and current player
- All player data (chips, names, types, statistics)
- Current game state
- Deck configuration

## 🤝 Contributing

Contributions are welcome! Feel free to:
- Report bugs via GitHub Issues
- Suggest new features
- Submit pull requests

## 📝 License

This project is open source and available for personal and educational use.

## 👨‍💻 Author

**HiTechCharles**
- GitHub: [@HiTechCharles](https://github.com/HiTechCharles)

## 🙏 Acknowledgments

- Card images included in Resources folder
- Speech synthesis powered by Microsoft Speech API
- Inspired by the classic casino card game "Acey-Deucey"

## 📸 Screenshots

*(Add screenshots of your game here)*

---

**Enjoy the game and may the odds be ever in your favor!** 🎰🃏
