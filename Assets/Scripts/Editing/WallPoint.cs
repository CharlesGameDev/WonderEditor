using UnityEngine;
using UnityEngine.EventSystems;

public class WallPoint : MonoBehaviour
{
    public Point point;
    public int index;
    public int bguIndex;
    public int group;
    public WallType type;
    public LineRenderer lineRenderer;
    Camera cam;
    SpriteRenderer sr;
    Color normalColor;
    [SerializeField] Color hoverColor;

    private void Awake()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            BgUnitManager.Instance.RemoveWall(this);
        }
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
        ObjectSelector.Instance.SelectWall(this);
    }

    private void OnMouseExit()
    {
        sr.color = normalColor;
        ObjectSelector.Instance.DeselectWall(this);
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos = pos.RoundToInt();
        pos.z = transform.position.z;
        point.Translate = pos.ToArray();
        transform.position = pos;
        lineRenderer.SetPosition(index, pos);
    }

    public void ToggleGroupIsClosed()
    {
        BgUnitManager.Instance.ToggleGroupIsClosed(this);
    }

    public void AddPositionZ(int amount)
    {
        if (type == WallType.Wall)
            LevelLoader.Level.root.BgUnits[bguIndex].Walls[group].ExternalRail.Points[index].Translate[2] += amount;
        if (type == WallType.BeltRail)
            LevelLoader.Level.root.BgUnits[bguIndex].BeltRails[group].Points[index].Translate[2] += amount;

        BgUnitManager.Instance.UpdateWallRenderers(bguIndex);
    }

    public void AddModelNumber(int amount)
    {
        LevelLoader.Level.root.BgUnits[bguIndex].ModelType += amount;
    }

    public long GetModelNumber()
    {
        return LevelLoader.Level.root.BgUnits[bguIndex].ModelType;
    }
}
