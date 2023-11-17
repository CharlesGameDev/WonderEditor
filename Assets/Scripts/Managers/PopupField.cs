using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class PopupField : MonoBehaviour
{
    private static PopupField instance;
    public static PopupField Instance
    {
        get
        {
            if (instance == null) instance = FindFirstObjectByType<PopupField>(FindObjectsInactive.Include);
            return instance;
        }
    }

    [SerializeField] TMP_InputField field1;
    [SerializeField] TMP_InputField field2;
    [SerializeField] TMP_Dropdown typeDropdown;
    [SerializeField] TMP_Text field1Text;
    [SerializeField] TMP_Text field2Text;
    [SerializeField] TMP_Text descriptionNameText;
    Action<string[], string> callbackWithType;
    Action<string[]> callback;
    string typeSelected;

    public void Show(string fieldName1, string fieldName2, string description, Action<string[]> callback)
    {
        field1Text.text = fieldName1;
        field1.text = "";
        field1.gameObject.SetActive(true);
        typeDropdown.gameObject.SetActive(false);
        if (string.IsNullOrEmpty(fieldName2))
            field2.gameObject.SetActive(false);
        else
        {
            field2Text.text = fieldName2;
            field2.text = "";
            field2.gameObject.SetActive(true);
        }
        descriptionNameText.text = description;
        this.callback = callback;
        this.callbackWithType = null;
        gameObject.SetActive(true);
    }

    public void ShowWithType(string fieldName1, string fieldName2, string[] type, string description, Action<string[], string> callback)
    {
        field1Text.text = fieldName1;
        field1.gameObject.SetActive(true);
        field1.text = "";
        field2Text.text = fieldName2;
        field2.gameObject.SetActive(true);
        field2.text = "";
        typeDropdown.ClearOptions();
        typeDropdown.AddOptions(type.ToList());
        typeDropdown.gameObject.SetActive(true);
        descriptionNameText.text = description;
        this.callbackWithType = callback;
        this.callback = null;
        SetTypeSelected(0);
        gameObject.SetActive(true);
    }

    public void Submit()
    {
        gameObject.SetActive(false);
        callback?.Invoke(new string[] { field1.text, field2.text });
        callbackWithType?.Invoke(new string[] { field1.text, field2.text }, typeSelected);
    }

    public void SetTypeSelected(int type)
    {
        typeSelected = typeDropdown.options[type].text;
    }
}
