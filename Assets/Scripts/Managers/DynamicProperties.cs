using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DynamicProperties : MonoBehaviour
{
    public static DynamicProperties Instance { get; private set; }

    [SerializeField] GameObject propertyPrefab;
    [SerializeField] Transform properties;
    Dictionary<string, object> values;
    Actor actor;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

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
            CreateProp(item.Key, item.Value.ToString());
        }
    }

    void CreateProp(string key, string value)
    {
        GameObject go = Instantiate(propertyPrefab, properties);
        TMP_InputField[] fields = go.GetComponentsInChildren<TMP_InputField>();
        
        TMP_InputField nameField = fields[0];
        nameField.text = key;
        nameField.name = key;

        nameField.onValueChanged.AddListener(value =>
        {
            SetPropertyName(nameField.name, value);
            nameField.name = value;
        });

        TMP_InputField field = fields[1];
        field.text = value.ToString();

        field.onValueChanged.AddListener(value =>
        {
            SetProperty(key, value);
        });
    }

    public void SetPropertyName(string key, string value)
    {
        values.ChangeKey(key, value);
    }

    public void SetProperty(string key, string value)
    {
        values[key] = value;
    }
}
