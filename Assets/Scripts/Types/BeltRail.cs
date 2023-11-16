using System.Collections.Generic;

[System.Serializable]
public class BeltRail
{
    public bool IsClosed;
    public List<Point> Points;

    public BeltRail()
    {
        Points = new List<Point>();
        IsClosed = true;
    }
}
