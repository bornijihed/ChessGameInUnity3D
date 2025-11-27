# 3D Chess Game

A fully-functional 3D chess game built in Unity with an immersive rhythmic visual style, complete chess rules, and dynamic camera perspectives.

![Unity](https://img.shields.io/badge/Unity-2020%2B-blue)
![C#](https://img.shields.io/badge/C%23-9.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

## Features

### Core Gameplay
- ✅ **Complete Chess Rules**: All standard chess piece movements (Pawn, Rook, Knight, Bishop, Queen, King)
- ✅ **Move Validation**: Legal move checking with path obstruction detection
- ✅ **Pawn Promotion**: Choose between Queen, Rook, Bishop, or Knight when pawns reach the last rank
- ✅ **Capture Mechanics**: Proper piece capturing with visual feedback
- ✅ **Turn-Based System**: Automatic turn switching between White and Black players

### Visual Experience
- 🎨 **Color-Coded Pieces**: Each piece type has unique accent colors for easy identification
  - Pawn: Green
  - Rook: Red
  - Knight: Orange
  - Bishop: Purple
  - Queen: Gold
  - King: Blue
- 🎭 **Detailed 3D Models**: Multi-component procedural models with metallic materials
- 🌟 **Rhythmic Background**: Pulsing animated background with ambient atmosphere
- ✨ **Visual Feedback**: 
  - Glowing green tiles show valid moves for selected pieces
  - Smooth piece animations with lerp movement
  - Subtle breathing animations on idle pieces
  - Tile hover effects with pulsing highlights

### Camera System
- 📹 **Dynamic Perspective**: Camera automatically rotates to show each player's viewpoint
- 🔄 **Smooth Transitions**: Animated camera movement between White and Black perspectives
- 👁️ **Optimized Angles**: Angled top-down view for better piece visibility

### UI & Information
- 📊 **Score Tracking**: Real-time score based on captured piece values
- ⏱️ **Timer System**: Individual time tracking for each player
- 📈 **Move Counter**: Separate move count for White and Black
- 🎯 **Move Indicators**: Interactive visual guides for valid moves

## Controls

### Piece Selection & Movement
1. **Click** on a piece to select it
2. **Green tiles** appear showing all valid moves
3. **Click** on a highlighted tile to move the piece

### Pawn Promotion
When a pawn reaches the opposite end:
- Press **Q** for Queen
- Press **R** for Rook
- Press **B** for Bishop
- Press **N** for Knight

## Installation

### Prerequisites
- Unity 2020.3 or later
- Basic understanding of Unity Editor

### Setup
1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/3d-chess-game.git
   ```

2. Open the project in Unity Editor

3. Open the main scene (if not already open)

4. Create an empty GameObject in the scene

5. Add the `GameManager` component to it

6. Press **Play** to start the game

## Project Structure

```
racing/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── ChessPiece.cs          # Base piece class with enums
│   │   │   ├── ChessBoard.cs          # Board logic and state management
│   │   │   ├── GameManager.cs         # Main game controller
│   │   │   ├── MoveValidator.cs       # Chess rules validation
│   │   │   ├── InputController.cs     # Mouse input and raycasting
│   │   │   ├── CameraController.cs    # Dynamic camera rotation
│   │   │   └── PieceValues.cs         # Piece scoring system
│   │   └── Visuals/
│   │       ├── BoardVisualizer.cs     # 3D board and piece generation
│   │       ├── PieceObject.cs         # Piece animations
│   │       ├── TileObject.cs          # Tile highlighting
│   │       └── RhythmicBackground.cs  # Animated background
│   ├── Materials/                      # (Auto-generated)
│   └── Prefabs/                        # (Future custom models)
└── README.md
```

## Technical Details

### Architecture
- **Separation of Concerns**: Clear separation between game logic (Core) and visuals (Visuals)
- **Component-Based**: Modular design using Unity's component system
- **MVC Pattern**: GameManager coordinates between Board (Model) and Visualizer (View)

### Key Systems

**Move Validation**
- Static validator class for efficient rule checking
- Path obstruction detection for sliding pieces (Rook, Bishop, Queen)
- Special move handling (Pawn initial double move, diagonal captures)

**Visual Rendering**
- Procedural mesh generation using Unity primitives
- Standard shader with metallic materials
- Real-time color lerping for animations
- Composite piece models (10+ primitives for King/Queen)

**Performance**
- Efficient grid-based coordinate system (8x8 array)
- Object pooling for piece management
- Minimal runtime allocations

## Customization

### Piece Colors
Modify accent colors in `BoardVisualizer.CreateEnhancedPiece()`:
```csharp
case PieceType.Pawn:
    accentColor = team == TeamColor.White 
        ? new Color(0.7f, 0.9f, 0.7f)  // Green for White
        : new Color(0.2f, 0.4f, 0.2f); // Dark green for Black
```

### Camera Settings
Adjust camera positions in `CameraController.cs`:
```csharp
private Vector3 whiteViewPosition = new Vector3(3.5f, 8, -3);
private Vector3 blackViewPosition = new Vector3(3.5f, 8, 10.5f);
```

### Background Animation
Modify pulsing speed in `RhythmicBackground.cs`:
```csharp
public float pulseSpeed = 1.5f; // Adjust rhythm speed
```

## Future Enhancements

- [ ] Checkmate detection
- [ ] Stalemate and draw conditions
- [ ] En passant capture
- [ ] Castling (King and Rook special move)
- [ ] Move history and replay
- [ ] AI opponent
- [ ] Custom 3D piece models
- [ ] Sound effects and music
- [ ] Online multiplayer
- [ ] Save/Load game state

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Built with Unity Engine
- Chess rules reference: [FIDE Handbook](https://handbook.fide.com/)
- Inspired by classic chess games with a modern 3D twist

## Contact

For questions or feedback, please open an issue on GitHub.

---

**Enjoy your game of 3D Chess! ♟️**
"# ChessGameInUnity3D" 
