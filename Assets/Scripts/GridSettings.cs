using TMPro;
using UnityEngine;

public class GridSettings : MonoBehaviour
{
    [SerializeField] GridSpawner spawner;
    [SerializeField] TMP_InputField mainInput;
    [SerializeField] TMP_InputField secondaryInput;
    [SerializeField] TMP_InputField backgroundInput;

    private void Start()
    {
        mainInput.text = PlayerPrefs.GetString("GridMain", "#FFFFFF");
        secondaryInput.text = PlayerPrefs.GetString("GridSecondary", "#FFFFFF");
        backgroundInput.text = PlayerPrefs.GetString("GridBackground", "#000000");
    }

    public void SetMainColor(string s)
    {
        if (ColorUtility.TryParseHtmlString(s, out Color color))
        {
            spawner.SetMainColor(color);
            PlayerPrefs.SetString("GridMain", s);
        }
    }

    public void SetSecondaryColor(string s)
    {
        if (ColorUtility.TryParseHtmlString(s, out Color color))
        {
            spawner.SetSecondaryColor(color);
            PlayerPrefs.SetString("GridSecondary", s);
        }
    }

    public void SetBackgroundColor(string s)
    {
        if (ColorUtility.TryParseHtmlString(s, out Color color))
        {
            spawner.SetBackgroundColor(color);
            PlayerPrefs.SetString("GridBackground", s);
        }
    }

    public void ResetValues()
    {
        SetMainColor("#FFFFFF");
        SetSecondaryColor("#FFFFFF");
        SetBackgroundColor("#000000");
        mainInput.text = "#FFFFFF";
        secondaryInput.text = "#FFFFFF";
        backgroundInput.text = "#000000";
    }
}
