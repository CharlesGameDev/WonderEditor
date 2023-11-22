using TMPro;
using UnityEngine;

public class ErrorPopup : MonoBehaviour
{
    [SerializeField] TMP_Text errorContent;
    [SerializeField] AudioClip showClip;

    static ErrorPopup _instance;
    public static ErrorPopup Instance
    {
        get
        {
            if (_instance == null) _instance = Instantiate(Resources.Load<ErrorPopup>("Prefabs/Error Popup"), GameObject.Find("Canvas").transform);
            return _instance;
        }
    }

    void _Show(string errorName, string content)
    {
        SceneScript.Instance.PlaySound(showClip);
        errorContent.text = $"{errorName}:\n{content}";
        gameObject.SetActive(true);
    }

    public static void Show(string errorName, string content)
    {
        Instance._Show(errorName, content);
    }
}
