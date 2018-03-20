using System;

[System.Serializable]
public class Coord {

    public int x;
    public int y;
    
    public Coord()
    {
        x = -1;
        y = -1;
    }
    
    public Coord(Coord original)
    {
        x = original.x;
        y = original.y;
    }

    public bool CompareTo(Coord other)
    {
        return (this.x == other.x && this.y == other.y);
    }

    public string DebugInfo()
    {
        return "x " + x + " y " + y;
    }

    public Coord(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public void Copy(Coord original)
    {
        x = original.x;
        y = original.y;
    }

}
