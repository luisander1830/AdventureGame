using System;
using System.IO;
using System.Linq;

namespace AdventureGame;

public class AdventureGame
{
	public readonly string GO_NORTH = "W";
	public readonly string GO_SOUTH = "S";
	public readonly string GO_EAST = "D";
	public readonly string GO_WEST = "A";
	public readonly string GET_LAMP = "L";
	public readonly string GET_KEY = "K";
	public readonly string OPEN_CHEST = "O";
	public readonly string QUIT = "Q";

	private Adventurer adventurer;

	private Room[,] dungeon;

	private int rows;
	private int cols;

	private int aRow;
	private int aCol;

	private int exitRow;
	private int exitCol;

	private int grueRow;
	private int grueCol;

	private bool isChestOpen;
	private bool hasPlayerQuit;
	private bool isAdventureAlive;
	private bool isPlayerWinner;

	private string lastDirection;

	public AdventureGame()
	{

	}

	public void Start()
	{
		Init();

		ShowGameStartScreen();

		string input;

		do
		{
			ShowScene();

			do
			{
				ShowInputOptions();

				input = GetInput();
			}
			while(!IsValidInput(input));

			ProcessInput(input);

			UpdateGameState();
		}
		while(!IsGameOver());

		ShowGameOverScreen();
	}

	private void Init()
	{
		adventurer = new Adventurer();

		LoadDungeon("dungeon1.txt");

		isChestOpen = false;
		hasPlayerQuit = false;
		isAdventureAlive = true;
		isPlayerWinner = false;

		lastDirection = string.Empty;
	}

	private void LoadDungeon(string filePath)
	{
		string[] lines = File.ReadAllLines(filePath);

		foreach(string rawLine in lines)
		{
			string line = rawLine.Trim();

			if(line.StartsWith("ROWS="))
			{
				rows = int.Parse(line.Replace("ROWS=", ""));
			}
			else if(line.StartsWith("COLS="))
			{
				cols = int.Parse(line.Replace("COLS=", ""));
			}
		}

		dungeon = new Room[rows, cols];

		
		foreach(string rawLine in lines)
		{
			string line = rawLine.Trim();

			if(line.StartsWith("ROOM="))
			{
				ParseRoom(line);
			}
		}

		
		foreach(string rawLine in lines)
		{
			string line = rawLine.Trim();

			if(line.StartsWith("START="))
			{
				string[] parts = line.Replace("START=", "").Split(',');

				aRow = int.Parse(parts[0]);
				aCol = int.Parse(parts[1]);
			}
			else if(line.StartsWith("EXIT="))
			{
				string[] parts = line.Replace("EXIT=", "").Split(',');

				exitRow = int.Parse(parts[0]);
				exitCol = int.Parse(parts[1]);
			}
			else if(line.StartsWith("GRUE="))
			{
				string[] parts = line.Replace("GRUE=", "").Split(',');

				grueRow = int.Parse(parts[0]);
				grueCol = int.Parse(parts[1]);
			}
			else if(line.StartsWith("LAMP="))
			{
				string[] parts = line.Replace("LAMP=", "").Split(',');

				dungeon[int.Parse(parts[0]), int.Parse(parts[1])].SetLamp(true);
			}
			else if(line.StartsWith("KEY="))
			{
				string[] parts = line.Replace("KEY=", "").Split(',');

				dungeon[int.Parse(parts[0]), int.Parse(parts[1])].SetKey(true);
			}
			else if(line.StartsWith("CHEST="))
			{
				string[] parts = line.Replace("CHEST=", "").Split(',');

				dungeon[int.Parse(parts[0]), int.Parse(parts[1])].SetChest(true);
			}
		}
	}

	private void ParseRoom(string line)
	{
		string data = line.Replace("ROOM=", "");

		string[] sections = data.Split(',');

		int r = int.Parse(sections[0]);
		int c = int.Parse(sections[1]);

		Room room = new Room();

		foreach(string section in sections.Skip(2))
		{
			string[] pair = section.Split(':');

			string key = pair[0];
			bool value = pair[1] == "1";

			switch(key)
			{
				case "N":
					room.SetNorth(value);
					break;

				case "S":
					room.SetSouth(value);
					break;

				case "E":
					room.SetEast(value);
					break;

				case "W":
					room.SetWest(value);
					break;

				case "LIT":
					room.SetLit(value);
					break;
			}
		}

		room.SetDescription($"Room [{r},{c}]");

		dungeon[r, c] = room;
	}

	private void ShowGameStartScreen()
	{
		Console.WriteLine("WELCOME TO THE ADVENTURE GAME");
		Console.WriteLine();
	}

