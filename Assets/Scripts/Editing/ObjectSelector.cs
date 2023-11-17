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
            ActorView av = null;
            if (currentSelected != null)
                av = currentSelected;
            else if (Inspector.Instance.selectedActor != null)
                av = Inspector.Instance.selectedActor;
            if (av != null)
            {
                if (currentSelected != null)
                {
                    Vector3 pos = currentSelected.transform.position;
                    string actorName = currentSelected.name;
                    if (actorName == "unknown actor")
                        actorName = currentSelected.actor.Gyaml;

                    string name = $"{currentSelected.actor.Name}\n{actorName}\nX: {pos.x}, Y: {pos.y}";
                    selectedText.text = name;
                    selectedText.transform.position = cam.WorldToScreenPoint(av.transform.position);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ClickObject(currentSelected);
                    }
                } else
                    selectedText.text = "";

                sr.sprite = av.GetComponent<SpriteRenderer>().sprite;
                sr.flipX = av.sr.flipX;
                transform.SetPositionAndRotation(av.transform.position, av.transform.rotation);
                transform.localScale = av.transform.localScale;

                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
                    ActorManager.Instance.DuplicateActor(av.actor);
            }

            if (currentSelectedWall != null)
            {
                selectedText.transform.position = cam.WorldToScreenPoint(currentSelectedWall.transform.position);
                Vector3 pos = currentSelectedWall.transform.position;
                selectedText.text = $"Wall Group: {currentSelectedWall.group}\nWall Index: {currentSelectedWall.index}\nBgUnit: {currentSelectedWall.bguIndex}\nModel: {currentSelectedWall.GetModelNumber()}\nType: {currentSelectedWall.type}\nX: {pos.x}, Y: {pos.y}, Z: {pos.z}\nE to toggle semisolid\n[] to move forward and back\n-= to change model";
                
                if (Input.GetKeyDown(KeyCode.E))
                    currentSelectedWall.ToggleGroupIsClosed();
                if (Input.GetKeyDown(KeyCode.RightBracket))
                    currentSelectedWall.AddPositionZ(1);
                if (Input.GetKeyDown(KeyCode.LeftBracket))
                    currentSelectedWall.AddPositionZ(-1);
                if (Input.GetKeyDown(KeyCode.Minus))
                    currentSelectedWall.AddModelNumber(-1);
                if (Input.GetKeyDown(KeyCode.Equals))
                    currentSelectedWall.AddModelNumber(1);
            }
        }
    }
}
