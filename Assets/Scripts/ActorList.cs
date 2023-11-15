using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActorList : MonoBehaviour
{
    private static ActorList instance;
    public static ActorList Instance
    {
        get
        {
            if (instance == null) instance = FindFirstObjectByType<ActorList>(FindObjectsInactive.Include);
            return instance;
        }
    }

    [Serializable]
    public class ActorEntry
    {
        public string name;
        public string gyaml;
        public string image2;
        public Sprite assignedImage1;
        public Sprite assignedImage2;
    }

    [HideInInspector]
    public List<ActorEntry> allActors;
    Action<string> callback;
    [SerializeField] TextAsset actorListText;
    [SerializeField] GameObject actorPrefab;
    [SerializeField] Transform actorListParent;
    [SerializeField] TMP_Text actorName;
    [SerializeField] TMP_Text actorQualifiedName;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Image actorImage;
    [SerializeField] Image actorImage2;
    public DynamicPropertyPreset[] propertyPresets;
    ActorEntry selectedActor;
    bool madeActors;
    public Dictionary<ActorEntry, GameObject> actorEntries;

    void Start()
    {
        if (madeActors) return;

        allActors = new List<ActorEntry>();

        Debug.Log("Making actors");
        foreach (string s in actorListText.text.Split("\n"))
        {
            if (string.IsNullOrWhiteSpace(s.Trim())) continue;

            string[] parts = s.Split(",");
            ActorEntry ae = new()
            {
                name = parts[0].Trim(),
                gyaml = parts[1].Trim()
            };

            if (parts.Length > 2)
                ae.image2 = parts[2].Trim();

            allActors.Add(ae);
        }

        actorEntries = new Dictionary<ActorEntry, GameObject>();
        foreach (ActorEntry entry in allActors)
        {
            GameObject go = Instantiate(actorPrefab, actorListParent);

            Transform c0 = go.transform.GetChild(0);
            Transform c1 = go.transform.GetChild(1);
            Transform c2 = go.transform.GetChild(2);
            Transform c3 = go.transform.GetChild(3);

            c0.GetComponent<TMP_Text>().text = entry.name;
            c1.GetComponent<TMP_Text>().text = entry.gyaml;

            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                Select(entry);
            });

            foreach (var pair in ActorManager.Instance.Sprites)
                if (entry.gyaml == pair.Key)
                {
                    entry.assignedImage1 = pair.Value;
                    c2.GetComponent<Image>().sprite = entry.assignedImage1;
                    break;
                }

            Image image2 = c3.GetComponent<Image>();
            if (!string.IsNullOrEmpty(entry.image2))
            {
                entry.assignedImage2 = ActorManager.Instance.Sprites[entry.image2];
                image2.sprite = entry.assignedImage2;
                image2.enabled = true;
            } else
                image2.enabled = false;

            actorEntries.Add(entry, go);
        }

        madeActors = true;
    }

    public static string GetActorName(string gyaml)
    {
        if (Instance.allActors == null || Instance.allActors.Count == 0)
            Instance.Start();

        foreach (ActorEntry ae in Instance.allActors)
        {
            if (gyaml == ae.gyaml)
                return ae.name;
        }

        return "unknown actor";
    }

    public void Select(ActorEntry actor)
    {
        selectedActor = actor;

        actorName.text = actor.name;
        actorQualifiedName.text = actor.gyaml;
        actorImage.sprite = actor.assignedImage1;

        if (actor.assignedImage2 != null)
        {
            actorImage2.sprite = actor.assignedImage2;
            actorImage2.enabled = true;
        } else
            actorImage2.enabled = false;
    }

    public void Show(Action<string> callback, string buttonText)
    {
        this.buttonText.text = buttonText;
        this.callback = callback;
        gameObject.SetActive(true);
    }
    
    public void AddActor()
    {
        if (selectedActor != null && callback != null)
        {
            callback.Invoke(selectedActor.gyaml);
            gameObject.SetActive(false);
        }
    }

    public void Search(string search)
    {
        if (string.IsNullOrEmpty(search.Trim()))
        {
            foreach (var pair in actorEntries)
                pair.Value.SetActive(true);

            return;
        }

        search = search.ToLower();
        foreach (var pair in actorEntries)
        {
            if (pair.Key.name.ToLower().Contains(search) || pair.Key.gyaml.ToLower().Contains(search))
                pair.Value.SetActive(true);
            else
                pair.Value.SetActive(false);
        }
    }
}
