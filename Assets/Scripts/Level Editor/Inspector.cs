using TMPro;
using UnityEngine;

public class Inspector : MonoBehaviour
{
    private static Inspector instance;
    public static Inspector Instance
    {
        get
        {
            if (instance == null) instance = FindFirstObjectByType<Inspector>(FindObjectsInactive.Include);
            return instance;
        }
    }

    public ActorView selectedActor;

    [SerializeField] string wikiLink;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField gyamlInput;
    [SerializeField] TMP_InputField hashInput;
    [SerializeField] TMP_InputField areaHashInput;
    [SerializeField] TMP_InputField translateXInput;
    [SerializeField] TMP_InputField translateYInput;
    [SerializeField] TMP_InputField translateZInput;
    [SerializeField] TMP_InputField scaleXInput;
    [SerializeField] TMP_InputField scaleYInput;
    [SerializeField] TMP_InputField scaleZInput;
    [SerializeField] TMP_InputField rotateXInput;
    [SerializeField] TMP_InputField rotateYInput;
    [SerializeField] TMP_InputField rotateZInput;

    private void Awake()
    {
        nameInput.onEndEdit.AddListener(SetName);
        gyamlInput.onEndEdit.AddListener(SetGyaml);
        hashInput.onEndEdit.AddListener(SetHash);
        areaHashInput.onEndEdit.AddListener(SetAreaHash);
        translateXInput.onEndEdit.AddListener(SetTranslateX);
        translateYInput.onEndEdit.AddListener(SetTranslateY);
        translateZInput.onEndEdit.AddListener(SetTranslateZ);
        scaleXInput.onEndEdit.AddListener(SetScaleX);
        scaleYInput.onEndEdit.AddListener(SetScaleY);
        scaleZInput.onEndEdit.AddListener(SetScaleZ);
        rotateXInput.onEndEdit.AddListener(SetRotateX);
        rotateYInput.onEndEdit.AddListener(SetRotateY);
        rotateZInput.onEndEdit.AddListener(SetRotateZ);
    }

    public void OpenWiki()
    {
        string link = string.Format(wikiLink, selectedActor.actor.Gyaml);
        Application.OpenURL(link);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        selectedActor = null;
    }

    public void ShowActor(ActorView actor)
    {
        selectedActor = actor;

        nameInput.text = selectedActor.actor.Name;
        gyamlInput.text = selectedActor.actor.Gyaml;
        hashInput.text = selectedActor.actor.Hash.ToString();
        areaHashInput.text = selectedActor.actor.AreaHash.ToString();
        translateXInput.text = selectedActor.actor.Translate[0].ToString();
        translateYInput.text = selectedActor.actor.Translate[1].ToString();
        translateZInput.text = selectedActor.actor.Translate[2].ToString();
        scaleXInput.text = selectedActor.actor.Scale[0].ToString();
        scaleYInput.text = selectedActor.actor.Scale[1].ToString();
        scaleZInput.text = selectedActor.actor.Scale[2].ToString();
        float[] rotate = selectedActor.actor.Rotate.RadToDeg();
        rotateXInput.text = rotate[0].ToString();
        rotateYInput.text = rotate[1].ToString();
        rotateZInput.text = rotate[2].ToString();
        gameObject.SetActive(true);
    }

    public void FindActor()
    {
        if (selectedActor == null) return;

        ActorList.Instance.Show(FindActorCallback, "Select Actor");
    }

    void FindActorCallback(string gyaml)
    {
        if (selectedActor == null) return;

        selectedActor.actor.Gyaml = gyaml;
        selectedActor.UpdateSprite();
    }

    public void EditDynamicValues()
    {
        DynamicProperties.Instance.Show(selectedActor);
    }

    public void Duplicate()
    {
        ActorManager.Instance.DuplicateActor(selectedActor.actor);
    }

    public void SetName(string v) => selectedActor.actor.Name = v;
    public void SetGyaml(string v)
    {
        selectedActor.actor.Gyaml = v;
        selectedActor.UpdateSprite();
    }

    public void SetHash(string v) => ulong.TryParse(v, out selectedActor.actor.Hash);
    public void SetAreaHash(string v) => uint.TryParse(v, out selectedActor.actor.AreaHash);

    public void SetTranslateX(string v) => float.TryParse(v, out selectedActor.actor.Translate[0]);
    public void SetTranslateY(string v) => float.TryParse(v, out selectedActor.actor.Translate[1]);
    public void SetTranslateZ(string v) => float.TryParse(v, out selectedActor.actor.Translate[2]);

    public void SetScaleX(string v) => float.TryParse(v, out selectedActor.actor.Scale[0]);
    public void SetScaleY(string v) => float.TryParse(v, out selectedActor.actor.Scale[1]);
    public void SetScaleZ(string v) => float.TryParse(v, out selectedActor.actor.Scale[2]);

    public void SetRotateX(string v)
    {
        float.TryParse(v, out selectedActor.actor.Rotate[0]);
        selectedActor.actor.Rotate[0] *= Mathf.Deg2Rad;
    }

    public void SetRotateY(string v)
    {
        float.TryParse(v, out selectedActor.actor.Rotate[1]);
        selectedActor.actor.Rotate[1] *= Mathf.Deg2Rad;
    }

    public void SetRotateZ(string v)
    {
        float.TryParse(v, out selectedActor.actor.Rotate[2]);
        selectedActor.actor.Rotate[2] *= Mathf.Deg2Rad;
    }
}
