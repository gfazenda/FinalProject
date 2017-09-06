[System.Serializable]
public class Coord {

    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public Coord parent = null;
    
    public Coord()
    {
        x = 0;
        y = 0;
    }

    public Coord(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    //public static bool operator ==(Coord c1, Coord c2)
    //{
    //    return c1.Equals(c2);
    //}

    //public static bool operator !=(Coord c1, Coord c2)
    //{
    //    return !c1.Equals(c2);
    //}

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
