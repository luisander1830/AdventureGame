# AdventureGameCSharp

Modified version of the original AdventureGameCSharp project.

## Features Added

- Dungeon file format using external `.txt` files
- Dungeon loader/parser inside `AdventureGame.cs`
- 8x8 dungeon support
- Dungeon exit coordinates
- Adventurer initial position coordinates
- Lamp position coordinates
- Key position coordinates
- Treasure chest position coordinates
- Grue initial position coordinates
- New win condition:
  - The player must open the treasure chest and escape through the dungeon exit
- New Grue mechanic:
  - After opening the treasure chest, the Grue begins pursuing the player
- Player loses if the Grue reaches the same room
- Two dungeon files included:
  - `dungeon1.txt`
  - `dungeon2.txt`

---

# How to Run

Open a terminal inside:

src/AdventureGame

Then run:

dotnet run

---

# Controls

| Key | Action |
|------|------|
| W | Move North |
| S | Move South |
| D | Move East |
| A | Move West |
| L | Pick Up Lamp |
| K | Pick Up Key |
| O | Open Treasure Chest |
| Q | Quit Game |

---

# Dungeon File Format Example

ROWS=8
COLS=8

START=0,0
EXIT=7,7

LAMP=1,1
KEY=3,3
CHEST=6,6
GRUE=7,0

ROOM=0,0,N:0,S:1,E:1,W:0,LIT:1

---

# Requirements

- .NET 9 SDK
