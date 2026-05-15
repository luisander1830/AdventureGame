namespace AdventureGame;

public class Room
{
	private string description;

	private bool north;
	private bool south;
	private bool east;
	private bool west;

	private bool lamp;
	private bool key;
	private bool chest;

	private bool lit;

	public Room()
	{

	}

	public string GetDescription()
	{
		return description;
	}

	public void SetDescription(string description)
	{
		this.description = description;
	}

	public bool HasNorth()
	{
		return north;
	}

	public void SetNorth(bool north)
	{
		this.north = north;
	}

	public bool HasSouth()
	{
		return south;
	}

	public void SetSouth(bool south)
	{
		this.south = south;
	}

	public bool HasEast()
	{
		return east;
	}

	public void SetEast(bool east)
	{
		this.east = east;
	}

	public bool HasWest()
	{
		return west;
	}

	public void SetWest(bool west)
	{
		this.west = west;
	}

	public bool HasLamp()
	{
		return lamp;
	}

	public void SetLamp(bool lamp)
	{
		this.lamp = lamp;
	}

	public bool HasKey()
	{
		return key;
	}

	public void SetKey(bool key)
	{
		this.key = key;
	}

	public bool HasChest()
	{
		return chest;
	}

	public void SetChest(bool chest)
	{
		this.chest = chest;
	}

	public bool IsLit()
	{
		return lit;
	}

	public void SetLit(bool lit)
	{
		this.lit = lit;
	}
}