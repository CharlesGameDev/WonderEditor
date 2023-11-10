using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public static ObjectSelector Instance { get; private set; }
    [SerializeField] ActorView currentSelected;
    [SerializeField] SpriteRenderer sr;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectObject(ActorView view)
    {
        if (view == null) return;

        DeselectObject(view);
        currentSelected = view;
    }

    public void DeselectObject(ActorView view)
    {
        if (view == null || view != currentSelected) return;

        currentSelected = null;
    }

    public void ClickObject(ActorView view)
    {
        if (view == null) return;

        Inspector.Instance.ShowActor(view);
    }

    private void Update()
    {
        if (currentSelected == null && Inspector.Instance.selectedActor == null)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            Transform t = null;
            if (currentSelected != null)
                t = currentSelected.transform;
            else if (Inspector.Instance.selectedActor != null)
                t = Inspector.Instance.selectedActor.transform;

            sr.sprite = t.GetComponent<SpriteRenderer>().sprite;
            transform.SetPositionAndRotation(t.position, t.rotation);
            transform.localScale = t.localScale;
        }
    }
}
