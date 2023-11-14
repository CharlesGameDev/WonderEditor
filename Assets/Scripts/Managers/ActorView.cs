using UnityEngine;
using UnityEngine.EventSystems;

public class ActorView : MonoBehaviour
{
    public Actor actor;
    public SpriteRenderer sr;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        ObjectSelector.Instance.SelectObject(this);

        if (Input.GetMouseButtonDown(1))
        {
            ObjectSelector.Instance.DeselectObject(this);
            ActorManager.Instance.DeleteActor(this);
            Destroy(gameObject);
        }
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
        actor.Translate = pos.ToArray();
        transform.position = ((Vector3)sr.sprite.pivot / sr.sprite.pixelsPerUnit) - pos;
    }

    private void Update()
    {
        transform.SetLocalPositionAndRotation(actor.Translate.ToVector3(), actor.Rotate.ToRotation());
        transform.localScale = actor.Scale.ToVector3();
    }

    public void UpdateSprite()
    {
        Sprite s = ActorManager.Instance.Sprites["unknown"];
        foreach (var sp in ActorManager.Instance.Sprites)
        {
            if (actor.Gyaml.Contains(sp.Value.name))
            {
                s = sp.Value;
                break;
            }
        }
        sr.sprite = s;
    }
}
