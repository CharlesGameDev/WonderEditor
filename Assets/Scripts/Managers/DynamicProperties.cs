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
    [SerializeField] string wikiUrlFormat;
    Dictionary<string, object> values;
    Dictionary<string, DynamicPropertyField> propObjects;
    ActorView actor;

    public void ApplyChanges()
    {
        actor.actor.Dynamic = new Dictionary<string, object>(values);
        actor.UpdateSpriteContent();
    }

    public void AddNewProperty()
    {
        values ??= new Dictionary<string, object>();

        values.Add("NewProperty", "Value");
        CreateProp("NewProperty", "Value");
    }

    public void Show(ActorView actor)
    {
        this.actor = actor;

        gameObject.SetActive(true);

        for (int i = 0; i < properties.childCount; i++)
            Destroy(properties.GetChild(i).gameObject);
        propObjects = new Dictionary<string, DynamicPropertyField>();

        if (actor.actor.Dynamic == null) return;

        values = new Dictionary<string, object>(actor.actor.Dynamic);

        foreach (var item in actor.actor.Dynamic)
        {
            DynamicPropertyField pfield = CreateProp(item.Key, item.Value);
            propObjects.Add(item.Key, pfield);
        }
    }

    DynamicPropertyField CreateProp(string key, object value)
    {
        DynamicPropertyField pfield = Instantiate(propertyPrefab, properties);
        
        TMP_InputField nameField = pfield.field1;
        nameField.text = key;
        nameField.name = key;
        pfield.onRemove.RemoveAllListeners();
        pfield.onRemove.AddListener(() =>
        {
            RemoveProperty(key);
        });

        nameField.onValueChanged.AddListener(value =>
        {
            SetPropertyName(nameField.name, value);
            nameField.name = value;
        });

        if (value != null && bool.TryParse(value.ToString(), out bool v))
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
            if (value == null) value = "";

            TMP_InputField field = pfield.field2Text;
            field.text = value.ToString();

            field.onValueChanged.AddListener(value =>
            {
                SetProperty(key, value);
            });

            field.gameObject.SetActive(true);
        }

        return pfield;
    }

    public void SetPropertyName(string key, string value)
    {
        values.ChangeKey(key, value);
    }

    public void SetProperty(string key, object value)
    {
        values[key] = value;
    }

    public void RemoveProperty(string key)
    {
        values.Remove(key);
        Destroy(propObjects[key].gameObject);
        Debug.Log(key);
        propObjects.Remove(key);
    }

    public void OpenWiki()
    {
        if (actor == null)
        {
            ErrorPopup.Show("NullReferenceException", "No actor selected.");
            return;
        }
        Application.OpenURL(string.Format(wikiUrlFormat, actor.actor.Gyaml));
    }
}
