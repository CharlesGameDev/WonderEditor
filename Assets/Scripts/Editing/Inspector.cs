using TMPro;
using Unity.VisualScripting;
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

    public void SetName(string v) => selectedActor.actor.Name = v;
    public void SetGyaml(string v) => selectedActor.actor.Gyaml = v;
    public void SetHash(string v) => selectedActor.actor.Hash = ulong.Parse(v);
    public void SetAreaHash(string v) => selectedActor.actor.AreaHash = uint.Parse(v);
}
