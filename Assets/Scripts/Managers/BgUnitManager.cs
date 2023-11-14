using System;
using System.Collections.Generic;
using System.Linq;
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

    private void Awake()
    {
        Instance = this;
    }

    public override void UpdateVisuals(Level level)
    {
        for (int i = 0; i < level.root.BgUnits.Length; i++)
        {
            wallObject = new($"Wall {i}");
            wallObject.transform.SetParent(transform);

            UpdateWallRenderers(i);
        }
    }

    void UpdateWallRenderers(int bguIndex)
    {
        if (wallRenderers != null)
            foreach (LineRenderer lr in wallRenderers)
                Destroy(lr.gameObject);
        if (wallPoints != null)
            foreach (WallPoint wp in wallPoints)
                Destroy(wp.gameObject);
        if (beltRailRenderers != null)
            foreach (LineRenderer lr in beltRailRenderers)
                Destroy(lr.gameObject);
        if (beltRailPoints != null)
            foreach (WallPoint wp in beltRailPoints)
                Destroy(wp.gameObject);

        BgUnit bgu = LevelLoader.Level.root.BgUnits[bguIndex];
        wallRenderers = new LineRenderer[bgu.Walls.Length];
        wallPoints = new List<WallPoint>();
        beltRailRenderers = new LineRenderer[bgu.BeltRails.Length];
        beltRailPoints = new List<WallPoint>();

        if (doBeltRails)
        {
            for (int i1 = 0; i1 < bgu.BeltRails.Length; i1++)
            {
                BeltRail br = bgu.BeltRails[i1];
                Color c = beltRailColor;
                if (!br.IsClosed) c = beltRailColorNotClosed;
                LineRenderer lr = CreateWall(br, wallObject.transform, c);
                beltRailRenderers[i1] = lr;

                for (int i = 0; i < br.Points.Count; i++)
                {
                    Point p = br.Points[i];
                    WallPoint wp = Instantiate(wallPointPrefab, p.Translate.ToVector3(), Quaternion.identity);
                    wp.lineIndex = i;
                    wp.lineRenderer = lr;
                    wp.lrIndex = i1;
                    wp.bguIndex = bguIndex;
                    wp.type = WallType.BeltRail;
                    wp.point = p;
                    beltRailPoints.Add(wp);
                }
            }
        }
        if (doWalls)
        {
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
                    wp.bguIndex = bguIndex;
                    wp.type = WallType.Wall;
                    wp.point = p;
                    wallPoints.Add(wp);
                }
            }
        }
    }

    public void AddWall()
    {
        if (!LevelLoader.Instance.levelIsLoaded) return;

        PopupField.Instance.ShowWithType("Group Index", "Wall Index", Enum.GetNames(typeof(WallType)), "Number", AddWallCallback);
    }

    public void RemoveWall(WallPoint wp)
    {
        if (wp.type == WallType.BeltRail)
        {
            LevelLoader.Level.root.BgUnits[wp.bguIndex].BeltRails[wp.lrIndex].Points.RemoveAt(wp.lineIndex);
        }
        else if (wp.type == WallType.Wall)
        {
            LevelLoader.Level.root.BgUnits[wp.bguIndex].Walls[wp.lrIndex].ExternalRail.Points.RemoveAt(wp.lineIndex);
        }
        UpdateWallRenderers(wp.bguIndex);
    }

    void AddWallCallback(string[] value, string type)
    {
        WallType t = (WallType)Enum.Parse(typeof(WallType), type);
        if (int.TryParse(value[0], out int group))
        {
            if (int.TryParse(value[1], out int index))
            {
                BgUnit bgu = LevelLoader.Level.root.BgUnits[0];
                if (t == WallType.Wall)
                {
                    BgWall wall = bgu.Walls[group];
                    Point p = new()
                    {
                        Translate = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToArray()
                    };
                    p.Translate[2] = 2f;
                    wall.ExternalRail.Points.Insert(index, p);

                    UpdateWallRenderers(0);
                }
                if (t == WallType.BeltRail)
                {
                    BeltRail rail = bgu.BeltRails[group];
                    Point p = new()
                    {
                        Translate = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToArray()
                    };
                    p.Translate[2] = 2f;
                    rail.Points.Insert(index, p);

                    UpdateWallRenderers(0);
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
