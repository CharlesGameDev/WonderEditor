using System.Collections.Generic;
using UnityEngine;

public class BgUnitManager : Manager
{
    public static BgUnitManager Instance {  get; private set; }

    [SerializeField] Material wallsMaterial;
    [SerializeField] int lineCornerVertices;
    [SerializeField] float lineWidth;
    [SerializeField] Color beltRailColor = Color.white;
    [SerializeField] Color beltRailColorNotClosed = Color.yellow;
    [SerializeField] Color wallColor = Color.green;
    [SerializeField] Color wallColorNotClosed = Color.red;
    [SerializeField] bool doBeltRails = true;
    [SerializeField] bool doWalls = true;
    [SerializeField] WallPoint wallPointPrefab;
    LineRenderer[] wallRenderers;
    List<WallPoint> wallPoints;
    LineRenderer[] beltRailRenderers;
    List<WallPoint> beltRailPoints;
    GameObject wallObject;
    GameObject beltRailsObject;

    private void Awake()
    {
        Instance = this;
    }

    public override void UpdateVisuals(Level level)
    {
        foreach (BgUnit bgu in level.root.BgUnits)
        {
            if (doBeltRails && bgu.BeltRails != null)
            {
                beltRailsObject = new("Belt Rails");
                beltRailsObject.transform.SetParent(transform);

                UpdateBeltRailRenderers(bgu);
            }
            if (doWalls && bgu.Walls != null)
            {
                wallObject = new("Walls");
                wallObject.transform.SetParent(transform);

                UpdateWallRenderers(bgu);
            }
        }
    }

    void UpdateWallRenderers(BgUnit bgu)
    {
        if (wallRenderers != null)
            foreach (LineRenderer lr in wallRenderers)
                Destroy(lr.gameObject);
        if (wallPoints != null)
            foreach (WallPoint wp in wallPoints)
                Destroy(wp.gameObject);

        wallRenderers = new LineRenderer[bgu.Walls.Length];
        wallPoints = new List<WallPoint>();

        for (int i1 = 0; i1 < bgu.Walls.Length; i1++)
        {
            BgWall wall = bgu.Walls[i1];
            BeltRail br = wall.ExternalRail;
            Color c = wallColor;
            if (!br.IsClosed) c = wallColorNotClosed;
            LineRenderer lr = CreateWall(br, wallObject.transform, c);
            wallRenderers[i1] = lr;

            for (int i = 0; i < br.Points.Count; i++)
            {
                Point p = br.Points[i];
                WallPoint wp = Instantiate(wallPointPrefab, p.Translate.ToVector3(), Quaternion.identity);
                wp.lineIndex = i;
                wp.lineRenderer = lr;
                wp.lrIndex = i1;
                wp.bgu = bgu;
                wp.point = p;
                wp.type = 1;
                wallPoints.Add(wp);
            }
        }
    }

    void UpdateBeltRailRenderers(BgUnit bgu)
    {
        if (beltRailRenderers != null)
            foreach (LineRenderer lr in beltRailRenderers)
                Destroy(lr.gameObject);
        if (beltRailPoints != null)
            foreach (WallPoint wp in beltRailPoints)
                Destroy(wp.gameObject);

        beltRailRenderers = new LineRenderer[bgu.BeltRails.Length];
        beltRailPoints = new List<WallPoint>();

        for (int i1 = 0; i1 < bgu.BeltRails.Length; i1++)
        {
            BeltRail br = bgu.BeltRails[i1];
            Color c = beltRailColor;
            if (!br.IsClosed) c = beltRailColorNotClosed;
            LineRenderer lr = CreateWall(br, beltRailsObject.transform, c);
            beltRailRenderers[i1] = lr;

            for (int i = 0; i < br.Points.Count; i++)
            {
                Point p = br.Points[i];
                WallPoint wp = Instantiate(wallPointPrefab, p.Translate.ToVector3(), Quaternion.identity);
                wp.lineIndex = i;
                wp.lineRenderer = lr;
                wp.lrIndex = i1;
                wp.bgu = bgu;
                wp.point = p;
                wp.type = 0;
                beltRailPoints.Add(wp);
            }
        }
    }

    public void AddWall()
    {
        if (LevelLoader.Level == null) return;

        PopupField.Instance.Show("Wall Index", "Number", AddWallCallback);
    }

    public void RemoveWall(WallPoint wp)
    {
        if (wp.type == 0)
        {
            wp.bgu.BeltRails[wp.lrIndex].Points.RemoveAt(wp.lineIndex);
            UpdateBeltRailRenderers(wp.bgu);
        }
        else if (wp.type == 1)
        {
            wp.bgu.Walls[wp.lrIndex].ExternalRail.Points.RemoveAt(wp.lineIndex);
            UpdateWallRenderers(wp.bgu);
        }
    }

    void AddWallCallback(string value)
    {
        if (int.TryParse(value, out int index))
        {
            foreach (BgUnit bgu in LevelLoader.Level.root.BgUnits)
            {
                if (bgu.Walls != null)
                {
                    BgWall wall = bgu.Walls[index];
                    Point p = new()
                    {
                        Translate = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToArray()
                    };
                    p.Translate[2] = 2f;
                    wall.ExternalRail.Points.Add(p);

                    UpdateWallRenderers(bgu);
                }
            }
        }
    }

    LineRenderer CreateWall(BeltRail br, Transform parent, Color color)
    {
        GameObject rail = new(br.Points.Count.ToString());
        rail.transform.SetParent(parent);

        LineRenderer lr = rail.AddComponent<LineRenderer>();
        lr.material = wallsMaterial;
        lr.numCornerVertices = lineCornerVertices;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.startColor = color;
        lr.endColor = color;
        lr.useWorldSpace = false;

        lr.positionCount = br.Points.Count;
        for (int i = 0; i < br.Points.Count; i++)
        {
            float[] point = br.Points[i].Translate;

            lr.SetPosition(i, point.ToVector3());
        }

        return lr;
    }
}
