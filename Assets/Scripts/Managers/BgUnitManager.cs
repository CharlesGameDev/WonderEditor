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
    List<LineRenderer[]> wallRenderers;
    List<List<WallPoint>> wallPoints;
    List<LineRenderer[]> beltRailRenderers;
    List<List<WallPoint>> beltRailPoints;
    GameObject wallObject;

    private void Awake()
    {
        Instance = this;
    }

    public override void UpdateVisuals(Level level)
    {
        if (wallRenderers != null)
            foreach (LineRenderer[] lr in wallRenderers)
                if (lr != null)
                    foreach (LineRenderer lr2 in lr)
                        Destroy(lr2.gameObject);
        if (wallPoints != null)
            foreach (List<WallPoint> wp in wallPoints)
                if (wp != null)
                    foreach (WallPoint wp2 in wp)
                        Destroy(wp2.gameObject);
        if (beltRailRenderers != null)
            foreach (LineRenderer[] lr in beltRailRenderers)
                if (lr != null)
                    foreach (LineRenderer lr2 in lr)
                        Destroy(lr2.gameObject);
        if (beltRailPoints != null)
            foreach (List<WallPoint> wp in beltRailPoints)
                if (wp != null)
                    foreach (WallPoint wp2 in wp)
                        Destroy(wp2.gameObject);

        wallRenderers = new List<LineRenderer[]>();
        wallPoints = new List<List<WallPoint>>();
        beltRailRenderers = new List<LineRenderer[]>();
        beltRailPoints = new List<List<WallPoint>>();

        for (int i = 0; i < level.root.BgUnits.Count; i++)
        {
            wallObject = new($"Wall {i}");
            wallObject.transform.SetParent(transform);
            wallRenderers.Add(new LineRenderer[] { });
            wallPoints.Add(new List<WallPoint>());
            beltRailRenderers.Add(new LineRenderer[] { });
            beltRailPoints.Add(new List<WallPoint>());

            UpdateWallRenderers(i);
        }
    }

    public void UpdateWallRenderers(int bguIndex)
    {
        Debug.Log($"Updating wall renderers: {bguIndex + 1}/{LevelLoader.Level.root.BgUnits.Count}");
        if (wallRenderers[bguIndex] != null)
            foreach (LineRenderer lr in wallRenderers[bguIndex])
                if (lr != null)
                    Destroy(lr.gameObject);
        if (wallPoints != null)
            foreach (WallPoint wp in wallPoints[bguIndex])
                if (wp != null)
                    Destroy(wp.gameObject);
        if (beltRailRenderers != null)
            foreach (LineRenderer lr in beltRailRenderers[bguIndex])
                if (lr != null)
                    Destroy(lr.gameObject);
        if (beltRailPoints != null)
            foreach (WallPoint wp in beltRailPoints[bguIndex])
                if (wp != null)
                    Destroy(wp.gameObject);

        BgUnit bgu = LevelLoader.Level.root.BgUnits[bguIndex];
        if (bgu.Walls != null)
            wallRenderers[bguIndex] = new LineRenderer[bgu.Walls.Count];
        else
            wallRenderers[bguIndex] = new LineRenderer[] { };
        wallPoints[bguIndex] = new List<WallPoint>();
        if (bgu.BeltRails != null)
        {
            beltRailRenderers[bguIndex] = new LineRenderer[bgu.BeltRails.Count];
            beltRailPoints[bguIndex] = new List<WallPoint>();
        }

        if (bgu.BeltRails != null && doBeltRails)
        {
            for (int i1 = 0; i1 < bgu.BeltRails.Count; i1++)
            {
                BeltRail br = bgu.BeltRails[i1];
                Color c = beltRailColor;
                if (!br.IsClosed) c = beltRailColorNotClosed;
                LineRenderer lr = CreateWall(br, wallObject.transform, c);
                beltRailRenderers[bguIndex][i1] = lr;

                for (int i = 0; i < br.Points.Count; i++)
                {
                    Point p = br.Points[i];
                    WallPoint wp = Instantiate(wallPointPrefab, p.Translate.ToVector3(), Quaternion.identity, transform);
                    wp.index = i;
                    wp.lineRenderer = lr;
                    wp.group = i1;
                    wp.bguIndex = bguIndex;
                    wp.type = WallType.BeltRail;
                    wp.point = p;
                    beltRailPoints[bguIndex].Add(wp);
                }
            }
        }
        if (bgu.Walls != null && doWalls)
        {
            for (int i1 = 0; i1 < bgu.Walls.Count; i1++)
            {
                BgWall wall = bgu.Walls[i1];
                BeltRail br = wall.ExternalRail;
                Color c = wallColor;
                if (!br.IsClosed) c = wallColorNotClosed;
                LineRenderer lr = CreateWall(br, wallObject.transform, c);
                wallRenderers[bguIndex][i1] = lr;

                for (int i = 0; i < br.Points.Count; i++)
                {
                    Point p = br.Points[i];
                    WallPoint wp = Instantiate(wallPointPrefab, p.Translate.ToVector3(), Quaternion.identity, transform);
                    wp.index = i;
                    wp.group = i1;
                    wp.bguIndex = bguIndex;
                    wp.lineRenderer = lr;
                    wp.type = WallType.Wall;
                    wp.point = p;
                    wallPoints[bguIndex].Add(wp);
                }
            }
        }
    }

    public void ToggleGroupIsClosed(WallPoint wp)
    {
        if (wp.type == WallType.Wall)
            LevelLoader.Level.root.BgUnits[wp.bguIndex].Walls[wp.group].ExternalRail.IsClosed = !LevelLoader.Level.root.BgUnits[wp.bguIndex].Walls[wp.group].ExternalRail.IsClosed;
        if (wp.type == WallType.BeltRail)
            LevelLoader.Level.root.BgUnits[wp.bguIndex].BeltRails[wp.group].IsClosed = !LevelLoader.Level.root.BgUnits[wp.bguIndex].BeltRails[wp.group].IsClosed;

        UpdateWallRenderers(wp.bguIndex);
    }

    public void AddWall()
    {
        if (!LevelLoader.Instance.levelIsLoaded) return;

        PopupField.Instance.ShowWithType("Group Index", "Wall Index", Enum.GetNames(typeof(WallType)), "Number", AddWallCallback1);
    }
    string[] addWallPopupValue;
    string addWallPopupType;
    int addWallPopupBGU;
    void AddWallCallback1(string[] value, string type)
    {
        addWallPopupValue = value;
        addWallPopupType = type;
        PopupField.Instance.Show("BgUnit", "", "Index", AddWallCallback2);
    }

    void AddWallCallback2(string[] bgu)
    {
        addWallPopupBGU = int.Parse(bgu[0]);
        AddWallCallback(addWallPopupValue, addWallPopupType, addWallPopupBGU);
    }

    void AddWallCallback(string[] value, string type, int bgui)
    {
        WallType t = (WallType)Enum.Parse(typeof(WallType), type);
        if (int.TryParse(value[0], out int group))
        {
            if (int.TryParse(value[1], out int index))
            {
                bool updateVisuals = false;
                if (LevelLoader.Level.root.BgUnits.Count <= bgui)
                {
                    LevelLoader.Level.root.BgUnits.Add(new BgUnit());
                    bgui = LevelLoader.Level.root.BgUnits.Count - 1;
                    updateVisuals = true;
                }

                BgUnit bgu = LevelLoader.Level.root.BgUnits[bgui];
                if (t == WallType.Wall)
                {
                    if (bgu.Walls.Count <= group)
                    {
                        bgu.Walls.Add(new BgWall());
                        group = bgu.Walls.Count - 1;
                    }

                    BgWall wall = bgu.Walls[group];
                    Point p = new()
                    {
                        Translate = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToArray()
                    };
                    p.Translate[2] = 2f;
                    wall.ExternalRail.Points.Insert(index, p);
                }
                if (t == WallType.BeltRail)
                {
                    if (bgu.BeltRails.Count <= group)
                    {
                        bgu.BeltRails.Add(new BeltRail());
                        group = bgu.BeltRails.Count - 1;
                    }

                    BeltRail rail = bgu.BeltRails[group];
                    Point p = new()
                    {
                        Translate = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToArray()
                    };
                    p.Translate[2] = 2f;
                    rail.Points.Insert(index, p);
                }

                if (updateVisuals)
                    UpdateVisuals(LevelLoader.Level);
                else
                    UpdateWallRenderers(bgui);
            }
        }
    }

    public void RemoveWall(WallPoint wp)
    {
        if (wp.type == WallType.BeltRail)
        {
            LevelLoader.Level.root.BgUnits[wp.bguIndex].BeltRails[wp.group].Points.RemoveAt(wp.index);
        }
        else if (wp.type == WallType.Wall)
        {
            LevelLoader.Level.root.BgUnits[wp.bguIndex].Walls[wp.group].ExternalRail.Points.RemoveAt(wp.index);
        }
        UpdateWallRenderers(wp.bguIndex);
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
        lr.useWorldSpace = true;

        lr.positionCount = br.Points.Count;
        for (int i = 0; i < br.Points.Count; i++)
        {
            float[] point = br.Points[i].Translate;

            lr.SetPosition(i, point.ToVector3());
        }

        return lr;
    }
}
