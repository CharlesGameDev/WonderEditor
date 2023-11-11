using UnityEngine;
using UnityEngine.EventSystems;

public class WallPoint : MonoBehaviour
{
    public Point point;
    public int lineIndex;
    public int lrIndex;
    public BgUnit bgu;
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
    }

    private void OnMouseExit()
    {
        sr.color = normalColor;
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos = pos.PutOnGrid(4);
        pos.z = transform.position.z;
        point.Translate = pos.ToArray();
        transform.position = pos;
        lineRenderer.SetPosition(lineIndex, pos);
    }
}
