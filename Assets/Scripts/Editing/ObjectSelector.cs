using System.Windows.Forms;
using TMPro;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public static ObjectSelector Instance { get; private set; }
    [SerializeField] ActorView currentSelected;
    [SerializeField] WallPoint currentSelectedWall;
    [SerializeField] SpriteRenderer sr;
    public TMP_Text selectedText;
    Camera cam;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
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

    public void SelectWall(WallPoint wp)
    {
        if (wp == null) return;

        DeselectWall(wp);
        currentSelectedWall = wp;
    }

    public void DeselectWall(WallPoint wp)
    {
        if (wp == null || wp != currentSelectedWall) return;

        currentSelectedWall = null;
    }

    public void ClickObject(ActorView view)
    {
        if (view == null) return;

        Inspector.Instance.ShowActor(view);
    }

    private void Update()
    {
        if (currentSelected == null && currentSelectedWall == null && Inspector.Instance.selectedActor == null)
        {
            selectedText.text = "";
            transform.localScale = Vector3.zero;
        }
        else
        {
            selectedText.text = "";

            ActorView av = null;
            if (currentSelected != null)
                av = currentSelected;
            else if (Inspector.Instance.selectedActor != null)
                av = Inspector.Instance.selectedActor;
            if (av != null)
            {
                if (currentSelected != null)
                {
                    string name = $"{currentSelected.actor.Name}\n{currentSelected.actor.Gyaml}";
                    selectedText.text = name;
                    selectedText.transform.position = cam.WorldToScreenPoint(av.transform.position);
                } else
                    selectedText.text = "";

                sr.sprite = av.GetComponent<SpriteRenderer>().sprite;
                transform.SetPositionAndRotation(av.transform.position, av.transform.rotation);
                transform.localScale = av.transform.localScale;

                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
                    ActorManager.Instance.DuplicateActor(av.actor);
            }

            if (currentSelectedWall != null)
            {
                selectedText.transform.position = cam.WorldToScreenPoint(currentSelectedWall.transform.position);
                Vector3 pos = currentSelectedWall.transform.position;
                selectedText.text += $"Wall Group: {currentSelectedWall.lrIndex}\nWall Index: {currentSelectedWall.lineIndex}\nType: {currentSelectedWall.type}\nX: {pos.x}, Y: {pos.y}";
            }
        }
    }
}