	private void ShowScene()
	{
		Room r = dungeon[aRow, aCol];

		if(adventurer.HasLamp() || r.IsLit())
		{
			Console.WriteLine(r.GetDescription());

			if(r.HasLamp())
			{
				Console.WriteLine("There is a lamp here.");
			}

			if(r.HasKey())
			{
				Console.WriteLine("There is a key here.");
			}

			if(r.HasChest())
			{
				Console.WriteLine("There is a treasure chest here.");
			}

			if(aRow == exitRow && aCol == exitCol)
			{
				Console.WriteLine("You see the dungeon exit.");
			}
		}
		else
		{
			Console.WriteLine("It is pitch black!");
		}

		Console.WriteLine();
	}

	private void ShowInputOptions()
	{
		string options = ""
		+ $"GO NORTH [{GO_NORTH}] | GO EAST [{GO_EAST}] | GET LAMP [{GET_LAMP}] | OPEN CHEST [{OPEN_CHEST}]\n"
		+ $"GO SOUTH [{GO_SOUTH}] | GO WEST [{GO_WEST}] | GET KEY  [{GET_KEY}] | QUIT       [{QUIT}]\n"
		+ $"> ";

		Console.Write(options);
	}

	private string GetInput()
	{
		return Console.ReadLine()!.ToUpper();
	}

	private bool IsValidInput(string input)
	{
		string[] validInputs =
		{
			GO_NORTH,
			GO_SOUTH,
			GO_EAST,
			GO_WEST,
			GET_LAMP,
			GET_KEY,
			OPEN_CHEST,
			QUIT
		};

		if(!validInputs.Contains(input))
		{
			Console.WriteLine("ERROR: Invalid input.");
			return false;
		}

		return true;
	}

	private void ProcessInput(string input)
	{
		Room r = dungeon[aRow, aCol];

		
		if(!adventurer.HasLamp() && !r.IsLit() && input != lastDirection)
		{
			Console.WriteLine("You got eaten alive by the Grue!");
			isAdventureAlive = false;
			return;
		}

		if(input == GO_NORTH)
		{
			GoNorth(r);
		}
		else if(input == GO_SOUTH)
		{
			GoSouth(r);
		}
		else if(input == GO_EAST)
		{
			GoEast(r);
		}
		else if(input == GO_WEST)
		{
			GoWest(r);
		}
		else if(input == GET_LAMP)
		{
			GetLamp(r);
		}
		else if(input == GET_KEY)
		{
			GetKey(r);
		}
		else if(input == OPEN_CHEST)
		{
			OpenChest(r);
		}
		else
		{
			Quit();
		}
	}

	private void UpdateGameState()
	{
		
		if(isChestOpen)
		{
			MoveGrue();
		}

		
		if(aRow == grueRow && aCol == grueCol)
		{
			Console.WriteLine("The Grue caught you!");
			isAdventureAlive = false;
		}

		
		if(isChestOpen && aRow == exitRow && aCol == exitCol)
		{
			Console.WriteLine("You escaped the dungeon with the treasure!");
			isPlayerWinner = true;
		}
	}

	private void MoveGrue()
	{
		if(grueRow < aRow)
		{
			grueRow++;
		}
		else if(grueRow > aRow)
		{
			grueRow--;
		}
		else if(grueCol < aCol)
		{
			grueCol++;
		}
		else if(grueCol > aCol)
		{
			grueCol--;
		}

		Console.WriteLine("You hear the Grue moving...");
	}

	private bool IsGameOver()
	{
		return hasPlayerQuit || !isAdventureAlive || isPlayerWinner;
	}

	private void ShowGameOverScreen()
	{
		Console.WriteLine();

		if(isPlayerWinner)
		{
			Console.WriteLine("YOU WIN!");
		}
		else if(!isAdventureAlive)
		{
			Console.WriteLine("YOU DIED!");
		}
		else
		{
			Console.WriteLine("GAME OVER!");
		}
	}

	private void GoNorth(Room r)
	{
		if(r.HasNorth())
		{
			aRow--;
			lastDirection = GO_SOUTH;
		}
	}

	private void GoSouth(Room r)
	{
		if(r.HasSouth())
		{
			aRow++;
			lastDirection = GO_NORTH;
		}
	}

	private void GoEast(Room r)
	{
		if(r.HasEast())
		{
			aCol++;
			lastDirection = GO_WEST;
		}
	}

	private void GoWest(Room r)
	{
		if(r.HasWest())
		{
			aCol--;
			lastDirection = GO_EAST;
		}
	}

	private void GetLamp(Room r)
	{
		if(r.HasLamp())
		{
			adventurer.SetLamp(true);

			r.SetLamp(false);

			Console.WriteLine("You picked up the lamp!");
		}
	}

	private void GetKey(Room r)
	{
		if(r.HasKey())
		{
			adventurer.SetKey(true);

			r.SetKey(false);

			Console.WriteLine("You picked up the key!");
		}
	}

	private void OpenChest(Room r)
	{
		if(r.HasChest() && adventurer.HasKey())
		{
			Console.WriteLine("You opened the treasure chest!");
			Console.WriteLine("THE GRUE HAS AWAKENED!");

			isChestOpen = true;
		}
	}

	private void Quit()
	{
		hasPlayerQuit = true;
	}
}
