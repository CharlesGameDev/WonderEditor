using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicProperties : MonoBehaviour
{
    private static DynamicProperties _instance;
    public static DynamicProperties Instance
    {
        get
        {
            if (_instance == null) return _instance = FindAnyObjectByType<DynamicProperties>(FindObjectsInactive.Include);
            return _instance;
        }
    }

    [SerializeField] DynamicPropertyField propertyPrefab;
    [SerializeField] Transform properties;
    Dictionary<string, object> values;
    Actor actor;

    public void ApplyChanges()
    {
        actor.Dynamic = new Dictionary<string, object>(values);
    }

    public void AddNewProperty()
    {
        values.Add("NewProperty", "Value");
        CreateProp("NewProperty", "Value");
    }

    public void Show(Actor actor)
    {
        this.actor = actor;

        gameObject.SetActive(true);

        for (int i = 0; i < properties.childCount; i++)
            Destroy(properties.GetChild(i).gameObject);

        if (actor.Dynamic == null) return;

        values = new Dictionary<string, object>(actor.Dynamic);

        foreach (var item in actor.Dynamic)
        {
            CreateProp(item.Key, item.Value);
        }
    }

    void CreateProp(string key, object value)
    {
        DynamicPropertyField pfield = Instantiate(propertyPrefab, properties);
        
        TMP_InputField nameField = pfield.field1;
        nameField.text = key;
        nameField.name = key;

        nameField.onValueChanged.AddListener(value =>
        {
            SetPropertyName(nameField.name, value);
            nameField.name = value;
        });

        if (bool.TryParse(value.ToString(), out bool v))
        {
            Toggle toggle = pfield.field2Toggle;
            toggle.isOn = v;

            toggle.onValueChanged.AddListener(value =>
            {
                SetProperty(key, value);
            });

            toggle.gameObject.SetActive(true);
        }
        else
        {
            TMP_InputField field = pfield.field2Text;
            field.text = value.ToString();

            field.onValueChanged.AddListener(value =>
            {
                SetProperty(key, value);
            });

            field.gameObject.SetActive(true);
        }
    }

    public void SetPropertyName(string key, string value)
    {
        values.ChangeKey(key, value);
    }

    public void SetProperty(string key, object value)
    {
        values[key] = value;
    }
}
