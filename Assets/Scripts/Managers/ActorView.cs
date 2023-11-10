using UnityEngine;
using UnityEngine.EventSystems;

public class ActorView : MonoBehaviour
{
    public Actor actor;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        ObjectSelector.Instance.SelectObject(this);
    }

    private void OnMouseExit()
    {
        ObjectSelector.Instance.DeselectObject(this);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        ObjectSelector.Instance.ClickObject(this);
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos = pos.PutOnGrid(4);
        pos.z = transform.position.z;
        transform.position = pos;
    }

    private void Update()
    {
        actor.Translate = transform.position.ToArray();
        actor.Scale = transform.localScale.ToArray();
        actor.Rotate = transform.rotation.ToArrayRad();
    }
}
