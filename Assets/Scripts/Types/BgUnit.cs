using System.Collections.Generic;

[System.Serializable]
public class BgUnit
{
    public List<BeltRail> BeltRails;
    public List<BgWall> Walls;
    public long ModelType;
    public long SkinDivision;

    public BgUnit()
    {
        BeltRails = new List<BeltRail>();
        Walls = new List<BgWall>();
    }
}
