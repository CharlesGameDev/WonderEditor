using System;
using TMPro;
using UnityEngine;

public class PopupField : MonoBehaviour
{
    public static PopupField Instance { get; private set; }

    [SerializeField] TMP_Text fieldNameText;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text descriptionNameText;
    Action<string> callback;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(string fieldName, string description, Action<string> callback)
    {
        fieldNameText.text = fieldName;
        descriptionNameText.text = description;
        this.callback = callback;
        gameObject.SetActive(true);
    }

    public void Submit()
    {
        callback.Invoke(inputField.text);
        gameObject.SetActive(false);
    }
}
