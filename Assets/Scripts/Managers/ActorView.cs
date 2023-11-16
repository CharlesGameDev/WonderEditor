using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActorView : MonoBehaviour
{
    public Actor actor;
    public SpriteRenderer sr;
    public SpriteRenderer sr2;
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
            if (actor.Gyaml == sp.Key)
            {
                s = sp.Value;
                break;
            }
        }

        sr.sprite = s;

        UpdateSpriteContent();
    }

    public void UpdateSpriteContent()
    {
        if (actor.Dynamic != null && actor.Dynamic.ContainsKey("InitDir"))
            sr.flipX = sr2.flipX = actor.Dynamic["InitDir"].ToString() == "1";
        else
        {
            sr.flipX = false;
            sr2.flipX = false;
        }

        switch (actor.Gyaml)
        {
            case "ObjectBlockClarityCharacter":
            case "BlockClarity":
            case "BlockRengaItem":
            case "BlockHatenaLong":
            case "BlockHatena":
                if (actor.Dynamic != null && actor.Dynamic.ContainsKey("ChildActorSelectName"))
                {
                    string item = actor.Dynamic["ChildActorSelectName"].ToString();

                    if (!ActorManager.Instance.Sprites.ContainsKey(item))
                        item = "ItemQuestion";

                    sr2.sprite = ActorManager.Instance.Sprites[item];
                    sr2.enabled = true;
                    return;
                }
                break;
        }

        sr2.enabled = false;
    }
}
