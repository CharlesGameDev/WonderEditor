using UnityEngine;

public class BgUnitManager : Manager
{
    [SerializeField] Material wallsMaterial;
    [SerializeField] int lineCornerVertices;
    [SerializeField] float lineWidth;
    [SerializeField] Color beltRailColor = Color.white;
    [SerializeField] Color beltRailColorNotClosed = Color.yellow;
    [SerializeField] Color wallColor = Color.green;
    [SerializeField] Color wallColorNotClosed = Color.red;

    public override void UpdateVisuals(Level level)
    {
        foreach (BgUnit bgu in level.root.BgUnits)
        {
            if (bgu.BeltRails != null)
            {
                GameObject unit = new GameObject("Belt Rails");
                unit.transform.SetParent(transform);

                foreach (BeltRail br in bgu.BeltRails)
                {
                    Color c = beltRailColor;
                    if (!br.IsClosed) c = beltRailColorNotClosed;
                    CreateWall(br, unit.transform, c);
                }
            }
            if (bgu.Walls != null)
            {
                GameObject unit = new GameObject("Walls");
                unit.transform.SetParent(transform);

                foreach (BgWall wall in bgu.Walls)
                {
                    BeltRail br = wall.ExternalRail;
                    Color c = wallColor;
                    if (!br.IsClosed) c = wallColorNotClosed;
                    CreateWall(br, unit.transform, c);
                }
            }
        }
    }

    void CreateWall(BeltRail br, Transform parent, Color color)
    {
        GameObject rail = new GameObject(br.Points.Length.ToString());
        rail.transform.SetParent(parent);

        LineRenderer lr = rail.AddComponent<LineRenderer>();
        lr.material = wallsMaterial;
        lr.numCornerVertices = lineCornerVertices;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.startColor = color;
        lr.endColor = color;
        lr.useWorldSpace = false;

        lr.positionCount = br.Points.Length;
        for (int i = 0; i < br.Points.Length; i++)
        {
            float[] point = br.Points[i].Translate;

            lr.SetPosition(i, point.ToVector3());
        }
    }
}
