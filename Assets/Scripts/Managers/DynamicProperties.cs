using System.Collections.Generic;
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
        Debug.Log(actor.Name);
        Debug.Log(actor.Dynamic["ChildActorSelectName"]);
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
            GameObject go = Instantiate(propertyPrefab, properties);
            go.GetComponentInChildren<TMP_Text>().text = item.Key;
            TMP_InputField field = go.GetComponentInChildren<TMP_InputField>();
            field.text = item.Value.ToString();

            field.onValueChanged.AddListener(value =>
            {
                SetProperty(item.Key, value);
            });
        }
    }

    public void SetProperty(string key, string value)
    {
        Debug.Log($"{key}: {value}");
        values[key] = value;
    }
}
