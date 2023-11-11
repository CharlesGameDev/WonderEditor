using TMPro;
using UnityEngine;

public class Inspector : MonoBehaviour
{
    public static Inspector Instance { get; private set; }

    public ActorView selectedActor;

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
        Instance = this;
        nameInput.onValueChanged.AddListener(SetName);
        gyamlInput.onValueChanged.AddListener(SetGyaml);
        hashInput.onValueChanged.AddListener(SetHash);
        areaHashInput.onValueChanged.AddListener(SetAreaHash);
        translateXInput.onValueChanged.AddListener(SetTranslateX);
        translateYInput.onValueChanged.AddListener(SetTranslateY);
        translateZInput.onValueChanged.AddListener(SetTranslateZ);
        scaleXInput.onValueChanged.AddListener(SetScaleX);
        scaleYInput.onValueChanged.AddListener(SetScaleY);
        scaleZInput.onValueChanged.AddListener(SetScaleZ);
        rotateXInput.onValueChanged.AddListener(SetRotateX);
        rotateYInput.onValueChanged.AddListener(SetRotateY);
        rotateZInput.onValueChanged.AddListener(SetRotateZ);
        Hide();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        selectedActor = null;
    }

    public void ShowActor(ActorView actor)
    {
        selectedActor = actor;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (selectedActor != null)
        {
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
        }
    }

    public void EditDynamicValues()
    {
        DynamicProperties.Instance.Show(selectedActor.actor);
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
